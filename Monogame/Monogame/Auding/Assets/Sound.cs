using Microsoft.Xna.Framework.Audio;
using ZZZ.Framework.Monogame.Assets;

namespace ZZZ.Framework.Monogame.Auding.Assets
{
    public sealed class Sound : Asset
    {
        internal SoundEffect SoundEffect { get; }

        internal Sound()
        {

        }
        public Sound(SoundEffect soundEffect)
        {
            this.SoundEffect = soundEffect;
        }

        public static Sound Create(SoundEffect soundEffect, string name)
        {
            if (soundEffect == null)
                throw new ArgumentNullException("sound");

            Sound sound = new Sound(soundEffect);
            sound.Name = name;

            return sound;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SoundEffect.Dispose();
            }


            base.Dispose(disposing);
        }
    }
}
