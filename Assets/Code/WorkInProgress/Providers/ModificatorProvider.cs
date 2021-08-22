using System;
using Code.Data;
using Code.Interfaces.Providers;
using UnityEngine;

namespace Code.Providers
{
    public sealed class ModificatorProvider: MonoBehaviour, IModificatorProvider
    {
        public event Action<int, ModificatorType> OnTriggerEnterChange = delegate(int gameObjectID, ModificatorType modificatorType) {  };
        public event Action<int, ModificatorType> OnTriggerExitChange = delegate(int gameObjectID, ModificatorType modificatorType) {  };

        [SerializeField] private ModificatorType _modificatorType;

        public ModificatorType ModificatorType => _modificatorType;
        
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterChange.Invoke(other.gameObject.GetInstanceID(), _modificatorType);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitChange.Invoke(other.gameObject.GetInstanceID(), _modificatorType);
        }
    }
}