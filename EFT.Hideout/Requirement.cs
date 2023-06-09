using EFT.InventoryLogic;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.Hideout;

public abstract class Requirement
{
	public readonly _ECEC OnFulfillmentChange = new _ECEC();

	private int _userValue = -1;

	[JsonProperty("type")]
	public abstract ERequirementType Type { get; }

	[JsonIgnore]
	public EAreaType SourceAreaType { get; private set; }

	[JsonIgnore]
	public int SourceAreaLevel { get; private set; }

	[JsonIgnore]
	public virtual bool Fulfilled { get; private set; }

	[JsonIgnore]
	public InventoryError Error { get; protected set; }

	public void Init(EAreaType sourceAreaType, int sourceAreaLevel)
	{
		SourceAreaType = sourceAreaType;
		SourceAreaLevel = sourceAreaLevel;
	}

	protected void TestRequirement(int userValue, int targetValue)
	{
		bool fulfilled = Fulfilled;
		Fulfilled = userValue >= targetValue && Error == null;
		userValue = Mathf.Clamp(userValue, -1, targetValue);
		if (_userValue != userValue || Fulfilled != fulfilled)
		{
			_userValue = userValue;
			OnFulfillmentChange?.Invoke();
		}
	}

	protected void SetFulfillment(bool value)
	{
		if (Fulfilled != value)
		{
			Fulfilled = value;
			OnFulfillmentChange?.Invoke();
		}
	}
}
