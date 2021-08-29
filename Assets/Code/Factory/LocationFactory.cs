using System;
using Code.Data;
using Code.Interfaces.Data;
using Code.Interfaces.Factory;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

// TODO: СДЕЛАТЬ СМЕНУ ЛОКАЦИИ... Только я не знаю как это грамотно реализовать...
namespace Code.Factory
{
    internal sealed class LocationFactory : ILocationFactory
    {
        private readonly Data.Data m_data;

        public LocationFactory(Data.Data data)
        {
            m_data = data;
        }
        
        public Transform CreateLocation([CanBeNull] string nameID)
        {
            IDictData dictData;
            dictData = nameID == null ? m_data.LocationDatas["default"] : m_data.LocationDatas[nameID];
            
            var location = dictData as LocationData;
            if (location == null)
                throw new Exception("Локация не найдена!");
            
            var gameObject = Object.Instantiate(location.LocationPrefab);
            return gameObject.transform;
        }
    }
}