using Cinemachine;
using Code.Data;
using Code.Interfaces.Factory;
using Code.Providers;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class PlayerFactory : IPlayerFactory
    {
        private readonly PlayerData m_playerData;
        private Transform m_player;

        public PlayerFactory(PlayerData playerData)
        {
            m_playerData = playerData;
        }

        public Transform CreatePlayer()
        {
            var prefab = Object.Instantiate(m_playerData.PlayerPrefab.gameObject);
            m_player = prefab.transform;
            return prefab.transform;
        }

        public CarProvider CreateTransport()
        {
            var carProvider = Object.Instantiate(m_playerData.Car.CarProvider, m_player);
            carProvider.UnitData = m_playerData.Car;
            return carProvider;
        }
    }
}