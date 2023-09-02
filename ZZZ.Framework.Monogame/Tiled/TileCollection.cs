namespace ZZZ.Framework.Monogame.Tiled
{
    internal sealed class TileCollection : List<TileRenderData>
    {
        public event TileCollectionEvent TileAdded;
        public event TileCollectionEvent TileRemoved;
        public event TileCollectionEvent TileReplaced;

        public TileRenderData Get(Point position)
        {
            return this.Find(x => x.Position == position);
        }

        public new void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                Remove(this[i]);
            }

            base.Clear();
        }

        public new void Insert(int index, TileRenderData item)
        {
            if (IndexOf(item) != -1)
                return;

            var tile = Get(item.Position);

            if(tile != null)
            {
                tile.Refresh(item);
                TileReplaced?.Invoke(this, item);
                return;
            }

            base.Insert(index, item);
            if (item != null)
            {
                TileAdded?.Invoke(this, item);
            }
        }

        public new bool Remove(TileRenderData item)
        {
            if (item == null)
                return true;

            var result = base.Remove(item);
            if (result)
            {
                TileRemoved?.Invoke(this, item);
            }
            return result;
        }

        public new void Add(TileRenderData item)
        {
            Insert(Count, item);
        }

        public new TileRenderData this[int index]
        {
            get
            {
                return base[index];
            }
        }
    }
}
