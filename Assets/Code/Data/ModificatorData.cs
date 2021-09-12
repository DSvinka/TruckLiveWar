using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "ModificatorSettings", menuName = "Data/Pickup/ModificatorSettings")]
    internal sealed class ModificatorData : ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля

        [Header("Информация")]
        [SerializeField] private string m_modificatorName = "Бонус";
        [SerializeField] private Color m_messageColor = Color.gray;

        [Header("Изменение Характеристик Попадаемого Объекта")]
        [SerializeField] [Range(-10f, 10f)] private float m_changeSpeed;
        [SerializeField] private float m_changeHealth;
        [SerializeField] private bool m_permaKiller;

        [Header("Прочее")]
        [SerializeField] [Tooltip("Время действия модификатора в секундах (время действия при подборе если ZoneObject = False, при выходе из зоны если ZoneObject = True)")]
        private int m_activeTime = 5;
        
        [SerializeField] [Tooltip("Будет ли срабатывать модификатор после выхода из тригера (если да то модификатор после выхода будет работать 'ActiveTime' секунд)")]
        private bool m_activateAfterExit;

        [SerializeField] [Tooltip(
            "Этот объект является зоной или нет. Если False то объект будет удалятся после срабатывания. " +
            "Если True то объект не будет удалятся после срабатывания, " +
            "А модификаторы будут держаться пока игрок не выйдет из зоны (а после выхода, пока не кончится таймер ActiveTime)")]
        private bool m_zoneObject;
        
        #endregion

        #region Свойства
        public string ModificatorName => m_modificatorName;
        public Color MessageColor => m_messageColor;

        public float ChangeSpeed => m_changeSpeed;
        public float ChangeHealth => m_changeHealth;
        
        /// <summary>
        /// Если True то игрок будет моментально умирать при подборе/входе
        /// </summary>
        public bool PermaKiller => m_permaKiller;

        /// <summary>
        /// Время действия модификатора в секундах
        /// </summary>
        public int ActiveTime => m_activeTime;

        /// <summary>
        /// Будет ли срабатывать модификатор после выхода из тригера
        /// </summary>
        public bool ActivateAfterExit => m_activateAfterExit;

        /// <summary>
        /// Этот объект является зоной или нет. Если False то объект будет удалятся после срабатывания.
        /// Если True то объект не будет удалятся после срабатывания,
        /// А модификаторы будут держаться пока игрок не выйдет из зоны (и пока не кончится таймер ActiveTime)
        /// </summary>
        public bool ZoneObject => m_zoneObject;
        
        #endregion
    }
}
