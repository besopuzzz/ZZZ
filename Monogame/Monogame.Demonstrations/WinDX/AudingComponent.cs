using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework;
using ZZZ.Framework.Monogame;
using ZZZ.Framework.Monogame.Auding.Assets;
using ZZZ.Framework.Monogame.Auding.Components;
using ZZZ.Framework.Monogame.Rendering.Components;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace WinDX
{
    [RequireComponent(Type = typeof(Transformer))]
    public class AudingComponent : UpdateComponent
    {
        private MouseState mouseState;
        private Transformer transformer;
        private Transformer transformerContainer;

        protected override void Startup()
        {
            transformer = GetComponent<Transformer>();  
            var container = AddContainer(new Container());

            var font = new Font(AssetManager.Load<SpriteFont>("DiagnosticsFont"));

            transformerContainer = container.AddComponent(new Transformer());
            container.AddComponent(new TextRenderer(font) { Offset = new Vector2(0, -10f)}).Text.Append("Sound emmiter here!");
            container.AddComponent(new SoundEmitter() { Sound = new Sound(AssetManager.Load<SoundEffect>("Musics/kalambur")), IsLooped = true, Volume = 0.05f });

            base.Startup();
        }

        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            transformerContainer.Local = Transform2D.CreateTranslation(mouseState.Position.ToVector2()) / transformer.World;

            base.Update(gameTime);
        }
    }
}
