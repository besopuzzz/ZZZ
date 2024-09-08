using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Core.Physics;
using ZZZ.Framework.Core.Registrars;

namespace ZZZ.Framework.Physics.Components
{
    /// <summary>
    /// Представляет регистратор, который обрабатывает физические Aether компоненты.
    /// </summary>
    public class PhysicRegistrar : BaseRegistrar<IBody>, IAnyRegistrar<IBody>
    {
        /// <summary>
        /// Получает экземпляр физического мира.
        /// </summary>
        [ContentSerializerIgnore]
        public World World => world;

        private World world;
        private List<IBody> bodies;
        private static World instance;

        /// <summary>
        /// Инициализирует новый экземпляр физического регистратора.
        /// </summary>
        public PhysicRegistrar()
        {
            world = new World(Vector2.Zero);

            instance = world;

            bodies = new List<IBody>();
        }

        /// <summary>
        /// Выпускает луч от точки до точки и возвращает результат.
        /// </summary>
        /// <param name="start">Точка старта луча.</param>
        /// <param name="end">Конечная точка луча.</param>
        /// <returns>Результат выпуска луча.</returns>
        public static RaycastResult Raycast(Vector2 start, Vector2 end)
        {
            return Raycast(start, end, ColliderLayer.All);
        }

        /// <summary>
        /// Выпускает луч от точки до точки и возвращает результат.
        /// </summary>
        /// <param name="start">Точка старта луча.</param>
        /// <param name="end">Конечная точка луча.</param>
        /// <param name="layer">Маска слоя коллайдеров, на которые будут действовать луч.</param>
        /// <returns>Результат выпуска луча.</returns>
        public static RaycastResult Raycast(Vector2 start, Vector2 end, ColliderLayer layer)
        {
            RaycastResult raycastResult = new RaycastResult();
            raycastResult.Start = start;
            raycastResult.End = end;
            raycastResult.Point = end;

            instance?.RayCast((fixture, point, normal, fraction) =>
            {
                var collider = fixture.Tag as Collider;

                if (!layer.HasFlag(collider.Layer))
                    return -1;

                raycastResult.Collider = collider;
                raycastResult.Point = point * IRigidbody.PixelsPerMeter;
                raycastResult.Normal = normal;
                raycastResult.Fraction = fraction * IRigidbody.PixelsPerMeter;

                return fraction;
            }, start / IRigidbody.PixelsPerMeter, end / IRigidbody.PixelsPerMeter);

            return raycastResult;
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var item in bodies)
            {
                item.UpdateBody();
            }

            World.Step(gameTime.ElapsedGameTime);

            foreach (var item in bodies)
            {
                item.UpdateTransformer();
            }


            base.Update(gameTime);
        }


        void IAnyRegistrar<IBody>.Reception(IBody body)
        {
            world.Add(body.Body);
            bodies.Add(body);
        }

        void IAnyRegistrar<IBody>.Departure(IBody body)
        {
            world.Remove(body.Body);
            bodies.Remove(body);    
        }

    }
}
