using ZZZ.Framework.Components;
using ZZZ.Framework.Designing.UnityStyle.Components;

namespace ZZZ.Framework.Designing.UnityStyle.Systems
{
    public class EnabledDisabledSystem : System
    {
        protected override void Input(IEnumerable<Component> components)
        {
            foreach (var component in components)
                if (component is IEnabledComponent enabled)
                    enabled.OnEnabled();

            base.Input(components);
        }

        protected override void Output(Component component)
        {
            base.Output(component);

            if (component is IDisabledComponent disabled)
                disabled.OnDisabled();
        }
    }
}
