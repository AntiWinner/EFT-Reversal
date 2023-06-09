using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class CardSelectionPanel : UIElement
{
	[SerializeField]
	private Image _selectedItemIcon;

	[SerializeField]
	private Sprite _emptyIcon;

	[SerializeField]
	private GameObject _noCardTooltip;

	[SerializeField]
	private Button _dropdownButton;

	[SerializeField]
	private Sprite _closeButtonIcon;

	[SerializeField]
	private Sprite _openButtonIcon;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private KeyCardItem _cardItemPrefab;

	private Item _E16C;

	private Action<Item> _E16D;

	private bool _E116;

	private _E3E2 _E11A;

	private void Awake()
	{
		_dropdownButton.onClick.AddListener(_E004);
		_E116 = false;
	}

	public void Show(Item currentSelectedCard, Item[] availableCards, Action<Item> cardSelected)
	{
		_E16D = cardSelected;
		if (availableCards == null || availableCards.Length < 1)
		{
			_noCardTooltip.SetActive(value: true);
		}
		else
		{
			_noCardTooltip.SetActive(value: false);
			UI.AddDisposable(new _EC71<Item, KeyCardItem>(new _ECEF<Item>(availableCards), _cardItemPrefab, _container, delegate(Item item, KeyCardItem view)
			{
				view.SetItem(item, _E000);
			}));
		}
		_E16C = currentSelectedCard;
		_E001(currentSelectedCard);
	}

	private void _E000(Item item)
	{
		_E005(isOpen: false);
		if (item != _E16C)
		{
			_E16C = item;
			_E16D?.Invoke(item);
			_E001(item);
		}
	}

	private void _E001(Item item)
	{
		if (item != null)
		{
			_E11A = ItemViewFactory.LoadItemIcon(item);
			UI.AddDisposable(_E11A.Changed.Bind(_E002));
		}
		else
		{
			_selectedItemIcon.sprite = _emptyIcon;
		}
	}

	private void _E002()
	{
		_selectedItemIcon.sprite = _E11A.Sprite;
		_selectedItemIcon.gameObject.SetActive(_E11A.Sprite != null);
		_selectedItemIcon.SetNativeSize();
		_E003(((RectTransform)base.transform).rect.size);
	}

	private void _E003(Vector2 size)
	{
		Vector2 sizeDelta = ((RectTransform)_selectedItemIcon.transform).sizeDelta;
		float num = size.x / sizeDelta.x;
		float num2 = size.y / sizeDelta.y;
		if (!(num > 1f) || !(num2 > 1f))
		{
			float num3 = Mathf.Min(num, num2);
			_selectedItemIcon.rectTransform.localScale = new Vector3(num3, num3, 1f);
		}
	}

	private void _E004()
	{
		_E005(!_E116);
	}

	private void _E005(bool isOpen)
	{
		_E116 = isOpen;
		_container.gameObject.SetActive(_E116);
		_dropdownButton.image.sprite = (_E116 ? _closeButtonIcon : _openButtonIcon);
	}

	public void Hide()
	{
		_E005(isOpen: false);
	}

	[CompilerGenerated]
	private void _E006(Item item, KeyCardItem view)
	{
		view.SetItem(item, _E000);
	}
}
