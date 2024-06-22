using UnityEngine;

namespace DriftGame.Cars
{
	public class CarModel : Model
	{
		public CarConfig CarData { get; private set; }
		public Vector2 MoveDirection { get; private set; }
		public bool HandbrakeInput { get; private set; }
		
		public CarModel(CarConfig carData)
		{
			CarData = carData;
		}

		public void UpgradeEngine()
		{
			Debug.Log("Upgraded");
		}

		public void UpdateCarInput(Vector2 newDirection, bool handbrakeInput)
		{
			MoveDirection = newDirection;
			HandbrakeInput = handbrakeInput;
		}
	}
}
