using Code.Entities.Base;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Code.Client.Interfaces.Hud.Components
{
    internal sealed class EnemyInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Image _panel;

        private bool _active;
        private Entity _entity;

        private void Start()
        {
            _active = false;
            _panel.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_active)
            {
                _nameText.text = _entity.Name;
                _healthText.text = $"HP: {_entity.Health}";
            }
        }

        public void SetEntity(Entity entity)
        {
            _entity = entity;
            _panel.gameObject.SetActive(true);
            _active = true;
        }

        public void DelEntity()
        {
            if (_active)
            {
                _panel.gameObject.SetActive(false);
                _active = false;
            }
        }
    }

}