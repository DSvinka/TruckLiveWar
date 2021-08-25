using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Interfaces;
using Code.Providers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controller
{
    internal class PickupController : IController, IInitialization, ICleanup
    {
        private readonly PickupProvider[] _pickupProviders;
        private readonly CarController _playerCarController;
        private readonly WeaponsController _weaponsController;

        public PickupController(PickupProvider[] pickupProviders, CarController CarController, WeaponsController weaponsController)
        {
            _pickupProviders = pickupProviders;
            _playerCarController = CarController;
            _weaponsController = weaponsController;
        }

        public void Initialization()
        {
            foreach (var pickupProvider in _pickupProviders)
            {
                pickupProvider.OnTriggerEnterChange += OnTriggerEnter;
            }
        }
        
        private void OnTriggerEnter(GameObject gameObject, PickupProvider pickupProvider)
        {
            var carProvider = gameObject.GetComponentInParent<CarProvider>();
            if (_playerCarController.CarProvider.gameObject.GetInstanceID() == carProvider.gameObject.GetInstanceID())
            {
                var weaponPlace = new Weapon(pickupProvider.WeaponData);
                _weaponsController.AddWeapon(0, weaponPlace);
                // TODO: Добавить выбор куда установить.

                Destroy(pickupProvider);
            }
        }
        
        public void Cleanup()
        {
            foreach (var pickupProvider in _pickupProviders)
            {
                pickupProvider.OnTriggerEnterChange -= OnTriggerEnter;
            }
        }
        
        private void Destroy(PickupProvider pickupProvider)
        {
            pickupProvider.OnTriggerEnterChange -= OnTriggerEnter;
            Object.Destroy(pickupProvider.gameObject);
        }
    }
}