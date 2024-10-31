//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ZZZ.Framework.Assets;
//using ZZZ.Framework.Components;
//using ZZZ.Framework.Components.Rendering;
//using ZZZ.Framework.Components.Transforming;
//using ZZZ.Framework.Core.Updating.Components;
//using ZZZ.Framework.Rendering.Assets;
//using ZZZ.Framework.Rendering.Assets.Pipeline;
//using ZZZ.Framework.UserInterfacing.Components;
//using Color = Microsoft.Xna.Framework.Color;
//using Label = ZZZ.Framework.UserInterfacing.Components.Label;
//using TextRenderer = ZZZ.Framework.Components.Rendering.TextRenderer;

//namespace ZZZ.Framework.Editor.GameComponents
//{
//    [RequiredComponent(typeof(FPSCounter))]
//    [RequiredComponent(typeof(Transformer))]
//    [RequiredComponent(typeof(SpriteRenderer))]
//    [RequiredComponent(typeof(Label))]
//    internal class TestComponent : Component, IUpdateComponent
//    {
//        private SpriteRenderer spriteRenderer;
//        private FPSCounter counter;
//        private Transformer transformer;
//        private Label label;

//        protected override void Awake()
//        {
//            label = GetComponent<Label>();
//            transformer = GetComponent<Transformer>();
            
            
//            label.Color = Color.Black;
//            label.Font = new Assets.Rendering.Font(AssetManager.Load<SpriteFont>("DiagnosticsFont"));
//            label.BackgroundColor = Color.Coral;
//            counter = GetComponent<FPSCounter>();
//            spriteRenderer = GetComponent<SpriteRenderer>();
//            spriteRenderer.Sprite = AssetManager.Load<Sprite>("duck");

//            GetComponent<UITransformer>().Size = new Vector2(200f);

//            base.Awake();
//        }

//        void IUpdateComponent.Update(GameTime gameTime)
//        {
//            label.Text = $"FPS: {counter.FPS}";


//            transformer.Local = Transform2D.CreateTranslation(Mouse.GetState().Position.ToVector2());
//        }
//    }
//}
