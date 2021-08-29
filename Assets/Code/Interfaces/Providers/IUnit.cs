using System;
using UnityEngine;

namespace Code.Interfaces.Providers
{
    internal interface IUnit
    {
        event Action<GameObject, IUnit, float> OnUnitDamage;
        event Action<GameObject, IUnit, float> OnUnitHealth;

        void Explosion();
        void AddDamage(GameObject attacker, float damage);
        void AddHealth(GameObject healer, float health);
    }
}