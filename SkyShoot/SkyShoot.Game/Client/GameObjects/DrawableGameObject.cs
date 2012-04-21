using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Contracts.Mobs;

using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Client.View;

using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.GameObjects
{
	public class DrawableGameObject : AGameObject, IDrawable
	{
		private readonly AudioEngine _engine;
		private readonly SoundBank _soundBank;
		private WaveBank _waveBank;

		public Vector2 CoordinatesM
		{
			get { return TypeConverter.XnaLite2Xna(Coordinates); }
			set { Coordinates = TypeConverter.Xna2XnaLite(value); }
		}

		public Vector2 RunVectorM
		{
			get { return TypeConverter.XnaLite2Xna(RunVector); }
			set { RunVector = TypeConverter.Xna2XnaLite(value); }
		}

		public Vector2 ShootVectorM
		{
			get { return TypeConverter.XnaLite2Xna(ShootVector); }
			set { ShootVector = TypeConverter.Xna2XnaLite(value); }
		}

		public Animation2D Animation { get; set; }
		protected Texture2D StaticTexture
		{
			get;
			set;
		}


		public Texture2D HealthTexture { get; private set; }

		public Vector2 HealthPosition;

		private int _healthTextureWidth;
		private readonly int _healthTextureHeight;

		private Color _healthTextureColor;
		private float OriginalRadius;

		private const int FrameTime = 500;
		private const bool Looping = true;

		public DrawableGameObject(AGameObject other, Animation2D animation)
		//: base(other)
		{
			_engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			_soundBank = new SoundBank(_engine, "Content\\Sounds\\Sound Bank.xsb");
			_waveBank = new WaveBank(_engine, "Content\\Sounds\\Wave Bank.xwb");

			_healthTextureHeight = 5;

			Animation = animation;
			Animation.Initialize(FrameTime, Looping);
			Copy(other);
			OriginalRadius = Radius;
		}

		public DrawableGameObject(AGameObject other, Texture2D staticTexture)
		{
			_engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			_soundBank = new SoundBank(_engine, "Content\\Sounds\\Sound Bank.xsb");
			_waveBank = new WaveBank(_engine, "Content\\Sounds\\Wave Bank.xwb");

			_healthTextureHeight = 5;

			Animation = null;
			StaticTexture = staticTexture;
			Copy(other);
			OriginalRadius = Radius;
		}

		public static void Stop(Cue cue)
		{
			cue.Stop(AudioStopOptions.Immediate);
		}

		public static void Scream(Cue cue)
		{
			cue.Play();
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			var rotation = (float)Math.Atan2(ShootVector.Y, ShootVector.X) + MathHelper.PiOver2;

			//Cue healthCue = soundBank.GetCue("heartbeat");
			if (Is(EnumObjectType.LivingObject))
			{

				if (HealthAmount >= 0.6f * MaxHealthAmount)
				{
					//Stop(healthCue);
					_healthTextureColor = Color.Lime;
				}
				else if (HealthAmount >= 0.3f * MaxHealthAmount)
				{
					//Stop(healthCue);
					_healthTextureColor = Color.Yellow;
				}
				else
				{
					/*if (!healthCue.IsPlaying)
					{
						healthCue.Play();
					}*/
					_healthTextureColor = Color.Red;
				}

				HealthPosition.X = Coordinates.X - 28;
				HealthPosition.Y = Coordinates.Y - 45;

				_healthTextureWidth = (int)(50f * HealthAmount / MaxHealthAmount);

				if (_healthTextureWidth > 0)
				{
					HealthTexture = Textures.HealthRect(_healthTextureWidth, _healthTextureHeight, _healthTextureColor);
					spriteBatch.Draw(HealthTexture, HealthPosition, null, Color.White);
				}
			}

			float scale = 0.5f;

			if (Animation != null)
			{
				spriteBatch.Draw(Animation.CurrentTexture,
												 CoordinatesM,
												 null,
												 Color.White,
												 rotation,
												 new Vector2(Animation.CurrentTexture.Width / 2f, Animation.CurrentTexture.Height / 2f),
												 scale,
												 SpriteEffects.None,
												 0);
			}
			else
			{
				if(Is(EnumObjectType.Wall))
				{
					scale = Radius / (StaticTexture.Width/2f);
				}
				if(Is(EnumObjectType.Bullet))
				{
					scale = Radius/OriginalRadius;
				}
				spriteBatch.Draw(StaticTexture,
												 CoordinatesM,
												 null,
												 Color.White,
												 rotation,
												 new Vector2(StaticTexture.Width / 2f, StaticTexture.Height / 2f),
												 scale,
												 SpriteEffects.None,
												 0);

			}
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!RunVector.Equals(Vector2.Zero) && Animation != null)
			{
				Animation.Update(gameTime);
			}

			int milliseconds = gameTime.ElapsedGameTime.Milliseconds;
			Coordinates += RunVector * Speed * milliseconds;

			if (Is(EnumObjectType.Player))
			{
				Coordinates.X = MathHelper.Clamp(Coordinates.X, 0, GameLevel.Width);
				Coordinates.Y = MathHelper.Clamp(Coordinates.Y, 0, GameLevel.Height);
			}

			if (Is(EnumObjectType.Mob))
			{
				//!! todo rewrite !!!!!!!!!!!!
				//Cue cue = _soundBank.GetCue("angry");
				//var random = new Random();
				//int k = random.Next(1000);
				//if (k <= 5) Scream(cue);
			}
		}
	}
}
