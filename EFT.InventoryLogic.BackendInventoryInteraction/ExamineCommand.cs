using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class ExamineCommand : _EB6F, ICommandWithItem
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(192998);

	[JsonProperty("item")]
	public string Item;

	[JsonIgnore]
	public string ItemTemplate { get; set; }
}
