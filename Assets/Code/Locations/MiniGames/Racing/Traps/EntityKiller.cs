using System;
using Code.Client;
using Code.Entities.Base;
using UnityEngine;

namespace Code.Locations.MiniGames.Racing.Traps
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