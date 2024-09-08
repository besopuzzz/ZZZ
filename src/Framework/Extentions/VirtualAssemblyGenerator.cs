using System.Reflection;
using System.Reflection.Emit;

namespace ZZZ.Framework.Extentions
{
    public class VirtualAssemblyGenerator
    {
        public string AssemblyName { get; }
        public Assembly Assembly => moduleBuilder.Assembly;

        private ModuleBuilder moduleBuilder;
        private AssemblyBuilder assemblyBuilder;

        public VirtualAssemblyGenerator(string assemblyName) 
        {
            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName));

            AssemblyName = assemblyName;
            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName),
                AssemblyBuilderAccess.Run);
            moduleBuilder = assemblyBuilder.DefineDynamicModule(AssemblyName);
        }

        public VirtualType AddType(string name, TypeAttributes typeAttributes, Type baseType = null)
        {
            var tb = moduleBuilder.DefineType(name, typeAttributes, baseType ?? typeof(object));

            return new VirtualType(tb);
        }

    }
}
