using Microsoft.Xna.Framework.Audio;
using ZZZ.Framework.Monogame.Content;

namespace ZZZ.Framework.Monogame.Audio.Content
{
    public sealed class Sound : Asset
    {
        internal SoundEffect SoundEffect { get; }

        internal Sound()
        {

        }
        internal Sound(SoundEffect soundEffect)
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
            SoundEffect.Dispose();

            base.Dispose(disposing);
        }
    }
}
