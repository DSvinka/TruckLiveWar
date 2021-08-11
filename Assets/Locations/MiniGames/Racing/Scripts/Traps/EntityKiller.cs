using System;
using Entities.Base;
using UnityEngine;

namespace Locations.MiniGames.Racing.Traps
{
    internal sealed class EntityKiller : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var entity = other.gameObject.GetComponentInParent<Entity>();
            if (entity)
                entity.AddDamage(entity.MaxHealth);
        }
    }
}