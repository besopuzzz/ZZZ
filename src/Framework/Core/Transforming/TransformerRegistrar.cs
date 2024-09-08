using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Core.Transforming
{
    public class TransformerRegistrar : BaseRegistrar<Transformer>
    {
        private List<Transformer> transformers = new List<Transformer>();

        public TransformerRegistrar()
        {
            UpdateOrder = int.MaxValue;
        }

        protected override void OnReception(Transformer component)
        {
            transformers.Add(component);
        }

        protected override void OnDeparture(Transformer component)
        {
            transformers.Remove(component);
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var item in transformers)
            {
                item.HasChanges = false;
            }

            base.Update(gameTime);
        }
    }
}
