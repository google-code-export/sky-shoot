using System;

using System.Diagnostics;

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
    internal class MultiplayerScreen : GameScreen
    {
        private GuiManager _gui;
        private ButtonControl _backButton;
        private ButtonControl _createGameButton;
        private ButtonControl _joinGameButton;
        private ButtonControl _refreshButton;
        private ListControl _gameList;
        private Screen _mainScreen;
        private LabelControl _mapLabel;
        private GameDescription[] _tempGameList;
		private static Texture2D _texture;
		private ContentManager _content;
		private SpriteBatch _spriteBatch;

        public override void LoadContent()
        {
            base.LoadContent();
            _gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;
			if (_content == null)
				_content = new ContentManager(ScreenManager.Game.Services, "Content");

			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");

			_mainScreen.Desktop.Bounds = new UniRectangle(
				new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
				new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
			);

            _mainScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
                );

            // CreateGame Button
            _createGameButton = new ButtonControl
            {
                Text = "Create Game",
                Bounds = new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -160f), 120, 32)
            };
            _createGameButton.Pressed += CreateGameButtonPressed;
            _mainScreen.Desktop.Children.Add(_createGameButton);

            // Back Button
            _backButton = new ButtonControl
            {
                Text = "Back",
                Bounds = new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -80f), 120, 32)
            };
            _backButton.Pressed += BackButtonPressed;
            _mainScreen.Desktop.Children.Add(_backButton);

            // JoinGame Button
            _joinGameButton = new ButtonControl
            {
                Text = "Join Game",
                Bounds = new UniRectangle(new UniScalar(0.5f, -350f), new UniScalar(0.4f, -120f), 120, 32)
            };
            _joinGameButton.Pressed += JoinGameButtonPressed;
            _mainScreen.Desktop.Children.Add(_joinGameButton);

            //Label of maps
            _mapLabel = new LabelControl
            {
                Bounds = new UniRectangle(300.0f, -30.0f, 200.0f, 24.0f), 
                Text = "Games"
            };
            _mainScreen.Desktop.Children.Add(_mapLabel);

            //Games List
            _gameList = new ListControl
            {
                Bounds = new UniRectangle(300f, -10f, 225f, 300f)
            };
            _gameList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _gameList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _gameList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            //
            // запрос списка игр с сервера и его вывод
            //
            _tempGameList = GameController.Instance.GetGameList();

            if(_tempGameList == null)
                return;

            foreach (GameDescription gameDescription in _tempGameList)
            {
                _gameList.Items.Add(gameDescription.ToString());
            }
            _gameList.SelectionMode = ListSelectionMode.Single;
            _gameList.SelectedItems.Add(4);
            _mainScreen.Desktop.Children.Add(_gameList);
            

            // Refresh Button
            _refreshButton = new ButtonControl
            {
                Text = "Refresh",
                Bounds = new UniRectangle(new UniScalar(0.5f, -20f), new UniScalar(0.4f, 140f), 120, 32)
            };
            _refreshButton.Pressed += RefreshButtonPressed;
            _mainScreen.Desktop.Children.Add(_refreshButton);

        	_gameList.SelectedItems[0] = 0;

        }

        private void BackButtonPressed(object sender, EventArgs args)
        {
            ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        private void JoinGameButtonPressed(object sender, EventArgs args)
        {
            //todo setActive
            ExitScreen();

            _tempGameList = GameController.Instance.GetGameList();

            if (_tempGameList.Length == 0)
                return;

			if ( _gameList.SelectedItems.Count != 0 )
			{
				ScreenManager.AddScreen(new WaitScreen(
					_tempGameList[_gameList.SelectedItems[0]].UsedTileSet + "",
					_tempGameList[_gameList.SelectedItems[0]].GameType + "",
					_tempGameList[_gameList.SelectedItems[0]].MaximumPlayersAllowed + "",
					_tempGameList[_gameList.SelectedItems[0]].GameId));	
			}

			
			

            if(!GameController.Instance.JoinGame(_tempGameList[_gameList.SelectedItems[0]]))
            {
                //todo popup
                Trace.WriteLine("Join game failed");
            }

        }

        private void CreateGameButtonPressed(object sender, EventArgs args)
        {
            //todo setActive
            //foreach (GameScreen screen in ScreenManager.GetScreens()) screen.ExitScreen();

            ExitScreen();

            ScreenManager.AddScreen(new CreateGameScreen());

        }

        private void RefreshButtonPressed(object sender, EventArgs args)
        {
            _gameList.Items.Clear();
            _tempGameList = GameController.Instance.GetGameList();

            if (_tempGameList == null)
                return;

            foreach (GameDescription gameDescription in _tempGameList)
            {
                _gameList.Items.Add(gameDescription.ToString());
            }
        }

        public override void Draw(GameTime gameTime)
        {
			_spriteBatch = ScreenManager.SpriteBatch;
			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

    }
}
