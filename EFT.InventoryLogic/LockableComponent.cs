using System.Linq;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public class LockableComponent : _EB19
{
	public class _E000 : InventoryError
	{
		public readonly LockableComponent Lockable;

		public readonly KeyComponent Key;

		public _E000(LockableComponent lockable, KeyComponent key)
		{
			Lockable = lockable;
			Key = key;
		}

		public override string ToString()
		{
			return string.Concat(_ED3E._E000(215634), Lockable.Item, _ED3E._E000(215616), Key.Item, _ED3E._E000(215657), string.Join(_ED3E._E000(2540), Lockable.KeyIds.Select((string x) => (x + _ED3E._E000(70087)).Localized()).ToArray()));
		}

		public override string GetLocalizedDescription()
		{
			return _ED3E._E000(215701).Localized();
		}
	}

	[_E63C]
	public bool Locked;

	private readonly _E9E2 m__E000;

	public string[] KeyIds => this.m__E000.KeyIds;

	public LockableComponent(Item item, _E9E2 template)
		: base(item)
	{
		this.m__E000 = template;
	}

	public _ECD7 Apply(Item item, _EB1E itemController, _E9EF[] grids, bool simulate)
	{
		KeyComponent itemComponent = item.GetItemComponent<KeyComponent>();
		if (itemComponent != null)
		{
			return ApplyKey(itemComponent, itemController as _EAED);
		}
		_ECD7 result = default(_ECD7);
		foreach (_E9EF obj in grids)
		{
			if (!obj.CanAccept(item))
			{
				continue;
			}
			_EB22 obj2 = obj.FindLocationForItem(item);
			if (obj2 != null)
			{
				result = _EB29.Move(item, obj2, itemController, simulate);
				if (result.Succeeded)
				{
					return result;
				}
			}
		}
		return result.Error ?? new _EA02(item, Item);
	}

	public _ECD8<_EB4B> ApplyKey(KeyComponent key, [CanBeNull] _EAED inventoryController)
	{
		if (inventoryController != null)
		{
			if (!inventoryController.Examined(key.Item))
			{
				return new _E9FC(key.Item);
			}
			if (!inventoryController.Examined(Item))
			{
				return new _E9FD(Item);
			}
		}
		if (!KeyIds.Contains(key.Template.KeyId))
		{
			return new _E000(this, key);
		}
		Locked = !Locked;
		return new _EB4B(key, this, Locked, inventoryController);
	}
}
