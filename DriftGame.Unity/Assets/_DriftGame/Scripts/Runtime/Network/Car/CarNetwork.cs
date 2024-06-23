using System;
using System.Collections;
using System.Collections.Generic;
using CarOut.Cars.MVP;
using DriftGame.Cars;
using DriftGame.Systems.SaveSystem;
using Fusion;
using UnityEngine;

namespace DriftGame.Network
{
    public class CarNetwork : NetworkBehaviour, IDataPersistence
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

        private void Start()
        {
            _carPresenter.Visual.InitVisual();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                _carPresenter.LogicUpdate(input.Direction, false);
            }
            
            _carPresenter.PhysicsUpdate();
        }
        
        public void LoadData(GameData data)
        {
            _carVisual.LoadData(data);
            _carVisual.ChangeColor(data.Color);
        }

        public void SaveData(ref GameData data)
        {
            _carVisual.SaveData(ref data);
        }
    }
}
