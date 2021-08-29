namespace Code.Interfaces.Data
{
    public interface IUnit
    {
        public string Name { get; }
        public float Health { get; }
        public float MaxHealth { get; }
        public float Fuel { get; }
        public float MaxFuel { get; }
        public bool InfinityFuel { get; }
    }
}