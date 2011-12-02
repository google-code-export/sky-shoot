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
        public Texture2D Texture { get; set; }

		public Texture2D healthRect { get; private set; }

		public Vector2 healthPosition;

		Color c;

        public Mob(Contracts.Mobs.AMob other, Texture2D texture) : base(other)
        {
            Texture = texture;			
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var rotation = (float)Math.Atan2(ShootVector.Y, ShootVector.X) + MathHelper.PiOver2;

			if (HealthAmount >= 0.6f * MaxHealthAmount) c = Color.Lime;
			else if (HealthAmount >= 0.3f * MaxHealthAmount) c = Color.Yellow;
			else c = Color.Red;

			healthPosition.X = Coordinates.X - 28;
			healthPosition.Y = Coordinates.Y - 45;

			healthRect = SkyShoot.Game.Client.View.Textures.HealthRect(5, (int) (0.5f*HealthAmount), c);

			spriteBatch.Draw(healthRect, healthPosition, null, Color.White);

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

        public virtual void Update(GameTime gameTime)
        {
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
