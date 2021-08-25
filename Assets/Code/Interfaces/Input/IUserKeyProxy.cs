using System;

namespace Code.Interfaces.UserInput
{
    public interface IUserKeyProxy
    {
        event Action<bool> KeyOnChange;
        void GetKey();
    }
}