using Code.Entities.Base;
using UnityEngine;

namespace Code.Locations.MiniGames.Racing.Wall
{
    internal sealed class Target : Entity
    {
        [HideInInspector] public Wall Wall;

        protected override void Death()
        {
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            Wall.TargetDestroy(this);
        }
    }
}