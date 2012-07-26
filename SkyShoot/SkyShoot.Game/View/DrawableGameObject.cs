using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.GameObject;
using SkyShoot.Contracts.Service;
using SkyShoot.Game.Utils;

namespace SkyShoot.Game.View
{
	public class DrawableGameObject : AGameObject, IDrawable
	{
		private int _healthTextureWidth;

		private Color _healthTextureColor;
		private float _originalRadius;

		private const int FRAME_TIME = 500;
		private const bool LOOPING = true;

		public Animation2D Animation { get; set; }

		protected Texture2D StaticTexture { get; set; }

		public Texture2D HealthTexture { get; private set; }

		public Vector2 HealthPosition;

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

		public DrawableGameObject(AGameObject other, Animation2D animation)
		{
			Animation = animation;
			Animation.Initialize(FRAME_TIME, LOOPING);
			Copy(other);
			MakeOriginalRadius();
		}

		public DrawableGameObject(AGameObject other, Texture2D staticTexture)
		{
			Animation = null;
			StaticTexture = staticTexture;
			Copy(other);
			MakeOriginalRadius();
		}

		void MakeOriginalRadius()
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
					_healthTextureColor = Color.Lime;
				}
				else if (HealthAmount >= 0.3f * MaxHealthAmount)
				{
					_healthTextureColor = Color.Yellow;
				}
				else
				{
					_healthTextureColor = Color.Red;
				}

				HealthPosition.X = Coordinates.X - 28;
				HealthPosition.Y = Coordinates.Y - 45;

				_healthTextureWidth = (int)(50f * HealthAmount / MaxHealthAmount);

				if (_healthTextureWidth > 0)
				{
					HealthTexture = Textures.HealthRect(_healthTextureWidth, Constants.HEALTH_TEXTURE_HEIGHT, _healthTextureColor);
					spriteBatch.Draw(HealthTexture,
									 HealthPosition,
									 null,
									 Color.White,
									 0,
									 Vector2.Zero,
									 1,
									 SpriteEffects.None,
									 Constants.HEALTHBAR_TEXTURE_LAYER);
				}
			}

			float scale = 0.5f;

			float textureLayer = Constants.MOVING_GAME_OBJECTS_TEXTURE_LAYER;

			if (Is(EnumObjectType.Wall))
			{
				textureLayer = Constants.WALLS_TEXTURE_LAYER;
			}

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
								 textureLayer);
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
								 textureLayer);
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!RunVector.Equals(Vector2.Zero) && Animation != null)
			{
				Animation.Update(gameTime);
			}
		}
	}
}
