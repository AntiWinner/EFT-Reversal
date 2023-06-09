using System;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

[UsedImplicitly]
public sealed class QuickSlotItemView : ItemView, _E647, _E63F, _E640, _E641
{
	[SerializeField]
	private TextMeshProUGUI _resourceValue;

	private _EAED _E057;

	protected override bool IsInteractable => false;

	public static QuickSlotItemView Create(Item item, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		QuickSlotItemView quickSlotItemView = ItemViewFactory.CreateFromPool<QuickSlotItemView>(_ED3E._E000(236256))._E000(item, ItemRotation.Horizontal, inventoryController, itemUiContext);
		quickSlotItemView.Init();
		return quickSlotItemView;
	}

	private QuickSlotItemView _E000(Item item, ItemRotation rotation, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		_E057 = inventoryController;
		NewItemView(item, new _EB62(), rotation, _E057, null, _E057, itemUiContext);
		base.RectTransform.anchorMin = new Vector2(0f, 0f);
		base.RectTransform.anchorMax = new Vector2(1f, 1f);
		base.RectTransform.sizeDelta = new Vector2(0f, 0f);
		base.RectTransform.pivot = new Vector2(0.5f, 0.5f);
		string text;
		bool flag = _E001(item, out text);
		if (flag || base.Item is _EA40)
		{
			_E057.RegisterView(this);
		}
		_resourceValue.gameObject.SetActive(flag);
		return this;
	}

	protected override void UpdateScale()
	{
		if (MainImage.gameObject.activeSelf)
		{
			Quaternion quaternion;
			Vector3 localScale;
			if (base.IconScale.HasValue)
			{
				quaternion = Quaternion.Euler(0f, 0f, 45f);
				Vector3 vector = quaternion * MainImage.rectTransform.rect.size;
				float val = base.IconScale.Value.x / Mathf.Abs(vector.x);
				float val2 = base.IconScale.Value.y / Mathf.Abs(vector.y);
				localScale = Vector3.one * Math.Min(val, val2);
			}
			else
			{
				localScale = Vector3.one;
				quaternion = Quaternion.identity;
			}
			Transform obj = MainImage.transform;
			obj.localRotation = quaternion;
			obj.localScale = localScale;
		}
	}

	void _E647.OnRefreshItem(_EAFF args)
	{
		if (_E002(args.Item, args.Item.CurrentAddress))
		{
			_E003(args.RefreshIcon);
		}
	}

	void _E640.OnItemAdded(_EAF2 args)
	{
		if (args.Status == CommandStatus.Succeed && _E002(args.Item, args.To))
		{
			_E003();
		}
	}

	void _E641.OnItemRemoved(_EAF3 args)
	{
		if (_E057 != null && args.Status == CommandStatus.Succeed && _E002(args.Item, args.From))
		{
			_E003();
		}
	}

	public override void UpdateInfo()
	{
		if (_E001(base.Item, out var text))
		{
			_resourceValue.text = text;
		}
		MainImage.color = ((base.Item is Weapon weapon && _E057.HasKnownMalfunction(weapon)) ? Color.red : Color.white);
	}

	private static bool _E001(Item item, out string text)
	{
		if (item.TryGetItemComponent<SideEffectComponent>(out var component))
		{
			text = _ED3E._E000(236306) + component.Value.ToString(_ED3E._E000(164283)) + _ED3E._E000(30703) + component.MaxResource.ToString(_ED3E._E000(164283)) + _ED3E._E000(59467);
			return true;
		}
		if (item.TryGetItemComponent<ResourceComponent>(out var component2))
		{
			text = component2.Value.ToString(_ED3E._E000(164283)) + _ED3E._E000(30703) + component2.MaxResource.ToString(_ED3E._E000(164283));
			return true;
		}
		if (item.TryGetItemComponent<MedKitComponent>(out var component3))
		{
			text = component3.HpResource.ToString(_ED3E._E000(27314)) + _ED3E._E000(30703) + component3.MaxHpResource.ToString(_ED3E._E000(27314));
			return true;
		}
		if (item.TryGetItemComponent<FoodDrinkComponent>(out var component4) && component4.MaxResource > 1f)
		{
			text = component4.HpPercent.ToString(_ED3E._E000(27314)) + _ED3E._E000(30703) + component4.MaxResource.ToString(_ED3E._E000(27314));
			return true;
		}
		text = null;
		return false;
	}

	public override void Kill()
	{
		base.Kill();
		_E057?.UnregisterView(this);
		_E057 = null;
	}

	private bool _E002(Item item, ItemAddress address)
	{
		if (item != base.Item)
		{
			return address.IsChildOf(base.Item, notMergedWithThisItem: false);
		}
		return true;
	}

	private void _E003(bool refreshIcon = true)
	{
		SetQuestItemViewPanel();
		if (refreshIcon)
		{
			RefreshIcon();
		}
		UpdateInfo();
	}
}
