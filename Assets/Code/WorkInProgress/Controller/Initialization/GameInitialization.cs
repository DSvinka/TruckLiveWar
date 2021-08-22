using System;
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
            
            var playerFactory = new PlayerFactory(data.Player);
            
            var inputInitialization = new InputInitialization();
            var playerInitialization = new PlayerInitialization(playerFactory, data.Player.SpawnPosition);

            var carController = new CarController(inputInitialization.GetInput(), playerInitialization.GetPlayerTransport(), data.Player.Car);
            var inputController = new InputController(inputInitialization.GetInput());
            var modificatorsController = new ModificatorsController(modificators, carController, data);

            controllers.Add(inputInitialization);
            controllers.Add(playerInitialization);
            
            controllers.Add(inputController);
            controllers.Add(carController);
            if (modificators != null)
                controllers.Add(modificatorsController);
        }
    }
}