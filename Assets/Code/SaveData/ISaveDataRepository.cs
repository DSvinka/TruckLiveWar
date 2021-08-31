using Code.Controller;

namespace Code.SaveData
{
    internal interface ISaveDataRepository
    {
        void Save(CarController player);
        void Load(CarController player);
    }
}