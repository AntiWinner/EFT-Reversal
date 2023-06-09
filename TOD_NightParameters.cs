using System;
using UnityEngine;

[Serializable]
public class TOD_NightParameters
{
	[Tooltip("Color of the light that hits the atmosphere at night.\nInterpolates from left (day) to right (night).")]
	public Gradient SkyColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Color of the light that hits the ground at night.\nInterpolates from left (day) to right (night).")]
	public Gradient LightColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Color of the god rays at night.\nInterpolates from left (day) to right (night).")]
	public Gradient RayColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Color of the clouds at night.\nInterpolates from left (day) to right (night).")]
	public Gradient CloudColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Color of the ambient light at night.\nInterpolates from left (day) to right (night).")]
	public Gradient AmbientColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(25, 40, 65, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Intensity of the light source at night.")]
	[_E058(0f)]
	public float LightIntensity = 0.1f;

	[Tooltip("Opacity of the shadows dropped by the light source at night.")]
	[_E05A(0f, 1f)]
	public float ShadowStrength = 1f;

	[Range(0f, 1f)]
	[Tooltip("Brightness multiplier of the color gradients at night.")]
	public float ColorMultiplier = 1f;

	[Range(0f, 1f)]
	[Tooltip("Brightness multiplier of the ambient light at night.")]
	public float AmbientMultiplier = 1f;

	[Tooltip("Brightness multiplier of the reflection probe at night.")]
	[Range(0f, 1f)]
	public float ReflectionMultiplier = 1f;
}
