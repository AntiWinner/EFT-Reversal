using System;
using UnityEngine;

namespace EFT.Interactive;

[Serializable]
public sealed class MaterialEmission : MaterialData
{
	[SerializeField]
	private float _maxEmissionVisibility = -1f;

	private static readonly int _emissionVisibility = Shader.PropertyToID(_ED3E._E000(168740));

	public override void Init()
	{
		base.Init();
		if (!(base.Material == null) && _maxEmissionVisibility < 0f)
		{
			_maxEmissionVisibility = base.Material.GetFloat(_emissionVisibility);
		}
	}

	public override void TurnLights(bool on)
	{
		if (Renderer != null && _mpb != null)
		{
			_mpb.SetFloat(_emissionVisibility, on ? _maxEmissionVisibility : 0f);
			Renderer.SetPropertyBlock(_mpb);
		}
	}

	public override void SetIntensity(float intensity)
	{
		if (Renderer != null && _mpb != null)
		{
			_mpb.SetFloat(_emissionVisibility, _maxEmissionVisibility * intensity);
			Renderer.SetPropertyBlock(_mpb);
		}
	}

	public void SetMaxEmissionVisibility(float value)
	{
		_maxEmissionVisibility = value;
	}
}
