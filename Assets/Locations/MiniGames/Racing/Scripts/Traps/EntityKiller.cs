using System;
using Client.Player;
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
            
            var player = other.gameObject.GetComponentInParent<Player>();
            if (player)
                player.Car.CarController.KillTrap();
        }
    }
}