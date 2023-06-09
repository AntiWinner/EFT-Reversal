using UnityEngine;

namespace EFT.UI.DragAndDrop;

public class TemplatedGridsView : ContainedGridsView
{
	[SerializeField]
	private GridView[] _presetGridViews;

	public override void Show(_EA40 compoundItem, _EB68 itemContext, _EAED inventoryController, FilterPanel filterPanel, ItemUiContext itemUiContext, bool magnify = false)
	{
		Show(compoundItem, itemContext, _presetGridViews, inventoryController, filterPanel, itemUiContext, magnify);
	}
}
