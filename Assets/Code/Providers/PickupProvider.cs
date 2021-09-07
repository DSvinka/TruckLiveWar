using System;
using Code.Data;
using Code.Interfaces.Providers;
using UnityEngine;

namespace Code.Providers
{
    internal sealed class PickupProvider : MonoBehaviour, IPickupProvider
    {
        public event Action<GameObject, PickupProvider> OnTriggerEnterChange = delegate(GameObject gameObject, PickupProvider pickupProvider) {  };

        [SerializeField] [AssetPath.Attribute(typeof(WeaponData))] private string m_weaponDataPath;
        [SerializeField] private GameObject m_parent;


        private WeaponData m_weaponData;
        
        public GameObject Parent => m_parent;

        public WeaponData WeaponData => DataUtils.GetData(m_weaponDataPath, ref m_weaponData);

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterChange.Invoke(other.gameObject, this);
        }
    }
}