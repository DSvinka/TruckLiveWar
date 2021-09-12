using Code.UserInput;
using Code.UserInput.Inputs;

namespace Code.Controller.Initialization
{
    internal sealed class InputInitialization
    {
        public InputInitialization()
        {
            var axisHorizontal = new AxisHorizontal();
            var axisVertical = new AxisVertical();
            var axisInput = new AxisInput(axisHorizontal, axisVertical);
            
            var inputHandbreak = new InputHandbreak();
            var inputRestart = new InputRestart();
            var inputEscape = new InputEscape();
            var inputHorn = new InputHorn();
            var keysInput = new KeysInput(inputHandbreak, inputRestart, inputEscape, inputHorn);
            
            var inputShot = new InputFireMouse();
            var mouseInput = new MouseInput(inputShot);
        }
    }
}