using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public class Mod : _EA40, _E9F8
{
	public int EffectiveDistance => GetTemplate<ModTemplate>().EffectiveDistance;

	public int Loudness => GetTemplate<ModTemplate>().Loudness;

	public int Accuracy => GetTemplate<ModTemplate>().Accuracy;

	public float DoubleActionAccuracyPenaltyMult => GetTemplate<ModTemplate>().DoubleActionAccuracyPenaltyMult;

	public float Recoil => GetTemplate<ModTemplate>().Recoil;

	public float Ergonomics => GetTemplate<ModTemplate>().Ergonomics;

	public float Velocity => GetTemplate<ModTemplate>().Velocity;

	public bool RaidModdable => GetTemplate<ModTemplate>().RaidModdable;

	public bool ToolModdable => GetTemplate<ModTemplate>().ToolModdable;

	public bool BlocksFolding => GetTemplate<ModTemplate>().BlocksFolding;

	public bool IsAnimated => GetTemplate<ModTemplate>().IsAnimated;

	public float SightingRange => GetTemplate<ModTemplate>().SightingRange;

	public virtual float DurabilityBurnModificator => 1f;

	public virtual float HeatFactor => 1f;

	public virtual float CoolFactor => 1f;

	public override List<EItemInfoButton> ItemInteractionButtons => base.ItemInteractionButtons.Concat(new List<EItemInfoButton>
	{
		EItemInfoButton.Install,
		EItemInfoButton.Uninstall
	}).ToList();

	public new ModTemplate Template => GetTemplate<ModTemplate>();

	public Mod(string id, ModTemplate template)
		: base(id, template)
	{
		if (template.HasShoulderContact)
		{
			Components.Add(new ProtrudableComponent(this));
		}
		Attributes.Add(new _EB10(EItemAttributeId.RaidModdable)
		{
			Name = EItemAttributeId.RaidModdable.GetName(),
			Base = () => RaidModdable ? 1 : 0,
			StringValue = () => (!RaidModdable) ? _ED3E._E000(215946).Localized().ToUpper() : _ED3E._E000(224787).Localized().ToUpper(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.EffectiveDistance)
		{
			Name = EItemAttributeId.EffectiveDistance.GetName(),
			Base = () => EffectiveDistance,
			StringValue = ((object)EffectiveDistance).ToString,
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.Loudness)
		{
			Name = EItemAttributeId.Loudness.GetName(),
			Base = () => Loudness,
			StringValue = ((object)Loudness).ToString,
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.Accuracy)
		{
			Name = EItemAttributeId.Accuracy.GetName(),
			Base = () => Accuracy,
			StringValue = () => Accuracy + _ED3E._E000(149464),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		SafelyAddAttributeToList(new _EB11(EItemAttributeId.Recoil)
		{
			Name = EItemAttributeId.Recoil.GetName(),
			Base = () => Recoil,
			LessIsGood = true,
			StringValue = () => Recoil + _ED3E._E000(149464),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.Ergonomics)
		{
			Name = EItemAttributeId.Ergonomics.GetName(),
			Base = () => Ergonomics,
			StringValue = () => Ergonomics.ToString(),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.Velocity)
		{
			Name = EItemAttributeId.Velocity.GetName(),
			Base = () => Velocity,
			StringValue = ((object)Velocity).ToString,
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.SightingRange)
		{
			Name = EItemAttributeId.SightingRange.GetName(),
			Base = () => SightingRange,
			StringValue = ((object)SightingRange).ToString,
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.DurabilityBurn)
		{
			Name = EItemAttributeId.DurabilityBurn.GetName(),
			Base = () => DurabilityBurnModificator - 1f,
			StringValue = () => (DurabilityBurnModificator - 1f).ToString(_ED3E._E000(229347)),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored,
			LessIsGood = true
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.HeatFactor)
		{
			Name = EItemAttributeId.HeatFactor.GetName(),
			Base = () => HeatFactor - 1f,
			StringValue = () => (HeatFactor - 1f).ToString(_ED3E._E000(229347)),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored,
			LessIsGood = true
		});
		SafelyAddAttributeToList(new _EB10(EItemAttributeId.CoolFactor)
		{
			Name = EItemAttributeId.CoolFactor.GetName(),
			Base = () => CoolFactor - 1f,
			StringValue = () => (CoolFactor - 1f).ToString(_ED3E._E000(229347)),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
	}

	public _ECD9<bool> CanBeMoved(IContainer toContainer)
	{
		if (!_E7A3.InRaid)
		{
			return true;
		}
		if (toContainer is Slot slot)
		{
			if (!RaidModdable)
			{
				return new _E9FF(this);
			}
			if (slot.Required)
			{
				return new _EA01(slot);
			}
		}
		return true;
	}

	public override bool Compare(Item item)
	{
		if (!base.Compare(item))
		{
			return false;
		}
		if (!(item is Mod mod))
		{
			return false;
		}
		if (EffectiveDistance != mod.EffectiveDistance || Loudness != mod.Loudness || Accuracy != mod.Accuracy || Recoil != mod.Recoil || Ergonomics != mod.Ergonomics || Velocity != mod.Velocity || RaidModdable != mod.RaidModdable || ToolModdable != mod.ToolModdable || SightingRange != mod.SightingRange)
		{
			return false;
		}
		return true;
	}

	public IEnumerable<Weapon> GetSuitableWeapons(_EA40[] collections)
	{
		Weapon[] array = collections.GetAllItemsFromCollections().Concat(collections).ToArray()
			.OfType<Weapon>()
			.Distinct()
			.ToArray();
		List<Weapon> list = new List<Weapon>();
		Weapon[] array2 = array;
		foreach (Weapon weapon in array2)
		{
			if (weapon.IsModSuitable(this))
			{
				list.Add(weapon);
			}
		}
		return list;
	}

	[CompilerGenerated]
	private float _E000()
	{
		return RaidModdable ? 1 : 0;
	}

	[CompilerGenerated]
	private string _E001()
	{
		if (!RaidModdable)
		{
			return _ED3E._E000(215946).Localized().ToUpper();
		}
		return _ED3E._E000(224787).Localized().ToUpper();
	}

	[CompilerGenerated]
	private float _E002()
	{
		return EffectiveDistance;
	}

	[CompilerGenerated]
	private float _E003()
	{
		return Loudness;
	}

	[CompilerGenerated]
	private float _E004()
	{
		return Accuracy;
	}

	[CompilerGenerated]
	private string _E005()
	{
		return Accuracy + _ED3E._E000(149464);
	}

	[CompilerGenerated]
	private float _E006()
	{
		return Recoil;
	}

	[CompilerGenerated]
	private string _E007()
	{
		return Recoil + _ED3E._E000(149464);
	}

	[CompilerGenerated]
	private float _E008()
	{
		return Ergonomics;
	}

	[CompilerGenerated]
	private string _E009()
	{
		return Ergonomics.ToString();
	}

	[CompilerGenerated]
	private float _E00A()
	{
		return Velocity;
	}

	[CompilerGenerated]
	private float _E00B()
	{
		return SightingRange;
	}

	[CompilerGenerated]
	private float _E00C()
	{
		return DurabilityBurnModificator - 1f;
	}

	[CompilerGenerated]
	private string _E00D()
	{
		return (DurabilityBurnModificator - 1f).ToString(_ED3E._E000(229347));
	}

	[CompilerGenerated]
	private float _E00E()
	{
		return HeatFactor - 1f;
	}

	[CompilerGenerated]
	private string _E00F()
	{
		return (HeatFactor - 1f).ToString(_ED3E._E000(229347));
	}

	[CompilerGenerated]
	private float _E010()
	{
		return CoolFactor - 1f;
	}

	[CompilerGenerated]
	private string _E011()
	{
		return (CoolFactor - 1f).ToString(_ED3E._E000(229347));
	}
}
