using Code.Controller.Initialization;
using Code.Interfaces;
using Code.Interfaces.UserInput;
using Code.Managers;
using Code.Providers;
using UnityEngine;

// TODO: СДЕЛАТЬ СМЕНУ ЛОКАЦИИ... Только я не знаю как это грамотно реализовать...
namespace Code.Controller
{
    internal sealed class LocationChangerController : IController, ICleanup, IInitialization, IExecute
    {
        private readonly IUserKeyProxy m_hornInputProxy;
        private readonly HudInitialization m_hudInitialization;
        private readonly HudController m_hudController;
        private readonly PlayerInitialization m_playerInitialization;

        private LocationChangerProvider[] m_locationChangerProviders;
        private bool m_hornInput;
        private bool m_triggered;

        public LocationChangerController(HudInitialization hudInitialization, PlayerInitialization playerInitialization, 
            HudController hudController, LocationChangerProvider[] locationChangerProviders,
            (IUserKeyProxy inputHandbreak, IUserKeyProxy inputRestart, IUserKeyProxy inputHorn) keysInput)
        {
            m_hornInputProxy = keysInput.inputHorn;
            m_locationChangerProviders = locationChangerProviders;
            m_hudController = hudController;
            m_hudInitialization = hudInitialization;
            m_playerInitialization = playerInitialization;
        }

        public void Initialization()
        {
            m_hornInputProxy.KeyOnChange += OnHornKeyChange;

            foreach (var locationChangerProvider in m_locationChangerProviders)
            {
                locationChangerProvider.OnTriggerEnterChange += OnTriggerEnter;
                locationChangerProvider.OnTriggerExitChange += OnTriggerExit;
            }
        }

        public void Cleanup()
        {
            m_hornInputProxy.KeyOnChange -= OnHornKeyChange;
            
            foreach (var locationChangerProvider in m_locationChangerProviders)
            {
                locationChangerProvider.OnTriggerEnterChange -= OnTriggerEnter;
                locationChangerProvider.OnTriggerExitChange -= OnTriggerExit;
            }
        }

        private void OnHornKeyChange(bool value)
        {
            m_hornInput = value;
        }
        
        private void OnTriggerEnter(GameObject gameObject, LocationChangerProvider locationChangerProvider)
        {
            if (gameObject.gameObject.GetInstanceID() != m_playerInitialization.GetPlayerTransport().gameObject.GetInstanceID())
                return;
            
            m_hudController.SetMessage(MessagesManager.CHANGE_LOCATION_MESSAGE);
            m_triggered = true;
        }
        
        private void OnTriggerExit(GameObject gameObject, LocationChangerProvider locationChangerProvider)
        {
            if (gameObject.gameObject.GetInstanceID() != m_playerInitialization.GetPlayerTransport().gameObject.GetInstanceID())
                return;
            
            m_hudController.RemoveMessage();
            m_triggered = false;
        }

        public void Execute(float deltaTime)
        {
            if (m_triggered && m_hornInput)
            {
                m_hudInitialization.DisableAllHud();
                m_hudInitialization.GetWinHud().gameObject.SetActive(true);
                m_triggered = false;
            }
        }
    }
}