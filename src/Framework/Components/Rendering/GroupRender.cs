using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Components.Rendering
{
    internal class GroupRender : Component, IGroupRender
    {
        public int Order => throw new NotImplementedException();

        public SortLayer Layer => throw new NotImplementedException();

        public event EventHandler<SortLayerArgs> LayerChanged;

        public void Render(RenderManager renderManager)
        {
            throw new NotImplementedException();
        }
    }
}
