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
		public AudioEngine engine;
		public SoundBank soundBank;
		public WaveBank waveBank;
		public AudioCategory musicCategory;
		private static SoundManager manager;

		public SoundManager()
		{
			Initialize();
		}

		public void Initialize()
		{
				engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
				soundBank = new SoundBank(engine, "Content\\Sounds\\Sound Bank.xsb");
				waveBank = new WaveBank(engine, "Content\\Sounds\\Wave Bank.xwb");
				musicCategory = engine.GetCategory("Music");
		}

		public static SoundManager Manager
		{
			get { return manager; }
		}

		public void SoundPlay(string songName)
		{
			Cue cue = soundBank.GetCue(songName);
			cue.Play();
		}

		public void CuePause(string songName)
		{
			Cue cue = soundBank.GetCue(songName);
			cue.Pause();
		}

		public void CueResume(string songName)
		{
			Cue cue = soundBank.GetCue(songName);
			cue.Resume();
		}

		public void CueStop(string songName)
		{
			Cue cue = soundBank.GetCue(songName);
			cue.Stop(AudioStopOptions.AsAuthored);
		}
	}
}
