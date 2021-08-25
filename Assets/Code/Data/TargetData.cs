using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "TargetSettings", menuName = "Data/Unit/Enemy/TargetSettings")]
    internal sealed class TargetData : ScriptableObject
    {
        [Header("Информация")] 
        [SerializeField] private string _name = "Мишень";
        [SerializeField] private float _maxHealth = 100f;

        public string Name => _name;
        public float MaxHealth => _maxHealth;
    }
}