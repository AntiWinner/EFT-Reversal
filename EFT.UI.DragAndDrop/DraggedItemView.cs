using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public sealed class DraggedItemView : MonoBehaviour
{
	[SerializeField]
	private Image _mainImage;

	[CompilerGenerated]
	private Item m__E000;

	[CompilerGenerated]
	private ItemAddress m__E001;

	private _E3E2 m__E002;

	private Action m__E003;

	private RectTransform _E004;

	private ItemUiContext _E005;

	private _EC9E _E006;

	private _EB68 _E007;

	private LocationInGrid _E008;

	[CompilerGenerated]
	private _EB69 _E009;

	public Item Item
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public ItemAddress ItemAddress
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

	public _EB69 ItemContext
	{
		[CompilerGenerated]
		get
		{
			return _E009;
		}
		[CompilerGenerated]
		private set
		{
			_E009 = value;
		}
	}

	private RectTransform _E00A
	{
		get
		{
			if (_E004 == null)
			{
				_E004 = (RectTransform)base.transform;
			}
			return _E004;
		}
	}

	public static DraggedItemView Create(_EB68 originalItemContext, ItemRotation itemRotation, Color imageColor, ItemUiContext itemUiContext)
	{
		return ItemViewFactory.CreateFromPrefab<DraggedItemView>(_ED3E._E000(235676))._E000(originalItemContext, itemRotation, imageColor, itemUiContext);
	}

	private DraggedItemView _E000(_EB68 originalItemContext, ItemRotation itemRotation, Color imageColor, ItemUiContext itemUiContext)
	{
		_E005 = itemUiContext;
		_E00A.SetParent(_E005.DragLayer);
		_E00A.ResetTransform();
		Item = originalItemContext.Item;
		ItemAddress = Item.Parent;
		this.m__E002 = ItemViewFactory.LoadItemIcon(Item);
		this.m__E003 = this.m__E002.Changed.Bind(_E001);
		ItemContext = new _EB69(originalItemContext, itemRotation);
		Vector2 vector = new Vector2(0.5f, 0.5f);
		_E00A.anchorMin = vector;
		_E00A.anchorMax = vector;
		_E00A.pivot = vector;
		_E002(itemRotation);
		_E005.RegisterView(ItemContext);
		_mainImage.color = imageColor;
		return this;
	}

	public void UpdateTargetUnderCursor(_EC9E containerUnderCursor, _EB68 itemUnderCursor)
	{
		LocationInGrid locationInGrid = ((containerUnderCursor is GridView gridView) ? gridView.CalculateItemLocation(ItemContext) : null);
		if (containerUnderCursor != _E006 || itemUnderCursor != _E007 || !(locationInGrid == _E008))
		{
			if ((UnityEngine.Object)_E006 != null && containerUnderCursor != _E006)
			{
				_E006.DisableHighlight();
			}
			_E006 = containerUnderCursor;
			_E007 = itemUnderCursor;
			_E008 = locationInGrid;
			containerUnderCursor?.HighlightItemViewPosition(ItemContext, itemUnderCursor, preview: false);
			(containerUnderCursor as _EC9F)?.FitGridForItem(Item);
		}
	}

	public void Kill()
	{
		UpdateTargetUnderCursor(null, null);
		this.m__E003();
		_E005.UnregisterView(ItemContext);
	}

	private void _E001()
	{
		_mainImage.gameObject.SetActive(this.m__E002.Sprite != null);
		_mainImage.sprite = this.m__E002.Sprite;
		_mainImage.SetNativeSize();
	}

	private void _E002(ItemRotation rotation)
	{
		ItemContext.ItemRotation = rotation;
		Quaternion rotation2 = ((rotation == ItemRotation.Horizontal) ? ItemViewFactory.HorizontalRotation : ItemViewFactory.VerticalRotation);
		_mainImage.transform.rotation = rotation2;
		_E00A.sizeDelta = ItemViewFactory.GetCellPixelSize(Item.CalculateRotatedSize(rotation));
		_E003();
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!(_E00A == null))
		{
			_E003();
			RectTransform rectTransform = base.transform.RectTransform();
			Vector2 size = rectTransform.rect.size;
			Vector2 pivot = rectTransform.pivot;
			Vector2 vector = size * pivot * rectTransform.lossyScale;
			Vector2 vector2 = base.transform.position;
			ItemContext.SetPosition(vector2, vector2 - vector);
		}
	}

	private void _E003()
	{
		_E00A.position = Input.mousePosition;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			_E002((ItemContext.ItemRotation == ItemRotation.Horizontal) ? ItemRotation.Vertical : ItemRotation.Horizontal);
			if ((UnityEngine.Object)_E006 != null)
			{
				_E006.HighlightItemViewPosition(ItemContext, _E007, preview: false);
			}
		}
		if ((Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.RightControl)) && (UnityEngine.Object)_E006 != null)
		{
			_E006.HighlightItemViewPosition(ItemContext, _E007, preview: false);
		}
	}

	private void OnDestroy()
	{
		ItemContext?.Dispose();
		ItemContext = null;
	}
}
