using ZZZ.Framework.Components;
using ZZZ.Framework.Tiling.Assets;

namespace ZZZ.Framework.Tiling.Components
{
    [RequiredComponent<Transformer>]
    public sealed class TileComponent : Component
    {
        public Point Position { get; internal set; }

        [ContentSerializer(SharedResource = true)]
        public ITile BaseTile { get; internal set; }

        private Tilemap tilemap;
        private Transformer transformer;

        protected override void OnCreated()
        {
            tilemap = Owner.Owner?.GetComponent<Tilemap>();

            if (BaseTile != null)
                return;

            tilemap?.SetReference(this);
        }

        protected override void Awake()
        {
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
            Transform2D offset = new();

            BaseTile.GetData(Position, tilemap, ref offset);

            transformer.Local = offset * Transform2D.CreateTranslation(tilemap.GetPositionFromCell(Position));
        }
    }
}
