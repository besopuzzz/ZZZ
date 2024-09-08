//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System;
//using ZZZ.Framework;
//using ZZZ.Framework.Components.Rendering;
//using ZZZ.Framework.Components.Transforming;
//using ZZZ.Framework.Core.Physics;
//using ZZZ.Framework.Core.Rendering;
//using ZZZ.Framework.Core.Rendering.Components;
//using ZZZ.Framework.Core.Updating.Components;
//using ZZZ.Framework.Physics.Components;

//namespace ZZZ.KNI.GameProject
//{
//    [RequireComponent(typeof(Transformer))]
//    internal class TestComponent : Framework.Component, IStartupComponent, IUpdateComponent, IRender, IStopComponent
//    {
//        public int Order { get => order; set => order = value; }
//        public SortLayer Layer
//        {
//            get => layer;
//            set
//            {
//                if (layer == value)
//                    return;

//                SortLayer oldValue = value;

//                layer = value;

//                LayerChanged?.Invoke(this, new SortLayerArgs(oldValue, value));
//            }
//        }

//        public event EventHandler<SortLayerArgs> LayerChanged;


//        private SortLayer layer = SortLayer.Layer11;
//        private int order;
//        private Collider collider;
//        private Transformer transformer;
//        private RaycastResult raycastResult;

//        protected override void Awake()
//        {
//            transformer = GetComponent<Transformer>();
//            collider = GetComponent<Collider>();
//            collider.ColliderEnter += Collider_ColliderEnter;
//            collider.ColliderExit += Collider_ColliderExit;

//            //Enabled = false;

//            base.Awake();
//        }

//        protected override void Shutdown()
//        {
//            collider.ColliderEnter -= Collider_ColliderEnter;
//            collider.ColliderExit -= Collider_ColliderExit;

//            base.Shutdown();
//        }

//        private void Collider_ColliderExit(Collider sender, Collider other)
//        {
//            GetComponent<SpriteRenderer>().Color = Color.Blue;

//        }

//        private void Collider_ColliderEnter(Collider sender, Collider other)
//        {
//            GetComponent<SpriteRenderer>().Color = Color.Red;
//        }

//        void IStopComponent.Stop()
//        {

//        }

//        void IStartupComponent.Startup()
//        {
//            //AddComponent(new TestComponent2());
//        }

//        void IRender.Render(SpriteBatch spriteBatch)
//        {
//            //spriteBatch.DrawLine(raycastResult.Start, raycastResult.Point, Color.Red, 1f);
//        }

//        void IUpdateComponent.Update(GameTime gameTime)
//        {
//            var pos = transformer.World.Position;

//            raycastResult = PhysicRegistrar.Raycast(pos, pos + Mouse.GetState().Position.ToVector2() - new Vector2(400, 240), ColliderLayer.Cat2);

//            //if(gameTime.TotalGameTime.TotalSeconds > 5)
//            //    Owner.RemoveComponent(this);
//        }
//    }

//    internal class TestComponent2 : Framework.Component, IStartupComponent, IUpdateComponent, IRender, IStopComponent
//    {
//        public int Order { get => order; set => order = value; }
//        public SortLayer Layer
//        {
//            get => layer;
//            set
//            {
//                if (layer == value)
//                    return;

//                SortLayer oldValue = value;

//                layer = value;

//                LayerChanged?.Invoke(this, new SortLayerArgs(oldValue, value));
//            }
//        }

//        public event EventHandler<SortLayerArgs> LayerChanged;

//        private SortLayer layer = SortLayer.Layer1;
//        private int order;

//        void IStopComponent.Stop()
//        {

//        }

//        void IStartupComponent.Startup()
//        {

//        }

//        void IRender.Render(SpriteBatch spriteBatch)
//        {

//        }

//        void IUpdateComponent.Update(GameTime gameTime)
//        {

//        }
//    }
//}