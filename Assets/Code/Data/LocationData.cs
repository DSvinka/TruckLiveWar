using Code.Interfaces.Data;
using Code.Providers;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "LocationSettings", menuName = "Data/LocationSettings")]
    internal sealed class LocationData : ScriptableObject, IDictData
    {
        [SerializeField] private GameObject m_locationPrefab;

        [Header("Информация")]
        [SerializeField] private string m_IDName = "location";
        [SerializeField] private string m_name = "Название";

        public GameObject LocationPrefab => m_locationPrefab;
        public string Name => m_name;
        public string IDName => m_IDName;
    }
}