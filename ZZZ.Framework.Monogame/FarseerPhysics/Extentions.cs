using Microsoft.Xna.Framework;
using FVector2 = tainicom.Aether.Physics2D.Common.Vector2;

namespace ZZZ.Framework.Monogame.FarseerPhysics
{
    public static class Extentions
    {
        public static Vector2 ToXna(this FVector2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
        public static FVector2 ToAether(this Vector2 vector)
        {
            return new FVector2(vector.X, vector.Y);
        }
    }
}
