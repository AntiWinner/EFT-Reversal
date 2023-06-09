using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.HealthSystem;
using UnityEngine;

namespace EFT.InventoryLogic;

public class SideEffectComponent : StimulatorBuffsComponent, _EA8E, IItemComponent
{
	private float _E003;

	private readonly _E9E8 m__E000;

	[_E63C]
	public float Value
	{
		get
		{
			return _E003;
		}
		set
		{
			if (value.ApproxEquals(_E003))
			{
				return;
			}
			_E003 = Mathf.Max(0f, value);
			if (value.IsZero() && MaxResource.Positive())
			{
				Item.Attributes.RemoveAll((_EB10 attribute) => attribute is _E9DE);
			}
		}
	}

	public float MaxResource => this.m__E000.MaxResource;

	public bool IsEmpty
	{
		get
		{
			if (MaxResource.Positive())
			{
				return (Value - 1f).Negative();
			}
			return false;
		}
	}

	public SideEffectComponent(Item item, _E9E8 template)
		: base(item, template)
	{
		this.m__E000 = template;
		Value = template.MaxResource;
		if (base.BuffSettings.Any((_E989._E016._E000 x) => x.BuffType == EStimulatorBuffType.UnknownToxin))
		{
			item.Attributes.Add(new _EB10(EItemAttributeId.PoisonedWeapon)
			{
				Name = EItemAttributeId.PoisonedWeapon.GetName(),
				StringValue = () => string.Format(_ED3E._E000(215941).Localized(), Math.Round(Value)),
				DisplayType = () => EItemAttributeDisplayType.Compact
			});
		}
		if (Value.Positive() || template.MaxResource.ZeroOrNegative())
		{
			SetupStimulatorBuffsAttributes(Item);
		}
	}

	[CompilerGenerated]
	private string _E000()
	{
		return string.Format(_ED3E._E000(215941).Localized(), Math.Round(Value));
	}
}
