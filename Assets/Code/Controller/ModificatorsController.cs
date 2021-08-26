using System;
using System.Collections.Generic;
using System.Linq;
using Code.Data;
using Code.Interfaces;
using Code.Providers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controller
{
    internal sealed class Modificator
    {
        private ModificatorData m_modificatorData;

        private float m_changeSpeed;
        private float m_changeHealth;

        public float Cooldown;
        public GameObject GameObject;

        public float ChangeSpeed() => m_changeSpeed;
        public float ChangeHealth() => m_changeHealth;

        public ModificatorData ModificatorData() => m_modificatorData;

        public Modificator(ModificatorData modificatorData)
        {
            m_modificatorData = modificatorData;

            m_changeHealth = modificatorData.ChangeHealth;
            m_changeSpeed = modificatorData.ChangeSpeed;
            Cooldown = modificatorData.ActiveTime;
            GameObject = null;
        }
    }

    internal sealed class ModificatorsController : IController, IInitialization, ICleanup, IExecute
    {
        private readonly ModificatorProvider[] _modificators;
        private readonly Data.Data _data;
        private readonly CarController _playerCarController;

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

            _speedBonusModificator = new Modificator(_data.SpeedBonus);
            _speedSlowingDownModificator = new Modificator(_data.SpeedSlowingDown);
            _playerKillerModificator = new Modificator(_data.PlayerKiller);

            foreach (var modificatorProvider in _modificators)
            {
                modificatorProvider.OnTriggerEnterChange += OnTriggerEnter;
                modificatorProvider.OnTriggerEnterChange += OnTriggerExit;
            }
        }

        private void OnTriggerEnter(GameObject gameObject, ModificatorProvider modificatorProvider, ModificatorType modificatorType)
        {
            var carProvider = gameObject.GetComponentInParent<CarProvider>();
            if (_playerCarController.CarProvider.gameObject.GetInstanceID() == carProvider.gameObject.GetInstanceID())
            {
                switch (modificatorType)
                {
                    case ModificatorType.SpeedBonus:
                        var speedBonus = _data.SpeedBonus;

                        if (speedBonus.ZoneObject)
                        {
                            _playerCarController.SpeedModificator = speedBonus.ChangeSpeed;
                        }
                        else
                        {
                            _activeModificators.Add(_speedBonusModificator);
                            Destroy(modificatorProvider);
                        }
                        break;

                    case ModificatorType.SpeedSlowingDown:
                        var speedSlowingDown = _data.SpeedSlowingDown;

                        if (speedSlowingDown.ZoneObject)
                        {
                            _playerCarController.SpeedModificator = speedSlowingDown.ChangeSpeed;
                        }
                        else
                        {
                            _activeModificators.Add(_speedSlowingDownModificator);
                            Destroy(modificatorProvider);
                        }
                        break;

                    case ModificatorType.PlayerKiller:
                        var playerKiller = _data.PlayerKiller;
                        if (playerKiller.ZoneObject)
                        {
                            _activeModificators.Add(_playerKillerModificator);
                        }
                        else
                        {
                            SetCarSettings(_playerKillerModificator);
                            Destroy(modificatorProvider);
                        }
                        break;

                    default:
                        throw new Exception("Указанный в объекте тип модификатора не найден!");
                }
            }
        }

        private void OnTriggerExit(GameObject gameObject, ModificatorProvider modificatorProvider, ModificatorType modificatorType)
        {
            var carProvider = gameObject.GetComponentInParent<CarProvider>();
            if (_playerCarController.CarProvider.gameObject.GetInstanceID() == carProvider.gameObject.GetInstanceID())
            {
                switch (modificatorType)
                {
                    case ModificatorType.SpeedBonus:
                        var speedBonus = _data.SpeedBonus;
                        if (speedBonus.ZoneObject)
                        {
                            SetDefaultCarSettings();
                            _activeModificators.Add(_speedSlowingDownModificator);
                        }
                        break;

                    case ModificatorType.SpeedSlowingDown:
                        var speedSlowingDown = _data.SpeedSlowingDown;
                        if (speedSlowingDown.ZoneObject)
                        {
                            SetDefaultCarSettings();
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
            if (_activeModificators.Count != 0)
            {
                Debug.Log(_activeModificators.Count);
                for (var index = 0; index < _activeModificators.Count; index++)
                {
                    var modificator = _activeModificators[index];

                    var time = modificator.Cooldown;

                    SetCarSettings(modificator);
                    modificator.Cooldown -= deltatime;

                    if (time < 0f)
                    {
                        SetDefaultCarSettings();
                        _activeModificators.Remove(modificator);
                        break;
                    }
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

        private void SetDefaultCarSettings()
        {
            _playerCarController.SpeedModificator = 0f;
        }

        private void SetCarSettings(Modificator modificator)
        {
            var changeHealth = modificator.ChangeHealth();
            var changeSpeed = modificator.ChangeSpeed();
            
            if (changeHealth > 0)
                _playerCarController.CarProvider.AddHealth(modificator.GameObject, changeHealth);
            else if (changeHealth < 0)
                _playerCarController.CarProvider.AddDamage(modificator.GameObject, -changeHealth);
            
            _playerCarController.SpeedModificator = changeSpeed;
        }

        private void Destroy(ModificatorProvider modificatorProvider)
        {
            modificatorProvider.OnTriggerEnterChange -= OnTriggerEnter;
            modificatorProvider.OnTriggerEnterChange -= OnTriggerExit;
            Object.Destroy(modificatorProvider.gameObject);
        }
    }
}