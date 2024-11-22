using ZZZ.Framework.Components;

namespace ZZZ.Framework.Designing.UnityStyle.Systems
{
    internal sealed class AwakeShutdownSystem : System
    {
        protected override void Input(IEnumerable<Component> components)
        {
            foreach (var component in components)
                component.InternalAwake();

            base.Input(components);
        }

        protected override void Output(Component component)
        {
            base.Output(component);

            component.InternalShutdown();
        }
    }
}
