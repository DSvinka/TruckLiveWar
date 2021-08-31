using System.Runtime.CompilerServices;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;

namespace Code.Controller
{
    internal sealed class InputController : IExecute
    {
        private readonly IUserAxisProxy m_horizontal;
        private readonly IUserAxisProxy m_vertical;
        private readonly IUserKeyProxy m_handbreak;
        private readonly IUserKeyProxy m_restart;
        private readonly IUserKeyProxy m_horn;
        private readonly IUserKeyProxy m_escape;
        private readonly IUserKeyProxy m_fireButton;

        internal InputController(
            (IUserAxisProxy inputHorizontal, IUserAxisProxy inputVertical) axisInput,
            (IUserKeyProxy inputHandbreak, IUserKeyProxy inputRestart, IUserKeyProxy inputHorn, IUserKeyProxy inputEscape) keysInput,
            IUserKeyProxy mouseInput
        )
        {
            m_horizontal = axisInput.inputHorizontal;
            m_vertical = axisInput.inputVertical;

            m_handbreak = keysInput.inputHandbreak;
            m_restart = keysInput.inputRestart;
            m_horn = keysInput.inputHorn;
            m_escape = keysInput.inputEscape;
            
            m_fireButton = mouseInput;
        }

        public void Execute(float deltaTime)
        {
            m_horizontal.GetAxis();
            m_vertical.GetAxis();
            m_handbreak.GetKey();
            m_restart.GetKey();
            m_horn.GetKey();
            m_escape.GetKey();
            
            m_fireButton.GetKey();
        }
    }
}