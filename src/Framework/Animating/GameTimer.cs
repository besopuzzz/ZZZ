namespace ZZZ.Framework.Animations
{
    public sealed class GameTimer
    {
        public float Tick { get; set; } = 0.01f;
        public float MaxTime { get; set; } = 10f;
        public float CurrentTime => currentTime;
        public bool Stoped => ended;
        public bool IsLooped { get; set; }
        public float[] Anchors { get; set; }
        public bool IsPaused { get; set; } 

        public event EventHandler TimerEnded;
        public event EventHandler<GameTimer, float> AnchorIvoked;

        private float currentTime = 0f;
        private float elapsedTime = 0f;
        private bool ended = false;
        private int index = 0;
        private float anchor = -1f;

        public GameTimer()
        {

        }

        public void Reset()
        {
            elapsedTime = 0f;
            currentTime = 0f;
            ended = false;

            if (Anchors != null)
            {
                index = 0;
                anchor = Anchors.OrderBy(x=>x).ToArray()[index];
            }
        }

        public void Update(float delta)
        {
            if (IsPaused)
                return;

            if (ended)
                return;

            elapsedTime += delta;

            if (elapsedTime >= Tick)
            {
                currentTime += Tick;
                elapsedTime = 0f;

                if (CurrentTime >= MaxTime)
                {
                    if(IsLooped)
                    {
                        Reset();
                    }
                    else
                    {
                        ended = true;
                        TimerEnded?.Invoke(this, EventArgs.Empty);
                    }
                }

                if(Anchors != null)
                {
                    if (index >= Anchors.Length)
                        return;

                    if (CurrentTime >= anchor)
                    {
                        AnchorIvoked?.Invoke(this, anchor);
                        index++;

                        if (index < Anchors.Length)
                            anchor = Anchors[index];
                    }
                }
            }

        }
    }
}
