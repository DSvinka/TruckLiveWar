using System;
using Code.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Providers
{
    internal sealed class RadarObjectProvider : MonoBehaviour
    {
        private enum ObjectType
        {
            House,
            Enemy,
            Item,
        }
        
        [SerializeField] private Image m_icon;
        [SerializeField] private ObjectType m_objectType = ObjectType.House;
        

        private void OnValidate()
        {
            var path = m_objectType switch
            {
                ObjectType.House => "Prefabs/UI/Hud/Components/Radar/Icons/House",
                ObjectType.Enemy => "Prefabs/UI/Hud/Components/Radar/Icons/Enemy",
                ObjectType.Item => "Prefabs/UI/Hud/Components/Radar/Icons/Item",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            m_icon = Resources.Load<Image>(path);
        }

        private void OnEnable()
        {
            RadarController.RegisterRadarObject(gameObject, m_icon);
        }

        private void OnDisable()
        {
            RadarController.RemoveRadarObject(gameObject);
        }
    }
}