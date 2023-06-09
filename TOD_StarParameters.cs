using System;
using UnityEngine;

[Serializable]
public class TOD_StarParameters
{
	[_E058(0f)]
	[Tooltip("Tiling of the stars texture.")]
	public float Tiling = 6f;

	[Tooltip("Brightness of the stars.")]
	[_E058(0f)]
	public float Brightness = 3f;

	[Tooltip("Type of the stars position calculation.")]
	public TOD_StarsPositionType Position = TOD_StarsPositionType.Rotating;
}
