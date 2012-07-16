using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.Client.View;
using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Game
{
	public class GameLevel : IDrawable
	{
		public static int Width { get; private set; }

		public static int Height { get; private set; }

		private static Texture2D _texture;

		private readonly SoundManager _soundManager;

		public GameLevel(Contracts.Session.GameLevel gameLevel)
		{
			Width = (int)gameLevel.LevelWidth;
			Height = (int)gameLevel.LevelHeight;

			SoundManager.Initialize();
			_soundManager = SoundManager.Instance;

			switch (gameLevel.UsedTileSet)
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
