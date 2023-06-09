using System;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class ProductionData
{
	[JsonProperty("Progress")]
	public float Progress;

	[JsonProperty("StartTimestamp", NullValueHandling = NullValueHandling.Ignore)]
	public int StartTimestamp;

	[JsonProperty("ProductionTime")]
	public int ProductionTime;

	[JsonProperty("inProgress")]
	public bool InProgress;

	[JsonProperty("RecipeId")]
	public string RecipeId;

	[JsonProperty("Products")]
	public _E53A[] Products;

	[JsonProperty("Interrupted")]
	public bool Interrupted;
}
