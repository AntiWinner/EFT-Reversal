using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.WeaponModding;

public sealed class ModdingItem : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _name;

	[SerializeField]
	private GameObject _infoPanel;

	[SerializeField]
	private Image _itemIcon;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private GameObject _missingInEquipment;

	[SerializeField]
	private Transform _itemViewContainer;

	[SerializeField]
	private Sprite _unexaminedSprite;

	[SerializeField]
	private Sprite _defaultSprite;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[CompilerGenerated]
	private Action<Item> m__E000;

	[CompilerGenerated]
	private Item m__E001;

	private _E3E2 m__E002;

	private ItemView m__E003;

	private Color _E004 = Color.white;

	private InventoryError _E005;

	private _EB1E _E006;

	private ItemUiContext _E007;

	public Item Item
	{
		[CompilerGenerated]
		get
		{
			return this.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E001 = value;
		}
	}

	public event Action<Item> ItemSelected
	{
		[CompilerGenerated]
		add
		{
			Action<Item> action = this.m__E000;
			Action<Item> action2;
			do
			{
				action2 = action;
				Action<Item> value2 = (Action<Item>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Item> action = this.m__E000;
			Action<Item> action2;
			do
			{
				action2 = action;
				Action<Item> value2 = (Action<Item>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void SetItem(Item item, bool unexamined, [CanBeNull] InventoryError inventoryError, ItemUiContext itemUiContext, _EB1E itemController, bool isInEquipment = true)
	{
		Item = item;
		_E007 = itemUiContext;
		_E006 = itemController;
		bool flag = _E007 != null || _E006 != null;
		_name.text = (unexamined ? _ED3E._E000(91186) : item.ShortName.Localized());
		_background.gameObject.SetActive(!flag);
		_background.sprite = (unexamined ? _unexaminedSprite : _defaultSprite);
		_E000(!unexamined);
		_E005 = inventoryError;
		_E004 = ((_E005 is _EB29._E00B) ? Color.red : Color.white);
		_background.color = _E004;
		_name.color = ((_E005 is _EB29._E00B) ? Color.red : Color.white);
		if (flag)
		{
			_E002(item);
		}
		else
		{
			this.m__E002 = ItemViewFactory.LoadItemIcon(item);
			if (unexamined)
			{
				_itemIcon.color = Color.black;
			}
			_itemIcon.SetNativeSize();
		}
		if (_missingInEquipment != null)
		{
			_missingInEquipment.SetActive(!isInEquipment);
		}
	}

	public void SetEmptyItem()
	{
		_name.text = _ED3E._E000(252245).Localized();
		_infoPanel.SetActive(value: false);
		_background.gameObject.SetActive(value: true);
	}

	private void _E000(bool value)
	{
		if (!(_canvasGroup == null))
		{
			_canvasGroup.alpha = (value ? 1f : 0.8f);
			_canvasGroup.interactable = value;
			_canvasGroup.blocksRaycasts = value;
		}
	}

	private void _E001()
	{
		_itemIcon.sprite = this.m__E002.Sprite;
		_itemIcon.gameObject.SetActive(this.m__E002.Sprite != null);
		_itemIcon.SetNativeSize();
	}

	private void _E002(Item slotItem)
	{
		_E003();
		this.m__E003 = _E007.CreateItemView(slotItem, new _EB62(EItemViewType.WeaponModdingInteractable), ItemRotation.Horizontal, _E006, null, null, null, slotView: false, canSelect: false, searched: true);
		Transform obj = this.m__E003.transform;
		obj.localPosition = Vector3.zero;
		obj.rotation = Quaternion.identity;
		obj.localScale = Vector3.one;
		obj.SetParent(_itemViewContainer, worldPositionStays: false);
	}

	private void _E003()
	{
		if (!(this.m__E003 == null))
		{
			this.m__E003.Kill();
			this.m__E003 = null;
		}
	}

	public void Close()
	{
		_E003();
	}
}
