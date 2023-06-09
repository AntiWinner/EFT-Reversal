using System;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class HealthRequirement : Requirement
{
	[JsonProperty("Energy")]
	private int _energyValue;

	[JsonProperty("Hydration")]
	private int _hydrationValue;

	private _E981 _healthController;

	public override ERequirementType Type => ERequirementType.Health;

	public override bool Fulfilled
	{
		get
		{
			if (_healthController.Energy.Current >= (float)_energyValue)
			{
				return _healthController.Hydration.Current >= (float)_hydrationValue;
			}
			return false;
		}
	}

	public void Test(_E981 healthController)
	{
		_healthController = healthController;
	}
}
