using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public sealed class PerfectCullingMaterialSettings : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Material mat;

		internal bool _E000(Material x)
		{
			return mat == x;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Material mat;

		internal bool _E000(Material x)
		{
			return mat == x;
		}
	}

	[SerializeField]
	private Material[] _forceOpaqueMaterials = Array.Empty<Material>();

	[SerializeField]
	private Material[] _forceTransparentMaterials = Array.Empty<Material>();

	private static PerfectCullingMaterialSettings m__E000;

	public Material[] ForceOpaqueMaterials => _forceOpaqueMaterials;

	public Material[] ForceTransparentMaterials => _forceTransparentMaterials;

	public static PerfectCullingMaterialSettings Instance
	{
		get
		{
			if (PerfectCullingMaterialSettings.m__E000 == null)
			{
				PerfectCullingMaterialSettings.m__E000 = UnityEngine.Object.FindObjectOfType<PerfectCullingMaterialSettings>();
			}
			return PerfectCullingMaterialSettings.m__E000;
		}
	}

	public static bool IsMaterialForcedOpaque(Material mat)
	{
		if ((bool)Instance)
		{
			return Array.Find(PerfectCullingMaterialSettings.m__E000._forceOpaqueMaterials, (Material x) => mat == x);
		}
		return false;
	}

	public static bool IsMaterialForcedTransparent(Material mat)
	{
		if ((bool)Instance)
		{
			return Array.Find(PerfectCullingMaterialSettings.m__E000._forceTransparentMaterials, (Material x) => mat == x);
		}
		return false;
	}
}
