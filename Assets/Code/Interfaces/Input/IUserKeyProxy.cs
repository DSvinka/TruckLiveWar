using System;

namespace Code.Interfaces.Input
{
    public interface IUserKeyProxy
    {
        event Action<bool> KeyOnChange;
        void GetKey();
    }
}