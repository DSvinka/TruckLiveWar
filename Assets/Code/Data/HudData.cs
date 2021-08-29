using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "HudSettings", menuName = "Data/Hud/HudSettings")]
    internal sealed class HudData : ScriptableObject
    {
        [Header("Объекты")]
        [SerializeField] private GameObject m_playerHudPrefab;
        [SerializeField] private GameObject m_winHudPrefab;
        [SerializeField] private GameObject m_deathHudPrefab;

        public GameObject PlayerHudPrefab => m_playerHudPrefab;
        public GameObject WinHudPrefab => m_winHudPrefab;
        public GameObject DeathHudPrefab => m_deathHudPrefab;
    }
}