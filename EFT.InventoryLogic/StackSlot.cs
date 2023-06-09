using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.InventoryLogic;

public sealed class StackSlot : IContainer
{
	public int MaxCount;

	private readonly List<Item> _items = new List<Item>();

	private readonly SortedList<int, Item> _deserializationList = new SortedList<int, Item>();

	public string ID { get; set; }

	public ItemFilter[] Filters { get; set; }

	public int Count
	{
		get
		{
			if (_items.Count == 0)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < _items.Count; i++)
			{
				num += _items[i].StackObjectsCount;
			}
			return num;
		}
	}

	public IEnumerable<Item> Items => _items;

	public Item ParentItem { get; }

	public EParentMergeType MergeContainerWithChildren => EParentMergeType.InheritFromItem;

	[CanBeNull]
	public Item Last
	{
		get
		{
			if (_items.Count <= 0)
			{
				return null;
			}
			return _items[_items.Count - 1];
		}
	}

	public Item FindItem(string itemId)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			Item item = _items[i];
			if (item.Id == itemId)
			{
				return item;
			}
			if (item is ContainerCollection containerCollection)
			{
				Item item2 = containerCollection.FindItem(itemId);
				if (item2 != null)
				{
					return item2;
				}
			}
		}
		return null;
	}

	public StackSlot()
	{
	}

	public StackSlot(StackSlot stackSlot, Item parentItem)
	{
		ID = stackSlot.ID;
		ParentItem = parentItem;
		Filters = stackSlot.Filters;
		MaxCount = stackSlot.MaxCount;
	}

	public void AddAtPosition(Item item, int position)
	{
		if (_deserializationList.ContainsKey(position))
		{
			throw new _E69D(string.Concat(_ED3E._E000(225812), item, _ED3E._E000(228611), this, _ED3E._E000(228661), position, _ED3E._E000(228651), _deserializationList[position], _ED3E._E000(228683)));
		}
		_deserializationList.Add(position, item);
	}

	public void FinalizeDeserialization()
	{
		int num = 0;
		foreach (KeyValuePair<int, Item> deserialization in _deserializationList)
		{
			if (deserialization.Key != num)
			{
				throw new _E69D(string.Concat(_ED3E._E000(228726), this, _ED3E._E000(228716), num));
			}
			num++;
			Add(deserialization.Value, simulate: false);
		}
		_deserializationList.Clear();
	}

	public bool AddOrMerge(Item item, _EB1E itemController)
	{
		if (_items.Count > 0)
		{
			Item target = _items[_items.Count - 1];
			if (_EB29.TransferMaxStackCount(item, target, MaxCount - Count, itemController, simulate: false).Succeeded)
			{
				return true;
			}
		}
		return Add(item, simulate: false).OrElse(0) > 0;
	}

	public _ECD9<int> GetMaxAddCount(Item item)
	{
		if (_items.Contains(item))
		{
			Debug.LogError(_ED3E._E000(228754));
			return new _EA0C(item, this);
		}
		if (!Filters.CheckItemFilter(item))
		{
			return new _EA0D(item, this);
		}
		return Mathf.Min(MaxCount - Count, item.StackObjectsCount);
	}

	public _ECD9<int> Add(Item item, bool simulate)
	{
		_ECD9<int> maxAddCount = GetMaxAddCount(item);
		if (maxAddCount.Failed)
		{
			return maxAddCount.Error;
		}
		if (maxAddCount.Value != item.StackObjectsCount)
		{
			return new _EA0E(item, this, MaxCount - Count);
		}
		if (!simulate)
		{
			if (item.CurrentAddress != null)
			{
				throw new _E69D(string.Concat(_ED3E._E000(225812), item, _ED3E._E000(151223), this, _ED3E._E000(225803), item.Parent));
			}
			_items.Add(item);
			item.CurrentAddress = new _EB21(this);
		}
		return item.StackObjectsCount;
	}

	public void Clear()
	{
		foreach (Item item in _items)
		{
			item.CurrentAddress = null;
		}
		_items.Clear();
	}

	public _ECD9<bool> Remove(Item item, string visitorId, bool simulate = false)
	{
		Item item2 = ((_items.Count > 0) ? _items[_items.Count - 1] : null);
		if (item != item2)
		{
			return new _EA0F(item, this);
		}
		if (!simulate)
		{
			_items.RemoveAt(_items.Count - 1);
			item.CurrentAddress = null;
		}
		return true;
	}

	public _ECD8<_EB2E> PopTo(_EB1E itemController, ItemAddress to)
	{
		if (Count <= 0)
		{
			return new _ECD2(_ED3E._E000(228774));
		}
		Item item = _items.Last();
		int stackObjectsCount = item.StackObjectsCount;
		if (stackObjectsCount == 1)
		{
			return _EB29.Move(item, to, itemController).Cast<_EB3B, _EB2E>();
		}
		if (stackObjectsCount > 1)
		{
			return _EB29.SplitExact(item, 1, to, itemController, itemController, simulate: false).Cast<_EB47, _EB2E>();
		}
		return new _ECD2(_ED3E._E000(228774));
	}

	public _ECD8<_EB2E> PopToNowhere(_EB1E itemController)
	{
		if (Count <= 0)
		{
			return new _ECD2(_ED3E._E000(228774));
		}
		Item item = _items.Last();
		int stackObjectsCount = item.StackObjectsCount;
		if (stackObjectsCount == 1)
		{
			return _EB29.Remove(item, itemController).Cast<_EB3A, _EB2E>();
		}
		if (stackObjectsCount > 1)
		{
			return _EB29.SplitToNowhere(item, 1, itemController, itemController).Cast<_EB46, _EB2E>();
		}
		return new _ECD2(_ED3E._E000(228774));
	}

	[CanBeNull]
	public Item Peek()
	{
		return _items.LastOrDefault();
	}

	public Item GetItemAtPosition(int index)
	{
		if (index >= Count)
		{
			throw new IndexOutOfRangeException();
		}
		int num = 0;
		for (int i = 0; i < _items.Count; i++)
		{
			num += _items[i].StackObjectsCount;
			if (index < num)
			{
				return _items[i];
			}
		}
		throw new _E69D(_ED3E._E000(228821) + index);
	}

	public int GetItemPosition(Item testedItem)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (testedItem.Equals(_items[i]))
			{
				return i;
			}
		}
		return -1;
	}

	public override string ToString()
	{
		return _ED3E._E000(228856) + ID + _ED3E._E000(63757) + ParentItem;
	}

	public int GetHashSum()
	{
		int num = 17;
		num = num * 23 + ParentItem.Id.GetHashCode();
		num = num * 23 + ID.GetHashCode();
		foreach (Item item in _items)
		{
			num = num * 23 + item.GetHashSum();
		}
		return num;
	}
}
