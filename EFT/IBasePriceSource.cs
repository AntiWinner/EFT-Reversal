namespace EFT;

public interface IBasePriceSource
{
	double GetBasePrice(string itemId);
}
