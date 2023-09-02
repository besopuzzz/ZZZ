using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using ZZZ.Framework.Monogame.Content;
using ZZZ.Framework.Monogame.Rendering;
using ZZZ.Framework.Monogame.Rendering.Components;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Main.UI
{
    public class Label : 
        RenderComponent, IUpdateComponent
    {
        public Font Font { get; set; }
        public string Text 
        { 
            get => text;
            set
            {
                if (text == value)
                    return;

                text = value;

                UpdatePosition();
            }
        }
        public int UpdateOrder { get; set; }
        public Vector4 Padding { get; set; }

        protected Color BackgroundColor = Color.Black;

        private Sprite pixel;
        private Rectangler  rectangler;
        private MouseState mouseState;
        private bool onMe;
        private Rectangle bounds = new Rectangle();

        private bool started = false;
        private string text;

        public event EventHandler<EventArgs> UpdateOrderChanged;

        protected override void Startup()
        {
            Layer = RenderLayer.Eighth;
            pixel = Renderer.CreateSprite(new Point(1), Color.White);
            rectangler = GetComponent<Rectangler>();

            RegistrationComponent<IUpdateComponent>(this);

            started = true;
            UpdatePosition();

            base.Startup();
        }

        protected override void Draw()
        {
            Renderer.DrawSprite(pixel, rectangler.World, BackgroundColor, Depth);
            Renderer.DrawText(Font, Text, rectangler.World, Color.White, bounds.Size.ToVector2(),Depth);

            base.Draw();
        }

        protected virtual void MouseEnter(MouseState mouseState)
        {

        }
        protected virtual void MouseExit(MouseState mouseState)
        {

        }

        void IUpdateComponent.Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            Rectangle local = rectangler.Local;
            Point location = local.Location;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                location.X += -1;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                location.X += 1;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                location.Y += -1;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                location.Y += 1;

            rectangler.Local = new Rectangle(location, rectangler.Local.Size);

            //if(bounds.Contains(mouseState.Position))
            //{
            //    MouseEnter(mouseState);
            //}
            //else
            //{
            //    MouseExit(mouseState);
            //}
        }

        private void UpdatePosition()
        {
            return;

            if (!started)
                return;

            if (Font == null)
                return;

            //measureString = Font.MeasureString(text);
            //var offset = new Vector2(Padding.X, Padding.Y);

            //backgroundTransform = new Transform2D((-measureString / 2) - offset,
            //    measureString + new Vector2(Padding.Z, Padding.W) + offset);
            //textTransform = new Transform2D(-measureString / 2);
        }
    }
}
