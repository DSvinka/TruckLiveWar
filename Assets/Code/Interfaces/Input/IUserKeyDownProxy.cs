using System;

namespace Code.Interfaces.Input
{
    public interface IUserKeyDownProxy
    {
        event Action<bool> KeyOnDown;
        void GetKeyDown();
    }
}