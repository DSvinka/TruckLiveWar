using System;
using Client.Interfaces.Hud.Components;
using Entities.Base;
using Transports.Base;
using UnityEngine;

namespace Client.Interfaces.HUD
{
    internal sealed class Hud : MonoBehaviour
    {
        [SerializeField] private EnemyInfo _enemyInfo;
        [SerializeField] private Bonuses _bonuses;
        [SerializeField] private Player.Player _player;
        [SerializeField] private Camera _camera;
        
        [SerializeField] private float _maxRaycastDistance = 100f;
        private bool _initialized;
        
        public EnemyInfo EnemyInfo => _enemyInfo;
        public Bonuses Bonuses => _bonuses;

        public void Init()
        {
            _enemyInfo = gameObject.GetComponentInChildren<EnemyInfo>();
            _bonuses = gameObject.GetComponentInChildren<Bonuses>();
            _player = gameObject.GetComponentInParent<Player.Player>();
            _camera = _player.Camera;
            
            _player.Car.CarController.PickupBonusEvent += OnBonus;
            _player.Car.CarController.TrapActivateEvent += OnTrap;

            _initialized = true;
        }

        private void Update()
        {
            if (_initialized)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastHit, _maxRaycastDistance))
                {
                    var entity = raycastHit.collider.gameObject.GetComponent<Entity>();
                    if (entity && _player.gameObject.GetInstanceID() != entity.gameObject.GetInstanceID())
                        _enemyInfo.SetEntity(entity);
                    else
                        _enemyInfo.DelEntity();
                }
            }
        }

        private void OnBonus(float time, PickupBonusType pickupBonusType, string bonusName)
        {
            Color color;
            switch (pickupBonusType)
            {
                case PickupBonusType.SpeedBonus:
                    color = Color.cyan;
                    break;
                case PickupBonusType.GunBox:
                    color = Color.green;
                    break;
                default:
                    color = Color.magenta;
                    break;
            }

            string text;
            string textTime = time == 0 ? "Бесконечно" : $"{time} секунд";
            text = $"{textTime} - {bonusName}";

            _bonuses.CreateMessage(text, color, time);
        }
        
        private void OnTrap(float time, TrapType trapType, string trapName)
        {
            Color color;
            switch (trapType)
            {
                case TrapType.DeathTrap:
                    color = Color.red;
                    break;
                case TrapType.SlowingTrap:
                    color = Color.yellow;
                    break;
                default:
                    color = Color.magenta;
                    break;
            }

            string text;
            string textTime = time == 0 ? "Бесконечно" : $"{time} секунд";
            text = $"{textTime} - {trapName}";

            _bonuses.CreateMessage(text, color, time);
        }
    }
}