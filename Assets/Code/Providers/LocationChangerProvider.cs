using System;
using Code.Interfaces.Providers;
using UnityEngine;

namespace Code.Providers
{
    internal sealed class LocationChangerProvider : MonoBehaviour, ILocationChangerProvider
    {
        [SerializeField] private string m_locationIDName;
        public string LocationIDName => m_locationIDName;
        
        public event Action<GameObject, LocationChangerProvider> OnTriggerEnterChange = delegate(GameObject o, LocationChangerProvider provider) {  };

        public event Action<GameObject, LocationChangerProvider> OnTriggerExitChange = delegate(GameObject o, LocationChangerProvider provider) { };

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterChange.Invoke(other.gameObject, this);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitChange.Invoke(other.gameObject, this);
        }
    }
}