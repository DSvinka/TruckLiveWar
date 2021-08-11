using Client.Player;
using UnityEngine;

namespace Locations.MiniGames.Racing.Traps
{
    internal sealed class SpeedSlowingDown : MonoBehaviour
    {
        [Header("Характеристики")]
        [SerializeField] [Tooltip("Деление мощности двигателя в ловушке")] [Range(2, 100)] 
        private int _slowing;
        [SerializeField] [Tooltip("Время действия замедления вне ловушки (в секундах)")]
        private int _time;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponentInParent<Player>();
            if (player)
            {
                Debug.Log("Замедление");
                player.Car.CarController.SpeedSlowing(_slowing);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            var player = other.gameObject.GetComponentInParent<Player>();
            if (player)
            {
                player.Car.CarController.SpeedSlowing(1f);
                player.Car.CarController.SpeedSlowing(_slowing, _time);
            }
        }
    }
}