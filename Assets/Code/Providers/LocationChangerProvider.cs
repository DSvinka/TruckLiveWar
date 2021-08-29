using System;
using Code.Interfaces.Providers;
using UnityEngine;

// TODO: СДЕЛАТЬ СМЕНУ ЛОКАЦИИ... Только я не знаю как это грамотно реализовать...
namespace Code.Providers
{
    internal sealed class LocationChangerProvider : MonoBehaviour, ILocationChangerProvider
    {
        public event Action<GameObject, LocationChangerProvider> OnTriggerEnterChange = delegate(GameObject o, LocationChangerProvider provider) {  };
        public event Action<GameObject, LocationChangerProvider> OnTriggerExitChange = delegate(GameObject o, LocationChangerProvider provider) {  };
        
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