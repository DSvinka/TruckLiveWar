using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Interfaces;
using Code.Providers;

namespace Code.Controller
{
    internal struct Modificator
    {
        private ModificatorData _modificatorData;
        private CarController _carController;
        public int Timer;
        
        public ModificatorData GetModificatorData()
        {
            return _modificatorData;
        }
        public CarController GetCarController()
        {
            return _carController;
        }

        public Modificator(ModificatorData modificatorData, CarController carController)
        {
            _modificatorData = modificatorData;
            _carController = carController;
            Timer = modificatorData.ActiveTime;
        }
    }
    
    internal class ModificatorsController : IController, IInitialization, ICleanup, IExecute
    {
        private ModificatorProvider[] _modificators;
        private Data.Data _data;
        private CarController _playerCarController;

        private Modificator _speedBonusModificator;
        private Modificator _speedSlowingDownModificator;
        private Modificator _playerKillerModificator;

        private List<Modificator> _activeModificators;
        
        public ModificatorsController(ModificatorProvider[] modificators, CarController CarController, Data.Data data)
        {
            _modificators = modificators;
            _playerCarController = CarController;
            _data = data;
        }

        public void Initialization()
        {
            _activeModificators = new List<Modificator>();

            _speedBonusModificator = new Modificator(_data.SpeedBonus, _playerCarController);
            _speedSlowingDownModificator = new Modificator(_data.SpeedSlowingDown, _playerCarController);
            _playerKillerModificator = new Modificator(_data.PlayerKiller, _playerCarController);

            foreach (var modificatorProvider in _modificators)
            {
                modificatorProvider.OnTriggerEnterChange += OnTriggerEnter;
                modificatorProvider.OnTriggerEnterChange += OnTriggerExit;
            }
        }

        // Сделать с таймером в Update, Типо список модификаторов и в них есть таймеры через deltatime которые проверяются.
        // Сделать проверку по gameObjectID.
        private void OnTriggerEnter(int gameObjectID, ModificatorType modificatorType)
        {
            if (_playerCarController.CarProvider.gameObject.GetInstanceID() == gameObjectID)
            {
                switch (modificatorType)
                {
                    case ModificatorType.SpeedBonus:
                        var speedBonus = _data.SpeedBonus;

                        if (speedBonus.ZoneObject)
                            _playerCarController.SpeedModificator = speedBonus.ChangeSpeed;
                        else
                            _activeModificators.Add(_speedBonusModificator);
                        break;
                
                    case ModificatorType.SpeedSlowingDown:
                        var speedSlowingDown = _data.SpeedSlowingDown;

                        if (speedSlowingDown.ZoneObject)
                            _playerCarController.SpeedModificator = speedSlowingDown.ChangeSpeed;
                        else
                            _activeModificators.Add(_speedSlowingDownModificator);
                        break;
                
                    case ModificatorType.PlayerKiller:
                        var playerKiller = _data.PlayerKiller;
                        if (playerKiller.ZoneObject)
                            _activeModificators.Add(_playerKillerModificator);
                        else
                            _playerCarController.AddDamage(playerKiller.ChangeHealth);
                        
                        break;
                    
                    default:
                        throw new Exception("Указанный в объекте тип модификатора не найден!");
                }
            }
        }
        
        private void OnTriggerExit(int gameObjectID, ModificatorType modificatorType)
        {
            if (_playerCarController.CarProvider.gameObject.GetInstanceID() == gameObjectID)
            {
                switch (modificatorType)
                {
                    case ModificatorType.SpeedBonus:
                        var speedBonus = _data.SpeedBonus;
                        if (speedBonus.ZoneObject)
                        {
                            _playerCarController.SpeedModificator = 0;
                            _activeModificators.Add(_speedBonusModificator);
                        }
                            
                        break;
                    case ModificatorType.SpeedSlowingDown:
                        var speedSlowingDown = _data.SpeedSlowingDown;
                        if (speedSlowingDown.ZoneObject)
                        {
                            _playerCarController.SpeedModificator = 0;
                            _activeModificators.Add(_speedSlowingDownModificator);
                        }
                        
                        break;
                    case ModificatorType.PlayerKiller:
                        var playerKiller = _data.PlayerKiller;
                        if (playerKiller.ZoneObject)
                            _activeModificators.Remove(_speedSlowingDownModificator);
                        break;
                    default:
                        throw new Exception("Указанный в объекте тип модификатора не найден!");
                }
            }
        }

        public void Execute(float deltatime)
        {
            if (_activeModificators.Count() != 0)
            {
                foreach (var modificator in _activeModificators)
                {
                    
                }
            }
        }

        public void Cleanup()
        {
            foreach (var modificatorProvider in _modificators)
            {
                modificatorProvider.OnTriggerEnterChange -= OnTriggerEnter;
                modificatorProvider.OnTriggerEnterChange -= OnTriggerExit;
            }
        }
    }
}