using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Components.Tiling;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering
{
    public class RenderEntity : Entity<RenderEntity, RenderEntityComponent, IRender>
    {
        private List<GroupRenderEntityComponent> zGroups = new List<GroupRenderEntityComponent>();

        public override void ForEveryComponent(Action<RenderEntityComponent> action)
        {
            EntityComponents.ForEach(action);

            if (!zGroups.Any())
                Childs.ForEach((e) => e.ForEveryComponent(action));
        }

        protected override void ComponentEntityAdded(RenderEntityComponent entityComponent)
        {
            if (entityComponent is GroupRenderEntityComponent group)
                zGroups.Add(group);
        }

        protected override void ComponentEntityRemoved(RenderEntityComponent entityComponent)
        {
            if (entityComponent is GroupRenderEntityComponent group)
                zGroups.Remove(group);
        }
    }
}
