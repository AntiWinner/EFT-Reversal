using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public sealed class Slot : IContainer
{
	private struct _E000
	{
		public readonly Item Item;

		public readonly string ConflictingTemplateId;

		public _E000(Item item, string conflictingTemplateId)
		{
			Item = item;
			ConflictingTemplateId = conflictingTemplateId;
		}
	}

	public class _E001 : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public _E001(Item item, Slot slot)
		{
			Item = item;
			Slot = slot;
		}

		public override string ToString()
		{
			return string.Concat(Slot, _ED3E._E000(225833), string.Join(_ED3E._E000(10270), Slot.BlockerSlots.Select((Slot x) => string.Concat(x.ContainedItem, _ED3E._E000(63757), x)).ToArray()), _ED3E._E000(225881), Item);
		}

		public override string GetLocalizedDescription()
		{
			return string.Format(_ED3E._E000(225871).Localized(), string.Join(_ED3E._E000(10270), Slot.BlockerSlots.Select((Slot x) => x.ContainedItem?.Name.Localized()).ToArray()));
		}
	}

	public class _E002 : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public readonly Slot ConflictingSlot;

		public _E002(Item item, Slot slot, Slot conflictingSlot)
		{
			Item = item;
			Slot = slot;
			ConflictingSlot = conflictingSlot;
		}

		public override string ToString()
		{
			return string.Concat(Item, _ED3E._E000(225896), ConflictingSlot, _ED3E._E000(225891), ConflictingSlot.ContainedItem, _ED3E._E000(225977), Slot);
		}

		public override string GetLocalizedDescription()
		{
			return string.Format(_ED3E._E000(225962).Localized(), ConflictingSlot.ID.Localized());
		}
	}

	public class _E003 : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public readonly int MaxAllowedCount;

		public _E003(Item item, Slot slot, int maxAllowedCount)
		{
			Item = item;
			Slot = slot;
			MaxAllowedCount = maxAllowedCount;
		}

		public override string ToString()
		{
			return string.Concat(Item, _ED3E._E000(225988), Item.StackObjectsCount, _ED3E._E000(226024), Slot, _ED3E._E000(226023), MaxAllowedCount);
		}
	}

	public class _E004 : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public _E004(Item item, Slot slot)
		{
			Item = item;
			Slot = slot;
		}

		public override string ToString()
		{
			return string.Concat(Slot, _ED3E._E000(226069), Item, _ED3E._E000(226108));
		}

		public override string GetLocalizedDescription()
		{
			if (!(Slot.ParentItem is _EB0B))
			{
				return _ED3E._E000(227189).Localized();
			}
			return _ED3E._E000(226084).Localized();
		}
	}

	public class _E005 : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public _E005(Item item, Slot slot)
		{
			Item = item;
			Slot = slot;
		}

		public override string ToString()
		{
			return string.Concat(Slot, _ED3E._E000(226112), Item, _ED3E._E000(226108));
		}

		public override string GetLocalizedDescription()
		{
			if (!(Slot.ParentItem is _EB0B))
			{
				return _ED3E._E000(226153).Localized();
			}
			return _ED3E._E000(226236).Localized();
		}
	}

	public class _E006 : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public IEnumerable<Slot> MissingParts;

		public _E006(Item item, Slot slot, IEnumerable<Slot> missingParts)
		{
			Item = item;
			Slot = slot;
			MissingParts = missingParts;
		}

		public override string ToString()
		{
			return string.Concat(_ED3E._E000(226261), Item, _ED3E._E000(151223), Slot, _ED3E._E000(226249), string.Join(_ED3E._E000(2540), MissingParts.Select((Slot x) => x.ID).ToArray()));
		}

		public override string GetLocalizedDescription()
		{
			return string.Format(_ED3E._E000(226278).Localized(), string.Join(_ED3E._E000(10270), MissingParts.Select((Slot x) => x.ID.ToUpperInvariant().Localized()).ToArray()));
		}
	}

	public class _E007 : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public _E007(Item item, Slot slot)
		{
			Item = item;
			Slot = slot;
		}

		public override string ToString()
		{
			return string.Concat(_ED3E._E000(228358), Item, _ED3E._E000(151223), Slot, _ED3E._E000(228410), Slot.ContainedItem);
		}

		public override string GetLocalizedDescription()
		{
			return string.Format(_ED3E._E000(228384).Localized(), Slot.ID.Localized());
		}
	}

	public class _E008 : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public _E008(Item item, Slot slot)
		{
			Item = item;
			Slot = slot;
		}

		public override string ToString()
		{
			string text = string.Join(_ED3E._E000(2540), (from x in Slot.Filters.SelectMany((ItemFilter x) => x.Filter)
				select (x + _ED3E._E000(70087)).Localized()).ToArray());
			string text2 = string.Join(_ED3E._E000(2540), (from x in Slot.Filters.Where((ItemFilter x) => x.ExcludedFilter != null).SelectMany((ItemFilter x) => x.ExcludedFilter)
				select (x + _ED3E._E000(70087)).Localized()).ToArray());
			return string.Format(_ED3E._E000(226684), Item, Slot, text, text2);
		}

		public override string GetLocalizedDescription()
		{
			return _ED3E._E000(226740).Localized();
		}
	}

	public class _E009 : InventoryError
	{
		public readonly Item Item;

		public readonly Item RootItem;

		public readonly Item ConflictingItem;

		public _E009(Item item, Item rootItem, Item conflictingItem)
		{
			Item = item;
			RootItem = rootItem;
			ConflictingItem = conflictingItem;
		}

		public override string ToString()
		{
			return string.Concat(RootItem, _ED3E._E000(228416), Item, _ED3E._E000(220841), ConflictingItem, _ED3E._E000(228462));
		}

		public override string GetLocalizedDescription()
		{
			return string.Format(_ED3E._E000(228504).Localized(), ConflictingItem.ShortName.Localized(), Item.ShortName.Localized());
		}
	}

	public class _E00A : InventoryError
	{
		public readonly Item Item;

		public readonly Item RootItem;

		public _E00A(Item item, Item rootItem)
		{
			Item = item;
			RootItem = rootItem;
		}

		public override string ToString()
		{
			return string.Concat(RootItem, _ED3E._E000(228539), Item);
		}

		public override string GetLocalizedDescription()
		{
			return _ED3E._E000(228573).Localized();
		}
	}

	public class _E00B : InventoryError
	{
		public readonly Item Item;

		public readonly Slot Slot;

		public _E00B(Item item, Slot slot)
		{
			Item = item;
			Slot = slot;
		}

		public override string ToString()
		{
			return string.Concat(_ED3E._E000(228601), Item, _ED3E._E000(226958), Slot, _ED3E._E000(228584), Item.Parent);
		}
	}

	[CompilerGenerated]
	private sealed class _E00E
	{
		public Item x;

		internal _E000 _E000(string y)
		{
			return new _E000(x, y);
		}
	}

	[CompilerGenerated]
	private sealed class _E00F
	{
		public Item x;

		internal _E000 _E000(string y)
		{
			return new _E000(x, y);
		}
	}

	[CompilerGenerated]
	private sealed class _E010
	{
		public Item existingItem;

		internal bool _E000(_E000 x)
		{
			return x.ConflictingTemplateId == existingItem.TemplateId;
		}
	}

	[CompilerGenerated]
	private sealed class _E011
	{
		public Item newItem;

		internal bool _E000(_E000 x)
		{
			return x.ConflictingTemplateId == newItem.TemplateId;
		}
	}

	public readonly string Id;

	public readonly bool Required;

	private readonly _ECF5<Item> _reactiveContainedItem = new _ECF5<Item>();

	public Dictionary<string, Slot> ConflictingSlots;

	public readonly List<Slot> BlockerSlots = new List<Slot>();

	private string _cachedSlotName;

	public bool Deleted;

	[CanBeNull]
	public Item ContainedItem { get; private set; }

	[NotNull]
	public Item ParentItem { get; }

	public _ECF5<Item> ReactiveContainedItem
	{
		get
		{
			_reactiveContainedItem.Value = ContainedItem;
			return _reactiveContainedItem;
		}
	}

	public string ID => Id;

	public string FullId => ID + _ED3E._E000(18502) + ParentItem.TemplateId;

	public string Name
	{
		get
		{
			if (!string.IsNullOrEmpty(_cachedSlotName))
			{
				return _cachedSlotName;
			}
			foreach (EWeaponModType value in Enum.GetValues(typeof(EWeaponModType)))
			{
				string text = value.ToString();
				if (!ID.Contains(text))
				{
					continue;
				}
				_cachedSlotName = text;
				return text;
			}
			_cachedSlotName = ID;
			return ID;
		}
	}

	public IEnumerable<Item> Items
	{
		get
		{
			if (ContainedItem != null)
			{
				yield return ContainedItem;
			}
		}
	}

	public EParentMergeType MergeContainerWithChildren { get; }

	public ItemFilter[] Filters { get; set; }

	public Item FindItem(string itemId)
	{
		if (ContainedItem == null)
		{
			return null;
		}
		if (ContainedItem.Id == itemId)
		{
			return ContainedItem;
		}
		if (ContainedItem is ContainerCollection containerCollection)
		{
			return containerCollection.FindItem(itemId);
		}
		return null;
	}

	public Slot(Slot slot, _EA40 parentItem)
		: this(slot.ID, slot.Filters, slot.Required, slot.MergeContainerWithChildren)
	{
		ParentItem = parentItem;
	}

	public Slot(string id, ItemFilter[] filters, bool required, EParentMergeType mergeSlotWithChildren)
	{
		Id = id;
		ParentItem = null;
		Required = required;
		MergeContainerWithChildren = mergeSlotWithChildren;
		Filters = filters;
	}

	public void ApplyContainedItem()
	{
		_reactiveContainedItem.Value = ContainedItem;
	}

	public bool Examined(Item item)
	{
		if (ParentItem.CurrentAddress.GetOwnerOrNull() is _EAED obj)
		{
			return obj.Examined(item);
		}
		return true;
	}

	private IEnumerable<Slot> _E000(Item item)
	{
		if (ConflictingSlots == null)
		{
			yield break;
		}
		SlotBlockerComponent itemComponent = item.GetItemComponent<SlotBlockerComponent>();
		if (itemComponent != null)
		{
			string[] conflictingSlotNames = itemComponent.ConflictingSlotNames;
			foreach (string key in conflictingSlotNames)
			{
				yield return ConflictingSlots[key];
			}
		}
	}

	private _ECD9<bool> _E001(Item item)
	{
		if (!this.MergesWithChildren())
		{
			return true;
		}
		Item rootItem = ParentItem.GetRootItem();
		Item[] array = rootItem.GetAllMergedItems().ToArray();
		_E000[] source = array.SelectMany((Item x) => x.ConflictingItems.Select((string y) => new _E000(x, y))).ToArray();
		Item[] array2 = item.GetAllMergedItems().ToArray();
		_E000[] source2 = array2.SelectMany((Item x) => x.ConflictingItems.Select((string y) => new _E000(x, y))).ToArray();
		Item[] array3 = array;
		foreach (Item existingItem in array3)
		{
			_E000 obj = source2.FirstOrDefault((_E000 x) => x.ConflictingTemplateId == existingItem.TemplateId);
			if (obj.Item != null)
			{
				return new _E009(obj.Item, rootItem, existingItem);
			}
		}
		array3 = array2;
		foreach (Item newItem in array3)
		{
			_E000 obj2 = source.FirstOrDefault((_E000 x) => x.ConflictingTemplateId == newItem.TemplateId);
			if (obj2.Item != null)
			{
				return new _E009(newItem, rootItem, obj2.Item);
			}
		}
		return true;
	}

	private _ECD9<bool> _E002(Item item, bool ignoreRestrictions = false, bool ignoreMalfunction = false)
	{
		if (!Examined(item) && !(item is _EA12))
		{
			return new _E004(item, this);
		}
		if (ContainedItem != null)
		{
			return new _E007(item, this);
		}
		if (!ignoreRestrictions)
		{
			if (!this.CanAccept(item))
			{
				return new _E008(item, this);
			}
			if (BlockerSlots.Count > 0)
			{
				return new _E001(item, this);
			}
			_ECD9<bool> obj = _E001(item);
			if (obj.Failed)
			{
				return obj.Error;
			}
			if (ConflictingSlots != null)
			{
				foreach (Slot item2 in _E000(item))
				{
					if (item2.ContainedItem != null)
					{
						return new _E002(item, this, item2);
					}
				}
			}
			if (!ignoreMalfunction && ParentItem.GetRootItem() is Weapon weapon && weapon.IncompatibleByMalfunction(item))
			{
				return new _EB29._E000(item, weapon);
			}
			if (item is Weapon weapon2)
			{
				List<Slot> list = weapon2.MissingVitalParts.ToList();
				if (list.Any())
				{
					return new _E006(weapon2, this, list);
				}
			}
		}
		return true;
	}

	public void Add(Item item)
	{
		if (item.CurrentAddress != null)
		{
			throw new _E69D(string.Concat(_ED3E._E000(225812), item, _ED3E._E000(151223), this, _ED3E._E000(225803), item.Parent));
		}
		ContainedItem = item;
		item.CurrentAddress = new _EB20(this);
	}

	public _ECD9<int> Add(Item item, bool simulate, bool ignoreMalfunction = false)
	{
		_ECD9<bool> obj = _E002(item, ignoreRestrictions: false, ignoreMalfunction);
		if (obj.Failed)
		{
			return obj.Error;
		}
		if (item.StackObjectsCount != 1)
		{
			return new _E003(item, this, 1);
		}
		if (!item.ParentRecursiveCheck(ParentItem))
		{
			return new _E008(item, this);
		}
		if (!simulate)
		{
			Add(item);
			foreach (Slot item2 in _E000(item))
			{
				item2.BlockerSlots.Add(this);
			}
		}
		return 1;
	}

	public _ECD9<int> AddWithoutRestrictions(Item item)
	{
		_ECD9<bool> obj = _E002(item, ignoreRestrictions: true);
		if (obj.Failed)
		{
			return obj.Error;
		}
		if (item.StackObjectsCount != 1)
		{
			return new _E003(item, this, 1);
		}
		Add(item);
		foreach (Slot item2 in _E000(item))
		{
			item2.BlockerSlots.Add(this);
		}
		return 1;
	}

	public _ECD9<bool> RemoveItem(bool simulate = false)
	{
		Item containedItem = ContainedItem;
		if (containedItem == null)
		{
			return new _E00B(containedItem, this);
		}
		if (!(containedItem is _EA12) && !Examined(containedItem))
		{
			return new _E005(containedItem, this);
		}
		if (ParentItem.GetRootItem() is Weapon weapon && weapon.IncompatibleByMalfunction(containedItem))
		{
			return new _EB29._E000(containedItem, weapon);
		}
		if (!simulate)
		{
			foreach (Slot item in _E000(containedItem))
			{
				item.BlockerSlots.Remove(this);
			}
			ContainedItem = null;
			containedItem.CurrentAddress = null;
		}
		return true;
	}

	public bool CanReplace(Item item)
	{
		if (RemoveItem(simulate: true).OrElse(elseValue: false))
		{
			return this.CanAccept(item);
		}
		return false;
	}

	public override string ToString()
	{
		return _ED3E._E000(225843) + ID + _ED3E._E000(63757) + ParentItem;
	}

	[CanBeNull]
	public _EB20 FindLocationForItem(Item item, out _ECD1 error)
	{
		_ECD9<bool> obj = _E002(item);
		_EB20 result = (obj.OrElse(elseValue: false) ? new _EB20(this) : null);
		error = obj.Error;
		return result;
	}

	public int GetHashSum()
	{
		int num = 17;
		num = num * 23 + ParentItem.Id.GetHashCode();
		num = num * 23 + ID.GetHashCode();
		if (ContainedItem != null)
		{
			num = num * 23 + ContainedItem.GetHashSum();
		}
		return num;
	}
}
