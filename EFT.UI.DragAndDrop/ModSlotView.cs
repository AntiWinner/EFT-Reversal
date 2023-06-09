using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public sealed class ModSlotView : SlotView, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E54D itemTemplates;

		public IEnumerable<Type> excludeTypes;

		internal Type _E000(string node)
		{
			if (!_EA59.TypeTable.ContainsKey(node))
			{
				if (!itemTemplates.ContainsKey(node))
				{
					return null;
				}
				return _EA59.TypeTable[itemTemplates[node]._parent];
			}
			return _EA59.TypeTable[node];
		}

		internal Type _E001(string node)
		{
			if (!_EA59.TypeTable.ContainsKey(node))
			{
				if (!itemTemplates.ContainsKey(node))
				{
					return null;
				}
				return _EA59.TypeTable[itemTemplates[node]._parent];
			}
			return _EA59.TypeTable[node];
		}

		internal bool _E002(Type t)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001
			{
				t = t
			};
			return excludeTypes.All((Type excludedType) => excludedType != CS_0024_003C_003E8__locals0.t);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Type t;

		internal bool _E000(Type excludedType)
		{
			return excludedType != t;
		}
	}

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private TextMeshProUGUI _slotName;

	[SerializeField]
	private RectTransform _filtersGroup;

	[SerializeField]
	private Sprite _uncheckedChamberSprite;

	private string m__E000;

	private bool m__E001;

	public void SetLocked(KeyValuePair<EModLockedState, string> lockedState, Item item)
	{
		this.m__E000 = lockedState.Value;
		SetActiveCamora(item, lockedState.Key);
		switch (lockedState.Key)
		{
		case EModLockedState.Unlocked:
			_E000(value: false);
			_E001(locked: false);
			break;
		case EModLockedState.TraderLock:
		case EModLockedState.RaidLock:
			_E001(locked: true);
			break;
		case EModLockedState.ChamberUnchecked:
			_E000(value: true);
			_E001(locked: true);
			SlotBackImage.gameObject.SetActive(value: true);
			_slotName.gameObject.SetActive(value: true);
			if (_E00E != null)
			{
				_E00E.gameObject.SetActive(value: false);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(236888), lockedState, null);
		}
	}

	public void SetActiveCamora(Item item, EModLockedState lockedState)
	{
		Weapon weapon = item as Weapon;
		if (!(weapon is _EAD1))
		{
			ActiveCamoraImage.gameObject.SetActive(value: false);
			return;
		}
		bool active = weapon.GetCurrentMagazine() is _EB13 obj && base.Slot == obj.GetActiveCamoraSlot(_E7A3.InRaid);
		ActiveCamoraImage.gameObject.SetActive(active);
		ActiveCamoraImage.transform.SetAsLastSibling();
	}

	private void _E000(bool value)
	{
		if (!(_uncheckedChamberSprite == null))
		{
			if (value)
			{
				SlotBackImage.sprite = _uncheckedChamberSprite;
			}
			else if (CachedSprite != null)
			{
				SlotBackImage.sprite = CachedSprite;
			}
			SlotBackImage.SetNativeSize();
		}
	}

	private void _E001(bool locked)
	{
		this.m__E001 = locked;
		_canvasGroup.alpha = (this.m__E001 ? 0.3f : 1f);
		_canvasGroup.blocksRaycasts = !this.m__E001;
		_canvasGroup.interactable = !this.m__E001;
	}

	protected override ItemView CreateItemViewKernel(Item item, IItemOwner itemOwner)
	{
		return ItemUiContext.CreateItemView(item, base.ParentItemContext, ItemRotation.Horizontal, InventoryController, itemOwner, null, null, slotView: true, canSelect: false, searched: true);
	}

	public void Show(Slot slot, _EB68 parentItemContext, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		Sprite sprite = ((parentItemContext.Item is _EA62) ? _E905.Pop<Sprite>(_ED3E._E000(236875)) : _E905.Pop<Sprite>(_ED3E._E000(236876) + slot.Name));
		if (sprite == null)
		{
			Debug.LogError(_ED3E._E000(236906) + slot.ID + _ED3E._E000(236896) + slot.Name);
		}
		else
		{
			SetSlotBackImage(sprite);
		}
		bool flag = slot.ContainedItem == null;
		_slotName.text = slot.Name.Localized().ToUpper();
		_slotName.gameObject.SetActive(flag);
		if (flag)
		{
			_E002(slot);
		}
		Show(slot, parentItemContext, inventoryController, itemUiContext, null, null);
	}

	protected override void SetupItemView(Item item)
	{
		ScaleItem(((RectTransform)base.transform).sizeDelta);
		SetSlotGraphics(fullSlot: true, selected: false);
		FullBorder.transform.SetAsLastSibling();
	}

	private void _E002(Slot slot)
	{
		string[] source = slot.Filters.SelectMany((ItemFilter x) => x.Filter).ToArray();
		_E54D itemTemplates = Singleton<_E63B>.Instance.ItemTemplates;
		IEnumerable<Type> source2 = source.Select((string node) => (!_EA59.TypeTable.ContainsKey(node)) ? ((!itemTemplates.ContainsKey(node)) ? null : _EA59.TypeTable[itemTemplates[node]._parent]) : _EA59.TypeTable[node]);
		string[] source3 = slot.Filters.Where((ItemFilter x) => x.ExcludedFilter != null).SelectMany((ItemFilter x) => x.ExcludedFilter).ToArray();
		IEnumerable<Type> excludeTypes = source3.Select((string node) => (!_EA59.TypeTable.ContainsKey(node)) ? ((!itemTemplates.ContainsKey(node)) ? null : _EA59.TypeTable[itemTemplates[node]._parent]) : _EA59.TypeTable[node]);
		foreach (Sprite item in (from sprite in source2.Where((Type t) => excludeTypes.All((Type excludedType) => excludedType != t)).Select(ItemViewFactory.LoadModIconSprite)
			where sprite != null
			select sprite).Distinct())
		{
			Image image = UnityEngine.Object.Instantiate(_E905.Pop<Image>(_ED3E._E000(257818)), _filtersGroup, worldPositionStays: false);
			image.sprite = item;
			image.SetNativeSize();
		}
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (this.m__E001 && this.m__E000 != string.Empty)
		{
			ItemUiContext.Tooltip.Show(this.m__E000);
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		ItemUiContext.Tooltip.Close();
	}
}
