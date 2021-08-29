using System;
using System.Linq;
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
        private readonly PlayerInitialization m_playerInitialization;
        private readonly ICarData m_carData;

        private float m_horizontalInput;
        private float m_verticalInput;
        private bool m_handBreakInput;

        private IUserAxisProxy m_horizontalAxisProxy;
        private IUserAxisProxy m_verticalAxisProxy;
        private IUserKeyProxy m_handbreakInputProxy;

        private float _angle;
        private float _torque;
        private float _handBrake;

        private float _health;
        private bool _death;

        public CarProvider CarProvider { get; private set; }

        public float SpeedModificator { get; set; }

        public CarController(
            (IUserAxisProxy inputHorizontal, IUserAxisProxy inputVertical) axisInput,
            (IUserKeyProxy inputHandbreak, IUserKeyProxy inputRestart) keysInput,
            PlayerInitialization playerInitialization, ICarData carData)
        {
            m_playerInitialization = playerInitialization;
            m_carData = carData;
            
            m_horizontalAxisProxy = axisInput.inputHorizontal;
            m_verticalAxisProxy = axisInput.inputVertical;
            m_handbreakInputProxy = keysInput.inputHandbreak;
        }

        public void Initialization()
        {
            m_horizontalAxisProxy.AxisOnChange += HorizontalOnAxisOnChange;
            m_verticalAxisProxy.AxisOnChange += VerticalOnAxisOnChange;
            m_handbreakInputProxy.KeyOnChange += HandbreakOnChange;
            
            CarProvider = m_playerInitialization.GetPlayerTransport();

            CarProvider.OnUnitDamage += AddDamage;
            CarProvider.OnUnitHealth += AddHealth;
            
            _health = m_carData.MaxHealth;

            foreach (var wheel in CarProvider.WheelAxies.SelectMany(wheelAxie => wheelAxie.Wheels))
            {
                wheel.WheelCollider.ConfigureVehicleSubsteps(m_carData.CriticalSpeed, m_carData.StepsBelow, m_carData.StepsAbove);
            }
        }

        public void Cleanup()
        {
            m_horizontalAxisProxy.AxisOnChange -= HorizontalOnAxisOnChange;
            m_verticalAxisProxy.AxisOnChange -= VerticalOnAxisOnChange;
            m_handbreakInputProxy.KeyOnChange -= HandbreakOnChange;
            
            CarProvider.OnUnitDamage -= AddDamage;
            CarProvider.OnUnitHealth -= AddHealth;
        }

        private void VerticalOnAxisOnChange(float value)
        {
            m_verticalInput = value;
        }
        private void HorizontalOnAxisOnChange(float value)
        {
            m_horizontalInput = value;
        }
        private void HandbreakOnChange(bool value)
        {
            m_handBreakInput = value;
        }

        public void Execute(float deltaTime)
        {
            if (!_death)
            {
                GetInput();
                foreach (var wheelAxie in CarProvider.WheelAxies)
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
            _angle = m_carData.MaxAngle * m_horizontalInput;
            _handBrake = m_handBreakInput ? m_carData.BrakeTorque : 0;
            
            if (SpeedModificator < 0f)
                _torque = (m_carData.MaxTorque / -SpeedModificator) * m_verticalInput;
            else if (SpeedModificator > 0f)
                _torque = (m_carData.MaxTorque * SpeedModificator) * m_verticalInput;
            else
                _torque = m_carData.MaxTorque * m_verticalInput;
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

                if (carProvider.gameObject.GetInstanceID() == CarProvider.gameObject.GetInstanceID())
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

                if (carProvider.gameObject.GetInstanceID() == CarProvider.gameObject.GetInstanceID())
                {
                    _health += health;
                    if (_health > m_carData.MaxHealth)
                        _health = m_carData.MaxHealth;
                }
            }
        }

        private void Death()
        {
            _death = true;
            _health = 0;
            CarProvider.Explosion();
        }
    }
}