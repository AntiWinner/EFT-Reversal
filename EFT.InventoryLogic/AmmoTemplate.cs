using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JsonType;
using UnityEngine;

namespace EFT.InventoryLogic;

public class AmmoTemplate : _EA9D, _E9D5, IItemTemplate
{
	private static float MaxMalfMisfireChance;

	private static float MaxMalfFeedChance;

	private static readonly string[] MalfChancesKeys = new string[6]
	{
		_ED3E._E000(229242),
		_ED3E._E000(229217),
		_ED3E._E000(229259),
		_ED3E._E000(229297),
		_ED3E._E000(229338),
		_ED3E._E000(229313)
	};

	public string ammoType;

	public int Damage;

	public int ammoAccr;

	public int ammoRec;

	public int ammoDist;

	public int buckshotBullets;

	public int PenetrationPower = 40;

	public float PenetrationPowerDiviation;

	public int ammoHear;

	public string ammoSfx;

	public float MisfireChance;

	public int MinFragmentsCount = 2;

	public int MaxFragmentsCount = 3;

	public int ammoShiftChance;

	public string casingName;

	public float casingEjectPower;

	public float casingMass;

	public string casingSounds;

	public int ProjectileCount = 1;

	public float InitialSpeed = 700f;

	public float PenetrationChance = 0.2f;

	public float RicochetChance = 0.1f;

	public float FragmentationChance = 0.03f;

	public float BallisticCoeficient = 1f;

	public bool Tracer;

	public TaxonomyColor TracerColor;

	public float TracerDistance;

	public int ArmorDamage;

	public string Caliber;

	public float StaminaBurnPerDamage;

	public bool HasGrenaderComponent;

	public float FuzeArmTimeSec;

	public float MinExplosionDistance;

	public float MaxExplosionDistance;

	public int FragmentsCount;

	public string FragmentType;

	public string ExplosionType;

	public bool ShowHitEffectOnExplode;

	public float ExplosionStrength;

	public bool ShowBullet;

	public float AmmoLifeTimeSec = 2f;

	public float MalfMisfireChance;

	public float MalfFeedChance;

	public Vector3 ArmorDistanceDistanceDamage;

	public Vector3 Contusion;

	public Vector3 Blindness;

	public float LightBleedingDelta;

	public float HeavyBleedingDelta;

	public bool IsLightAndSoundShot;

	public float LightAndSoundShotAngle;

	public float LightAndSoundShotSelfContusionTime;

	public float LightAndSoundShotSelfContusionStrength;

	public float DurabilityBurnModificator = 1f;

	public float HeatFactor = 1f;

	public float BulletMassGram;

	public float BulletDiameterMilimeters;

	public bool RemoveShellAfterFire;

	private List<_EB10> _cachedQualities;

	private SonicBulletSoundPlayer.SonicType? _cachedSonicType;

	public float AmmoFactor
	{
		get
		{
			if (ammoAccr <= 0)
			{
				return (100f + (float)Mathf.Abs(ammoAccr)) / 100f;
			}
			return 100f / (float)(100 + ammoAccr);
		}
	}

	public float ArmorDamagePortion => (float)ArmorDamage / 100f;

	float _E9D5.FuzeArmTimeSec => FuzeArmTimeSec;

	float _E9D5.MinExplosionDistance => MinExplosionDistance;

	float _E9D5.MaxExplosionDistance => MaxExplosionDistance;

	int _E9D5.FragmentsCount => FragmentsCount;

	string _E9D5.FragmentType => FragmentType;

	string _E9D5.ExplosionType => ExplosionType;

	float _E9D5.ExplosionStrength => ExplosionStrength;

	bool _E9D5.ShowHitEffectOnExplode => ShowHitEffectOnExplode;

	Vector3 _E9D5.ArmorDistanceDistanceDamage => ArmorDistanceDistanceDamage;

	Vector3 _E9D5.Contusion => Contusion;

	Vector3 _E9D5.Blindness => Blindness;

	public bool GrenadeComponentIsDummy
	{
		get
		{
			if (Blindness.y < float.Epsilon && Contusion.y < float.Epsilon)
			{
				return MaxExplosionDistance < float.Epsilon;
			}
			return false;
		}
	}

	public override void OnInit()
	{
		MaxMalfFeedChance = Math.Max(MalfFeedChance, MaxMalfFeedChance);
		MaxMalfMisfireChance = Math.Max(MalfMisfireChance, MaxMalfMisfireChance);
	}

