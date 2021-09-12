using Code.Controller.Initialization;
using Code.Data.DataStores;
using Code.Interfaces.Factory;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class UIFactory : IHudFactory
    {
        private readonly UIDatas m_UIDatas;
        private readonly PlayerInitialization m_playerInitialization;

        public UIFactory(UIDatas uiDatas, PlayerInitialization playerInitialization)
        {
            m_UIDatas = uiDatas;
            m_playerInitialization = playerInitialization;
        }

        public Transform CreateHud()
        {
            var prefab = Object.Instantiate(m_UIDatas.PlayerHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
        
        public Transform CreateWinWindow()
        {
            var prefab = Object.Instantiate(m_UIDatas.WinHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
        
        public Transform CreateDeathWindow()
        {
            var prefab = Object.Instantiate(m_UIDatas.DeathHudPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }

        public Transform CreateEscapeMenu()
        {
            var prefab = Object.Instantiate(m_UIDatas.EscapeMenuPrefab, m_playerInitialization.GetPlayer());
            return prefab.transform;
        }
    }
}