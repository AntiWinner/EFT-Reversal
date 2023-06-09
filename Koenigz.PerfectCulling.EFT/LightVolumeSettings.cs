using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[RequireComponent(typeof(PerfectCullingLightGroupPreProcess))]
public sealed class LightVolumeSettings : MonoBehaviour
{
	[Serializable]
	public sealed class LightSettings
	{
		public CullingObject LightObject;

		public Vector3 Offset;

		public Quaternion Rotation;

		public LightMeshQuality MeshQuality;

		public float RangeModifier;

		public float CollisionMargin;

		public float SpotAngleModifier;
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public CullingObject light;

		internal bool _E000(LightSettings x)
		{
			return x.LightObject == light;
		}
	}

	[SerializeField]
	private List<LightSettings> _lightParameters = new List<LightSettings>();

	public IReadOnlyCollection<LightSettings> LightBakeParameters => _lightParameters;

	public LightSettings GetLightSettings(CullingObject light)
	{
		LightSettings lightSettings = _lightParameters.Find((LightSettings x) => x.LightObject == light);
		if (lightSettings == null)
		{
			lightSettings = new LightSettings();
			lightSettings.LightObject = light;
			lightSettings.Rotation = Quaternion.identity;
			lightSettings.MeshQuality = LightMeshQuality.Medium;
			lightSettings.RangeModifier = 1f;
			lightSettings.SpotAngleModifier = 1f;
			_lightParameters.Add(lightSettings);
		}
		return lightSettings;
	}
}
