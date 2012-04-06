using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Client.View;

using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.GameObjects
{
	public class Projectile : AProjectile, IDrawable
	{
		public Texture2D Texture { get; private set; }

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

		private readonly AudioEngine _engine;
		private readonly SoundBank _soundBank;
		private WaveBank _waveBank;

		public Projectile(AGameObject projectile) :
			base(projectile)
		{
			_engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			_soundBank = new SoundBank(_engine, "Content\\Sounds\\Sound Bank.xsb");
			_waveBank = new WaveBank(_engine, "Content\\Sounds\\Wave Bank.xwb");

			ObjectType = EnumObjectType.LaserBullet;
			// todo
			switch (ObjectType)
			{
				case EnumObjectType.Bullet:
					break;
				case EnumObjectType.Flame:
					break;
				case EnumObjectType.Rocket:
					break;
				case EnumObjectType.LaserBullet:
					Cue laserCue = _soundBank.GetCue("LASER");
					laserCue.Play();
					break;
				case EnumObjectType.ShutgunBullet:
					Cue shutgunCue = _soundBank.GetCue("GUNSHOT");
					shutgunCue.Play();
					break;
			}

			Texture = Textures.ProjectileTexture;

			// todo temporary
			HealthAmount = 1000;

			IsActive = true;
		}

		public void Update(GameTime gameTime)
		{
			int milliseconds = gameTime.ElapsedGameTime.Milliseconds;

			Vector2 movement = RunVectorM * Speed * milliseconds;

			CoordinatesM += movement;

			HealthAmount -= movement.Length();

			if (Coordinates.X < 0 || Coordinates.X >= GameLevel.Width)
				IsActive = false;
			if (Coordinates.Y < 0 || Coordinates.Y >= GameLevel.Height)
				IsActive = false;
			if (HealthAmount <= 0f)
				IsActive = false;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			var rotation = (float) Math.Atan2(RunVector.Y, RunVector.X) + MathHelper.PiOver2;

			spriteBatch.Draw(Texture,
			                 TypeConverter.XnaLite2Xna(Coordinates),
			                 null,
			                 Color.White,
			                 rotation,
			                 new Vector2(Texture.Width / 2f, Texture.Height / 2f),
			                 1,
			                 SpriteEffects.None,
			                 0);
		}
	}
}
