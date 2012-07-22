﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using Nuclex.UserInterface.Controls;

namespace SkyShoot.Game.Controls
{
	public abstract class Controller
	{
		//public delegate void EventHandler(object sender, EventArgs e);

        protected readonly InputManager InputManager;

        protected readonly IDictionary<Control, List<EventHandler>> Listeners = new Dictionary<Control, List<EventHandler>>();

        protected int Index = 0;

		protected Controller(InputManager inputManager)
		{
			InputManager = inputManager;
		}

		public abstract Vector2? RunVector { get; }

		public abstract Vector2 SightPosition { get; }

		public abstract ButtonState ShootButton { get; }

        protected GameScreen ActiveScreen
        {
            get { return ScreenManager.Instance.GetActiveScreen(); }
        }

        protected int Length
        {
            get { return Controls.Count; }
        }

        private Collection<Control> Controls
        {
            get
            {
                var controls = new Collection<Control>();
                foreach (Control control in ActiveScreen.Desktop.Children)
                {
                    if (control is IFocusable)
                    {
                        controls.Add(control);
                    }
                }
                return controls;
            }
        }

        public abstract void Update();

		public virtual void AddListener(Control control, EventHandler eventHandler)
		{
			List<EventHandler> currentListeners;

			if (!Listeners.TryGetValue(control, out currentListeners))
			{
				currentListeners = new List<EventHandler>();
				Listeners.Add(control, currentListeners);
			}

			currentListeners.Add(eventHandler);
		}

		public virtual void RemoveListener(Control control, EventHandler eventHandler)
		{
			if (Listeners.ContainsKey(control))
			{
				List<EventHandler> currentListeners = Listeners[control];
				currentListeners.Remove(eventHandler);
			}
		}

		protected void FocusChanged()
		{
			ActiveScreen.FocusedControl = Controls[Index];
		}

		protected void NotifyListeners(Control control)
		{
		    if (!Listeners.ContainsKey(control))
		        return;

		    foreach (var listener in Listeners[control])
			{
				listener(control, null);
			}
		}

		protected void NotifyListeners(int index)
		{
			NotifyListeners(Controls[index]);
		}
	}
}