using Code.Client;
using Code.Guns.Base;
using UnityEngine;

namespace Code.Locations.MiniGames.Racing.Pickup
{
    internal sealed class BoxWithGun : MonoBehaviour
    {
        [Header("Характеристики")]
        [SerializeField] [Tooltip("Название Оружия")] private Gun _gunName;
        [SerializeField] [Tooltip("Оружие")] private Gun _gunPrefab;
        [SerializeField] [Tooltip("В какой слот транспорта будет устанавливаться оружие")] private int _slotIndex;
        
        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponentInParent<Player>();
            if (player)
            {
                player.Car.PlaceGun(_slotIndex, _gunPrefab);
                Destroy(gameObject);
            }
        }
    }
}