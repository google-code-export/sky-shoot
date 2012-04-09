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

		public static Dictionary<SoundEnum, Cue> sounds = new Dictionary<SoundEnum, Cue>();
		//public static Cue[] sounds = new Cue [CueAmount];

		public static SoundManager Instance
		{
			get 
			{
				if (_instance == null)
					_instance = new SoundManager();
				return _instance; 
			}
		}

		public SoundManager()
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

			/*sounds[0] = _soundBank.GetCue("RICOCHET");
			sounds[1] = _soundBank.GetCue("wind03");
			sounds[2] = _soundBank.GetCue("cricket00");
			sounds[3] = _soundBank.GetCue("GUNSHOT");
			sounds[4] = _soundBank.GetCue("LASER");
			sounds[5] = _soundBank.GetCue("lava_burn1");
			sounds[6] = _soundBank.GetCue("lava");
			sounds[7] = _soundBank.GetCue("STARWARS");
			sounds[8] = _soundBank.GetCue("wind03");
			sounds[9] = _soundBank.GetCue("wind01b");
			sounds[10] = _soundBank.GetCue("angry");*/

			sounds.Add(SoundEnum.Click, _soundBank.GetCue("RICOCHET"));
			sounds.Add(SoundEnum.Desert, _soundBank.GetCue("wind03"));
			sounds.Add(SoundEnum.Grass, _soundBank.GetCue("cricket00"));
			sounds.Add(SoundEnum.Gunshot, _soundBank.GetCue("GUNSHOT"));
			sounds.Add(SoundEnum.Laser, _soundBank.GetCue("LASER"));
			sounds.Add(SoundEnum.Lava, _soundBank.GetCue("lava_burn1"));
			sounds.Add(SoundEnum.Lava2, _soundBank.GetCue("lava"));
			sounds.Add(SoundEnum.MainTheme, _soundBank.GetCue("STARWARS"));
			sounds.Add(SoundEnum.Sand, _soundBank.GetCue("wind03"));
			sounds.Add(SoundEnum.Snow, _soundBank.GetCue("wind01b"));
			sounds.Add(SoundEnum.Spider, _soundBank.GetCue("angry"));
		}

		/*private void RenewSound(SoundEnum sound)
		{
			sounds[SoundEnum.Click] = _soundBank.GetCue("RICOCHET");
			sounds[SoundEnum.Desert] = _soundBank.GetCue("wind03");
			sounds[SoundEnum.Grass] = _soundBank.GetCue("cricket00");
			sounds[SoundEnum.Gunshot] = _soundBank.GetCue("GUNSHOT");
			sounds[SoundEnum.Laser] = _soundBank.GetCue("LASER");
			sounds[SoundEnum.Lava] = _soundBank.GetCue("lava_burn1");
			sounds[SoundEnum.Lava2] = _soundBank.GetCue("lava");
			sounds[SoundEnum.MainTheme] = _soundBank.GetCue("STARWARS");
			sounds[SoundEnum.Sand] = _soundBank.GetCue("wind03");
			sounds[SoundEnum.Snow] = _soundBank.GetCue("wind01b");
			sounds[SoundEnum.Spider] = _soundBank.GetCue("angry");
		}*/

		public void SoundPlay(SoundEnum Sound)
		{
			switch (Sound)
			{
				case SoundEnum.Click:
					sounds[SoundEnum.Click] = _soundBank.GetCue("RICOCHET");
					sounds[SoundEnum.Click].Play();
					break;
				case SoundEnum.Desert:
					sounds[SoundEnum.Desert] = _soundBank.GetCue("wind03");
					sounds[SoundEnum.Desert].Play();
					break;
				case SoundEnum.Grass:
					sounds[SoundEnum.Grass] = _soundBank.GetCue("cricket00");
					sounds[SoundEnum.Grass].Play();
					break;
				case SoundEnum.Gunshot:
					sounds[SoundEnum.Gunshot] = _soundBank.GetCue("GUNSHOT");
					sounds[SoundEnum.Gunshot].Play();
					break;
				case SoundEnum.Laser:
					sounds[SoundEnum.Gunshot] = _soundBank.GetCue("LASER");
					sounds[SoundEnum.Laser].Play();
					break;
				case SoundEnum.Lava:
					sounds[SoundEnum.Lava] = _soundBank.GetCue("lava_burn1");
					sounds[SoundEnum.Lava].Play();
					break;
				case SoundEnum.Lava2:
					sounds[SoundEnum.Lava2] = _soundBank.GetCue("lava");
					sounds[SoundEnum.Lava2].Play();
					break;
				case SoundEnum.MainTheme:
					sounds[SoundEnum.MainTheme] = _soundBank.GetCue("STARWARS");
					sounds[SoundEnum.MainTheme].Play();
					break;
				case SoundEnum.Sand:
					sounds[SoundEnum.Sand] = _soundBank.GetCue("wind03");
					sounds[SoundEnum.Sand].Play();
					break;
				case SoundEnum.Snow:
					sounds[SoundEnum.Snow] = _soundBank.GetCue("wind01b");
					sounds[SoundEnum.Snow].Play();
					break;
				case SoundEnum.Spider:
					sounds[SoundEnum.Spider] = _soundBank.GetCue("angry");
					sounds[SoundEnum.Spider].Play();
					break;
				default:
					sounds[SoundEnum.MainTheme] = _soundBank.GetCue("STARWARS");
					sounds[SoundEnum.MainTheme].Play();
					break;
			}

			//sounds[Sound].Play();
		}

		public void CuePause(SoundEnum Sound)
		{
			sounds[Sound].Pause();
		}

		public void CueResume(SoundEnum Sound)
		{
			sounds[Sound].Resume();
		}

		public void CueStop(SoundEnum Sound)
		{
			sounds[Sound].Stop(AudioStopOptions.AsAuthored);
		}
	}
}
