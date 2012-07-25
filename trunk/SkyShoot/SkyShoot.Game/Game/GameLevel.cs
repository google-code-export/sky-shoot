using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.View;

namespace SkyShoot.Game.Game
{
	public class GameLevel : Contracts.Session.GameLevel, View.IDrawable
	{
		private static Texture2D _texture;

		private readonly SoundManager _soundManager;

		public GameLevel(int width, int height, TileSet tileSet) : base(width, height, tileSet)
		{
			_soundManager = SoundManager.Instance;

			switch (tileSet)
			{
				case TileSet.Grass:
					_soundManager.SoundPlay(SoundManager.SoundEnum.Grass);
					_texture = Textures.Clone(Textures.GrassLandscape);
					break;
				case TileSet.Desert:
					_soundManager.SoundPlay(SoundManager.SoundEnum.Desert);
					_texture = Textures.Clone(Textures.DesertLandscape);
					break;
				case TileSet.Sand:
					_soundManager.SoundPlay(SoundManager.SoundEnum.Desert);
					_texture = Textures.Clone(Textures.SandLandscape);
					break;
				case TileSet.Snow:
					_soundManager.SoundPlay(SoundManager.SoundEnum.Snow);
					_texture = Textures.Clone(Textures.SnowLandscape);
					break;
				case TileSet.Volcanic:
					_soundManager.SoundPlay(SoundManager.SoundEnum.Lava);
					_soundManager.SoundPlay(SoundManager.SoundEnum.Lava2);
					_texture = Textures.Clone(Textures.VolcanicLandscape);
					break;
			}
		}

		public void AddTexture(Texture2D texture, Vector2 position)
		{
			Textures.Merge(_texture, texture, position);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, Vector2.Zero, new Rectangle(0, 0, Width, Height), Color.White);
		}
	}
}
