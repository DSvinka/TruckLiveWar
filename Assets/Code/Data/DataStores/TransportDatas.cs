using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "Transports", menuName = "Data/Data Stores/Transports")]
    internal sealed class TransportDatas: ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля
        
        [Header("Транспортные средства")]
        [SerializeField] [AssetPath.Attribute(typeof(TransportData))] private string m_vanPath;

        #endregion
        
        #region Объекты
        
        private TransportData m_van;

        #endregion
        
        #region Свойства
        
        public TransportData Van => GetData(m_vanPath, ref m_van);

        #endregion
    }
}