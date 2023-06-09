using System;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.Interactive;

[Serializable]
public abstract class MaterialData
{
	public Renderer Renderer;

	public int MaterialId;

	protected MaterialPropertyBlock _mpb;

	[CanBeNull]
	protected Material Material
	{
		get
		{
			if (!(Renderer == null))
			{
				return Renderer.sharedMaterials[MaterialId];
			}
			return null;
		}
	}

	public virtual void Init()
	{
		_mpb = new MaterialPropertyBlock();
	}

	public abstract void TurnLights(bool on);

	public abstract void SetIntensity(float intensity);
}
