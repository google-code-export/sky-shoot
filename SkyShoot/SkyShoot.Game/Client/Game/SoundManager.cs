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
		public const int CueAmount = 11;
		public SoundEnum Sound;

		public enum SoundEnum
		{			
			Click,
			Desert,
			Grass,
			Gunshot,
			Laser,
			Lava,
			Lava2,
			MainTheme,
			Spider,					
			Snow,
			Sand
		}

		public static Cue[] sounds = new Cue [CueAmount];

		public static SoundManager Instance
		{
			get { return _instance; }
		}

		private SoundManager()
		{
			//Initialize();	
			LoadSounds();

			switch (Sound)
			{
				case SoundEnum.Click:
					break;
				case SoundEnum.Desert:
					break;
				case SoundEnum.Grass:
					break;
				case SoundEnum.Gunshot:
					break;
				case SoundEnum.Laser:
					break;
				case SoundEnum.Lava:
					break;
				case SoundEnum.Lava2:
					break;
				case SoundEnum.MainTheme:
					break;
				case SoundEnum.Sand:
					break;
				case SoundEnum.Snow:
					break;
				case SoundEnum.Spider:
					break;
			}
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

			sounds[0] = _soundBank.GetCue("RICOCHET");
			sounds[1] = _soundBank.GetCue("wind03");
			sounds[2] = _soundBank.GetCue("cricket00");
			sounds[3] = _soundBank.GetCue("GUNSHOT");
			sounds[4] = _soundBank.GetCue("LASER");
			sounds[5] = _soundBank.GetCue("lava_burn1");
			sounds[6] = _soundBank.GetCue("lava");
			sounds[7] = _soundBank.GetCue("STARWARS");
			sounds[8] = _soundBank.GetCue("wind03");
			sounds[9] = _soundBank.GetCue("wind01b");
			sounds[10] = _soundBank.GetCue("angry");
		}		

		public void SoundPlay(short cueNumber)
		{
			//Cue cue = _soundBank.GetCue(songName);
			sounds[cueNumber].Play();
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
