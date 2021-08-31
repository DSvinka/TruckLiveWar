using Code.Controller.Initialization;
using UnityEngine;

namespace Code.Controller.Starter
{
    internal sealed class GameStarter : MonoBehaviour
    {
        [SerializeField] private Data.Data m_data;
        [SerializeField] private string m_locationNameID;
        private Controllers m_controllers;

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

        private void Start()
        {
            m_controllers = new Controllers();

            var location = new LocationInitialization(m_locationNameID, m_data, this, m_data.GameStarterPrefab);
            location.LoadLocation();
            
            var game = new GameInitialization(m_controllers, location, m_data);
            m_controllers.Initialization();
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
