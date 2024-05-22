using ZZZ.Framework.Animations.Assets;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Animations
{
    /// <summary>
    /// Предоставляет класс плеера для вопроизведения анимации.
    /// </summary>
    public sealed class AnimationPlayer : IDisposable
    {
        /// <summary>
        /// Анимация для вопроизведения.
        /// </summary>
        public Animation Animation
        {
            get => animation;
            set
            {
                if (animation == value)
                    return;

                animation = value;
                Reset();
            }
        }

        /// <summary>
        /// Текущий кадр плеера.
        /// </summary>
        public Sprite CurrentSprite => currentSprite;

        /// <summary>
        /// Возвращает <see cref="true"/>, если анимации будет вопроизводиться повторно, иначе <see cref="false"/>.
        /// </summary>
        public bool IsLoop
        {
            get => timer.IsLooped;
            set
            {
                if (timer.IsLooped == value)
                    return;

                timer.IsLooped = value;
            }
        }

        /// <summary>
        /// Событие, когда поменялся текущий кадр плеера.
        /// </summary>
        public event EventHandler CurrentSpriteChanged;

        private Sprite currentSprite = null!;
        private GameTimer timer = null!;
        private Animation animation = null!;
        private bool disposedValue;
        private bool started = false;

        /// <summary>
        /// Предоставляет экземпляр плеера для воспроизведения анимации. Только для десериализации.
        /// </summary>
        internal AnimationPlayer()
        {
            timer = new GameTimer();
            timer.Tick = 0.01f;
        }

        /// <summary>
        /// Предоставляет экземпляр плеера для воспроизведения анимации.
        /// </summary>
        /// <param name="animation">Анимация, которую плеер будет воспроизводить.</param>
        public AnimationPlayer(Animation animation) : this()
        {
            Animation = animation;
        }

        /// <summary>
        /// Сбрасывает в начальное состояние.
        /// </summary>
        public void Reset()
        {
            timer.Anchors = animation.Frames.Keys.ToArray();
            timer.MaxTime = animation.Duration;
            timer.Reset();
        }

        /// <summary>
        /// Запускает воспроизведение. Используйте метод <see cref="Update(float)"/> для обновления внутренного счетчика плеера.
        /// </summary>
        public void Start()
        {
            Reset();
            timer.AnchorIvoked += Timer_AnchorIvoked;
            started = true;
        }

        /// <summary>
        /// Останавливает и сбрасывает плеер в начальное состояние.
        /// </summary>
        public void Stop()
        {
            if (!started)
                return;

            Reset();
            timer.AnchorIvoked -= Timer_AnchorIvoked;
        }

        /// <summary>
        /// Останавливает воспроизведение. Используйте <see cref="Resume"/> для продолжения.
        /// </summary>
        public void Pause() => timer.IsPaused = false;

        /// <summary>
        /// Продолжает воспроизведение, если была пауза.
        /// </summary>
        public void Resume() => timer.IsPaused = false;

        /// <summary>
        /// Обновляет внутренний счетчик плеера и увеличивает его текущее значение.
        /// </summary>
        /// <param name="delta">Зачение, на которое увеличится счетчик.</param>
        public void Update(float delta)
        {
            timer.Update(delta);
        }

        private void Timer_AnchorIvoked(object sender, float e)
        {
            currentSprite = Animation.Frames[e];
            CurrentSpriteChanged?.Invoke(this, EventArgs.Empty);
        }
        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                Stop();
                ((IDisposable)animation)?.Dispose();
            }

            animation = null!;
        }
        ~AnimationPlayer()
        {
            if (disposedValue)
                return;

            Dispose(disposing: false);
            disposedValue = true;
        }
        void IDisposable.Dispose()
        {
            if (disposedValue)
                return;

            Dispose(disposing: true);
            disposedValue = true;
            GC.SuppressFinalize(this);
        }
    }
}
