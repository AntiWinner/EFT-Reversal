using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public sealed class ItemFilter
{
	public string[] Filter;

	[CanBeNull]
	public string[] ExcludedFilter;

	public bool CheckItemFilter(Item item)
	{
		return CheckItem(item, Filter);
	}

	public bool CheckItemExcludedFilter(Item item)
	{
		if (ExcludedFilter != null && ExcludedFilter.Length != 0)
		{
			return !CheckItem(item, ExcludedFilter);
		}
		return true;
	}

	public static bool CheckItem(Item item, string[] acceptableNodes)
	{
		foreach (string text in acceptableNodes)
		{
			if (item.TemplateId == text)
			{
				return true;
			}
			if (_EA59.TypeTable.ContainsKey(text) && _EA59.TypeTable[text].IsInstanceOfType(item))
			{
				return true;
			}
		}
		return false;
	}
}
