using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class EditNoteCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197851);

	[JsonProperty("index")]
	public int Index;

	[JsonProperty("note")]
	public _E9CD Note;
}
