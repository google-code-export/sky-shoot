using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace SkyShoot.Game.Client.Game
{
	class SoundManager
	{
		public AudioEngine _engine;
		public SoundBank _soundBank;
		public WaveBank _waveBank;
		public AudioCategory _musicCategory;
		private static SoundManager _instance;	

		public static SoundManager Instance
		{
			get { return _instance; }
		}

		private SoundManager()
		{
			//Initialize();	
			LoadSounds();
				
		}

		public static void Initialize()
		{				
				if (_instance == null)
					_instance = new SoundManager();
				//else
				//{
				//  throw new Exception("Already initialized");
				//}
		}

		public void LoadSounds()
		{
			_engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			_soundBank = new SoundBank(_engine, "Content\\Sounds\\Sound Bank.xsb");
			_waveBank = new WaveBank(_engine, "Content\\Sounds\\Wave Bank.xwb");
			_musicCategory = _engine.GetCategory("Music");
		}		

		public void SoundPlay(string songName)
		{
			Cue cue = _soundBank.GetCue(songName);
			cue.Play();
		}

		public void CuePause(string songName)
		{
			Cue cue = _soundBank.GetCue(songName);
			cue.Pause();
		}

		public void CueResume(string songName)
		{
			Cue cue = _soundBank.GetCue(songName);
			cue.Resume();
		}

		public void CueStop(string songName)
		{
			Cue cue = _soundBank.GetCue(songName);
			cue.Stop(AudioStopOptions.AsAuthored);
		}
	}
}
