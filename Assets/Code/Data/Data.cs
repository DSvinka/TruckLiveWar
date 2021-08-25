using System.IO;
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
        #region Пути

        [Header("Игрок")]
        [SerializeField] private string _playerDataPath;
        
        [Header("Модификаторы")] // TODO: Сделать автоматический сбор модификаторов (ввод только названия папки)
        [SerializeField] private string _speedBonusPath;
        [SerializeField] private string _speedSlowingDownPath;
        [SerializeField] private string _playerKillerPath;
        
        [Header("Вооружение")] // TODO: Сделать автоматический сбор транспорта (ввод только названия папки)
        [SerializeField] private string _baseWeaponPath;
        
        [Header("Траспортные средства")] // TODO: Сделать автоматический сбор вооружения (ввод только названия папки)
        [SerializeField] private string _baseCarPath;
        
        [Header("Объекты")] // TODO: Сделать автоматический сбор объектов (ввод только названия папки)
        [SerializeField] private string _targetPath;

        #endregion
        
        #region Объекты

        private PlayerData _player;
        
        private ModificatorData _speedBonus;
        private ModificatorData _speedSlowingDown;
        private ModificatorData _playerKiller;

        private WeaponData _baseWeapon;

        private CarData _baseCar;
        private TargetData _targetData;
        
        #endregion
        
        #region Свойства

        public PlayerData Player => GetData(_playerDataPath, ref _player);

        public ModificatorData SpeedBonus => GetData(_speedBonusPath, ref _speedBonus);
        public ModificatorData SpeedSlowingDown => GetData(_speedSlowingDownPath, ref _speedSlowingDown);
        public ModificatorData PlayerKiller => GetData(_playerKillerPath, ref _playerKiller);

        public WeaponData BaseWeapon => GetData(_baseWeaponPath, ref _baseWeapon);

        public CarData BaseCar => GetData(_baseCarPath, ref _baseCar);
        
        public TargetData TargetData => GetData(_targetPath, ref _targetData);

        #endregion
    }
}