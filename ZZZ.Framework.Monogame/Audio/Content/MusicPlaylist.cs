namespace ZZZ.Framework.Monogame.Audio.Content
{
    public sealed class MusicPlaylist : List<Music>
    {
        public bool IsRepeat { get; set; }
        public bool IsRandom { get; set; }

        internal MusicPlaylist()
        {

        }

        public MusicPlaylist(List<Music> musics)
        {
            AddRange(musics);
        }

    }
}
