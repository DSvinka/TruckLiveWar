using Client.Player;
using Guns.Base;
using UnityEngine;

namespace Locations.MiniGames.Racing.Pickup
{
    internal sealed class BoxWithGun : MonoBehaviour
    {
        [Header("Характеристики")]
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