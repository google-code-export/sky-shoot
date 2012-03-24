using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SlXnaApp1
{
	public partial class GamePage : PhoneApplicationPage
	{
		ContentManager _contentManager;
		readonly GameTimer _timer;
		SpriteBatch _spriteBatch;

		public GamePage()
		{
			InitializeComponent();

			// Get the content manager from the application
			var app = Application.Current as App;
			if (app != null) _contentManager = app.Content;

			// Create a timer for this page
			_timer = new GameTimer();
			_timer.UpdateInterval = TimeSpan.FromTicks(333333);
			_timer.Update += OnUpdate;
			_timer.Draw += OnDraw;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			// Set the sharing mode of the graphics device to turn on XNA rendering
			SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

			// TODO: use this.content to load your game content here

			// Start the timer
			_timer.Start();

			base.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			// Stop the timer
			_timer.Stop();

			// Set the sharing mode of the graphics device to turn off XNA rendering
			SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

			base.OnNavigatedFrom(e);
		}

		/// <summary>
		/// Allows the page to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		private void OnUpdate(object sender, GameTimerEventArgs e)
		{
			// TODO: Add your update logic here
		}

		/// <summary>
		/// Allows the page to draw itself.
		/// </summary>
		private void OnDraw(object sender, GameTimerEventArgs e)
		{
			SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
		}
	}
}