using Code.Factory;
using UnityEngine;

// TODO: СДЕЛАТЬ СМЕНУ ЛОКАЦИИ... Только я не знаю как это грамотно реализовать...
namespace Code.Controller.Initialization
{
    internal sealed class LocationInitialization
    {
        private readonly LocationFactory m_locationFactory;
        private readonly Data.Data m_data;
        
        private Transform m_currentLocation;
        
        public LocationInitialization(LocationFactory LocationFactory, Data.Data Data)
        {
            m_locationFactory = LocationFactory;
            m_data = Data;
        }

        public void LoadLocation(string nameID)
        {
            if (m_currentLocation != null)
                Object.Destroy(m_currentLocation.gameObject);

            m_currentLocation = m_locationFactory.CreateLocation(nameID);
        }
    }
}