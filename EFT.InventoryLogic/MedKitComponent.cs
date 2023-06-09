using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public sealed class MedKitComponent : _EB19
{
	[_E63C]
	public float HpResource;

	private readonly _E9E4 m__E000;

	public int MaxHpResource => this.m__E000.MaxHpResource;

	public float HpResourceRate => this.m__E000.HpResourceRate;

	public float RelativeResource => HpResource / (float)MaxHpResource * 100f;

	public MedKitComponent(Item item, _E9E4 template)
		: base(item)
	{
		this.m__E000 = template;
		HpResource = template.MaxHpResource;
		Item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.HpResource)
		{
			Name = EItemAttributeId.HpResource.GetName(),
			Base = () => MaxHpResource,
			StringValue = () => (int)HpResource + _ED3E._E000(30703) + MaxHpResource + _ED3E._E000(18502) + _ED3E._E000(215815).Localized(),
			FullStringValue = () => (int)HpResource + _ED3E._E000(30703) + MaxHpResource + _ED3E._E000(18502) + _ED3E._E000(215815).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
	}

	[CompilerGenerated]
	private float _E000()
	{
		return MaxHpResource;
	}

	[CompilerGenerated]
	private string _E001()
	{
		return (int)HpResource + _ED3E._E000(30703) + MaxHpResource + _ED3E._E000(18502) + _ED3E._E000(215815).Localized();
	}

	[CompilerGenerated]
	private string _E002()
	{
		return (int)HpResource + _ED3E._E000(30703) + MaxHpResource + _ED3E._E000(18502) + _ED3E._E000(215815).Localized();
	}
}
