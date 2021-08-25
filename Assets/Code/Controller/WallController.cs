using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Code.Data;
using Code.Interfaces;
using Code.Interfaces.Providers;
using Code.Providers;
using Unity.Collections;
using UnityEngine;

namespace Code.Controller
{
    internal struct Target
    {
        private readonly TargetProvider _targetProvider;
        private readonly WallProvider _wall;
    }
    
    internal sealed class WallController: IController, IInitialization, ICleanup
    {
        private readonly List<WallProvider> _wallProviders;
        private readonly TargetData _targetData;

        public WallController(WallProvider[] wallProviders, TargetData targetData)
        {
            _wallProviders = wallProviders.ToList();
            _targetData = targetData;
        }
        
        public void Initialization()
        {
            foreach (var wallProvider in _wallProviders)
            {
                wallProvider.TargetCount = wallProvider.TargetProviders.Length;
                foreach (var targetProvider in wallProvider.TargetProviders)
                {
                    targetProvider.Wall = wallProvider;
                    targetProvider.Health = _targetData.MaxHealth;
                    targetProvider.OnUnitDamage += OnTargetDamage;
                }
            }
        }

        public void OnTargetDamage(GameObject gameObject, IUnit unit, float damage)
        {
            var target = unit as TargetProvider;
            if (target == null)
                throw new Exception("unit не является TargetProvider'ом");
                
            target.Health -= damage;
            if (target.Health <= 0)
            {
                var wall = target.Wall;
                
                target.OnUnitDamage -= OnTargetDamage;
                target.Explosion();
                wall.TargetCount -= 1;
                if (wall.TargetCount == 0)
                {
                    _wallProviders.Remove(wall);
                    wall.Explosion();
                }
            }
                
        }

        public void Cleanup()
        {
            foreach (var wallProvider in _wallProviders)
            {
                foreach (var targetProvider in wallProvider.TargetProviders)
                {
                    targetProvider.OnUnitDamage -= OnTargetDamage;
                }
            }
        }
    }
}