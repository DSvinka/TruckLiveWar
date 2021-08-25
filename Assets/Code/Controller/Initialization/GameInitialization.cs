using System;
using System.Runtime.InteropServices;
using Code.Factory;
using Code.Interfaces;
using Code.Providers;
using Object = UnityEngine.Object;

namespace Code.Controller.Initialization
{
    internal sealed class GameInitialization
    {
        public GameInitialization(Controllers controllers, Data.Data data)
        {
            var modificators = Object.FindObjectsOfType<ModificatorProvider>();
            var pickups = Object.FindObjectsOfType<PickupProvider>();
            var walls = Object.FindObjectsOfType<WallProvider>();
            
            var playerFactory = new PlayerFactory(data.Player);
            
            var inputInitialization = new InputInitialization();
            var playerInitialization = new PlayerInitialization(playerFactory, data.Player.SpawnPosition);
            
            var axisInput = inputInitialization.GetAxisInput();
            var keysInput = inputInitialization.GetKeysInput();
            var mouseInput = inputInitialization.GetMouseInput();

            var inputController = new InputController(axisInput, keysInput, mouseInput);
            var carController = new CarController(axisInput, keysInput, playerInitialization, data.Player.Car);
            var weaponController = new WeaponsController(mouseInput, playerInitialization, data.Player);
            
            controllers.Add(playerInitialization);
            controllers.Add(inputController);
            controllers.Add(carController);
            controllers.Add(weaponController);

            if (pickups.Length != 0)
            {
                var pickupController = new PickupController(pickups, carController, weaponController);
                controllers.Add(pickupController);
            }

            if (modificators.Length != 0)
            {
                var modificatorsController = new ModificatorsController(modificators, carController, data);
                controllers.Add(modificatorsController);
            }

            if (walls.Length != 0)
            {
                var wallController = new WallController(walls, data.TargetData);
                controllers.Add(wallController);
            }
        }
    }
}