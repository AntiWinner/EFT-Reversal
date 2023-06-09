using System;
using System.Linq;

namespace EFT;

[Serializable]
public class DroppedItem
{
	public string QuestId;

	public string ItemId;

	public string ZoneId;

	public DroppedItem()
	{
	}

	public DroppedItem(_E933 quest, string itemId, string zoneId)
	{
		QuestId = quest.Id;
		ItemId = itemId;
		ZoneId = zoneId;
	}

	public bool Equals(string[] itemIds, string zoneId)
	{
		if (itemIds.Contains(ItemId))
		{
			return ZoneId == zoneId;
		}
		return false;
	}
}
