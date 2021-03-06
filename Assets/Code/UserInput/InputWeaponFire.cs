using System;
using Code.Interfaces.Input;
using Code.Managers;
using UnityEngine;

namespace Code.UserInput
{
    internal sealed class InputWeaponFire: IUserKeyProxy
    {
        public event Action<bool> KeyOnChange = delegate(bool f) {  };

        public void GetKey()
        {
            KeyOnChange.Invoke(Input.GetKey(ButtonsManager.MOUSE_FIRE_BUTTON));
        }
    }
}