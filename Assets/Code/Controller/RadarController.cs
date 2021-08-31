using System.Collections.Generic;
using Code.Controller.Initialization;
using Code.Interfaces;
using Code.Providers;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Controller
{
    internal sealed class RadarObject
    {
        public GameObject Owner { get; set; }
        public Image Icon { get; set; }
    }
    
    internal sealed class RadarController: IController, IExecute, IInitialization
    {
        private readonly HudInitialization m_hudInitialization;
        private readonly PlayerInitialization m_playerInitialization;
        private readonly float m_mapScale;

        private Transform m_carPosition;
        private RadarComponent m_radarComponent;

        private static List<RadarObject> RadarObjects = new List<RadarObject>();

        public RadarController(PlayerInitialization playerInitialization, HudInitialization hudInitialization, float mapScale = 0.5f)
        {
            m_hudInitialization = hudInitialization;
            m_playerInitialization = playerInitialization;
            m_mapScale = mapScale;
        }

        public void Initialization()
        {
            var hudProvider = m_hudInitialization.GetPlayerHud().GetComponent<HudProvider>();
            m_radarComponent = hudProvider.Radar;
            m_carPosition = m_playerInitialization.GetPlayerTransport().transform;
        }

        public void Execute(float deltatime)
        {
            if (Time.frameCount % 2 == 0)
            {
                DrawRadarDots();
            }
        }
        
        public static void RegisterRadarObject(GameObject o, Image i)
        {
            var image = Object.Instantiate(i);
            RadarObjects.Add(new RadarObject {Owner = o, Icon = image});
        }

        public static void RemoveRadarObject(GameObject o)
        {
            var newList = new List<RadarObject>();
            foreach (var t in RadarObjects)
            {
                if (t.Owner == o)
                {
                    Object.Destroy(t.Icon);
                    continue;
                }
                newList.Add(t);
            }
            RadarObjects.RemoveRange(0, RadarObjects.Count);
            RadarObjects.AddRange(newList);
        }
        
        private void DrawRadarDots()
        {
            foreach (var radarObject in RadarObjects)
            {
                var carPosition = m_carPosition.position;
                var radarView = m_radarComponent.Content;
                
                var radarPosition = (radarObject.Owner.transform.position - carPosition);
                var distToObject = Vector3.Distance(carPosition, radarObject.Owner.transform.position) * m_mapScale;
                var deltay = Mathf.Atan2(radarPosition.x, radarPosition.z) * Mathf.Rad2Deg - 270 - m_carPosition.transform.eulerAngles.y;

                radarPosition.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
                radarPosition.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);
                
                radarObject.Icon.transform.SetParent(radarView);
                radarObject.Icon.transform.position = new Vector3(radarPosition.x, radarPosition.z, 0) + radarView.position;
            }
        }
    }
}