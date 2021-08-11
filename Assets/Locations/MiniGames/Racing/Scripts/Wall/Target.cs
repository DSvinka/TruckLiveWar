using Entities.Base;
using UnityEngine;

namespace Locations.MiniGames.Racing.Wall
{
    internal sealed class Target : Entity
    {
        [HideInInspector] public Wall Wall;

        private void OnDestroy()
        {
            Wall.TargetDestroy(this);
        }
    }
}