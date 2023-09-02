using Microsoft.Xna.Framework;

namespace ZZZ.Framework.Monogame.Extentions
{
    public static class Vector2Extention
    {
        public static Vector2 ToDirection(this Vector2 vector)
        {
            Vector2 normalize = Vector2.Zero;

            if (vector.X < 0)
                normalize.X = -1;
            else if(vector.X > 1)
                normalize.X = 1;

            if (vector.Y < 0)
                normalize.Y = -1;
            else if(vector.Y > 1)
                normalize.Y = 1;

            return normalize;
        }
    }
}
