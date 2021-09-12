using System;
using System.Linq;
using Code.Data;
using Code.Interfaces;
using Code.Interfaces.Providers;
using Code.Providers;
using UnityEngine;

namespace Code.Controller
{
    internal sealed class WallController : IController, IInitialization, ICleanup
    {
        private static WallProvider[] s_wallProviders;
        private readonly TargetData m_targetData;

        public static WallProvider[] WallProviders => s_wallProviders;

        public WallController(WallProvider[] wallProviders, TargetData targetData)
        {
            s_wallProviders = wallProviders;
            m_targetData = targetData;
        }

        public void Initialization()
        {
            foreach (var wallProvider in s_wallProviders)
            {
                if (wallProvider.gameObject.activeSelf)
                {
                    wallProvider.TargetProviders = wallProvider.GetComponentsInChildren<TargetProvider>();
                    wallProvider.TargetCount = wallProvider.TargetProviders.Length;
                
                    foreach (var targetProvider in wallProvider.TargetProviders)
                    {
                        targetProvider.Wall = wallProvider;
                        targetProvider.UnitData = m_targetData;
                        targetProvider.Health = m_targetData.MaxHealth;
                        targetProvider.OnUnitDamage += OnTargetDamage;
                    }
                }
            }
        }

        private void OnTargetDamage(GameObject gameObject, IUnit unit, float damage)
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
                    wall.Explosion();
            }
        }

        public void Cleanup()
        {
            foreach (var targetProvider in s_wallProviders.SelectMany(wallProvider => wallProvider.TargetProviders))
            {
                targetProvider.OnUnitDamage -= OnTargetDamage;
            }
        }
    }
}