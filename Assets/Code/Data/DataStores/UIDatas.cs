using static Code.Data.DataUtils;
using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "UIs", menuName = "Data/Data Stores/UIs")]
    internal sealed class UIDatas : ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля
        
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string m_playerHudPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string m_winHudPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string m_deathHudPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string m_escapeMenuPrefabPath;
        
        #endregion
        
        #region Объекты
        
        private GameObject m_playerHudPrefab;
        private GameObject m_winHudPrefab;
        private GameObject m_deathHudPrefab;
        private GameObject m_escapeMenuPrefab;
        
        #endregion
        
        #region Свойства
        
        public GameObject PlayerHudPrefab => GetData(m_playerHudPrefabPath, ref m_playerHudPrefab);
        public GameObject WinHudPrefab => GetData(m_winHudPrefabPath, ref m_winHudPrefab);
        public GameObject DeathHudPrefab => GetData(m_deathHudPrefabPath, ref m_deathHudPrefab);
        public GameObject EscapeMenuPrefab => GetData(m_escapeMenuPrefabPath, ref m_escapeMenuPrefab);
        
        #endregion
    }
}