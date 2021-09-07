/*
using System;
using UnityEngine;

namespace Code.Utils.Testing
{
    public sealed class CreateWayPoint: MonoBehaviour
    {
        [SerializeField] private GameObject m_prefab;
        private PathBot m_rootWayPoint;

        public GameObject InstantiateObj(Vector3 pos)
        {
            if (!m_rootWayPoint)
            {
                m_rootWayPoint = new GameObject("WayPoint").AddComponent<PathBot>();
            }

            if (m_prefab != null)
            {
                return Instantiate(m_prefab, pos, Quaternion.identity, m_rootWayPoint.transform);
            }

            throw new Exception($"Нет префаба на компоненте {typeof(CreateWayPoint)} {gameObject.name}");
        }
    }
}*/