using System;
using Code.Controller.Initialization;
using UnityEngine;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;
using Code.Providers;
using IUnit = Code.Interfaces.Providers.IUnit;

namespace Code.Controller
{
	internal sealed class CarController : IInitialization, IExecute, ICleanup
	{
		private readonly PlayerInitialization _playerInitialization;
		private readonly ICarData _carData;
		
		private CarProvider _car;
		
		private float _horizontalInput;
		private float _verticalInput;
		private bool _handBreakInput;

		private IUserAxisProxy _horizontalAxisProxy;
		private IUserAxisProxy _verticalAxisProxy;
		private IUserKeyProxy _handbreakInputProxy;
		
		private float _angle;
		private float _torque;
		private float _handBrake;

		private float _speedModificator;
		private float _health;
		private bool _death;

		public CarProvider CarProvider => _car;
		public float SpeedModificator { get => _speedModificator; set => _speedModificator = value; }

		public CarController(
			(IUserAxisProxy inputHorizontal, IUserAxisProxy inputVertical) axisInput,
			(IUserKeyProxy inputHandbreak, IUserKeyProxy inputRestart) keysInput,
			PlayerInitialization playerInitialization, ICarData carData)
		{
			_playerInitialization = playerInitialization;
			_carData = carData;
			
			_horizontalAxisProxy = axisInput.inputHorizontal;
			_verticalAxisProxy = axisInput.inputVertical;
			_handbreakInputProxy = keysInput.inputHandbreak;
		}
		
		public void Initialization()
		{
			_horizontalAxisProxy.AxisOnChange += HorizontalOnAxisOnChange;
			_verticalAxisProxy.AxisOnChange += VerticalOnAxisOnChange;
			_handbreakInputProxy.KeyOnChange += HandbreakOnChange;
			
			_car = _playerInitialization.GetPlayerTransport();

			_car.OnUnitDamage += AddDamage;
			_car.OnUnitHealth += AddHealth;
			
			_health = _carData.MaxHealth;

			foreach (var wheelAxie in _car.WheelAxies)
			{
				foreach (var wheel in wheelAxie.Wheels)
				{
					wheel.WheelCollider.ConfigureVehicleSubsteps(_carData.CriticalSpeed, _carData.StepsBelow, _carData.StepsAbove);
				}
			}
		}
		
		public void Cleanup()
		{
			_horizontalAxisProxy.AxisOnChange -= HorizontalOnAxisOnChange;
			_verticalAxisProxy.AxisOnChange -= VerticalOnAxisOnChange;
			_handbreakInputProxy.KeyOnChange -= HandbreakOnChange;
			
			_car.OnUnitDamage -= AddDamage;
			_car.OnUnitHealth -= AddHealth;
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
			if (!_death)
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
		}

		private void GetInput()
		{
			_angle = _carData.MaxAngle * _horizontalInput;
			_handBrake = _handBreakInput ? _carData.BrakeTorque : 0;
			
			if (_speedModificator < 0f)
				_torque = (_carData.MaxTorque / -SpeedModificator) * _verticalInput;
			else if (_speedModificator > 0f)
				_torque = (_carData.MaxTorque * SpeedModificator) * _verticalInput;
			else
				_torque = _carData.MaxTorque * _verticalInput;
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
		
		private void AddDamage(GameObject damager, IUnit unit, float damage)
		{
			if (!_death)
			{
				var carProvider = unit as CarProvider;
				if (carProvider == null)
					throw new Exception("Аргумент unit не является CarProvider'ом");
			
				if (carProvider.gameObject.GetInstanceID() == _car.gameObject.GetInstanceID())
				{
					_health -= damage;
					if (_health <= 0)
						Death();
				}
			}
		}

		private void AddHealth(GameObject healer, IUnit unit, float health)
		{
			if (!_death)
			{
				var carProvider = unit as CarProvider;
				if (carProvider == null)
					throw new Exception("Аргумент unit не является CarProvider'ом");

				if (carProvider.gameObject.GetInstanceID() == _car.gameObject.GetInstanceID())
				{
					_health += health;
					if (_health > _carData.MaxHealth)
						_health = _carData.MaxHealth;
				}
			}
		}

		private void Death()
		{
			_death = true;
			_health = 0;
			_car.Explosion();
		}
	}
}