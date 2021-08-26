using System;
using Code.Interfaces.UserInput;
using Code.Managers;
using UnityEngine;

namespace Code.UserInput
{
    internal sealed class InputFireMouse: IUserKeyProxy
    {
        public event Action<bool> KeyOnChange = delegate(bool f) {  };

        public void GetKey()
        {
            KeyOnChange.Invoke(Input.GetMouseButton(ButtonsManager.MOUSE_FIRE_BUTTON));
        }
    }
}