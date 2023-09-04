using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework;
using ZZZ.Framework.Monogame;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace WinDX
{
    public class PrefabCreaterComponent : UpdateComponent
    {
        public IContainer Prefab { get; set; }
        public IContainer DropContainer { get; set; }

        private MouseState mouseState;
        private MouseState oldMouseState;

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            if(mouseState.LeftButton == ButtonState.Pressed & oldMouseState.LeftButton == ButtonState.Released) 
            {
                var copy = AssetManager.CreateCopy2<IContainer>(Prefab);
                Transformer transformer = copy.GetComponent<Transformer>();

                Transform2D mouseTransform = new Transform2D(mouseState.Position.ToVector2());

                transformer.Local = mouseTransform;

                DropContainer.AddContainer(copy);
            }

            oldMouseState = mouseState;

            base.Update(gameTime);
        }
    }
}
