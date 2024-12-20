﻿using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Tiling.Components
{
    public struct TileRenderData
    {
        public Sprite Sprite { get; set; } = null;
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        public Color Color { get; set; } = Color.White;

        public TileRenderData()
        {

        }
    }
}
