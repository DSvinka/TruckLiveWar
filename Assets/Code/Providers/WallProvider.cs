using UnityEngine;

namespace Code.Providers
{ 
    internal sealed class WallProvider : MonoBehaviour
    {
        [SerializeField] private TargetProvider[] _targetProviders;

        [HideInInspector] public int TargetCount;
        public TargetProvider[] TargetProviders => _targetProviders;

        public void Explosion()
        {
            Destroy(gameObject);
        }
    }
}