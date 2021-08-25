using System;
using Code.Interfaces.Providers;
using UnityEngine;

namespace Code.Providers
{
    internal sealed class TargetProvider : MonoBehaviour, IUnit
    {
        public event Action<GameObject, IUnit, float> OnUnitDamage = delegate(GameObject damager, IUnit targetProvider, float damage) {  };
        public event Action<GameObject, IUnit, float> OnUnitHealth = delegate(GameObject healer, IUnit targetProvider, float health) {  };

        [HideInInspector] public WallProvider Wall;
        [HideInInspector] public float Health;

        public void Explosion()
        {
            Destroy(gameObject);
        }
        
        public void AddDamage(GameObject damager, float damage)
        {
            OnUnitDamage.Invoke(damager, this, damage);
        }
        
        public void AddHealth(GameObject healer, float health)
        {
            OnUnitHealth.Invoke(healer, this, health);
        }
    }
}