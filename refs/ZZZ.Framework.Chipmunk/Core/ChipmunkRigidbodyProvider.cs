using ChipmunkSharp;
using Microsoft.Xna.Framework;
using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Chipmunk.Core
{
    internal class ChipmunkRigidbodyProvider : IRigidbodyProvider
    {
        public Vector2 Velocity { get { var vcVect = body.GetVelocity(); return new Vector2(vcVect.x, vcVect.y); } set => body.SetVelocity(new cpVect(value.X, value.Y)); }
        public bool IsKinematic { get => body.bodyType == cpBodyType.KINEMATIC; set { if (value) body.bodyType = cpBodyType.KINEMATIC; } }
        public float AngularVelocity { get => body.GetAngularVelocity(); set => body.SetAngularVelocity(value); }
        public float Mass { get => body.GetMass(); set => body.SetMass(value); }
        public float LinearDamping { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float AngularDamping { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Inertia { get => body.GetMoment(); set => body.SetMoment(value); }
        public bool IsBullet { get => false; set { } }
        public bool FixedRotation { get => fixedRotation; set => fixedRotation = value; }
        public Vector2 Gravity { get => gravity; set => gravity = value; }
        public bool Enabled 
        {
            get => enabled;
            set
            {
                if (value == enabled)
                    return;

                enabled = value;

                if (enabled)
                    body.Activate();
                else body.Sleep();
            }
        }

        private bool enabled = true;
        private bool fixedRotation = false;

        private Vector2 gravity;
        private cpBody body;

        public ChipmunkRigidbodyProvider()
        {
            body = new cpBody(1f, cp.PHYSICS_INFINITY);
        }

        public void Attach(cpSpace cpSpace)
        {
            cpSpace.AddBody(body);
        }

        public void ApplyAngularImpulse(float impulse)
        {
            body.ApplyImpulse(new cpVect(impulse, impulse), new cpVect(0f, 0f));
        }

        public void ApplyForce(Vector2 vector)
        {
            body.ApplyForce(new cpVect(vector.X, vector.Y), new cpVect(0f, 0f));
        }

        public void ApplyLinearImpulse(Vector2 vector)
        {

        }

        public void ApplyTorque(float torque)
        {
            body.SetTorque(torque);
        }
    }
}
