using System;
using UnityEngine;

namespace EFT.UI.Map;

[Serializable]
public class EntryPoint
{
	[NonSerialized]
	public int Index;

	public string Name;

	public Vector2 PositionOnMap;

	public bool IsActive = true;

	public bool Locked;

	public string[] OpenExtractionPoints;

	public bool ShowRadius;

	public float Radius = 1f;

	public Color32 MainColor;

	public event Action<EntryPoint> OnDisable;

	public void Disable()
	{
		if (this.OnDisable != null)
		{
			this.OnDisable(this);
		}
	}
}
