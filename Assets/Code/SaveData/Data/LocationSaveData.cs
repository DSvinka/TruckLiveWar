using System;
using Code.Providers;

namespace Code.SaveData.Data
{
    [Serializable]
    internal struct Modificator
    {
        public bool IsActive;
    }

    [Serializable]
    internal struct Target
    {
        public bool IsActive;
        public float Health;
    }
    
    [Serializable]
    internal struct Wall
    {
        public bool IsActive;
        public Target[] Targets;
    }
    
    [Serializable]
    internal struct Pickup
    {
        public bool IsActive;
    }
    
    [Serializable]
    internal sealed class LocationSaveData
    {
        public string LocationNameID;
        public Modificator[] Modificators;
        public Pickup[] Pickups;
        public Wall[] Walls;

        public void ModificatorsSet(ModificatorProvider[] modificatorProviders)
        {
            Modificators = new Modificator[modificatorProviders.Length];
            for (var index = 0; index < modificatorProviders.Length; index++)
            {
                var modificatorProvider = modificatorProviders[index];
                Modificators[index].IsActive = modificatorProvider.Parent.activeSelf;
            }
        }
        
        public void PickupsSet(PickupProvider[] pickupProviders)
        {
            Pickups = new Pickup[pickupProviders.Length];
            for (var index = 0; index < pickupProviders.Length; index++)
            {
                var pickupProvider = pickupProviders[index];
                Pickups[index].IsActive = pickupProvider.Parent.activeSelf;
            }
        }
        
        public void WallsSet(WallProvider[] wallProviders)
        {
            Walls = new Wall[wallProviders.Length];
            for (var index = 0; index < wallProviders.Length; index++)
            {
                var wallProvider = wallProviders[index];
                var wallTargets = wallProvider.TargetProviders;
                var newWallTargets = new Target[wallProvider.TargetProviders.Length];
                
                for (var i = 0; i < wallTargets.Length; i++)
                {
                    var target = wallTargets[i];
                    newWallTargets[i] = new Target
                    {
                        Health = target.Health,
                        IsActive = target.gameObject.activeSelf,
                    };
                }

                Walls[index].IsActive = wallProvider.gameObject.activeSelf;
                Walls[index].Targets = newWallTargets;
            }
        }
    }
}