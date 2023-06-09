using System;
using System.Collections.Generic;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class ResourceSelectionCell : ItemSelectionCell
{
	[SerializeField]
	private TextMeshProUGUI _details;

	[SerializeField]
	private Color _defaultDetailsColor;

	private int _E112;

	private ResourceComponent _E000 => CurrentItem.GetItemComponent<ResourceComponent>();

	public void Show(Item currentSelectedItem, IEnumerable<string> filters, Func<ItemSelectionCell, Item, bool, bool> itemSelectionChanged)
	{
		base.SelectingItems = filters;
		Show(currentSelectedItem, itemSelectionChanged);
	}

	public override void SetItem(Item item)
	{
		base.SetItem(item);
		_E000();
	}

	private void _E000()
	{
		ResourceComponent resourceComponent = this._E000;
		_details.text = _E001(resourceComponent);
		_E112 = Mathf.CeilToInt(resourceComponent?.Value ?? 0f);
	}

	protected override string GetDetails(Item item)
	{
		ResourceComponent itemComponent = item.GetItemComponent<ResourceComponent>();
		if (itemComponent == null)
		{
			return string.Empty;
		}
		return _E001(itemComponent);
	}

	private string _E001(ResourceComponent resourceHolder)
	{
		int num = 0;
		float num2 = 100f;
		if (resourceHolder != null)
		{
			num = (int)resourceHolder.Value;
			num2 = resourceHolder.MaxResource;
		}
		return string.Format(_ED3E._E000(252186), _defaultDetailsColor.GetRichTextColor(), num, num2);
	}

	private void Update()
	{
		if (_E112 > 0 && this._E000 != null && Mathf.CeilToInt(this._E000.Value) != _E112)
		{
			_E000();
		}
	}
}
