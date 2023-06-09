using System;
using UnityEngine;

[Obsolete]
public class WeaponWetDropsController : WeaponCubeMapper
{
	[SerializeField]
	private Shader _shaderToReplace;

	[SerializeField]
	private float _refraction = 0.2f;

	[SerializeField]
	private Texture2D _refractionMap;

	[SerializeField]
	private Texture2D _mask;

	private static readonly int _refr = Shader.PropertyToID("_Refr");

	private static readonly int _lerp = Shader.PropertyToID("_Lerp");

	private static readonly int _dropsMap = Shader.PropertyToID("_DropsMap");

	private static readonly int _noise = Shader.PropertyToID("_Noise");

	public void Init(Texture2D rainDistort, Texture2D rainMask)
	{
		_refractionMap = rainDistort;
		_mask = rainMask;
		Invoke("InternalInit", 1f);
	}

	private void InternalInit()
	{
		if (_shaderToReplace == null)
		{
			_shaderToReplace = _E3AC.Find("Rain/Bumped Specular SMap Wet");
		}
		Renderer[] componentsInChildren = base.transform.parent.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!(renderer.name != "MuzzleJetCombinedMesh"))
			{
				continue;
			}
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (material.shader.name == _weaponShaderNameToReplace)
				{
					material.shader = _shaderToReplace;
					material.SetFloat(_refr, _refraction);
					material.SetFloat(_lerp, 1f);
					material.SetTexture(_dropsMap, _refractionMap);
					material.SetTexture(_noise, _mask);
				}
			}
		}
	}
}
