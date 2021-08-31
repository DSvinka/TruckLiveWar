using Cinemachine;
using Code.Controller.Initialization;
using Code.Data;
using Code.Interfaces.Factory;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class HudFactory : IHudFactory
    {
        private readonly HudData m_hudData;
        private readonly PlayerInitialization m_playerInitialization;

        public HudFactory(HudData hudData, PlayerInitialization playerInitialization)
        {
            m_hudData = hudData;
            m_playerInitialization = playerInitialization;
        }

        public Transform CreateHud()
        {
            var prefab = Object.Instantiate(m_hudData.PlayerHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
        
        public Transform CreateWinWindow()
        {
            var prefab = Object.Instantiate(m_hudData.WinHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
        
        public Transform CreateDeathWindow()
        {
            var prefab = Object.Instantiate(m_hudData.DeathHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
    }
}