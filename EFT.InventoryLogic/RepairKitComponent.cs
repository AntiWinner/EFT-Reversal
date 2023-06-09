using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public sealed class RepairKitComponent : _EB19
{
	[_E63C]
	public float Resource;

	private readonly _EA8C m__E000;

	public RepairKitComponent(Item item, _EA8C template)
		: base(item)
	{
		this.m__E000 = template;
		Resource = template.MaxRepairResource;
		Item.Attributes.Add(new _EB10(EItemAttributeId.RaidModdable)
		{
			Name = EItemAttributeId.CanBeUsedInRaid.GetName(),
			Base = () => 0f,
			StringValue = () => _ED3E._E000(215946).Localized().ToUpper(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.Attributes.Add(new _EB10(EItemAttributeId.RepairResource)
		{
			Name = EItemAttributeId.RepairResource.GetName(),
			Base = () => this.m__E000.MaxRepairResource,
			StringValue = () => Resource + _ED3E._E000(30703) + this.m__E000.MaxRepairResource + _ED3E._E000(18502) + _ED3E._E000(215951).Localized(),
			FullStringValue = () => Resource + _ED3E._E000(30703) + this.m__E000.MaxRepairResource + _ED3E._E000(18502) + _ED3E._E000(215951).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
	}

	[CompilerGenerated]
	private float _E000()
	{
		return this.m__E000.MaxRepairResource;
	}

	[CompilerGenerated]
	private string _E001()
	{
		return Resource + _ED3E._E000(30703) + this.m__E000.MaxRepairResource + _ED3E._E000(18502) + _ED3E._E000(215951).Localized();
	}

	[CompilerGenerated]
	private string _E002()
	{
		return Resource + _ED3E._E000(30703) + this.m__E000.MaxRepairResource + _ED3E._E000(18502) + _ED3E._E000(215951).Localized();
	}
}
