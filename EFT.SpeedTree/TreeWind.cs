using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFT.SpeedTree;

[ExecuteInEditMode]
public class TreeWind : MonoBehaviour
{
	[Serializable]
	public struct BaseTreeData
	{
		public Vector4 _ST_WindGlobal;

		public Vector4 _ST_WindBranch;

		public Vector4 _ST_WindBranchTwitch;

		public Vector4 _ST_WindBranchWhip;

		public Vector4 _ST_WindBranchAnchor;

		public Vector4 _ST_WindBranchAdherences;

		public Vector4 _ST_WindTurbulences;

		public Vector4 _ST_WindLeaf1Ripple;

		public Vector4 _ST_WindLeaf1Tumble;

		public Vector4 _ST_WindLeaf1Twitch;

		public Vector4 _ST_WindLeaf2Ripple;

		public Vector4 _ST_WindLeaf2Tumble;

		public Vector4 _ST_WindLeaf2Twitch;

		public Vector4 _ST_WindFrondRipple;

		public bool Equals(BaseTreeData other)
		{
			if (_ST_WindGlobal.Equals(other._ST_WindGlobal) && _ST_WindBranch.Equals(other._ST_WindBranch) && _ST_WindBranchTwitch.Equals(other._ST_WindBranchTwitch) && _ST_WindBranchWhip.Equals(other._ST_WindBranchWhip) && _ST_WindBranchAnchor.Equals(other._ST_WindBranchAnchor) && _ST_WindBranchAdherences.Equals(other._ST_WindBranchAdherences) && _ST_WindTurbulences.Equals(other._ST_WindTurbulences) && _ST_WindLeaf1Ripple.Equals(other._ST_WindLeaf1Ripple) && _ST_WindLeaf1Tumble.Equals(other._ST_WindLeaf1Tumble) && _ST_WindLeaf1Twitch.Equals(other._ST_WindLeaf1Twitch) && _ST_WindLeaf2Ripple.Equals(other._ST_WindLeaf2Ripple) && _ST_WindLeaf2Tumble.Equals(other._ST_WindLeaf2Tumble) && _ST_WindLeaf2Twitch.Equals(other._ST_WindLeaf2Twitch))
			{
				return _ST_WindFrondRipple.Equals(other._ST_WindFrondRipple);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is BaseTreeData other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((((((((((((((((((((((((_ST_WindGlobal.GetHashCode() * 397) ^ _ST_WindBranch.GetHashCode()) * 397) ^ _ST_WindBranchTwitch.GetHashCode()) * 397) ^ _ST_WindBranchWhip.GetHashCode()) * 397) ^ _ST_WindBranchAnchor.GetHashCode()) * 397) ^ _ST_WindBranchAdherences.GetHashCode()) * 397) ^ _ST_WindTurbulences.GetHashCode()) * 397) ^ _ST_WindLeaf1Ripple.GetHashCode()) * 397) ^ _ST_WindLeaf1Tumble.GetHashCode()) * 397) ^ _ST_WindLeaf1Twitch.GetHashCode()) * 397) ^ _ST_WindLeaf2Ripple.GetHashCode()) * 397) ^ _ST_WindLeaf2Tumble.GetHashCode()) * 397) ^ _ST_WindLeaf2Twitch.GetHashCode()) * 397) ^ _ST_WindFrondRipple.GetHashCode();
		}
	}

	[Serializable]
	public struct FactorTreeData
	{
		public Vector4 _ST_WindGlobal_B;

		public Vector4 _ST_WindBranch_B;

		public Vector4 _ST_WindLeaf1Ripple_B;

		public Vector4 _ST_WindLeaf1Tumble_B;

		public Vector4 _ST_WindLeaf1Twitch_B;

		public Vector4 _ST_WindLeaf2Ripple_B;

		public Vector4 _ST_WindLeaf2Tumble_B;

		public Vector4 _ST_WindLeaf2Twitch_B;

