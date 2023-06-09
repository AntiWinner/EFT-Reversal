using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class AddNoteCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197795);

	[JsonProperty("note")]
	public _E9CD Note;
}
