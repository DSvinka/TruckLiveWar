using System;
using Code.Providers;
using Code.Utils.Extensions;
using UnityEngine;
using UnityEditor;

namespace Code.Utils.VehicleTools.Editor
{
    public sealed class VehicleSkeletonWizard : EditorWindow
    {
        private int m_axlesCount = 2;
        private float m_mass = 1000;
        private float m_axleStep = 2;
        private float m_axleWidth = 2;
        private float m_axleShift = -0.5f;
        private float m_wheelSize = 1f;
        private GameObject m_wheelModelPrefab;

        private UnityEditor.Editor m_gameObjectEditor;
        private GameObject m_car;

        private Vector3 m_addToWheelModel;
        private const float k_wheelSizeAdd = 0.1f;

        [MenuItem("Vehicles/Создать скелет...")]
        public static void ShowWindow()
        {
            GetWindow(typeof(VehicleSkeletonWizard));
        }

        private void OnEnable()
        {
            m_addToWheelModel = new Vector3(0, -0.1f, 0f);
        }

        private void OnGUI()
        {
            m_axlesCount = EditorGUILayout.IntSlider("Количество Осей: ", m_axlesCount, 2, 10);
            m_mass = EditorGUILayout.FloatField("Масса: ", m_mass);
            m_axleStep = EditorGUILayout.FloatField("Расстояние между осями: ", m_axleStep);
            m_axleWidth = EditorGUILayout.FloatField("Ширина автомобиля: ", m_axleWidth);
            m_axleShift = EditorGUILayout.FloatField("Смещение оси: ", m_axleShift);
            m_wheelSize = EditorGUILayout.FloatField("Размер колёс: ", m_wheelSize);
            m_wheelModelPrefab = (GameObject) EditorGUILayout.ObjectField("Префаб колеса: ", m_wheelModelPrefab, typeof(GameObject), true);
            
            if (GUILayout.Button("Сгенерировать"))
                CreateCar();

            if (m_gameObjectEditor != null)
                m_gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(500, 500), EditorStyles.whiteLabel);
        }

        private void CreateCar()
        {
            DestroyImmediate(m_car);
            m_car = new GameObject("Car");
            m_gameObjectEditor = UnityEditor.Editor.CreateEditor(m_car);
            var body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.transform.parent = m_car.transform;
            
            var rootBody = m_car.GetOrAddComponent<Rigidbody>();
            rootBody.mass = m_mass;
            
            m_car.AddComponent<CarProvider>();
            m_car.AddComponent<EasySuspension>();

            var length = (m_axlesCount - 1) * m_axleStep;
            var firstOffset = length * 0.5f;

            var wheelAxie = new WheelAxie[m_axlesCount];

            body.transform.localScale = new Vector3(m_axleWidth, 1, length);

            var wheelModels = new GameObject("Wheel Models");
            var wheelColliders = new GameObject("Wheel Colliders");

            var wheelModelsTransform = wheelModels.transform;
            var wheelCollidersTransform = wheelColliders.transform;
            
            wheelModelsTransform.SetParent(m_car.transform);
            wheelCollidersTransform.SetParent(m_car.transform);

            for (var i = 0; i < m_axlesCount; ++i)
            {
                var leftWheel = new GameObject($"Wheel{i}Left");
                var rightWheel = new GameObject($"Wheel{i}Right");
                
                leftWheel.transform.SetParent(wheelCollidersTransform);
                rightWheel.transform.SetParent(wheelCollidersTransform);

                var leftWheelCollider = leftWheel.GetOrAddComponent<WheelCollider>();
                var rightWheelCollider = rightWheel.GetOrAddComponent<WheelCollider>();
                
                leftWheelCollider.radius = (leftWheelCollider.radius + k_wheelSizeAdd) * m_wheelSize;
                rightWheelCollider.radius = (leftWheelCollider.radius + k_wheelSizeAdd) * m_wheelSize;

                leftWheel.transform.localPosition = new Vector3(-m_axleWidth * 0.5f, m_axleShift, firstOffset - m_axleStep * i);
                rightWheel.transform.localPosition = new Vector3(m_axleWidth * 0.5f, m_axleShift, firstOffset - m_axleStep * i);
                
                // ===

                var leftWheelModel = (GameObject) PrefabUtility.InstantiatePrefab(m_wheelModelPrefab, wheelModelsTransform);
                var rightWheelModel = (GameObject) PrefabUtility.InstantiatePrefab(m_wheelModelPrefab, wheelModelsTransform);

                leftWheelModel.name = $"Wheel{i}Left";
                rightWheelModel.name = $"Wheel{i}Right";

                leftWheelModel.transform.SetParent(wheelModelsTransform);
                rightWheelModel.transform.SetParent(wheelModelsTransform);

                leftWheelModel.transform.localScale = leftWheelModel.transform.localScale.UpdateAll(m_wheelSize);
                rightWheelModel.transform.localScale = rightWheelModel.transform.localScale.UpdateAll(m_wheelSize);

                leftWheelModel.transform.localPosition = leftWheel.transform.localPosition + m_addToWheelModel;
                rightWheelModel.transform.localPosition = rightWheel.transform.localPosition + m_addToWheelModel;
                
                // ===
                
                var wheels = new Wheel[]
                {
                    new Wheel(leftWheelModel, leftWheelCollider, WheelSide.Left),
                    new Wheel(rightWheelModel, rightWheelCollider, WheelSide.Right)
                };
                
                wheelAxie[i] = new WheelAxie(wheels, i != 0, i == m_axlesCount - 1, i == 0);
            }
            
            EditorUtility.SetDirty(m_car);
        }
    }
}