using System;
using System.Data;
using Code.Client;
using UnityEngine;

namespace Code.Guns.Base
{
    internal sealed class Gun : MonoBehaviour, IGun
    {
        [Header("Информация")] [SerializeField]
        private string _name = "Птичка";

        [SerializeField] [TextArea] private string _description = "Самое лучшее оружие в игре!";
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _price = 100;

        [Header("Характеристики")] [SerializeField]
        private int _damage = 10;

        [SerializeField] private float _maxDistance = 100;

        [SerializeField] [Tooltip("Скорострельность оружия (в секундах)")]
        private float _fireRate = 0.5f;

        [SerializeField] [Range(0.0f, 1.0f)] private float _turnSpeed = 0.1f;

        [SerializeField] [Tooltip("Максимальное Количество патрон в обойме")]
        private int _clipAmmo = 60;

        [SerializeField] [Tooltip("Максимальное Количество патрон в запасе (не работает при InfinityAmmo)")]
        private int _maxAmmo = 360;

        [SerializeField] [Tooltip("Включение бесконечного количества патрон в запасе")]
        private bool _infinityAmmo = true;

        // TODO: Добавить инвентарь для информации о оружии
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public int Price => _price;
        public int Damage => _damage;
        public float MaxDistance => _maxDistance;
        public float FireRate => _fireRate;
        public float TurnSpeed => _turnSpeed;

        // TODO: Добавить патроны
        public int ClipAmmo => _clipAmmo;
        public int MaxAmmo => _maxAmmo;
        public bool InfinityAmmo => _infinityAmmo;

        private GunController _gunController;

        private void Awake()
        {
            _gunController = GetComponent<GunController>();
        }

        public void Init(Player player)
        {
            _gunController.Player = player;
        }
    }
}