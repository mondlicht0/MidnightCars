using System;
using CarOut.Cars.MVP;
using UnityEngine;

namespace DriftGame.Cars
{
	public class CarPresenter : Presenter
	{
		private CarVisual _visual;
		private CarModel _model;
		private CarController _controller;

		private CarPresenter(CarModel model, CarVisual view, CarController controller) : base(model, view) 
		{
			_model = model;
			_visual = view;
			_controller = controller;
			
			_visual.Init(this);
		}

		public void LogicUpdate(Vector2 input)
		{
			_model.UpdateCarInput(input);
		}

		public void PhysicsUpdate(Vector2 movementInput, bool handbrakeInput)
		{
			_controller.ApplyController(movementInput, handbrakeInput);
		}
		
		public class Builder
		{
			private CarModel _model;

			public Builder WithConfig(CarConfig config)
			{
				_model = new CarModel(config);
				return this;
			}

			public CarPresenter Build(CarVisual visual, CarController controller)
			{
				if (visual == null)
				{
					throw new Exception("Visual is null");
				}

				if (controller == null)
				{
					throw new Exception("Controller is null");
				}
			
				return new CarPresenter(_model, visual, controller);
			}
		}
	}
}
