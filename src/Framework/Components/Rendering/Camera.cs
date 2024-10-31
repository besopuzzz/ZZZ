using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Components;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.Framework.Components.Attributes;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.Core.Rendering
{
    /// <summary>
    /// Представляет компонент для проекции изображения на экран.
    /// </summary>
    /// <remarks>Для отображение хоть какого-то объекта на экране нужен как минимум один экземпляр компонент камеры. 
    /// Существование нескольких камер <see cref="ICamera"/> не будет вызывать конфликт, но рисование будет выполнено повторно
    /// для каждой активной камеры. Используйте <see cref="RenderEntity.Layer"/> для указания слоев, 
    /// которые камера будет проецировать на экран.</remarks>

    [RequiredComponent(typeof(Transformer))]
    public class Camera : Component, ICamera
    {
        public bool FocusToCenter
        {
            get => focusToCenter;
            set => focusToCenter = value;
        }

        /// <summary>
        /// Получает значение, зафиксировано ли вращение камеры. <see href="true"/> - вращение зафиксировано, иначе <see href="false"/>.
        /// </summary>
        /// <remarks>По умолчанию значение <see href="true"/>. Фиксируется вращение относительно родительского контейнера, а не локального.</remarks>
        public bool FixedRotation
        {
            get => fixedRotation;
            set
            {
                if (value == fixedRotation)
                    return;

                fixedRotation = value;
            }
        }

        /// <inheritdoc cref="ICamera.Projection"/>
        public Matrix Projection => projection;

        public Matrix View => view;

        public Matrix World => world;



        /// <summary>
        /// Точка фокуса камеры. 
        /// </summary>
        public Vector2 PointOfFocus { get; set; }

        public bool IsMain
        {
            get => isMain;
            set
            {
                if (isMain == value) return;

                ((Camera)mainCamera).IsMain = false;

                isMain = value;

                mainCamera = value ? this : null;
            }
        }

        public static ICamera MainCamera => mainCamera;

        public SortLayer LayerMask
        {
            get => layerMask;
            set
            {
                layerMask = value;
            }
        }

        private bool focusToCenter = true;
        private bool isMain = false;
        private bool fixedRotation = true;
        private Matrix projection = Matrix.Identity;
        private Matrix view = Matrix.Identity;
        private Matrix world = Matrix.Identity;
        private SortLayer layerMask = SortLayer.All;
        private Transformer transformer;
        private BasicEffect basicEffect;
        private GraphicsDevice graphicsDevice;

        private static ICamera mainCamera;

        /// <summary>
        /// Представляет экземпляр компонента камеры.
        /// </summary>
        public Camera()
        {
            if (mainCamera == null)
            {
                isMain = true;
                mainCamera = this;
            }
        }

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();
            graphicsDevice = GameSettings.Instance.Game.Services.GetService<IGraphicsDeviceService>().GraphicsDevice;
            basicEffect = new BasicEffect(graphicsDevice);

            PointOfFocus = focusToCenter ? graphicsDevice.Viewport.Bounds.Size.ToVector2() / 2 : PointOfFocus / 2;


            base.Awake();
        }

        protected override void Shutdown()
        {

            base.Shutdown();
        }

        void ICamera.UpdateMatrix()
        {
            if (!transformer.HasChanges)
                return;

            Transform2D local = transformer.Local;
            Transform2D worldTransform = transformer.Parent.World;

            float rotation = FixedRotation ? local.Rotation : local.Rotation - worldTransform.Rotation;

            PointOfFocus = focusToCenter ? graphicsDevice.Viewport.Bounds.Size.ToVector2() / 2 : PointOfFocus / 2;

            projection = Matrix.CreateOrthographicOffCenter(-PointOfFocus.X, PointOfFocus.X,
                PointOfFocus.Y, -PointOfFocus.Y, 0f, 1000);

            if (graphicsDevice.UseHalfPixelOffset)
            {
                projection.M41 += -0.5f * projection.M11;
                projection.M42 += -0.5f * projection.M22;
            }

            world = 
                Matrix.CreateTranslation(new Vector3(-worldTransform.Position, 0f))
                * Matrix.CreateRotationZ(rotation)
                * Matrix.CreateScale(new Vector3(local.Scale, 1f))
                * Matrix.CreateTranslation(new Vector3(-local.Position, 0f));

            view = Matrix.CreateLookAt(new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Up);
        }

        public void Apply(RenderContext renderContext)
        {
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.World = world;
            basicEffect.TextureEnabled = true;

            renderContext.Start(/*transformMatrix: projection * world,*/sortMode: SpriteSortMode.Texture, effect: basicEffect, samplerState: SamplerState.PointClamp);
        }
    }
}
