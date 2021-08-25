using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;
using Code.UserInput;

namespace Code.Controller.Initialization
{
    internal sealed class InputInitialization
    {
        // Axis
        private IUserAxisProxy _axisHorizontal;
        private IUserAxisProxy _axisVertical;
        // Keys
        private IUserKeyProxy _inputHandbreak;
        private IUserKeyProxy _inputRestart;
        // Mouse
        private IUserKeyProxy _inputFireMouse;

        public InputInitialization()
        {
            // Axis
            _axisHorizontal = new AxisHorizontal();
            _axisVertical = new AxisVertical();
            
            // Keys
            _inputHandbreak = new InputHandbreak();
            _inputRestart = new InputRestart();
            
            // Mouse
            _inputFireMouse = new InputFireMouse();
        }
        
        public (IUserAxisProxy inputHorizontal, IUserAxisProxy inputVertical) GetAxisInput()
        {
            return (_axisHorizontal, _axisVertical);
        }
        
        public (IUserKeyProxy inputHandbreak, IUserKeyProxy _inputRestart) GetKeysInput()
        {
            return (_inputHandbreak, _inputRestart);
        }

        public IUserKeyProxy GetMouseInput()
        {
            return (_inputFireMouse);
        }
    }
}