using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

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
		private ContentManager _content;
		private SpriteBatch _spriteBatch;
		private SpriteFont _spriteFont;

        public WaitScreen()
        {
			//_tile = tile;
			//_gameMode = gameMod;
			//_maxPlayers = maxPlayers;
			//_gameId = gameId;

            ScreenState = ScreenState.Active;
        }

		public static String Tile { get; set; }

		public static String GameMod { get; set; }

		public static String MaxPlayers { get; set; }

		public static Int32 GameId { get; set; }

        public override void LoadContent()
        {
            base.LoadContent();
			_gui = ScreenManager.ScreenManager.Instance.Gui;
			Viewport viewport = ScreenManager.ScreenManager.Instance.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;
			if (_content == null)
				_content = new ContentManager(ScreenManager.ScreenManager.Instance.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_02_fix");

        	_spriteFont = _content.Load<SpriteFont>("Times New Roman");

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

			if (_tmpPlayersList == null)
				return;

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
            //ExitScreen();
			ScreenManager.ScreenManager.Instance.ActiveScreen = ScreenManager.ScreenManager.ScreenEnum.MultiplayerScreen;
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
			_spriteBatch = ScreenManager.ScreenManager.Instance.SpriteBatch;
			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.DrawString(_spriteFont, "Players", new Vector2(20f, 25f), Color.Red, 0, 
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, "Map: ", new Vector2(280f, 260f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, Tile, new Vector2(400f, 260f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, "Game Mode:", new Vector2(280f, 290f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, GameMod, new Vector2(400f, 290f), Color.Red, 0, 
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, "Max Players:", new Vector2(280f, 320f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.DrawString(_spriteFont, MaxPlayers, new Vector2(400f, 320f), Color.Red, 0,
									new Vector2(0f, 0f), 0.8f, SpriteEffects.None, 1f);
			_spriteBatch.End();
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

    }

}
