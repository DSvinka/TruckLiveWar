using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "ModificatorSettings", menuName = "Data/Pickup/ModificatorSettings")]
    public sealed class ModificatorData : ScriptableObject
    {
        [Header("Информация")]
        [SerializeField] private string m_modificatorName = "Бонус";
        [SerializeField] private Color m_messageColor = Color.gray;

        [Header("Изменение Характеристик Попадаемого Объекта")]
        [SerializeField][Range(-10f, 10f)] private float m_changeSpeed = 0f;
        [SerializeField] private float m_changeHealth = 0f;

        [Header("Прочее")]
        [SerializeField][Tooltip("Время действия модификатора в секундах (время действия при подборе если ZoneObject = False, при выходе из зоны если ZoneObject = True)")]
        private int m_activeTime = 5;

        [SerializeField][Tooltip(
            "Этот объект является зоной или нет. Если False то объект будет удалятся после срабатывания. " +
            "Если True то объект не будет удалятся после срабатывания, " +
            "А модификаторы будут держаться пока игрок не выйдет из зоны (а после выхода, пока не кончится таймер ActiveTime)")]
        private bool m_zoneObject = false;

        public string ModificatorName => m_modificatorName;
        public Color MessageColor => m_messageColor;

        public float ChangeSpeed => m_changeSpeed;
        public float ChangeHealth => m_changeHealth;

        /// <summary>
        /// Время действия модификатора в секундах
        /// </summary>
        public int ActiveTime => m_activeTime;

        /// <summary>
        /// Этот объект является зоной или нет. Если False то объект будет удалятся после срабатывания.
        /// Если True то объект не будет удалятся после срабатывания,
        /// А модификаторы будут держаться пока игрок не выйдет из зоны (и пока не кончится таймер ActiveTime)
        /// </summary>
        public bool ZoneObject => m_zoneObject;
    }
}
