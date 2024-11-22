using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Components;
using ZZZ.Framework.Components.Transforming;

namespace ZZZ.Framework.Tiling.Components
{
    [RequiredComponent<Transformer>]
    public class TileComponent : Component
    {
        public Point Position { get; set; }

        [ContentSerializer(SharedResource = true)]
        public ITile BaseTile { get; set; }

        private Tilemap tilemap;
        private Transformer transformer;

        public TileComponent()
        {

        }

        protected override void Awake()
        {
            tilemap = Owner.Owner?.GetComponent<Tilemap>();

            if (tilemap == null)
                throw new Exception($"Tilemap not found in parent GameObject");

            transformer = GetComponent<Transformer>();

            BaseTile.Startup(Position, tilemap, Owner);
            BaseTile.Refresh(Position, tilemap);

            base.Awake();
        }

        protected override void Shutdown()
        {
            BaseTile.Refresh(Position, tilemap);
            BaseTile.Shutdown(Position, tilemap, Owner);

            base.Shutdown();
        }

        public void SetData()
        {
            Transform2D offset = new Transform2D();

            BaseTile.GetData(Position, tilemap, ref offset);

            transformer.Local = offset * Transform2D.CreateTranslation(tilemap.GetPositionFromPoint(Position));
        }
    }
}
