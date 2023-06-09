using EFT.InventoryLogic;

namespace EFT;

public interface IExchangeRequirement
{
	Item Item { get; }

	string TemplateId { get; }

	string ItemName { get; }

	int IntCount { get; }

	double PreciseCount { get; }

	bool OnlyFunctional { get; }

	bool IsEncoded { get; }
}
