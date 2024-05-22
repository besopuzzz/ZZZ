using ZZZ.Framework.Assets;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Animations.Assets
{
    /// <summary>
    /// Предоставляет класс анимации. Данный класс является ассетом.
    /// </summary>
    public class Animation : Asset
    {
        /// <summary>
        /// Коллекция кадров, где ключ <see cref="float"/> является якорем на временной шкале.
        /// </summary>
        [ContentSerializer(CollectionItemName = "Frame", Optional = true)]
        public Dictionary<float, Sprite> Frames => frames;

        /// <summary>
        /// Общая продолжительность анимации. 
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Duration { get; set; }

        private Dictionary<float, Sprite> frames = new Dictionary<float, Sprite>();

        /// <summary>
        /// Предоставляет экземпляр анимации без кадров. Только для десериализации.
        /// </summary>
        internal Animation()
        {

        }

        /// <summary>
        /// Предоставляет экземпляр анимации с указанными кадрами.
        /// </summary>
        /// <param name="frames">Коллекция кадры</param>
        public Animation(Dictionary<float, Sprite> frames)
        {
            this.frames = frames;
        }

        /// <summary>
        /// Предоставляет экземпляр анимации с указанами параметрами для постройки кадров.
        /// </summary>
        /// <param name="startTime">Время начала первого кадра.</param>
        /// <param name="durationOne">Продолжительность одного кадра.</param>
        /// <param name="frames">Коллекция спрайтов.</param>
        public Animation(float startTime = 0f, float durationOne = 1f, params Sprite[] frames)
        {
            Duration = startTime + durationOne * frames.Length;

            foreach (var item in frames)
            {
                Frames.Add(startTime, item);
                startTime += durationOne;
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in frames.Values)
                {
                    ((IDisposable)item)?.Dispose();
                }
            }

            frames = null;

            base.Dispose(disposing);
        }
    }
}
