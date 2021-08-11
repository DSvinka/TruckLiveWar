using System.Collections.Generic;
using UnityEngine;

namespace Locations.MiniGames.Racing.Wall
{
    internal sealed class Wall : MonoBehaviour
    {
        public List<Target> Targets;

        public void Start()
        {
            foreach (var target in Targets)
                target.Wall = this;
        }
        public void TargetDestroy(Target item)
        {
            Targets.Remove(item);

            if (Targets.Count == 0)
                Destroy(gameObject);
        }
    }
}