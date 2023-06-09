using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class DeleteNoteCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197836);

	[JsonProperty("index")]
	public int Index;
}
