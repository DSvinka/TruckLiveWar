using System.Data;
using Cinemachine;
using Code.Data;
using Code.Factory;
using Code.Interfaces;
using Code.Interfaces.Factory;
using Code.Providers;
using Code.Utils.Extensions;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

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
            
            Camera = m_player.GetComponentInChildren<Camera>();
            CinemachineCamera = m_player.GetComponentInChildren<CinemachineFreeLook>();
            
            InitCar();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            m_player.position = m_playerSpawnPosition;
            m_playerCar.transform.position = m_playerSpawnPosition;
        }

        private void InitCar(CarData carData = null)
        {
            if (carData != null) 
                m_playerFactory.ChangePlayerCar(carData);
            m_playerCar = m_playerFactory.CreateTransport();

            CinemachineCamera.Follow = m_playerCar.CameraFollow;
            CinemachineCamera.LookAt = m_playerCar.CameraLookAt;
        }
        
        public void ChangeCar(CarData carData)
        {
            if (m_playerCar.gameObject != null)
                Object.Destroy(m_playerCar.gameObject);
            InitCar(carData);
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