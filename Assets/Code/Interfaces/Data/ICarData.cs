using Code.Providers;

namespace Code.Interfaces.Data
{
    internal interface ICarData
    {
        public float MaxAngle { get; }
        float MaxTorque { get; }
        float BrakeTorque { get; }

        float CriticalSpeed { get; }
        int StepsBelow { get; }
        int StepsAbove { get; }

        public string Name { get; }
        public float MaxHealth { get; }

        CarProvider CarPrefab { get; }
    }
}