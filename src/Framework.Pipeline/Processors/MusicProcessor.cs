using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Audio;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace ZZZ.KNI.Content.Pipeline.Processors
{
    [ContentProcessor(DisplayName = "Music Processor - ZZZ")]
    public class MusicProcessor : ContentProcessor<AudioContent, MusicContent>
    {
        public ConversionQuality Quality
        {
            get
            {
                return songProcessor.Quality;
            }
            set
            {
                songProcessor.Quality = value;
            }
        }

        private SongProcessor  songProcessor = new SongProcessor();

        public override MusicContent Process(AudioContent input, ContentProcessorContext context)
        {
            return new MusicContent() { SongContent = songProcessor.Process(input, context) };
        }
    }
}
