using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using SkyShoot.Contracts.Session;

using SkyShoot.Game.Client.View;
using IDrawable = SkyShoot.Game.Client.View.IDrawable;

namespace SkyShoot.Game.Client.Game
{
	public class GameLevel : IDrawable
	{
		public const int StonesNumber = 50;

		public static int Width { get; private set; }

		public static int Height { get; private set; }

		private static Texture2D _texture;

		private static Texture2D _gunTexture;
		private static Texture2D _laserTexture;

		private SoundManager _soundManager;

		public GameLevel(Contracts.Session.GameLevel gameLevel)
		{
			Width = (int) gameLevel.levelWidth;
			Height = (int) gameLevel.levelHeight;

			//_gunTexture = Textures.Clone(Textures.Gun);
			//_laserTexture = Textures.Clone(Textures.Laser);

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

			var random = new Random();

			for (int i = 0; i < StonesNumber; i++)
			{
				int stone = random.Next(3);
				var randomPosition = new Vector2(random.Next(Width - Textures.Stones[stone].Width),
				                                 random.Next(Height - Textures.Stones[stone].Height));
				Textures.Merge(_texture, Textures.Stones[stone], randomPosition);
			}

			//Textures.Merge(_texture, Textures.Gun, weaponPosition);
		}

		public void AddTexture(Texture2D texture, Vector2 position)
		{
			Textures.Merge(_texture, texture, position);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, Vector2.Zero, new Rectangle(0, 0, Width, Height), Color.White);
			//spriteBatch.Draw(_gunTexture, new Rectangle(Width + 5, Height + 29, Width + 45, Height + 5), Color.White);
			//spriteBatch.Draw(_laserTexture, new Rectangle(Width + 50, Height + 29, Width + 90, Height + 5), Color.White);
		}
	}
}
