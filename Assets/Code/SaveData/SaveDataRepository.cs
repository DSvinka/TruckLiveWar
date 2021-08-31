using System.Data;
using System.IO;
using Code.Controller;
using Code.Controller.Initialization;
using Code.Data;
using Code.Interfaces.SaveData;
using UnityEngine;

namespace Code.SaveData
{
    internal sealed class SaveDataRepository: ISaveDataRepository
    {
        private readonly IData<PlayerSaveData> m_data;
        
        private const string m_folderName = "dataSave";
        private const string m_fileName = "data.bat";
        private readonly string m_path;

        public SaveDataRepository()
        {
            m_data = new JsonData<PlayerSaveData>();
            m_path = Path.Combine(Application.dataPath, m_folderName);
        }

        public void Save(CarController player)
        {
            if (!Directory.Exists(Path.Combine(m_path)))
                Directory.CreateDirectory(m_path);
            var savePlayer = new PlayerSaveData
            {
                Position = player.CarProvider.transform.position,
                Health = player.CarProvider.Health,
                Car = player.CarProvider.UnitData as CarData,
                Weapons = player.CarProvider.WeaponSlots
            };
            
            m_data.Save(savePlayer, Path.Combine(m_path, m_fileName));
            Debug.Log("Save");
        }

        public void Load(PlayerInitialization playerInitialization)
        {
            var file = Path.Combine(m_path, m_fileName);

            if (!File.Exists(file))
                throw new DataException($"File {file} not found");

            var loadedPlayer = m_data.Load(file);

            //playerInitialization.ChangeCar(loadedPlayer.Car);
            var car = playerInitialization.GetPlayerTransport();
            var player = playerInitialization.GetPlayer();
            
            player.transform.position = loadedPlayer.Position;
            car.transform.localPosition = Vector3.zero;
            car.Health = loadedPlayer.Health;
            
            for (var index = 0; index < loadedPlayer.Weapons.Length; index++)
            {
                var weaponSlot = loadedPlayer.Weapons[index];
                if (weaponSlot.Weapon != null) 
                    car.PlaceWeapon(index, weaponSlot.Weapon);
            }

            Debug.Log(loadedPlayer);
        }
    }
}