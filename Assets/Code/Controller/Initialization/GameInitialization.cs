using System;
using Code.Controller.Starter;
using Code.Controller.UI;
using Code.Factory;
using Code.Markers;
using Code.Providers;
using Object = UnityEngine.Object;

namespace Code.Controller.Initialization
{
    internal sealed class GameInitialization
    {
        private Controllers m_controllers;
        private Data.Data m_data;
        
        public GameInitialization(Controllers controllers, LocationInitialization locationInitialization, Data.Data data)
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
            
            var playerFactory = new PlayerFactory(data.Player);
            
            var inputInitialization = new InputInitialization();
            var playerInitialization = new PlayerInitialization(playerFactory, playerSpawn.transform.position);
            
            var uiFactory = new UIFactory(data.UIData, playerInitialization);
            var hudInitialization = new HudInitialization(uiFactory);
            var escapeMenuInitialization = new EscapeMenuInitilization(uiFactory);

            var axisInput = inputInitialization.GetAxisInput();
            var keysInput = inputInitialization.GetKeysInput();
            var mouseInput = inputInitialization.GetMouseInput();

            var inputController = new InputController(axisInput, keysInput, mouseInput);
            var carController = new CarController(axisInput, keysInput, playerInitialization, data.Player.Car);
            var weaponController = new WeaponsController(mouseInput, playerInitialization);
            var modificatorsController = new ModificatorsController(modificators, carController, data);
            var hudController = new HudController(hudInitialization, modificatorsController, carController, playerInitialization, data.Player);
            var radarController = new RadarController(playerInitialization, hudInitialization, data.Player.RadarSize);
            var escapeMenuController = new EscapeMenuController(escapeMenuInitialization, playerInitialization, carController, locationInitialization, keysInput);
            
            controllers.Add(playerInitialization);
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
                    hudController, locationChangers, keysInput);
                controllers.Add(locationChangerController);
            }
            
            if (pickups.Length != 0)
            {
                var pickupController = new PickupController(pickups, carController, weaponController);
                controllers.Add(pickupController);
            }

            if (walls.Length != 0)
            {
                var wallController = new WallController(walls, data.TargetData);
                controllers.Add(wallController);
            }
        }
    }
}