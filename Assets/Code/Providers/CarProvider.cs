using System;
using Code.Controller;
using Code.Data;
using Code.Interfaces.Data;
using Code.Interfaces.Providers;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Providers
{
    public enum WheelSide
    {
        Left,
        Right,
    }

    [Serializable]
    public class WheelAxie
    {
        [SerializeField] private Wheel[] m_wheels;

        [Header("Настройка")]
        [SerializeField] private bool m_isMotorAxie;
        [SerializeField] private bool m_isHandbreakAxie;
        [SerializeField] private bool m_isSteeringAxie;

        public Wheel[] Wheels => m_wheels;

        public bool IsMotorAxie => m_isMotorAxie;
        public bool IsHandbreakAxie => m_isHandbreakAxie;
        public bool IsSteeringAxie => m_isSteeringAxie;
        
        public WheelAxie(Wheel[] wheels, bool isMotorAxie = true, bool isHandbreakAxie = true, bool isSteeringAxie = false)
        {
            m_wheels = wheels;
            m_isMotorAxie = isMotorAxie;
            m_isHandbreakAxie = isHandbreakAxie;
            m_isSteeringAxie = isSteeringAxie;
        }
    }

    [Serializable]
    public struct Wheel
    {
        [SerializeField] private GameObject m_wheelShape;
        [SerializeField] private WheelCollider m_wheelCollider;
        [SerializeField] [Tooltip("На какой стороне находится колесо")] private WheelSide m_wheelSide;

        public GameObject WheelShape => m_wheelShape;
        public WheelCollider WheelCollider => m_wheelCollider;
        public WheelSide WheelSide => m_wheelSide;

        public Wheel(GameObject wheelShape, WheelCollider wheelCollider, WheelSide wheelSide)
        {
            m_wheelShape = wheelShape;
            m_wheelCollider = wheelCollider;
            m_wheelSide = wheelSide;
        }
    }

    [Serializable]
    public class WeaponSlot
    {
        [Header("Объекты")]
        [SerializeField] [Tooltip("Слот")] private Transform m_slot;
        [SerializeField] [Tooltip("Место куда будет устанавливаться оружие")] private Transform m_placePoint;

        [Header("Настройка")]
        [SerializeField] [Tooltip("Тип оружия которое может быть установлено на этот слот")]
        private WeaponSlotType m_slotType;

        public Transform Slot => m_slot;
        public Transform PlacePoint => m_placePoint;
        public WeaponSlotType SlotType => m_slotType;
        public WeaponProvider WeaponProvider { get; set; }
        public Weapon Weapon { get; set; }
    }

    public sealed class CarProvider : MonoBehaviour, IUnit
    {
        [Header("Объекты отслеживания камеры")]
        [SerializeField] private Transform m_cameraFollow;
        [SerializeField] private Transform m_cameraLookAt;

        [Header("Настройка осей колёс")]
        [SerializeField] private WheelAxie[] m_wheelAxies;

        [Header("Настройка слотов для оружия")]
        [SerializeField] private WeaponSlot[] m_weaponSlots;

        public event Action<GameObject, IUnit, float> OnUnitDamage = delegate(GameObject attacker, IUnit unit, float damage) { };
        public event Action<GameObject, IUnit, float> OnUnitHealth = delegate(GameObject healer, IUnit unit, float health) { };

        public IUnitData UnitData { get; set; }

        public float Health { get; set; }

        public WeaponSlot[] WeaponSlots => m_weaponSlots;
        public WheelAxie[] WheelAxies => m_wheelAxies;
        public Transform CameraFollow => m_cameraFollow;
        public Transform CameraLookAt => m_cameraLookAt;
        
        [Obsolete("Не рекомендуется использовать где либо кроме контроллера WeaponController. Эта функция нужна для работы Контроллера с Провайдером.")]
        public WeaponProvider PlaceWeapon(int index, Weapon weapon)
        {
            var slot = m_weaponSlots[index];

            var weaponObject = Instantiate(weapon.WeaponProviderPrefab, slot.PlacePoint);

            slot.Weapon = weapon;
            slot.WeaponProvider = weaponObject;

            return weaponObject;
        }

        [Obsolete("Не рекомендуется использовать где либо кроме контроллера WeaponController. Эта функция нужна для работы Контроллера с Провайдером.")]
        public void RemoveWeapon(int index)
        {
            var slot = m_weaponSlots[index];
            if (slot.WeaponProvider != null)
                Destroy(slot.WeaponProvider.gameObject);
            
            slot.WeaponProvider = null;
        }

        // TODO: Сделать эффект взрыва
        public void Explosion()
        {
            Destroy(gameObject);
        }

        public void AddDamage(GameObject damager, float damage)
        {
            OnUnitDamage.Invoke(damager, this, damage);
        }

        public void AddHealth(GameObject healer, float health)
        {
            OnUnitHealth.Invoke(healer, this, health);
        }
    }
}