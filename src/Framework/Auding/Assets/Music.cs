using Microsoft.Xna.Framework.Media;
using ZZZ.Framework.Assets;

namespace ZZZ.Framework.Auding.Assets
{
    public sealed class Music : Asset
    {
        public TimeSpan Duration => Song.Duration;
        public bool IsProtected => Song.IsProtected;
        public bool IsRated => Song.IsRated;
        public int PlayCount => Song.PlayCount;
        public int Rating => Song.Rating;
        public int TrackNumber => Song.TrackNumber;

        internal Song Song { get; set; }

        internal Music()
        {
            
        }

        internal Music(Song song)
        {
            Song = song;
        }

        public override int GetHashCode()
        {
            return Song.GetHashCode();
        }

        public bool Equals(Music other)
        {
            if (other != null)
            {
                return Song == other.Song;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as Music);
        }

        public static bool operator ==(Music left, Music right)
        {
            return left?.Equals(right) ?? ((object)right == null);
        }

        public static bool operator !=(Music left, Music right)
        {
            return !(left == right);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Song.Dispose();

            base.Dispose(disposing);
        }
    }
}
