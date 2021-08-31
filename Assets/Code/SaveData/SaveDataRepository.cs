using System.Data;
using System.IO;
using Code.Controller;
using UnityEngine;

namespace Code.SaveData
{
    internal sealed class SaveDataRepository: ISaveDataRepository
    {
        private readonly IData<SavedData> m_data;
        
        private const string m_folderName = "dataSave";
        private const string m_fileName = "data.bat";
        private readonly string m_path;

        public SaveDataRepository()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                m_data = new PlayerPrefsData();
            else
                m_data = new JsonData<SavedData>();
            m_path = Path.Combine(Application.dataPath, m_folderName);
        }

        public void Save(CarController player)
        {
            if (!Directory.Exists(Path.Combine(m_path)))
                Directory.CreateDirectory(m_path);
            var savePlayer = new SavedData
            {
                Position = player.CarProvider.transform.position,
                Health = player.CarProvider.Health,
                IsEnabled = true
            };
            
            m_data.Save(savePlayer, Path.Combine(m_path, m_fileName));
            Debug.Log("Save");
        }

        public void Load(CarController player)
        {
            var file = Path.Combine(m_path, m_fileName);

            if (!File.Exists(file))
                throw new DataException($"File {file} not found");

            var newPlayer = m_data.Load(file);
            player.CarProvider.transform.position = newPlayer.Position;
            player.CarProvider.Health = newPlayer.Health;
            player.CarProvider.gameObject.SetActive(newPlayer.IsEnabled);
            
            Debug.Log(newPlayer);
        }
    }
}