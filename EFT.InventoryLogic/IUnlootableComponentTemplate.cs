namespace EFT.InventoryLogic;

public interface IUnlootableComponentTemplate
{
	string SlotName { get; }

	EPlayerSideMask Side { get; }
}
