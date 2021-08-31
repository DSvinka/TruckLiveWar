using Code.Controller.Initialization;
using Code.Interfaces;
using Code.Interfaces.UserInput;
using Code.Managers;
using Code.Providers;
using UnityEngine;

namespace Code.Controller
{
    internal sealed class LocationChangerController : IController, ICleanup, IInitialization, IExecute
    {
        private readonly IUserKeyProxy m_hornInputProxy;
        private readonly HudController m_hudController;
        private readonly PlayerInitialization m_playerInitialization;
        private readonly LocationInitialization m_locationInitialization;

        private LocationChangerProvider[] m_locationChangerProviders;
        private string m_locationIDName;
        private bool m_hornInput;
        private bool m_triggered;

        public LocationChangerController(PlayerInitialization playerInitialization, LocationInitialization locationInitialization,
            HudController hudController, LocationChangerProvider[] locationChangerProviders,
            (IUserKeyProxy inputHandbreak, IUserKeyProxy inputRestart, IUserKeyProxy inputHorn) keysInput)
        {
            m_hornInputProxy = keysInput.inputHorn;
            m_locationChangerProviders = locationChangerProviders;
            m_playerInitialization = playerInitialization;
            m_hudController = hudController;
            m_locationInitialization = locationInitialization;
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
            m_locationIDName = locationChangerProvider.LocationIDName;
            m_triggered = true;
        }
        
        private void OnTriggerExit(GameObject gameObject, LocationChangerProvider locationChangerProvider)
        {
            if (gameObject.gameObject.GetInstanceID() != m_playerInitialization.GetPlayerTransport().gameObject.GetInstanceID())
                return;
            
            m_hudController.RemoveMessage();
            m_locationIDName = null;
            m_triggered = false;
        }

        public void Execute(float deltaTime)
        {
            if (m_triggered && m_hornInput)
            {
                m_locationInitialization.ChangeLocation(m_locationIDName);
                m_triggered = false;
            }
        }
    }
}