using UnityEngine;

namespace Code.Locations.MiniGames.Racing.Pickup
{
    internal class ItemRotation : MonoBehaviour
    {
        [Header("Визуальные Настройки")]
        [SerializeField] private Transform _model;
        [SerializeField] [Tooltip("Вращение модели")]
        private Quaternion _rotate;

        [SerializeField] [Tooltip("Высота на которую будет подниматься и опускаться моделька")]
        private float _height;
        [SerializeField] [Tooltip("Скорость изменения высоты модельки")]
        private float _speed;

        private Vector3 _position;

        private void Start()
        {
            _position = _model.transform.localPosition;
        }
        private void Update()
        {
            _model.rotation *= Quaternion.Euler(_rotate.x * Time.deltaTime, _rotate.y * Time.deltaTime, _rotate.z * Time.deltaTime);
            // TODO: Как нибудь улучшить это
            
            _position.y = Mathf.Sin(Time.time * _speed);
            _model.transform.localPosition = _position * _height;
        }
    }
}