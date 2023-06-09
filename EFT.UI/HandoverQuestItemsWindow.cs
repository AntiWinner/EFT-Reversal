using System;
using System.Linq;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class HandoverQuestItemsWindow : HandoverItemsWindow
{
	[SerializeField]
	private TextMeshProUGUI _count;

	private float _E00A;

	private Action<Item[]> _E001;

	private int _E000 => ItemsList.Sum((Item x) => x.StackObjectsCount);

	public void Show(float neededValue, double currentValue, Item[] items, Profile profile, _EAED controller, Action<Item[]> acceptAction, Action cancelAction)
	{
		_E00A = neededValue;
		CurrentValue = currentValue;
		_E001 = acceptAction;
		_count.text = string.Format(_ED3E._E000(252126), (double)_E00A - CurrentValue);
		Show(_ED3E._E000(252118).Localized() + _ED3E._E000(54246) + items[0].ShortName.Localized() + _ED3E._E000(27308), _ED3E._E000(252159).Localized(), profile, controller, new _EB66(EItemViewType.Quest, this), cancelAction);
		UpdateItems(items);
	}

	protected override bool IsSelected(Item item)
	{
		return ItemsList.Contains(item);
	}

	protected override bool IsActive(Item item, out string tooltip)
	{
		tooltip = string.Empty;
		return true;
	}

	public override void Accept()
	{
		_E001?.Invoke(base.ItemsToHandover);
		base.Accept();
	}

	protected override void TrySelectItemToHandover(Item item)
	{
		if (!item.IsNotEmpty())
		{
			int num = (int)((double)_E00A - CurrentValue);
			if (ItemsList.Contains(item))
			{
				ItemsList.Remove(item);
				DeselectView(item);
			}
			else if (_E000 < num)
			{
				ItemsList.Add(item);
				SelectView(item);
			}
			_count.text = string.Format(_ED3E._E000(252128), Mathf.Min(_E000, num), num);
			SetAcceptActive(ItemsList.Count > 0);
		}
	}

	protected override void ClearSelectedList()
	{
		foreach (Item items in ItemsList)
		{
			DeselectView(items);
		}
		base.ClearSelectedList();
	}
}
