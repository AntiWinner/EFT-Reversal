using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class ItemOnGrid : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemOnGrid _003C_003E4__this;

		public _E3E2 itemIcon;

		public bool examined;

		internal void _E000()
		{
			_003C_003E4__this.Icon.gameObject.SetActive(itemIcon.Sprite != null);
			_003C_003E4__this.Icon.sprite = itemIcon.Sprite;
			_003C_003E4__this.Icon.SetNativeSize();
			_003C_003E4__this.Icon.color = (examined ? new Color(255f, 255f, 255f, 255f) : new Color(0f, 0f, 0f, 178f));
		}
	}

	[SerializeField]
	protected Image Icon;

	public void Show([CanBeNull] Item item, bool correctSize = true, bool examined = true)
	{
		ShowGameObject();
		_E313 cellPixelSize = ItemViewFactory.GetCellPixelSize(item?.CalculateCellSize() ?? new _E313(1, 1));
		LayoutElement component = GetComponent<LayoutElement>();
		if (component != null)
		{
			component.minWidth = cellPixelSize.X;
			component.minHeight = cellPixelSize.Y;
		}
		else
		{
			((RectTransform)base.transform).sizeDelta = cellPixelSize;
		}
		_E3E2 itemIcon = ((item != null) ? ItemViewFactory.LoadItemIcon(item) : null);
		if (itemIcon != null)
		{
			UI.BindEvent(itemIcon.Changed, delegate
			{
				Icon.gameObject.SetActive(itemIcon.Sprite != null);
				Icon.sprite = itemIcon.Sprite;
				Icon.SetNativeSize();
				Icon.color = (examined ? new Color(255f, 255f, 255f, 255f) : new Color(0f, 0f, 0f, 178f));
			});
		}
		else
		{
			Icon.gameObject.SetActive(value: false);
		}
	}
}
