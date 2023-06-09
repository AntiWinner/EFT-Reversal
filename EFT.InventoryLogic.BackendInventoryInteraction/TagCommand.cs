using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class TagCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197831);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("TagName")]
	public string TagName;

	[JsonProperty("TagColor")]
	public int TagColor;
}
