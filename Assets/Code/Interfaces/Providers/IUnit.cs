using System;
using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Interfaces.Providers
{
    internal interface IUnit
    {
        event Action<GameObject, IUnit, float> OnUnitDamage;
        event Action<GameObject, IUnit, float> OnUnitHealth;

        IUnitData UnitData { get; set; }
        float Health { get; set; }
        
        void Explosion();
        void AddDamage(GameObject attacker, float damage);
        void AddHealth(GameObject healer, float health);
    }
}