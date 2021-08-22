using Code.Client;
using Code.Entities.Base;
using Code.Guns.Base;
using UnityEngine;

namespace Code.Transports.Base
{
    internal sealed class Car : Entity
    {
        [Header("Объекты")]
        [SerializeField] private GunSlot[] _slots;

        [Header("Настройка Камеры")]
        [SerializeField] private Transform _cameraLookAt;
        [SerializeField] private Transform _cameraFollow;
        
        private CarController _carController;
        private Player _player;

        public Player Player
        {
            get => _player;
            set => _player = value;
        }
        public CarController CarController => _carController;
        public Transform CameraLookAt => _cameraLookAt;
        public Transform CameraFollow => _cameraFollow;

        private void Awake()
        {
            _carController = GetComponent<CarController>();
        }
        
        protected override void Death()
        {
            base.Death();
            _player.Death();
            Destroy(gameObject); // TODO: СДЕЛАТЬ ЭФФЕКТ ВЗРЫВА
        }

        public void PlaceGun(int slotIndex, Gun gun)
        {
            var slot = _slots[slotIndex];
            if (slot.PlacedGun)
                slot.RemoveGun();
            else
                slot.PlaceGun(_player, gun);
        }

        public void RemoveGun(int slotIndex)
        {
            var slot = _slots[slotIndex];
            slot.RemoveGun();
        }
    }
}
