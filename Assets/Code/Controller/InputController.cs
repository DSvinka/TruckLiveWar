using Code.Interfaces;
using Code.UserInput.Inputs;

namespace Code.Controller
{
    internal sealed class InputController : IExecute
    {
        public void Execute(float deltaTime)
        {
            AxisInput.Horizontal.GetAxis();
            AxisInput.Vertical.GetAxis();
            
            KeysInput.Handbreak.GetKey();
            KeysInput.Restart.GetKey();
            KeysInput.Horn.GetKey();
            KeysInput.Escape.GetKeyDown();
            
            MouseInput.Shot.GetKey();
        }
    }
}