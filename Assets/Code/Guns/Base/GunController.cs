using System;
using Code.Entities.Base;
using Code.Entities.Enemy;
using Code.Client;
using UnityEngine;
using static UnityEngine.Debug;

namespace Code.Guns.Base
{
    [RequireComponent(typeof(Gun))]
    internal sealed class GunController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] [Tooltip("Режим отладки: показ луча, вывод информации о попадании")] 
        private bool _debug;
        [SerializeField] [Tooltip("Режим отладки: Сколько времени луч будет виден")] 
        private float _debugRayTime;
        
        [Header("Элементы оружия")]
        [SerializeField] [Tooltip("Оружие")] 
        private Transform _gun;
        
        [SerializeField] [Tooltip("Ствол")] 
        private Transform _barrel;
    
        [SerializeField] [Tooltip("Стойка на которой стоит оружие")] 
        private Transform _handle;

        private Gun _gunInfo;
        private float _shotCooldown;
        
        public Player Player { get => _player; set => _player = value; }

        private void Awake()
        {
            _gunInfo = GetComponent<Gun>();
        }
        private void Update()
        {
            Move();
            Shot();
        }

        private void Move()
        {
            var gunRotation = _gun.rotation;
            gunRotation = Quaternion.Lerp(gunRotation, _player.Camera.transform.rotation, _gunInfo.TurnSpeed);
            _gun.rotation = gunRotation;

            var handleRotation = _handle.rotation;
            handleRotation.Set(handleRotation.x, gunRotation.y, handleRotation.z, handleRotation.w); 
        }
        
        private void Shot()
        {
            if (Input.GetMouseButton((int) _player.ShotButton) && _shotCooldown <= 0)
            {
                RaycastHit raycastHit;
                Vector3 barrelPosition = _barrel.transform.position;
                Vector3 barrelForward = _barrel.transform.forward;
                
                if (Physics.Raycast(barrelPosition, barrelForward, out raycastHit, _gunInfo.MaxDistance))
                {
                    var entity = raycastHit.collider.gameObject.GetComponent<Entity>();
                    if (entity)
                    {
                        entity.AddDamage(_gunInfo.Damage);
                        
                        if (_debug)
                        {
                            Log($"<color=green>[Отладка Оружия]</color> Damage - {raycastHit.collider.gameObject.name} [HP: {entity.Health}]");
                        }
                    }

                    if (_debug)
                    {
                        Log($"<color=green>[Отладка Оружия]</color> Hit - {raycastHit.collider.gameObject.name}");
                    }
                }
                
                if (_debug)
                {
                    DrawRay(barrelPosition, barrelForward * _gunInfo.MaxDistance, Color.green, _debugRayTime);
                }

                _shotCooldown = _gunInfo.FireRate;
            }
            
            _shotCooldown -= Time.deltaTime;
        }
    }   
}
