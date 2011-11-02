using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SkyShoot.Game.ScreenManager
{
    public abstract class GameScreen
    {
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;

        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        TimeSpan transitionOnTime = TimeSpan.Zero;

        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        TimeSpan transitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// Текущее состояние перехода. 0 - Экран активен, 1 - неактивен.
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        float transitionPosition = 1;

        public float TransitionAlpha
        {
            get { return 1f - transitionPosition; }
        }

        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransitionOn;

        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting = false;

        public bool otherScreenHasFocus;

        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                        (screenState == ScreenState.Active ||
                         screenState == ScreenState.TransitionOn);
            }
        }

        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                screenState = ScreenState.TransitionOff;
                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {             
                    screenState = ScreenState.TransitionOff;
                }
                else
                {                
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    screenState = ScreenState.Active;
                }
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
            transitionPosition += transitionDelta * direction;

            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
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
                isExiting = true;
            }
        }
    }
}
