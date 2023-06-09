using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.InventoryLogic;

public sealed class ResourceComponent : _EB19, _EA8E, IItemComponent
{
	[CompilerGenerated]
	private float _E016;

	private readonly _EA8F m__E000;

	[_E63C]
	public float Value
	{
		[CompilerGenerated]
		get
		{
			return _E016;
		}
		[CompilerGenerated]
		set
		{
			_E016 = value;
		}
	}

	public float MaxResource => this.m__E000.MaxResource;

	public ResourceComponent(Item item, _EA8F template)
		: base(item)
	{
		this.m__E000 = template;
		Value = template.MaxResource;
		item.SafelyAddAttributeToList(new _EB11(EItemAttributeId.Resource)
		{
			Name = EItemAttributeId.Resource.GetName(),
			Base = () => Value,
			StringValue = () => Value.ToString(_ED3E._E000(29354)),
			DisplayType = () => EItemAttributeDisplayType.FullBar,
			Range = new Vector2(0f, MaxResource)
		});
	}

	[CompilerGenerated]
	private float _E000()
	{
		return Value;
	}

	[CompilerGenerated]
	private string _E001()
	{
		return Value.ToString(_ED3E._E000(29354));
	}
}
