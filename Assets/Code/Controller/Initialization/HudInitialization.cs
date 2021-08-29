using Code.Factory;
using Code.Interfaces;
using Code.Interfaces.Factory;
using Code.Providers;
using UnityEngine;

namespace Code.Controller.Initialization
{
    internal sealed class HudInitialization : IInitialization
    {
        private readonly HudFactory m_hudFactory;
        
        private Transform m_playerHud;
        private Transform m_winHud;
        private Transform m_deathHud;

        public HudInitialization(HudFactory hudFactory)
        {
            m_hudFactory = hudFactory;
        }

        public void Initialization()
        {
            m_playerHud = m_hudFactory.CreateHud();

            m_winHud = m_hudFactory.CreateWinWindow();
            m_deathHud = m_hudFactory.CreateDeathWindow();

            m_playerHud.gameObject.SetActive(true);
            m_winHud.gameObject.SetActive(false);
            m_deathHud.gameObject.SetActive(false);
        }

        public void DisableAllHud()
        {
            m_playerHud.gameObject.SetActive(false);
            m_winHud.gameObject.SetActive(false);
            m_deathHud.gameObject.SetActive(false);
        }

        public Transform GetPlayerHud()
        {
            return m_playerHud;
        }

        public Transform GetWinHud()
        {
            return m_winHud;
        }

        public Transform GetDeathHud()
        {
            return m_deathHud;
        }
    }
}