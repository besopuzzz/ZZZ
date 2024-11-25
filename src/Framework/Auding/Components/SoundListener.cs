using System.Drawing;
using ZZZ.Framework.Components;

namespace ZZZ.Framework.Auding.Components
{
    public class SoundListener : Component, ISoundListener
    {
        public float Volume
        {
            get => volume;
            set
            {
                if (volume == value)
                    return;

                volume = MathHelper.Clamp(value, 0f, 1f);
            }
        }
        public float Pan
        {
            get => pan;
            set
            {
                if (pan == value)
                    return;

                pan = MathHelper.Clamp(value, -1f, 1f);
            }
        }

        internal static ISoundListener Instance { get; private set; }

        private Transformer transformer;
        private float volume = 1f;
        private float pan = 0f;
        private float enabledValue = 1f;

        protected override void Awake()
        {
            if (Instance == null)
                Instance = this;

            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        protected override void Shutdown()
        {
            if (Instance == this)
                Instance = null;

            transformer = null;

            base.Shutdown();
        }

        protected override void Dispose(bool disposing)
        {
            if (Instance == this)
                Instance = null;

            base.Dispose(disposing);
        }

        public virtual void GetVolumeFromPosition(Vector2 position, float minRadius, float maxRadius, out float newVolume)
        {
            if (minRadius < 0 | minRadius < 0)
            {
                newVolume = 0f;
                return;
            }

            float distance = Vector2.Distance(transformer.World.Position, position);

            if (distance <= maxRadius)
                newVolume = distance >= minRadius ? 1f - distance / maxRadius : 1f;
            else newVolume = 0f;

            newVolume = MathHelper.Clamp(newVolume * volume * enabledValue, 0f, 1f);
        }

        public virtual float GetPanFromEmitter(Vector2 position, ISoundEmitter emitter)
        {
            return pan * enabledValue;
        }
    }
}
