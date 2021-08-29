using System;
using Code.Interfaces.Data;
using Code.Interfaces.Providers;
using UnityEngine;

namespace Code.Providers
{
    internal sealed class TargetProvider : MonoBehaviour, IUnit
    {
        public event Action<GameObject, IUnit, float> OnUnitDamage = delegate(GameObject damager, IUnit targetProvider, float damage) {  };
        public event Action<GameObject, IUnit, float> OnUnitHealth = delegate(GameObject healer, IUnit targetProvider, float health) {  };

        public IUnitData UnitData { get; set; }
        public float Health { get; set; }
        
        [HideInInspector] public WallProvider Wall;

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