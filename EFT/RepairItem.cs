using System;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public sealed class RepairItem
{
	[JsonProperty("_id")]
	public string Id;

	[JsonProperty("count")]
	public float Count;

	public RepairItem(string id, float repairAmount)
	{
		Id = id;
		Count = repairAmount;
	}
}
