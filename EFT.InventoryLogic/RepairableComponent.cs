using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.InventoryLogic;

public class RepairableComponent : _EB19
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EB11 maxDurabilityAttribute;

		public RepairableComponent _003C_003E4__this;

		internal float _E000()
		{
			return maxDurabilityAttribute.Base() - _003C_003E4__this.Durability;
		}
	}

	[_E63C]
	public float MaxDurability;

	[_E63C]
	public float Durability;

	private readonly _E9E7 m__E000;

	public int TemplateDurability => this.m__E000.MaxDurability;

	public Vector2 RepairDegradation
	{
		get
		{
			ArmorComponent itemComponent = Item.GetItemComponent<ArmorComponent>();
			if (itemComponent != null)
			{
				_E5CB._E026 obj = Singleton<_E5CB>.Instance.ArmorMaterials[itemComponent.Template.ArmorMaterial];
				return new Vector2(obj.MinRepairDegradation, obj.MaxRepairDegradation);
			}
			return new Vector2(this.m__E000.MinRepairDegradation, this.m__E000.MaxRepairDegradation);
		}
	}

	public Vector2 RepairKitDegradation
	{
		get
		{
			ArmorComponent itemComponent = Item.GetItemComponent<ArmorComponent>();
			if (itemComponent != null)
			{
				_E5CB._E026 obj = Singleton<_E5CB>.Instance.ArmorMaterials[itemComponent.Template.ArmorMaterial];
				return new Vector2(obj.MinRepairKitDegradation, obj.MaxRepairKitDegradation);
			}
			return new Vector2(this.m__E000.MinRepairKitDegradation, this.m__E000.MaxRepairKitDegradation);
		}
	}

	public RepairableComponent(Item item, _E9E7 template)
		: base(item)
	{
		this.m__E000 = template;
		Durability = template.Durability;
		MaxDurability = template.MaxDurability;
		_EB11 maxDurabilityAttribute = new _EB11(EItemAttributeId.MaxDurability)
		{
			Name = EItemAttributeId.MaxDurability.GetName(),
			Base = () => MaxDurability,
			DisplayType = () => EItemAttributeDisplayType.Special,
			Range = new Vector2(0f, TemplateDurability)
		};
		item.Attributes.Add(new _EB11(EItemAttributeId.Durability)
		{
			Name = EItemAttributeId.Durability.GetName(),
			Base = () => Durability,
			Delta = () => maxDurabilityAttribute.Base() - Durability,
			DisplayType = () => EItemAttributeDisplayType.Special,
			Range = new Vector2(0f, TemplateDurability)
		});
		item.Attributes.Add(maxDurabilityAttribute);
	}

	[CompilerGenerated]
	private float _E000()
	{
		return MaxDurability;
	}

	[CompilerGenerated]
	private float _E001()
	{
		return Durability;
	}
}
