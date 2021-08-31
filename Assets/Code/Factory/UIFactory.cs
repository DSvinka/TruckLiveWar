using Code.Controller.Initialization;
using Code.Data;
using Code.Interfaces.Factory;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class UIFactory : IHudFactory
    {
        private readonly UIData m_UIData;
        private readonly PlayerInitialization m_playerInitialization;

        public UIFactory(UIData uiData, PlayerInitialization playerInitialization)
        {
            m_UIData = uiData;
            m_playerInitialization = playerInitialization;
        }

        public Transform CreateHud()
        {
            var prefab = Object.Instantiate(m_UIData.PlayerHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
        
        public Transform CreateWinWindow()
        {
            var prefab = Object.Instantiate(m_UIData.WinHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
        
        public Transform CreateDeathWindow()
        {
            var prefab = Object.Instantiate(m_UIData.DeathHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }

        public Transform CreateEscapeMenu()
        {
            var prefab = Object.Instantiate(m_UIData.EscapeMenuPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
    }
}