using Microsoft.Xna.Framework.Media;
using ZZZ.Framework.Monogame.Auding.Assets;

namespace ZZZ.Framework.Monogame.Auding.Components
{
    public static class Musicbox
    {
        public static event MusicboxEvent Playing;
        public static event MusicboxEvent Stopped;
        public static event MusicboxEvent Paused;
        public static event MusicboxEvent Resumed;

        public static event MusicboxEvent ActiveMusicChanged;

        public static PlayState State { get; private set; }
        public static bool IsRepeat { get; set; }

        private static MusicPlaylist lastPlaylist;
        private static Music lastMusic;
        private static int currentIndex = 0;
        private static bool[] played;
        private static Random random = new Random();

        static Musicbox()
        {
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            MediaPlayer.ActiveSongChanged += MediaPlayer_ActiveSongChanged;
        }

        private static void MediaPlayer_ActiveSongChanged(object sender, EventArgs e)
        {
            if (lastPlaylist != null)
                PlayNext(false);
        }

        private static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            switch (MediaPlayer.State)
            {
                case MediaState.Stopped:
                    State = PlayState.Stopped;
                    Stopped?.Invoke(lastMusic, currentIndex);
                    break;
                case MediaState.Playing:
                    State = PlayState.Playing;
                    Playing?.Invoke(lastMusic, currentIndex);
                    break;
                case MediaState.Paused:
                    State = PlayState.Paused;
                    Paused?.Invoke(lastMusic, currentIndex);
                    break;
                default:
                    break;
            }
        }

        private static void Play(int index)
        {
            MediaPlayer.Play(lastPlaylist[index].Song);
        }
        public static void Stop()
        {
            if (State == PlayState.Playing)
                MediaPlayer.Stop();
        }
        public static void Pause()
        {
            if (State == PlayState.Playing)
                MediaPlayer.Pause();
        }
        public static void Resume()
        {
            if (State == PlayState.Paused)
            {
                MediaPlayer.Resume();
            }
            else if (State == PlayState.Stopped & lastMusic != null)
                Play(lastMusic);
        }

        public static void Play(Music music)
        {
            Play(new MusicPlaylist() { music });
        }
        public static void Play(MusicPlaylist musics)
        {
            Stop();

            lastPlaylist = musics;
            Prepaire();

            Play(currentIndex);
        }

        public static void PlayNext(bool fromUser = false)
        {
            if (IsRepeat)
            {
                Play(lastPlaylist[currentIndex]);
                return;
            }

            if (lastPlaylist.IsRandom)
            {
                if (fromUser)
                    Prepaire();
                else played[currentIndex] = true;
            }


            Play(lastPlaylist[currentIndex]);
        }

        public static void PlayPrevious(bool fromUser = false)
        {
            currentIndex--;

            if (currentIndex < 0)
                currentIndex = 0;

            Play(lastPlaylist[currentIndex]);
        }

        private static int GetNext()
        {
            int nextIndex = 0;

            if (lastPlaylist.IsRandom)
                nextIndex = GetNextRandom();
            else
            {
                nextIndex = currentIndex + 1;

                if (nextIndex >= lastPlaylist.Count)
                    nextIndex = 0;
            }

            return nextIndex;
        }

        private static void CurrentPlayed()
        {
            played[currentIndex] = true;
        }
        private static void Prepaire()
        {
            currentIndex = 0;
            played = new bool[lastPlaylist.Count];

            for (int i = 0; i < lastPlaylist.Count; i++)
            {
                played[i] = false;
            }
        }

        private static int GetNextRandom()
        {
            List<int> notPlayed = new List<int>();

            for (int i = 0; i < played.Length; i++)
            {
                if (!played[i])
                    notPlayed.Add(i);
            }

            int index = random.Next(0, notPlayed.Count);

            return notPlayed[index];
        }
    }
}
