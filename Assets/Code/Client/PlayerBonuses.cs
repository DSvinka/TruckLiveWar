using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Transports.Base;
using UnityEngine;

namespace Code.Client
{
    internal sealed class PlayerBonuses : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private CarController _carController;
        private List<Modificator> _modificators;
        
        /// <summary>Ивент подбора бонуса, предметов и попадания в ловушку, п</summary>
        /// <param name="arg1">Время действия бонуса (0 если бесконечно или не требуется)</param>
        /// <param name="arg2">Тип бонуса</param>
        /// <param name="arg3">Название бонуса</param>
        public event Action<int, string, Color> PickupEvent;
        
        public void Init()
        {
            _carController = _player.Car.CarController;
            _modificators = new List<Modificator>();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void AddModificator(Modificator modificator)
        {
            if (_modificators.Count == 0 || _modificators.All(x => modificator.GameObjectID != x.GameObjectID))
            {
                _modificators.Add(modificator);
                StartCoroutine(ModificatorActivate(modificator));
            }
        }

        private IEnumerator ModificatorActivate(Modificator modificator)
        {
            var defaultTorque = _carController.MaxTorque;
            var defaultBreak = _carController.BrakeTorque;

            if (modificator.ChangeSpeed < 0)
                _carController.MaxTorque /= Math.Abs(modificator.ChangeSpeed);
            else if (modificator.ChangeSpeed > 0)
                _carController.MaxTorque *= modificator.ChangeSpeed;
            
            if (modificator.ChangeBreak < 0)
                _carController.BrakeTorque /= Math.Abs(modificator.ChangeBreak);
            else if (modificator.ChangeBreak > 0)
                _carController.BrakeTorque *= modificator.ChangeBreak;
            
            PickupEvent?.Invoke(modificator.ActiveTime, modificator.ModificatorName, modificator.MessageColor);
            yield return new WaitForSecondsRealtime(modificator.ActiveTime);
            
            _carController.MaxTorque = defaultTorque;
            _carController.BrakeTorque = defaultBreak;
            
            _modificators.Remove(modificator);
        }
    }
    
    [Serializable]
    internal sealed class Modificator
    {
        [SerializeField] private string _modificatorName = "Бонус";
        [SerializeField] private float _changeSpeed = 1f;
        [SerializeField] private float _changeBreak = 1f;
        [SerializeField] private Color _messageColor = Color.gray;
        [SerializeField] private int _activeTime = 5;
        private int _gameObjectID;

        public string ModificatorName => _modificatorName;
        public float ChangeSpeed => _changeSpeed;
        public float ChangeBreak => _changeBreak;

        /// <summary>
        /// Цвет сообщения
        /// </summary>
        public Color MessageColor => _messageColor;

        /// <summary>
        /// Время действия модификатора в секундах
        /// </summary>
        public int ActiveTime => _activeTime;

        /// <summary>
        /// ID объекта который прислал модификатор
        /// </summary>
        public int GameObjectID
        {
            get => _gameObjectID;
            set => _gameObjectID = value;
        }
    }
}