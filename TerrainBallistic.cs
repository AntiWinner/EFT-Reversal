using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Ballistics;
using UnityEngine;

public class TerrainBallistic : BaseBallistic
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ESurfaceSound terrainMaterial;

		internal bool _E000(BallisticCollider c)
		{
			return c.SurfaceSound == terrainMaterial;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public int i;

		public TerrainBallistic _003C_003E4__this;

		internal bool _E000(BallisticCollider c)
		{
			return c.SurfaceSound == _003C_003E4__this.TerrainMaterials[i];
		}
	}

	public TerrainData TerrainData;

	public TextAsset TextureMixDataAsset;

	private static GameObject _E00B;

	private int _E00C;

	private Vector3 _E00D;

	private BallisticCollider[] _E00E;

	private _E35B _E00F;

	public ESurfaceSound[] TerrainMaterials;

	[CompilerGenerated]
	private static readonly MaterialType[] _E010 = new MaterialType[6]
	{
		MaterialType.Asphalt,
		MaterialType.Soil,
		MaterialType.GrassHigh,
		MaterialType.Concrete,
		MaterialType.Gravel,
		MaterialType.Water
	};

	private static MaterialType[] _E000
	{
		[CompilerGenerated]
		get
		{
			return _E010;
		}
	}

	private void Awake()
	{
		_E00C++;
		if (_E00B == null)
		{
			_E00B = new GameObject(_ED3E._E000(53254));
			_E00B.isStatic = true;
			MaterialType[] array = TerrainBallistic._E000;
			foreach (MaterialType typeOfMaterial in array)
			{
				BallisticCollider ballisticCollider = _E00B.AddComponent<BallisticCollider>();
				ballisticCollider.TypeOfMaterial = typeOfMaterial;
				ballisticCollider.Associate(ballisticCollider.TypeOfMaterial);
				ApplyPresetValues(ballisticCollider);
			}
		}
		BallisticCollider[] components = _E00B.GetComponents<BallisticCollider>();
		if (TerrainData == null)
		{
			Terrain component = GetComponent<Terrain>();
			if (component != null)
			{
				TerrainData = component.terrainData;
			}
			else
			{
				Debug.LogError(_ED3E._E000(53296) + this, this);
			}
		}
		int alphamapLayers = TerrainData.alphamapLayers;
		int num = TerrainMaterials.Length;
		if (num < alphamapLayers)
		{
			num = alphamapLayers;
			_E00E = new BallisticCollider[num];
		}
		else
		{
			_E00E = new BallisticCollider[num];
		}
		for (int j = 0; j < num; j++)
		{
			ESurfaceSound terrainMaterial = ((j < TerrainMaterials.Length) ? TerrainMaterials[j] : ESurfaceSound.Soil);
			_E00E[j] = components.FirstOrDefault((BallisticCollider c) => c.SurfaceSound == terrainMaterial);
			if (_E00E[j] == null)
			{
				_E00E[j] = components.First();
			}
		}
		if (TextureMixDataAsset != null)
		{
			_E00F = _E35B.LoadFromAsset(TextureMixDataAsset);
		}
		_E00D = base.transform.position;
	}

	private void OnValidate()
	{
		if (_E00B == null)
		{
			return;
		}
		BallisticCollider[] components = _E00B.GetComponents<BallisticCollider>();
		_E00E = new BallisticCollider[TerrainMaterials.Length];
		int i;
		for (i = 0; i < TerrainMaterials.Length; i++)
		{
			_E00E[i] = components.FirstOrDefault((BallisticCollider c) => c.SurfaceSound == TerrainMaterials[i]);
			if (_E00E[i] == null)
			{
				_E00E[i] = components.First();
			}
		}
	}

	private void OnDestroy()
	{
		_E00C--;
		if (_E00C < 1)
		{
			Object.Destroy(_E00B);
		}
	}

	public override ESurfaceSound GetSurfaceSound(Vector3 pos)
	{
		return Get(pos).SurfaceSound;
	}

	public override BallisticCollider Get(Vector3 pos)
	{
		int num = _E000(pos);
		if (num > _E00E.Length)
		{
			Debug.LogErrorFormat(_ED3E._E000(53338), base.gameObject.name, num);
			return _E00E.FirstOrDefault((BallisticCollider b) => b.SurfaceSound == SurfaceSound);
		}
		return _E00E[num];
	}

	private int _E000(Vector3 worldPos)
	{
		if (_E00F != null)
		{
			return _E00F.GetTextureMix(worldPos, _E00D);
		}
		return _E35B.GetMainTexture(worldPos, TerrainData, _E00D);
	}

	public static void ApplyPresetValues(BallisticCollider bCollider)
	{
		BallisticPreset[] colliderPresets = EFTHardSettings.Instance.ColliderPresets;
		int typeOfMaterial = (int)bCollider.TypeOfMaterial;
		if (typeOfMaterial >= colliderPresets.Length)
		{
			Debug.LogError(_ED3E._E000(53357));
			return;
		}
		BallisticPreset ballisticPreset = colliderPresets[typeOfMaterial];
		if (ballisticPreset != null)
		{
			bCollider.TypeOfMaterial = ballisticPreset.MaterialType;
			bCollider.PenetrationLevel = ballisticPreset[0];
			bCollider.PenetrationChance = ballisticPreset[1];
			bCollider.RicochetChance = ballisticPreset[2];
			bCollider.FragmentationChance = ballisticPreset[3];
			bCollider.TrajectoryDeviationChance = ballisticPreset[4];
			bCollider.TrajectoryDeviation = ballisticPreset[5];
		}
	}

	public static bool IsNoneDefault(BallisticCollider bCollider)
	{
		BallisticPreset[] colliderPresets = EFTHardSettings.Instance.ColliderPresets;
		int typeOfMaterial = (int)bCollider.TypeOfMaterial;
		if (typeOfMaterial >= colliderPresets.Length)
		{
			Debug.LogError(_ED3E._E000(53357));
			return true;
		}
		BallisticPreset ballisticPreset = colliderPresets[typeOfMaterial];
		if (ballisticPreset == null)
		{
			return true;
		}
		if (Mathf.Approximately(bCollider.PenetrationLevel, ballisticPreset[0]) && Mathf.Approximately(bCollider.PenetrationChance, ballisticPreset[1]) && Mathf.Approximately(bCollider.RicochetChance, ballisticPreset[2]) && Mathf.Approximately(bCollider.FragmentationChance, ballisticPreset[3]) && Mathf.Approximately(bCollider.TrajectoryDeviationChance, ballisticPreset[4]))
		{
			return !Mathf.Approximately(bCollider.TrajectoryDeviation, ballisticPreset[5]);
		}
		return true;
	}

	[CompilerGenerated]
	private bool _E001(BallisticCollider b)
	{
		return b.SurfaceSound == SurfaceSound;
	}
}
