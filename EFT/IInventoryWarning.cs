namespace EFT;

public interface IInventoryWarning
{
	int ErrorCode { get; }

	bool TryGetMessage(out string header, out string description);
}
