using System;
using System.Collections.Generic;
using Comfort.Common;
using EFT.HandBook;
using EFT.Hideout;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class ItemSelectionCell : UIElement
{
	[SerializeField]
	private IEnumerable<string> _selectingItems;

	[SerializeField]
	private EntityIcon _selectedItemIcon;

	[SerializeField]
	private GameObject _emptyIcon;

	[SerializeField]
	private Button _dropdownButton;

	[SerializeField]
	private Sprite _closeButtonIcon;

	[SerializeField]
	private Sprite _openButtonIcon;

	[SerializeField]
	private SelectingItemView _itemViewPrefab;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private RectTransform _mainTransform;

	[SerializeField]
	private bool _showEmptyCell = true;

	[SerializeField]
	private Vector2 _contextMenuOffset = new Vector2(-5f, -5f);

	protected Item CurrentItem;

	private Func<ItemSelectionCell, Item, bool, bool> _E115;

	private bool _E116;

	private bool _E117;

	private ItemUiContext _E118;

	protected virtual EContextPriorDirection Direction => EContextPriorDirection.LeftUpDown;

	private IEnumerable<Item> _E000
	{
		get
		{
			ResourceComponent component;
			return Singleton<_E815>.Instance.GetAvailableItemsByFilter<Item>(_selectingItems, (Item item) => !item.TryGetItemComponent<ResourceComponent>(out component) || component.Value.Positive());
		}
	}

	protected IEnumerable<string> SelectingItems
	{
		set
		{
			_selectingItems = value;
		}
	}

	protected Button DropdownButton => _dropdownButton;

	protected virtual void Awake()
	{
		_dropdownButton.onClick.AddListener(_E002);
		_E116 = false;
	}

	public void Show([CanBeNull] Item currentSelectedItem, Func<ItemSelectionCell, Item, bool, bool> itemSelectionChanged)
	{
		ShowGameObject();
		_E115 = itemSelectionChanged;
		_E118 = ItemUiContext.Instance;
		SetItem(currentSelectedItem);
	}

	public virtual void SetItem(Item item)
	{
		CurrentItem = item;
		_E001(item);
	}

	private void _E000(Item item)
	{
		if (item != CurrentItem && _E115 != null && _E115(this, CurrentItem, arg3: false))
		{
			SetItem(item);
			_E115(this, item, arg3: true);
		}
	}

	private void _E001([CanBeNull] Item item)
	{
		if (_selectedItemIcon.gameObject.activeSelf)
		{
			_selectedItemIcon.Close();
		}
		string text = item?.TemplateId;
		if (!string.IsNullOrEmpty(text))
		{
			_selectedItemIcon.Show(Singleton<_E63B>.Instance.GetPresetItem(text));
		}
		_emptyIcon.SetActive(item == null);
	}

	private async void _E002()
	{
		if (!_E117)
		{
			_E117 = true;
			_E003(!_E116);
			(Item, bool) obj = await _E118.SelectItem(this._E000, GetDetails, _showEmptyCell, _itemViewPrefab, _mainTransform, _contextMenuOffset, Direction);
			var (item, _) = obj;
			if (obj.Item2)
			{
				_E000(item);
			}
			_E003(!_E116);
			_E117 = false;
		}
	}

	protected virtual string GetDetails(Item item)
	{
		return string.Empty;
	}

	private void _E003(bool isOpen)
	{
		_E116 = isOpen;
		_dropdownButton.image.sprite = (_E116 ? _closeButtonIcon : _openButtonIcon);
	}

	public void Hide()
	{
		_E003(isOpen: false);
	}

	public override void Close()
	{
		_E118.CloseSelectItemMenu();
		_selectedItemIcon.Close();
		_E117 = false;
		base.Close();
	}
}
