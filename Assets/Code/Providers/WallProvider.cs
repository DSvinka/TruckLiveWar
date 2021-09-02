using UnityEngine;

namespace Code.Providers
{
    internal sealed class WallProvider : MonoBehaviour
    {
        [HideInInspector] public int TargetCount;
        public TargetProvider[] TargetProviders { get; set; }

        public void Explosion()
        {
            gameObject.SetActive(false);
        }
    }
}