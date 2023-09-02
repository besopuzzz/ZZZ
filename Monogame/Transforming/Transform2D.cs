using Microsoft.Xna.Framework;
using ZZZ.Framework.Monogame.Content;

namespace ZZZ.Framework.Monogame.Transforming
{
    public struct Transform2D
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public static Transform2D CreateTranslation(Vector2 position) => new Transform2D(position);
        public static Transform2D CreateTranslation(float x, float y) => new Transform2D(new Vector2(x,y));
        public static Transform2D CreateScale(Vector2 scale) => new Transform2D(Vector2.Zero, scale);
        public static Transform2D CreateScale(float scaleXY) => new Transform2D(Vector2.Zero, new Vector2(scaleXY));
        public static Transform2D CreateScale(float x, float y) => new Transform2D(Vector2.Zero, new Vector2(x, y));
        public static Transform2D CreateRotation(float rotation) => new Transform2D(rotation);
        public Matrix GetMatrix()
        {
            return Matrix.CreateRotationZ(Rotation)
                * Matrix.CreateScale(new Vector3(Scale, 1f))
                * Matrix.CreateTranslation(Position.X, Position.Y, 0);
        }

        public Transform2D() : this(new Vector2(), Vector2.One, 0f)
        {
        }
        public Transform2D(Transform2D copy) : this(copy.Position, copy.Scale, copy.Rotation)
        {

        }
        public Transform2D(Vector2 position) : this(position, Vector2.One, 0f)
        {

        }
        public Transform2D(Vector2 position, Vector2 scale) : this(position, scale, 0f)
        {

        }
        public Transform2D(float rotation) : this(Vector2.Zero, Vector2.One, rotation)
        {

        }
        public Transform2D(float rotation, Vector2 scale) : this(Vector2.Zero, scale, rotation)
        {

        }
        public Transform2D(Vector2 position, float rotation) : this(position, Vector2.One, rotation)
        {

        }

        public Transform2D(float x, float y) : this(new Vector2(x,y))
        {

        }

        public Transform2D(float x, float y, float rotation) : this(new Vector2(x,y), rotation)
        {

        }

        public Transform2D(float x, float y, float rotation, float scaleXY) : this(new Vector2(x,y), new Vector2(scaleXY), rotation)
        {

        }

        public Transform2D(Vector2 position, Vector2 scale, float rotation)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public static Transform2D operator *(Transform2D a, Transform2D b)
        {
            return new Transform2D(Vector2.Transform(a.Position, b.GetMatrix()), a.Scale * b.Scale, a.Rotation + b.Rotation);
        }

        public static Transform2D operator /(Transform2D a, Transform2D b)
        {
            return new Transform2D(Vector2.Transform(a.Position, Matrix.Invert(b.GetMatrix())), a.Scale / b.Scale, a.Rotation - b.Rotation);
        }

        public Vector2 ToWorld(Vector2 position)
        {
            return Vector2.Transform(position, GetMatrix());
        }

        public Vector2 ToLocal(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(GetMatrix()));
        }

        public static bool operator ==(Transform2D a, Transform2D b)
        {
            if (a.Position.X == b.Position.X && a.Position.Y == b.Position.Y
                && a.Scale.X == b.Scale.X && a.Scale.Y == b.Scale.Y)
                return a.Rotation == b.Rotation;
            return false;
        }

        public static bool operator !=(Transform2D a, Transform2D b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Transform2D)
                return this == (Transform2D)obj;
            return false;
        }

        public bool Equals(Transform2D other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return ((((17 * 23 + Position.X.GetHashCode()) * 23 + Position.Y.GetHashCode()) * 23 + Scale.X.GetHashCode()) * 23 + Scale.Y.GetHashCode())
                * 23 + Rotation.GetHashCode();
        }

        public override string ToString()
        {
            return $"Position: {Position}, Scale: {Scale}, Rotation: {Rotation}";
        }
        public bool Intersects(Transform2D value)
        {
            if (value.Position.X < Position.X + Scale.X && Position.X < value.Position.X + value.Scale.X && value.Position.Y < Position.Y + Scale.Y)
            {
                return Position.Y < value.Position.Y + value.Scale.Y;
            }
            return false;
        }
        public static Transform2D IntersectWithoutRotation(Transform2D value1, Transform2D value2)
        {
            Transform2D result = new Transform2D(0f, 0f, 0f, 0f);

            if (value1.Intersects(value2))
            {
                float num = Math.Min(value1.Position.X + value1.Scale.X, value2.Position.X + value2.Scale.X);
                float num2 = Math.Max(value1.Position.X, value2.Position.X);
                float num3 = Math.Max(value1.Position.Y, value2.Position.Y);
                float num4 = Math.Min(value1.Position.Y + value1.Scale.Y, value2.Position.Y + value2.Scale.Y);

                result.Position = new Vector2(num2, num3);
                result.Scale = new Vector2(num - num2, num4 - num3);
            }

            return result;
        }
    }
}
