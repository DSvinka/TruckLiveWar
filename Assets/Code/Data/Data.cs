using System.Collections.Generic;
using System.IO;
using Code.Controller.Starter;
using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;


// TODO: Перенести префабы в Resources
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
        #region Пути

        [Header("Игрок")]
        [SerializeField] private string m_playerDataPath;

        [Header("Модификаторы")] // TODO: Сделать автоматический сбор модификаторов (ввод только названия папки)
        [SerializeField] private string m_speedBonusPath;
        [SerializeField] private string m_speedSlowingDownPath;
        [SerializeField] private string m_playerKillerPath;

        [Header("Вооружение")] // TODO: Сделать автоматический сбор транспорта (ввод только названия папки)
        [SerializeField] private string m_baseWeaponPath;

        [Header("Траспортные средства")] // TODO: Сделать автоматический сбор вооружения (ввод только названия папки)
        [SerializeField] private string m_baseCarPath;

        [Header("Объекты")] // TODO: Сделать автоматический сбор объектов (ввод только названия папки)
        [SerializeField] private string m_targetPath;
        
        [Header("Интерфейс")] // TODO: Сделать автоматический сбор интерфейсов (ввод только названия папки)
        [SerializeField] private string m_hudPath;

        [Header("Игра")] 
        [SerializeField] private GameStarter m_gameStarterPrefab;
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

        private HudData m_hudData;
        
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

        public HudData HudData => GetData(m_hudPath, ref m_hudData);

        public GameStarter GameStarterPrefab => m_gameStarterPrefab;
        public Dictionary<string, LocationData> LocationDatas => GetDatasDict(m_locationsDirPath, ref m_locations);

        #endregion
    }
}