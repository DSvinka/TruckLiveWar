using UnityEngine;
using UnityEngine.UI;

namespace Code.Providers
{
    public class EscapeMenuProvider : MonoBehaviour
    {
        [SerializeField] private Button m_restartButton;
        [SerializeField] private Button m_saveButton;
        [SerializeField] private Button m_loadButton;
        [SerializeField] private Button m_exitButton;

        public Button RestartButton => m_restartButton;
        public Button SaveButton => m_saveButton;
        public Button LoadButton => m_loadButton;
        public Button ExitButton => m_exitButton;
    }
}
