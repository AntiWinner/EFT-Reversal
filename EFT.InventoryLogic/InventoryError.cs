using Comfort.Common;

namespace EFT.InventoryLogic;

public abstract class InventoryError : _ECD1
{
	public const string PLAYER_IS_BUSY_ERROR = "Inventory/PlayerIsBusy";

	public virtual string GetLocalizedDescription()
	{
		return ToString();
	}

	public IResult ToResult()
	{
		return new _E9F9(this);
	}
}
