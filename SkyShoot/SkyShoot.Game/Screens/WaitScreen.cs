using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.Client.Game;
using SkyShoot.Game.ScreenManager;

namespace SkyShoot.Game.Screens
{
    internal class WaitScreen : GameScreen
    {
        private GuiManager _gui;
        private Screen _mainScreen;
        private LabelControl _playersLabel;
        private ListControl _playersList;
        private ButtonControl _leaveButton;
        private LabelControl _gameModeLabel;
        private LabelControl _maxPlayersLabel;
        private string Tile;
        private string GameMode;
        private string MaxPlayers;
        private List<string> tmpPlayersList;
        private int GameId;
		private static Texture2D _texture;
		private ContentManager _content;
		private SpriteBatch spriteBatch;

        public WaitScreen(string tile, string gameMod, string maxPlayers, int gameId)
        {
            this.Tile = tile;
            this.GameMode = gameMod;
            this.MaxPlayers = maxPlayers;
            this.GameId = gameId;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;
			if (_content == null)
				_content = new ContentManager(ScreenManager.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_02_fix");

            _mainScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
                );

            _playersLabel = new LabelControl
            {
                Bounds = new UniRectangle(-60f, -34f, 100f, 24f),
                Text = "Players"
            };
            _mainScreen.Desktop.Children.Add(_playersLabel);

            //
            // список игроков
            //
            _playersList = new ListControl
            {
                Bounds = new UniRectangle(-60f, -4f, 200f, 300f)
            };
            // вывод списка игрков
            GameDescription[] tmpGameDescriptionList;
            tmpGameDescriptionList = GameController.Instance.GetGameList();
            for (int i = 0; tmpGameDescriptionList != null && i < tmpGameDescriptionList.Length; i++)
            {
                if (this.GameId == tmpGameDescriptionList[i].GameId)
                {
                    tmpPlayersList = tmpGameDescriptionList[i].Players;
                }
            }
            for (int i = 0; i < tmpPlayersList.Count; i++)
            {
                _playersList.Items.Add(tmpPlayersList[i]);
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
            
            // информация о игре
            // Tile label
            _maxPlayersLabel = new LabelControl
            {
                Bounds = new UniRectangle(200f, 200f, 100f, 24f),
                Text = "Map :"
            };
            _mainScreen.Desktop.Children.Add(_maxPlayersLabel);

            _maxPlayersLabel = new LabelControl
            {
                Bounds = new UniRectangle(320f, 200f, 100f, 24f),
                Text = Tile
            };
            _mainScreen.Desktop.Children.Add(_maxPlayersLabel);

            // GameMode label
            _gameModeLabel = new LabelControl
            {
                Bounds = new UniRectangle(200f, 234f, 100f, 24f),
                Text = "Game Mode :"
            };
            _mainScreen.Desktop.Children.Add(_gameModeLabel);

            _gameModeLabel = new LabelControl
            {
                Bounds = new UniRectangle(320f, 234f, 100f, 24f),
                Text = GameMode
            };
            _mainScreen.Desktop.Children.Add(_gameModeLabel);

            // Max Players label
            _maxPlayersLabel = new LabelControl
            {
                Bounds = new UniRectangle(200f, 268f, 100f, 24f),
                Text = "Max Players :"
            };
            _mainScreen.Desktop.Children.Add(_maxPlayersLabel);

            _maxPlayersLabel = new LabelControl
            {
                Bounds = new UniRectangle(320f, 268f, 100f, 24f),
                Text = MaxPlayers
            };
            _mainScreen.Desktop.Children.Add(_maxPlayersLabel);
            
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
        	for (int i = 0; i < names.Length; i++)
        	{
        		_playersList.Items.Add(names[i]);	
        	}
        }

        public override void Draw(GameTime gameTime)
        {
			spriteBatch = ScreenManager.SpriteBatch;
			spriteBatch.Begin();
			spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			spriteBatch.End();
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

    }

}
