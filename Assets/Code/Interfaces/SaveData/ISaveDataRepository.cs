using Code.Controller;
using Code.Controller.Initialization;

namespace Code.Interfaces.SaveData
{
    internal interface ISaveDataRepository
    {
        void Save(CarController player);
        void Load(PlayerInitialization playerInitialization, bool active);
    }
}