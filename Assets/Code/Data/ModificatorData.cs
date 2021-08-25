using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "ModificatorSettings", menuName = "Data/Pickup/ModificatorSettings")]
    public sealed class ModificatorData : ScriptableObject
    {
        [Header("Информация")]
        [SerializeField] private string _modificatorName = "Бонус";
        [SerializeField] private Color _messageColor = Color.gray;
        
        [Header("Изменение Характеристик Попадаемого Объекта")]
        [SerializeField, Range(-10f, 10f)] private float _changeSpeed = 0f;
        [SerializeField] private float _changeHealth = 0f;

        [Header("Прочее")]
        [SerializeField, Tooltip("Время действия модификатора в секундах (время действия при подборе если ZoneObject = False, при выходе из зоны если ZoneObject = True)")] 
        private int _activeTime = 5;

        [SerializeField, Tooltip(
             "Этот объект является зоной или нет. Если False то объект будет удалятся после срабатывания. " +
             "Если True то объект не будет удалятся после срабатывания, " +
             "А модификаторы будут держаться пока игрок не выйдет из зоны (а после выхода, пока не кончится таймер ActiveTime)")]
        private bool _zoneObject = false;

        public string ModificatorName => _modificatorName;
        public Color MessageColor => _messageColor;
        
        public float ChangeSpeed => _changeSpeed;
        public float ChangeHealth => _changeHealth;

        /// <summary>
        /// Время действия модификатора в секундах
        /// </summary>
        public int ActiveTime => _activeTime;

        /// <summary>
        /// Этот объект является зоной или нет. Если False то объект будет удалятся после срабатывания.
        /// Если True то объект не будет удалятся после срабатывания,
        /// А модификаторы будут держаться пока игрок не выйдет из зоны (и пока не кончится таймер ActiveTime)
        /// </summary>
        public bool ZoneObject => _zoneObject;
    }
}
