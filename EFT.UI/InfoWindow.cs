using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class InfoWindow : Window<_EC7C>, IPointerClickHandler, IEventSystemHandler, _E640, _E63F, _E641, _E647
{
	private const float m__E000 = 40f;

	private const float m__E001 = 12f;

	[SerializeField]
	private ItemSpecificationPanel _itemSpecificationPanel;

	[SerializeField]
	private TextMeshProUGUI _tagName;

	[SerializeField]
	private Image _tagColor;

	private Item m__E002;

	private _EB68 m__E003;

	private _EAE6 m__E004;

	private IItemOwner m__E005;

	private _E8B2 m__E006;

	private Action _E007;

	public void Show(_EB68 itemContext, _EAE6 itemController, _E8B2 trader, Action onSelectedAction, ItemUiContext itemUiContext, Action onClosedAction, _EC4E<EItemInfoButton> contextInteractions)
	{
		Show(onClosedAction);
		ShowGameObject();
		_E007 = onSelectedAction;
		this.m__E003 = itemContext.CreateChild(itemContext.Item);
		UI.AddDisposable(this.m__E003);
		this.m__E002 = this.m__E003.Item;
		this.m__E004 = itemController;
		this.m__E005 = this.m__E002.Owner;
		this.m__E005?.RegisterView(this);
		this.m__E006 = trader;
		if (this.m__E006 != null)
		{
			UI.AddDisposable(this.m__E006.AssortmentChanged.Bind(_E000));
		}
		this.m__E004.OnChamberCheck += _E003;
		this.m__E004.ExamineEvent += _E002;
		itemUiContext.InitSpecificationPanel(_itemSpecificationPanel, this.m__E003, contextInteractions);
		_E004();
		CorrectPosition();
		this.m__E003.OnCloseWindow += Close;
		this.WaitOneFrame(delegate
		{
			CorrectPosition();
		});
	}

	private void _E000()
	{
		if (this.m__E006.CurrentAssortment != null)
		{
			this.m__E006.CurrentAssortment.ItemPrepared += _E001;
		}
	}

	private void _E001(Item item)
	{
		if (item == this.m__E002)
		{
			Close();
		}
	}

	public void OnItemAdded(_EAF2 eventArgs)
	{
		_EB20 obj = eventArgs.To as _EB20;
		if (eventArgs.Status == CommandStatus.Succeed && obj != null)
		{
			if (eventArgs.Item == this.m__E002)
			{
				Close();
			}
			_itemSpecificationPanel.OnItemAddedEvent(eventArgs);
		}
	}

	public void OnItemRemoved(_EAF3 eventArgs)
	{
		if (eventArgs.Item == this.m__E002)
		{
			Close();
		}
		if (eventArgs.Status == CommandStatus.Succeed)
		{
			_itemSpecificationPanel.OnRemoveFromSlotEvent(eventArgs);
		}
	}

	public void OnRefreshItem(_EAFF obj)
	{
		_E004();
	}

	private void _E002(_EAF6 eventArgs)
	{
		_itemSpecificationPanel.OnItemExaminedEvent(eventArgs);
	}

	private void _E003(Weapon weapon)
	{
		if (this.m__E002 == weapon)
		{
			_itemSpecificationPanel.OnRefreshItemEvent(weapon);
		}
	}

	private void _E004()
	{
		if (!(_tagColor == null) && !(_tagName == null))
		{
			TagComponent itemComponent = this.m__E002.GetItemComponent<TagComponent>();
			if (itemComponent != null && !string.IsNullOrEmpty(itemComponent.Name))
			{
				_tagColor.gameObject.SetActive(value: true);
				_tagColor.color = EditTagWindow.GetColor(itemComponent.Color);
				_tagName.text = itemComponent.Name;
				StartCoroutine(_E005());
			}
			else
			{
				_tagColor.gameObject.SetActive(value: false);
			}
		}
	}

	private IEnumerator _E005()
	{
		yield return null;
		float x = Mathf.Max(_tagName.renderedWidth + 12f, 40f);
		RectTransform rectTransform = _tagColor.rectTransform;
		rectTransform.sizeDelta = new Vector2(x, rectTransform.sizeDelta.y);
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E007?.Invoke();
	}

	public override void Close()
	{
		this.m__E003.OnCloseWindow -= Close;
		if (this.m__E004 != null)
		{
			this.m__E004.OnChamberCheck -= _E003;
			this.m__E004.ExamineEvent -= _E002;
		}
		if (this.m__E006?.CurrentAssortment != null)
		{
			this.m__E006.CurrentAssortment.ItemPrepared -= _E001;
		}
		this.m__E005?.UnregisterView(this);
		_itemSpecificationPanel.Close();
		_EC45.SetCursor(ECursorType.Idle);
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInspectorWindowClose);
		base.Close();
		this.m__E003 = null;
		this.m__E004 = null;
		this.m__E005 = null;
		_E007 = null;
		this.m__E002 = null;
	}

	[CompilerGenerated]
	private void _E006()
	{
		CorrectPosition();
	}
}
