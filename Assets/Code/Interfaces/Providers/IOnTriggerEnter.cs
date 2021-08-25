using System;
using Code.Data;
using Code.Providers;
using UnityEngine;

namespace Code.Interfaces.Providers
{
    internal interface IModificatorProvider
    {
        event Action<GameObject, ModificatorProvider, ModificatorType> OnTriggerEnterChange;
        event Action<GameObject, ModificatorProvider, ModificatorType> OnTriggerExitChange;
    }
    
    internal interface IPickupProvider 
    {
        event Action<GameObject, PickupProvider> OnTriggerEnterChange;
    }
}