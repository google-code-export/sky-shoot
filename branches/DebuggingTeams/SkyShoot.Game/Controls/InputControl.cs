using Microsoft.Xna.Framework.Input;

namespace SkyShoot.Game.Controls
{
	internal class InputControl : Nuclex.UserInterface.Controls.Desktop.InputControl
	{
		KeyboardState _keyboardState;
		private int n = Settings.Default.password.Length;

		public bool IsHidden
		{
			get;
			set;
		}

		public string RealText
		{
			get;
			set;
		}

	    public static string HiddenText(string text)
	    {
            return new string('*', text.Length);
		}

		protected override void OnCharacterEntered(char character)
		{
			if (character == '\b')
			{
				RealText = RealText.Substring(0, n - 1);
				n--;
			}
			else
				if (char.IsLetter(character) || char.IsDigit(character) || (character == '_'))
				{
					n++;
					RealText += character;
					Text += IsHidden ? '*' : character;
					CaretPosition++;
				}
		}
	}
}
