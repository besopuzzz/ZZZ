using Microsoft.Xna.Framework.Audio;
using ZZZ.Framework.Assets;

namespace ZZZ.Framework.Auding.Assets
{
    public sealed class Sound : Asset
    {
        public static float MasterVolume
        {
            get
            {
                return SoundEffect.MasterVolume;
            }
            set
            {
                SoundEffect.MasterVolume = value;
            }
        }

        internal SoundEffect SoundEffect { get; set; }

        internal Sound()
        {

        }

        internal Sound(SoundEffect soundEffect)
        {
            SoundEffect = soundEffect;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                SoundEffect?.Dispose();

            SoundEffect = null;

            base.Dispose(disposing);
        }
    }
}
