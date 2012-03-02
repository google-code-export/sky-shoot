using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Client.View;
using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Weapon
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

		AudioEngine engine;
		SoundBank soundBank;
		WaveBank waveBank;

		public Projectile(AProjectile projectile) :
			base(projectile)
		{
			engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			soundBank = new SoundBank(engine, "Content\\Sounds\\Sound Bank.xsb");
			waveBank = new WaveBank(engine, "Content\\Sounds\\Wave Bank.xwb");

			Cue cue = soundBank.GetCue("LASER");
			cue.Play();
			// todo
			switch (Type)
			{
				case EnumObjectType.Bullet:
					break;
				case EnumObjectType.Flame:
					break;
				case EnumObjectType.Rocket:
					break;
			}

			Texture = Textures.ProjectileTexture;

			IsActive = true;
		}

		public void Update(GameTime gameTime)
		{		
			int milliseconds = gameTime.ElapsedGameTime.Milliseconds;

			Vector2 movement = RunVectorM * Speed * milliseconds;

			CoordinatesM += movement;

			LifeDistance -= movement.Length();

			if (Coordinates.X < 0 || Coordinates.X >= GameLevel.Width)
				IsActive = false;
			if (Coordinates.Y < 0 || Coordinates.Y >= GameLevel.Height)
				IsActive = false;
			if (LifeDistance <= 0f)
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
