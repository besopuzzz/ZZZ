using ZZZ.Framework.Components;
using ZZZ.Framework.Tiling.Components;

namespace ZZZ.Framework.Tiling
{
    public sealed class RequiredTilemapAttribute : RequiredComponentAttribute
    {
        private sealed class TilemapAutoReference : AutoReferenceComponent<Tilemap>
        {
            protected override void Connect(Component caller, Tilemap other)
            {
                if (caller is ITilemap tilemap1)
                    other.Tilemaps.Add(tilemap1);
            }

            protected override void Disconnect(Component caller, Tilemap other)
            {
                if (caller is ITilemap tilemap1)
                    other.Tilemaps.Remove(tilemap1);
            }
        }

        public RequiredTilemapAttribute() : base(typeof(Tilemap), false, typeof(TilemapAutoReference))
        {

        }
    }
}
