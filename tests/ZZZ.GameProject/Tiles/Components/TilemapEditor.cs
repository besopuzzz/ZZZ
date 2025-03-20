using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using ZZZ.Framework;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.Framework.Rendering.Components;
using ZZZ.Framework.Tiling.Components;
using ZZZ.Framework.Updating;
using ZZZ.KNI.GameProject;

namespace ZZZ.GameProject.Tiles.Components
{
    [RequiredComponent<Tilemap>]
    public class TilemapEditor : Component, IUpdater, ILateUpdater
    {
        private Tilemap tilemap;

        private TestTile heroTile = new TestTile();

        private MouseState mouseState;

        private MouseState oldmouseState;

        private Camera camera;

        private Transformer transformer;

        public class TestTile : Tile
        {
            public override void Startup(Point position, Tilemap tilemap, GameObject container)
            {
                base.Startup(position, tilemap, container);
            }

            public override void Refresh(Point position, Tilemap tilemap)
            {
                base.Refresh(position, tilemap);
            }
        }

        protected override void Awake()
        {
            Sprite main = AssetManager.Load<Sprite>("Sprites/main");

            heroTile.Color = Color.Red;
            heroTile.Sprite = main[0];
            heroTile.Sprites = [main[0], main[1], main[2]];

            tilemap = GetComponent<Tilemap>();

            transformer = new GameObject().AddComponent<Transformer>();
            transformer.Owner.AddComponent<SpriteRenderer>().Sprite = main[20];

            AddGameObject(transformer.Owner);

            base.Awake();
        }

        void IUpdater.Update(TimeSpan time)
        {
            //if (mouseState.LeftButton == ButtonState.Pressed & oldmouseState.LeftButton == ButtonState.Released)
            //{
            //    tilemap.Add(pos, heroTile);
            //}


            //if (mouseState.RightButton == ButtonState.Pressed & oldmouseState.RightButton == ButtonState.Released)
            //{
            //    tilemap.Remove(pos);
            //}

        }

        void ILateUpdater.LateUpdate()
        {
            if (!MainGame.instance.IsActive)
                return;

            camera = Camera.MainCamera;

            if (camera == null)
                return;

            mouseState = Mouse.GetState();

            transformer.Local = camera.ToWorld(Transform2D.CreateTranslation(mouseState.Position.ToVector2()));

            MainGame.instance.Window.Title = transformer.Local.ToString();

            oldmouseState = mouseState;
        }
    }
}
