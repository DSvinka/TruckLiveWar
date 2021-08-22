using System;
using Code.Interfaces.UserInput;
using Code.Managers;
using UnityEngine;

namespace Code.UserInput
{
    internal class InputHandbreak: IUserKeyProxy
    {
        public event Action<bool> KeyOnChange = delegate(bool f) {  };
        
        public void GetKey()
        {
            KeyOnChange.Invoke(Input.GetKey(AxisManager.HANDBRAKE));
        }
    }
}