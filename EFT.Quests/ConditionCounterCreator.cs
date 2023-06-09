using System;
using System.Collections.Generic;
using System.Text;

namespace EFT.Quests;

public sealed class ConditionCounterCreator : Condition
{
	public _E91F counter;

	public _E937.EQuestType type;

	public bool doNotResetIfCounterCompleted;

	private static readonly Dictionary<Type, string> _localizationTypes = new Dictionary<Type, string>
	{
		{
			typeof(ConditionLocation),
			"{location}"
		},
		{
			typeof(ConditionExitName),
			"{exitName}"
		},
		{
			typeof(ConditionKills),
			"{kill}"
		},
		{
			typeof(ConditionInZone),
			"{zone}"
		},
		{
			typeof(ConditionFindItem),
			"{findItem}"
		},
		{
			typeof(ConditionEquipment),
			"{equipment}"
		}
	};

	public override string FormattedDescription
	{
		get
		{
			if (!base.DynamicLocale)
			{
				return base.FormattedDescription;
			}
			return LocalizeDescription();
		}
	}

	protected override List<object> IdentityFields()
	{
		List<object> list = base.IdentityFields();
		list.Add(counter.id);
		return list;
	}

	private string LocalizeDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		string empty = string.Empty;
		stringBuilder.Append((type switch
		{
			_E937.EQuestType.Completion => "QuestCondition/SurviveOnLocation", 
			_E937.EQuestType.Elimination => "QuestCondition/Elimination", 
			_E937.EQuestType.PickUp => "QuestCondition/PickUp", 
			_ => throw new ArgumentOutOfRangeException(), 
		}).Localized());
		foreach (Condition condition in counter.conditions)
		{
			if (_localizationTypes.TryGetValue(condition.GetType(), out var oldValue))
			{
				stringBuilder.Replace(oldValue, condition.FormattedDescription);
			}
		}
		foreach (string value in _localizationTypes.Values)
		{
			stringBuilder.Replace(value, string.Empty);
		}
		return stringBuilder.ToString();
	}

	public override string ToString()
	{
		return base.ToString() + " >> " + counter;
	}
}
