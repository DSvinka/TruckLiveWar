using Code.Data;
using Code.Interfaces.Factory;
using Code.Providers;
using UnityEngine;

namespace Code.Factory
{
    public sealed class PlayerFactory : IPlayerFactory
    {
        private readonly PlayerData m_playerData;
        private TransportData m_transportData;
        private Transform m_player;

        public PlayerFactory(PlayerData playerData)
        {
            m_playerData = playerData;
            m_transportData = playerData.Transport;
        }

        public void ChangePlayerCar(TransportData transportData)
        {
            m_transportData = transportData;
        }

        public Transform CreatePlayer()
        {
            var prefab = Object.Instantiate(m_playerData.PlayerPrefab.gameObject);
            m_player = prefab.transform;
            return prefab.transform;
        }

        public CarProvider CreateTransport()
        {
            var carProvider = Object.Instantiate(m_transportData.CarPrefab, m_player);
            carProvider.UnitData = m_playerData.Transport;
            return carProvider;
        }
    }
}