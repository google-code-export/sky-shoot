using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Client.Game;
using SkyShoot.Game.Client.View;
using SkyShoot.Game.Controls;

namespace SkyShoot.Game.Screens
{
	internal class GameplayScreen : GameScreen
	{
		private ContentManager _content;

		public override bool IsMenuScreen
		{
			get { return false; }
		}

		public override void LoadContent()
		{
			if (_content == null)
				_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");

			// load landscapes
			Textures.SandLandscape = _content.Load<Texture2D>("Textures/Landscapes/SandLandscape");
			Textures.GrassLandscape = _content.Load<Texture2D>("Textures/Landscapes/GrassLandscape");
			Textures.SnowLandscape = _content.Load<Texture2D>("Textures/Landscapes/SnowLandscape");
			Textures.DesertLandscape = _content.Load<Texture2D>("Textures/Landscapes/DesertLandscape");
			Textures.VolcanicLandscape = _content.Load<Texture2D>("Textures/Landscapes/VolcanicLandscape");

			// load stones
			for (int i = 1; i <= Textures.StonesAmount; i++)
				Textures.Stones[i - 1] = _content.Load<Texture2D>("Textures/Landscapes/Stone" + i);

			// load player
			Textures.PlayerTexture = _content.Load<Texture2D>("Textures/Mobs/Man");

			// load mobs
			for (int i = 1; i <= Textures.MobsAmount; i++)
				Textures.MobTextures[i - 1] = _content.Load<Texture2D>("Textures/Mobs/Spider" + i);

			// load dead player
			Textures.DeadPlayerTexture =
				_content.Load<Texture2D>("Textures/Mobs/man_animation(new man)/death_animation/death_animation_06");

			// load dead spider
			Textures.DeadSpiderTexture =
				_content.Load<Texture2D>(
					"Textures/Mobs/mob_animation(v.2)/paukan_death_animation/paukan_death_animation_03");

			// load mob animation
			for (int i = 1; i <= Textures.SpiderAnimationFrameCount; i++)
				Textures.SpiderAnimation.AddFrame(
					_content.Load<Texture2D>("Textures/Mobs/spider_animation(uncomplete)/spider_" + i.ToString("D2")));

			// load player animation
			for (int i = 1; i <= Textures.PlayerAnimationFrameCount; i++)
				Textures.PlayerAnimation.AddFrame(
					_content.Load<Texture2D>("Textures/Mobs/man_animation(new man)/run/run_" + i.ToString("D2")));

			ScreenManager.Instance.Game.ResetElapsedTime();
		}

		public override void UnloadContent()
		{
			Textures.PlayerAnimation.Clear();
			Textures.SpiderAnimation.Clear();

			if (_content != null)
				_content.Unload();
		}

		public override void HandleInput(Controller controller)
		{
			GameController.Instance.HandleInput(controller);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (GameController.Instance.GameModel == null)
				return;

			GameController.Instance.GameModel.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice graphicsDevice = ScreenManager.Instance.GraphicsDevice;
			SpriteBatch spriteBatch = ScreenManager.Instance.SpriteBatch;
			graphicsDevice.Clear(Color.SkyBlue);

			GameController.Instance.GameModel.Draw(spriteBatch);
		}
	}
}
