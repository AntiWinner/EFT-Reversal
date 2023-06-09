using System.Linq;
using System.Runtime.CompilerServices;
using Diz.Binding;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.DragAndDrop;

public sealed class RagfairNewOfferItemView : GridItemView
{
	[SerializeField]
	private GameObject _selectedMark;

	[SerializeField]
	private GameObject _selectedBackground;

	private bool _E058;

	private bool _E059;

	private readonly _ECF5<bool> _E05A = new _ECF5<bool>();

	private readonly _ECF5<bool> _E05B = new _ECF5<bool>();

	private _EC69 _E05C;

	private bool _E000 => _E059;

	protected override IBindable<float> Transparency => _ECF3.Combine(base.Transparency, _E05A, _E05B, (float baseTransparent, bool canBeOffered, bool maxCountReached) => (canBeOffered && (!maxCountReached || _E058)) ? baseTransparent : 0.25f);

	private new bool _E001 => base.Item.IsEmpty();

	public static RagfairNewOfferItemView Create(Item item, _ECA2 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, [CanBeNull] FilterPanel filterPanel, [CanBeNull] _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance, bool canSelect)
	{
		RagfairNewOfferItemView ragfairNewOfferItemView = ItemViewFactory.CreateFromPool<RagfairNewOfferItemView>(_ED3E._E000(236290))._E000(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance, canSelect);
		ragfairNewOfferItemView.Init();
		return ragfairNewOfferItemView;
	}

	private RagfairNewOfferItemView _E000(Item item, _ECA2 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, [CanBeNull] FilterPanel filterPanel, [CanBeNull] _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance, bool canSelect)
	{
		_E059 = canSelect;
		_E058 = false;
		NewGridItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		_E05C = sourceContext.OfferContext;
		_E05A.Value = this._E001 && _E05C.HighlightedAtRagfair(item);
		_E05C.OnOfferTakesMaxCellsChanged += _E001;
		_E05C.OnItemSelectionChanged += _E002;
		_E001(_E05C.IsOfferTakesMaxCellsCount, _E05C.SelectedItem);
		_E003(_E05C.SelectedItems.Contains(base.Item));
		return this;
	}

	public override void OnItemAdded(_EAF2 eventArgs)
	{
		base.OnItemAdded(eventArgs);
		if (eventArgs.To.IsChildOf(base.Item, notMergedWithThisItem: false))
		{
			_E004();
		}
	}

	public override void OnItemRemoved(_EAF3 eventArgs)
	{
		base.OnItemRemoved(eventArgs);
		if (eventArgs.From.IsChildOf(base.Item, notMergedWithThisItem: false))
		{
			_E004();
		}
	}

	private void _E001(bool isMaxCellsCount, Item templateItem)
	{
		_E05B.Value = isMaxCellsCount && templateItem.Compare(base.Item);
	}

	private void _E002(Item item, bool selected)
	{
		if (base.Item == item)
		{
			_E003(selected);
		}
	}

	private void _E003(bool selected)
	{
		_E058 = selected;
		_selectedMark.SetActive(selected);
		_selectedBackground.SetActive(selected);
	}

	private void _E004()
	{
		bool flag = this._E001 && _E05C.HighlightedAtRagfair(base.Item);
		_E05A.Value = flag;
		if (!flag)
		{
			_E05C.DeselectItem(base.Item);
		}
		_E05C.InvokeItemChanged(base.Item);
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Left:
			if (doubleClick)
			{
				base.NewContextInteractions.ExecuteInteraction(EItemInfoButton.Inspect);
			}
			else if (this._E000)
			{
				_E05C.SelectItem(base.Item);
			}
			break;
		case PointerEventData.InputButton.Middle:
			ExecuteMiddleClick();
			break;
		case PointerEventData.InputButton.Right:
			ShowContextMenu(position);
			break;
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (!(base.DraggedItemView == null))
		{
			base.OnEndDrag(eventData);
		}
	}

	public override void Kill()
	{
		_E05C.OnOfferTakesMaxCellsChanged -= _E001;
		_E05C.OnItemSelectionChanged -= _E002;
		_E05C.DeselectItem(base.Item);
		_E05C = null;
		base.Kill();
	}

	[CompilerGenerated]
	private float _E005(float baseTransparent, bool canBeOffered, bool maxCountReached)
	{
		if (canBeOffered && (!maxCountReached || _E058))
		{
			return baseTransparent;
		}
		return 0.25f;
	}
}
