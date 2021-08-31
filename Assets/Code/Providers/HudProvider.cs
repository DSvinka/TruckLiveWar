using System;
using TMPro;
using UnityEngine;

namespace Code.Providers
{
    [Serializable]
    internal struct MessageComponent
    {
        [SerializeField] private GameObject m_gameObject;
        [SerializeField] private TMP_Text m_text;

        public GameObject GameObject => m_gameObject;
        public TMP_Text Text => m_text;
    }
    
    [Serializable]
    internal struct EnemyInformationComponent
    {
        [SerializeField] private GameObject m_gameObject;
        [SerializeField] private TMP_Text m_nameText;
        [SerializeField] private TMP_Text m_healthText;

        public GameObject GameObject => m_gameObject;
        public TMP_Text NameText => m_nameText;
        public TMP_Text HealthText => m_healthText;
    }

    [Serializable]
    internal struct BonusesListComponent
    {
        [SerializeField] private GameObject m_gameObject;
        [SerializeField] private GameObject m_messagePrefab;
        [SerializeField] private Transform m_content;
        
        public GameObject GameObject => m_gameObject;
        public GameObject MessagePrefab => m_messagePrefab;
        public Transform Content => m_content;
    }
    
    [Serializable]
    internal struct RadarComponent
    {
        [SerializeField] private GameObject m_gameObject;
        [SerializeField] private Transform m_content;

        public GameObject GameObject => m_gameObject;
        public Transform Content => m_content;
    }
    
    internal sealed class HudProvider : MonoBehaviour
    {
        [SerializeField] private EnemyInformationComponent m_enemyInformation;
        [SerializeField] private BonusesListComponent m_bonusesList;
        [SerializeField] private MessageComponent m_message;
        [SerializeField] private RadarComponent m_radar;

        public RadarComponent Radar => m_radar;
        public EnemyInformationComponent EnemyInformation => m_enemyInformation;
        public BonusesListComponent BonusesList => m_bonusesList;
        public MessageComponent Message => m_message;
    }
}