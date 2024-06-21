using UnityEngine;
using Zenject;

namespace CarOut.Cars.Attributes 
{
	[CreateAssetMenu(fileName = "New Car Config", menuName = "Configs/Car Config", order = 0)]
	public class CarConfig : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public float MotorPower { get; private set; }
		[field: SerializeField] public float BrakePower { get; private set; }
	}
}
