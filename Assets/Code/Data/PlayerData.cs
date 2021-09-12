using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Data/Unit/PlayerSettings")]
    public sealed class PlayerData : ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля
        [SerializeField] [AssetPath.Attribute(typeof(TransportData))] private string m_carDataPath;

        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string m_playerPrefabPath;
        
        [Header("Параметры")] 
        [SerializeField] private float m_rayDistance = 100f;
        [SerializeField, Range(1f, 0.01f)] private float m_radarSize = 100f;
        #endregion
        
        #region Объекты
        
        private TransportData m_transportData;
        private GameObject m_playerPrefab;
        
        #endregion
        
        #region Свойства
        
        public GameObject PlayerPrefab => GetData(m_playerPrefabPath, ref m_playerPrefab);
        public float RayDistance => m_rayDistance;
        public float RadarSize => m_radarSize;

        public TransportData Transport => GetData(m_carDataPath, ref m_transportData);
        
        #endregion
    }
}