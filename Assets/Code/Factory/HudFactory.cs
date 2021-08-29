using Cinemachine;
using Code.Data;
using Code.Interfaces.Factory;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class HudFactory : IHudFactory
    {
        private readonly HudData m_hudData;

        public HudFactory(HudData hudData)
        {
            m_hudData = hudData;
        }

        public Transform CreateHud()
        {
            var prefab = Object.Instantiate(m_hudData.PlayerHudPrefab);
            return prefab.transform;
        }
        
        public Transform CreateWinWindow()
        {
            var prefab = Object.Instantiate(m_hudData.WinHudPrefab);
            return prefab.transform;
        }
        
        public Transform CreateDeathWindow()
        {
            var prefab = Object.Instantiate(m_hudData.DeathHudPrefab);
            return prefab.transform;
        }
    }
}