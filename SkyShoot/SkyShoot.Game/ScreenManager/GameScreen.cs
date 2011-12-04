using System;

using Microsoft.Xna.Framework;

namespace SkyShoot.Game.ScreenManager
{
    public abstract class GameScreen
    {
        public bool IsPopup { get; protected set; }

        public TimeSpan TransitionOnTime
        {
            get { return _transitionOnTime; }
            protected set { _transitionOnTime = value; }
        }

        private TimeSpan _transitionOnTime = TimeSpan.Zero;

        public TimeSpan TransitionOffTime
        {
            get { return _transitionOffTime; }
            protected set { _transitionOffTime = value; }
        }

        private TimeSpan _transitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// Текущее состояние перехода. 0 - Экран активен, 1 - неактивен.
        /// </summary>
        public float TransitionPosition
        {
            get { return _transitionPosition; }
            protected set { _transitionPosition = value; }
        }

        private float _transitionPosition = 1;

        public float TransitionAlpha
        {
            get { return 1f - _transitionPosition; }
        }

        public ScreenState ScreenState
        {
            get { return _screenState; }
            protected set { _screenState = value; }
        }

        private ScreenState _screenState = ScreenState.TransitionOn;

        public bool IsExiting
        {
            get { return _isExiting; }
            protected internal set { _isExiting = value; }
        }

        private bool _isExiting;

        public bool OtherScreenHasFocus;

        public bool IsActive
        {
            get
            {
                return !OtherScreenHasFocus &&
                        (_screenState == ScreenState.Active ||
                         _screenState == ScreenState.TransitionOn);
            }
        }

        public ScreenManager ScreenManager { get; internal set; }

        protected GameScreen()
        {
            IsPopup = false;
        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime, bool otherHasFocus,
                                                      bool coveredByOtherScreen)
        {
            OtherScreenHasFocus = otherHasFocus;

            if (_isExiting)
            {
                _screenState = ScreenState.TransitionOff;
                if (!UpdateTransition(gameTime, _transitionOffTime, 1))
                {
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                _screenState = UpdateTransition(gameTime, _transitionOffTime, 1) ? ScreenState.TransitionOff : ScreenState.Hidden;
            }
            else
            {
                _screenState = UpdateTransition(gameTime, _transitionOnTime, -1) ? ScreenState.TransitionOn : ScreenState.Active;
            }
        }

        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;
            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);
            _transitionPosition += transitionDelta * direction;

            if (((direction < 0) && (_transitionPosition <= 0)) ||
                ((direction > 0) && (_transitionPosition >= 1)))
            {
                _transitionPosition = MathHelper.Clamp(_transitionPosition, 0, 1);
                return false;
            }
            
            return true;
        }

        public virtual void HandleInput(InputState input) { }

        public virtual void Draw(GameTime gameTime) { }

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                _isExiting = true;
            }
        }
    }
}
