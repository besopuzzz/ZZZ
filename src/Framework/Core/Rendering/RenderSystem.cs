//using Microsoft.Xna.Framework.Graphics;
//using System.ComponentModel;
//using ZZZ.Framework.Assets.Pipeline.Readers;
//using ZZZ.Framework.Core.Rendering.Components;

//namespace ZZZ.Framework.Core.Rendering
//{
//    public class RenderSystem : GameSystem<IRender, RenderEntity>
//    {
//        private GraphicsDevice device;
//        private SpriteBatch spriteBatch;

//        protected override RenderEntity CreateEntityObject()
//        {
//            return new RenderEntity(SceneLoader.CurrentScene);
//        }

//        protected override void Draw(GameTime gameTime)
//        {
//            if(device == null)
//            {
//                device = ((GraphicsDeviceManager)GameManager.Game.Services.GetService<IGraphicsDeviceManager>()).GraphicsDevice;
                
//                if(spriteBatch == null)
//                    spriteBatch = new SpriteBatch(device);
//            }

//            spriteBatch.Begin();

//            EntityScene.InternalRender(spriteBatch);

//            spriteBatch.End();
//        }
//    }

//    public class RenderEntity : EntityObject<IRender, RenderEntity>
//    {
//        protected IRender RenderComponent { get; private set; }

//        internal RenderEntity(Scene scene) : base(scene) { }

//        protected RenderEntity(IRender render) : base()
//        {
//            this.RenderComponent = render;
//        }

//        protected RenderEntity(GameObject gameObject) : base(gameObject)
//        {

//        }

//        public override RenderEntity CreateEntity(GameObject gameObject)
//        {
//            return new RenderEntity(gameObject);
//        }

//        internal void InternalRender(SpriteBatch spriteBatch)
//        {
//            Render(spriteBatch);
//        }

//        protected virtual void Render(SpriteBatch spriteBatch)
//        {
//            var component = RenderComponent as IRenderElement;

//            if (component != null)
//            {
//                if (component.Enabled)
//                    component.Render(spriteBatch);
//            }

//            if (Childs == null)
//                return;

//            foreach (var item in Childs)
//            {
//                item.Render(spriteBatch);
//            }
//        }

//        protected override RenderEntity GetRemovedEntity(GameObject gameObject)
//        {
//            return Childs.Find(x => x.GameObject == gameObject & x.RenderComponent == null);
//        }

//        protected override void OnComponentAdded(IRender component)
//        {
//            RenderEntity renderEntity;

//            if (component is IGroupRender group)
//                renderEntity = new GroupRenderEntity(group);
//            else renderEntity = new RenderEntity(component);

//            Childs.Add(renderEntity);
//        }

//        protected override void OnComponentRemoved(IRender component)
//        {
//            var entity = Childs.Find(x => x.RenderComponent == component);

//            Childs.Remove(entity);
//        }



//        private class GroupRenderEntity : RenderEntity
//        {
//            private SpriteBatch localSpriteBatch;

//            internal GroupRenderEntity(IGroupRender render) : base(render)
//            {
//            }

//            protected override void Render(SpriteBatch spriteBatch)
//            {
//                if (!RenderComponent.Enabled)
//                {
//                    base.Render(spriteBatch);

//                    return;
//                }

//                localSpriteBatch.Begin();

//                foreach (var item in Childs)
//                {
//                    item.Render(localSpriteBatch);
//                }

//                localSpriteBatch.End();
//            }
//        }
//    }


//}
