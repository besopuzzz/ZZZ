using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering
{
    /// <summary>
    /// Представляет компонент для проекции изображения на экран.
    /// </summary>
    /// <remarks>Для отображение хоть какого-то объекта на экране нужен как минимум один экземпляр компонент камеры. 
    /// Существование нескольких камер <see cref="ICamera"/> не будет вызывать конфликт, но рисование будет выполнено повторно
    /// для каждой активной камеры. Используйте <see cref="RenderObject.Layer"/> для указания слоев, 
    /// которые камера будет проецировать на экран.</remarks>
    [RequireComponent(typeof(Transformer))]
    public class Camera : Component, ICamera
    {
        public SortLayer Layer
        {
            get => layer;
            set
            {
                if (layer == value) return;

                layer = value;
            }
        }

        private SortLayer layer = SortLayer.All;

        /// <summary>
        /// Получает значение, зафиксировано ли вращение камеры. <see href="true"/> - вращение зафиксировано, иначе <see href="false"/>.
        /// </summary>
        /// <remarks>По умолчанию значение <see href="true"/>. Фиксируется вращение относительно родительского контейнера, а не локального.</remarks>
        public bool FixedRotation
        {
            get => ignoreZ;
            set
            {
                if (value == ignoreZ)
                    return;

                ignoreZ = value;
            }
        }

        /// <inheritdoc cref="ICamera.Projection"/>
        public Matrix Projection => matrix;

        /// <summary>
        /// Точка вращения и скалирования камеры. 
        /// </summary>
        public Vector2 Anchor { get; set; }

        public int Mask
        {
            get
            {
                return  (1 << 2) | (1 << 4);
            }
        }

        Matrix ICamera.Projection => matrix;

        private Matrix matrix = Matrix.Identity;
        private bool ignoreZ = true;
        private Transformer transformer;

        /// <summary>
        /// Представляет экземпляр компонента камеры.
        /// </summary>
        public Camera()
        {
            Anchor = new Vector2(400, 240);
        }

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();


            base.Awake();
        }

        void ICamera.Render(RenderManager renderManager, IReadOnlyList<IRender> components)
        {
            renderManager.Begin(transformMatrix: matrix, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.Immediate);

            foreach (var item in components)
            {
                item.Render(renderManager);
            }

            renderManager.End();
        }

        void ICamera.UpdateMatrix()
        {
            if (!transformer.HasChanges)
                return;

            Transform2D local = transformer.Local;
            Transform2D world = transformer.Parent.World;

            float rotation = FixedRotation ? local.Rotation : local.Rotation - world.Rotation;

            matrix = Matrix.CreateTranslation(new Vector3(-Vector2.Round(world.Position), 0f))
                * Matrix.CreateRotationZ(rotation)
                * Matrix.CreateScale(new Vector3(local.Scale, 1f))
                * Matrix.CreateTranslation(new Vector3(-Vector2.Round(local.Position), 0f))
                * Matrix.CreateTranslation(new Vector3(Anchor, 0f));
        }
    }
}
