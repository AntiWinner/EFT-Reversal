using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class InsureCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(141606);

	[JsonProperty("tid")]
	public string TraderId;

	[JsonProperty("items")]
	public string[] Items;
}
