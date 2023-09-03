using ZZZ.Framework.Monogame.Rendering.Content;

namespace ZZZ.Framework.Monogame.Tiling
{
    public struct TileAnimationData
    {
        public float Duration { get; set; }
        public float StartOffset { get; set; }
        public List<Sprite> Sprites { get; set; }

        public static bool operator ==(TileAnimationData a, TileAnimationData b)
        {
            if (a.Duration == b.Duration && a.StartOffset == b.StartOffset)
                return a.Sprites == b.Sprites;
            return false;
        }

        public static bool operator !=(TileAnimationData a, TileAnimationData b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is TileAnimationData)
                return this == (TileAnimationData)obj;
            return false;
        }

        public bool Equals(TileAnimationData other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return ((((17 * 23 + Duration.GetHashCode()) * 23) * 23 + StartOffset.GetHashCode()) * 23)
                * 23 + Sprites.GetHashCode();
        }

    }
}
