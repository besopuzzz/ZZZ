using Microsoft.Xna.Framework;
using ZZZ.Framework.Monogame.Extentions;
using ZZZ.Framework.Monogame.Transforming.Components;

namespace ZZZ.Framework.Monogame.Tiling.Components
{
    [RequireComponent(Type = typeof(Transformer))]
    public sealed class Grid : Component
    {
        public Vector2 CellSize { get; set; } = new Vector2(32);

        public Vector2 GetPositionFromPoint(Point position)
        {
            return (position.ToVector2() * CellSize) - CellSize/ 2;
        }

        public Point GetPointFromPosition(Vector2 position)
        {
            return ((position + CellSize * position.ToDirection() / 2) / CellSize).ToPoint();
        }
    }
}
