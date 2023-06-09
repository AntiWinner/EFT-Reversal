using System.Collections.Generic;
using Newtonsoft.Json;

namespace EFT.Quests;

public sealed class ConditionQuest : ConditionOneTarget
{
	[JsonProperty("status")]
	public EQuestStatus[] statuses;

	public string name;

	public int availableAfter;

	protected override List<object> IdentityFields()
	{
		List<object> list = base.IdentityFields();
		EQuestStatus[] array = statuses;
		foreach (EQuestStatus eQuestStatus in array)
		{
			list.Add(eQuestStatus);
		}
		list.Add(availableAfter);
		return list;
	}
}
