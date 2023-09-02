// Copyright (c) 2017 Kastellanos Nikolaos

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = tainicom.Aether.Physics2D.Common.Vector2;
using Vector3 = tainicom.Aether.Physics2D.Common.Vector3;

namespace tainicom.Aether.Physics2D.Diagnostics
{
    public interface IPrimitiveBatch
    {        
        void Begin(ref Matrix projection, ref Matrix view, ref Matrix world, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, float alpha);
        void End();
        bool IsReady();
        int AddVertex(Vector3 position, Color color, PrimitiveType primitiveType);
        int AddVertex(Vector2 position, Color color, PrimitiveType primitiveType);
        int AddVertex(ref Vector2 position, Color color, PrimitiveType primitiveType);
        int AddVertex(ref Vector3 position, Color color, PrimitiveType primitiveType);        
    }
}
