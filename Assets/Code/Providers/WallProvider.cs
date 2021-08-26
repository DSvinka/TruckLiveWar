using UnityEngine;

namespace Code.Providers
{
    internal sealed class WallProvider : MonoBehaviour
    {
        [SerializeField] private TargetProvider[] m_targetProviders;

        [HideInInspector] public int TargetCount;
        public TargetProvider[] TargetProviders => m_targetProviders;

        public void Explosion()
        {
            Destroy(gameObject);
        }
    }
}