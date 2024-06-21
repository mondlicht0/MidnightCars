using System;
using UnityEngine;

namespace CarOut.Cars
{
    public class CarController : MonoBehaviour
    {
        [SerializeField, Range(20, 190)] private int _maxSpeed = 90;
        [SerializeField, Range(10, 120)] private int _maxReverseSpeed = 45;
        [SerializeField, Range(1, 10)]  private int _accelerationMultiplier = 2;
        [SerializeField, Range(10, 45)] private int _maxSteeringAngle = 27;
        [SerializeField, Range(0.1f, 1f)] private float _steeringSpeed = 0.5f;
        [SerializeField, Range(100, 600)] private int _brakeForce = 350;
        [SerializeField, Range(1, 10)] private int _decelerationMultiplier = 2;
        [SerializeField, Range(1, 10)] private int _handbrakeDriftMultiplier = 5;
        [SerializeField] private Vector3 _bodyMassCenter;
        
        [Header("Wheels")]
        [SerializeField] private GameObject _frontLeftMesh;
        [SerializeField] private WheelCollider _frontLeftCollider;
        [Space(10)]
        [SerializeField] private GameObject _frontRightMesh;
        [SerializeField] private WheelCollider _frontRightCollider;
        [Space(10)]
        [SerializeField] private GameObject _rearLeftMesh;
        [SerializeField] private WheelCollider _rearLeftCollider;
        [Space(10)]
        [SerializeField] private GameObject _rearRightMesh;
        [SerializeField] private WheelCollider _rearRightCollider;
        
        private float _carSpeed;
        private bool _isDrifting;
        private bool _isTractionLocked;

        private Rigidbody _carRigidbody;
        private float _steeringAxis;
        private float _throttleAxis;
        private float _driftingAxis;
        private float _localVelocityZ;
        private float _localVelocityX;
        private bool _deceleratingCar;
        private bool _touchControlsSetup = false;

        private float _FLWextremumSlip;
        private float _FRWextremumSlip;
        private float _RLWextremumSlip;
        private float _RRWextremumSlip;
        private WheelFrictionCurve _FLwheelFriction;
        private WheelFrictionCurve _FRwheelFriction;
        private WheelFrictionCurve _RLwheelFriction;
        private WheelFrictionCurve _RRwheelFriction;

        private void Awake()
        {
            _carRigidbody = GetComponent<Rigidbody>();
            _carRigidbody.centerOfMass = _bodyMassCenter;
        }

        private void Start()
        {
            SetupWheelsFriction();
        }

        private void Update()
        {
            HandleVelocity();
            HandleInput();
        }

        private void HandleVelocity()
        {
            _carSpeed = (2 * Mathf.PI * _frontLeftCollider.radius * _frontLeftCollider.rpm * 60) / 1000;
            _localVelocityX = transform.InverseTransformDirection(_carRigidbody.velocity).x;
            _localVelocityZ = transform.InverseTransformDirection(_carRigidbody.velocity).z;
        }

