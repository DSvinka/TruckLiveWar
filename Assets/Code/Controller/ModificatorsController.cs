using System;
using System.Collections.Generic;
using Code.Data;
using Code.Interfaces;
using Code.Providers;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Controller
{
    internal sealed class Modificator
    {
        private ModificatorData m_modificatorData;

        public ModificatorData ModificatorData => m_modificatorData;

        public float Timer { get; set; }
        public GameObject GameObject { get; }

        public Modificator(ModificatorData modificatorData, GameObject gameObject)
        {
            m_modificatorData = modificatorData;
            Timer = modificatorData.ActiveTime;
            GameObject = gameObject;
        }
    }
    
    internal sealed class ModificatorsController : IController, IInitialization, ICleanup, IExecute
    {
        private static ModificatorProvider[] m_modificators;
        private readonly CarController m_playerCarController;

        public static ModificatorProvider[] ModificatorProviders => m_modificators;
        
        public event Action<Modificator> ModificatorCreate = delegate(Modificator modificator) {};
        public event Action<Modificator> ModificatorRemove = delegate(Modificator modificator) {};

        private List<Modificator> m_activeModificators;

        public ModificatorsController(ModificatorProvider[] modificators, CarController CarController, Data.Data data)
        {
            m_modificators = modificators;
            m_playerCarController = CarController;
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

        private void OnTriggerEnter(GameObject gameObject, ModificatorProvider modificatorProvider, ModificatorData modificatorData)
        {
            if (m_playerCarController.CarProvider.gameObject.GetInstanceID() != gameObject.GetInstanceID()) 
                return;
            
            var modificator = new Modificator(modificatorData, modificatorProvider.Parent);
            
            modificatorProvider.Modificator = modificator;
            AddModificator(modificator, modificatorProvider);
            ModificatorCreate.Invoke(modificator);
        }
        private void OnTriggerExit(GameObject gameObject, ModificatorProvider modificatorProvider, Modificator modificator)
        {
            if (m_playerCarController.CarProvider.gameObject.GetInstanceID() != gameObject.GetInstanceID()) 
                return;

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
                
                modificator.Timer -= deltatime;
                if (modificator.Timer < 0f)
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
                modificatorProvider.OnTriggerExitChange -= OnTriggerExit;
            }
        }

        private void AddModificator(Modificator modificator, ModificatorProvider modificatorProvider)
        {
            var data = modificator.ModificatorData;
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
            var data = modificator.ModificatorData;
            if (!data.ZoneObject) 
                return;
            
            SetDefaultCarSettings();
            if (data.ActivateAfterExit)
                m_activeModificators.Add(modificator);
        }

        private void SetCarSettings(Modificator modificator)
        {
            var data = modificator.ModificatorData;
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
            modificatorProvider.OnTriggerExitChange -= OnTriggerExit;
            modificatorProvider.Parent.SetActive(false);
        }
    }
}