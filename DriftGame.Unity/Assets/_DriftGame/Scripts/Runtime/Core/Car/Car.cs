using CarOut.Cars.MVP;
using DriftGame.Network;
using Fusion;
using UnityEngine;
using Zenject;

namespace DriftGame.Cars
{
	public class Car : NetworkBehaviour
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
		}

		private void Start()
		{
			_carPresenter = new CarPresenter.Builder()
				.WithConfig(_carData)
				.Build(_carVisual, _controller);
		}
		
		public override void FixedUpdateNetwork()
		{
			//_carPresenter.LogicUpdate(_inputHandler.MovementInput);
			//_carPresenter.PhysicsUpdate(_inputHandler.MovementInput, _inputHandler.IsHandbraking);

			if (GetInput(out NetworkInputData data))
			{
				data.direction.Normalize();
				_carPresenter.LogicUpdate(data.direction);
				_carPresenter.PhysicsUpdate(false);
			}
		}
	}
}