        private void HandleInput()
        {
            if(Input.GetKey(KeyCode.W)){
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoForward();
            }
            if(Input.GetKey(KeyCode.S)){
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                GoReverse();
            }

            if(Input.GetKey(KeyCode.A)){
                TurnLeft();
            }
            if(Input.GetKey(KeyCode.D)){
                TurnRight();
            }
            if(Input.GetKey(KeyCode.Space)){
                CancelInvoke("DecelerateCar");
                _deceleratingCar = false;
                Handbrake();
            }
            if(Input.GetKeyUp(KeyCode.Space)){
                RecoverTraction();
            }
            if((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))){
                ThrottleOff();
            }
            if((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space) && !_deceleratingCar){
                InvokeRepeating("DecelerateCar", 0f, 0.1f);
                _deceleratingCar = true;
            }
            if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && _steeringAxis != 0f){
                ResetSteeringAngle();
            }
        }

        private void TurnLeft(){
            _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * _steeringSpeed);
            if(_steeringAxis < -1f){
                _steeringAxis = -1f;
            }
            var steeringAngle = _steeringAxis * _maxSteeringAngle;
            _frontLeftCollider.steerAngle = Mathf.Lerp(_frontLeftCollider.steerAngle, steeringAngle, _steeringSpeed);
            _frontRightCollider.steerAngle = Mathf.Lerp(_frontRightCollider.steerAngle, steeringAngle, _steeringSpeed);
        }

        private void TurnRight(){
            _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * _steeringSpeed);
            if(_steeringAxis > 1f){
                _steeringAxis = 1f;
            }
            var steeringAngle = _steeringAxis * _maxSteeringAngle;
            _frontLeftCollider.steerAngle = Mathf.Lerp(_frontLeftCollider.steerAngle, steeringAngle, _steeringSpeed);
            _frontRightCollider.steerAngle = Mathf.Lerp(_frontRightCollider.steerAngle, steeringAngle, _steeringSpeed);
        }

        private void ResetSteeringAngle(){
            if (_steeringAxis < 0f){
                _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * _steeringSpeed);
            }
            else if(_steeringAxis > 0f){
                _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * _steeringSpeed);
            }
            if(Mathf.Abs(_frontLeftCollider.steerAngle) < 1f){
                _steeringAxis = 0f;
            }
            var steeringAngle = _steeringAxis * _maxSteeringAngle;
            _frontLeftCollider.steerAngle = Mathf.Lerp(_frontLeftCollider.steerAngle, steeringAngle, _steeringSpeed);
            _frontRightCollider.steerAngle = Mathf.Lerp(_frontRightCollider.steerAngle, steeringAngle, _steeringSpeed);
        }

        private void GoForward(){
            _isDrifting = Mathf.Abs(_localVelocityX) > 2.5f;
            _throttleAxis += (Time.deltaTime * 3f);
            if(_throttleAxis > 1f){
                _throttleAxis = 1f;
            }
            if(_localVelocityZ < -1f){
                Brakes();
            }else{
                if(Mathf.RoundToInt(_carSpeed) < _maxSpeed){
                    _frontLeftCollider.brakeTorque = 0;
                    _frontLeftCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
                    _frontRightCollider.brakeTorque = 0;
                    _frontRightCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
                    _rearLeftCollider.brakeTorque = 0;
                    _rearLeftCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
                    _rearRightCollider.brakeTorque = 0;
                    _rearRightCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
                }else {
                    _frontLeftCollider.motorTorque = 0;
                    _frontRightCollider.motorTorque = 0;
                    _rearLeftCollider.motorTorque = 0;
                    _rearRightCollider.motorTorque = 0;
                }
            }
        }

        private void GoReverse(){
            _isDrifting = Mathf.Abs(_localVelocityX) > 2.5f;
            _throttleAxis -= (Time.deltaTime * 3f);
            if(_throttleAxis < -1f){
                _throttleAxis = -1f;
            }
            if(_localVelocityZ > 1f){
                Brakes();
            }else{
                if(Mathf.Abs(Mathf.RoundToInt(_carSpeed)) < _maxReverseSpeed){
                    _frontLeftCollider.brakeTorque = 0;
                    _frontLeftCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
                    _frontRightCollider.brakeTorque = 0;
                    _frontRightCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
                    _rearLeftCollider.brakeTorque = 0;
                    _rearLeftCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
                    _rearRightCollider.brakeTorque = 0;
                    _rearRightCollider.motorTorque = (_accelerationMultiplier * 50f) * _throttleAxis;
                }else {
                    _frontLeftCollider.motorTorque = 0;
                    _frontRightCollider.motorTorque = 0;
                    _rearLeftCollider.motorTorque = 0;
                    _rearRightCollider.motorTorque = 0;
                }
            }
        }

        private void ThrottleOff(){
            _frontLeftCollider.motorTorque = 0;
            _frontRightCollider.motorTorque = 0;
            _rearLeftCollider.motorTorque = 0;
            _rearRightCollider.motorTorque = 0;
        }
        public void DecelerateCar(){
            _isDrifting = Mathf.Abs(_localVelocityX) > 2.5f;
            
            if(_throttleAxis != 0f){
                if(_throttleAxis > 0f){
                    _throttleAxis = _throttleAxis - (Time.deltaTime * 10f);
                }else if(_throttleAxis < 0f){
                    _throttleAxis = _throttleAxis + (Time.deltaTime * 10f);
                }
                if(Mathf.Abs(_throttleAxis) < 0.15f){
                    _throttleAxis = 0f;
                }
            }
            _carRigidbody.velocity = _carRigidbody.velocity * (1f / (1f + (0.025f * _decelerationMultiplier)));
            _frontLeftCollider.motorTorque = 0;
            _frontRightCollider.motorTorque = 0;
            _rearLeftCollider.motorTorque = 0;
            _rearRightCollider.motorTorque = 0;
            if(_carRigidbody.velocity.magnitude < 0.25f){
                _carRigidbody.velocity = Vector3.zero;
                CancelInvoke("DecelerateCar");
            }
        }

        public void Brakes(){
            _frontLeftCollider.brakeTorque = _brakeForce;
            _frontRightCollider.brakeTorque = _brakeForce;
            _rearLeftCollider.brakeTorque = _brakeForce;
            _rearRightCollider.brakeTorque = _brakeForce;
        }

        private void Handbrake(){
            CancelInvoke("RecoverTraction");
            _driftingAxis = _driftingAxis + (Time.deltaTime);
            float secureStartingPoint = _driftingAxis * _FLWextremumSlip * _handbrakeDriftMultiplier;

            if(secureStartingPoint < _FLWextremumSlip){
                _driftingAxis = _FLWextremumSlip / (_FLWextremumSlip * _handbrakeDriftMultiplier);
            }
            if(_driftingAxis > 1f){
                _driftingAxis = 1f;
            }
            _isDrifting = Mathf.Abs(_localVelocityX) > 2.5f;
            
            if(_driftingAxis < 1f){
                _FLwheelFriction.extremumSlip = _FLWextremumSlip * _handbrakeDriftMultiplier * _driftingAxis;
                _frontLeftCollider.sidewaysFriction = _FLwheelFriction;

                _FRwheelFriction.extremumSlip = _FRWextremumSlip * _handbrakeDriftMultiplier * _driftingAxis;
                _frontRightCollider.sidewaysFriction = _FRwheelFriction;

                _RLwheelFriction.extremumSlip = _RLWextremumSlip * _handbrakeDriftMultiplier * _driftingAxis;
                _rearLeftCollider.sidewaysFriction = _RLwheelFriction;

                _RRwheelFriction.extremumSlip = _RRWextremumSlip * _handbrakeDriftMultiplier * _driftingAxis;
                _rearRightCollider.sidewaysFriction = _RRwheelFriction;
            }
            _isTractionLocked = true;
        }
        public void RecoverTraction(){
            _isTractionLocked = false;
            _driftingAxis = _driftingAxis - (Time.deltaTime / 1.5f);
            if(_driftingAxis < 0f){
                _driftingAxis = 0f;
            }
      
            if(_FLwheelFriction.extremumSlip > _FLWextremumSlip){
                _FLwheelFriction.extremumSlip = _FLWextremumSlip * _handbrakeDriftMultiplier * _driftingAxis;
                _frontLeftCollider.sidewaysFriction = _FLwheelFriction;

                _FRwheelFriction.extremumSlip = _FRWextremumSlip * _handbrakeDriftMultiplier * _driftingAxis;
                _frontRightCollider.sidewaysFriction = _FRwheelFriction;

                _RLwheelFriction.extremumSlip = _RLWextremumSlip * _handbrakeDriftMultiplier * _driftingAxis;
                _rearLeftCollider.sidewaysFriction = _RLwheelFriction;

                _RRwheelFriction.extremumSlip = _RRWextremumSlip * _handbrakeDriftMultiplier * _driftingAxis;
                _rearRightCollider.sidewaysFriction = _RRwheelFriction;

                Invoke("RecoverTraction", Time.deltaTime);

            }else if (_FLwheelFriction.extremumSlip < _FLWextremumSlip){
                _FLwheelFriction.extremumSlip = _FLWextremumSlip;
                _frontLeftCollider.sidewaysFriction = _FLwheelFriction;

                _FRwheelFriction.extremumSlip = _FRWextremumSlip;
                _frontRightCollider.sidewaysFriction = _FRwheelFriction;

                _RLwheelFriction.extremumSlip = _RLWextremumSlip;
                _rearLeftCollider.sidewaysFriction = _RLwheelFriction;

                _RRwheelFriction.extremumSlip = _RRWextremumSlip;
                _rearRightCollider.sidewaysFriction = _RRwheelFriction;

                _driftingAxis = 0f;
            }
        }

        private void SetupWheelsFriction()
        {
            SetupWheelFrictionCurve(ref _FLwheelFriction, ref _frontLeftCollider, ref _FLWextremumSlip);
            SetupWheelFrictionCurve(ref _FRwheelFriction, ref _frontRightCollider, ref _FRWextremumSlip);
            SetupWheelFrictionCurve(ref _RLwheelFriction, ref _rearLeftCollider, ref _RLWextremumSlip);
            SetupWheelFrictionCurve(ref _RRwheelFriction, ref _rearRightCollider, ref _RRWextremumSlip);
        }

        private void SetupWheelFrictionCurve(ref WheelFrictionCurve wheelFrictionCurve, ref WheelCollider wheelCollider, ref float wheelExtremumSlip)
        {
            wheelFrictionCurve = new WheelFrictionCurve();
            wheelExtremumSlip = wheelCollider.sidewaysFriction.extremumSlip;
            wheelFrictionCurve.extremumSlip = wheelCollider.sidewaysFriction.extremumSlip;
            wheelFrictionCurve.extremumValue = wheelCollider.sidewaysFriction.extremumValue;
            wheelFrictionCurve.asymptoteSlip = wheelCollider.sidewaysFriction.asymptoteSlip;
            wheelFrictionCurve.asymptoteValue = wheelCollider.sidewaysFriction.asymptoteValue;
            wheelFrictionCurve.stiffness = wheelCollider.sidewaysFriction.stiffness;
        }
    }
}