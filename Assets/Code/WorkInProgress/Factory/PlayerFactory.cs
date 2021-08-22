using Cinemachine;
using Code.Data;
using Code.Interfaces.Factory;
using Code.Providers;
using UnityEngine;

namespace Code.Factory
{
    public sealed class PlayerFactory : IPlayerFactory
    {
        private readonly PlayerData _playerData;

        public PlayerFactory(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public Transform CreatePlayer()
        {
            var prefab = Object.Instantiate(_playerData.PlayerPrefab.gameObject);
            _playerData.CinemachineCamera = prefab.GetComponentInChildren<CinemachineFreeLook>();
            return prefab.transform;
        }

        public CarProvider CreateTransport()
        {
            var carProvider = Object.Instantiate(_playerData.Car.CarProvider);
            _playerData.CinemachineCamera.Follow = carProvider.CameraFollow;
            _playerData.CinemachineCamera.LookAt = carProvider.CameraLookAt;
            return carProvider;
        }
    }
}