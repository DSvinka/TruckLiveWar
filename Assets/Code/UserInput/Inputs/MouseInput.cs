using Code.Interfaces.Input;

namespace Code.UserInput.Inputs
{
    internal class MouseInput
    {
        public static IUserKeyProxy Shot { get; private set; }

        public MouseInput(IUserKeyProxy shot)
        {
            Shot = shot;
        }
    }
}