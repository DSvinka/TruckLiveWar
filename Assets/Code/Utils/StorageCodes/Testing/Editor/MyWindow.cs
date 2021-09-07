/*using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Code.Utils.Testing.Editor
{
    public class MyWindow: EditorWindow
    {
        public static GameObject ObjectInstantiate;
        public string m_nameObject = "Hello World";
        public bool m_groupEnabled;
        public bool m_randomColor = true;
        public int m_countObject = 1;
        public float m_radius = 10;
        private static int m_offset;
        private Queue<Transform> m_queueRoot = new Queue<Transform>();

        private void OnGUI()
        {
            GUILayout.Label("Базовые Настройки", EditorStyles.boldLabel);
            ObjectInstantiate = EditorGUILayout.ObjectField("Объект который хотим вставить", ObjectInstantiate, typeof(GameObject), true) as GameObject;
            m_nameObject = EditorGUILayout.TextField("Имя Объекта", m_nameObject);
            m_groupEnabled = EditorGUILayout.BeginToggleGroup("Дополнительные настройки", m_groupEnabled);
            m_randomColor = EditorGUILayout.Toggle("Случайный цвет", m_randomColor);
            m_countObject = EditorGUILayout.IntSlider("Количество Объектов", m_countObject, 1, 100);
            m_radius = EditorGUILayout.Slider("Радиус Окружности", m_radius, 0, 50);
            
            EditorGUILayout.EndToggleGroup();

            var button = GUILayout.Button("Создать Объекты");
            if (button)
            {
                if (ObjectInstantiate)
                {
                    var root = new GameObject("Root");
                    m_queueRoot.Enqueue(root.transform);
                    for (var i = 0; i < m_countObject; i++)
                    {
                        var angle = i * Mathf.PI * 2 / m_countObject;
                        var pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * m_radius;
                        var temp = Instantiate(ObjectInstantiate, pos, Quaternion.identity);
                        temp.name = m_nameObject + "(" + i + ")";
                        temp.transform.parent = root.transform;
                        var tempRenderer = temp.GetComponent<Renderer>();
                        if (tempRenderer && m_randomColor)
                            tempRenderer.sharedMaterial.color = Random.ColorHSV();
                    }

                    root.transform.position = new Vector3(0, m_offset, 0);
                }
            }

            var destroyRoot = false;
            if (m_queueRoot.Count > 0)
                destroyRoot = GUILayout.Button("Удалить Объекты");

            if (destroyRoot)
                DestroyImmediate(m_queueRoot.Dequeue().gameObject);
        }
    }
}*/