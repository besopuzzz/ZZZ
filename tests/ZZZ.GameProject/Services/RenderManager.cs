//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using ZZZ.Framework.Core.Rendering;
//using ZZZ.Framework.Rendering.Assets;



//namespace ZZZ.KNI.GameProject.Services
//{
//    public class RenderManager : IRenderProvider
//    {
//        private SpriteBatch spriteBatch;
//        private GraphicsDeviceManager graphicsDevice;

//        public RenderManager(Game game) 
//        {
//            this.graphicsDevice = game.Services.GetService<IGraphicsDeviceManager>() as GraphicsDeviceManager;
//            graphicsDevice.DeviceCreated += GraphicsDevice_DeviceCreated;

//        }

//        private void GraphicsDevice_DeviceCreated(object sender, System.EventArgs e)
//        {
//            if (spriteBatch == null)
//                spriteBatch = new SpriteBatch(graphicsDevice.GraphicsDevice);
//        }

//        public virtual void EndRendering()
//        {
//            spriteBatch.End();
//        }

//        public virtual void Render(Sprite sprite, float positionX, float positionY, float rotation,
//            float scaleX, float scaleY, uint color, bool flipX, bool flipY, float order)
//        {
//            var sprite2 = sprite as Sprite;

//            SpriteEffects spriteEffects = SpriteEffects.None;

//            if (flipX)
//                spriteEffects |= SpriteEffects.FlipHorizontally;

//            if (flipY)
//                spriteEffects |= SpriteEffects.FlipVertically;

//            spriteBatch.Draw(sprite2.Texture, new Vector2(positionX, positionY), sprite2.Source, new Color(color), rotation, sprite2.Origin, new Vector2(scaleX, scaleY), spriteEffects, order);
//        }

//        public virtual void RenderText(string text, float positionX, float positionY, float rotation, float scaleX, float scaleY, float originX, float originY, uint color, bool flipX, bool flipY, float order)
//        {
//            SpriteEffects spriteEffects = SpriteEffects.None;

//            if (flipX)
//                spriteEffects |= SpriteEffects.FlipHorizontally;

//            if (flipY)
//                spriteEffects |= SpriteEffects.FlipVertically;

//            //spriteBatch.DrawString(text, new Vector2(positionX, positionY), sprite2.Source, new Color(color), rotation, sprite2.Origin, new Vector2(scaleX, scaleY), spriteEffects, order);
//        }

//        public virtual void StartRendering()
//        {
//            spriteBatch.Begin();
//        }
//    }
//}
