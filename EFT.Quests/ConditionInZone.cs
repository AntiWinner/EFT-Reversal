using System.Linq;

namespace EFT.Quests;

public sealed class ConditionInZone : Condition
{
	public string[] zoneIds;

	public override string FormattedDescription
	{
		get
		{
			if (!base.DynamicLocale)
			{
				return string.Format(base.FormattedDescription, string.Join(",", zoneIds));
			}
			return GenerateFormattedDescription();
		}
	}

	private string GenerateFormattedDescription()
	{
		if (!zoneIds.IsNullOrEmpty())
		{
			return string.Join(",", zoneIds.Select((string x) => x.Localized()));
		}
		return string.Empty;
	}
}
