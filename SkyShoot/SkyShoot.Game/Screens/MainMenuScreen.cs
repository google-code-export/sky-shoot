using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SkyShoot.Game.ScreenManager;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace SkyShoot.Game.Screens
{
    class MainMenuScreen : ScreenManager.GameScreen
    {
        private GuiManager _gui;
        private Screen _mainScreen;

        public override void LoadContent()
        {
            base.LoadContent();
            _gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            _mainScreen = new Screen(viewport.Width, viewport.Height);
            _mainScreen.Desktop.Bounds = new UniRectangle(
              new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
              new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
            );

            ButtonControl _playGameButton = new ButtonControl()
            {
                Text = "Multiplayer",
                Bounds = new UniRectangle(new UniScalar(0.30f,0),new UniScalar(0.2f,0),new UniScalar(0.4f,0), new UniScalar(0.1f,0)),
            };
            _mainScreen.Desktop.Children.Add(_playGameButton);

            ButtonControl _optionsButton = new ButtonControl()
            {
                Text = "Options",
                Bounds = new UniRectangle(new UniScalar(0.30f,0),new UniScalar(0.35f,0),new UniScalar(0.4f,0), new UniScalar(0.1f,0)),
            };
            _mainScreen.Desktop.Children.Add(_optionsButton);

            ButtonControl _exitButton = new ButtonControl()
            {
                Text = "Exit",
                Bounds = new UniRectangle(new UniScalar(0.30f,0),new UniScalar(0.5f,0),new UniScalar(0.4f,0), new UniScalar(0.1f,0)),
            };
            _mainScreen.Desktop.Children.Add(_exitButton);

            

            _playGameButton.Pressed += PlayGameButtonPressed;
            _optionsButton.Pressed += OptionsButtonPressed;
            _exitButton.Pressed += ExitMenuButtonPressed;

        }
        
        void PlayGameButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new LoginScreen()); 
        }


        void OptionsButtonPressed(object sender, EventArgs e)
        {
            //ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        void ExitMenuButtonPressed(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _gui.Screen = _mainScreen;
            _gui.Draw(gameTime);
        }
    }
}
