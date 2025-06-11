using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Linq;

namespace ZZZ.Framework.Injecting.Substances.MethodListener
{
    [InjectingSubstance]
    public class MethodListenerSubstance : IInjectingSubstance
    {
        public void Inject(ModuleDefinition module)
        {

            // Находим все необходимые типы и методы
            var methodListenerAttribute = module.Types.FirstOrDefault(t => t.FullName == "MethodListenerAttribute");
            if (methodListenerAttribute == null)
                throw new InvalidOperationException("MethodListenerAttribute not found in module");

            var methodInfoArgsType = module.Types.FirstOrDefault(t => t.FullName == "MethodInfoArgs");
            if (methodInfoArgsType == null)
                throw new InvalidOperationException("MethodInfoArgs type not found in module");

            var objectType = module.ImportReference(typeof(object));
            var methodInfoType = module.ImportReference(typeof(System.Reflection.MethodInfo));
            var exceptionType = module.ImportReference(typeof(Exception));

            // Проходим по всем типам в модуле
            foreach (var type in module.Types)
            {
                // Пропускаем статические классы (абстрактные и запечатанные)
                if (type.IsAbstract && type.IsSealed)
                    continue;

                // Проходим по всем методам в типе
                foreach (var method in type.Methods)
                {
                    // Пропускаем статические методы
                    if (method.IsStatic)
                        continue;

                    // Пропускаем методы без тела
                    if (!method.HasBody)
                        continue;

                    // Проверяем, есть ли у метода атрибут MethodListenerAttribute
                    var hasAttribute = method.CustomAttributes.Any(a =>
                        a.AttributeType.FullName == "MethodListenerAttribute");

                    if (!hasAttribute)
                        continue;

                    // Начинаем модификацию метода
                    var processor = method.Body.GetILProcessor();
                    method.Body.SimplifyMacros();

                    // Создаем переменные для хранения listener и args
                    var listenerVar = new VariableDefinition(objectType);

                    method.Body.Variables.Add(listenerVar);

                    var argsVar = new VariableDefinition(methodInfoArgsType);
                    method.Body.Variables.Add(argsVar);

                    var exceptionVar = new VariableDefinition(exceptionType);
                    method.Body.Variables.Add(exceptionVar);

                    // Получаем первый инструкцию метода
                    var firstInstruction = method.Body.Instructions.First();

                    // Вставляем в начало метод код для получения listener
                    var getAttributeMethod = new GenericInstanceMethod(
                        module.ImportReference(typeof(System.Reflection.CustomAttributeExtensions))
                        .Resolve()
                        .Methods.First(m => m.Name == "GetCustomAttribute"));

                    getAttributeMethod.GenericArguments.Add(methodListenerAttribute);

                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Ldarg_0)); // this
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Call,
                        module.ImportReference(typeof(object).GetMethod("GetType"))));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Ldtoken, method));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Call,
                        module.ImportReference(typeof(System.Reflection.MethodBase)
                        .GetMethod("GetMethodFromHandle", new[] { typeof(RuntimeMethodHandle) }))));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Castclass, methodInfoType));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Call,
                        module.ImportReference(getAttributeMethod)));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Stloc, listenerVar));

                    // Создаем MethodInfoArgs
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Ldarg_0)); // this
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Ldtoken, method));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Call,
                        module.ImportReference(typeof(System.Reflection.MethodBase)
                        .GetMethod("GetMethodFromHandle", new[] { typeof(RuntimeMethodHandle) }))));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Castclass, methodInfoType));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Newobj,
                        module.ImportReference(methodInfoArgsType.Resolve()
                        .Methods.First(m => m.IsConstructor && m.Parameters.Count == 2))));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Stloc, argsVar));

                    // Вызываем listener.Entry(info)
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Ldloc, listenerVar));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Ldloc, argsVar));
                    processor.InsertBefore(firstInstruction, processor.Create(OpCodes.Callvirt,
                        module.ImportReference(methodListenerAttribute.Resolve()
                        .Methods.First(m => m.Name == "Entry"))));

                    // Создаем блоки try-catch
                    var tryStart = firstInstruction;
                    var tryEnd = method.Body.Instructions[method.Body.Instructions.Count - 1];

                    var handlerStart = Instruction.Create(OpCodes.Stloc, exceptionVar);
                    processor.Append(handlerStart);

                    // Вызываем listener.Exception(info, exception)
                    processor.Append(processor.Create(OpCodes.Ldloc, listenerVar));
                    processor.Append(processor.Create(OpCodes.Ldloc, argsVar));
                    processor.Append(processor.Create(OpCodes.Ldloc, exceptionVar));
                    processor.Append(processor.Create(OpCodes.Callvirt,
                        module.ImportReference(methodListenerAttribute.Resolve()
                        .Methods.First(m => m.Name == "Exception"))));

                    var handlerEnd = Instruction.Create(OpCodes.Nop);
                    processor.Append(handlerEnd);

                    // Вставляем вызов listener.Success(info) перед каждым return
                    var returns = method.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret).ToList();
                    foreach (var ret in returns)
                    {
                        processor.InsertBefore(ret, processor.Create(OpCodes.Ldloc, listenerVar));
                        processor.InsertBefore(ret, processor.Create(OpCodes.Ldloc, argsVar));
                        processor.InsertBefore(ret, processor.Create(OpCodes.Callvirt,
                            module.ImportReference(methodListenerAttribute.Resolve()
                            .Methods.First(m => m.Name == "Success"))));
                    }

                    // В конце метода вызываем listener.Exit(info)
                    var lastInstruction = method.Body.Instructions.Last();
                    if (lastInstruction.OpCode != OpCodes.Ret)
                    {
                        var ret = Instruction.Create(OpCodes.Ret);
                        processor.Append(ret);
                        lastInstruction = ret;
                    }

                    processor.InsertBefore(lastInstruction, processor.Create(OpCodes.Ldloc, listenerVar));
                    processor.InsertBefore(lastInstruction, processor.Create(OpCodes.Ldloc, argsVar));
                    processor.InsertBefore(lastInstruction, processor.Create(OpCodes.Callvirt,
                        module.ImportReference(methodListenerAttribute.Resolve()
                        .Methods.First(m => m.Name == "Exit"))));

                    // Создаем обработчик исключений
                    var handler = new ExceptionHandler(ExceptionHandlerType.Catch)
                    {
                        TryStart = tryStart,
                        TryEnd = handlerStart,
                        HandlerStart = handlerStart,
                        HandlerEnd = handlerEnd,
                        CatchType = exceptionType
                    };

                    method.Body.ExceptionHandlers.Add(handler);
                    method.Body.OptimizeMacros();
                }
            }
        } 
    }
}
