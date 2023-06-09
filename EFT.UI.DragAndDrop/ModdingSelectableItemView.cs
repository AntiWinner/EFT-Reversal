using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.DragAndDrop;

public sealed class ModdingSelectableItemView : SelectableItemView
{
	[SerializeField]
	private GameObject _missingInInventory;

	[SerializeField]
	private GameObject _infoIcons;

	private _EB65 _E055;

	[CompilerGenerated]
	private bool _E056;

	private bool _E000
	{
		[CompilerGenerated]
		get
		{
			return _E056;
		}
		[CompilerGenerated]
		set
		{
			_E056 = value;
		}
	}

	public static ModdingSelectableItemView Create(Item item, _EB65 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance, bool canSelect)
	{
		return ItemViewFactory.CreateFromPool<ModdingSelectableItemView>(_ED3E._E000(236093))._E000(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance, canSelect);
	}

	private ModdingSelectableItemView _E000(Item item, _EB65 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance, bool canSelect)
	{
		this._E000 = canSelect;
		NewSelectableItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		Init();
		_E055 = (_EB65)base.ItemContext;
		bool flag = _E055.ModdingSelectionContext.IsInventoryItem(item);
		_missingInInventory.SetActive(!flag);
		_infoIcons.SetActive(flag);
		return this;
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		_E055.Highlight(selected: true);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		_E055.Highlight(selected: false);
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Left:
			if (this._E000)
			{
				base.SelectableContext.ToggleSelection();
			}
			else if (doubleClick)
			{
				base.NewContextInteractions.ExecuteInteraction(EItemInfoButton.Inspect);
			}
			break;
		case PointerEventData.InputButton.Right:
			ShowContextMenu(position);
			break;
		}
	}
}
