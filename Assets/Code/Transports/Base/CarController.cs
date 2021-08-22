using UnityEngine;
using System;

namespace Code.Transports.Base
{
	[Serializable]
	public enum DriveType
	{
		RearWheel,
		FrontWheel,
		AllWheel
	}

	public sealed class CarController : MonoBehaviour
	{
		[Header("Характиристики")]
		[SerializeField, Tooltip("Максимальный угол поворота колес")]
		public float MaxAngle = 30f;
		[SerializeField, Tooltip("Максимальный крутящий момент, прилагаемый к ведущим колесам")]
		public float MaxTorque = 300f;
		[SerializeField, Tooltip("Максимальный тормозной момент, прилагаемый к ведущим колесам")]
		public float BrakeTorque = 30000f;

		[Header("Настройки Физического Движка")]
		[SerializeField, Tooltip("Скорость автомобиля, когда физический движок может использовать разное количество подшагов (в м/с).")]
		private float criticalSpeed = 5f;
		[SerializeField, Tooltip("Подэтапы моделирования, когда скорость выше критической.")]
		private int stepsBelow = 12;
		[SerializeField, Tooltip("Подэтапы моделирования при скорости ниже критической.")]
		private int stepsAbove = 15;
		
		[Header("Дополнительные Настройки")]
		[SerializeField, Tooltip("Автоматическое прикрепление колёс")]
		private GameObject wheelShape;
		[SerializeField, Tooltip("Тип привода автомобиля: задний привод, передний привод или полноприводный.")]
		private DriveType driveType;
		[SerializeField, Tooltip("Тип ручного тормоза автомобиля: задний, передний или полноприводный.")]
		private DriveType handBrakeType;

		private WheelCollider[] _wheels;

	    private Vector3 _wheelsSize;
	    private float _angle;
	    private float _torque;
	    private float _handBrake;

	    private void Awake()
	    {
		    _wheels = GetComponentsInChildren<WheelCollider>();
	    }
		private void Start()
		{
			_wheelsSize.y = _wheelsSize.z = _wheels[0].radius;

			foreach (var wheel in _wheels)
			{
				wheel.ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);
					
				if (wheelShape != null)
				{
					var ws = Instantiate (wheelShape, wheel.transform, true);
					ws.transform.localScale += _wheelsSize;
				}
			}
		}
		
		private void GetInput()
		{
			_angle = MaxAngle * Input.GetAxis("Horizontal");
			_torque = MaxTorque * Input.GetAxis("Vertical");

			_handBrake = Input.GetKey(KeyCode.Space) ? BrakeTorque : 0;
		}
		private void UpdateVisual(WheelCollider wheel)
		{
			Quaternion q;
			Vector3 p;
			wheel.GetWorldPose (out p, out q);
			
			Transform shapeTransform = wheel.transform.GetChild (0);

			if (wheel.name == "a0l" || wheel.name == "a1l" || wheel.name == "a2l") // TODO: Сделать нормальные оси (классы двумя колёсами) чтобы не использовать строки
			{
				shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
				shapeTransform.position = p;
			}
			else
			{
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}
		}

		private void CarMove(WheelCollider wheel)
		{
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = _angle;

			if (wheel.transform.localPosition.z < 0 && handBrakeType != DriveType.FrontWheel)
				wheel.brakeTorque = _handBrake;

			if (wheel.transform.localPosition.z >= 0 && handBrakeType != DriveType.RearWheel)
				wheel.brakeTorque = _handBrake;

			if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheel)
				wheel.motorTorque = _torque;

			if (wheel.transform.localPosition.z >= 0 && driveType != DriveType.RearWheel)
				wheel.motorTorque = _torque;
		}
		
		private void Update()
		{
			GetInput();
			foreach (var wheel in _wheels)
			{
				CarMove(wheel);
				if (wheelShape)
					UpdateVisual(wheel);
			}
		}
	}
}