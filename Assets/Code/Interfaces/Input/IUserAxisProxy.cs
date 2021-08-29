using System;

namespace Code.Interfaces.Input
{
    public interface IUserAxisProxy
    {
        event Action<float> AxisOnChange;
        void GetAxis();
    }
}