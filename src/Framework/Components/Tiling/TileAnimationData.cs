using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Components.Tiling
{
    public struct TileAnimationData
    {
        public float Duration { get; set; } = 1f;
        public float StartOffset { get; set; } = 0f;
        public Sprite[] Sprites { get; set; } = null;

        public TileAnimationData()
        {

        }
    }
}
