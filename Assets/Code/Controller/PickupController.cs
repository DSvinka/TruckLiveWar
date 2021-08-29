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
    internal sealed class PickupController : IController, IInitialization, ICleanup
    {
        private readonly PickupProvider[] m_pickupProviders;
        private readonly CarController m_playerCarController;
        private readonly WeaponsController m_weaponsController;

        public PickupController(PickupProvider[] pickupProviders, CarController CarController, WeaponsController weaponsController)
        {
            m_pickupProviders = pickupProviders;
            m_playerCarController = CarController;
            m_weaponsController = weaponsController;
        }

        public void Initialization()
        {
            foreach (var pickupProvider in m_pickupProviders)
            {
                pickupProvider.OnTriggerEnterChange += OnTriggerEnter;
            }
        }

        private void OnTriggerEnter(GameObject gameObject, PickupProvider pickupProvider)
        {
            if (m_playerCarController.CarProvider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                var weaponPlace = new Weapon(pickupProvider.WeaponData);
                m_weaponsController.AddWeapon(0, weaponPlace);
                // TODO: Добавить выбор куда установить.

                Destroy(pickupProvider);
            }
        }

        public void Cleanup()
        {
            foreach (var pickupProvider in m_pickupProviders)
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