/*using System;
using System.Linq;
using UnityEngine;

namespace Code.Utils.Testing
{
    public sealed class PathBot: MonoBehaviour
    {
        [SerializeField] private Color m_lineColor = Color.red;
        [SerializeField] private Color m_sphereColor = Color.green;
        [SerializeField, Range(0.1f, 5.0f)] private float m_radius = 0.5f;
        
        // OnDrawGizmosSelected()
        private void OnDrawGizmos()
        {
            var nodes = GetComponentsInChildren<Transform>().Skip(1).Select(t => t.position).ToArray();
            for (var i = 0; i < nodes.Length; i++)
            {
                var currentNode = nodes[i];
                var previousNode = Vector3.zero;

                if (i > 0)
                    previousNode = nodes[i - 1];
                else if (i == 0 && nodes.Length > 1)
                    previousNode = nodes[nodes.Length - 1];

                Gizmos.color = m_lineColor;
                Gizmos.DrawLine(previousNode, currentNode);
                Gizmos.color = m_sphereColor;
            }
        }
    }
}*/