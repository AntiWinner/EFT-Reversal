using System;
using UnityEngine;

[Serializable]
public class TOD_DayParameters
{
	[Tooltip("Color of the light that hits the atmosphere at day.\nInterpolates from left (day) to right (night).")]
	[Header("left (day) to right (night) (2 hours)")]
	public Gradient SkyColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(byte.MaxValue, 243, 234, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(byte.MaxValue, 243, 234, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Color of the light that hits the ground at day.\nInterpolates from left (day) to right (night).")]
	public Gradient LightColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(byte.MaxValue, 243, 234, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(byte.MaxValue, 107, 0, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Color of the god rays at day.\nInterpolates from left (day) to right (night).")]
	public Gradient RayColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(byte.MaxValue, 243, 234, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(byte.MaxValue, 107, 0, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Color of the clouds at day.\nInterpolates from left (day) to right (night).")]
	public Gradient CloudColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(byte.MaxValue, 200, 100, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Color of the ambient light at day.\nInterpolates from left (day) to right (night).")]
	public Gradient AmbientColor = new Gradient
	{
		alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(1f, 0f),
			new GradientAlphaKey(1f, 1f)
		},
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(new Color32(94, 89, 87, byte.MaxValue), 0f),
			new GradientColorKey(new Color32(94, 89, 87, byte.MaxValue), 1f)
		}
	};

	[Tooltip("Intensity of the light source.")]
	[_E058(0f)]
	public float LightIntensity = 1f;

	[_E05A(0f, 1f)]
	[Tooltip("Opacity of the shadows dropped by the light source.")]
	public float ShadowStrength = 1f;

	[Tooltip("Brightness multiplier of the color gradients at day.")]
	[Range(0f, 1f)]
	public float ColorMultiplier = 1f;

	[Tooltip("Brightness multiplier of the ambient light at day.")]
	[Range(0f, 1f)]
	public float AmbientMultiplier = 1f;

	[Tooltip("Brightness multiplier of the reflection probe at day.")]
	[Range(0f, 1f)]
	public float ReflectionMultiplier = 1f;
}
