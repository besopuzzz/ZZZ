namespace ZZZ.Framework.Monogame.Auding
{
    public interface ISoundComponent : IComponent
    {
    }

    public interface ISoundEmitter : ISoundComponent
    {
        ISoundListener Listener { get; set; }
        void ApplyDistancing();
        public void Play();
        public void Pause();
        public void Resume();
        public void Stop();
    }

    public interface ISoundListener : ISoundComponent
    {
        List<ISoundEmitter> Emitters { get; }
    }
}
