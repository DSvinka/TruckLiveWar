using System;
using Code.Data;

namespace Code.Interfaces.Providers
{
    public interface IModificatorProvider
    {
        event Action<int, ModificatorType> OnTriggerEnterChange;
        event Action<int, ModificatorType> OnTriggerExitChange;
    }
}