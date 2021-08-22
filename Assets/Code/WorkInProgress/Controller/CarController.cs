using UnityEngine;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;
using Code.Providers;

namespace Code.Controller
{
	public sealed class CarController : IInitialization, IExecute, ICleanup
	{
		private readonly CarProvider _car;
		private readonly ICarData _carData;
		
		private float _horizontalInput;
		private float _verticalInput;
		private bool _handBreakInput;

		private IUserInputProxy _horizontalInputProxy;
		private IUserInputProxy _verticalInputProxy;
		private IUserKeyProxy _handbreakInputProxy;

		private Vector3 _wheelsSize;
		private float _angle;
		private float _torque;
		private float _handBrake;

		private float _speedModificator;
		private float _health;

		public CarProvider CarProvider => _car;
		public float SpeedModificator { get => _speedModificator; set => _speedModificator = value; }

		public CarController((IUserInputProxy inputHorizontal, IUserInputProxy inputVertical, IUserKeyProxy inputHandbreak) input,
			CarProvider car, ICarData carData)
		{
			_car = car;
			_carData = carData;
			
			_horizontalInputProxy = input.inputHorizontal;
			_verticalInputProxy = input.inputVertical;
			_handbreakInputProxy = input.inputHandbreak;
		}
		
		public void Initialization()
		{
			_horizontalInputProxy.AxisOnChange += HorizontalOnAxisOnChange;
			_verticalInputProxy.AxisOnChange += VerticalOnAxisOnChange;
			_handbreakInputProxy.KeyOnChange += HandbreakOnChange;
			
			foreach (var wheelAxie in _car.WheelAxies)
			{
				foreach (var wheel in wheelAxie.Wheels)
				{
					wheel.WheelCollider.ConfigureVehicleSubsteps(_carData.CriticalSpeed, _carData.StepsBelow, _carData.StepsAbove);
				}
			}
		}

		private void VerticalOnAxisOnChange(float value)
		{
			_verticalInput = value;
		}
		private void HorizontalOnAxisOnChange(float value)
		{
			_horizontalInput = value;
		}
		private void HandbreakOnChange(bool value)
		{
			_handBreakInput = value;
		}

		public void Execute(float deltaTime)
		{
			GetInput();
			foreach (var wheelAxie in _car.WheelAxies)
			{
				foreach (var wheel in wheelAxie.Wheels)
				{
					CarMove(wheel, wheelAxie);
					if (wheel.WheelShape)
						UpdateVisual(wheel);
				}
			}
		}

		public void Cleanup()
		{
			_horizontalInputProxy.AxisOnChange -= HorizontalOnAxisOnChange;
			_verticalInputProxy.AxisOnChange -= VerticalOnAxisOnChange;
		}

		private void GetInput()
		{
			_angle = _carData.MaxAngle * _horizontalInput;
			_torque = (_carData.MaxTorque * SpeedModificator) * _verticalInput;

			_handBrake = _handBreakInput ? _carData.BrakeTorque : 0;
		}
		
		private void UpdateVisual(Wheel wheel)
		{
			wheel.WheelCollider.GetWorldPose(out var position, out var rotation);
			
			Transform shapeTransform = wheel.WheelShape.transform;

			if (wheel.WheelSide == WheelSide.Left)
			{
				shapeTransform.rotation = rotation * Quaternion.Euler(0, 180, 0);
				shapeTransform.position = position;
			}
			else
			{
				shapeTransform.position = position;
				shapeTransform.rotation = rotation;
			}
		}

		private void CarMove(Wheel wheel, WheelAxie wheelAxie)
		{
			var wheelCollider = wheel.WheelCollider;
			
			if (wheelAxie.IsSteeringAxie)
				wheelCollider.steerAngle = _angle;

			if (wheelAxie.IsHandbreakAxie)
				wheelCollider.brakeTorque = _handBrake;

			if (wheelAxie.IsMotorAxie)
				wheelCollider.motorTorque = _torque;
		}
		
		public void AddDamage(float damage)
		{
			_health -= damage;
			if (_health <= 0)
				Death();
		}

		public void AddHealth(float health)
		{
			_health += health;
			if (_health > _carData.MaxHealth)
				_health = _carData.MaxHealth;
		}

		private void Death()
		{
			_health = 0;
			_car.Explosion();
		}
	}
}