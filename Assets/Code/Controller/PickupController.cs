using Code.Interfaces;
using Code.Providers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controller
{
    internal sealed class PickupController : IController, IInitialization, ICleanup
    {
        private static PickupProvider[] m_pickupProviders;
        private readonly CarController m_playerCarController;
        private readonly WeaponsController m_weaponsController;

        public static PickupProvider[] PickupProviders => m_pickupProviders;

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
                m_weaponsController.SetWeapon(0, weaponPlace);
                // TODO: Добавить выбор куда установить.
                
                pickupProvider.Parent.gameObject.SetActive(false);
            }
        }

        public void Cleanup()
        {
            foreach (var pickupProvider in m_pickupProviders)
            {
                pickupProvider.OnTriggerEnterChange -= OnTriggerEnter;
            }
        }
    }
}