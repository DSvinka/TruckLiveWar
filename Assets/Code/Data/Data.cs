using System.Collections.Generic;
using Code.Controller.Starter;
using static Code.Data.DataUtils;
using UnityEngine;


namespace Code.Data
{
    // TODO: Как то улучшить этот ENUM, не удобно.
    internal enum ModificatorType
    {
        SpeedBonus,
        SpeedSlowingDown,
        PlayerKiller
    }

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Data")]
    internal sealed class Data : ScriptableObject
    {
        #region Поля

        [Header("Игрок")]
        [SerializeField] [AssetPath.Attribute(typeof(PlayerData))] private string m_playerDataPath;

        [Header("Модификаторы")]
        [SerializeField] [AssetPath.Attribute(typeof(ModificatorData))] private string m_speedBonusPath;
        [SerializeField] [AssetPath.Attribute(typeof(ModificatorData))] private string m_speedSlowingDownPath;
        [SerializeField] [AssetPath.Attribute(typeof(ModificatorData))] private string m_playerKillerPath;

        [Header("Вооружение")]
        [SerializeField] [AssetPath.Attribute(typeof(WeaponData))] private string m_baseWeaponPath;

        [Header("Траспортные средства")]
        [SerializeField] [AssetPath.Attribute(typeof(CarData))] private string m_baseCarPath;

        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(TargetData))] private string m_targetPath;
        
        [Header("Интерфейс")]
        [SerializeField] [AssetPath.Attribute(typeof(UIData))] private string m_uiPath;

        [Header("Игра")] 
        [SerializeField] [AssetPath.Attribute(typeof(GameStarter))] private string m_gameStarterPrefabPath;
        [SerializeField] private string m_locationsDirPath;

        #endregion

        #region Объекты

        private PlayerData m_player;

        private ModificatorData m_speedBonus;
        private ModificatorData m_speedSlowingDown;
        private ModificatorData m_playerKiller;

        private WeaponData m_baseWeapon;

        private CarData m_baseCar;
        
        private TargetData m_targetData;

        private UIData m_uiData;

        private GameStarter m_gameStarterPrefab;
        private Dictionary<string, LocationData> m_locations;

        #endregion

        #region Свойства

        public PlayerData Player => GetData(m_playerDataPath, ref m_player);

        public ModificatorData SpeedBonus => GetData(m_speedBonusPath, ref m_speedBonus);
        public ModificatorData SpeedSlowingDown => GetData(m_speedSlowingDownPath, ref m_speedSlowingDown);
        public ModificatorData PlayerKiller => GetData(m_playerKillerPath, ref m_playerKiller);

        public WeaponData BaseWeapon => GetData(m_baseWeaponPath, ref m_baseWeapon);

        public CarData BaseCar => GetData(m_baseCarPath, ref m_baseCar);

        public TargetData TargetData => GetData(m_targetPath, ref m_targetData);

        public UIData UIData => GetData(m_uiPath, ref m_uiData);

        public GameStarter GameStarterPrefab => GetData(m_gameStarterPrefabPath, ref m_gameStarterPrefab);
        public Dictionary<string, LocationData> LocationDatas => GetDatasDict(m_locationsDirPath, ref m_locations);

        #endregion
    }
}