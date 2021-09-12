using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "Enemys", menuName = "Data/Data Stores/Enemys")]
    internal sealed class EnemyDatas: ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля
        
        [Header("Противники")]
        [SerializeField] [AssetPath.Attribute(typeof(TargetData))] private string m_targetDataPath;

        #endregion
        
        #region Объекты
        
        private TargetData m_targetData;

        #endregion
        
        #region Свойства
        
        public TargetData TargetData => GetData(m_targetDataPath, ref m_targetData);

        #endregion
    }
}