	public List<_EB10> GetCachedReadonlyQualities()
	{
		if (_cachedQualities != null)
		{
			return _cachedQualities;
		}
		_cachedQualities = new List<_EB10>(6);
		SafelyAddQualityToList(new _EB10(EItemAttributeId.BulletSpeed)
		{
			Name = EItemAttributeId.BulletSpeed.GetName(),
			Base = () => InitialSpeed,
			StringValue = () => InitialSpeed + _ED3E._E000(18502) + _ED3E._E000(229348).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		_cachedQualities.Add(new _EB10(EItemAttributeId.Caliber)
		{
			Name = EItemAttributeId.Caliber.GetName(),
			Base = () => 0f,
			StringValue = () => Caliber.Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddQualityToList(new _EB10(EItemAttributeId.CenterOfImpact)
		{
			Name = EItemAttributeId.CenterOfImpact.GetName(),
			Base = () => ammoAccr,
			StringValue = () => ammoAccr.ToString(_ED3E._E000(229344)) + _ED3E._E000(149464),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		SafelyAddQualityToList(new _EB10(EItemAttributeId.Recoil)
		{
			Name = EItemAttributeId.Recoil.GetName(),
			Base = () => ammoRec,
			StringValue = () => ammoRec.ToString(),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored,
			LessIsGood = true
		});
		SafelyAddQualityToList(new _EB10(EItemAttributeId.HeavyBleedingDelta)
		{
			Name = EItemAttributeId.HeavyBleedingDelta.GetName(),
			Base = () => HeavyBleedingDelta,
			StringValue = () => HeavyBleedingDelta.ToString(_ED3E._E000(45972)),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddQualityToList(new _EB10(EItemAttributeId.LightBleedingDelta)
		{
			Name = EItemAttributeId.LightBleedingDelta.GetName(),
			Base = () => LightBleedingDelta,
			StringValue = () => LightBleedingDelta.ToString(_ED3E._E000(45972)),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddQualityToList(new _EB10(EItemAttributeId.MalfMisfireChance)
		{
			Name = EItemAttributeId.MalfMisfireChance.GetName(),
			Base = () => MalfMisfireChance,
			StringValue = delegate
			{
				if (MalfMisfireChance <= 0f)
				{
					return MalfChancesKeys[0].Localized();
				}
				string text2 = null;
				int num = MalfChancesKeys.Length - 3 - 1;
				for (int i = 3; i < 7; i++)
				{
					if (MalfMisfireChance < (float)i * MaxMalfMisfireChance / 7f)
					{
						text2 = MalfChancesKeys[i - num];
						break;
					}
				}
				text2 = text2 ?? MalfChancesKeys[MalfChancesKeys.Length - 1];
				return text2.Localized();
			},
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddQualityToList(new _EB10(EItemAttributeId.MalfFeedChance)
		{
			Name = EItemAttributeId.MalfFeedChance.GetName(),
			Base = () => MalfFeedChance,
			StringValue = delegate
			{
				if (MalfFeedChance <= 0f)
				{
					return MalfChancesKeys[0].Localized();
				}
				string text = null;
				text = ((MalfFeedChance < MaxMalfFeedChance / 7f) ? MalfChancesKeys[1] : ((MalfFeedChance < 3f * MaxMalfFeedChance / 7f) ? MalfChancesKeys[2] : ((MalfFeedChance < 5f * MaxMalfFeedChance / 7f) ? MalfChancesKeys[3] : ((!(MalfFeedChance < 6f * MaxMalfFeedChance / 7f)) ? MalfChancesKeys[5] : MalfChancesKeys[4]))));
				return text.Localized();
			},
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddQualityToList(new _EB10(EItemAttributeId.DurabilityBurn)
		{
			Name = EItemAttributeId.DurabilityBurn.GetName(),
			Base = () => DurabilityBurnModificator - 1f,
			StringValue = () => (DurabilityBurnModificator - 1f).ToString(_ED3E._E000(229347)),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored,
			LessIsGood = true
		});
		SafelyAddQualityToList(new _EB10(EItemAttributeId.HeatFactor)
		{
			Name = EItemAttributeId.HeatFactor.GetName(),
			Base = () => HeatFactor - 1f,
			StringValue = () => (HeatFactor - 1f).ToString(_ED3E._E000(229347)),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored,
			LessIsGood = true
		});
		return _cachedQualities;
	}

	public void SafelyAddQualityToList(_EB10 itemAttribute)
	{
		if (itemAttribute.Base() != 0f)
		{
			_cachedQualities.Add(itemAttribute);
		}
	}

	public SonicBulletSoundPlayer.SonicType GetCachedSonicType()
	{
		if (_cachedSonicType.HasValue)
		{
			return _cachedSonicType.Value;
		}
		if (casingSounds.Contains(_ED3E._E000(32735)) || casingSounds.Contains(_ED3E._E000(229195)))
		{
			_cachedSonicType = SonicBulletSoundPlayer.SonicType.Sonic9;
		}
		else if (casingSounds.Contains(_ED3E._E000(229184)))
		{
			_cachedSonicType = SonicBulletSoundPlayer.SonicType.Sonic762;
		}
		else if (casingSounds.Contains(_ED3E._E000(32714)) || casingSounds.Contains(_ED3E._E000(229244)))
		{
			_cachedSonicType = SonicBulletSoundPlayer.SonicType.SonicShotgun;
		}
		else
		{
			_cachedSonicType = SonicBulletSoundPlayer.SonicType.Sonic545;
		}
		return _cachedSonicType.Value;
	}

	protected override List<IItemComponent> CreateReadonlyComponentsCollection()
	{
		List<IItemComponent> list = base.CreateReadonlyComponentsCollection();
		if (HasGrenaderComponent)
		{
			list.Add(new _E9D6(this));
		}
		return list;
	}

	[CompilerGenerated]
	private float _E000()
	{
		return InitialSpeed;
	}

	[CompilerGenerated]
	private string _E001()
	{
		return InitialSpeed + _ED3E._E000(18502) + _ED3E._E000(229348).Localized();
	}

	[CompilerGenerated]
	private string _E002()
	{
		return Caliber.Localized();
	}

	[CompilerGenerated]
	private float _E003()
	{
		return ammoAccr;
	}

	[CompilerGenerated]
	private string _E004()
	{
		return ammoAccr.ToString(_ED3E._E000(229344)) + _ED3E._E000(149464);
	}

	[CompilerGenerated]
	private float _E005()
	{
		return ammoRec;
	}

	[CompilerGenerated]
	private string _E006()
	{
		return ammoRec.ToString();
	}

	[CompilerGenerated]
	private float _E007()
	{
		return HeavyBleedingDelta;
	}

	[CompilerGenerated]
	private string _E008()
	{
		return HeavyBleedingDelta.ToString(_ED3E._E000(45972));
	}

	[CompilerGenerated]
	private float _E009()
	{
		return LightBleedingDelta;
	}

	[CompilerGenerated]
	private string _E00A()
	{
		return LightBleedingDelta.ToString(_ED3E._E000(45972));
	}

	[CompilerGenerated]
	private float _E00B()
	{
		return MalfMisfireChance;
	}

	[CompilerGenerated]
	private string _E00C()
	{
		if (MalfMisfireChance <= 0f)
		{
			return MalfChancesKeys[0].Localized();
		}
		string text = null;
		int num = MalfChancesKeys.Length - 3 - 1;
		for (int i = 3; i < 7; i++)
		{
			if (MalfMisfireChance < (float)i * MaxMalfMisfireChance / 7f)
			{
				text = MalfChancesKeys[i - num];
				break;
			}
		}
		text = text ?? MalfChancesKeys[MalfChancesKeys.Length - 1];
		return text.Localized();
	}

	[CompilerGenerated]
	private float _E00D()
	{
		return MalfFeedChance;
	}

	[CompilerGenerated]
	private string _E00E()
	{
		if (MalfFeedChance <= 0f)
		{
			return MalfChancesKeys[0].Localized();
		}
		string text = null;
		text = ((MalfFeedChance < MaxMalfFeedChance / 7f) ? MalfChancesKeys[1] : ((MalfFeedChance < 3f * MaxMalfFeedChance / 7f) ? MalfChancesKeys[2] : ((MalfFeedChance < 5f * MaxMalfFeedChance / 7f) ? MalfChancesKeys[3] : ((!(MalfFeedChance < 6f * MaxMalfFeedChance / 7f)) ? MalfChancesKeys[5] : MalfChancesKeys[4]))));
		return text.Localized();
	}

	[CompilerGenerated]
	private float _E00F()
	{
		return DurabilityBurnModificator - 1f;
	}

	[CompilerGenerated]
	private string _E010()
	{
		return (DurabilityBurnModificator - 1f).ToString(_ED3E._E000(229347));
	}

	[CompilerGenerated]
	private float _E011()
	{
		return HeatFactor - 1f;
	}

	[CompilerGenerated]
	private string _E012()
	{
		return (HeatFactor - 1f).ToString(_ED3E._E000(229347));
	}
}
