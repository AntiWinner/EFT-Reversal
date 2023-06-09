namespace EFT.InventoryLogic;

public class CantRemoveFromSlotsDuringRaidComponent : _EB19
{
	private readonly _EA3F _E000;

	public CantRemoveFromSlotsDuringRaidComponent(Item item, _EA3F template)
		: base(item)
	{
		_E000 = template;
	}

	public bool CanRemoveFromSlotDuringRaid(string equipmentSlotId)
	{
		if (!_E7A3.InRaid)
		{
			return true;
		}
		for (int i = 0; i < _E000.CantRemoveFromSlotsDuringRaid.Length; i++)
		{
			EquipmentSlot equipmentSlot = _E000.CantRemoveFromSlotsDuringRaid[i];
			if (equipmentSlotId == equipmentSlot.ToString())
			{
				return false;
			}
		}
		return true;
	}
}
