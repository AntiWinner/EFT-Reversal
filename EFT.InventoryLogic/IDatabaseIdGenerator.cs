namespace EFT.InventoryLogic;

public interface IDatabaseIdGenerator
{
	MongoID NextId { get; }

	void RollBack();
}
