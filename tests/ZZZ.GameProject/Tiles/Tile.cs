using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ZZZ.Framework;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Assets.Tiling.Physics;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Rendering.Assets;
using ZZZ.Framework.Tiling.Components;

namespace ZZZ.KNI.GameProject
{
    public class Tile : IAsset, ITile, IRenderTile, IColliderTile, IAnimatedTile
    {
        public Sprite[] Sprites { get; set; }
        public float Duration { get; set; } = 1f;
        public Sprite Sprite { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public Color Color { get; set; } = Color.White;
        public List<Vector2> Vertices { get; set; } = new List<Vector2>();

        public string Name { get; set; } = "";

        public void Dispose()
        {

        }

        public virtual void GetAnimationData(Point position, Tilemap tilemap, ref TileAnimationData data)
        {
            data.Duration = Duration;
            data.Sprites = Sprites;
        }

        public virtual void GetColliderData(Point position, Tilemap tilemap, ref TileColliderData renderedTile)
        {

            //renderedTile.Offset = new Vector2(-16);
            //renderedTile.Vertices = new List<Vector2>() { new Vector2(0, 0), new Vector2(0, 32), new Vector2(32), new Vector2(32,0) };
        }

        public virtual void GetData(Point position, Tilemap tilemap, ref Transform2D offset)
        {
            //offset = Transform2D.CreateTranslation(new Vector2( 32f));
        }

        public virtual void GetRenderingData(Point position, Tilemap tilemap, ref TileRenderData renderedTile)
        {
            renderedTile.Sprite = Sprite;
            renderedTile.SpriteEffect = SpriteEffect;
            renderedTile.Color = Color;
        }

        public virtual void Refresh(Point position, Tilemap tilemap)
        {

        }

        public virtual void Shutdown(Point position, Tilemap tilemap, GameObject container)
        {

        }

        public virtual void Startup(Point position, Tilemap tilemap, GameObject container)
        {
            //container.AddComponent<Rigidbody>();
        }
    }
}
