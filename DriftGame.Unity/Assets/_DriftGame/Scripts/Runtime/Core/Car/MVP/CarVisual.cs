using System;
using UnityEngine;

namespace CarOut.Cars.MVP 
{
	public class CarVisual : View
	{
		[SerializeField] private MeshFilter _carMesh;
		[SerializeField] private MeshRenderer _carRenderer;
		[SerializeField] private GameObject _spoiler;

		private void Start()
		{
			InitVisual();
		}

		public void ChangeColor(Color color)
		{
			_carRenderer.material.color = color;
		}

		public void AddSpoiler()
		{
			_spoiler.SetActive(true);
		}

		private void InitVisual()
		{
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
	}
}
