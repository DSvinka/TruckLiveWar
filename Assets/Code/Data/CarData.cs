using static Code.Data.DataUtils;
using Code.Interfaces.Data;
using Code.Providers;
using UnityEditor;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "CarSettings", menuName = "Data/Transport/CarSettings")]
    public sealed class CarData : ScriptableObject, IData, ICarData, IUnitData
    {
        public string Path { get; set; }
        
        #region Поля
        [SerializeField] [AssetPath.Attribute(typeof(CarProvider))] private string m_carPrefabPath;

        [Header("Информация")]
        [SerializeField] private string m_name = "Машина";
        [SerializeField] [Tooltip("Максимальное количество жизней")]
        private float m_maxHealth = 100f;
        [SerializeField] [Tooltip("Максимальное количество топлива")] 
        private float m_maxFuel = 100f;
        [SerializeField] [Tooltip("Топливо конечное или бесконечное?")] 
        private bool m_infinityFuel;

        [Header("Характиристики Автомобиля")]
        [SerializeField] [Tooltip("Максимальный угол поворота колес")]
        public float m_maxAngle = 30f;
        [SerializeField] [Tooltip("Максимальный крутящий момент, прилагаемый к ведущим колесам")]
        public float m_maxTorque = 300f;
        [SerializeField] [Tooltip("Максимальный тормозной момент, прилагаемый к ведущим колесам")]
        public float m_brakeTorque = 30000f;

        [Header("Настройки Физического Движка")]
        [SerializeField] [Tooltip("Скорость автомобиля, когда физический движок может использовать разное количество подшагов (в м/с).")]
        private float m_criticalSpeed = 5f;
        [SerializeField] [Tooltip("Подэтапы моделирования, когда скорость выше критической.")]
        private int m_stepsBelow = 12;
        [SerializeField] [Tooltip("Подэтапы моделирования при скорости ниже критической.")]
        private int m_stepsAbove = 15;
        #endregion
        
        #region Объекты
        private CarProvider m_carPrefab;
        #endregion
        
        #region Свойства
        public CarProvider CarPrefab => GetData(m_carPrefabPath, ref m_carPrefab);
        
        public float MaxAngle => m_maxAngle;
        public float MaxTorque => m_maxTorque;
        public float BrakeTorque => m_brakeTorque;

        public float CriticalSpeed => m_criticalSpeed;
        public int StepsBelow => m_stepsBelow;
        public int StepsAbove => m_stepsAbove;
        
        public string Name => m_name;
        
        public float MaxHealth => m_maxHealth;
        public float MaxFuel => m_maxFuel;
        public bool InfinityFuel => m_infinityFuel;
        #endregion
    }
}