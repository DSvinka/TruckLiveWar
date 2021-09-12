using System;
using System.Data;
using System.IO;
using Code.Controller;
using Code.Controller.Initialization;
using Code.Data;
using Code.Interfaces.SaveData;
using Code.SaveData.Data;
using Code.Utils.Extensions;
using UnityEditor;
using UnityEngine;
using WeaponSave = Code.SaveData.Data.Weapon;
using Weapon = Code.Controller.Weapon;

namespace Code.SaveData
{
    internal sealed class SaveDataRepository: ISaveDataRepository
    {
        private readonly IData<GameSaveData> m_data;
        private readonly LocationInitialization m_locationInitialization;

        private const string m_folderName = "dataSave";
        private const string m_fileName = "data.bat";
        private readonly string m_path;

        private const float PlayerSpawnChangeY = 1.5f;

        public SaveDataRepository(LocationInitialization locationInitialization)
        {
            m_data = new JsonData<GameSaveData>();
            m_path = Path.Combine(Application.dataPath, m_folderName);
            m_locationInitialization = locationInitialization;
        }

        public void Save(CarController player)
        {
            if (!Directory.Exists(Path.Combine(m_path)))
                Directory.CreateDirectory(m_path);

            var carPosition = player.CarProvider.transform.position;
            var carProvider = player.CarProvider;

            carPosition = carPosition.UpdateY(carPosition.y + PlayerSpawnChangeY);
            var carData = player.CarProvider.UnitData as TransportData;
            if (carData == null)
                throw new Exception("UnitData в CarProvider не является CarData");

            var weapons = new WeaponSave[carProvider.WeaponSlots.Length];
            
            for (var index = 0; index < carProvider.WeaponSlots.Length; index++)
            {
                var weapon = carProvider.WeaponSlots[index];
                if (weapon.Weapon != null)
                {
                    weapons[index] = new WeaponSave
                    {
                        PathToData = weapon.Weapon.WeaponData.Path,
                        Ammo = 10, // TODO: Добавить патроны когда будут патроны в оружии.
                        AmmoInClip = 20
                    };   
                }
            }

            var car = new Car()
            {
                PathToData = carData.Path,
                Health = carProvider.Health,
                Fuel = 100 // TODO: Добавить топливо когда будет топливо в машине
            };

            var savePlayer = new PlayerSaveData
            {
                Position = carPosition,
                Rotation = carProvider.transform.eulerAngles,
                Car = car,
                Weapons = weapons
            };

            var saveLocation = new LocationSaveData
            {
                LocationNameID = LocationInitialization.LocationNameID,
            };

            saveLocation.ModificatorsSet(ModificatorsController.ModificatorProviders);
            saveLocation.WallsSet(WallController.WallProviders);
            saveLocation.PickupsSet(PickupController.PickupProviders);

            var saveGame = new GameSaveData
            {
                Player = savePlayer,
                Location = saveLocation,
            };
            
            m_data.Save(saveGame, Path.Combine(m_path, m_fileName));
        }

        public void Load(PlayerInitialization playerInitialization, CarController carController = null, bool loaded = false)
        {
            var file = Path.Combine(m_path, m_fileName);

            if (!File.Exists(file))
                throw new DataException($"File {file} not found");

            var loadedGame = m_data.Load(file);
            var loadedPlayer = loadedGame.Player;
            var loadedLocation = loadedGame.Location;
            
            if (!loaded)
            {
                m_locationInitialization.ChangeLocation(loadedLocation.LocationNameID, loadedGame);
                return;
            }

            if (carController == null)
                throw new Exception("CarController Отсутствует!");

            #region Инициализация Локации

            var modificatorProviders = ModificatorsController.ModificatorProviders;
            var modificators = loadedLocation.Modificators;
            if (modificatorProviders.Length != 0)
            {
                for (var index = 0; index < modificatorProviders.Length; index++)
                {
                    var modificatorProvider = modificatorProviders[index];
                    var modificator = modificators[index];
                    modificatorProvider.Parent.SetActive(modificator.IsActive);
                }   
            }

            var wallProviders = WallController.WallProviders;
            var walls = loadedLocation.Walls;
            if (wallProviders.Length != 0)
            {
                for (var index = 0; index < wallProviders.Length; index++)
                {
                    var wallProvider = wallProviders[index];
                    var wall = walls[index];
                    
                    wallProvider.gameObject.SetActive(wall.IsActive);
                    
                    for (var i = 0; i < wallProvider.TargetProviders.Length; i++)
                    {
                        var wallTarget = wall.Targets[i];
                        var targetProvider = wallProvider.TargetProviders[i];
                        
                        targetProvider.gameObject.SetActive(wallTarget.IsActive);
                        targetProvider.Health = wallTarget.Health;
                    }
                }
            }

            var pickupProviders = PickupController.PickupProviders;
            var pickups = loadedLocation.Pickups;
            if (pickupProviders.Length != 0)
            {
                for (var index = 0; index < pickupProviders.Length; index++)
                {
                    var pickupProvider = pickupProviders[index];
                    var pickup = pickups[index];
                    pickupProvider.Parent.SetActive(pickup.IsActive);
                }
            }

            #endregion
            
            #region Инициализация Игрока

            var carData = AssetDatabase.LoadAssetAtPath<TransportData>(loadedPlayer.Car.PathToData);
            
            playerInitialization.ChangeCar(carData);
            var car = playerInitialization.GetPlayerTransport();

            car.transform.position = loadedPlayer.Position;
            car.transform.eulerAngles = loadedPlayer.Rotation;
            car.Health = loadedPlayer.Car.Health;

            var weapons = new Weapon[loadedPlayer.Weapons.Length];
            
            for (var index = 0; index < loadedPlayer.Weapons.Length; index++)
            {
                var weapon = loadedPlayer.Weapons[index];
                if (!string.IsNullOrEmpty(weapon.PathToData))
                {
                    var weaponData = AssetDatabase.LoadAssetAtPath<WeaponData>(weapon.PathToData);
                    weapons[index] = new Weapon(weaponData);
                }
            }
            
            carController.WeaponsController.SetWeaponOnSpawn = weapons;

            #endregion
        }
    }
}