using UnityEngine;

// TODO: СДЕЛАТЬ СМЕНУ ЛОКАЦИИ... Только я не знаю как это грамотно реализовать...
namespace Code.Interfaces.Factory
{
    internal interface ILocationFactory
    {
        Transform CreateLocation(string nameID);
    }
}