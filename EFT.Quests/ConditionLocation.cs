using System.Linq;

namespace EFT.Quests;

public sealed class ConditionLocation : Condition
{
	public string[] target;

	private static string LocaleLocalizationKey => "QuestCondition/SurviveOnLocation/Location";

	private static string AnyLocaleLocalizationKey => "QuestCondition/SurviveOnLocation/Any";

	public override string FormattedDescription
	{
		get
		{
			if (!base.DynamicLocale)
			{
				return base.FormattedDescription;
			}
			return GenerateFormattedDescription();
		}
	}

	private string GenerateFormattedDescription()
	{
		if (target == null || target.First().IsNullOrEmpty())
		{
			return base.FormattedDescription;
		}
		if (!(target.First() == "Any"))
		{
			return LocaleLocalizationKey.Localized();
		}
		return AnyLocaleLocalizationKey.Localized();
	}
}
