namespace EFT.InventoryLogic;

public class CantPutIntoDuringRaidComponent : _EB19
{
	public CantPutIntoDuringRaidComponent(Item item)
		: base(item)
	{
	}

	public bool CanPutInto()
	{
		return !_E7A3.InRaid;
	}
}
