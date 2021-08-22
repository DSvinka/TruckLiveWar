using System;
using UnityEngine;

namespace Code.Providers
{
    public enum WheelSide
    {
        Left,
        Right,
    }

    public enum WeaponType
    {
        Small
    }

    [Serializable]
    public struct WheelAxie
    {
        [SerializeField] private Wheel[] _wheels;

        [Header("Настройка")] 
        [SerializeField] private bool _isMotorAxie;
        [SerializeField] private bool _isHandbreakAxie;
        [SerializeField] private bool _isSteeringAxie;

        public Wheel[] Wheels => _wheels;

        public bool IsMotorAxie => _isMotorAxie;
        public bool IsHandbreakAxie => _isHandbreakAxie;
        public bool IsSteeringAxie => _isSteeringAxie;
    }

    [Serializable]
    public struct Wheel
    {
        [SerializeField] private GameObject _wheelShape;
        [SerializeField] private WheelCollider _wheelCollider;
        [SerializeField, Tooltip("На какой стороне находится оружие")] private WheelSide _wheelSide;

        public GameObject WheelShape => _wheelShape;
        public WheelCollider WheelCollider => _wheelCollider;
        public WheelSide WheelSide => _wheelSide;
    }
    
    [Serializable]
    public struct WeaponSlot
    {
        [Header("Объекты")]
        [SerializeField, Tooltip("Слот")] private Transform _slot;
        [SerializeField, Tooltip("Место куда будет устанавливаться оружие")] private Transform _placePoint;

        [Header("Настройка")]
        [SerializeField, Tooltip("Тип оружия которое может быть установлено на этот слот")] 
        private WeaponType _weaponType;

        public Transform Slot => _slot;
        public Transform PlacePoint => _placePoint;
        public WeaponType WeaponType => _weaponType;
    }

    // TODO: Может можно сделать лучше
    public sealed class CarProvider: MonoBehaviour
    {
        [Header("Объекты отслеживания камеры")]
        [SerializeField] private Transform _cameraFollow;
        [SerializeField] private Transform _cameraLookAt;
        
        [Header("Настройка осей колёс")]
        [SerializeField] private WheelAxie[] _wheelAxies;
        
        [Header("Настройка слотов для оружия")]
        [SerializeField] private WeaponSlot[] _weaponSlots;

        public WeaponSlot[] WeaponSlots => _weaponSlots;
        public WheelAxie[] WheelAxies => _wheelAxies;
        public Transform CameraFollow => _cameraFollow;
        public Transform CameraLookAt => _cameraLookAt;
        
        // TODO: Сделать эффект взрыва
        public void Explosion()
        {
            Destroy(gameObject);
        }
    }
}