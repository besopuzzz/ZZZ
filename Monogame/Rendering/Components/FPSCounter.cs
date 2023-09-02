namespace ZZZ.Framework.Monogame.Rendering.Components
{
    public sealed class FPSCounter : RenderComponent
    {
        [ContentSerializerIgnore]
        public float FPS => fps;

        private int frameCount = 0;
        private float elapsedTime = 0.0f;
        private float fps = 0f;

        protected override void Draw()
        {
            UpdateFPS(Renderer.GameTime);

            base.Draw();
        }


        private void UpdateFPS(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;
            if (elapsedTime >= 1.0f)
            {
                fps = frameCount / elapsedTime;
                frameCount = 0;
                elapsedTime = 0.0f;
            }
        }
    }
}
