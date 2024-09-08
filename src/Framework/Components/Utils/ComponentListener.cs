using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZZ.Framework.Components.Utils
{
    public class ComponentListener<TComponent> : Component
        where TComponent : IComponent
    {
        public Action<TComponent> ComponentAdded;
        public Action<TComponent> ComponentRemoved;

        protected override void Awake()
        {



            Owner.GameObjectAdded += Owner_GameObjectAdded;
            Owner.GameObjectRemoved += Owner_GameObjectRemoved;
        }

        private void Owner_GameObjectAdded(GameObject sender, GameObject e)
        {
            throw new NotImplementedException();
        }

        private void Owner_GameObjectRemoved(GameObject sender, GameObject e)
        {
            throw new NotImplementedException();
        }
    }
}
