namespace ZZZ.Framework.Monogame.Extentions
{
    public static class RectangleExtentions
    {
        public static Rectangle Sum(this Rectangle rectangle, Rectangle other)
        {
            int num = rectangle.X + other.X;
            int num2 = rectangle.Y + other.Y;
            return new Rectangle(num, num2, Math.Min(rectangle.Width, other.Width), Math.Min(rectangle.Height, other.Height));
        }
    }
}
