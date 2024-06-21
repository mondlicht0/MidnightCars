using CarOut.Cars.MVP;
using UnityEngine;
using Zenject;

namespace DriftGame.Cars
{
	public class Car : MonoBehaviour
	{
		[SerializeField] private CarConfig _carData;

		private CarVisual _carVisual;
		private CarPresenter _carPresenter;
		private CarController _controller;
		private InputHandler _inputHandler;
		private Vector2 _moveInput;
		
		public Rigidbody RigidBody { get; private set; }

		[Inject]
		private void Construct(InputHandler inputHandler)
		{
			_inputHandler = inputHandler;
		}
		
		private void Awake()
		{
			RigidBody = GetComponent<Rigidbody>();
			_controller = GetComponent<CarController>();
			_carVisual = GetComponentInChildren<CarVisual>();
		}

		private void Start()
		{
			_carPresenter = new CarPresenter.Builder()
				.WithConfig(_carData)
				.Build(_carVisual, _controller);
		}

		private void Update()
		{
			_carPresenter.LogicUpdate(_inputHandler.MovementInput);
		}

		private void FixedUpdate()
		{
			_carPresenter.PhysicsUpdate();
		}
	}
}
