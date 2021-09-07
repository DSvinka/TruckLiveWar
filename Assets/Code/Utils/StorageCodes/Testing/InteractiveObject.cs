/*using System;
using Code.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Utils.Testing
{
    public abstract class InteractiveObject: MonoBehaviour, IExecute
    {
        [SerializeField] private bool m_isAllowScaling;
        [SerializeField] private float ActiveDis;
        protected Color m_color;

        private bool m_isInteractable;

        protected bool IsInteractable
        {
            get { return m_isInteractable; }
            private set
            {
                m_isInteractable = value;
                GetComponent<Renderer>().enabled = m_isInteractable;
                GetComponent<Collider>().enabled = m_isInteractable;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsInteractable || !other.CompareTag("Player"))
                return;

            Interaction();
            IsInteractable = false;
        }

        protected abstract void Interaction();
        public abstract void Execute(float deltaTime);

        private void Start()
        {
            IsInteractable = true;
            m_color = Random.ColorHSV();;
            if (TryGetComponent(out Renderer renderer))
                renderer.material.color = m_color;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "bot.jpg", m_isAllowScaling);
        }

        private void OnDrawGizmosSelected()
        {
            #if UNITY_EDITOR
            var t = transform;
            var flat = new Vector3(ActiveDis, 0, ActiveDis);
            Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, flat);
            Gizmos.DrawWireSphere(Vector3.zero, 5);
            #endif
        }
    }
}*/