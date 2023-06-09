using System;
using System.Linq;
using System.Text;

namespace EFT.Quests;

public class ConditionHit : Condition
{
	private enum EHitTagTypes
	{
		Target,
		Distance,
		Weapon,
		WeaponType,
		BodyPart,
		OneSession,
		BotRole
	}

	public string target;

	public _E91C distance;

	public string[] weapon;

	public string[] weaponCategories;

	public string[][] weaponModsInclusive;

	public string[][] weaponModsExclusive;

	public EBodyPart[] bodyPart;

	public string[] savageRole;

	public _E91D daytime;

	public new bool resetOnSessionEnd;

	public string[][] enemyEquipmentInclusive;

	public string[][] enemyEquipmentExclusive;

	public _E994[] enemyHealthEffects;

	protected virtual string LocalizationKey => "QuestCondition/Hit";

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
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(LocalizationKey.Localized());
		foreach (EHitTagTypes value in _E3A5<EHitTagTypes>.Values)
		{
			string oldValue = "{" + _E3A5<EHitTagTypes>.GetName(value).ToLower() + "}";
			string localeKey = LocalizationKey + "/" + _E3A5<EHitTagTypes>.GetName(value);
			switch (value)
			{
			case EHitTagTypes.Target:
			{
				string text = localeKey + "/" + target;
				string newValue3 = ((string.IsNullOrEmpty(target) || !savageRole.IsNullOrEmpty()) ? string.Empty : text.Localized());
				stringBuilder.Replace(oldValue, newValue3);
				break;
			}
			case EHitTagTypes.Distance:
			{
				if (distance == null || distance.value == 0)
				{
					stringBuilder.Replace(oldValue, string.Empty);
					break;
				}
				string arg = string.Empty;
				switch (distance.compareMethod)
				{
				case ECompareMethod.Less:
				case ECompareMethod.LessOrEqual:
					arg = " " + ECompareMethod.Less.Localized(EStringCase.None);
					break;
				case ECompareMethod.MoreOrEqual:
				case ECompareMethod.More:
					arg = " " + ECompareMethod.More.Localized(EStringCase.None);
					break;
				}
				string newValue4 = string.Format(localeKey.Localized(), arg, distance.value);
				stringBuilder.Replace(oldValue, newValue4);
				break;
			}
			case EHitTagTypes.Weapon:
			{
				string newValue6 = ((weapon == null || string.IsNullOrEmpty(weapon.First())) ? string.Empty : string.Format(localeKey.Localized(), weapon.First().LocalizedName()));
				stringBuilder.Replace(oldValue, newValue6);
				break;
			}
			case EHitTagTypes.WeaponType:
			{
				string newValue7 = (weaponCategories.IsNullOrEmpty() ? string.Empty : string.Format(localeKey.Localized(), string.Join("StringSeparator/Or".Localized(), weaponCategories.Select((string x) => x.Localized()))));
				stringBuilder.Replace(oldValue, newValue7);
				break;
			}
			case EHitTagTypes.BodyPart:
			{
				string newValue2 = (bodyPart.IsNullOrEmpty() ? string.Empty : string.Format(localeKey.Localized(), string.Join("StringSeparator/Or".Localized(), bodyPart.Select((EBodyPart x) => (localeKey + "/" + _E3A5<EBodyPart>.GetName(x)).Localized(EStringCase.Lower)))));
				stringBuilder.Replace(oldValue, newValue2);
				break;
			}
			case EHitTagTypes.OneSession:
			{
				string newValue5 = (resetOnSessionEnd ? localeKey.Localized() : string.Empty);
				stringBuilder.Replace(oldValue, newValue5);
				break;
			}
			case EHitTagTypes.BotRole:
			{
				string newValue = (savageRole.IsNullOrEmpty() ? string.Empty : string.Format(localeKey.Localized(), (localeKey + "/" + savageRole.First()).Localized()));
				stringBuilder.Replace(oldValue, newValue);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		return stringBuilder.ToString();
	}
}
