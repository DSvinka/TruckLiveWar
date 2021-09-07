using Code.Controller.Initialization;
using Code.Markers;
using Code.SaveData;
using Code.SaveData.Data;
using UnityEngine;

namespace Code.Controller.Starter
{
    internal sealed class GameStarter : MonoBehaviour
    {
        [SerializeField] private Data.Data m_data;
        [SerializeField] private string m_locationNameID;
        private Controllers m_controllers;
        private GameSaveData m_gameSaveData;

        public Data.Data Data
        {
            get => m_data;
            set => m_data = value;
        }
        public string LocationNameID
        {
            get => m_locationNameID;
            set => m_locationNameID = value;
        }
        public GameSaveData GameSave
        {
            get => m_gameSaveData;
            set => m_gameSaveData = value;
        }

        private void Start()
        {
            m_controllers = new Controllers();
            
            var location = new LocationInitialization(m_locationNameID, m_data, m_data.GameStarterPrefab);
            location.LoadLocation();

            var saveRepository = new SaveDataRepository(location);
            var game = new GameInitialization(m_controllers, location, saveRepository, m_data);

            m_controllers.Initialization();
            
            if (GameSave != null)
                saveRepository.Load(game.PlayerInitialization, game.CarController, true);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            m_controllers.Execute(deltaTime);
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            m_controllers.LateExecute(deltaTime);
        }

        private void OnDestroy()
        {
            m_controllers.Cleanup();
        }
    }
}
