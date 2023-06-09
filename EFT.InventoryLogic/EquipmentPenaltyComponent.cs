using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public sealed class EquipmentPenaltyComponent : _EB19
{
	public readonly _E9D4 Template;

	public EquipmentPenaltyComponent(Item item, _E9D4 template)
		: base(item)
	{
		Template = template;
		item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.ChangeMovementSpeed)
		{
			Name = EItemAttributeId.ChangeMovementSpeed.GetName(),
			Base = () => Template.SpeedPenaltyPercent,
			StringValue = () => Template.SpeedPenaltyPercent + _ED3E._E000(215182),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.ChangeTurningSpeed)
		{
			Name = EItemAttributeId.ChangeTurningSpeed.GetName(),
			Base = () => Template.MousePenalty,
			StringValue = () => Template.MousePenalty + _ED3E._E000(215182),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.Ergonomics)
		{
			Name = EItemAttributeId.Ergonomics.GetName(),
			Base = () => Template.WeaponErgonomicPenalty,
			StringValue = () => string.Format(_ED3E._E000(215177), Template.WeaponErgonomicPenalty),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
	}

	[CompilerGenerated]
	private float _E000()
	{
		return Template.SpeedPenaltyPercent;
	}

	[CompilerGenerated]
	private string _E001()
	{
		return Template.SpeedPenaltyPercent + _ED3E._E000(215182);
	}

	[CompilerGenerated]
	private float _E002()
	{
		return Template.MousePenalty;
	}

	[CompilerGenerated]
	private string _E003()
	{
		return Template.MousePenalty + _ED3E._E000(215182);
	}

	[CompilerGenerated]
	private float _E004()
	{
		return Template.WeaponErgonomicPenalty;
	}

	[CompilerGenerated]
	private string _E005()
	{
		return string.Format(_ED3E._E000(215177), Template.WeaponErgonomicPenalty);
	}
}
