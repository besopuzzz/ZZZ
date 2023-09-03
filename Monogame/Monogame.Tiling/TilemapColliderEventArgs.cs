using Microsoft.Xna.Framework;
using ZZZ.Framework.Monogame.FarseerPhysics;
using ZZZ.Framework.Monogame.FarseerPhysics.Components;
using ZZZ.Framework.Monogame.Tiling.Assets;

namespace ZZZ.Framework.Monogame.Tiling
{
    public class TilemapColliderEventArgs : ColliderEventArgs
    {
        public Tile Tile { get; }
        public Point Position { get; }

        public TilemapColliderEventArgs(Collider other, Tile tile, Point position) : base(other)
        {
            Tile = tile;
            Position = position;
        }
    }
}
