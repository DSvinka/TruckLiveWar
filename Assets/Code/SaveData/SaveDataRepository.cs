using System.Data;
using System.IO;
using Code.Controller;
using Code.Controller.Initialization;
using Code.Data;
using Code.Interfaces.SaveData;
using Code.Utils.Extensions;
using UnityEngine;

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

            var savePlayer = new PlayerSaveData
            {
                Position = carPosition,
                Rotation = carProvider.transform.eulerAngles,
                Health = carProvider.Health,
                Car = carProvider.UnitData as CarData,
                Weapons = carProvider.WeaponSlots,
            };

            var saveGame = new GameSaveData()
            {
                Player = savePlayer,
                LocationNameID = LocationInitialization.LocationNameID,
            };
            
            m_data.Save(saveGame, Path.Combine(m_path, m_fileName));
        }

        public void Load(PlayerInitialization playerInitialization, bool loaded = false)
        {
            var file = Path.Combine(m_path, m_fileName);

            if (!File.Exists(file))
                throw new DataException($"File {file} not found");

            var loadedGame = m_data.Load(file);
            var loadedPlayer = loadedGame.Player;

            if (!loaded)
            {
                m_locationInitialization.ChangeLocation(loadedGame.LocationNameID, loadedGame);
                return;
            }

            //playerInitialization.ChangeCar(loadedPlayer.Car);
            var car = playerInitialization.GetPlayerTransport();
            var player = playerInitialization.GetPlayer();
            
            car.transform.position = loadedPlayer.Position;
            car.transform.eulerAngles = loadedPlayer.Rotation;
            car.Health = loadedPlayer.Health;
            
            for (var index = 0; index < loadedPlayer.Weapons.Length; index++)
            {
                var weaponSlot = loadedPlayer.Weapons[index];
                if (weaponSlot.Weapon != null) 
                    car.PlaceWeapon(index, weaponSlot.Weapon);
            }
        }
    }
}