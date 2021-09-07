using static Code.Data.DataUtils;
using Code.Interfaces.Data;
using Code.Markers;
using UnityEditor;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "LocationSettings", menuName = "Data/LocationSettings")]
    internal sealed class LocationData : ScriptableObject, IDictData, IData
    {
        public string Path { get; set; }
        
        #region Поля
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string m_locationPrefabPath;

        [Header("Информация")]
        [SerializeField] private string m_IDName = "location";
        [SerializeField] private string m_name = "Название";
        #endregion
        
        #region Объекты
        private GameObject m_locationPrefab;
        #endregion

        #region Свойства
        public GameObject LocationPrefab => GetData(m_locationPrefabPath, ref m_locationPrefab);
        public string Name => m_name;
        public string IDName => m_IDName;
        #endregion
    }
}