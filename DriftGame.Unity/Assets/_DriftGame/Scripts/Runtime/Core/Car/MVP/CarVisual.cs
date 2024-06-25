using DriftGame.Systems.SaveSystem;
using UnityEngine;

namespace CarOut.Cars.MVP 
{
	public class CarVisual : View, IDataPersistence
	{
		[SerializeField] private MeshFilter _carMesh;
		[SerializeField] private MeshRenderer _carRenderer;
		[SerializeField] private GameObject _spoiler;
		
		public Color SavedColor { get; set; }
		
		public bool HasSpoiler { get; set; }

		public void Start()
		{
			//_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
		}

		public void InitVisual()
		{
			Debug.Log("Car Init Visual");
			
			if (_carMesh == null && TryGetComponent(out MeshFilter meshFilter))
			{
				_carMesh = meshFilter;
			}

			if (_carRenderer == null && TryGetComponent(out MeshRenderer meshRenderer))
			{
				_carRenderer = meshRenderer;
			}
		}
		
		public void ChangeColor(Color color)
		{
			_carRenderer.material.color = color;
			SavedColor = color;
		}

		public void AddSpoiler()
		{
			_spoiler.SetActive(true);
			HasSpoiler = true;
		}

		public void LoadData(GameData data)
		{
			Debug.Log("Car Visual Data Loaded");
			SavedColor = data.Color;
			HasSpoiler = data.HasSpoiler;
			
			ChangeColor(SavedColor);
			_spoiler.SetActive(HasSpoiler);
		}

		public void SaveData(ref GameData data)
		{
			Debug.Log("Car Visual Data Loaded");
			SavedColor = data.Color;
			HasSpoiler = data.HasSpoiler;
			ChangeColor(SavedColor);
			_spoiler.SetActive(HasSpoiler);
		}
	}
}
