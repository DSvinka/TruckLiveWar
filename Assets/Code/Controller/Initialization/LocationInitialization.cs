using System;
using Code.Controller.Starter;
using Code.Data.DataStores;
using Code.Markers;
using Code.SaveData.Data;
using Object = UnityEngine.Object;

namespace Code.Controller.Initialization
{
    internal sealed class LocationInitialization
    { 
        private static string m_locationNameID;
        private readonly DataStore m_data;
        private readonly GameStarter m_gameStarterPrefab;

        public static string LocationNameID => m_locationNameID;

        public LocationInitialization(string locationNameID, DataStore data, GameStarter gameStarterPrefab)
        {
            m_gameStarterPrefab = gameStarterPrefab;
            m_locationNameID = locationNameID;
            m_data = data;
        }
        
        public void ChangeLocation(string locationNameID = null, GameSaveData loadedGame = null)
        {
            if (locationNameID == null)
                locationNameID = m_locationNameID;
            
            var gameStarterOld = Object.FindObjectOfType<GameStarter>();
            var location = Object.FindObjectOfType<LocationMarker>();
            var player = Object.FindObjectOfType<PlayerMarker>();

            Object.Destroy(gameStarterOld.gameObject);
            Object.Destroy(location.gameObject);
            Object.Destroy(player.gameObject);

            var gameStarter = Object.Instantiate(m_gameStarterPrefab);
            
            gameStarter.Data = m_data;
            gameStarter.LocationNameID = locationNameID;
            if (loadedGame != null)
                gameStarter.GameSave = loadedGame;
            
            gameStarter.gameObject.SetActive(true);
        }

        public void LoadLocation()
        {
            var marker = Object.FindObjectOfType<LocationMarker>();
            if (marker != null)
            {
                Object.Destroy(marker.gameObject);
                marker.gameObject.SetActive(false);
            }

            CreateLocation();
        }

        private void CreateLocation()
        {
            m_data.LocationDatas.TryGetValue(m_locationNameID, out var location);
            if (location == null)
                throw new Exception("?????????????? ???? ??????????????!");
            
            Object.Instantiate(location.LocationPrefab);
        }
    }
}