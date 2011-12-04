using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace SkyShoot.Game.Client.View
{
    public class Animation2D
    {
        // The collection of images used for animation
        private readonly List<Texture2D> _animationTextures = new List<Texture2D>();

        // The time since we last updated the frame
        private int _elapsedTime;

        // The time we display a frame until the next one
        private int _frameTime;

        // The number of frames that the animation contains
        private int FrameCount
        {
            get { return _animationTextures.Count; }
        }

        // The index of the current frame we are displaying
        private int _currentFrame;

        // The state of the Animation
        public bool Active;

        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        public Texture2D CurrentTexture
        {
            get { return _animationTextures[_currentFrame]; }
        }

        public void Initialize(int frameTime, bool looping)
        {
            _frameTime = frameTime;
            Looping = looping;

            // Set the time to zero
            _elapsedTime = 0;
            _currentFrame = 0;

            // Set the Animation to active by default
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the animation if we are not active
            if (Active == false)
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
                    if (Looping == false)
                        Active = false;
                }

                // Reset the elapsed time to zero
                _elapsedTime = 0;
            }
        }

        public void AddFrame(Texture2D texture)
        {
            _animationTextures.Add(texture);
        }

    }
}
