using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.InventoryLogic;

public sealed class BuffComponent : _EB19
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public int value;

		internal float _E000()
		{
			return value;
		}

		internal string _E001()
		{
			return value + _ED3E._E000(18502) + _ED3E._E000(149464).Localized();
		}
	}

	private const float m__E001 = 0.5f;

	private ERepairBuffType m__E002;

	private double m__E003;

	private double _E004;

	[_E63C]
	[JsonProperty("rarity")]
	public EBuffRarity Rarity;

	[CompilerGenerated]
	private EItemAttributeId _E005;

	[CompilerGenerated]
	private double _E006;

	[CompilerGenerated]
	private double _E007;

	[CompilerGenerated]
	private double _E008;

	public bool IsActive
	{
		get
		{
			if (BuffType >= ERepairBuffType.WeaponSpread && Rarity >= EBuffRarity.Common)
			{
				return !Value.Equals(1.0);
			}
			return false;
		}
	}

	public override bool Serialized => IsActive;

	[JsonProperty("buffType")]
	[_E63C]
	public ERepairBuffType BuffType
	{
		get
		{
			return this.m__E002;
		}
		set
		{
			this.m__E002 = value;
			_E000();
		}
	}

	[JsonProperty("value")]
	[_E63C]
	public double Value
	{
		get
		{
			return this.m__E003;
		}
		set
		{
			this.m__E003 = value;
			_E000();
		}
	}

	[JsonProperty("thresholdDurability")]
	[_E63C]
	public double ThresholdDurability
	{
		get
		{
			return _E004;
		}
		set
		{
			_E004 = value;
			_E000();
		}
	}

	public EItemAttributeId BuffAttributeId
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public double WeaponSpread
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		private set
		{
			_E006 = value;
		}
	}

	public double DamageReduction
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
		[CompilerGenerated]
		private set
		{
			_E007 = value;
		}
	}

	public double MalfunctionProtections
	{
		[CompilerGenerated]
		get
		{
			return _E008;
		}
		[CompilerGenerated]
		private set
		{
			_E008 = value;
		}
	}

	public BuffComponent(Item item)
		: base(item)
	{
		DisableComponent();
	}

	public void DisableComponent()
	{
		this.m__E002 = (ERepairBuffType)(-1);
		Rarity = (EBuffRarity)(-1);
		this.m__E003 = 1.0;
		_E004 = -1.0;
		_E000();
	}

	public void TryDisableComponent(float durability)
	{
		if ((double)durability < Math.Max(_E004, 0.5))
		{
			DisableComponent();
		}
	}

	private void _E000()
	{
		WeaponSpread = 1.0;
		DamageReduction = 1.0;
		MalfunctionProtections = 1.0;
		EItemAttributeId eItemAttributeId;
		switch (this.m__E002)
		{
		case ERepairBuffType.DamageReduction:
			DamageReduction = this.m__E003;
			eItemAttributeId = ((Rarity == EBuffRarity.Common) ? EItemAttributeId.DamageReductionCommonBuff : EItemAttributeId.DamageReductionRareBuff);
			break;
		case ERepairBuffType.MalfunctionProtections:
			MalfunctionProtections = this.m__E003;
			eItemAttributeId = ((Rarity == EBuffRarity.Common) ? EItemAttributeId.MalfunctionProtectionCommonBuff : EItemAttributeId.MalfunctionProtectionRareBuff);
			break;
		case ERepairBuffType.WeaponSpread:
			WeaponSpread = this.m__E003;
			eItemAttributeId = ((Rarity == EBuffRarity.Common) ? EItemAttributeId.WeaponSpreadCommonBuff : EItemAttributeId.WeaponSpreadRareBuff);
			break;
		default:
			_E001();
			return;
		}
		int value = (int)((1.0 - this.m__E003) * 100.0);
		RepairableComponent itemComponent = Item.GetItemComponent<RepairableComponent>();
		float durability = itemComponent.Durability;
		_E9D2 buff = new _E9D2(eItemAttributeId, itemComponent, this)
		{
			Name = eItemAttributeId.GetName(),
			Base = () => value,
			StringValue = () => value + _ED3E._E000(18502) + _ED3E._E000(149464).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		};
		_E002(buff);
		float val = (float)itemComponent.TemplateDurability * Singleton<_E5CB>.Instance.RepairSettings.maxDurabilityLossToRemoveBuff;
		float val2 = durability * Singleton<_E5CB>.Instance.RepairSettings.currentDurabilityLossToRemoveBuff;
		if (_E004.Negative() && _E3A5<ERepairBuffType>.IsDefined(this.m__E002) && _E3A5<EBuffRarity>.IsDefined(Rarity) && Value.Equals(1.0))
		{
			Debug.LogError(_ED3E._E000(215286));
			_E004 = Math.Max(durability - Math.Max(val, val2), 0f);
		}
	}

	private void _E001()
	{
		if (BuffAttributeId == EItemAttributeId.Undefined)
		{
			return;
		}
		_E003(BuffAttributeId, out var enhancedAttributeId);
		for (int num = Item.Attributes.Count - 1; num >= 0; num--)
		{
			_EB10 obj = Item.Attributes[num];
			if (obj.Id.Equals(enhancedAttributeId))
			{
				obj.Enhancement = null;
			}
			if (obj.Id.Equals(BuffAttributeId))
			{
				Item.Attributes.Remove(obj);
			}
		}
		BuffAttributeId = EItemAttributeId.Undefined;
	}

	private void _E002(_E9D2 buff)
	{
		_E001();
		BuffAttributeId = (EItemAttributeId)(object)buff.Id;
		Item.Attributes.Add(buff);
		if (!_E003(BuffAttributeId, out var enhancedAttributeId))
		{
			return;
		}
		foreach (_EB10 attribute in Item.Attributes)
		{
			if (attribute.Id.Equals(enhancedAttributeId))
			{
				attribute.Enhancement = buff;
			}
		}
	}

	private bool _E003(EItemAttributeId buffAttributeId, out EItemAttributeId enhancedAttributeId)
	{
		if (buffAttributeId == EItemAttributeId.WeaponSpreadCommonBuff || buffAttributeId == EItemAttributeId.WeaponSpreadRareBuff)
		{
			enhancedAttributeId = EItemAttributeId.CenterOfImpact;
			return true;
		}
		enhancedAttributeId = EItemAttributeId.Undefined;
		return false;
	}
}
