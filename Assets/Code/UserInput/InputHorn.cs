using System;
using Code.Interfaces.Input;
using Code.Managers;
using UnityEngine;

namespace Code.UserInput
{
    internal sealed class InputHorn : IUserKeyProxy
    {
        public event Action<bool> KeyOnChange = delegate(bool f) {  };

        public void GetKey()
        {
            KeyOnChange.Invoke(Input.GetKey(ButtonsManager.HORN_BUTTON));
        }
    }
}