using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "TargetSettings", menuName = "Data/Unit/Enemy/TargetSettings")]
    internal sealed class TargetData : ScriptableObject
    {
        [Header("Информация")]
        [SerializeField] private string m_name = "Мишень";
        [SerializeField] private float m_maxHealth = 100f;

        public string Name => m_name;
        public float MaxHealth => m_maxHealth;
    }
}