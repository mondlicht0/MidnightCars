using System.Collections;
using System.Collections.Generic;
using CarOut.Cars.MVP;
using DriftGame.Cars;
using Fusion;
using UnityEngine;

namespace DriftGame.Network
{
    public class CarNetwork : NetworkBehaviour
    {
        [SerializeField] private CarConfig _carData;

        private CarVisual _carVisual;
        private CarPresenter _carPresenter;
        private CarController _controller;
        private InputHandler _inputHandler;
        private Vector2 _moveInput;

        public Rigidbody RigidBody { get; private set; }

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
            _inputHandler = GetComponent<InputHandler>();
            _controller = GetComponent<CarController>();
            _carVisual = GetComponentInChildren<CarVisual>();
            
            _carPresenter = new CarPresenter.Builder()
                .WithConfig(_carData)
                .Build(_carVisual, _controller);
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                _carPresenter.LogicUpdate(input.Direction, false);
            }
            
            _carPresenter.PhysicsUpdate();
        }
    }
}
