using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;

namespace Code.Controller
{
    public sealed class InputController : IExecute
    {
        private readonly IUserInputProxy _horizontal;
        private readonly IUserInputProxy _vertical;
        private readonly IUserKeyProxy _handbreak;
        
        public InputController((IUserInputProxy inputHorizontal, IUserInputProxy inputVertical, IUserKeyProxy inputHandbreak) input)
        {
            _horizontal = input.inputHorizontal;
            _vertical = input.inputVertical;
            _handbreak = input.inputHandbreak;
        }

        public void Execute(float deltaTime)
        {
            _horizontal.GetAxis();
            _vertical.GetAxis();
            _handbreak.GetKey();
        }
    }
}