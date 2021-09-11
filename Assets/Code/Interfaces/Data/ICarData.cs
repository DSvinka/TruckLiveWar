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
        
        public float NaturalFrequency { get; }
        public float DampingRatio { get; }
        public float ForceShift { get; }
        public bool SetSuspensionDistance { get; }

        public string Name { get; }
        public float MaxHealth { get; }

        CarProvider CarPrefab { get; }
    }
}