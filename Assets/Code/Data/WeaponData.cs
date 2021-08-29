using Code.Controller;
using Code.Providers;
using UnityEngine;

namespace Code.Data
{
    internal enum WeaponSlotType
    {
        Small,
        Middle
    }

    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "Data/Pickup/WeaponSettings")]
    internal sealed class WeaponData : ScriptableObject
    {
        [SerializeField] private WeaponProvider m_prefab;

        [Header("Информация")]
        [SerializeField] private string m_name = "Птичка";
        [SerializeField] [TextArea] private string m_description = "Скорострельный пистолет-пулемёт";
        [SerializeField] private Sprite m_icon;
        [SerializeField] private int m_price = 100;

        [Header("Характеристики")]
        [SerializeField] private int m_damage = 10;
        [SerializeField] private float m_maxDistance = 100;

        [SerializeField] [Tooltip("Размер слота в которое можно поместить оружие")]
        private WeaponSlotType m_slotType = WeaponSlotType.Small;

        [SerializeField] [Tooltip("Скорострельность оружия (Промежуток между выстрелами в секундах)")]
        private float m_fireRate = 0.3f;

        [SerializeField] [Range(0.0f, 1.0f)]
        private float m_turnSpeed = 0.1f;

        [SerializeField] [Tooltip("Максимальное Количество патрон в обойме")]
        private int m_clipAmmo = 32;

        [SerializeField] [Tooltip("Максимальное Количество патрон в запасе (не работает при InfinityAmmo)")]
        private int m_maxAmmo = 360;

        [SerializeField] [Tooltip("Включение бесконечного количества патрон в запасе")]
        private bool m_infinityAmmo = true;

        // TODO: Добавить инвентарь для информации о оружии
        public string Name => m_name;
        public string Description => m_description;
        public Sprite Icon => m_icon;
        public int Price => m_price;
        public int Damage => m_damage;
        public float MaxDistance => m_maxDistance;
        public WeaponSlotType SlotType => m_slotType;
        public float FireRate => m_fireRate;
        public float TurnSpeed => m_turnSpeed;

        public WeaponProvider Prefab => m_prefab;

        // TODO: Добавить патроны
        public int ClipAmmo => m_clipAmmo;
        public int MaxAmmo => m_maxAmmo;
        public bool InfinityAmmo => m_infinityAmmo;
    }
}