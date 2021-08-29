using System;
using Code.Controller;
using Code.Data;
using Code.Interfaces.Providers;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Providers
{
    public enum WheelSide
    {
        Left,
        Right,
    }

    [Serializable]
    public struct WheelAxie
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
    }

    [Serializable]
    public struct Wheel
    {
        [SerializeField] private GameObject m_wheelShape;
        [SerializeField] private WheelCollider m_wheelCollider;
        [SerializeField] [Tooltip("На какой стороне находится оружие")] private WheelSide m_wheelSide;

        public GameObject WheelShape => m_wheelShape;
        public WheelCollider WheelCollider => m_wheelCollider;
        public WheelSide WheelSide => m_wheelSide;
    }

    [Serializable]
    public struct WeaponSlot
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
    }

    internal sealed class CarProvider : MonoBehaviour, IUnit
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

        public WeaponSlot[] WeaponSlots => m_weaponSlots;
        public WheelAxie[] WheelAxies => m_wheelAxies;
        public Transform CameraFollow => m_cameraFollow;
        public Transform CameraLookAt => m_cameraLookAt;

        public WeaponProvider PlaceWeapon(int index, Weapon weapon)
        {
            var slot = m_weaponSlots[index];
            var weaponObject = Instantiate(weapon.WeaponProviderPrefab, slot.PlacePoint);
            slot.WeaponProvider = weaponObject;
            return weaponObject;
        }

        public void RemoveWeapon(int index)
        {
            var slot = m_weaponSlots[index];
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