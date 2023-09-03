using Microsoft.Xna.Framework;
using ZZZ.Framework.Monogame.Tiling.Components;

namespace ZZZ.Framework.Monogame.Tiling.Assets
{
    public class RuledTile : Tile
    {
        public List<TileRule> Rules { get; set; }

        public Tile Some
        {
            get
            {
                return some;
            }
            set
            {
                if (some == value)
                    return;

                some = value;
                Sprite = some.Sprite;
                Color = some.Color;
                SpriteEffect = some.SpriteEffect;
            }
        }

        private Tile some;

        internal RuledTile()
        {

        }

        public RuledTile(Tile some, List<TileRule> rules) : base(some.Sprite, some.Color, some.SpriteEffect)
        {
            Some = some;
            Rules = rules;
        }

        public override void Refresh(Point position, Tilemap tilemap)
        {
            foreach (var item in Rules)
            {
                foreach (var checker in item.Neighbors)
                {
                    if (HasRuledTile(tilemap, position + checker.Position))
                        tilemap.Refresh(position + checker.Position);
                }
            }
        }

        public override Tile GetTile(Point position, Tilemap tilemap)
        {
            var tile = Check(tilemap, position);

            if (tile != this)
            {
                return tile;
            }

            return some;
        }

        private Tile Check(Tilemap tilemap, Point position)
        {
            foreach (var item in Rules)
            {
                var result = false;

                foreach (var checker in item.Neighbors)
                {
                    var tile = HasRuledTile(tilemap, position + checker.Position);
                    result = GetResultLigament(checker.Type, tile);
                    if (!result)
                        break;
                }

                if (result)
                    return item.Tile;
            }

            return this;
        }

        private bool HasRuledTile(Tilemap tilemap, Point position)
        {
            return tilemap.GetTile(position) == this;
        }

        private bool GetResultLigament(TileRulePointType ruleType, bool exist)
        {
            switch (ruleType)
            {
                case TileRulePointType.Exist:
                    return exist;
                case TileRulePointType.None:
                    return !exist;
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                some.Dispose();

                foreach (var item in Rules)
                {
                    item.Dispose();
                }
            }
            some = null;
            Rules = null;

            base.Dispose(disposing);
        }
    }
}
