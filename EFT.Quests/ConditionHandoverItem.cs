using System;
using System.Linq;
using System.Text;
using EFT.InventoryLogic;

namespace EFT.Quests;

public sealed class ConditionHandoverItem : ConditionItem, _E921
{
	private enum ETagHandoverTypes
	{
		Item,
		OnlyFoundInRaid,
		Durability,
		DurabilityStrict,
		IsEncoded
	}

	public override string FormattedDescription
	{
		get
		{
			if (!base.DynamicLocale)
			{
				return string.Format(base.FormattedDescription, (target.First() + " ShortName").Localized(), base.value, minDurability, maxDurability, dogtagLevel, onlyFoundInRaid);
			}
			return GenerateDescription();
		}
	}

	private string LocalizationActionKey => "QuestCondition/HandoverItem";

	public static _E557[] ConvertToHandoverItems(Item[] items)
	{
		_E557[] array = new _E557[items.Length];
		for (int i = 0; i < array.Length; i++)
		{
			Item item = items[i];
			int stackObjectsCount = item.StackObjectsCount;
			array[i] = new _E557(item, stackObjectsCount);
		}
		return array;
	}

	private string GenerateDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(LocalizationActionKey.Localized());
		foreach (ETagHandoverTypes value in _E3A5<ETagHandoverTypes>.Values)
		{
			string oldValue = "{" + _E3A5<ETagHandoverTypes>.GetName(value).ToLower() + "}";
			string text = LocalizationActionKey + "/" + value;
			switch (value)
			{
			case ETagHandoverTypes.Item:
			{
				string newValue3 = (string.IsNullOrEmpty(target.First()) ? string.Empty : target.First().LocalizedName());
				stringBuilder.Replace(oldValue, newValue3);
				break;
			}
			case ETagHandoverTypes.OnlyFoundInRaid:
			{
				string newValue4 = ((!onlyFoundInRaid) ? string.Empty : text.Localized());
				stringBuilder.Replace(oldValue, newValue4);
				break;
			}
			case ETagHandoverTypes.IsEncoded:
			{
				string newValue2 = ((!isEncoded) ? string.Empty : text.Localized());
				stringBuilder.Replace(oldValue, newValue2);
				break;
			}
			case ETagHandoverTypes.Durability:
				if ((minDurability == 0 && maxDurability == 100) || minDurability == maxDurability)
				{
					stringBuilder.Replace(oldValue, string.Empty);
				}
				else
				{
					stringBuilder.Replace(oldValue, string.Format(text.Localized(), minDurability, maxDurability));
				}
				break;
			case ETagHandoverTypes.DurabilityStrict:
			{
				string newValue = ((minDurability == maxDurability && minDurability != 0) ? string.Format(text.Localized(), maxDurability.ToString()) : string.Empty);
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
