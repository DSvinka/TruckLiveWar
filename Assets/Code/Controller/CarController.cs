using System;
using System.Linq;
using Code.Controller.Initialization;
using Code.Data;
using UnityEngine;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Input;
using Code.Providers;
using Code.UserInput.Inputs;
using IUnit = Code.Interfaces.Providers.IUnit;

namespace Code.Controller
{
    internal sealed class CarController : IInitialization, IExecute, ICleanup
    {
        private readonly PlayerInitialization m_playerInitialization;
        private ICarData m_carData;

        public event Action<CarController> CarExplosion = delegate(CarController carController) { };

        private Rigidbody m_carRigidbody;

        private float m_horizontalInput;
        private float m_verticalInput;
        private bool m_handBreakInput;

        private IUserAxisProxy m_horizontalAxisProxy;
        private IUserAxisProxy m_verticalAxisProxy;
        private IUserKeyProxy m_handbreakInputProxy;

        private float m_angle;
        private float m_torque;
        private float m_handBrake;
        
        private bool m_death;

        public CarProvider CarProvider { get; private set; }

        public WeaponsController WeaponsController { get; }
        public float SpeedModificator { get; set; }

        public CarController(PlayerInitialization playerInitialization, WeaponsController weaponsController, ICarData carData)
        {
            m_playerInitialization = playerInitialization;
            WeaponsController = weaponsController;
            m_carData = carData;
            
            m_horizontalAxisProxy = AxisInput.Horizontal;
            m_verticalAxisProxy = AxisInput.Vertical;
            m_handbreakInputProxy = KeysInput.Handbreak;
            
            // TODO: Сделать звук гудка.
        }

        public void Initialization()
        {
            m_horizontalAxisProxy.AxisOnChange += HorizontalOnAxisOnChange;
            m_verticalAxisProxy.AxisOnChange += VerticalOnAxisOnChange;
            m_handbreakInputProxy.KeyOnChange += HandbreakOnChange;
            
            CarProvider = m_playerInitialization.GetPlayerTransport();
            m_carRigidbody = CarProvider.GetComponent<Rigidbody>();
            if (m_carRigidbody == null)
                throw new Exception("Rigidbody отсуствует у транспорта!");
            
            m_carData = CarProvider.UnitData as TransportData;

            CarProvider.Health = m_carData.MaxHealth;
            CarProvider.UnitData = m_carData as IUnitData;
            CarProvider.OnUnitDamage += AddDamage;
            CarProvider.OnUnitHealth += AddHealth;
            
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
            if (m_death) 
                return;
            
            if (CarProvider == null)
            {
                CarProvider = m_playerInitialization.GetPlayerTransport();
                m_carRigidbody = CarProvider.GetComponent<Rigidbody>();
            }
            
            GetInput();
            PlayAudio();
            foreach (var wheelAxie in CarProvider.WheelAxies)
            {
                foreach (var wheel in wheelAxie.Wheels)
                { 
                    CarMove(wheel, wheelAxie);
                    SetSuspension(wheel.WheelCollider);
                    if (wheel.WheelShape)
                        UpdateVisual(wheel);
                }
            }
        }

        private void GetInput()
        {
            m_angle = m_carData.MaxAngle * m_horizontalInput;
            m_handBrake = m_handBreakInput ? m_carData.BrakeTorque : 0;
            
            if (SpeedModificator < 0f)
                m_torque = (m_carData.MaxTorque / -SpeedModificator) * m_verticalInput;
            else if (SpeedModificator > 0f)
                m_torque = (m_carData.MaxTorque * SpeedModificator) * m_verticalInput;
            else
                m_torque = m_carData.MaxTorque * m_verticalInput;
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
                wheelCollider.steerAngle = m_angle;

            if (wheelAxie.IsHandbreakAxie)
                wheelCollider.brakeTorque = m_handBrake;

            if (wheelAxie.IsMotorAxie)
                wheelCollider.motorTorque = m_torque;
        }

        private void PlayAudio()
        {
            var currentSpeed = m_carRigidbody.velocity.magnitude * 3.6f;
            
            var pitch = currentSpeed / m_carData.MaxSpeed;
            CarProvider.AudioSource.pitch = pitch;
        }

        private void SetSuspension(WheelCollider wheelCollider)
        {
            var spring = wheelCollider.suspensionSpring;

            var sqrtWcSprungMass = Mathf.Sqrt(wheelCollider.sprungMass);
            spring.spring = sqrtWcSprungMass * m_carData.NaturalFrequency * sqrtWcSprungMass * m_carData.NaturalFrequency;
            spring.damper = 2f * m_carData.DampingRatio * Mathf.Sqrt(spring.spring * wheelCollider.sprungMass);

            wheelCollider.suspensionSpring = spring;

            var wheelRelativeBody = CarProvider.transform.InverseTransformPoint(wheelCollider.transform.position);
            var distance = m_carRigidbody.centerOfMass.y - wheelRelativeBody.y + wheelCollider.radius;

            wheelCollider.forceAppPointDistance = distance - m_carData.ForceShift;

            if (spring.targetPosition > 0 && m_carData.SetSuspensionDistance)
                wheelCollider.suspensionDistance = wheelCollider.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);
        }

        private void AddDamage(GameObject damager, IUnit unit, float damage)
        {
            if (m_death) 
                return;
            
            var carProvider = unit as CarProvider;
            if (carProvider == null)
                throw new Exception("Аргумент unit не является CarProvider'ом");

            if (carProvider.gameObject.GetInstanceID() != CarProvider.gameObject.GetInstanceID()) 
                return;
            
            CarProvider.Health -= damage;
            if (CarProvider.Health <= 0)
                Death();
        }

        private void AddHealth(GameObject healer, IUnit unit, float health)
        {
            if (m_death) return;

            var carProvider = unit as CarProvider;
            if (carProvider == null)
                throw new Exception("Аргумент unit не является CarProvider'ом");

            if (carProvider.gameObject.GetInstanceID() != CarProvider.gameObject.GetInstanceID()) 
                return;

            CarProvider.Health += health;
            if (CarProvider.Health > m_carData.MaxHealth)
                CarProvider.Health = m_carData.MaxHealth;
        }

        private void Death()
        {
            m_death = true;
            CarProvider.Health = 0;
            CarProvider.Explosion();
            CarExplosion.Invoke(this);
        }
    }
}