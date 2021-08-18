using UnityEngine;

namespace Code.Guns.Base
{
    internal interface IGun
    {
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        int Price { get; }
        
        int Damage { get; }
        float MaxDistance { get; }
        float TurnSpeed { get; }
        float FireRate { get; }
        int ClipAmmo { get; }
        int MaxAmmo { get; }
        bool InfinityAmmo { get; }
    }
}