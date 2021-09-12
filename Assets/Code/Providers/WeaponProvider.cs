using UnityEngine;

namespace Code.Providers
{
    public sealed class WeaponProvider : MonoBehaviour
    {
        [Header("Элементы оружия")]
        [SerializeField] [Tooltip("Оружие")]
        private Transform m_gun;

        [SerializeField] [Tooltip("Стойка на которой стоит оружие")]
        private Transform m_handle;

        [SerializeField] [Tooltip("Точка от куда будут производится выстрелы")]
        private Transform m_firePoint;
        
        private ParticleSystem m_shotEffect;
        private AudioSource m_audioSource;

        public Transform Gun => m_gun;
        public Transform FirePoint => m_firePoint;
        public Transform Handle => m_handle;
        public ParticleSystem ShotEffect => m_shotEffect;
        public AudioSource AudioSource => m_audioSource;

        private void Start()
        {
            m_shotEffect = GetComponentInChildren<ParticleSystem>();
            m_audioSource = GetComponentInChildren<AudioSource>();
        }
    }
}