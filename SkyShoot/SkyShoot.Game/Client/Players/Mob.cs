using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Client.View;

using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Players
{
	public class Mob : Contracts.Mobs.AMob, IDrawable
	{
		public Animation2D Animation { get; set; }

		public Texture2D HealthTexture { get; private set; }

		public Vector2 HealthPosition;

		private int _healthTextureWidth;
		private readonly int _healthTextureHeight;

		private Color _healthTextureColor;

		private const int FrameTime = 500;
		private const bool Looping = true;

		public Mob(Contracts.Mobs.AMob other, Animation2D animation)
			: base(other)
		{
			_healthTextureHeight = 5;

			Animation = animation;
			Animation.Initialize(FrameTime, Looping);
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			var rotation = (float) Math.Atan2(ShootVector.Y, ShootVector.X) + MathHelper.PiOver2;

			if (HealthAmount >= 0.6f * MaxHealthAmount)
				_healthTextureColor = Color.Lime;
			else if (HealthAmount >= 0.3f * MaxHealthAmount)
				_healthTextureColor = Color.Yellow;
			else
				_healthTextureColor = Color.Red;

			HealthPosition.X = Coordinates.X - 28;
			HealthPosition.Y = Coordinates.Y - 45;

			_healthTextureWidth = (int) (50f * HealthAmount / MaxHealthAmount);

			if (_healthTextureWidth > 0)
			{
				HealthTexture = Textures.HealthRect(_healthTextureWidth, _healthTextureHeight, _healthTextureColor);
				spriteBatch.Draw(HealthTexture, HealthPosition, null, Color.White);
			}

			spriteBatch.Draw(Animation.CurrentTexture,
			                 Coordinates,
			                 null,
			                 Color.White,
			                 rotation,
			                 new Vector2(Animation.CurrentTexture.Width / 2f, Animation.CurrentTexture.Height / 2f),
			                 1,
			                 SpriteEffects.None,
			                 0);
		}

		public virtual void Update(GameTime gameTime)
		{
			if (!RunVector.Equals(Vector2.Zero))
				Animation.Update(gameTime);

			int milliseconds = gameTime.ElapsedGameTime.Milliseconds;
			Coordinates += RunVector * Speed * milliseconds;

			if (IsPlayer)
			{
				Coordinates.X = MathHelper.Clamp(Coordinates.X, 0, GameLevel.Width);
				Coordinates.Y = MathHelper.Clamp(Coordinates.Y, 0, GameLevel.Height);
			}
		}
	}
}
