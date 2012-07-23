using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace SkyShoot.Game.Game
{
	class SoundManager
	{
		public const int CUE_AMOUNT = 11;
		private static SoundManager _instance;
		private AudioEngine _engine;
		private SoundBank _soundBank;

		private WaveBank _waveBank;

		public enum SoundEnum
		{
			Click,
			Desert,
			DeadSpider,
			Grass,
			Gunshot,
			Heartbeat,
			Laser,
			Lava,
			Lava2,
			MainTheme,
			Spider,
			Snow,
			Sand
		}

		public static Dictionary<SoundEnum, Cue> Sounds = new Dictionary<SoundEnum, Cue>();

		public static SoundManager Instance
		{
			get { return _instance ?? (_instance = new SoundManager()); }
		}

		public SoundManager()
		{
			LoadSounds();
		}

		public void LoadSounds()
		{
			_engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			_soundBank = new SoundBank(_engine, "Content\\Sounds\\Sound Bank.xsb");
			_waveBank = new WaveBank(_engine, "Content\\Sounds\\Wave Bank.xwb");
			_engine.GetCategory("Music");

			Sounds.Add(SoundEnum.Click, _soundBank.GetCue("RICOCHET"));
			Sounds.Add(SoundEnum.Desert, _soundBank.GetCue("wind03"));
			Sounds.Add(SoundEnum.DeadSpider, _soundBank.GetCue("guts04a"));
			Sounds.Add(SoundEnum.Grass, _soundBank.GetCue("cricket00"));
			Sounds.Add(SoundEnum.Gunshot, _soundBank.GetCue("GUNSHOT"));
			Sounds.Add(SoundEnum.Heartbeat, _soundBank.GetCue("heartbeat"));
			Sounds.Add(SoundEnum.Laser, _soundBank.GetCue("LASER"));
			Sounds.Add(SoundEnum.Lava, _soundBank.GetCue("lava_burn1"));
			Sounds.Add(SoundEnum.Lava2, _soundBank.GetCue("lava"));
			Sounds.Add(SoundEnum.MainTheme, _soundBank.GetCue("STARWARS"));
			Sounds.Add(SoundEnum.Sand, _soundBank.GetCue("wind03"));
			Sounds.Add(SoundEnum.Snow, _soundBank.GetCue("wind01b"));
			Sounds.Add(SoundEnum.Spider, _soundBank.GetCue("angry"));
		}

		public void SoundPlay(SoundEnum sound)
		{
			switch (sound)
			{
				case SoundEnum.Click:
					Sounds[SoundEnum.Click] = _soundBank.GetCue("RICOCHET");
					Sounds[SoundEnum.Click].Play();
					break;
				case SoundEnum.Desert:
					Sounds[SoundEnum.Desert] = _soundBank.GetCue("wind03");
					Sounds[SoundEnum.Desert].Play();
					break;
				case SoundEnum.DeadSpider:
					Sounds[SoundEnum.Desert] = _soundBank.GetCue("guts04a");
					Sounds[SoundEnum.Desert].Play();
					break;
				case SoundEnum.Grass:
					Sounds[SoundEnum.Grass] = _soundBank.GetCue("cricket00");
					Sounds[SoundEnum.Grass].Play();
					break;
				case SoundEnum.Gunshot:
					Sounds[SoundEnum.Gunshot] = _soundBank.GetCue("GUNSHOT");
					Sounds[SoundEnum.Gunshot].Play();
					break;
				case SoundEnum.Heartbeat:
					Sounds[SoundEnum.Gunshot] = _soundBank.GetCue("heartbeat");
					Sounds[SoundEnum.Gunshot].Play();
					break;
				case SoundEnum.Laser:
					Sounds[SoundEnum.Gunshot] = _soundBank.GetCue("LASER");
					Sounds[SoundEnum.Laser].Play();
					break;
				case SoundEnum.Lava:
					Sounds[SoundEnum.Lava] = _soundBank.GetCue("lava_burn1");
					Sounds[SoundEnum.Lava].Play();
					break;
				case SoundEnum.Lava2:
					Sounds[SoundEnum.Lava2] = _soundBank.GetCue("lava");
					Sounds[SoundEnum.Lava2].Play();
					break;
				case SoundEnum.MainTheme:
					Sounds[SoundEnum.MainTheme] = _soundBank.GetCue("STARWARS");
					Sounds[SoundEnum.MainTheme].Play();
					break;
				case SoundEnum.Sand:
					Sounds[SoundEnum.Sand] = _soundBank.GetCue("wind03");
					Sounds[SoundEnum.Sand].Play();
					break;
				case SoundEnum.Snow:
					Sounds[SoundEnum.Snow] = _soundBank.GetCue("wind01b");
					Sounds[SoundEnum.Snow].Play();
					break;
				case SoundEnum.Spider:
					Sounds[SoundEnum.Spider] = _soundBank.GetCue("angry");
					Sounds[SoundEnum.Spider].Play();
					break;
				default:
					Sounds[SoundEnum.MainTheme] = _soundBank.GetCue("STARWARS");
					Sounds[SoundEnum.MainTheme].Play();
					break;
			}

			//sounds[Sound].Play();
		}

		public void CuePause(SoundEnum sound)
		{
			Sounds[sound].Pause();
		}

		public void CueResume(SoundEnum sound)
		{
			Sounds[sound].Resume();
		}

		public void CueStop(SoundEnum sound)
		{
			Sounds[sound].Stop(AudioStopOptions.AsAuthored);
		}
	}
}
