using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using SkyShoot.Game.ScreenManager;
using Microsoft.Xna.Framework.Input;

namespace SkyShoot.Game.Screens
{
    internal class MultiplayerScreen : ScreenManager.GameScreen
    {
        private GuiManager gui;
        private ButtonControl backButton;
        private ButtonControl createGameButton;
        private ButtonControl joinGameButton;
        private ListControl mapList;
        private Screen mainScreen;
        private LabelControl mapLabel;

        public MultiplayerScreen()
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();
            gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            mainScreen = new Screen(viewport.Width, viewport.Height);
            gui.Screen = mainScreen;

            mainScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
                );

            // CreateGame Button
            createGameButton = new ButtonControl();
            createGameButton.Text = "Create Game";
            createGameButton.Bounds = new UniRectangle(
                new UniScalar(0.5f, -350f), new UniScalar(0.4f, -160f), 120, 32
                );
            createGameButton.Pressed += CreateGameButtonPressed;
            mainScreen.Desktop.Children.Add(createGameButton);

            // Back Button
            backButton = new ButtonControl();
            backButton.Text = "Back";
            backButton.Bounds = new UniRectangle(
                new UniScalar(0.5f, -350f), new UniScalar(0.4f, -80f), 120, 32
                );
            backButton.Pressed += BackButtonPressed;
            mainScreen.Desktop.Children.Add(backButton);

            // JoinGame Button
            joinGameButton = new ButtonControl();
            joinGameButton.Text = "Join Game";
            joinGameButton.Bounds = new UniRectangle(
                new UniScalar(0.5f, -350f), new UniScalar(0.4f, -120f), 120, 32
                );
            joinGameButton.Pressed += JoinGameButtonPressed;
            mainScreen.Desktop.Children.Add(joinGameButton);

            //Label of maps
            mapLabel = new LabelControl();
            mapLabel.Bounds = new UniRectangle(300.0f, -30.0f, 200.0f, 24.0f);
            mapLabel.Text = "Maps";
            mainScreen.Desktop.Children.Add(mapLabel);

            //Map List
            mapList = new ListControl();
            mapList.Bounds = new UniRectangle(300f, -10f, 200f, 300f);
            mapList.Slider.Bounds.Location.X.Offset -= 1.0f;
            mapList.Slider.Bounds.Location.Y.Offset += 1.0f;
            mapList.Slider.Bounds.Size.Y.Offset -= 2.0f;
            for (int i = 0; i < 20; i++)
            {
                mapList.Items.Add("Map " + (i + 1) );
            }
            mapList.Items.Add("Map 100500");
            mapList.SelectionMode = ListSelectionMode.Single;
            mapList.SelectedItems.Add(4);
            mainScreen.Desktop.Children.Add(mapList);

        }

        private void BackButtonPressed(object sender, EventArgs args)
        {
            ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        private void JoinGameButtonPressed(object sender, EventArgs args)
        {
            ExitScreen();
            ScreenManager.AddScreen(new GameplayScreen());
        }

        private void CreateGameButtonPressed(object sender, EventArgs args)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens()) screen.ExitScreen();
            ScreenManager.AddScreen(new LoadingScreen(true, new GameplayScreen()));
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            gui.Draw(gameTime);
        }

    }
}