		public Vector4 _ST_WindFrondRipple_B;

		public bool Equals(FactorTreeData other)
		{
			if (_ST_WindGlobal_B.Equals(other._ST_WindGlobal_B) && _ST_WindBranch_B.Equals(other._ST_WindBranch_B) && _ST_WindLeaf1Ripple_B.Equals(other._ST_WindLeaf1Ripple_B) && _ST_WindLeaf1Tumble_B.Equals(other._ST_WindLeaf1Tumble_B) && _ST_WindLeaf1Twitch_B.Equals(other._ST_WindLeaf1Twitch_B) && _ST_WindLeaf2Ripple_B.Equals(other._ST_WindLeaf2Ripple_B) && _ST_WindLeaf2Tumble_B.Equals(other._ST_WindLeaf2Tumble_B) && _ST_WindLeaf2Twitch_B.Equals(other._ST_WindLeaf2Twitch_B))
			{
				return _ST_WindFrondRipple_B.Equals(other._ST_WindFrondRipple_B);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is FactorTreeData other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((((((((((((((_ST_WindGlobal_B.GetHashCode() * 397) ^ _ST_WindBranch_B.GetHashCode()) * 397) ^ _ST_WindLeaf1Ripple_B.GetHashCode()) * 397) ^ _ST_WindLeaf1Tumble_B.GetHashCode()) * 397) ^ _ST_WindLeaf1Twitch_B.GetHashCode()) * 397) ^ _ST_WindLeaf2Ripple_B.GetHashCode()) * 397) ^ _ST_WindLeaf2Tumble_B.GetHashCode()) * 397) ^ _ST_WindLeaf2Twitch_B.GetHashCode()) * 397) ^ _ST_WindFrondRipple_B.GetHashCode();
		}
	}

	[Serializable]
	public struct Settings
	{
		private sealed class _E000 : IEqualityComparer<Settings>
		{
			public bool Equals(Settings x, Settings y)
			{
				if (x.BaseMinWindData.Equals(y.BaseMinWindData) && x.BaseMaxWindData.Equals(y.BaseMaxWindData) && x.MinWindData.Equals(y.MinWindData))
				{
					return x.MaxWindData.Equals(y.MaxWindData);
				}
				return false;
			}

			public int GetHashCode(Settings obj)
			{
				return (((((obj.BaseMinWindData.GetHashCode() * 397) ^ obj.BaseMaxWindData.GetHashCode()) * 397) ^ obj.MinWindData.GetHashCode()) * 397) ^ obj.MaxWindData.GetHashCode();
			}
		}

		public BaseTreeData BaseMinWindData;

		public BaseTreeData BaseMaxWindData;

		public FactorTreeData MinWindData;

		public FactorTreeData MaxWindData;

		public static IEqualityComparer<Settings> SettingsComparer { get; } = new _E000();

	}

	public BaseTreeData BaseMinWindData;

	public BaseTreeData BaseMaxWindData;

	public FactorTreeData MinWindData;

	public FactorTreeData MaxWindData;

	[HideInInspector]
	public Settings settings;

