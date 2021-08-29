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

        public PlayerFactory(PlayerData playerData)
        {
            m_playerData = playerData;
        }

        public Transform CreatePlayer()
        {
            var prefab = Object.Instantiate(m_playerData.PlayerPrefab.gameObject);
            return prefab.transform;
        }

        public CarProvider CreateTransport()
        {
            var carProvider = Object.Instantiate(m_playerData.Car.CarProvider);
            carProvider.UnitData = m_playerData.Car;
            return carProvider;
        }
    }
}