using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;

using SkyShoot.Contracts.Session;

using SkyShoot.Game.ScreenManager;

using SkyShoot.Game.Client.Game;

namespace SkyShoot.Game.Screens
{
    internal class WaitScreen : GameScreen
    {
        private GuiManager _gui;
        private Screen _mainScreen;
        private ListControl _playersList;
        private ButtonControl _leaveButton;
        private readonly string _tile;
        private readonly string _gameMode;
        private readonly string _maxPlayers;
        private List<string> _tmpPlayersList;
        private readonly int _gameId;
		private static Texture2D _texture;
		private ContentManager content;
		private SpriteBatch _spriteBatch;
		private SpriteFont spriteFont;

        public WaitScreen(string tile, string gameMod, string maxPlayers, int gameId)
        {
            _tile = tile;
            _gameMode = gameMod;
            _maxPlayers = maxPlayers;
            _gameId = gameId;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;
			if (content == null)
				content = new ContentManager(ScreenManager.Game.Services, "Content");

			_texture = content.Load<Texture2D>("Textures/screens/screen_02_fix");

        	spriteFont = content.Load<SpriteFont>("Times New Roman");

            _mainScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
                );
            //
            // список игроков
            //
            _playersList = new ListControl
            {
                Bounds = new UniRectangle(-60f, -4f, 200f, 300f)
            };

            // вывод списка игрков
            GameDescription[] tmpGameDescriptionList = GameController.Instance.GetGameList();

            if(tmpGameDescriptionList == null)
                return;

            foreach (GameDescription gameDescription in tmpGameDescriptionList)
            {
                if (_gameId == gameDescription.GameId)
                {
                    _tmpPlayersList = gameDescription.Players;
                }
            }
            foreach (string player in _tmpPlayersList)
            {
                _playersList.Items.Add(player);
            }
            _playersList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _playersList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _playersList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            _playersList.SelectionMode = ListSelectionMode.Single;
            _playersList.SelectedItems.Add(4);
            _mainScreen.Desktop.Children.Add(_playersList);

            // Leave Button
            _leaveButton = new ButtonControl
            {
                Text = "Leave",
                Bounds = new UniRectangle(new UniScalar(0.5f, -378f), new UniScalar(0.4f, 190f), 120, 32)
            };
            _leaveButton.Pressed += LeaveButtonPressed;
            _mainScreen.Desktop.Children.Add(_leaveButton);
            
        }

        private void LeaveButtonPressed(object sender, EventArgs args)
        {  
            GameController.Instance.LeaveGame();
            ExitScreen();
            ScreenManager.AddScreen(new MultiplayerScreen());
        }

        public void ChangePlayerList(String[] names)
        {
            _playersList.Items.Clear();
            foreach (string playerName in names)
            {
                _playersList.Items.Add(playerName);
            }
        }

        public override void Draw(GameTime gameTime)
        {
			_spriteBatch = ScreenManager.SpriteBatch;
			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.DrawString(spriteFont, "Players", new Vector2(20f, 25f), Color.Red, 0, 
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(spriteFont, "Map: ", new Vector2(280f, 260f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(spriteFont, _tile, new Vector2(400f, 260f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(spriteFont, "Game Mode:", new Vector2(280f, 290f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(spriteFont, _gameMode, new Vector2(400f, 290f), Color.Red, 0, 
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(spriteFont, "Max Players:", new Vector2(280f, 320f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(spriteFont, _maxPlayers, new Vector2(400f, 320f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.End();
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

    }

}
