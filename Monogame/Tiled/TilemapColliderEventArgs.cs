using ZZZ.Framework.Monogame.FarseerPhysics;
using ZZZ.Framework.Monogame.FarseerPhysics.Components;
using ZZZ.Framework.Monogame.Tiled.Content;

namespace ZZZ.Framework.Monogame.Tiled
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
