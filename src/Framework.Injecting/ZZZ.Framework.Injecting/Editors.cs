using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZZZ.Framework.Injecting
{
    public sealed class Editors
    {
        public IEnumerable<IInjectingSubstance> Substances { get; } = Enumerable.Empty<IInjectingSubstance>();

        private IEnumerable<IInjectingSubstance> FindSubstances(Assembly assembly)
        {
            List<IInjectingSubstance> substances = new List<IInjectingSubstance>();

            Type iInjectingSubstanceType = typeof(IInjectingSubstance);
            Type injectingSubstanceAttrType = typeof(InjectingSubstanceAttribute);

            // Ищем все классы, которые:
            // 1. Реализуют IInjectingSubstance
            // 2. Имеют атрибут InjectingSubstanceAttribute
            IEnumerable<Type> injectingSubstanceTypes = assembly.GetTypes()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.CustomAttributes.Any(attr => attr.AttributeType == injectingSubstanceAttrType)
                );

            foreach (Type type in injectingSubstanceTypes)
            {
                if (!DoesTypeImplementInterface(type, iInjectingSubstanceType))
                    continue;

                try
                {
                    // Получаем Runtime-тип через отражение (если сборка уже загружена)
                    Type runtimeType = assembly.GetType(type.FullName);
                    if (runtimeType == null)
                    {
                        Console.WriteLine($"Не удалось загрузить тип {type.FullName}");
                        continue;
                    }

                    // Создаём экземпляр
                    IInjectingSubstance instance = Activator.CreateInstance(runtimeType) as IInjectingSubstance;

                    substances.Add(instance);

                    Console.WriteLine($"Создан экземпляр: {instance}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при создании экземпляра {type.FullName}: {ex.Message}");
                }
            }

            return substances;
        }

        private bool DoesTypeImplementInterface(Type type, Type interfaceType)
        {
            // Проверяем, реализует ли текущий тип интерфейс напрямую
            if (type.GetInterfaces().Any(i => i == interfaceType))
                return true;

            // Если есть базовый класс - проверяем его рекурсивно
            if (type.BaseType != null)
            {
                Type baseType = type.BaseType; // Разрешаем ссылку на тип
                if (baseType != null && DoesTypeImplementInterface(baseType, interfaceType))
                    return true;
            }

            return false;
        }

        public Editors(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                Substances = Enumerable.Concat(Substances, FindSubstances(assembly));
            }
        }

    }
}
