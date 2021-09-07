namespace Code.Interfaces.Data
{
    public interface IUnitData
    {
        public string Name { get; }
        public float MaxHealth { get; }
        public float MaxFuel { get; }
        public bool InfinityFuel { get; }
    }
}