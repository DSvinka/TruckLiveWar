using UnityEngine;
using UnityEditor;
using Code.Transports.Base;

namespace Code.Utils.VehicleTools.EditorGUI
{
	internal sealed class VehicleSkeletonWizard : EditorWindow
	{
		int m_AxlesCount = 2;
		float m_Mass = 1000;
		float m_AxleStep = 2;
		float m_AxleWidth = 2;
		float m_AxleShift = -0.5f;

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
			{
				CreateCar();
			}
		}

		private void CreateCar()
		{
			var root = new GameObject("Car");
			var rootBody = root.AddComponent<Rigidbody>();
			rootBody.mass = m_Mass;

			var body = GameObject.CreatePrimitive(PrimitiveType.Cube);
			body.transform.parent = root.transform;

			float length = (m_AxlesCount - 1) * m_AxleStep;
			float firstOffset = length * 0.5f;

			body.transform.localScale = new Vector3(m_AxleWidth, 1, length);

			for (int i = 0; i < m_AxlesCount; ++i)
			{
				var leftWheel = new GameObject(string.Format("Wheel{0}Left", i));
				var rightWheel = new GameObject(string.Format("Wheel{0}Right", i));

				leftWheel.AddComponent<WheelCollider>();
				rightWheel.AddComponent<WheelCollider>();

				leftWheel.transform.parent = root.transform;
				rightWheel.transform.parent = root.transform;

				leftWheel.transform.localPosition =
					new Vector3(-m_AxleWidth * 0.5f, m_AxleShift, firstOffset - m_AxleStep * i);
				rightWheel.transform.localPosition =
					new Vector3(m_AxleWidth * 0.5f, m_AxleShift, firstOffset - m_AxleStep * i);
			}

			root.AddComponent<EasySuspension>();
		}
	}
}