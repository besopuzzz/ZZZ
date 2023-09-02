using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.Rendering
{
    public class ShaderRenderer : RenderComponent, IShaderComponent
    {
        public Effect Effect { get; set; }



        protected override void Draw()
        {


            base.Draw();
        }

        void IShaderComponent.Draw(RenderBatch renderBatch, RenderTarget2D renderTarget)
        {
            if (Effect == null)
                return;

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                renderBatch.Begin(SpriteSortMode.Immediate);

                pass.Apply();
                renderBatch.Draw(renderTarget);
                renderBatch.End();
            }
        }
    }
}
