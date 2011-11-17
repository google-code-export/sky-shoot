using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.Input;
using Nuclex.UserInterface.Controls;
using SkyShoot.Game.ScreenManager;

namespace SkyShoot.Game.Screens
{
    class LoginScreen:ScreenManager.GameScreen
    {
        private GuiManager gui;
        private LabelControl loginLabel;
        private InputControl loginBox;
        private LabelControl passwordLabel;
        private InputControl passwordBox;
        private ButtonControl backButton;
        private ButtonControl loginButton;
        private Screen mainScreen;

        public LoginScreen()
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

            // Login Input
            loginBox = new InputControl();
            loginBox.Bounds = new UniRectangle(new UniScalar(0.5f, -100f),new UniScalar(0.4f,-30), 200, 30);
            loginBox.Text = "";
            mainScreen.Desktop.Children.Add(loginBox);

            // Password Input
            passwordBox = new InputControl();
            passwordBox.Bounds = new UniRectangle(new UniScalar(0.5f,-100f), new UniScalar(0.4f,30), 200, 30);
            passwordBox.Text = "";
            mainScreen.Desktop.Children.Add(passwordBox);

            // Login Label
            loginLabel = new LabelControl("Username");
            loginLabel.Bounds = new UniRectangle(
                new UniScalar(0.5f, -32), new UniScalar(0.4f, -70), 100, 30
            );
            mainScreen.Desktop.Children.Add(loginLabel);
            
            // Password Label
            passwordLabel = new LabelControl("Password");
            passwordLabel.Bounds = new UniRectangle(
                new UniScalar(0.5f, -32), new UniScalar(0.4f, 0), 100, 30
            );
            mainScreen.Desktop.Children.Add(passwordLabel);

            // Login Button
            loginButton = new ButtonControl();
            loginButton.Text = "Login";
            loginButton.Bounds = new UniRectangle(
              new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32
            );
            loginButton.Pressed += LoginButtonPressed;
            mainScreen.Desktop.Children.Add(loginButton);

            // Password Button
            backButton = new ButtonControl();
            backButton.Text = "Back";
            backButton.Bounds = new UniRectangle(
              new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32
            );
            backButton.Pressed += BackButtonPressed;
            mainScreen.Desktop.Children.Add(backButton);

        }
        private void BackButtonPressed(object sender, EventArgs args)
        {
            ExitScreen();   
        }
        private void LoginButtonPressed(object sender, EventArgs args)
        {

            if(loginBox.Text.Length < 3) ScreenManager.AddScreen(new MessageBox("Username is too short!\nPress Enter to continue"));
            else if (passwordBox.Text.Length < 3) ScreenManager.AddScreen(new MessageBox("Password is too short!\nPress Enter to continue"));
            else
            {
                // todo login
                foreach (GameScreen screen in ScreenManager.GetScreens()) screen.ExitScreen();
                ScreenManager.AddScreen(new LoadingScreen(true, new GameplayScreen()));
            }
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            gui.Draw(gameTime);
        }
    }
}
