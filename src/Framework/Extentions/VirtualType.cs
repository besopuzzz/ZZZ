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
                attribute.GetConstructor(constructorTypes ?? Array.Empty<Type>()), constructorParametres ?? Array.Empty<object>()));

            return this;
        }

        public Type Create()
        {
            return typeBuilder.CreateType();
        }
    }
}
