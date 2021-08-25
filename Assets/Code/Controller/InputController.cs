using System.Runtime.CompilerServices;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;

namespace Code.Controller
{
    public sealed class InputController : IExecute
    {
        private readonly IUserAxisProxy _horizontal;
        private readonly IUserAxisProxy _vertical;
        private readonly IUserKeyProxy _handbreak;
        private readonly IUserKeyProxy _restart;
        private readonly IUserKeyProxy _fireButton;
        
        public InputController(
            (IUserAxisProxy inputHorizontal, IUserAxisProxy inputVertical) axisInput, 
            (IUserKeyProxy inputHandbreak, IUserKeyProxy inputRestart) keysInput,
            IUserKeyProxy mouseInput 
        )
        {
            _horizontal = axisInput.inputHorizontal;
            _vertical = axisInput.inputVertical;
            
            _handbreak = keysInput.inputHandbreak;
            _restart = keysInput.inputRestart;
            
            _fireButton = mouseInput;
        }

        public void Execute(float deltaTime)
        {
            _horizontal.GetAxis();
            _vertical.GetAxis();
            _handbreak.GetKey();
            _restart.GetKey();
            _fireButton.GetKey();
        }
    }
}