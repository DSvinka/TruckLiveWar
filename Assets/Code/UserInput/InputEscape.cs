using System;
using Code.Interfaces.Input;
using Code.Managers;
using UnityEngine;

namespace Code.UserInput
{
    internal sealed class InputEscape : IUserKeyDownProxy
    {
        public event Action<bool> KeyOnDown = delegate(bool f) { };

        public void GetKeyDown()
        {
            KeyOnDown.Invoke(Input.GetKeyDown(ButtonsManager.ESCAPE_BUTTON));
        }
    }
}