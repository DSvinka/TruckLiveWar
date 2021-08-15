using UnityEngine;
using System;
using System.Collections;

namespace Transports.Base
{
	[Serializable]
	public enum DriveType
	{
		RearWheel,
		FrontWheel,
		AllWheel
	}
	
	public enum PickupBonusType 
	{
		SpeedBonus = 0,
		GunBox = 1,
	}
    
	public enum TrapType 
	{
		SlowingTrap = 0,
		DeathTrap = 1,
	}

	public sealed class CarController : MonoBehaviour
	{
	    [Tooltip("Максимальный угол поворота колес")]
		[SerializeField] private float maxAngle = 30f;
		[Tooltip("Максимальный крутящий момент, прилагаемый к ведущим колесам")]
		[SerializeField] private float maxTorque = 300f;
		[Tooltip("Максимальный тормозной момент, прилагаемый к ведущим колесам")]
		[SerializeField] private float brakeTorque = 30000f;
		[Tooltip("Если вам нужно, чтобы визуальные колеса прикреплялись автоматически, перетащите сюда объект колеса.")]
		[SerializeField] private GameObject wheelShape;

		[Tooltip("Скорость автомобиля, когда физический движок может использовать разное количество подшагов (в м/с).")]
		[SerializeField] private float criticalSpeed = 5f;
		[Tooltip("Подэтапы моделирования, когда скорость выше критической.")]
		[SerializeField] private int stepsBelow = 12;
		[Tooltip("Подэтапы моделирования при скорости ниже критической.")]
		[SerializeField] private int stepsAbove = 15;

		[Tooltip("Тип привода автомобиля: задний привод, передний привод или полноприводный.")]
		[SerializeField] private DriveType driveType;
		
		[Tooltip("Тип ручного тормоза автомобиля: задний, передний или полноприводный.")]
		[SerializeField] private DriveType handBrakeType;
		
		/// <summary>Ивент подбора бонуса</summary>
		/// <param name="arg1">Время действия бонуса (0 если бесконечно или не требуется)</param>
		/// <param name="arg2">Тип бонуса</param>
		/// <param name="arg3">Название бонуса</param>
		public event Action< float, PickupBonusType, string> PickupBonusEvent;
		
		/// <summary>Ивент попадания в ловушку</summary>
		/// <param name="arg1">Время действия ловушки (0 если бесконечно или не требуется)</param>
		/// <param name="arg2">Тип ловушки</param>
		/// <param name="arg3">Название ловушки</param>
		public event Action<float, TrapType, string> TrapActivateEvent;

		private WheelCollider[] _wheels;

	    private Vector3 _wheelsSize;
	    private float _angle;
	    private float _torque;
	    private float _handBrake;
	    
	    private float _boostTorque = 1f;
	    private float _slowingTorque = 1f;

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
			_angle = maxAngle * Input.GetAxis("Horizontal");
			_torque = (maxTorque * _boostTorque / _slowingTorque) * Input.GetAxis("Vertical");

			_handBrake = Input.GetKey(KeyCode.Space) ? brakeTorque : 0;
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

		private void OnDestroy()
		{
			StopAllCoroutines();
		}

		/// <summary>
		/// Постоянное замедление
		/// </summary>
		/// <param name="slowing">Сила замедления</param>
		public void SpeedSlowing(float slowing)
		{
			_slowingTorque = slowing;
			TrapActivateEvent?.Invoke(0, TrapType.SlowingTrap, "Замедление");
		}
		/// <summary>
		/// Временное замедление
		/// </summary>
		/// <param name="slowing">Сила замедления</param>
		/// <param name="time">Время действия (в секундах)</param>
		public void SpeedSlowing(float slowing, float time)
		{
			TrapActivateEvent?.Invoke(time, TrapType.SlowingTrap, "Замедление");
			StartCoroutine(SpeedSlowingCoroutine(slowing, time));
		}
		
		/// <summary>
		/// Постоянный буст
		/// </summary>
		/// <param name="speed">Сила буста</param>
		public void SpeedBoost(float speed)
		{
			_boostTorque = speed;
			PickupBonusEvent?.Invoke(0, PickupBonusType.SpeedBonus, "Ускорение");
		}
		/// <summary>
		/// Временный буст
		/// </summary>
		/// <param name="speed">Сила буста</param>
		/// <param name="time">Время действия (в секундах)</param>
		public void SpeedBoost(float speed, float time)
		{
			PickupBonusEvent?.Invoke(time, PickupBonusType.SpeedBonus, "Ускорение");
			StartCoroutine(SpeedBoostCoroutine(speed, time));
		}

		public void KillTrap()
		{
			TrapActivateEvent?.Invoke(0, TrapType.DeathTrap, "Смертельная ловушка");
		}
		
		public void PickupGun(string gunName)
		{
			PickupBonusEvent?.Invoke(0, PickupBonusType.GunBox, $"Оружие \"{gunName}\"");
		}

		private IEnumerator SpeedBoostCoroutine(float speed, float time)
		{
			var currentBoost = _boostTorque;
			
			_boostTorque = speed;
			yield return new WaitForSecondsRealtime(time);
			_boostTorque = currentBoost;
		}
		private IEnumerator SpeedSlowingCoroutine(float speed, float time)
		{
			var currentSlowing = _slowingTorque;
			
			_slowingTorque = speed;
			yield return new WaitForSecondsRealtime(time);
			_slowingTorque = currentSlowing;
		}
	}
}