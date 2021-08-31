using System;
using Code.Interfaces.UserInput;
using Code.Managers;
using UnityEngine;

namespace Code.UserInput
{
    internal sealed class InputEscape : IUserKeyProxy
    {
        public event Action<bool> KeyOnChange = delegate(bool f) { };

        public void GetKey()
        {
            // TODO: Испарвить этот костыль
            KeyOnChange.Invoke(Input.GetKeyDown(ButtonsManager.ESCAPE_BUTTON));
        }
    }
}