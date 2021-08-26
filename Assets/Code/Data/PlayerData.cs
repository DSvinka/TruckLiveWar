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

        [Header("Появление")]
        [SerializeField] private Vector3 m_spawnPosition;

        private CarData m_car;

        public CinemachineFreeLook CinemachineCamera { get; set; }
        public Camera Camera { get; set; }

        public GameObject PlayerPrefab => m_playerPrefab;
        public Vector3 SpawnPosition => m_spawnPosition;

        public CarData Car => GetData(m_carDataPath, ref m_car);
    }
}