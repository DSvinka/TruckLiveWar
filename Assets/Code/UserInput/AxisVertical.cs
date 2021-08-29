using System;
using Code.Interfaces.Input;
using Code.Managers;
using UnityEngine;

namespace Code.UserInput
{
    public sealed class AxisVertical : IUserAxisProxy
    {
        public event Action<float> AxisOnChange = delegate(float f) {  };

        public void GetAxis()
        {
            AxisOnChange.Invoke(Input.GetAxis(AxisManager.VERTICAL));
        }
    }
}