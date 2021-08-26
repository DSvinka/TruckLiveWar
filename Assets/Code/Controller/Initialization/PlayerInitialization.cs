using System.Data;
using Code.Factory;
using Code.Interfaces;
using Code.Interfaces.Factory;
using Code.Providers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Controller.Initialization
{
    internal sealed class PlayerInitialization : IInitialization
    {
        private IPlayerFactory m_playerFactory;
        private Transform m_player;
        private CarProvider m_playerCar;

        private Vector3 m_playerSpawnPosition;

        public PlayerInitialization(PlayerFactory playerFactory, Vector3 playerSpawnPosition)
        {
            m_playerFactory = playerFactory;
            m_playerSpawnPosition = playerSpawnPosition;
        }

        public void Initialization()
        {
            m_player = m_playerFactory.CreatePlayer();
            m_playerCar = m_playerFactory.CreateTransport();

            m_player.position = m_playerSpawnPosition;
            m_playerCar.transform.position = m_playerSpawnPosition;
        }

        public Transform GetPlayer()
        {
            return m_player;
        }

        public CarProvider GetPlayerTransport()
        {
            return m_playerCar;
        }
    }
}