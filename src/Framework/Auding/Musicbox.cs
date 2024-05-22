using Microsoft.Xna.Framework.Media;
using ZZZ.Framework.Auding.Assets;

namespace ZZZ.Framework.Auding
{
    public static class Musicbox
    {
        public static bool IsMuted
        {
            get => MediaPlayer.IsMuted;
            set => MediaPlayer.IsMuted = value;
        }
        public static bool IsRepeating
        {
            get => MediaPlayer.IsRepeating;
            set => MediaPlayer.IsRepeating = value;
        }
        public static bool IsShuffled
        {
            get => MediaPlayer.IsShuffled;
            set => MediaPlayer.IsShuffled = value;
        }
        public static float Volume
        {
            get => MediaPlayer.Volume;
            set => MediaPlayer.Volume = value;
        }
        public static TimeSpan PlayPosition
        {
            get => MediaPlayer.PlayPosition;
        }
        public static Music ActiveMusic
        {
            get
            {
                var song = MediaPlayer.Queue.ActiveSong;

                if(song == null) return null;

                return lastPlaylist.Find(x=>x.Song == song);
            }
        }
        public static MusicboxState State
        {
            get
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Stopped:
                        return MusicboxState.Stopped;
                    case MediaState.Playing:
                        return MusicboxState.Playing;
                    case MediaState.Paused:
                        return MusicboxState.Paused;
                }

                return default(MusicboxState);
            }
        }

        public static event EventHandler<EventArgs> ActiveMusicChanged
        {
            add
            {
                MediaPlayer.ActiveSongChanged += value;
            }

            remove
            {
                MediaPlayer.ActiveSongChanged -= value;
            }
        }
        public static event EventHandler<EventArgs> MusicboxStateChanged
        {
            add
            {
                MediaPlayer.MediaStateChanged += value;
            }

            remove
            {
                MediaPlayer.MediaStateChanged += value;
            }
        }

        private static List<Music> lastPlaylist = new List<Music>();

        public static void Play(Music music)
        {
            lastPlaylist.Clear();
            lastPlaylist.Add(music);

            MediaPlayer.Play(music.Song);
        }
        public static void Play(Musicplaylist playlist)
        {
            lastPlaylist.Clear();

            SongCollection songsCollection = Activator.CreateInstance(typeof(SongCollection), true) as SongCollection;

            foreach (var item in playlist.Musics)
            {
                lastPlaylist.Add(item);
                songsCollection.Add(item.Song);
            }

            if (songsCollection.Count <= 0)
                return;

            MediaPlayer.Play(songsCollection);
        }
        public static void Stop()
        {
            lastPlaylist.Clear();

            MediaPlayer.Stop();
        }
        public static void Resume()
        {
            MediaPlayer.Resume();
        }        
        public static void MoveNext()
        {
            MediaPlayer.MoveNext();
        }
        public static void MovePrevious()
        {
            MediaPlayer.MovePrevious();
        }
    }
}
