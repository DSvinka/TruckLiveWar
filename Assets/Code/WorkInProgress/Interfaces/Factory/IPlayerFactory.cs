using Code.Providers;
using UnityEngine;

namespace Code.Interfaces.Factory
{
    public interface IPlayerFactory: IFactory
    {
        Transform CreatePlayer();
        CarProvider CreateTransport();
    }
}