using Microsoft.Xna.Framework.Audio;

namespace ZZZ.Framework.Auding.Assets.Pipeline
{
    public class SoundReader : ContentTypeReader<Sound>
    {
        protected override Sound Read(ContentReader input, Sound existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new Sound();

            existingInstance.Name = input.AssetName;
            existingInstance.SoundEffect = input.ReadObject<SoundEffect>();

            return existingInstance;
        }
    }
}
