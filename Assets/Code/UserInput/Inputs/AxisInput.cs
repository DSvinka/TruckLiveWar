using Code.Interfaces.Input;

namespace Code.UserInput.Inputs
{
    internal class AxisInput
    {
        public static IUserAxisProxy Horizontal { get; private set; }
        public static IUserAxisProxy Vertical { get; private set; }

        public AxisInput(IUserAxisProxy horizontal, IUserAxisProxy vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }
}