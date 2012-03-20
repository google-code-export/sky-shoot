using System;

using Nuclex.UserInterface;

using Nuclex.UserInterface.Controls;

using Nuclex.UserInterface.Controls.Desktop;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SkyShoot.Game.Controls;

using SkyShoot.Game.Client.Game;
using Microsoft.Xna.Framework.Audio;

namespace SkyShoot.Game.Screens
{
	internal class LoginScreen : GameScreen
	{
		AudioEngine engine;
		SoundBank soundBank;
		WaveBank waveBank;

		private LabelControl _loginLabel;
		private LabelControl _passwordLabel;

		private Controls.InputControl _loginBox;
		private Controls.InputControl _passwordBox;

		private ButtonControl _exitButton;
		private ButtonControl _loginButton;
		private ButtonControl _newAccountButton;

		private static Texture2D _texture;

		private readonly ContentManager _content;

		private SpriteBatch _spriteBatch;

		public override bool IsMenuScreen
		{
			get { return true; }
		}

		public LoginScreen()
		{
			CreateControls();
			InitializeControls();

			_content = new ContentManager(ScreenManager.Instance.Game.Services, "Content");
		}

		private void CreateControls()
		{
			// Login Input
			_loginBox = new Controls.InputControl
			            	{
			            		Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, -30), 200, 30),
			            		Text = Settings.Default.login
			            	};

			// Password Input
			_passwordBox = new Controls.InputControl
			               	{
			               		Bounds = new UniRectangle(new UniScalar(0.5f, -100f), new UniScalar(0.4f, 30), 200, 30),
			               		Text = Settings.Default.password
			               	};

			// Login Label
			_loginLabel = new LabelControl("Username")
			              	{
			              		Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, -70), 100, 30)
			              	};

			// Password Label
			_passwordLabel = new LabelControl("Password")
			                 	{
			                 		Bounds = new UniRectangle(new UniScalar(0.5f, -32), new UniScalar(0.4f, 0), 100, 30)
			                 	};

			// Login Button
			_loginButton = new ButtonControl
			               	{
			               		Text = "Login",
			               		Bounds = new UniRectangle(new UniScalar(0.5f, 110), new UniScalar(0.4f, 70), 100, 32)
			               	};

			// Back Button
			_exitButton = new ButtonControl
			              	{
			              		Text = "Exit",
			              		Bounds = new UniRectangle(new UniScalar(0.5f, -210f), new UniScalar(0.4f, 70), 100, 32),												
			              	};

			// New Account Button
			_newAccountButton = new ButtonControl
			                    	{
			                    		Text = "Create new account",
			                    		Bounds = new UniRectangle(new UniScalar(0.5f, -75f), new UniScalar(0.4f, 70), 150, 32)
			                    	};
		}

		private void InitializeControls()
		{
			Desktop.Children.Add(_loginBox);
			Desktop.Children.Add(_passwordBox);
			Desktop.Children.Add(_loginLabel);
			Desktop.Children.Add(_passwordLabel);
			Desktop.Children.Add(_exitButton);
			Desktop.Children.Add(_newAccountButton);
			Desktop.Children.Add(_loginButton);

			ScreenManager.Instance.Controller.AddListener(_loginButton, LoginButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_exitButton, ExitButtonPressed);
			ScreenManager.Instance.Controller.AddListener(_newAccountButton, NewAccountButtonPressed);

			engine = new AudioEngine("Content\\Sounds\\BackSounds.xgs");
			soundBank = new SoundBank(engine, "Content\\Sounds\\Sound Bank.xsb");
			waveBank = new WaveBank(engine, "Content\\Sounds\\Wave Bank.xwb");
		}

		public override void LoadContent()
		{
			_texture = _content.Load<Texture2D>("Textures/screens/screen_05_fix");
		}

		public override void UnloadContent()
		{
			_content.Unload();
		}

		private void ExitButtonPressed(object sender, EventArgs args)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			ScreenManager.Instance.Game.Exit();
		}

		private void LoginButtonPressed(object sender, EventArgs args)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			if (_loginBox.Text.Length < 3)
			{
				MessageBox.Message = "Username is too short!\nPress Ok to continue";
				ScreenManager.Instance.SetActiveScreen(typeof (MessageBox));
			}
			else if (_passwordBox.Text.Length < 3)
			{
				MessageBox.Message = "Password is too short!\nPress Ok to continue";
				ScreenManager.Instance.SetActiveScreen(typeof (MessageBox));
			}
			else
			{
				Settings.Default.login = _loginBox.Text;
				Settings.Default.password = _passwordBox.Text;
				Settings.Default.Save();

				if (GameController.Instance.Login(_loginBox.Text, _passwordBox.Text).HasValue)
				{
					ScreenManager.Instance.SetActiveScreen(typeof (MainMenuScreen));
				}				
			}
		}

		private void NewAccountButtonPressed(object sender, EventArgs args)
		{
			Cue cue = soundBank.GetCue("RICOCHET");
			cue.Play();

			if (_loginBox.Text.Length < 3)
			{
				MessageBox.Message = "Username is too short!\nPress Ok to continue";
				ScreenManager.Instance.SetActiveScreen(typeof (MessageBox));
			}
			else if (_passwordBox.Text.Length < 3)
			{
				MessageBox.Message = "Password is too short!\nPress Ok to continue";
				ScreenManager.Instance.SetActiveScreen(typeof (MessageBox));
			}
			else
			{
				Settings.Default.login = _loginBox.Text;
				Settings.Default.password = _passwordBox.Text;
				Settings.Default.Save();

				if (GameController.Instance.Register(_loginBox.Text, _passwordBox.Text))
				{
					if (GameController.Instance.Login(_loginBox.Text, _passwordBox.Text).HasValue)
					{
						ScreenManager.Instance.SetActiveScreen(typeof (MainMenuScreen));
					}
				}
				else
				{
					MessageBox.Message = "Registration failed";
					ScreenManager.Instance.SetActiveScreen(typeof (MessageBox));
				}
			}			
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch = ScreenManager.Instance.SpriteBatch;

			_spriteBatch.Begin();
			_spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
			_spriteBatch.End();
		}
	}
}
