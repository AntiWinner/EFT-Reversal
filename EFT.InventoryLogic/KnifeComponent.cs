using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public class KnifeComponent : _EB19
{
	public readonly _E9E0 Template;

	public KnifeComponent(Item item, _E9E0 template)
		: base(item)
	{
		Template = template;
		Item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.KnifeHitRadius)
		{
			Name = EItemAttributeId.KnifeHitRadius.GetName(),
			Base = () => Template.KnifeHitRadius,
			StringValue = () => Template.KnifeHitRadius + _ED3E._E000(18502) + _ED3E._E000(215643).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.KnifeHitSlashDam)
		{
			Name = EItemAttributeId.KnifeHitSlashDam.GetName(),
			Base = () => Template.KnifeHitSlashDam,
			StringValue = () => Template.KnifeHitSlashDam.ToString(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.KnifeHitStabDam)
		{
			Name = EItemAttributeId.KnifeHitStabDam.GetName(),
			Base = () => Template.KnifeHitStabDam,
			StringValue = () => Template.KnifeHitStabDam.ToString(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
	}

	[CompilerGenerated]
	private float _E000()
	{
		return Template.KnifeHitRadius;
	}

	[CompilerGenerated]
	private string _E001()
	{
		return Template.KnifeHitRadius + _ED3E._E000(18502) + _ED3E._E000(215643).Localized();
	}

	[CompilerGenerated]
	private float _E002()
	{
		return Template.KnifeHitSlashDam;
	}

	[CompilerGenerated]
	private string _E003()
	{
		return Template.KnifeHitSlashDam.ToString();
	}

	[CompilerGenerated]
	private float _E004()
	{
		return Template.KnifeHitStabDam;
	}

	[CompilerGenerated]
	private string _E005()
	{
		return Template.KnifeHitStabDam.ToString();
	}
}
