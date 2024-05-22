namespace ZZZ.Framework.Auding
{
    public interface ISoundComponent
    {
    }

    public interface ISoundEmitter
    {
        public bool UseDistanceScaling { get; }
        public float MaxRadius { get; }
        public float MinRadius { get; }
    }

    public interface ISoundListener
    {
        void GetVolumeFromPosition(Vector2 position, float minRadius, float maxRadius, out float newVolume);
        float GetPanFromEmitter(Vector2 position, ISoundEmitter emitter);
    }
}
