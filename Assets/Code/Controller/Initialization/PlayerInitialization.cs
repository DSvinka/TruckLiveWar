using System.Data;
using Cinemachine;
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
        
        public CinemachineFreeLook CinemachineCamera { get; private set; }
        public Camera Camera { get; private set; }

        public PlayerInitialization(PlayerFactory playerFactory, Vector3 playerSpawnPosition)
        {
            m_playerFactory = playerFactory;
            m_playerSpawnPosition = playerSpawnPosition;
        }

        public void Initialization()
        {
            m_player = m_playerFactory.CreatePlayer();
            m_playerCar = m_playerFactory.CreateTransport();
            
            CinemachineCamera = m_player.GetComponentInChildren<CinemachineFreeLook>();
            Camera = m_player.GetComponentInChildren<Camera>();
            
            CinemachineCamera.Follow = m_playerCar.CameraFollow;
            CinemachineCamera.LookAt = m_playerCar.CameraLookAt;

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