using System;
using UnityEngine;

namespace EFT.Interactive;

[Serializable]
public sealed class MaterialColor : MaterialData
{
	public string ParemeterName;

	[ColorUsage(true, true)]
	public Color ColorOn;

	[ColorUsage(true, true)]
	public Color ColorOff;

	public override void TurnLights(bool on)
	{
		if (Renderer != null && _mpb != null)
		{
			_mpb.SetColor(ParemeterName, on ? ColorOn : ColorOff);
			Renderer.SetPropertyBlock(_mpb);
		}
	}

	public override void SetIntensity(float intensity)
	{
		if (Renderer != null && _mpb != null)
		{
			_mpb.SetColor(ParemeterName, Color.Lerp(ColorOff, ColorOn, intensity));
			Renderer.SetPropertyBlock(_mpb);
		}
	}
}
