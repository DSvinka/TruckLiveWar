using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;
using Code.UserInput;

namespace Code.Controller.Initialization
{
    internal sealed class InputInitialization
    {
        // Axis
        private IUserAxisProxy m_axisHorizontal;
        private IUserAxisProxy m_axisVertical;
        // Keys
        private IUserKeyProxy m_inputHandbreak;
        private IUserKeyProxy m_inputRestart;
        private IUserKeyProxy m_inputHorn;
        private IUserKeyProxy m_inputEscape;
        // Mouse
        private IUserKeyProxy m_inputFireMouse;

        public InputInitialization()
        {
            // Axis
            m_axisHorizontal = new AxisHorizontal();
            m_axisVertical = new AxisVertical();
            
            // Keys
            m_inputHandbreak = new InputHandbreak();
            m_inputRestart = new InputRestart();
            m_inputHorn = new InputHorn();
            m_inputEscape = new InputEscape();
            
            // Mouse
            m_inputFireMouse = new InputFireMouse();
        }

        public (IUserAxisProxy inputHorizontal, IUserAxisProxy inputVertical) GetAxisInput()
        {
            return (m_axisHorizontal, m_axisVertical);
        }

        public (IUserKeyProxy inputHandbreak, IUserKeyProxy m_inputRestart, IUserKeyProxy m_inputHorn, IUserKeyProxy m_inputEscape) GetKeysInput()
        {
            return (m_inputHandbreak, m_inputRestart, m_inputHorn, m_inputEscape);
        }

        public IUserKeyProxy GetMouseInput()
        {
            return (m_inputFireMouse);
        }
    }
}