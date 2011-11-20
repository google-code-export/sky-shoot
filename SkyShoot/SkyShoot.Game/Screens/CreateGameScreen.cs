using System;
using System.Net.Mime;
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
    class CreateGameScreen : GameScreen
    {

        private GuiManager _gui;
        private Screen _mainScreen;
        private ListControl _maxPlayersList;
        private ListControl _tileList;
        private ListControl _gameModList;
        private LabelControl _maxPlaersLabel;
        private LabelControl _tileLabel;
        private LabelControl _gameModeLabel;

        public CreateGameScreen()
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

            // выбора максимального кол-ва игроков
            _maxPlaersLabel = new LabelControl
            {
                Bounds = new UniRectangle(-60f, -34f, 100f, 24f),
                Text = "Max Players"
            };
            _mainScreen.Desktop.Children.Add(_maxPlaersLabel);

            _maxPlayersList = new ListControl
            {
                Bounds = new UniRectangle(-60f, -4f, 100f, 300f)
            };
            for (int i = 1; i < 11; i++)
            {
                _maxPlayersList.Items.Add(i + "");    
            }
            _maxPlayersList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _maxPlayersList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _maxPlayersList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            _maxPlayersList.SelectionMode = ListSelectionMode.Single;
            _maxPlayersList.SelectedItems.Add(4);
            _mainScreen.Desktop.Children.Add(_maxPlayersList);

            // выбор карты
            _tileLabel = new LabelControl
            {
                Bounds = new UniRectangle(60f, -34f, 150f, 24f),
                Text = "Map"
            };
            _mainScreen.Desktop.Children.Add(_tileLabel);

            _tileList = new ListControl
            {
                Bounds = new UniRectangle(60f, -4f, 150f, 300f)
            };
            _tileList.Items.Add("Snow");
            _tileList.Items.Add("Desert");
            _tileList.Items.Add("Grass");
            _tileList.Items.Add("Sand");
            _tileList.Items.Add("Volcanic");
            _tileList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _tileList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _tileList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            _tileList.SelectionMode = ListSelectionMode.Single;
            _tileList.SelectedItems.Add(4);
            _mainScreen.Desktop.Children.Add(_tileList);

            // выбор режима игры
            _gameModeLabel = new LabelControl
            {
                Bounds = new UniRectangle(230f, -34f, 150f, 24f),
                Text = "Mode"
            };
            _mainScreen.Desktop.Children.Add(_gameModeLabel);

            _gameModList = new ListControl
            {
                Bounds = new UniRectangle(230f, -4f, 150f, 300f)
            };
            _gameModList.Items.Add("Coop");
            _gameModList.Items.Add("Deathmatch");
            _gameModList.Items.Add("Campaign");
            _gameModList.Slider.Bounds.Location.X.Offset -= 1.0f;
            _gameModList.Slider.Bounds.Location.Y.Offset += 1.0f;
            _gameModList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            _gameModList.SelectionMode = ListSelectionMode.Single;
            _gameModList.SelectedItems.Add(4);
            _mainScreen.Desktop.Children.Add(_gameModList);

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _gui.Draw(gameTime);
        }

        

    }
}
