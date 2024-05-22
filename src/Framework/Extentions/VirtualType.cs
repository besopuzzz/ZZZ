using System.Reflection.Emit;

namespace ZZZ.Framework.Extentions
{
    public sealed class VirtualType
    {
        private readonly TypeBuilder typeBuilder;

        internal VirtualType(TypeBuilder typeBuilder)
        {
            this.typeBuilder = typeBuilder;
        }

        public VirtualType AddInterface(Type interfaceType)
        {
            typeBuilder.AddInterfaceImplementation(interfaceType);

            return this;
        }

        public VirtualType AddInterfaces(params Type[] interfaces)
        {
            foreach (var type in interfaces)
            {
                AddInterface(type);
            }

            return this;
        }

        public VirtualType AddCustomAttribute( Type attribute, Type[] constructorTypes = null, object[] constructorParametres = null)
        {
            typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                attribute.GetConstructor(constructorTypes ?? new Type[0]), constructorParametres ?? new object[0]));

            return this;
        }

        public Type Create()
        {
            return typeBuilder.CreateType();
        }
    }
}
