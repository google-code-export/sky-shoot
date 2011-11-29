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
using SkyShoot.Game.Client.View;
using System.Drawing.Drawing2D;

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
        private Nuclex.UserInterface.Controls.Desktop.ListControl keyboardList;
        //private LabelControl asdwLabel;
        //private ChoiceControl asdwButton;
        //private LabelControl arrowsLabel;
        //private ChoiceControl arrowsButton;
        private LabelControl cursorLabel;
        private ChoiceControl arrowButton;
		private ChoiceControl plusButton;
		private ChoiceControl crossButton;
		private ChoiceControl targetButton;
        private ButtonControl backButton;
        private ContentManager _content;

        public static short curs = 1;

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
            fullscreenButton.Selected = Settings.Default.FullScreenSelected;
            fullscreenButton.Changed += FullScreenSelected;

            keyboardLabel = new LabelControl("Keyboard:");
            keyboardLabel.Bounds = new UniRectangle(new UniScalar(0.5f, -150), new UniScalar(0.5f, -70), 50, 30);
            optionsScreen.Desktop.Children.Add(keyboardLabel);

			keyboardList = new Nuclex.UserInterface.Controls.Desktop.ListControl
			{
				Bounds = new UniRectangle(260f, 175f, 150f, 50f)
			};
			keyboardList.Items.Add("A, S, D, W");
			keyboardList.Items.Add("Arrows");
			keyboardList.Slider.Bounds.Location.X.Offset -= 1.0f;
			keyboardList.Slider.Bounds.Location.Y.Offset += 1.0f;
			keyboardList.Slider.Bounds.Size.Y.Offset -= 2.0f;
			keyboardList.SelectionMode = ListSelectionMode.Single;
			if (Settings.Default.KeyboardLayout == 0) keyboardList.SelectedItems.Add(0);
			else keyboardList.SelectedItems.Add(1);
			keyboardList.SelectionChanged += keyboardChoice;
			optionsScreen.Desktop.Children.Add(keyboardList);

			//asdwLabel = new LabelControl();
			//asdwLabel.Text = "A, S, D, W";
			//asdwLabel.Bounds = new UniRectangle(new UniScalar(0.5f, -70), new UniScalar(0.5f, -70), 100, 30);
			//optionsScreen.Desktop.Children.Add(asdwLabel);

			//asdwButton = new ChoiceControl();
			//asdwButton.Bounds = new UniRectangle(new UniScalar(0.5f, 30), new UniScalar(0.5f, -70), 100, 30);
			//if (Settings.Default.KeyboardLayout == 0) asdwButton.Selected = true;
			//asdwButton.Changed += ASDWButtonPressed;
			//optionsScreen.Desktop.Children.Add(asdwButton);

			//arrowsLabel = new LabelControl();
			//arrowsLabel.Text = "Arrows";
			//arrowsLabel.Bounds = new UniRectangle(new UniScalar(0.5f, -60), new UniScalar(0.6f, -70), 100, 30);
			//optionsScreen.Desktop.Children.Add(arrowsLabel);

			//arrowsButton = new ChoiceControl();
			//arrowsButton.Bounds = new UniRectangle(new UniScalar(0.5f, 30), new UniScalar(0.6f, -70), 100, 30);
			//arrowsButton.Changed += ArrowsButtonPressed;
			//if (Settings.Default.KeyboardLayout == 1) arrowsButton.Selected = true;
			//optionsScreen.Desktop.Children.Add(arrowsButton);

            cursorLabel = new LabelControl("Cursor:");
            cursorLabel.Bounds = new UniRectangle(new UniScalar(0.5f, -220), new UniScalar(0.8f, -70), 70, 30);
            optionsScreen.Desktop.Children.Add(cursorLabel);

			arrowButton = new ChoiceControl();
            arrowButton.Bounds = new UniRectangle(new UniScalar(0.5f, -140), new UniScalar(0.9f, -70), 70, 30);
            arrowButton.Changed += ArrowButtonPressed;
			if (Settings.Default.Cursor == 1) arrowButton.Selected = true;
            optionsScreen.Desktop.Children.Add(arrowButton);

			plusButton = new ChoiceControl();
            plusButton.Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(0.9f, -70), 70, 30);
			plusButton.Changed += PlusButtonPressed;
			if (Settings.Default.Cursor == 2) plusButton.Selected = true;
            optionsScreen.Desktop.Children.Add(plusButton);

			crossButton = new ChoiceControl();
            crossButton.Bounds = new UniRectangle(new UniScalar(0.5f, 40), new UniScalar(0.9f, -70), 70, 30);
			crossButton.Changed += CrossButtonPressed;
			if (Settings.Default.Cursor == 3) plusButton.Selected = true;
            optionsScreen.Desktop.Children.Add(crossButton);

			targetButton = new ChoiceControl();
            targetButton.Bounds = new UniRectangle(new UniScalar(0.5f, 130), new UniScalar(0.9f, -70), 70, 30);
			targetButton.Changed += TargetButtonPressed;
			if (Settings.Default.Cursor == 4) targetButton.Selected = true;
            optionsScreen.Desktop.Children.Add(targetButton);

            backButton = new ButtonControl();
            backButton.Text = "Back";
            backButton.Bounds = new UniRectangle(new UniScalar(0.5f, -50), new UniScalar(1.1f, -70), 100, 30);
            backButton.Pressed += BackButtonPressed;
            optionsScreen.Desktop.Children.Add(backButton);

			if (_content == null)
				_content = new ContentManager(ScreenManager.Game.Services, "Content");

			Textures.Arrow = _content.Load<Texture2D>("Textures/Cursors/Arrow");
			Textures.Plus = _content.Load<Texture2D>("Textures/Cursors/Plus");
			Textures.Cross = _content.Load<Texture2D>("Textures/Cursors/Cross");
			Textures.Target = _content.Load<Texture2D>("Textures/Cursors/Target");
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

		//void ASDWButtonPressed(object sender, EventArgs e)
		//{
		//    Settings.Default.KeyboardLayout = 0;
		//    Settings.Default.Save();

		//}

		//void ArrowsButtonPressed(object sender, EventArgs e)
		//{
		//    Settings.Default.KeyboardLayout = 1;
		//    Settings.Default.Save();
		//}

		void keyboardChoice(object sender, EventArgs e)
		{
			if (keyboardList.SelectedItems[0] == 0) Settings.Default.KeyboardLayout = 0;
			else Settings.Default.KeyboardLayout = 1;
			Settings.Default.Save();
		}

        void BackButtonPressed(object sender, EventArgs e)
        {
            ExitScreen();
			ScreenManager.AddScreen(new MainMenuScreen());
        }

		void FullScreenSelected(object sender, EventArgs e)
		{
			if (fullscreenButton.Selected == false)
				Settings.Default.FullScreenSelected = false;
			else Settings.Default.FullScreenSelected = true;
			Settings.Default.Save();
		}

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            gui.Draw(gameTime);
			GraphicsDevice graphicsDevice = ScreenManager.GraphicsDevice;
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
			Vector2 pos1 = new Vector2(250f, 370f);
			Vector2 pos2 = new Vector2(340f, 370f);
			Vector2 pos3 = new Vector2(430f, 370f);
			Vector2 pos4 = new Vector2(520f, 370f);
			spriteBatch.Draw(Textures.Arrow, pos1, Color.White);
			spriteBatch.Draw(Textures.Plus, pos2, Color.White);
			spriteBatch.Draw(Textures.Cross, pos3, Color.White);
			spriteBatch.Draw(Textures.Target, pos4, Color.White);
			spriteBatch.End(); 
        }

    }
}
