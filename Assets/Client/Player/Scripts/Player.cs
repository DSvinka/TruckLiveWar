using Cinemachine;
using Entities.Base;
using Transports.Base;
using UnityEngine;

namespace Client.Player
{
    internal enum MouseButtons
    {
        Left = 0,
        Right = 1,
    }
    
    internal class Player : MonoBehaviour
    {
        [Header("Объекты")]
        [SerializeField] private Car _carPrefab;
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineFreeLook _CinemachineCamera;
        
        [Header("Настройки")]
        [SerializeField] private MouseButtons _shotButton; // TODO: Сделать InputManager
        
        private Car _car;
        
        public Car Car => _car;
        public Camera Camera => _camera;
        public MouseButtons ShotButton => _shotButton;

        private void Start()
        {
            var car = Instantiate(_carPrefab.gameObject, transform.transform);
            _car = car.GetComponent<Car>();
            _car.Player = this;

            _CinemachineCamera.Follow = _car.CameraFollow;
            _CinemachineCamera.LookAt = _car.CameraLookAt;
        }

        public void Death()
        {
            Debug.Log("Вы погибли"); // TODO: Заменить это на HUD
        }
    }
}
