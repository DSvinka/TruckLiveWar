using System;
using Code.Client.Interfaces.Hud.Components;
using Code.Entities.Base;
using Code.Transports.Base;
using UnityEngine;

namespace Code.Client.Interfaces.HUD
{
    internal sealed class Hud : MonoBehaviour
    {
        [SerializeField] private EnemyInfo _enemyInfo;
        [SerializeField] private Bonuses _bonuses;
        [SerializeField] private Player _player;
        [SerializeField] private Camera _camera;
        
        [SerializeField] private float _maxRaycastDistance = 100f;
        private bool _initialized;
        
        public EnemyInfo EnemyInfo => _enemyInfo;
        public Bonuses Bonuses => _bonuses;

        public void Init()
        {
            _enemyInfo = gameObject.GetComponentInChildren<EnemyInfo>();
            _bonuses = gameObject.GetComponentInChildren<Bonuses>();
            _player = gameObject.GetComponentInParent<Player>();
            _camera = _player.Camera;
            
            _player.Bonuses.PickupEvent += OnPickup;

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

        private void OnPickup(int time, string pickupName, Color color)
        {
            _bonuses.CreateMessage(pickupName, time, time != 0, color);
        }
    }
}