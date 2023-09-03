using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ZZZ.Framework.Monogame.Auding.Assets;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Monogame.Auding.Components
{
    public class SoundEmitter : UpdateComponent, ISoundEmitter
    {
        public Sound Sound
        {
            get => sound;
            set
            {
                if(sound == value)
                    return;

                PlayState state =  PlayState.Stopped;

                if (value == null)
                {
                    Stop();
                    state = State;
                }
                sound = value;
                effectInstance?.Dispose();

                if (sound != null)
                {
                    effectInstance = sound.SoundEffect.CreateInstance();

                    if (state == PlayState.Playing)
                        Play();
                }
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

                Apply();
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

                Apply();
            }
        }
        public float Volume
        {
            get => volume;
            set
            {
                if (volume == value)
                    return;

                volume = value;

                if (volume > 1)
                    volume = 1;

                if (volume < 0)
                    volume = 0;

                Apply();
            }
        }
        public float Pan
        {
            get => pan;
            set
            {
                if (pan == value)
                    return;

                pan = value;

                if (value > 1)
                    pan = 1;

                if (value < -1)
                    pan = -1;

                Apply();
            }
        }
        public float Pitch
        {
            get => pitch;
            set
            {
                if (pitch == value)
                    return;

                pitch = value;

                if (value > 1)
                    pitch = 1;

                if (value < -1)
                    pitch = -1;

                Apply();
            }
        }
        public bool PlayAwake { get; set; } = true;
        public float MaxRadius { get; set; } = 300f;
        public float MinRadius { get; set; } = 30f;
        public  PlayState State
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

        public event SoundEvent PlayStarted;
        public event SoundEvent PlayPaused;
        public event SoundEvent PlayResumed;
        public event SoundEvent PlayStopped;

        ISoundListener ISoundEmitter.Listener
        {
            get => listener;
            set
            {
                if (listener == value)
                    return;

                listener = value;
                CalculateAndSetVolume();
            }
        }

        private ISoundListener listener;
        private Sound sound;
        private SoundEffectInstance effectInstance;
        private bool isLooped = false;
        private bool isMuted = false;
        private float volume = 1f;
        private float pan = 0f;
        private float pitch = 0f;
        private float localVolume = 1f;
        private float localPan = 0f;
        private PlayState lastState = PlayState.Stopped;
        private Transformer transformer;

        protected override void Startup()
        {
            RegistrationComponent<ISoundComponent>(this);

            transformer = GetComponent<Transformer>();
            transformer.WorldChanged += Transformer_WorldChanged;

            if (PlayAwake & Enabled)
                Play();

            base.Startup();
        }

        private void Transformer_WorldChanged(ITransformer sender, Transform2D args)
        {
            CalculateAndSetVolume();
        }

        protected override void Shutdown()
        {
            Stop();
            UnregistrationComponent<ISoundComponent>(this);
            transformer.WorldChanged -= Transformer_WorldChanged;

            base.Shutdown();
        }
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                effectInstance.Dispose();
                sound.Dispose();
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
        protected virtual void ApplyDistancing(out float volume, out float pan)
        {
            pan = 0f;
            volume = 1f;

            if (listener?.Owner == null)
                return;

            if (!listener.Enabled)
            {
                volume = 0f;

                return;
            }

            Transformer listenerTransformer = listener.Owner.GetComponent<Transformer>();
            float distance = Vector2.Distance(transformer.World.Position, listenerTransformer.World.Position);


            if (distance <= MaxRadius)
            {
                if (distance > MinRadius)
                {
                    float ratio = (distance / MaxRadius);

                    volume = 1 - ratio;

                    var differenceX = (transformer.World.Position - listenerTransformer.World.Position).X;

                    if (differenceX != 0)
                    {
                        var differenceAbs = Math.Abs(differenceX);

                        if (differenceAbs >= MinRadius / 2)
                            pan = ratio * (differenceX / differenceAbs);
                    }

                }
            }
            else volume = 0f;
        }
        protected override void Update(GameTime gameTime)
        {
            if(lastState == PlayState.Playing & State == PlayState.Stopped)
            {
                PlayStopped?.Invoke(this, sound);
            }
            else
            {
                if(lastState == PlayState.Playing & State == PlayState.Paused)
                {
                    PlayPaused?.Invoke(this, sound);
                }
            }

            lastState = State;

            base.Update(gameTime);
        }

        void ISoundEmitter.ApplyDistancing()
        {
            ApplyDistancing(out var volume, out var pan);

            localVolume = volume;
            localPan = pan;

            Apply();
        }

        private void CalculateAndSetVolume()
        {
            ((ISoundEmitter)this).ApplyDistancing();
        }
        private void Apply()
        {
            if (effectInstance == null)
                return;

            effectInstance.IsLooped = isLooped;

            if(isMuted)
            {
                effectInstance.Volume = 0f;
                effectInstance.Pan = 0f;
                effectInstance.Pitch = 0f;
            }
            else
            {
                effectInstance.Volume = localVolume * volume;
                effectInstance.Pan = MathHelper.Clamp(pan + localPan, -1f, 1f);
                effectInstance.Pitch = pitch;
            }
        }

        public void Play()
        {
            if (State == PlayState.Playing)
            {
                Stop();
                Play();

                return;
            }

            if (listener?.Owner == null)
                return;

            CalculateAndSetVolume();
            Apply();
            effectInstance?.Play();
            PlayStarted?.Invoke(this, sound);
        }
        public void Pause()
        {
            if (State != PlayState.Playing)
                return;

            effectInstance?.Pause();
            PlayPaused?.Invoke(this, sound);
        }
        public void Resume()
        {
            if (State != PlayState.Paused)
                return;

            CalculateAndSetVolume();
            Apply();
            effectInstance?.Resume();
            PlayResumed?.Invoke(this, sound);
        }
        public void Stop()
        {
            if (State != PlayState.Playing)
                return;

            effectInstance?.Stop();
            PlayStopped?.Invoke(this, sound);
        }
    }
}