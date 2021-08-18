using Code.Client;
using UnityEngine;

namespace Code.Locations.MiniGames.Racing.Pickup
{
    internal sealed class PickupItem : MonoBehaviour
    {
        [SerializeField] private Modificator _modificator;

        private void Start()
        {
            _modificator.GameObjectID = gameObject.GetInstanceID();
        }
        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponentInParent<Player>();
            if (player)
            {
                player.Bonuses.AddModificator(_modificator);
                Destroy(gameObject);
            }
        }
    }
}