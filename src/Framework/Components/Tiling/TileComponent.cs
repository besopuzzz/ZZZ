using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Components.Transforming;

namespace ZZZ.Framework.Components.Tiling
{
    [RequireComponent(typeof(Transformer))]
    internal class TileComponent : Component
    {
        public Point Position { get; set; }

        [ContentSerializer(SharedResource = true)]
        public ITile BaseTile { get; set; }

        private Tilemap tilemap { get;  set; }

        public event EventHandler<Point, TileComponent> NeedSetData;

        private Transformer transformer;

        internal TileComponent()
        {

        }

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();
            tilemap = Owner.Owner.GetComponent<Tilemap>();

            BaseTile.Startup(Position, tilemap, Owner);
            BaseTile.Refresh(Position, tilemap);

            //transformer.Local = Transform2D.CreateTranslation(tilemap.GetPositionFromPoint(Position));

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

            transformer.Local = Transform2D.CreateTranslation(tilemap.GetPositionFromPoint(Position)) * offset;
        }
    }
}
