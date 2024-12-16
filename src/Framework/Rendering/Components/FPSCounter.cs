using ZZZ.Framework.Assets;
using ZZZ.Framework.Components;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Updating;

namespace ZZZ.Framework.Rendering.Components
{
    /// <summary>
    /// Представляет компонент для подсчета кадров за секунду.
    /// </summary>
    [RequiredComponent<Transformer>]
    public sealed class FPSCounter : Component, IUpdater, IRenderer
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

        public SpriteFont Font { get; set; }

        private SortLayer layer = SortLayer.Layer1;

        private int frameCount = 0;
        private float elapsedTime = 0.0f;
        private float fps = 0f;
        private Transformer transformer;
        private int order = 10000;


        public FPSCounter()
        {

        }

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();

            base.Awake();
        }

        void IRenderer.Render(IRenderProvider provider)
        {
            frameCount++;
            if (elapsedTime >= 1.0f)
            {
                fps = frameCount / elapsedTime;
                frameCount = 0;
                elapsedTime = 0.0f;
            }

            provider.RenderText(Font, $"FPS: {fps}", transformer.World, Color.White, Vector2.Zero, SpriteEffects.None);
        }

        void IUpdater.Update(TimeSpan time)
        {
            elapsedTime += (float)time.TotalSeconds;
        }
    }
}
