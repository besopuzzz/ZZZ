using ZZZ.Framework.Serialization;

namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет структуру для трансформации 2D вектора.
    /// </summary>
    /// <remarks>Используйте любой доступной способ для инициализации трансформации
    /// и переводите в локальные или мировые координаты. Порядок перевода необходимо соблюдать. Например, 
    /// для перевода из локальных координат в мировые, необходимо выполнить операцию inWorld = Local * World.</remarks>
    [Serialize]
    public struct Transform2D : IEquatable<Transform2D>
    {
        /// <summary>
        /// Получает значение положения по координате X.
        /// </summary>
        [Serialize]
        public Vector2 Position;

        /// <summary>
        /// Получает скаляр по координате X.
        /// </summary>
        [Serialize]
        public Vector2 Scale;

        /// <summary>
        /// Получает значение вращения.
        /// </summary>
        [Serialize]
        public float Rotation;

        /// <summary>
        /// Переводит точку в указанное положение и получает трансформацию.
        /// </summary>
        /// <param name="x">Точка по x.</param>
        /// <param name="y">Точка по y.</param>
        /// <returns>Новый экземпляр трансформации с единичным скаляром и нулевым поворотом.</returns>
        public static Transform2D CreateTranslation(float x, float y) => new Transform2D(x, y, 1f, 1f, 0f);
        public static Transform2D CreateTranslation(Vector2 position) => new Transform2D(position.X, position.Y, 1f, 1f, 0f);

        /// <summary>
        /// Устанавливает скаляр и получает трансформацию.
        /// </summary>
        /// <param name="scaleXY">Скаляр, который необходимо установить. Если 0, то будет установлено 1.</param>
        /// <returns>Новый экземпляр трансформации с нулевым положением и нулевым поворотом.</returns>
        public static Transform2D CreateScale(float scaleXY) => CreateScale(scaleXY,scaleXY);

        /// <summary>
        /// Устанавливает скаляр и получает трансформацию.
        /// </summary>
        /// <param name="x">Скаляр по x, который необходимо установить. Если 0, то будет установлено 1.</param>
        /// <param name="y">Скаляр по y, который необходимо установить. Если 0, то будет установлено 1.</param>
        /// <returns>Новый экземпляр трансформации с нулевым положением и нулевым поворотом.</returns>
        public static Transform2D CreateScale(float x, float y) => new Transform2D(0f, 0f, x, y, 0f);
        public static Transform2D CreateScale(Vector2 scale) => CreateScale(scale.X, scale.Y);

        /// <summary>
        /// Устанавливает вращение и получает трансформацию.
        /// </summary>
        /// <param name="rotation">Вращение, которое необходимо установить.</param>
        /// <returns>Новый экземпляр трансформации с нулевым положением и единичным скаляром.</returns>
        public static Transform2D CreateRotation(float rotation) => new Transform2D(0f,0f,1f,1f, rotation);

        /// <summary>
        /// Инициалирует пустую трансформацию и возвращает ее.
        /// </summary>
        public Transform2D() : this(0f, 0f, 1f, 1f, 0f)
        {
        }

        public Transform2D(float x, float y) : this(x, y, 1f, 1f, 0f)
        {
        }

        public Transform2D(Vector2 position) : this(position, Vector2.One, 0f)
        {

        }

        public Transform2D(Transform2D copy) : this(copy.Position, copy.Scale, copy.Rotation)
        {

        }
        public Transform2D(Vector2 position, Vector2 scale) : this(position, scale, 0f)
        {

        }

        /// <inheritdoc cref="Transform2D.Transform2D(Vector2)"/>
        public Transform2D(float rotation) : this(Vector2.Zero, Vector2.One, rotation)
        {

        }

        /// <inheritdoc cref="Transform2D.Transform2D(Vector2)"/>
        public Transform2D(float rotation, Vector2 scale) : this(Vector2.Zero, scale, rotation)
        {

        }

        /// <inheritdoc cref="Transform2D.Transform2D(Vector2)"/>
        public Transform2D(Vector2 position, float rotation) : this(position, Vector2.One, rotation)
        {

        }

        /// <inheritdoc cref="Transform2D.Transform2D(Vector2)"/>
        public Transform2D(float x, float y, float rotation) : this(new Vector2(x, y), rotation)
        {

        }

        /// <inheritdoc cref="Transform2D.Transform2D(Vector2)"/>
        public Transform2D(float x, float y, float rotation, float scaleXY) : this(new Vector2(x, y), new Vector2(scaleXY), rotation)
        {

        }

        /// <inheritdoc cref="Transform2D.Transform2D(Vector2)"/>
        public Transform2D(float positionX, float positionY, float scaleX, float scaleY, float rotation)
        {
            Position.X = positionX;
            Position.Y = positionY;
            Scale.X = scaleX;
            Scale.Y = scaleY;
            Rotation = rotation;
        }

        public Transform2D(Vector2 position, Vector2 scale, float rotation)
        {
            Position.X = position.X;
            Position.Y = position.Y;
            Scale.X = scale.X;
            Scale.Y = scale.Y;
            Rotation = rotation;
        }

        /// <summary>
        /// Складывает две трансформации и возвращает ее.
        /// </summary>
        /// <param name="a">Первая трансформация.</param>
        /// <param name="b">Вторая трансформация.</param>
        /// <returns>Новый экземпляр трансформации.</returns>
        public static Transform2D operator *(Transform2D a, Transform2D b)
        {
            Vector2 scaled = a.Position * b.Scale;

            float sin = (float)Math.Sin(b.Rotation);
            float cos = (float)Math.Cos(b.Rotation);

            Vector2 rotated = new Vector2(cos * scaled.X - sin * scaled.Y, sin * scaled.X + cos * scaled.Y);

            return new Transform2D(rotated + b.Position, a.Scale * b.Scale, a.Rotation + b.Rotation);
        }
        public static Vector2 operator *(Vector2 position, Transform2D world)
        {
            return (CreateTranslation(position) * world).Position;
        }

        /// <summary>
        /// Вычитает две трансформации и возвращает ее.
        /// </summary>
        /// <param name="a">Первая трансформация.</param>
        /// <param name="b">Вторая трансформация.</param>
        /// <returns>Новый экземпляр трансформации.</returns>
        public static Transform2D operator /(Transform2D a, Transform2D b)
        {
            Vector2 rotated = a.Position - b.Position;

            float sin = (float)Math.Sin(-b.Rotation);
            float cos = (float)Math.Cos(-b.Rotation);

            Vector2 scaled = new Vector2(cos * rotated.X - sin * rotated.Y, sin * rotated.X + cos * rotated.Y) / b.Scale;

            return new Transform2D(scaled, a.Scale / b.Scale, a.Rotation - b.Rotation);
        }

        public static Vector2 operator /(Vector2 position, Transform2D world)
        {
            return (CreateTranslation(position) / world).Position;
        }

        /// <summary>
        /// Вычитает две трансформации и возвращает ее.
        /// </summary>
        /// <param name="a">Первая трансформация.</param>
        /// <param name="b">Вторая трансформация.</param>
        /// <returns>Новый экземпляр трансформации.</returns>
        
        //public bool Contains(float width, float height, float x, float y)
        //{
        //    //To check the intersection of a point and a rotating rectangle,
        //    //it is enough to rotate the rectangle into the usual non-rotating
        //    //Rectangle, rotate the point there, and then check the intersections
        //    //using the classical algorithm.

        //    Transform2D scaledSize = Transform2D.CreateScale( width, height) * Transform2D.CreateScale(ScaleX, ScaleY); // Scale current size
        //    Vector2 position = (this * Transform2D.CreateRotation(-Rotation)).Position - scaledSize/2; // Rotate to 0f and move center
        //    Vector2 rotatedPoint = (Transform2D.CreateTranslation(point) * Transform2D.CreateRotation(-Rotation)).Position; // Rotate point

        //    if ((float)position.X <= rotatedPoint.X
        //        && rotatedPoint.X < (float)(position.X + scaledSize.X)
        //        && (float)position.Y <= rotatedPoint.Y)
        //    {
        //        return rotatedPoint.Y < (float)(position.Y + scaledSize.Y);
        //    }

        //    return false;
        //}

        /// <summary>
        /// Сравнивает две трансформации на идентичность и возвращает результат.
        /// </summary>
        /// <param name="a">Первая трансформация.</param>
        /// <param name="b">Вторая трансформация.</param>
        /// <returns>Возвращает true, если трансформации равны. Иначе - false.</returns>
        public static bool operator ==(Transform2D a, Transform2D b)
        {
            if (a.Position == b.Position & a.Scale == b.Scale)
                return a.Rotation == b.Rotation;
            return false;
        }

        /// <summary>
        /// Сравнивает две трансформации на различность и возвращает результат.
        /// </summary>
        /// <param name="a">Первая трансформация.</param>
        /// <param name="b">Вторая трансформация.</param>
        /// <returns>Возвращает true, если трансформации не равны. Иначе - false.</returns>
        public static bool operator !=(Transform2D a, Transform2D b)
        {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Transform2D)
                return this == (Transform2D)obj;
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Transform2D other)
        {
            return this == other;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return ((17 * 23 + Position.GetHashCode()) * 23 + Scale.GetHashCode())
                * 23 + Rotation.GetHashCode();
        }

        /// <summary>
        /// Создает и возвращает строку с описанием трансформации.
        /// </summary>
        /// <returns>Строка, которая содержит информации о позиции, скалировании и поворота.</returns>
        public override string ToString()
        {
            return $"X: {Position.X}, Y: {Position.Y}, SX: {Scale.X}, SY: {Scale.Y}, R: {Rotation}";
        }


        /// <summary>
        /// Создает и возвращает матрицу скалирования.
        /// </summary>
        /// <returns>Матрица, полученная из скаляров трансформации.</returns>
        public Matrix GetMatrixScale()
        {
            return Matrix.CreateScale(Scale.X, Scale.Y, 1f);
        }

        /// <summary>
        /// Создает и возвращает матрицу вращения.
        /// </summary>
        /// <returns>Матрица, полученная из вращения трансформации.</returns>
        public Matrix GetMatrixRotation()
        {
            return Matrix.CreateRotationZ(Rotation);
        }

        /// <summary>
        /// Создает и возвращает матрицу точки.
        /// </summary>
        /// <returns>Матрица, полученная из точки трансформации.</returns>
        public Matrix GetMatrixTranslation()
        {
            return Matrix.CreateTranslation(Position.X, Position.Y, 0f);
        }

        public Matrix GetMatrix()
        {
            return GetMatrixRotation()
                * GetMatrixScale()
                * GetMatrixTranslation();
        }

        public Rectangle CreateBounds(Vector2 size)
        {
            return CreateBounds(size, this);
        }

        public static Rectangle CreateBounds(Vector2 size, Transform2D transform)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Location = transform.Position.ToPoint();
            rectangle.Size = (size * transform.Scale).ToPoint();

            return rectangle;
        }
    }
}
