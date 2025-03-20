using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ZZZ.Framework.Physics.Aether.Components
{
    [RequiredComponent<BodyComponent>]
    public class GroupCollider : Component
    {
        public IEnumerable<Collider> Colliders => bodyComponent.Colliders;

        public event ColliderEvent ColliderEnter;
        public event ColliderEvent ColliderExit;

        private BodyComponent bodyComponent;

        protected override void OnCreated()
        {
            bodyComponent = GetComponent<BodyComponent>();

            base.OnCreated();
        }

        public T Add<T>(T collider)
            where T : Collider
        {
            bodyComponent.Attach(collider);

            return collider;
        }

        public void Add(params Collider[] colliders)
        {
            foreach (var collider in colliders)
                Add<Collider>(collider);
        }

        public void Remove(Collider collider)
        {
            bodyComponent.Detach(collider);
        }

        public void Clear()
        {
            foreach (var collider in Colliders)
                Remove(collider);
        }
    }
}
