using Code.Interfaces.Data;
using Code.Providers;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "CarSettings", menuName = "Data/Transport/CarSettings")]
    internal sealed class CarData : ScriptableObject, ICarData
    {
        [SerializeField] private CarProvider _carProvider;
        
        [Header("Информация")] 
        [SerializeField] private string _name = "Машина";
        [SerializeField] private float _maxHealth = 100f;

        [Header("Характиристики Автомобиля")]
        [SerializeField, Tooltip("Максимальный угол поворота колес")]
        public float _maxAngle = 30f;
        [SerializeField, Tooltip("Максимальный крутящий момент, прилагаемый к ведущим колесам")]
        public float _maxTorque = 300f;
        [SerializeField, Tooltip("Максимальный тормозной момент, прилагаемый к ведущим колесам")]
        public float _brakeTorque = 30000f;

        [Header("Настройки Физического Движка")]
        [SerializeField, Tooltip("Скорость автомобиля, когда физический движок может использовать разное количество подшагов (в м/с).")]
        private float _criticalSpeed = 5f;
        [SerializeField, Tooltip("Подэтапы моделирования, когда скорость выше критической.")]
        private int _stepsBelow = 12;
        [SerializeField, Tooltip("Подэтапы моделирования при скорости ниже критической.")]
        private int _stepsAbove = 15;

        public CarProvider CarProvider => _carProvider;
        public string Name => _name;
        public float MaxHealth => _maxHealth;

        public float MaxAngle => _maxAngle;
        public float MaxTorque => _maxTorque;
        public float BrakeTorque => _brakeTorque;

        public float CriticalSpeed => _criticalSpeed;
        public int StepsBelow => _stepsBelow;
        public int StepsAbove => _stepsAbove;
    }
}