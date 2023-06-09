namespace EFT.Hideout;

public class BitcoinFarmBehaviour : _E831, _E821
{
	public float GetProductionCoefficient(bool isEnergyOn)
	{
		return isEnergyOn ? 1 : 0;
	}
}
