using System;
using Code.Controller;
using Code.Data;
using Code.Interfaces.Providers;
using UnityEngine;

namespace Code.Providers
{
    internal sealed class ModificatorProvider: MonoBehaviour, IModificatorProvider
    {
        public event Action<GameObject, ModificatorProvider, ModificatorData> OnTriggerEnterChange = delegate(GameObject gameObject, ModificatorProvider modificatorProvider, ModificatorData ModificatorData) {  };
        public event Action<GameObject, ModificatorProvider, Modificator> OnTriggerExitChange = delegate(GameObject gameObject, ModificatorProvider modificatorProvider, Modificator Modificator) {  };
        
        [SerializeField] private ModificatorData m_modificatorData;
        [SerializeField] private GameObject m_parent;

        public Modificator Modificator { get; set; }
        public GameObject Parent => m_parent;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterChange.Invoke(other.gameObject, this, m_modificatorData);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitChange.Invoke(other.gameObject, this, Modificator);
        }
    }
}