using System;
using UnityEngine;

[Serializable]
public class PixelationUtilities
{
	public _E417.PixelationMode Mode;

	public Shader PixelationShader;

	[Range(2f, 2048f)]
	public float BlockCount = 256f;

	[Range(0f, 1f)]
	public float Alpha = 1f;

	public Texture PixelationMask;
}
