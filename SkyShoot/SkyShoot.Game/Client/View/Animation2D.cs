using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Client.View
{
	public class Animation2D
	{
		/// <summary>
		/// The collection of images used for animation
		/// </summary>
		private readonly List<Texture2D> _animationTextures = new List<Texture2D>();

		/// <summary>
		/// The state of the Animation
		/// </summary>
		private bool _active;

		/// <summary>
		/// Determines if the animation will keep playing or deactivate after one run
		/// </summary>
		private bool _looping;

		/// <summary>
		/// The time since we last updated the frame
		/// </summary>
		private int _elapsedTime;

		/// <summary>
		/// // The time we display a frame until the next one
		/// </summary>
		private int _frameTime;

		/// <summary>
		/// The index of the current frame we are displaying
		/// </summary>
		private int _currentFrame;

		public Texture2D CurrentTexture
		{
			get { return _animationTextures[_currentFrame]; }
		}

		/// <summary>
		/// The number of frames that the animation contains
		/// </summary>
		private int FrameCount
		{
			get { return _animationTextures.Count; }
		}

		public void Initialize(int frameTime, bool looping)
		{
			_frameTime = frameTime;
			_looping = looping;

			// Set the time to zero
			_elapsedTime = 0;
			_currentFrame = 0;

			// Set the Animation to active by default
			_active = true;
		}

		public void Update(GameTime gameTime)
		{
			// Do not update the animation if we are not active
			if (_active == false)
				return;

			// Update the elapsed time
			_elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

			// If the elapsed time is larger than the frame time
			// we need to switch frames
			if (_elapsedTime > _frameTime)
			{
				// Move to the next frame
				_currentFrame++;

				// If the currentFrame is equal to frameCount reset currentFrame to zero
				if (_currentFrame == FrameCount)
				{
					_currentFrame = 0;

					// If we are not looping deactivate the animation
					if (_looping == false)
						_active = false;
				}

				// Reset the elapsed time to zero
				_elapsedTime = 0;
			}
		}

		public void AddFrame(Texture2D texture)
		{
			_animationTextures.Add(texture);
		}

		public void Clear()
		{
			_animationTextures.Clear();
		}
	}
}
