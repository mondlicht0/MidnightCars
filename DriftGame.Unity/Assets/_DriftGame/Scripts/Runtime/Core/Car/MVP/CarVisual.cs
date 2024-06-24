using DriftGame.Systems.SaveSystem;
using UnityEngine;

namespace CarOut.Cars.MVP 
{
	public class CarVisual : View, IDataPersistence
	{
		[SerializeField] private MeshFilter _carMesh;
		[SerializeField] private MeshRenderer _carRenderer;
		[SerializeField] private GameObject _spoiler;
		
		private Color _savedColor;
		private bool _hasSpoiler;

		public void ChangeColor(Color color)
		{
			_carRenderer.material.color = color;
			_savedColor = color;
		}

		public void AddSpoiler()
		{
			_spoiler.SetActive(true);
			_hasSpoiler = true;
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

		public void LoadData(GameData data)
		{
			Debug.Log("Car Visual Data Loaded");
			_savedColor = data.Color;
			_hasSpoiler = data.HasSpoiler;
			ChangeColor(_savedColor);
		}

		public void SaveData(ref GameData data)
		{
			Debug.Log("Car Visual Data Saved");
			data.Color = _savedColor;
			data.HasSpoiler = _hasSpoiler;
		}
	}
}
