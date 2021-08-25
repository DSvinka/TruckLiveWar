using System;
using Code.Data;
using Code.Interfaces.Providers;
using UnityEngine;

namespace Code.Providers
{
    internal sealed class ModificatorProvider: MonoBehaviour, IModificatorProvider
    {
        public event Action<GameObject, ModificatorProvider, ModificatorType> OnTriggerEnterChange = delegate(GameObject gameObject, ModificatorProvider modificatorProvider, ModificatorType modificatorType) {  };
        public event Action<GameObject, ModificatorProvider, ModificatorType> OnTriggerExitChange = delegate(GameObject gameObject, ModificatorProvider modificatorProvider, ModificatorType modificatorType) {  };

        [SerializeField] private ModificatorType _modificatorType;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterChange.Invoke(other.gameObject, this, _modificatorType);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitChange.Invoke(other.gameObject, this, _modificatorType);
        }
    }
}