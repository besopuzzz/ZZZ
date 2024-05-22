using Microsoft.Xna.Framework.Media;

namespace ZZZ.Framework.Auding.Assets.Pipeline
{
    public class MusicReader : ContentTypeReader<Music>
    {
        protected override Music Read(ContentReader input, Music existingInstance)
        {
            if (existingInstance == null)
                existingInstance = new Music();

            existingInstance.Name = input.AssetName;
            existingInstance.Song = input.ReadObject<Song>();

            return existingInstance;
        }
    }
}
