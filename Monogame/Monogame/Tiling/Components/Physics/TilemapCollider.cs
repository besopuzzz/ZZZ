using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using ZZZ.Framework.Monogame.FarseerPhysics;
using ZZZ.Framework.Monogame.FarseerPhysics.Components;
using ZZZ.Framework.Monogame.Tiling.Assets;

namespace ZZZ.Framework.Monogame.Tiling.Components.Physics
{
    public class TilemapCollider : Collider
    {
        public event EventHandler<TilemapColliderEventArgs> Collision;

        private Tilemap tilemap;
        private Dictionary<Fixture, Point> fixtures = new Dictionary<Fixture, Point>();

        protected override void Startup()
        {
            tilemap = GetComponent<Tilemap>();
            tilemap.TileAdded += Tilemap_TileAdded;
            tilemap.TileRemoved += Tilemap_TileRemoved;

            base.Startup();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ((IDisposable)tilemap)?.Dispose();

            tilemap = null;
            fixtures = null;

            base.Dispose(disposing);
        }

        private void CreatBody(TileRenderData tile)
        {
            var fixture = Body.CreateRectangle(tilemap.GetCellSize().X, tilemap.GetCellSize().Y, 1f,
                 (tile.Local.Position + tilemap.GetCellSize() / 2 - tilemap.CellOrigin).ToAether());

            fixture.CollisionCategories = Category;
            fixture.CollidesWith = Category;
            fixture.Tag = this;
            fixture.OnCollision = OnCollision;
            fixture.IsSensor = IsTrigger;

            fixtures.Add(fixture, tile.Position);
        }

        private bool OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (Collision == null)
                return true;

            if (sender.Tag == this)
            {
                var point = fixtures[sender];
                var args = new TilemapColliderEventArgs((Collider)other.Tag, tilemap.GetTile(point), point);

                Collision?.Invoke(this, args);

                return !args.Ignore;
            }

            return true;
        }

        private void RemoveBody(Fixture fixture)
        {
            fixture.OnCollision = null;

            Body.Remove(fixture);
            fixtures.Remove(fixture);
        }

        private void Tilemap_TileRemoved(Point position, Tile baseTile)
        {
            RemoveBody(fixtures.First(x => x.Value == position).Key);
        }

        private void Tilemap_TileAdded(Point position, Tile baseTile)
        {
            CreatBody(tilemap.Tiles.First(x => x.Position == position));
        }

        protected override void Create()
        {
            foreach (var item in tilemap.Tiles)
            {
                CreatBody(item);
            }
        }

        protected override void Clear()
        {
            foreach (var item in fixtures.Keys.ToList())
            {
                RemoveBody(item);
            }
        }

        protected override void OnIsTriggerChanged()
        {
            foreach (var item in fixtures.Keys.ToList())
            {
                item.IsSensor = IsTrigger;
            }
        }

        protected override void OnMaterialChanged()
        {
            foreach (var item in fixtures.Keys.ToList())
            {
                item.Friction = Material.Friction;
                item.Restitution = Material.Restitution;
            }
        }
        protected override void OnOffsetChanged(Vector2 oldValue, Vector2 newValue)
        {
            foreach (var item in fixtures.Keys.ToList())
            {
                var shape = item.Shape as PolygonShape;

                shape.Vertices.Translate(oldValue.ToAether());
                shape.Vertices.Translate(-newValue.ToAether());
            }
        }

        protected override void OnLayerChanged()
        {
            foreach (var item in fixtures.Keys.ToList())
            {
                item.CollisionCategories = Category;
                item.CollidesWith = Category;
            }

            base.OnLayerChanged();
        }
    }
}
