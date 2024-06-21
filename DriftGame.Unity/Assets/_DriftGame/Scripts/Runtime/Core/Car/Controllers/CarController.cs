using System;
using UnityEngine;
using Zenject;

namespace DriftGame.Cars
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private CarConfig _carConfig;
        [Space]
        [Header("Wheels")]
        [SerializeField] private Wheel _FRWheel;
        [SerializeField] private Wheel _FLWheel;
        [SerializeField] private Wheel _RRWheel;
        [SerializeField] private Wheel _RLWheel;
        [Space] 
        [SerializeField] private AnimationCurve _steeringCurve;
        
        private InputHandler _inputHandler;
        private Rigidbody _rigidbody;
        private float _currentSpeed;
        private float _slipAngle;
        private float _brake;

        private float _accelerationInput => _inputHandler.MovementInput.y;

        private float _steeringInput => _inputHandler.MovementInput.x;
        private bool _handbrakeInput => _inputHandler.IsHandbraking;
        
        [Inject]
        private void Construct(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void ApplyController()
        {
            _currentSpeed = _RRWheel.WheelCollider.rpm * _RRWheel.WheelCollider.radius * 2f * Mathf.PI / 10f;
            UpdateSlipAngle();
            ApplyAcceleration();
            ApplySteering();
            ApplyBrake();
            ApplyWheelTransforms();
        }

        private void UpdateSlipAngle()
        {
            _slipAngle = Vector3.Angle(transform.forward, _rigidbody.velocity - transform.forward);
            float movingDirection = Vector3.Dot(transform.forward, _rigidbody.velocity);
            
            if (movingDirection < -0.5f && _accelerationInput > 0)
            {
                _brake = Mathf.Abs(_accelerationInput);
            }
            else if (movingDirection > 0.5f && _accelerationInput < 0)
            {
                _brake = Mathf.Abs(_accelerationInput);
            }
            else
            {
                _brake = 0;
            }
        }

        private void ApplyAcceleration()
        {
            _RRWheel.WheelCollider.motorTorque = _carConfig.MotorPower * _accelerationInput;
            _RLWheel.WheelCollider.motorTorque = _carConfig.MotorPower * _accelerationInput;
        }
        
        private void ApplyBrake()
        {
            _FRWheel.WheelCollider.brakeTorque = _brake * _carConfig.BrakePower * 0.7f;
            _FLWheel.WheelCollider.brakeTorque = _brake * _carConfig.BrakePower * 0.7f;

            _RRWheel.WheelCollider.brakeTorque = _brake * _carConfig.BrakePower * 0.3f;
            _RLWheel.WheelCollider.brakeTorque = _brake * _carConfig.BrakePower * 0.3f;
            
            if (_handbrakeInput)
            {
                _RRWheel.WheelCollider.brakeTorque = _carConfig.BrakePower * 1000f;
                _RLWheel.WheelCollider.brakeTorque = _carConfig.BrakePower * 1000f;
            }
        }

        private void ApplySteering()
        {
            float steeringAngle = _steeringInput * _steeringCurve.Evaluate(_currentSpeed);
            
            if (_slipAngle < 120f)
            {
                steeringAngle += Vector3.SignedAngle(transform.forward, _rigidbody.velocity + transform.forward, Vector3.up);
            }
            
            steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
            _FRWheel.WheelCollider.steerAngle = steeringAngle;
            _FLWheel.WheelCollider.steerAngle = steeringAngle;
        }

        private void ApplyWheelTransforms()
        {
            UpdateWheel(_FRWheel);
            UpdateWheel(_FLWheel);
            UpdateWheel(_RRWheel);
            UpdateWheel(_RLWheel);
        }

        private void UpdateWheel(Wheel wheel)
        {
            Quaternion rotation = Quaternion.identity;
            Vector3 position = Vector3.zero;
            
            wheel.WheelCollider.GetWorldPose(out position, out rotation);
            wheel.WheelMesh.transform.position = position;
            wheel.WheelMesh.transform.rotation = rotation;
        }
    }
    
    [System.Serializable]
    public class Wheel
    {
        [field: SerializeField] public WheelCollider WheelCollider { get; private set; }
        [field: SerializeField] public MeshRenderer WheelMesh { get; private set; }
    }
}