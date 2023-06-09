using System;
using UnityEngine;

[Serializable]
public class TOD_SunParameters
{
	[Tooltip("Color of the sun spot.\nInterpolates from left (day) to right (night).")]
	public Gradient MeshColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(253, 171, 50, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(253, 171, 50, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Size of the sun spot in degrees.")]
	[_E058(0f)]
	public float MeshSize = 1f;

	[Tooltip("Brightness of the sun spot.")]
	[_E058(0f)]
	public float MeshBrightness = 2f;

	[Tooltip("Contrast of the sun spot.")]
	[_E058(0f)]
	public float MeshContrast = 1f;
}
