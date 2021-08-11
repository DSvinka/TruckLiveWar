using UnityEngine;

namespace Entities.Base
{
    public class Entity: MonoBehaviour
    {
        [Header("Характеристики")]
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _fuel; // TODO: Использовать топливо.
        [SerializeField] private float _maxFuel;
        [SerializeField] private bool _infinityFuel;
        private bool _isDeath;

        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public float Fuel => _fuel;
        public float MaxFuel => _maxFuel;
        public bool InfinityFuel => _infinityFuel;
        public bool IsDeath => _isDeath;

        /// <summary>
        /// Забрать у объекта определённое количество жизней
        /// </summary>
        public void AddDamage(float damage)
        {
            if (!_isDeath)
            {   
                if (damage >= _health)
                    Death();
                else
                    _health -= damage;
            }
        }
        
        /// <summary>
        /// Пополнить определённое количество жизней объекту
        /// </summary>
        public void AddHealth(float health)
        {
            if (!_isDeath)
            {
                if (health >= _health)
                    _health = _maxHealth;
                else
                    _health += health;
            }
        }

        protected virtual void Death()
        {
            _health = 0;
            _isDeath = true;
        }
    }
}