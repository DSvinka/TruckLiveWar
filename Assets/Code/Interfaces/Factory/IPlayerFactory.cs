using Code.Data;
using Code.Providers;
using UnityEngine;

namespace Code.Interfaces.Factory
{
    internal interface IPlayerFactory: IFactory
    {
        Transform CreatePlayer();
        CarProvider CreateTransport();
        void ChangePlayerCar(CarData carProviderUnitData);
    }
}