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
    [RequireComponent(Type = typeof(Transformer))]
    public class PrefabCreaterComponent : UpdateComponent
    {
        public IContainer Prefab { get; set; }
        public IContainer DropContainer { get; set; }

        private MouseState mouseState;
        private MouseState oldMouseState;
        private Task<IContainer> prefab;
        
        private Transformer parentTranaform;

        protected override void Startup()
        {
            parentTranaform = GetComponent<Transformer>();

            base.Startup();
        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            if (prefab != null)
            {
                if (prefab.IsCompleted)
                {
                    var component = prefab.Result as IContainer;

                    Transformer transformer = component.GetComponent<Transformer>();
                    Transform2D mouseTransform = Transform2D.CreateTranslation(mouseState.Position.ToVector2()) / parentTranaform.World;

                    transformer.Local = transformer.Local * Transform2D.CreateTranslation(mouseTransform.Position - transformer.Local.Position);

                    DropContainer.AddContainer(component);

                    prefab = null;
                }
            }
            else
            {

                if (mouseState.LeftButton == ButtonState.Pressed & oldMouseState.LeftButton == ButtonState.Released)
                {
                    prefab = AssetManager.CreateCopyAsync<IContainer>(Prefab);
                }
            }

            oldMouseState = mouseState;

            base.Update(gameTime);
        }
    }
}
