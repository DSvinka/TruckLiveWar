using System.IO;
using Cinemachine;
using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Data/Unit/PlayerSettings")]
    internal sealed class PlayerData : ScriptableObject
    {
        [SerializeField] private string m_carDataPath;

        [Header("Объекты")]
        [SerializeField] private GameObject m_playerPrefab;
        
        [Header("Параметры")] 
        [SerializeField] private float m_rayDistance = 100f;
        [SerializeField, Range(1f, 0.01f)] private float m_radarSize = 100f;
        
        private CarData m_car;

        public GameObject PlayerPrefab => m_playerPrefab;
        public float RayDistance => m_rayDistance;
        public float RadarSize => m_radarSize;

        public CarData Car => GetData(m_carDataPath, ref m_car);
    }
}