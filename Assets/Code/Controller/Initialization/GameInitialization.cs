using System;
using Code.Controller.Starter;
using Code.Factory;
using Code.Markers;
using Code.Providers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controller.Initialization
{
    internal sealed class GameInitialization
    {
        private Controllers m_controllers;
        private Data.Data m_data;
        
        public GameInitialization(Controllers controllers, Data.Data data)
        {
            m_controllers = controllers;
            m_data = data;
            
            var modificators = Object.FindObjectsOfType<ModificatorProvider>();
            var pickups = Object.FindObjectsOfType<PickupProvider>();
            var walls = Object.FindObjectsOfType<WallProvider>();
            var locationChangers = Object.FindObjectsOfType<LocationChangerProvider>();

            var playerSpawn = Object.FindObjectOfType<PlayerSpawn>();
            if (playerSpawn == null)
                throw new Exception("Спавнер игрока отсуствует!");
            
            var playerFactory = new PlayerFactory(data.Player);
            var hudFactory = new HudFactory(data.HudData);
            
            var inputInitialization = new InputInitialization();
            var playerInitialization = new PlayerInitialization(playerFactory, playerSpawn.transform.position);
            var hudInitialization = new HudInitialization(hudFactory);

            var axisInput = inputInitialization.GetAxisInput();
            var keysInput = inputInitialization.GetKeysInput();
            var mouseInput = inputInitialization.GetMouseInput();

            var inputController = new InputController(axisInput, keysInput, mouseInput);
            var carController = new CarController(axisInput, keysInput, playerInitialization, data.Player.Car);
            var weaponController = new WeaponsController(mouseInput, playerInitialization);
            var modificatorsController = new ModificatorsController(modificators, carController, data);
            var hudController = new HudController(hudInitialization, modificatorsController, carController, playerInitialization, data.Player);
            
            controllers.Add(playerInitialization);
            controllers.Add(hudInitialization);
            
            controllers.Add(inputController);
            controllers.Add(carController);
            controllers.Add(weaponController);
            controllers.Add(modificatorsController);
            controllers.Add(hudController);

            if (locationChangers.Length != 0)
            {
                var locationChangerController = new LocationChangerController(hudInitialization, playerInitialization,
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