using System.Linq;
using UnityEngine;

namespace Code.Locations.MiniGames.Racing.Wall
{
    internal sealed class Wall : MonoBehaviour
    {
        [SerializeField] private Target[] Targets;
        private int _targetsCount;

        public void Start()
        {
            if (Targets.Length == 0)
                Targets = GetComponentsInChildren<Target>();
            _targetsCount = Targets.Length;
            
            foreach (var target in Targets)
                target.Wall = this;
        }
        public void TargetDestroy(Target item)
        {
            if (Targets.Contains(item))
            {
                _targetsCount -= 1;
                if (_targetsCount <= 0)
                    Destroy(gameObject);
            }
        }
    }
}