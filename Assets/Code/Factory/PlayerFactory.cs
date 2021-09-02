using Code.Data;
using Code.Interfaces.Factory;
using Code.Providers;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class PlayerFactory : IPlayerFactory
    {
        private readonly PlayerData m_playerData;
        private CarData m_carData;
        private Transform m_player;

        public PlayerFactory(PlayerData playerData)
        {
            m_playerData = playerData;
            m_carData = playerData.Car;
        }

        public void ChangePlayerCar(CarData carData)
        {
            m_carData = carData;
        }

        public Transform CreatePlayer()
        {
            var prefab = Object.Instantiate(m_playerData.PlayerPrefab.gameObject);
            m_player = prefab.transform;
            return prefab.transform;
        }

        public CarProvider CreateTransport()
        {
            var carProvider = Object.Instantiate(m_carData.CarPrefab, m_player);
            carProvider.UnitData = m_playerData.Car;
            return carProvider;
        }
    }
}