using System.Linq;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public abstract class ContainedGridsView : MonoBehaviour
{
	public GridView[] GridViews;

	[CanBeNull]
	public static ContainedGridsView CreateGrids(Item item, ContainedGridsView containedGridsTemplate)
	{
		GridLayoutComponent itemComponent = item.GetItemComponent<GridLayoutComponent>();
		return Object.Instantiate((itemComponent != null) ? _E905.Pop<ContainedGridsView>(_ED3E._E000(236777) + itemComponent.Template.LayoutName) : containedGridsTemplate);
	}

	public abstract void Show(_EA40 compoundItem, _EB68 itemContext, _EAED inventoryController, FilterPanel filterPanel, ItemUiContext itemUiContext, bool magnify = false);

	protected void Show(_EA40 compoundItem, _EB68 itemContext, GridView[] gridViews, _EAED inventoryController, FilterPanel filterPanel, ItemUiContext itemUiContext, bool magnify = false)
	{
		base.gameObject.SetActive(value: true);
		GridViews = gridViews;
		_E9EF[] array = compoundItem.Containers.OfType<_E9EF>().ToArray();
		for (int i = 0; i < gridViews.Length && i < array.Length; i++)
		{
			gridViews[i].Show(array[i], itemContext, inventoryController, itemUiContext, filterPanel, magnify);
		}
	}

	public virtual void Hide()
	{
		GridView[] gridViews = GridViews;
		for (int i = 0; i < gridViews.Length; i++)
		{
			gridViews[i].Hide();
		}
		base.gameObject.SetActive(value: false);
	}
}
