using System;
using Microsoft.Xna.Framework.Input;

namespace SkyShoot.Game.Controls
{
	internal class InputControl : Nuclex.UserInterface.Controls.Desktop.InputControl
	{
		KeyboardState _keyboardState;
		int n = Settings.Default.password.Length;

		public Boolean IsHidden
		{
			get;
			set;
		}

		public String RealText
		{
			get;
			set;
		}

		public static string HiddenText (string text)
		{			
			string _newString = "";
			for (int i = 1; i <= text.Length; i++) _newString += "*";
			return _newString;
		}

		protected override void OnCharacterEntered(char character)
		{
			if (character == '\b')
			{
				RealText = RealText.Substring(0, n - 1);
				n--;
			}
			else
				if (Char.IsLetter(character) || Char.IsDigit(character) || (character == '_'))
				{
					n++;
					RealText += character;
					if (IsHidden) Text += "*";
					else Text += character;
					CaretPosition += 1;
				}
		}

		
	}
}
