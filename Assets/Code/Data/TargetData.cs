using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "TargetSettings", menuName = "Data/Unit/Enemy/TargetSettings")]
    internal sealed class TargetData : ScriptableObject, IUnitData
    {
        [Header("Информация")]
        [SerializeField] private string m_name = "Мишень";
        [SerializeField] private float m_maxHealth = 30f;
        [SerializeField] private float m_maxFuel;
        [SerializeField] private bool m_infinityFuel = true;
        

        public string Name => m_name;
        public float MaxHealth => m_maxHealth;
        public float MaxFuel => m_maxFuel;
        public bool InfinityFuel => m_infinityFuel;
    }
}