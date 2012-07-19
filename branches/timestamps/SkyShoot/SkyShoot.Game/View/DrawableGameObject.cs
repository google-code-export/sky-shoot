using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Game.Game;
using SkyShoot.Game.Utils;

namespace SkyShoot.Game.View
{
	public class DrawableGameObject : AGameObject, IDrawable
	{
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

		protected Texture2D StaticTexture { get; set; }

		public Texture2D HealthTexture { get; private set; }

		public Vector2 HealthPosition;

		private int _healthTextureWidth;
		private readonly int _healthTextureHeight;

		private Color _healthTextureColor;
		private float _originalRadius;

		private const int FRAME_TIME = 500;
		private const bool LOOPING = true;

		public DrawableGameObject(AGameObject other, Animation2D animation)
		//: base(other)
		{
			SoundManager.Initialize();

			_healthTextureHeight = 5;

			Animation = animation;
			Animation.Initialize(FRAME_TIME, LOOPING);
			Copy(other);
			MakeOroginalRadius();
		}

		public DrawableGameObject(AGameObject other, Texture2D staticTexture)
		{
			_healthTextureHeight = 5;

			Animation = null;
			StaticTexture = staticTexture;
			Copy(other);
			MakeOroginalRadius();
		}

		void MakeOroginalRadius()
		{
			switch (ObjectType)
			{
				case EnumObjectType.PistolBullet:
					_originalRadius = Constants.DEFAULT_BULLET_RADIUS;
					break;
				case EnumObjectType.RocketBullet:
					_originalRadius = Constants.ROCKET_BULLET_RADIUS;
					break;
				case EnumObjectType.Flame:
					_originalRadius = Constants.FLAME_RADIUS;
					break;
				case EnumObjectType.Explosion:
					_originalRadius = Constants.EXPLOSION_RADIUS;
					break;
				case EnumObjectType.HeaterBullet:
					_originalRadius = Constants.DEFAULT_BULLET_RADIUS;
					break;
				case EnumObjectType.PoisonBullet:
					_originalRadius = Constants.POISON_BULLET_RADIUS;
					break;
				default:
					_originalRadius = Radius;
					break;
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			var rotation = (float)Math.Atan2(ShootVector.Y, ShootVector.X) + MathHelper.PiOver2;
			if (Is(EnumObjectType.Bonus))
				rotation = 0;

			if (Is(EnumObjectType.LivingObject))
			{
				if (HealthAmount >= 0.6f * MaxHealthAmount)
				{
					//if (Is(EnumObjectType.Player)) _soundManager.CueStop(SoundManager.SoundEnum.Heartbeat);
					_healthTextureColor = Color.Lime;
				}
				else if (HealthAmount >= 0.3f * MaxHealthAmount)
				{
					//if (Is(EnumObjectType.Player)) _soundManager.CueStop(SoundManager.SoundEnum.Heartbeat);
					_healthTextureColor = Color.Yellow;
				}
				else
				{
					//if (Is(EnumObjectType.Player)) _soundManager.SoundPlay(SoundManager.SoundEnum.Heartbeat);
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
				if (Is(EnumObjectType.Wall))
				{
					scale = Radius / (StaticTexture.Width / 2f);
				}
				if (Is(EnumObjectType.Bullet))
				{
					scale *= Radius / _originalRadius;
					if (Is(EnumObjectType.PistolBullet) || Is(EnumObjectType.HeaterBullet)) scale *= 2f;
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

			// todo remove this! 

			int milliseconds = gameTime.ElapsedGameTime.Milliseconds;
			Coordinates += RunVector * Speed * milliseconds;

			if (Is(EnumObjectType.Player))
			{
				Coordinates.X = MathHelper.Clamp(Coordinates.X, 0, GameLevel.Width);
				Coordinates.Y = MathHelper.Clamp(Coordinates.Y, 0, GameLevel.Height);
			}

			if (Is(EnumObjectType.Mob))
			{
				// todo //!! rewrite !!!!!!!!!!!!
				//Cue cue = _soundBank.GetCue("angry");
				//var random = new Random();
				//int k = random.Next(1000);
				//if (k <= 5) Scream(cue);
			}
		}
	}
}
