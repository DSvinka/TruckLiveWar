using System.IO;
using Cinemachine;
using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Data/Unit/PlayerSettings")]
    public sealed class PlayerData : ScriptableObject
    {
        [SerializeField] private string _carDataPath;
        
        [Header("Объекты")]
        [SerializeField] private GameObject _playerPrefab;

        [Header("Появление")]
        [SerializeField] private Vector3 _spawnPosition;

        private CarData _car;

        public CinemachineFreeLook CinemachineCamera { get; set; }

        public GameObject PlayerPrefab => _playerPrefab;
        public Vector3 SpawnPosition => _spawnPosition;

        public CarData Car => GetData(_carDataPath, ref _car);
    }
}