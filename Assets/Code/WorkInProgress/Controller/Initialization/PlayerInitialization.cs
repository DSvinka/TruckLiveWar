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
        private IPlayerFactory _playerFactory;
        private Transform _player;
        private CarProvider _playerCar;

        private Vector3 _playerSpawnPosition;

        public PlayerInitialization(PlayerFactory playerFactory, Vector3 playerSpawnPosition)
        {
            _playerFactory = playerFactory;
            _playerSpawnPosition = playerSpawnPosition;
        }

        public void Initialization()
        {
            _player = _playerFactory.CreatePlayer();
            _playerCar = _playerFactory.CreateTransport();
            
            _player.position = _playerSpawnPosition;

            _playerCar.transform.localPosition = Vector3.zero;
        }

        public Transform GetPlayer()
        {
            return _player;
        }
        
        public CarProvider GetPlayerTransport()
        {
            return _playerCar;
        }
    }
}