	private MaterialPropertyBlock _E000;

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(175926));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(175270));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(175317));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(175300));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(175345));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(175388));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(175369));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(175410));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(175454));

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(175434));

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(175478));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(175458));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(175502));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(175546));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(175526));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(175915));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(175956));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(175941));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(175980));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(176025));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(176000));

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(176043));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(176081));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(176127));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(176101));

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(174099));

	private static readonly int _E01B = Shader.PropertyToID(_ED3E._E000(174137));

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(174119));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(174157));

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(174203));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(174180));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(174229));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(174211));

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(174249));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(174295));

	private static readonly int _E024 = Shader.PropertyToID(_ED3E._E000(174333));

	private static readonly int _E025 = Shader.PropertyToID(_ED3E._E000(174315));

	private static readonly int _E026 = Shader.PropertyToID(_ED3E._E000(174353));

	private static readonly int _E027 = Shader.PropertyToID(_ED3E._E000(174399));

	private static readonly int _E028 = Shader.PropertyToID(_ED3E._E000(174376));

	private static readonly int _E029 = Shader.PropertyToID(_ED3E._E000(174425));

	private static readonly int _E02A = Shader.PropertyToID(_ED3E._E000(174407));

	private static readonly int _E02B = Shader.PropertyToID(_ED3E._E000(174445));

	private static readonly int _E02C = Shader.PropertyToID(_ED3E._E000(174491));

	private static readonly int _E02D = Shader.PropertyToID(_ED3E._E000(174465));

	private static readonly int _E02E = Shader.PropertyToID(_ED3E._E000(174511));

	private static readonly int _E02F = Shader.PropertyToID(_ED3E._E000(174549));

	public void Init(MaterialPropertyBlock mpb)
	{
		_E000 = mpb;
		Tree component = GetComponent<Tree>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
			Invoke(_ED3E._E000(175887), 0.5f);
		}
		else
		{
			SetParams();
		}
	}

	public void FillSettings()
	{
		settings = default(Settings);
		settings.BaseMinWindData = BaseMinWindData;
		settings.BaseMaxWindData = BaseMaxWindData;
		settings.MinWindData = MinWindData;
		settings.MaxWindData = MaxWindData;
	}

	public void SetDrawMotionVectors(bool isEnable)
	{
		GetComponent<Renderer>().motionVectorGenerationMode = (isEnable ? MotionVectorGenerationMode.Object : MotionVectorGenerationMode.Camera);
	}

	[ContextMenu("SetParams")]
	public void SetParamsForce()
	{
		SetParams(CreateMaterialPropertyBlock(BaseMinWindData, BaseMaxWindData, MinWindData, MaxWindData));
	}

	public void SetParams()
	{
		if (_E000 == null)
		{
			Debug.LogError(_ED3E._E000(175873));
			SetParamsForce();
		}
		else
		{
			SetParams(_E000);
		}
	}

	public void SetParams(MaterialPropertyBlock mpb)
	{
		if (TryGetComponent<Renderer>(out var component))
		{
			component.SetPropertyBlock(mpb);
		}
	}

	public static MaterialPropertyBlock CreateMaterialPropertyBlock(BaseTreeData baseMinWindData, BaseTreeData baseMaxWindData, FactorTreeData minWindData, FactorTreeData maxWindData)
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		materialPropertyBlock.SetFloat(_E001, (baseMinWindData._ST_WindGlobal.magnitude > 0f) ? 1f : 0f);
		materialPropertyBlock.SetVector(_E002, baseMinWindData._ST_WindGlobal);
		materialPropertyBlock.SetVector(_E003, baseMinWindData._ST_WindBranch);
		materialPropertyBlock.SetVector(_E004, baseMinWindData._ST_WindBranchTwitch);
		materialPropertyBlock.SetVector(_E005, baseMinWindData._ST_WindBranchWhip);
		materialPropertyBlock.SetVector(_E006, baseMinWindData._ST_WindBranchAnchor);
		materialPropertyBlock.SetVector(_E007, baseMinWindData._ST_WindBranchAdherences);
		materialPropertyBlock.SetVector(_E008, baseMinWindData._ST_WindTurbulences);
		materialPropertyBlock.SetVector(_E009, baseMinWindData._ST_WindLeaf1Ripple);
		materialPropertyBlock.SetVector(_E00A, baseMinWindData._ST_WindLeaf1Tumble);
		materialPropertyBlock.SetVector(_E00B, baseMinWindData._ST_WindLeaf1Twitch);
		materialPropertyBlock.SetVector(_E00C, baseMinWindData._ST_WindLeaf2Ripple);
		materialPropertyBlock.SetVector(_E00D, baseMinWindData._ST_WindLeaf2Tumble);
		materialPropertyBlock.SetVector(_E00E, baseMinWindData._ST_WindLeaf2Twitch);
		materialPropertyBlock.SetVector(_E00F, baseMinWindData._ST_WindFrondRipple);
		materialPropertyBlock.SetVector(_E010, baseMaxWindData._ST_WindGlobal);
		materialPropertyBlock.SetVector(_E011, baseMaxWindData._ST_WindBranch);
		materialPropertyBlock.SetVector(_E012, baseMaxWindData._ST_WindBranchTwitch);
		materialPropertyBlock.SetVector(_E013, baseMaxWindData._ST_WindBranchWhip);
		materialPropertyBlock.SetVector(_E014, baseMaxWindData._ST_WindBranchAnchor);
		materialPropertyBlock.SetVector(_E015, baseMaxWindData._ST_WindBranchAdherences);
		materialPropertyBlock.SetVector(_E016, baseMaxWindData._ST_WindTurbulences);
		materialPropertyBlock.SetVector(_E017, baseMaxWindData._ST_WindLeaf1Ripple);
		materialPropertyBlock.SetVector(_E018, baseMaxWindData._ST_WindLeaf1Tumble);
		materialPropertyBlock.SetVector(_E019, baseMaxWindData._ST_WindLeaf1Twitch);
		materialPropertyBlock.SetVector(_E01A, baseMaxWindData._ST_WindLeaf2Ripple);
		materialPropertyBlock.SetVector(_E01B, baseMaxWindData._ST_WindLeaf2Tumble);
		materialPropertyBlock.SetVector(_E01C, baseMaxWindData._ST_WindLeaf2Twitch);
		materialPropertyBlock.SetVector(_E01D, baseMaxWindData._ST_WindFrondRipple);
		materialPropertyBlock.SetVector(_E01E, maxWindData._ST_WindGlobal_B);
		materialPropertyBlock.SetVector(_E01F, maxWindData._ST_WindBranch_B);
		materialPropertyBlock.SetVector(_E020, maxWindData._ST_WindLeaf1Ripple_B);
		materialPropertyBlock.SetVector(_E021, maxWindData._ST_WindLeaf1Tumble_B);
		materialPropertyBlock.SetVector(_E022, maxWindData._ST_WindLeaf1Twitch_B);
		materialPropertyBlock.SetVector(_E023, maxWindData._ST_WindLeaf2Ripple_B);
		materialPropertyBlock.SetVector(_E024, maxWindData._ST_WindLeaf2Tumble_B);
		materialPropertyBlock.SetVector(_E025, maxWindData._ST_WindLeaf2Twitch_B);
		materialPropertyBlock.SetVector(_E026, maxWindData._ST_WindFrondRipple_B);
		materialPropertyBlock.SetVector(_E027, minWindData._ST_WindGlobal_B);
		materialPropertyBlock.SetVector(_E028, minWindData._ST_WindBranch_B);
		materialPropertyBlock.SetVector(_E029, minWindData._ST_WindLeaf1Ripple_B);
		materialPropertyBlock.SetVector(_E02A, minWindData._ST_WindLeaf1Tumble_B);
		materialPropertyBlock.SetVector(_E02B, minWindData._ST_WindLeaf1Twitch_B);
		materialPropertyBlock.SetVector(_E02C, minWindData._ST_WindLeaf2Ripple_B);
		materialPropertyBlock.SetVector(_E02D, minWindData._ST_WindLeaf2Tumble_B);
		materialPropertyBlock.SetVector(_E02E, minWindData._ST_WindLeaf2Twitch_B);
		materialPropertyBlock.SetVector(_E02F, minWindData._ST_WindFrondRipple_B);
		return materialPropertyBlock;
	}

	[ContextMenu("Clear Wind")]
	public void SetClearWind()
	{
		_E000 = new MaterialPropertyBlock();
		SetParams(_E000);
	}
}
