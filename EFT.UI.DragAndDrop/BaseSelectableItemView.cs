using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.DragAndDrop;

public abstract class BaseSelectableItemView : StaticGridItemView
{
	[SerializeField]
	private GameObject _selectedMark;

	[SerializeField]
	private GameObject _selectedBackground;

	[CompilerGenerated]
	private _EB66 _E02D;

	protected _EB66 SelectableContext
	{
		[CompilerGenerated]
		get
		{
			return _E02D;
		}
		[CompilerGenerated]
		private set
		{
			_E02D = value;
		}
	}

	protected void NewBaseSelectableItemView(Item item, _EB66 sourceItemContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		NewGridItemView(item, sourceItemContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		SelectableContext = (_EB66)base.ItemContext;
		SelectableContext.OnSelected += SetItemSelected;
		CompositeDisposable.AddDisposable(delegate
		{
			SelectableContext.OnSelected += SetItemSelected;
		});
		SetAvailability(SelectableContext.IsActive(out var tooltip), tooltip);
		SetItemSelected(SelectableContext.IsSelected);
	}

	protected abstract void SetAvailability(bool available, string tooltip);

	protected virtual void SetItemSelected(bool selected)
	{
		_selectedMark.SetActive(selected);
		_selectedBackground.SetActive(selected);
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Left:
			SelectableContext.ToggleSelection();
			break;
		case PointerEventData.InputButton.Middle:
			if (!ExecuteMiddleClick())
			{
				base.NewContextInteractions.ExecuteInteraction(EItemInfoButton.CheckMagazine);
			}
			break;
		case PointerEventData.InputButton.Right:
			ShowContextMenu(position);
			break;
		}
	}

	public override void UpdateRemoveError(bool ignoreMalfunctions = true)
	{
	}

	public override void Kill()
	{
		base.Kill();
		SelectableContext = null;
	}

	[CompilerGenerated]
	private void _E000()
	{
		SelectableContext.OnSelected += SetItemSelected;
	}
}
