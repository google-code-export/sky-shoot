using System;

namespace SkyShoot.Game.Screens.Controls
{
    class InputControl:Nuclex.UserInterface.Controls.Desktop.InputControl
    {
        protected override void OnCharacterEntered(char character)
        {
            if (Char.IsLetter(character) || Char.IsDigit(character) || (character == '_'))
            {
                Text += character;
                CaretPosition += 1;
            }
        }
    }
}
