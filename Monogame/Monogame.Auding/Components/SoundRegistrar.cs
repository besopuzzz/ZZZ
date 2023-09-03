namespace ZZZ.Framework.Monogame.Auding.Components
{
    internal class SoundRegistrar : MonogameRegistrar<ISoundComponent>
    {
        private ISoundListener listener;
        private List<ISoundEmitter> emitters = new List<ISoundEmitter>();

        protected override void Reception(ISoundComponent component)
        {
            if (component is ISoundListener soundListener)
            {
                if (listener != null)
                    return;

                listener = soundListener;

                foreach (var emitter in emitters)
                {
                    emitter.Listener = listener;
                    listener.Emitters.Add(emitter);
                }

                emitters.Clear();
            }
            else
            {
                if (component is ISoundEmitter emitter)
                {
                    if (listener == null)
                        emitters.Add(emitter);
                    else
                    {
                        emitter.Listener = listener;
                        listener.Emitters.Add(emitter);
                    }
                }
            }

            base.Reception(component);
        }
        protected override void Departure(ISoundComponent component)
        {
            if (component is ISoundListener soundListener)
            {
                soundListener.Emitters.Clear();
                listener = null;
            }
            else
            {
                if (component is ISoundEmitter emitter)
                {
                    if (listener == null)
                        emitters.Remove(emitter);
                    else
                    {
                        emitter.Listener = null;
                        listener.Emitters.Remove(emitter);
                    }
                }
            }

            base.Departure(component);
        }

        protected override void Shutdown()
        {
            emitters.Clear();

            base.Shutdown();
        }
    }
}
