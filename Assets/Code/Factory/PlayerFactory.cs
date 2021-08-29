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
            m_playerData.CinemachineCamera = prefab.GetComponentInChildren<CinemachineFreeLook>();
            m_playerData.Camera = prefab.GetComponentInChildren<Camera>();
            return prefab.transform;
        }

        public CarProvider CreateTransport()
        {
            var carProvider = Object.Instantiate(m_playerData.Car.CarProvider);
            m_playerData.CinemachineCamera.Follow = carProvider.CameraFollow;
            m_playerData.CinemachineCamera.LookAt = carProvider.CameraLookAt;
            return carProvider;
        }
    }
}