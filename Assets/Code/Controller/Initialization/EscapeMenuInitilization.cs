using Code.Factory;
using Code.Interfaces;
using UnityEngine;

namespace Code.Controller.Initialization
{
    internal sealed class EscapeMenuInitilization : IInitialization
    {
        private readonly UIFactory m_uiFactory;
        private Transform m_escapeMenu;

        public EscapeMenuInitilization(UIFactory uiFactory)
        {
            m_uiFactory = uiFactory;
        }

        public void Initialization()
        {
            m_escapeMenu = m_uiFactory.CreateEscapeMenu();
            m_escapeMenu.gameObject.SetActive(false);
        }

        public Transform GetEscapeMenu()
        {
            return m_escapeMenu;
        }
    }
}