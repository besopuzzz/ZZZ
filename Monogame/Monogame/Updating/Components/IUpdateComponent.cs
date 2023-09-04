using Microsoft.Xna.Framework;

namespace ZZZ.Framework.Monogame.Updating.Components
{
    public interface IUpdateComponent : IComponent
    {
        int UpdateOrder { get; }
        event EventHandler<EventArgs> UpdateOrderChanged;
        void Update(GameTime gameTime);
    }
}
