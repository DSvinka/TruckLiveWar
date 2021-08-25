using UnityEngine;

namespace Code.Providers
{
    // TODO: Добавить эффект выстрела.
    public class WeaponProvider : MonoBehaviour
    {
        [Header("Элементы оружия")]
        [SerializeField] [Tooltip("Оружие")] 
        private Transform _gun;

        [SerializeField] [Tooltip("Стойка на которой стоит оружие")] 
        private Transform _handle;
        
        [SerializeField] [Tooltip("Точка от куда будут производится выстрелы")] 
        private Transform _firePoint;

        public Transform Gun => _gun;
        public Transform FirePoint => _firePoint;
        public Transform Handle => _handle;
        
    }
}