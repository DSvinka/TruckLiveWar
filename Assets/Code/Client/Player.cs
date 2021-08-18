using Cinemachine;
using Code.Client.Interfaces.HUD;
using Code.Transports.Base;
using UnityEngine;

namespace Code.Client
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
        [SerializeField] private PlayerBonuses _bonuses;
        [SerializeField] private CinemachineFreeLook _CinemachineCamera;

        [Header("HUD")]
        [SerializeField] private Hud _hud;
        [SerializeField] private GameObject _deathHudPrefab;
        
        [Header("Настройки")]
        [SerializeField] private MouseButtons _shotButton; // TODO: Сделать InputManager
        
        private Car _car;
        
        public Car Car => _car;
        public Camera Camera => _camera;
        public MouseButtons ShotButton => _shotButton;
        public PlayerBonuses Bonuses => _bonuses;
        public Hud Hud => _hud;

        private void Start()
        {
            var car = Instantiate(_carPrefab.gameObject, transform.transform);
            _car = car.GetComponent<Car>();
            _car.Player = this;

            _bonuses = GetComponent<PlayerBonuses>();
            _bonuses.Init();
            
            _hud = gameObject.GetComponentInChildren<Hud>();
            _hud.Init();

            _CinemachineCamera.Follow = _car.CameraFollow;
            _CinemachineCamera.LookAt = _car.CameraLookAt;
        }

        public void Death()
        {
            _hud.gameObject.SetActive(false);
            Instantiate(_deathHudPrefab);
        }
    }
}
