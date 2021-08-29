using UnityEngine;
using UnityEditor;
using Utils.Extensions;

namespace Code.Utils.VehicleTools.Editor
{
    internal sealed class VehicleSkeletonWizard : EditorWindow
    {
        private int m_AxlesCount = 2;
        private float m_Mass = 1000;
        private float m_AxleStep = 2;
        private float m_AxleWidth = 2;
        private float m_AxleShift = -0.5f;

        [MenuItem("Vehicles/Создать скелет...")]
        public static void ShowWindow()
        {
            GetWindow(typeof(VehicleSkeletonWizard));
        }

        private void OnGUI()
        {
            m_AxlesCount = EditorGUILayout.IntSlider("Количество Осей: ", m_AxlesCount, 2, 10);
            m_Mass = EditorGUILayout.FloatField("Масса: ", m_Mass);
            m_AxleStep = EditorGUILayout.FloatField("Расстояние между осями: ", m_AxleStep);
            m_AxleWidth = EditorGUILayout.FloatField("Ширина автомобиля: ", m_AxleWidth);
            m_AxleShift = EditorGUILayout.FloatField("Смещение оси: ", m_AxleShift);

            if (GUILayout.Button("Сгенерировать"))
                CreateCar();
        }

        private void CreateCar()
        {
            var root = new GameObject("Car");
            var rootBody = root.GetOrAddComponent<Rigidbody>();
            rootBody.mass = m_Mass;

            var body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.transform.parent = root.transform;

            var length = (m_AxlesCount - 1) * m_AxleStep;
            var firstOffset = length * 0.5f;

            body.transform.localScale = new Vector3(m_AxleWidth, 1, length);

            for (var i = 0; i < m_AxlesCount; ++i)
            {
                var leftWheel = new GameObject($"Wheel{i}Left");
                var rightWheel = new GameObject($"Wheel{i}Right");

                leftWheel.GetOrAddComponent<WheelCollider>();
                rightWheel.GetOrAddComponent<WheelCollider>();

                leftWheel.transform.parent = root.transform;
                rightWheel.transform.parent = root.transform;

                leftWheel.transform.localPosition = new Vector3(-m_AxleWidth * 0.5f, m_AxleShift, firstOffset - m_AxleStep * i);
                rightWheel.transform.localPosition = new Vector3(m_AxleWidth * 0.5f, m_AxleShift, firstOffset - m_AxleStep * i);
            }
        }
    }
}