using Newtonsoft.Json;

namespace EFT.Hideout;

public abstract class RelatedData
{
	[JsonIgnore]
	public abstract bool IsActive { get; }

	[JsonIgnore]
	public abstract EPanelType Type { get; }

	[JsonIgnore]
	public abstract object Value { get; }
}
