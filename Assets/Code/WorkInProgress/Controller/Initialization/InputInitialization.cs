using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;
using Code.UserInput;

namespace Code.Controller.Initialization
{
    internal sealed class InputInitialization : IInitialization
    {
        private IUserInputProxy _inputHorizontal;
        private IUserInputProxy _inputVertical;
        private IUserKeyProxy _inputHandbreak;

        public void Initialization()
        {
            _inputHorizontal = new InputHorizontal();
            _inputVertical = new InputVertical();
            _inputHandbreak = new InputHandbreak();
        }

        public (IUserInputProxy inputHorizontal, IUserInputProxy inputVertical, IUserKeyProxy inputHandbreak) GetInput()
        {
            (IUserInputProxy inputHorizontal, IUserInputProxy inputVertical, IUserKeyProxy inputHandbreak) result = (_inputHorizontal, _inputVertical, _inputHandbreak);
            return result;
        }
    }
}