using System;

namespace SkyShoot.Game.Controls
{
	internal class InputControl : Nuclex.UserInterface.Controls.Desktop.InputControl
	{
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
			if (Char.IsLetter(character) || Char.IsDigit(character) || (character == '_'))
			{
				RealText += character;
				if (IsHidden) Text += "*";
				else Text += character;
				CaretPosition += 1;
			}
		}
	}
}
