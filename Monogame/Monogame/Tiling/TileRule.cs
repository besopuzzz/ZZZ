using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ZZZ.Framework.Monogame.Tiling.Assets;

namespace ZZZ.Framework.Monogame.Tiling
{
    public class TileRule : IDisposable
    {
        public List<TileNeighbor> Neighbors { get; set; }

        [ContentSerializer(SharedResource = true)]
        public Tile Tile { get; set; }

        public TileRule()
        {
            Neighbors = new List<TileNeighbor>();
        }

        public TileRule(List<TileNeighbor> neighbors, Tile tile)
        {
            Neighbors = neighbors;
            Tile = tile;
        }

        public void Dispose()
        {
            Neighbors?.Clear();
            Tile?.Dispose();

            Neighbors = null;
            Tile = null;
        }
    }

}