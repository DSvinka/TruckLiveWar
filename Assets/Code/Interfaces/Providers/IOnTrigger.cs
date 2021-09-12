using System;
using Code.Controller;
using Code.Data;
using Code.Providers;
using UnityEngine;

namespace Code.Interfaces.Providers
{
    internal interface IModificatorProvider
    {
        event Action<GameObject, ModificatorProvider, ModificatorData> OnTriggerEnterChange;
        event Action<GameObject, ModificatorProvider, Modificator> OnTriggerExitChange;
    }

    internal interface IPickupProvider
    {
        event Action<GameObject, PickupProvider> OnTriggerEnterChange;
    }

    internal interface ILocationChangerProvider
    {
        event Action<GameObject, LocationChangerProvider> OnTriggerEnterChange;
        event Action<GameObject, LocationChangerProvider> OnTriggerExitChange;
    }
}