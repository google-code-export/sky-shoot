using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Contracts.Session;
using SkyShoot.Game.Client.Game;
using SkyShoot.Game.ScreenManager;

namespace SkyShoot.Game.Screens
{
    internal class MultiplayerScreen : GameScreen
    {
        private GuiManager _gui;
        private ButtonControl _backButton;
        private ButtonControl _createGameButton;
        private ButtonControl _joinGameButton;
        private ListControl _mapList;
        private Screen _mainScreen;
        private LabelControl _mapLabel;

        public MultiplayerScreen()
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();
            _gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _gui.Screen = _mainScreen;

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
                Bounds = new UniRectangle(300.0f, -30.0f, 200.0f, 24.0f), Text = "Maps"
            };
            _mainScreen.Desktop.Children.Add(_mapLabel);

            //Map List
            _mapList = new ListControl
            {
                Bounds = new UniRectangle(300f, -10f, 200f, 300f)
            };
            _mapList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _mapList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _mapList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            for (int i = 0; i < 20; i++)
            {
                _mapList.Items.Add("Map " + (i + 1) );
            }
            _mapList.Items.Add("Map 100500");
            _mapList.SelectionMode = ListSelectionMode.Single;
            _mapList.SelectedItems.Add(4);
            _mainScreen.Desktop.Children.Add(_mapList);

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

            //todo temporary
            GameController.Instance.JoinGame(GameController.Instance.GetGameList()[0]);

        }

        private void CreateGameButtonPressed(object sender, EventArgs args)
        {
            //todo setActive
            foreach (GameScreen screen in ScreenManager.GetScreens()) screen.ExitScreen();

            //todo field for maxPlayers
            GameController.Instance.CreateGame(GameMode.Deathmatch, 2);

            //todo refresh button
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

    }
}
