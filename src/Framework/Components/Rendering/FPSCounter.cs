using ZZZ.Framework.Assets;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Components.Updating;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;

namespace ZZZ.Framework.Components.Rendering
{
    /// <summary>
    /// Представляет компонент для подсчета кадров за секунду.
    /// </summary>
    [RequireComponent(typeof(Transformer))]
    public sealed class FPSCounter : Component, IUpdateComponent, IRender
    {
        /// <summary>
        /// Количество кадров за секунду.
        /// </summary>
        [ContentSerializerIgnore]
        public float FPS => fps;

        public SortLayer Layer
        {
            get => layer;
            set => layer = value;
        }

        public int Order { get => order; set => order = value; }

        private SortLayer layer = SortLayer.Layer1;

        private int frameCount = 0;
        private float elapsedTime = 0.0f;
        private float fps = 0f;
        private SpriteFont font;
        private Transformer transformer;
        private int order = 10000;


        public FPSCounter()
        {

        }

        protected override void Awake()
        {

            font = AssetManager.Load<SpriteFont>("DiagnosticsFont");
            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        void IRender.Render(SpriteBatch spriteBatch)
        {
            frameCount++;
            if (elapsedTime >= 1.0f)
            {
                fps = frameCount / elapsedTime;
                frameCount = 0;
                elapsedTime = 0.0f;
            }


            spriteBatch.DrawText(font, $"FPS: {fps}", transformer.World, Color.White, Vector2.Zero, SpriteEffects.None, false);
        }

        void IUpdateComponent.Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
