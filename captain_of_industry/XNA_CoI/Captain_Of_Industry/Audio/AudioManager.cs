//audio implementation raised from: https://audiomanagerxna.codeplex.com/SourceControl/latest#AudioManager/AudioManager/Audio/AudioManager.cs

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Captain_Of_Industry
{
    class AudioManager
    {
        private const int MAXSOUNDS = 10;

        private ContentManager content;

        private Dictionary<string, Song> songs = new Dictionary<string, Song>();
        private Dictionary<string, SoundEffect> sfx = new Dictionary<string, SoundEffect>();

        private Song currentSong;
        private SoundEffectInstance[] currentSFX = new SoundEffectInstance[MAXSOUNDS];

        //the volume at which music plays.
        public float MusicVolume
        {
            get
            { return MediaPlayer.Volume; }
            set
            { MediaPlayer.Volume = value; }
        }
        
        //the volume at which sfx play.
        public float SoundVolume
        {
            get
            { return SoundEffect.MasterVolume; }
            set
            { SoundEffect.MasterVolume = value; }
        }

        //checks to see if music/sounds are playing.
        public bool IsMusicActive
        {
            get
            { return (currentSong != null && MediaPlayer.State != MediaState.Stopped); }
        }

        //allows musics to be paused and resume.
        private bool isMusicPaused; 
        public bool IsMusicPaused
        {
            get
            { return (currentSong != null && MediaPlayer.State != MediaState.Stopped); }
        }

        private bool isMusicEnabled;
        public bool EnableMusic
        {
            get
            { return isMusicEnabled; }
            set
            {
                isMusicEnabled = value;
                if (!isMusicEnabled)
                    StopMusic();
            }
        }

        private bool areSoundsEnabled;
        public bool EnableSounds
        {
            get
            { return areSoundsEnabled; }
            set
            {
                areSoundsEnabled = value;
                if (!areSoundsEnabled)
                    StopAllSounds();
            }
        }

        #region constructor

        public AudioManager(ContentManager _content) 
        {
            content = _content;
            
            //if the game has control of the system, then music is enabled. Otherwise, nah.
            isMusicEnabled = MediaPlayer.GameHasControl;
            areSoundsEnabled = true;

            isMusicPaused = false;
            currentSong = null;
        }

        #endregion

        #region Content Loading/Unloading
        //loads content into the content manager.
        //public void LoadContent(ContentManager _manager) //KW --> option for loading content.
        public void LoadContent()
        {
           //put loading here.
           LoadMusic("bgAudio", "Audio Assets/wakeup_bg_music");
           // LoadSound("xxx", "Audio Assets/explosion");
        }
        
        //loads a specific song.
        void LoadMusic(string _name, string _path)
        {
            //if the song is not already in the dictionary, add it in.
            if (!songs.ContainsKey(_name))
            {
                songs.Add(_name, content.Load<Song>(_path));
            }
        }

        //loads 
        void LoadSound(string _name, string _path)
        {
            if (!sfx.ContainsKey(_name))
            {
                sfx.Add(_name, content.Load<SoundEffect>(_path));
            }
        }
        
        //releases content from the manager. 
        void UnloadContent()
        {
            StopMusic();
            StopAllSounds();
            content.Unload();
        }

        #endregion

        #region playback control.

        public void PlayMusic(string _name, bool _isLooping)
        {
            if (isMusicEnabled == false)
                if (currentSong != null)
                    if (currentSong.Name == _name)
                        return;

            //debug statement.
            if (!songs.TryGetValue(_name, out currentSong))
                throw new ArgumentException(string.Format("Could not find song."));

            if (currentSong != null)
                MediaPlayer.Stop();

            isMusicPaused = false;

            MediaPlayer.IsRepeating = _isLooping;
            MediaPlayer.Play(currentSong);

        }

        void PauseMusic()
        {
            if (currentSong != null && isMusicPaused == false)
            {
                MediaPlayer.Pause();
                isMusicPaused = true;
            }
        }

        void ContinueMusic()
        {
            if (currentSong != null && isMusicPaused == true)
            {
                MediaPlayer.Resume();
                isMusicPaused = false;
            }
        }

        void StopMusic()
        {
            if (currentSong != null && MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                isMusicPaused = false;
            }
        }

        SoundEffectInstance PlaySound(string _name, float _vol, float _pitch, float _pan)
        {
            if (areSoundsEnabled)
            {
                SoundEffect temp;

                if (!sfx.TryGetValue(_name, out temp))
                    throw new ArgumentException(string.Format("Cannot find sound."));

                int index = GetAvailableSoundIndex();

                if (index != -1)
                {
                    currentSFX[index] = temp.CreateInstance();
                    currentSFX[index].Volume = _vol;
                    currentSFX[index].Pitch = _pitch;
                    currentSFX[index].Pan = _pan;
                    currentSFX[index].Play();

                    return currentSFX[index];
                }
            }

            return null;
        }

        //returns a spot in currentSFX for a new song. 
        private int GetAvailableSoundIndex()
        {
            for (int i = 0; i < currentSFX.Length; ++i)
            {
                if (currentSFX[i] == null)
                    return i;
            }

            return -1;
        }

        void StopAllSounds()
        {
            for (int i = 0; i < currentSFX.Length; ++i)
            {
                currentSFX[i].Stop();
                currentSFX[i].Dispose();
                currentSFX[i] = null;
            }
        }

        #endregion

        //update mostly checks to see if any audio assets need to be removed from the 'active' playback parts.
        public void Update(GameTime _dt)
        {
            //remove the current sounds if they're not playing.
            for (int i = 0; i < currentSFX.Length; ++i)
            {
                if (currentSFX[i] != null && currentSFX[i].State == SoundState.Stopped)
                {
                    currentSFX[i].Dispose();
                    currentSFX[i] = null;
                }
            }

            //remove the song if it's not playing.
            if (currentSong != null && MediaPlayer.State == MediaState.Stopped)
            {
                currentSong = null;
                isMusicPaused = false;
            }
        }

    }
}

//KW
