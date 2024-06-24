using DriftGame.Systems.SaveSystem;
using Fusion;
using UnityEngine;

namespace CarOut.Cars.MVP 
{
	public class CarVisual : View, IDataPersistence
	{
		[SerializeField] private MeshFilter _carMesh;
		[SerializeField] private MeshRenderer _carRenderer;
		[SerializeField] private GameObject _spoiler;
		
		[Networked, OnChangedRender(nameof(OnColorChanged))] public Color SavedColor { get; set; }
		[Networked] private bool HasSpoiler { get; set; }

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

		public void InitVisual()
		{
			Debug.Log("Car Init Visual");
			
			if (_carMesh != null)
			{
				return;
			}
			
			if (TryGetComponent(out MeshFilter meshFilter))
			{
				_carMesh = meshFilter;
			}

			if (_carRenderer != null)
			{
				return;
			}

			if (TryGetComponent(out MeshRenderer meshRenderer))
			{
				_carRenderer = meshRenderer;
			}
		}
		
		private void OnColorChanged()
		{
			_carRenderer.material.color = SavedColor;
		}

		public void LoadData(GameData data)
		{
			Debug.Log("Car Visual Data Loaded");
			SavedColor = data.Color;
			HasSpoiler = data.HasSpoiler;
			ChangeColor(SavedColor);
		}

		public void SaveData(ref GameData data)
		{
			Debug.Log("Car Visual Data Saved");
			data.Color = SavedColor;
			data.HasSpoiler = HasSpoiler;
		}
	}
}
