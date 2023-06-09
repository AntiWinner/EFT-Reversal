using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class ApplyInventoryChangesCommand : _EB6E
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197722);

	[JsonProperty("changedItems")]
	public IEnumerable<_E53A> ChangedItems;
}
