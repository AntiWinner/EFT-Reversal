using System;
using UnityEngine;

namespace EFT.UI.Map;

[Serializable]
public class ExtractionPoint
{
	public string Name;

	public bool NotGuaranteed;

	public Vector2 PositionOnMap;

	public bool ShowRadius;

	public float Radius = 1f;
}
