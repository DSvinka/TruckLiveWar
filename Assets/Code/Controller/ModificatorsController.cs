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
        private readonly ModificatorData m_data;
        
        public float Cooldown;
        public GameObject GameObject;

        public ModificatorData Data => m_data;

        public Modificator(ModificatorData modificatorData)
        {
            m_data = modificatorData;
            Cooldown = modificatorData.ActiveTime;
            GameObject = null;
        }
    }

    internal sealed class ModificatorsController : IController, IInitialization, ICleanup, IExecute
    {
        private readonly ModificatorProvider[] m_modificators;
        private readonly CarController m_playerCarController;

        private readonly Modificator m_speedBonusModificator;
        private readonly Modificator m_speedSlowingDownModificator;
        private readonly Modificator m_playerKillerModificator;
        
        public event Action<Modificator> ModificatorCreate = delegate(Modificator modificator) {};
        public event Action<Modificator> ModificatorRemove = delegate(Modificator modificator) {};

        private List<Modificator> m_activeModificators;

        public ModificatorsController(ModificatorProvider[] modificators, CarController CarController, Data.Data data)
        {
            m_modificators = modificators;
            m_playerCarController = CarController;
            
            m_speedBonusModificator = new Modificator(data.SpeedBonus);
            m_speedSlowingDownModificator = new Modificator(data.SpeedSlowingDown);
            m_playerKillerModificator = new Modificator(data.PlayerKiller);
        }

        public void Initialization()
        {
            m_activeModificators = new List<Modificator>();

            foreach (var modificatorProvider in m_modificators)
            {
                modificatorProvider.OnTriggerEnterChange += OnTriggerEnter;
                modificatorProvider.OnTriggerExitChange += OnTriggerExit;
            }
        }

        private void OnTriggerEnter(GameObject gameObject, ModificatorProvider modificatorProvider, ModificatorType modificatorType)
        {
            if (m_playerCarController.CarProvider.gameObject.GetInstanceID() != gameObject.GetInstanceID()) 
                return;

            var modificator = modificatorType switch
            {
                ModificatorType.SpeedBonus => m_speedBonusModificator,
                ModificatorType.SpeedSlowingDown => m_speedSlowingDownModificator,
                ModificatorType.PlayerKiller => m_playerKillerModificator,
                _ => throw new Exception("Указанный тип модификатора не найден!")
            };

            AddModificator(modificator, modificatorProvider);
            ModificatorCreate.Invoke(modificator);
        }
        private void OnTriggerExit(GameObject gameObject, ModificatorProvider modificatorProvider, ModificatorType modificatorType)
        {
            if (m_playerCarController.CarProvider.gameObject.GetInstanceID() != gameObject.GetInstanceID()) 
                return;
            
            var modificator = modificatorType switch
            {
                ModificatorType.SpeedBonus => m_speedBonusModificator,
                ModificatorType.SpeedSlowingDown => m_speedSlowingDownModificator,
                ModificatorType.PlayerKiller => m_playerKillerModificator,
                _ => throw new Exception("Указанный тип модификатора не найден!")
            };
                
            RemoveModificator(modificator);
            ModificatorRemove.Invoke(modificator);
        }
        
        public void Execute(float deltatime)
        {
            if (m_activeModificators.Count == 0) 
                return;
            
            for (var index = 0; index < m_activeModificators.Count; index++)
            {
                var modificator = m_activeModificators[index];
                
                SetCarSettings(modificator);
                
                modificator.Cooldown -= deltatime;
                if (modificator.Cooldown < 0f)
                {
                    SetDefaultCarSettings();
                    m_activeModificators.Remove(modificator);
                    ModificatorRemove.Invoke(modificator);
                    break;
                }

                m_activeModificators[index] = modificator;
            }
        }
        public void Cleanup()
        {
            foreach (var modificatorProvider in m_modificators)
            {
                modificatorProvider.OnTriggerEnterChange -= OnTriggerEnter;
                modificatorProvider.OnTriggerEnterChange -= OnTriggerExit;
            }
        }

        private void AddModificator(Modificator modificator, ModificatorProvider modificatorProvider)
        {
            var data = modificator.Data;
            if (data.ZoneObject)
            {
                SetCarSettings(modificator);
            }
            else
            {
                m_activeModificators.Add(modificator);
                Destroy(modificatorProvider);
            }
        }
        private void RemoveModificator(Modificator modificator)
        {
            var data = modificator.Data;
            if (!data.ZoneObject) 
                return;
            
            SetDefaultCarSettings();
            if (data.ActivateAfterExit)
                m_activeModificators.Add(modificator);
        }

        private void SetCarSettings(Modificator modificator)
        {
            var data = modificator.Data;
            var changeHealth = data.ChangeHealth;
            var changeSpeed = data.ChangeSpeed;
            var permaKill = data.PermaKiller;
            
            if (changeHealth > 0)
                m_playerCarController.CarProvider.AddHealth(modificator.GameObject, changeHealth);
            else if (changeHealth < 0)
                m_playerCarController.CarProvider.AddDamage(modificator.GameObject, -changeHealth);
            
            if (permaKill)
                m_playerCarController.CarProvider.AddDamage(modificator.GameObject, m_playerCarController.CarProvider.Health);
            
            m_playerCarController.SpeedModificator = changeSpeed;
        }
        private void SetDefaultCarSettings()
        {
            m_playerCarController.SpeedModificator = 0f;
        }

        private void Destroy(ModificatorProvider modificatorProvider)
        {
            modificatorProvider.OnTriggerEnterChange -= OnTriggerEnter;
            modificatorProvider.OnTriggerEnterChange -= OnTriggerExit;
            Object.Destroy(modificatorProvider.gameObject);
        }
    }
}