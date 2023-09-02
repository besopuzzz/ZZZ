using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;

namespace ZZZ.Framework.Monogame.Audio.Components
{
    public class SoundListener : Component, ISoundListener
    {
        private List<ISoundEmitter> emitters = new List<ISoundEmitter>();
        private Transformer transformer;

        List<ISoundEmitter> ISoundListener.Emitters => emitters;

        private void ApplyDistancing()
        {
            foreach (var item in emitters)
            {
                item.ApplyDistancing();
            }
        }

        protected override void Startup()
        {
            RegistrationComponent<ISoundComponent>(this);

            transformer = GetComponent<Transformer>();
            transformer.WorldChanged += Transformer_WorldChanged;

            ApplyDistancing();

            base.Startup();
        }

        protected override void Shutdown()
        {
            UnregistrationComponent<ISoundComponent>(this);
            transformer.WorldChanged -= Transformer_WorldChanged;

            base.Shutdown();
        }

        private void Transformer_WorldChanged(ITransformer sender, Transform2D args)
        {
            ApplyDistancing();
        }

        protected override void OnEnabledChanged()
        {
            ApplyDistancing();

            base.OnEnabledChanged();
        }
    }
}
