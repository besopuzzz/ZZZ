using Microsoft.Xna.Framework.Content;
using System.Collections;
using ZZZ.Framework.Monogame.Asseting;
using ZZZ.Framework.Monogame.Asseting.Assets;

namespace ZZZ.Framework.Monogame.Tiling.Assets
{
    public class Tileset : Asset, IEnumerable<Tile>
    {
        public int Count => tiles.Count;
        public Tile this[int index] => tiles[index];

        [ContentSerializer(ElementName = "Tiles")]
        private SharedList<Tile> tiles = new SharedList<Tile>();

        public Tileset()
        {

        }

        public Tileset(List<Tile> tiles)
        {
            foreach (var item in tiles)
            {
                tiles.Add(item);
                item.Tileset = this;
            }
        }

        public void Add(Tile tile)
        {
            if (tiles.Contains(tile))
                throw new Exception("The tile already exist!");

            tiles.Add(tile);
        }

        public void Remove(Tile tile)
        {
            if (!tiles.Contains(tile))
                throw new Exception("The tile not exist!");

            tiles.Remove(tile);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach (var item in tiles)
                {
                    item.Dispose();
                }
            }

            tiles = null!;

            base.Dispose(disposing);

        }


        public IEnumerator<Tile> GetEnumerator() => tiles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => tiles.GetEnumerator();
    }
}
