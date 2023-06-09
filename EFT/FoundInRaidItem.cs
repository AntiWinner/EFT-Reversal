using System;

namespace EFT;

[Serializable]
public class FoundInRaidItem
{
	public string QuestId;

	public string ItemId;

	public FoundInRaidItem(string questId, string itemId)
	{
		QuestId = questId;
		ItemId = itemId;
	}

	public bool Acceptable(string questId, string itemId)
	{
		if (QuestId == questId)
		{
			return ItemId == itemId;
		}
		return false;
	}
}
