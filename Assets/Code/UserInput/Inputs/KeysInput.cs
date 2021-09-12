using Code.Interfaces.Input;

namespace Code.UserInput.Inputs
{
    internal class KeysInput
    {
        public static IUserKeyProxy Handbreak { get; private set; }
        public static IUserKeyProxy Restart { get; private set; }
        public static IUserKeyProxy Horn { get; private set; }
        public static IUserKeyDownProxy Escape { get; private set; }

        public KeysInput(IUserKeyProxy handbreak, IUserKeyProxy restart, IUserKeyDownProxy escape, IUserKeyProxy horn)
        {
            Handbreak = handbreak;
            Restart = restart;
            Escape = escape;
            Horn = horn;
        }
    }
}