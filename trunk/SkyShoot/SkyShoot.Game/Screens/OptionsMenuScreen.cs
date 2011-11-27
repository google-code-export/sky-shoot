using System;
using Nuclex.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Input;
using SkyShoot.Game.ScreenManager;

namespace SkyShoot.Game.Screens
{
    class OptionsMenuScreen : ScreenManager.GameScreen
    {
        private GuiManager gui;
        private Nuclex.UserInterface.Screen optionsScreen;
        private LabelControl titleLabel;
        private LabelControl fullscreenLabel;
        private OptionControl fullscreenButton;
        private LabelControl keyboardLabel;
        private ButtonControl asdwButton;
        private ButtonControl arrowsButton;
        private LabelControl cursorLabel;
        private ButtonControl arrowButton;
        private ButtonControl plusButton;
        private ButtonControl crossButton;
        private ButtonControl targetButton;
        private ButtonControl backButton;

        public static int i = 0;
        public static short curs = 4;
        public static int f = 0;

        public OptionsMenuScreen()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            gui = ScreenManager.Gui;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            optionsScreen = new Nuclex.UserInterface.Screen(viewport.Width, viewport.Height);
            gui.Screen = optionsScreen;

            optionsScreen.Desktop.Bounds = new UniRectangle(
                new UniScalar(0.1f, 0.0f), new UniScalar(0.1f, 0.0f),
                new UniScalar(0.8f, 0.0f), new UniScalar(0.8f, 0.0f)
            );

            titleLabel = new LabelControl("Options");
            titleLabel.Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.1f, -70), 100, 30);
            optionsScreen.Desktop.Children.Add(titleLabel);

            fullscreenLabel = new LabelControl("FullScreen: ");
            fullscreenLabel.Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.3f, -70), 80, 30);
            optionsScreen.Desktop.Children.Add(fullscreenLabel);

            fullscreenButton = new OptionControl();
            fullscreenButton.Bounds = new UniRectangle(new UniScalar(0.5f, 30), new UniScalar(0.3f, -70), 100, 30);
            optionsScreen.Desktop.Children.Add(fullscreenButton);
            //fullscreenButton.Selected = Settings.Default.FullScreenSelected;
            fullscreenButton.Selected = false;
            fullscreenButton.Changed += FullScreenSelected;

            keyboardLabel = new LabelControl("Keyboard:");
            keyboardLabel.Bounds = new UniRectangle(new UniScalar(0.5f, -150), new UniScalar(0.5f, -70), 50, 30);
            optionsScreen.Desktop.Children.Add(keyboardLabel);

            asdwButton = new ButtonControl();
            asdwButton.Text = "A, S, D, W";
            asdwButton.Bounds = new UniRectangle(new UniScalar(0.5f, -70), new UniScalar(0.5f, -70), 100, 30);
            asdwButton.Pressed += ASDWButtonPressed;
            optionsScreen.Desktop.Children.Add(asdwButton);

            arrowsButton = new ButtonControl();
            arrowsButton.Text = "Arrows";
            arrowsButton.Bounds = new UniRectangle(new UniScalar(0.5f, 40), new UniScalar(0.5f, -70), 100, 30);
            arrowsButton.Pressed += ArrowsButtonPressed;
            optionsScreen.Desktop.Children.Add(arrowsButton);

            cursorLabel = new LabelControl("Cursor:");
            cursorLabel.Bounds = new UniRectangle(new UniScalar(0.5f, -220), new UniScalar(0.7f, -70), 70, 30);
            optionsScreen.Desktop.Children.Add(cursorLabel);

            arrowButton = new ButtonControl();
            arrowButton.Text = "Arrow";
            arrowButton.Bounds = new UniRectangle(new UniScalar(0.5f, -140), new UniScalar(0.7f, -70), 70, 30);
            arrowsButton.Pressed += ArrowButtonPressed;
            optionsScreen.Desktop.Children.Add(arrowButton);

            plusButton = new ButtonControl();
            plusButton.Text = "Plus";
            plusButton.Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.7f, -70), 70, 30);
            arrowsButton.Pressed += PlusButtonPressed;
            optionsScreen.Desktop.Children.Add(plusButton);

            crossButton = new ButtonControl();
            crossButton.Text = "Cross";
            crossButton.Bounds = new UniRectangle(new UniScalar(0.5f, 40), new UniScalar(0.7f, -70), 70, 30);
            arrowsButton.Pressed += CrossButtonPressed;
            optionsScreen.Desktop.Children.Add(crossButton);

            targetButton = new ButtonControl();
            targetButton.Text = "Target";
            targetButton.Bounds = new UniRectangle(new UniScalar(0.5f, 130), new UniScalar(0.7f, -70), 70, 30);
            arrowsButton.Pressed += TargetButtonPressed;
            optionsScreen.Desktop.Children.Add(targetButton);

            backButton = new ButtonControl();
            backButton.Text = "Back";
            backButton.Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.9f, -70), 100, 30);
            backButton.Pressed += BackButtonPressed;
            optionsScreen.Desktop.Children.Add(backButton);

        }

        private void ArrowButtonPressed(object sender, EventArgs e)
        {
            curs = 1;
            Settings.Default.Cursor = curs;
            Settings.Default.Save();
        }

        private void PlusButtonPressed(object sender, EventArgs e)
        {
            curs = 2;
            Settings.Default.Cursor = curs;
            Settings.Default.Save();
        }

        private void CrossButtonPressed(object sender, EventArgs e)
        {
            curs = 3;
            Settings.Default.Cursor = curs;
            Settings.Default.Save();
        }

        private void TargetButtonPressed(object sender, EventArgs e)
        {
            curs = 4;
            Settings.Default.Cursor = curs;
            Settings.Default.Save();
        }

        void ASDWButtonPressed(object sender, EventArgs e)
        {
            Settings.Default.KeyboardLayout = 0;
            Settings.Default.Save();

        }

        void ArrowsButtonPressed(object sender, EventArgs e)
        {
            Settings.Default.KeyboardLayout = 1;
            Settings.Default.Save();
        }

        void BackButtonPressed(object sender, EventArgs e)
        {
            ExitScreen();
        }

        void FullScreenSelected(object sender, EventArgs e)
        {
            if (fullscreenButton.Selected == false)
                Settings.Default.FullScreen = 0;
            else Settings.Default.FullScreen = 1;
            Settings.Default.Save();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            gui.Draw(gameTime);
        }

    }
}
