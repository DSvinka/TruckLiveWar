using UnityEngine;

namespace Code.Providers
{
    // TODO: Добавить эффект выстрела.
    internal sealed class WeaponProvider : MonoBehaviour
    {
        [Header("Элементы оружия")]
        [SerializeField] [Tooltip("Оружие")]
        private Transform m_gun;

        [SerializeField] [Tooltip("Стойка на которой стоит оружие")]
        private Transform m_handle;

        [SerializeField] [Tooltip("Точка от куда будут производится выстрелы")]
        private Transform m_firePoint;

        public Transform Gun => m_gun;
        public Transform FirePoint => m_firePoint;
        public Transform Handle => m_handle;

    }
}