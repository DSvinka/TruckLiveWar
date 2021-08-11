using System;
using System.Collections;
using System.Collections.Generic;
using Client.Player;
using UnityEngine;

namespace Locations.MiniGames.Racing.Pickup
{
    internal sealed class SpeedBoost : MonoBehaviour
    {
        [Header("Характеристики")]
        [SerializeField] [Tooltip("Умножение мощности двигателя при бонусе")] [Range(5, 30)] 
        private int _speed;
        [SerializeField] [Tooltip("Время действия бонуса в секундах")] 
        private float _time;
        
        private void OnTriggerEnter(Collider other)
        {
            var player = other.gameObject.GetComponentInParent<Player>();
            if (player)
            {
                player.Car.CarController.SpeedBoost(_time, _speed);
                Destroy(gameObject);
            }
        }
    }
}
