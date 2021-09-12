using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "Weapons", menuName = "Data/Data Stores/Weapons")]
    internal sealed class WeaponDatas: ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля
        
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(WeaponData))] private string m_baseGunPath;

        #endregion
        
        #region Объекты
        
        private WeaponData m_baseGun;

        #endregion
        
        #region Свойства
        
        public WeaponData BaseGun => GetData(m_baseGunPath, ref m_baseGun);

        #endregion
    }
}