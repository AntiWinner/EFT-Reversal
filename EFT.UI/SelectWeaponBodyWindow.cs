using System;
using EFT.InventoryLogic;

namespace EFT.UI;

public sealed class SelectWeaponBodyWindow : HandoverItemsWindow
{
	private Item _E00B;

	private Action<Weapon> _E001;

	public void Show(Item[] items, Item selectedItem, Profile profile, _EAED inventoryController, Action<Weapon> acceptAction, Action cancelAction)
	{
		Show(_ED3E._E000(248503).Localized(), string.Empty, profile, inventoryController, new _EB66(EItemViewType.WeaponModdingSimple, this), cancelAction);
		_E001 = acceptAction;
		UpdateItems(items);
		if (selectedItem != null)
		{
			TrySelectItemToHandover(selectedItem);
		}
	}

	protected override bool IsSelected(Item item)
	{
		return item == _E00B;
	}

	protected override bool IsActive(Item item, out string tooltip)
	{
		tooltip = string.Empty;
		return true;
	}

	public override void Accept()
	{
		_E001?.Invoke(_E00B as Weapon);
		base.Accept();
	}

	protected override void TrySelectItemToHandover(Item item)
	{
		if (!ItemsList.Contains(item))
		{
			_E000();
			_E00B = item;
			SelectView(item);
			ItemsList.Add(item);
		}
		SetAcceptActive(ItemsList.Count >= 0);
	}

	private void _E000()
	{
		if (_E00B != null)
		{
			DeselectView(_E00B);
			ItemsList.Remove(_E00B);
			_E00B = null;
		}
	}

	public override void Close()
	{
		_E000();
		base.Close();
	}
}
