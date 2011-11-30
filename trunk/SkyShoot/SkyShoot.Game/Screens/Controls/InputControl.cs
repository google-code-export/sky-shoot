using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.Game.Screens.Controls
{
    class InputControl:Nuclex.UserInterface.Controls.Desktop.InputControl
    {
        protected override void OnCharacterEntered(char character)
        {
            if (Char.IsLetter(character) || Char.IsDigit(character))
            {
                Text += character;
                CaretPosition += 1;
            }
        }
    }
}
