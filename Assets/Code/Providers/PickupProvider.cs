using System;
using Code.Data;
using Code.Interfaces.Providers;
using UnityEngine;

namespace Code.Providers
{
    internal sealed class PickupProvider : MonoBehaviour, IPickupProvider
    {
        public event Action<GameObject, PickupProvider> OnTriggerEnterChange = delegate(GameObject gameObject, PickupProvider pickupProvider) {  };

        [SerializeField] private WeaponData m_weaponData;

        public WeaponData WeaponData => m_weaponData;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterChange.Invoke(other.gameObject, this);
        }
    }
}