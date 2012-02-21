using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nuclex.Input;
using Nuclex.UserInterface.Controls;

namespace SkyShoot.Game.Controls
{
	public abstract class Controller
	{
		public delegate void ButtonPressed(object sender, EventArgs e);

		protected GameScreen ActiveScreen
		{
			get { return ScreenManager.Instance.CurrentScreen; }
		}

		protected int Length
		{
			get { return ActiveScreen.Desktop.Children.Count; }
		}

		protected int Index = 0;

		protected readonly InputManager InputManager;

		protected IDictionary<Control, List<ButtonPressed>> Listeners = new Dictionary<Control, List<ButtonPressed>>();   

		protected Controller(InputManager inputManager)
		{
			InputManager = inputManager;
		}

		public abstract void Update();

		public abstract Vector2? RunVector { get; }

		public abstract Vector2 SightPosition { get; }

		public abstract ButtonState ShootButton { get; }

		public virtual void RegisterListener(Control control, ButtonPressed buttonPressed)
		{
			List<ButtonPressed> currentListeners;

			if (!Listeners.TryGetValue(control, out currentListeners))
			{
				currentListeners = new List<ButtonPressed>();
				Listeners.Add(control, currentListeners);
			}

			currentListeners.Add(buttonPressed);
		}

		protected void FocusChanged()
		{
			ActiveScreen.FocusedControl = ActiveScreen.Desktop.Children[Index];
		}

		protected void NotifyListeners(Control control)
		{
			if(!Listeners.ContainsKey(control))
				return;

			foreach (var listener in Listeners[control])
			{
				listener(control, null);
			}
		}

		protected void NotifyListeners(int index)
		{
			NotifyListeners(ActiveScreen.Desktop.Children[index]);
		}
	}
}
