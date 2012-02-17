using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Contracts.Weapon.Projectiles;

using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Client.View;

using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Weapon
{
	public class Projectile : AProjectile, IDrawable
	{
		public Texture2D Texture { get; private set; }

		public Boolean IsActive { get; private set; }

		public Projectile(AProjectile projectile) :
			base(projectile)
		{
			// todo
			switch (Type)
			{
				case EnumBulletType.Bullet:
					break;
				case EnumBulletType.Flame:
					break;
				case EnumBulletType.Rocket:
					break;
			}

			Texture = Textures.ProjectileTexture;

			IsActive = true;
		}

		public void Update(GameTime gameTime)
		{
			int milliseconds = gameTime.ElapsedGameTime.Milliseconds;

			Vector2 movement = Direction * Speed * milliseconds;

			Coordinates += movement;

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
			var rotation = (float) Math.Atan2(Direction.Y, Direction.X) + MathHelper.PiOver2;

			spriteBatch.Draw(Texture,
			                 Coordinates,
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
