using Microsoft.Xna.Framework.Content;

namespace ZZZ.KNI.Content.Pipeline
{
    public class MusicplaylistContent
    {
        [ContentSerializer(CollectionItemName = "Song")]
        public List<string> SongPaths { get; set; }
    }
}
