using System.Collections.Generic;

namespace EFT.InventoryLogic;

public class SlotBlockerComponent : _EB19
{
	public readonly _E9EA Template;

	public readonly string[] ConflictingSlotNames;

	public SlotBlockerComponent(Item item, _E9EA template)
		: base(item)
	{
		Template = template;
		List<string> list = new List<string>();
		if (template.BlocksEarpiece)
		{
			list.Add(EquipmentSlot.Earpiece.ToString());
		}
		if (template.BlocksEyewear)
		{
			list.Add(EquipmentSlot.Eyewear.ToString());
		}
		if (template.BlocksFaceCover)
		{
			list.Add(EquipmentSlot.FaceCover.ToString());
		}
		if (template.BlocksHeadwear)
		{
			list.Add(EquipmentSlot.Headwear.ToString());
		}
		if (template.BlocksArmorVest)
		{
			list.Add(EquipmentSlot.ArmorVest.ToString());
		}
		ConflictingSlotNames = list.ToArray();
	}
}
