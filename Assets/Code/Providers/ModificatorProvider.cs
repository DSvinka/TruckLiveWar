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

        [SerializeField] private ModificatorType m_modificatorType;
        [SerializeField] private GameObject m_parent;

        public GameObject Parent => m_parent;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterChange.Invoke(other.gameObject, this, m_modificatorType);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitChange.Invoke(other.gameObject, this, m_modificatorType);
        }
    }
}