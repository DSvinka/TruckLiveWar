using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "Modificators", menuName = "Data/Data Stores/Modificators")]
    internal sealed class ModificatorDatas: ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля
        
        [Header("Модификаторы")]
        [SerializeField] [AssetPath.Attribute(typeof(ModificatorData))] private string m_speedBonusPath;
        [SerializeField] [AssetPath.Attribute(typeof(ModificatorData))] private string m_speedSlowingDownPath;
        [SerializeField] [AssetPath.Attribute(typeof(ModificatorData))] private string m_playerKillerPath;

        #endregion
        
        #region Объекты
        
        private ModificatorData m_speedBonus;
        private ModificatorData m_speedSlowingDown;
        private ModificatorData m_playerKiller;

        #endregion
        
        #region Свойства
        
        public ModificatorData SpeedBonus => GetData(m_speedBonusPath, ref m_speedBonus);
        public ModificatorData SpeedSlowingDown => GetData(m_speedSlowingDownPath, ref m_speedSlowingDown);
        public ModificatorData PlayerKiller => GetData(m_playerKillerPath, ref m_playerKiller);

        #endregion
    }
}