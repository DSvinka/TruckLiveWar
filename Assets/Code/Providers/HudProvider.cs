using System;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Code.Providers
{
    [Serializable]
    internal sealed class MessageComponent
    {
        [SerializeField] private GameObject m_gameObject;
        [SerializeField] private TMP_Text m_text;

        public GameObject GameObject => m_gameObject;
        public TMP_Text Text => m_text;
    }
    
    [Serializable]
    internal sealed class EnemyInformationComponent
    {
        [SerializeField] private GameObject m_gameObject;
        [SerializeField] private TMP_Text m_nameText;
        [SerializeField] private TMP_Text m_healthText;

        public GameObject GameObject => m_gameObject;
        public TMP_Text NameText => m_nameText;
        public TMP_Text HealthText => m_healthText;
    }

    [Serializable]
    internal sealed class BonusesListComponent
    {
        [SerializeField] private GameObject m_gameObject;
        [SerializeField] private GameObject m_messagePrefab;
        [SerializeField] private Transform m_content;
        
        public GameObject GameObject => m_gameObject;
        public GameObject MessagePrefab => m_messagePrefab;
        public Transform Content => m_content;
    }
    
    internal sealed class HudProvider : MonoBehaviour
    {
        [SerializeField] private EnemyInformationComponent m_enemyInformation;
        [SerializeField] private BonusesListComponent m_bonusesList;
        [SerializeField] private MessageComponent m_message;

        public EnemyInformationComponent EnemyInformation => m_enemyInformation;
        public BonusesListComponent BonusesList => m_bonusesList;
        public MessageComponent Message => m_message;
    }
}