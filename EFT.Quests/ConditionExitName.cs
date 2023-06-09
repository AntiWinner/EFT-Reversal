namespace EFT.Quests;

public sealed class ConditionExitName : Condition
{
	public string exitName;

	private static string LocaleLocalizationKey => "QuestCondition/SurviveOnLocation/ExitName";

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
		if (!exitName.IsNullOrEmpty())
		{
			return string.Format(LocaleLocalizationKey.Localized(), exitName.Localized());
		}
		return base.FormattedDescription;
	}
}
