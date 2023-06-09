using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class SwapCommand : _EB70
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197753);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("to")]
	public _EB60._E000 To;

	[JsonProperty("item2")]
	public string Item2;

	[JsonProperty("to2")]
	public _EB60._E000 To2;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public OwnerInfo fromOwner2;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public OwnerInfo toOwner2;
}
