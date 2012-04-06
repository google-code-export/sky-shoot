using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Contracts.Bonuses;
using SkyShoot.Contracts.Mobs;

using SkyShoot.Game.Client.View;

namespace SkyShoot.Game.Client.GameObjects
{
	public class GameBonus : AGameBonus, IDrawable
	{
		public GameBonus(AGameObject other)
			: base(other)
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			
		}
	}
}
