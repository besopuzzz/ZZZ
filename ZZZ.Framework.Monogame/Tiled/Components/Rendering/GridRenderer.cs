//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using ZZZ.Framework.Monogame.Components.Tiled;

//namespace ZZZ.Framework.Monogame.Components.Rendering
//{
//    public class GridRenderer : Renderer
//    {
//        public Color Color { get; set; } = Color.Gray;

//        private Grid grid;
//        private GraphicsDevice device;
//        private Texture2D pixel;

//        protected override void StartUp()
//        {
//            grid = GetComponent<Grid>();
//            device = GameManager.game.GraphicsDevice;
//            pixel = new Texture2D(device, 1, 1);
//            pixel.SetData(new Color[] { Color.White });

//            base.StartUp();
//        }

//        protected override void Draw(SpriteBatch spriteBatch)
//        {
//            Transform world = GameObject.World;
//            Point size = device.Viewport.Bounds.Size / grid.CellSize;

//            Vector2 sizeX = new Vector2(1, device.Viewport.Height);
//            Vector2 sizeY = new Vector2(device.Viewport.Width, 1);

//            for (int x = 0; x <= size.X; x++)
//            {
//                Transform worldX = new Transform(new Vector2(x * grid.CellSize.X, 0), sizeX) * GameObject.World;

//                spriteBatch.Draw(pixel, worldX.Position, null, Color, worldX.Rotation,
//                    Vector2.Zero, new Vector2(1, worldX.Scale.Y), SpriteEffects.None, 0f);
//            }

//            for (int y = 0; y <= size.Y; y++)
//            {
//                Transform worldY = new Transform(new Vector2(0, y * grid.CellSize.Y), sizeY) * GameObject.World;

//                spriteBatch.Draw(pixel, worldY.Position, null, Color, worldY.Rotation,
//                    Vector2.Zero, new Vector2(worldY.Scale.X, 1), SpriteEffects.None, 0f);
//            }

//            base.Draw(spriteBatch);
//        }
//    }
//}
