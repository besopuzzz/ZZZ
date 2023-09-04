using Microsoft.Xna.Framework.Media;
using ZZZ.Framework.Monogame.Assets;

namespace ZZZ.Framework.Monogame.Auding.Assets
{
    public class Music : Asset
    {
        internal Song Song { get; private set; }

        internal Music()
        {

        }

        public static Music Create(Song song, string name)
        {
            Music music = new Music();
            music.Song = song;
            music.Name = name;

            return music;
        }
    }
}
