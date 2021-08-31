using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "UISettings", menuName = "Data/UI/UISettings")]
    internal sealed class UIData : ScriptableObject
    {
        [Header("Объекты")]
        [SerializeField] private GameObject m_playerHudPrefab;
        [SerializeField] private GameObject m_winHudPrefab;
        [SerializeField] private GameObject m_deathHudPrefab;
        [SerializeField] private GameObject m_escapeMenuPrefab;

        public GameObject PlayerHudPrefab => m_playerHudPrefab;
        public GameObject WinHudPrefab => m_winHudPrefab;
        public GameObject DeathHudPrefab => m_deathHudPrefab;
        public GameObject EscapeMenuPrefab => m_escapeMenuPrefab;
    }
}