using System;
using Code.Controller.Starter;
using Code.Controller.UI;
using Code.Data.DataStores;
using Code.Factory;
using Code.Markers;
using Code.Providers;
using Code.SaveData;
using Object = UnityEngine.Object;

namespace Code.Controller.Initialization
{
    internal sealed class GameInitialization
    {
        private Controllers m_controllers;
        private DataStore m_data;

        public PlayerInitialization PlayerInitialization { get; }
        public CarController CarController { get; }

        public GameInitialization(Controllers controllers, LocationInitialization locationInitialization, SaveDataRepository saveRepository, DataStore data)
        {
            m_controllers = controllers;
            m_data = data;
            
            var modificators = Object.FindObjectsOfType<ModificatorProvider>();
            var pickups = Object.FindObjectsOfType<PickupProvider>();
            var walls = Object.FindObjectsOfType<WallProvider>();
            var locationChangers = Object.FindObjectsOfType<LocationChangerProvider>();

            var playerSpawn = Object.FindObjectOfType<PlayerSpawnMarker>();
            if (playerSpawn == null)
                throw new Exception("Спавнер игрока отсуствует!");
            
            var playerFactory = new PlayerFactory(data.PlayerData);
            
            var inputInitialization = new InputInitialization();
            var playerInitialization = new PlayerInitialization(playerFactory, playerSpawn.transform.position);

            var uiFactory = new UIFactory(data.UIDatas, playerInitialization);
            var hudInitialization = new HudInitialization(uiFactory);
            var escapeMenuInitialization = new EscapeMenuInitilization(uiFactory);

            var inputController = new InputController();
            var weaponController = new WeaponsController(playerInitialization);
            var carController = new CarController(playerInitialization, weaponController, data.PlayerData.Transport);
            var modificatorsController = new ModificatorsController(modificators, carController);
            var hudController = new HudController(hudInitialization, modificatorsController, carController, playerInitialization, data.PlayerData);
            var radarController = new RadarController(playerInitialization, hudInitialization, data.PlayerData.RadarSize);
            var escapeMenuController = new EscapeMenuController(escapeMenuInitialization, playerInitialization, carController, 
                locationInitialization, saveRepository);

            CarController = carController;
            PlayerInitialization = playerInitialization;

            controllers.Add(PlayerInitialization);
            controllers.Add(hudInitialization);
            controllers.Add(escapeMenuInitialization);
            
            controllers.Add(inputController);
            controllers.Add(carController);
            controllers.Add(weaponController);
            controllers.Add(modificatorsController);
            controllers.Add(hudController);
            controllers.Add(radarController);
            controllers.Add(escapeMenuController);

            if (locationChangers.Length != 0)
            {
                var locationChangerController = new LocationChangerController(playerInitialization, locationInitialization,
                    hudController, locationChangers);
                controllers.Add(locationChangerController);
            }
            
            if (pickups.Length != 0)
            {
                var pickupController = new PickupController(pickups, carController, weaponController);
                controllers.Add(pickupController);
            }

            if (walls.Length != 0)
            {
                var wallController = new WallController(walls, data.EnemyDatas.TargetData);
                controllers.Add(wallController);
            }
        }
    }
}