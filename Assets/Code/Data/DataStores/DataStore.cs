using System.Collections.Generic;
using Code.Controller.Starter;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "DataStore", menuName = "Data/Data Stores/DataStore")]
    internal sealed class DataStore : ScriptableObject
    {
        #region Поля
        
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(PlayerData))] private string m_playerDataPath;
        
        [SerializeField] [AssetPath.Attribute(typeof(GameStarter))] private string m_gameStarterPrefabPath;
        [SerializeField] private string m_locationsDirPath;
        
        [Header("Хранилища Объектов")]
        [SerializeField] [AssetPath.Attribute(typeof(ModificatorDatas))] private string m_modificatorsPath;
        [SerializeField] [AssetPath.Attribute(typeof(WeaponDatas))] private string m_weaponsPath;
        [SerializeField] [AssetPath.Attribute(typeof(TransportDatas))] private string m_transportsPath;
        [SerializeField] [AssetPath.Attribute(typeof(EnemyDatas))] private string m_enemysPath;
        [SerializeField] [AssetPath.Attribute(typeof(UIDatas))] private string m_uisPath;

        #endregion

        #region Объекты

        private PlayerData m_player;
        
        private GameStarter m_gameStarterPrefab;
        private Dictionary<string, LocationData> m_locations;
        
        private ModificatorDatas m_modificators;
        private WeaponDatas m_weapons;
        private TransportDatas m_transports;
        private EnemyDatas m_enemys;
        private UIDatas m_uis;

        #endregion

        #region Свойства

        public PlayerData PlayerData => GetData(m_playerDataPath, ref m_player);
        
        public GameStarter GameStarterPrefab => GetData(m_gameStarterPrefabPath, ref m_gameStarterPrefab);
        public Dictionary<string, LocationData> LocationDatas => GetDatasDict(m_locationsDirPath, ref m_locations);

        public ModificatorDatas ModificatorDatas => GetData(m_modificatorsPath, ref m_modificators);
        public WeaponDatas WeaponDatas => GetData(m_weaponsPath, ref m_weapons);
        public TransportDatas TransportDatas => GetData(m_transportsPath, ref m_transports);
        public EnemyDatas EnemyDatas => GetData(m_enemysPath, ref m_enemys);
        public UIDatas UIDatas => GetData(m_uisPath, ref m_uis);
        
        #endregion
    }
}