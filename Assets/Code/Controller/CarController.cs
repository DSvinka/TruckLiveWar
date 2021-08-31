using System;
using System.Linq;
using Code.Controller.Initialization;
using UnityEngine;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Input;
using Code.Interfaces.UserInput;
using Code.Providers;
using Code.Utils.Extensions;
using IUnit = Code.Interfaces.Providers.IUnit;
using Object = UnityEngine.Object;

namespace Code.Controller
{
    internal sealed class CarController : IInitialization, IExecute, ICleanup
    {
        private readonly PlayerInitialization m_playerInitialization;
        private readonly ICarData m_carData;

        public event Action<CarController> CarExplosion = delegate(CarController carController) { };

        private CarProvider m_carProvider;

        private float m_horizontalInput;
        private float m_verticalInput;
        private bool m_handBreakInput;

        private IUserAxisProxy m_horizontalAxisProxy;
        private IUserAxisProxy m_verticalAxisProxy;
        private IUserKeyProxy m_handbreakInputProxy;

        private float _angle;
        private float _torque;
        private float _handBrake;
        
        private bool _death;

        public CarProvider CarProvider => m_carProvider;

        public float SpeedModificator { get; set; }

        public CarController(
            (IUserAxisProxy inputHorizontal, IUserAxisProxy inputVertical) axisInput,
            (IUserKeyProxy inputHandbreak, IUserKeyProxy inputRestart, IUserKeyProxy inputHorn, IUserKeyProxy inputEscape) keysInput,
            PlayerInitialization playerInitialization, ICarData carData)
        {
            m_playerInitialization = playerInitialization;
            m_carData = carData;
            
            m_horizontalAxisProxy = axisInput.inputHorizontal;
            m_verticalAxisProxy = axisInput.inputVertical;
            m_handbreakInputProxy = keysInput.inputHandbreak;
            
            // TODO: Сделать звук гудка.
        }

        public void Initialization()
        {
            m_horizontalAxisProxy.AxisOnChange += HorizontalOnAxisOnChange;
            m_verticalAxisProxy.AxisOnChange += VerticalOnAxisOnChange;
            m_handbreakInputProxy.KeyOnChange += HandbreakOnChange;
            
            m_carProvider = m_playerInitialization.GetPlayerTransport();

            m_carProvider.Health = m_carData.MaxHealth;
            m_carProvider.UnitData = m_carData as IUnitData;
            m_carProvider.OnUnitDamage += AddDamage;
            m_carProvider.OnUnitHealth += AddHealth;
            
            foreach (var wheel in m_carProvider.WheelAxies.SelectMany(wheelAxie => wheelAxie.Wheels))
            {
                wheel.WheelCollider.ConfigureVehicleSubsteps(m_carData.CriticalSpeed, m_carData.StepsBelow, m_carData.StepsAbove);
            }
        }

        public void Cleanup()
        {
            m_horizontalAxisProxy.AxisOnChange -= HorizontalOnAxisOnChange;
            m_verticalAxisProxy.AxisOnChange -= VerticalOnAxisOnChange;
            m_handbreakInputProxy.KeyOnChange -= HandbreakOnChange;
            
            m_carProvider.OnUnitDamage -= AddDamage;
            m_carProvider.OnUnitHealth -= AddHealth;
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
            if (_death) 
                return;
            
            GetInput();
            foreach (var wheelAxie in m_carProvider.WheelAxies)
            {
                foreach (var wheel in wheelAxie.Wheels)
                {
                    CarMove(wheel, wheelAxie);
                    if (wheel.WheelShape)
                        UpdateVisual(wheel);
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
            
            var shapeTransform = wheel.WheelShape.transform;

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
            if (_death) 
                return;
            
            var carProvider = unit as CarProvider;
            if (carProvider == null)
                throw new Exception("Аргумент unit не является CarProvider'ом");

            if (carProvider.gameObject.GetInstanceID() != m_carProvider.gameObject.GetInstanceID()) 
                return;
            
            m_carProvider.Health -= damage;
            if (m_carProvider.Health <= 0)
                Death();
        }

        private void AddHealth(GameObject healer, IUnit unit, float health)
        {
            if (_death) return;

            var carProvider = unit as CarProvider;
            if (carProvider == null)
                throw new Exception("Аргумент unit не является CarProvider'ом");

            if (carProvider.gameObject.GetInstanceID() != m_carProvider.gameObject.GetInstanceID()) 
                return;

            m_carProvider.Health += health;
            if (m_carProvider.Health > m_carData.MaxHealth)
                m_carProvider.Health = m_carData.MaxHealth;
        }

        private void Death()
        {
            _death = true;
            m_carProvider.Health = 0;
            m_carProvider.Explosion();
            CarExplosion.Invoke(this);
        }
    }
}