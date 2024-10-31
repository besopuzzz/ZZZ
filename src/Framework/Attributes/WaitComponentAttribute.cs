using System.Reflection;
using ZZZ.Framework.Components;

namespace ZZZ.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class WaitComponentAttribute : Attribute, IComponentAttribute<WaitComponentAttribute>
    {
        public Type ComponentType { get; }
        public bool WaitDescendants { get; }

        public enum WaitComponentOperation
        {
            Add,
            Remove
        }

        private Func<object, object[], object> invoke;
        private IComponent target;

        public WaitComponentAttribute(Type componentType, bool waitDescendants = false) 
        {
            ArgumentNullException.ThrowIfNull(componentType);

            ComponentType = componentType;

            WaitDescendants = waitDescendants;
        }

        public object Invoke(IComponent parameter, WaitComponentOperation waitComponentOperation)
        {
            return invoke?.Invoke(target, [parameter, waitComponentOperation]);
        }

        public void Initialize(AttributeInfo<WaitComponentAttribute> attributeInfo)
        {
            if (attributeInfo.MemberInfo.MemberType != MemberTypes.Method)
                throw new Exception($"That attribute support methods only!");

            var method = attributeInfo.MemberInfo as MethodInfo;

            var parameters = method.GetParameters();

            if (parameters.Length != 2)
                throw new ArgumentException($"The method {method.Name} of class {method.DeclaringType} must contain two parameter IComponent!");
           
            var parameter = parameters[0];

            if(parameter.ParameterType != ComponentType & !ComponentType.IsAssignableTo(parameter.ParameterType))
                    throw new ArgumentException($"Type {ComponentType} is not inherited from {parameter.ParameterType}" +
                        $" of method {method.Name} on class {method.DeclaringType}!");

            invoke = method.Invoke;
        }

        public void Bind(IComponent target, GameObject gameObject)
        {
            this.target = target;

            Subscript(gameObject);
        }

        private void Subscript(GameObject gameObject)
        {
            var components = gameObject.GetComponents(x => x.GetType().IsAssignableFrom(ComponentType));

            foreach (var component in components)
            {
                Invoke(component, WaitComponentOperation.Add);
            }

            gameObject.ComponentAdded += GameObject_ComponentAdded;
            gameObject.ComponentRemoved += GameObject_ComponentRemoved;

            if (!WaitDescendants)
                return;

            foreach (var child in gameObject.GetGameObjects())
            {
                Subscript(child);

                child.GameObjectAdded += Child_GameObjectAdded;
                child.GameObjectRemoved += Child_GameObjectRemoved;
            }
        }

        private void UnSubscript(GameObject gameObject)
        {
            gameObject.ComponentAdded -= GameObject_ComponentAdded;
            gameObject.ComponentRemoved -= GameObject_ComponentRemoved;

            if (!WaitDescendants)
                return;

            foreach (var child in gameObject.GetGameObjects())
            {
                UnSubscript(child);

                child.GameObjectAdded -= Child_GameObjectAdded;
                child.GameObjectRemoved -= Child_GameObjectRemoved;
            }
        }

        private void Child_GameObjectRemoved(GameObject sender, GameObject e)
        {
            UnSubscript(e);
        }

        private void Child_GameObjectAdded(GameObject sender, GameObject e)
        {
            Subscript(e);
        }

        private void GameObject_ComponentRemoved(GameObject sender, IComponent e)
        {
            if (e.GetType().IsAssignableFrom(ComponentType))
                Invoke(e, WaitComponentOperation.Remove);
        }

        private void GameObject_ComponentAdded(GameObject sender, IComponent e)
        {
            if (e.GetType().IsAssignableFrom(ComponentType))
                Invoke(e, WaitComponentOperation.Add);
        }

        public void Unbind()
        {
            UnSubscript(target.Owner);
        }
    }    
}
