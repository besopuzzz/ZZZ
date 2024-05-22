using ZZZ.Framework.Animations.Assets;
using ZZZ.Framework.Assets;

namespace ZZZ.Framework.Auding.Assets
{
    public sealed class Musicplaylist : Asset
    {
        [ContentSerializerIgnore]
        public List<Music> Musics => musics;

        private List<Music>  musics;

        public Musicplaylist()
        {
        }

        public Musicplaylist(params Music[] musics)
        {
            this.musics = musics.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach(Music s in musics)
                ((IDisposable)s)?.Dispose();
            }

            musics = null;

            base.Dispose(disposing);
        }
    }
}
