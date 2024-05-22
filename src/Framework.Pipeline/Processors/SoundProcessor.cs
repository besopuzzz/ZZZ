using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Audio;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace ZZZ.KNI.Content.Pipeline.Processors
{
    [ContentProcessor(DisplayName = "Sound Processor - ZZZ")]
    public class SoundProcessor : ContentProcessor<AudioContent, SoundContent>
    {
        public ConversionQuality Quality
        {
            get
            {
                return effectProcessor.Quality;
            }
            set
            {
                effectProcessor.Quality = value;
            }
        }

        private SoundEffectProcessor effectProcessor = new SoundEffectProcessor();

        public override SoundContent Process(AudioContent input, ContentProcessorContext context)
        {
            return new SoundContent() { SoundEffectContent = effectProcessor.Process(input, context) };
        }
    }
}
