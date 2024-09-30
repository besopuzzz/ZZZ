using Microsoft.Xna.Framework.Audio;
using ZZZ.Framework.Auding.Assets;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Components.Updating;

namespace ZZZ.Framework.Auding.Components
{
    public class SoundEmitter : Component, ISoundEmitter, IUpdateComponent
    {
        public Sound Sound
        {
            get => sound;
            set
            {
                effectInstance?.Dispose();

                if (sound == value)
                    return;

                sound = value;

                if (sound == null & !Started)
                    return;

                effectInstance = sound.SoundEffect?.CreateInstance();

                SetSettings();
            }
        }
        public bool IsLooped
        {
            get => isLooped;
            set
            {
                if (isLooped == value)
                    return;

                isLooped = value;

                if (effectInstance != null)
                    effectInstance.IsLooped = isLooped;
            }
        }
        public bool Mute
        {
            get => isMuted;
            set
            {
                if (isMuted == value)
                    return;

                isMuted = value;

                if (effectInstance != null)
                    effectInstance.Volume = isMuted ? 0f : localVolume * volume;
            }
        }
        public float Volume
        {
            get => volume;
            set
            {
                if (volume == value)
                    return;

                volume = MathHelper.Clamp(value, 0f, 1f);

                if (effectInstance != null)
                    effectInstance.Volume = isMuted ? 0f : localVolume * volume;
            }
        }
        public float Pan
        {
            get => pan;
            set
            {
                if (pan == value)
                    return;

                pan = MathHelper.Clamp(value, -1f, 1f);

                if (effectInstance != null)
                    effectInstance.Pan = MathHelper.Clamp(pan + localPan, -1f, 1f);
            }
        }
        public float Pitch
        {
            get => pitch;
            set
            {
                if (pitch == value)
                    return;

                pitch = MathHelper.Clamp(value, -1f, 1f);

                if (effectInstance != null)
                    effectInstance.Pitch = pitch;
            }
        }
        public bool PlayImmediately { get; set; } = true;
        public float MaxRadius { get; set; } = 600f;
        public float MinRadius { get; set; } = 10f;
        public PlayState State
        {
            get
            {
                if (effectInstance == null)
                    return PlayState.Stopped;

                return effectInstance.State switch
                {
                    SoundState.Playing => PlayState.Playing,
                    SoundState.Paused => PlayState.Paused,
                    SoundState.Stopped => PlayState.Stopped,
                    _ => PlayState.Stopped,
                };
            }
        }
        public bool UseDistanceScaling { get; set; }

        private Sound sound;
        private SoundEffectInstance effectInstance;
        private bool isLooped = false;
        private bool isMuted = false;
        private float volume = 0.3f;
        private float pan = 0f;
        private float pitch = 0f;
        private float localVolume = 1f;
        private float localPan = 0f;
        private PlayState lastState = PlayState.Stopped;
        private Transformer transformer;

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();

            SetSettings();

            if (PlayImmediately & Enabled & SoundListener.Instance != null)
                Play();

            base.Awake();
        }
        protected override void Shutdown()
        {
            Stop();

            base.Shutdown();
        }
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                effectInstance.Dispose();
                ((IDisposable)sound)?.Dispose();
            }

            effectInstance = null;
            sound = null;

            base.Dispose(disposing);
        }
        protected override void OnEnabledChanged()
        {
            if (effectInstance == null)
                return;

            if (Enabled & lastState == PlayState.Playing)
            {
                Play();
            }
            else
            {
                lastState = State;
                Pause();
            }

            base.OnEnabledChanged();
        }
        void IUpdateComponent.Update(GameTime gameTime)
        {
            if (!UseDistanceScaling)
                return;

            var listener = SoundListener.Instance;

            if (listener == null)
                return;

            //localVolume = listener.GetVolumeFromPosition(transformer.World.Position, this);
            localPan = listener.GetPanFromEmitter(transformer.World.Position, this);

            listener.GetVolumeFromPosition(transformer.World.Position, MinRadius, MaxRadius, out localVolume);

            localVolume = MathHelper.Clamp( localVolume, 0f, 1f);

            if (effectInstance == null)
                return;

            effectInstance.Volume = isMuted ? 0f : localVolume * volume;
            effectInstance.Pan = MathHelper.Clamp(pan + localPan, -1f, 1f);
        }

        private void SetSettings()
        {
            if (effectInstance == null)
                return;

            effectInstance.IsLooped = isLooped;
            effectInstance.Volume = isMuted ? 0f : localVolume * volume;
            effectInstance.Pan = MathHelper.Clamp(pan + localPan, -1f, 1f);
            effectInstance.Pitch = pitch;
        }

        public void Play()
        {
            effectInstance?.Play();
        }
        public void Pause()
        {
            effectInstance?.Pause();
        }
        public void Resume()
        {
            effectInstance?.Resume();
        }
        public void Stop()
        {
            effectInstance?.Stop();
        }
    }
}