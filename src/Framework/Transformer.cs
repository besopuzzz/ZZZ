using ZZZ.Framework.Components;

namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет основной компонент для позиционирования игровых <see cref="GameObject"/>.
    /// </summary>
    /// <remarks>Используйте компонент для установки локальной трансформации игрового объекта <see cref="Local"/> и получайте мировую
    /// трансформацию <see cref="World"/>.</remarks>
    public class Transformer : Component
    {
        public Transform2D Local
        {
            get => local;
            set
            {
                if (local == value)
                    return;

                local = value;

                world = local * parent.World;

                OnWorldChanged(this, world);
            }
        }

        [ContentSerializerIgnore]
        public Transform2D World
        {
            get => world;
            set
            {
                if (world == value)
                    return;

                local = value / world * local;

                world = value;

                OnWorldChanged(this, world);
            }
        }

        [ContentSerializerIgnore]
        public Transformer Parent => parent;

        [ContentSerializerIgnore]
        public bool HasChanges
        {
            get => hasChanges;
            internal set
            {
                if (hasChanges == value)
                    return;

                hasChanges = value;
            }
        }

        public event EventHandler<Transformer, Transform2D> WorldChanged;

        private Transformer parent;
        private Transform2D local = new Transform2D();
        private Transform2D world = new Transform2D();
        private bool hasChanges = true;
        private static readonly Transformer empty = new Transformer();

        /// <summary>
        /// Возвращает экземпляр трансформера с значениями по умолчанию.
        /// </summary>
        public Transformer()
        {
            parent = empty;
        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальной трансформацией.
        /// </summary>
        /// <param name="local">Локальные трансформация.</param>
        public Transformer(Transform2D local) : this()
        {
            this.local = local;
        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальными параметрами.
        /// </summary>
        /// <param name="position">Локальная позиция.</param>
        public Transformer(Vector2 position) : this(position, Vector2.One, 0f)
        {

        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальными параметрами.
        /// </summary>
        /// <param name="position">Локальная позиция.</param>
        /// <param name="scale">Локальный скаляр.</param>
        public Transformer(Vector2 position, Vector2 scale) : this(position, scale, 0f)
        {

        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальными параметрами.
        /// </summary>
        /// <param name="rotation">Локальный поворот.</param>
        public Transformer(float rotation) : this(Vector2.Zero, Vector2.One, rotation)
        {

        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальными параметрами.
        /// </summary>
        /// <param name="rotation">Локальный поворот.</param>
        /// <param name="scale">Локальный скаляр.</param>
        public Transformer(float rotation, Vector2 scale) : this(Vector2.Zero, scale, rotation)
        {

        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальными параметрами.
        /// </summary>
        /// <param name="x">Локальная позиция по x.</param>
        /// <param name="y">Локальная позиция по y.</param>
        public Transformer(float x, float y) : this(new Vector2(x, y))
        {

        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальными параметрами.
        /// </summary>
        /// <param name="x">Локальная позиция по x.</param>
        /// <param name="y">Локальная позиция по y.</param>
        /// <param name="xyScale">Локальный скаляр по x и y.</param>
        public Transformer(float x, float y, float xyScale) : this(new Vector2(x, y), new Vector2(xyScale), 0f)
        {

        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальными параметрами.
        /// </summary>
        /// <param name="position">Локальная позиция.</param>
        /// <param name="rotation">Локальный поворот.</param>
        public Transformer(Vector2 position, float rotation) : this(position, Vector2.One, rotation)
        {

        }

        /// <summary>
        /// Возвращает экземпляр трансформера с указанным локальными параметрами.
        /// </summary>
        /// <param name="position">Локальная позиция.</param>
        /// <param name="scale">Локальный скаляр.</param>
        /// <param name="rotation">Локальный поворот.</param>
        public Transformer(Vector2 position, Vector2 scale, float rotation) : this(new Transform2D(position, scale, rotation))
        {

        }

        /// <inheritdoc/>
        protected override void Awake()
        {
            parent = Owner?.Owner?.GetComponent<Transformer>();

            if (parent == null)
                parent = empty;

            world = local * parent.World;

            parent.WorldChanged += Parent_WorldChanged;

            base.Awake();
        }

        /// <inheritdoc/>
        protected override void Shutdown()
        {
            parent.WorldChanged -= Parent_WorldChanged;

            world = local / parent.World;

            base.Shutdown();
        }

        private void Parent_WorldChanged(Transformer sender, Transform2D args)
        {
            world = local * parent.World;

            OnWorldChanged(this, world);
        }

        protected virtual void OnWorldChanged(Transformer sender, Transform2D args)
        {
            HasChanges = true;

            WorldChanged?.Invoke(this, world);
        }
    }
}
