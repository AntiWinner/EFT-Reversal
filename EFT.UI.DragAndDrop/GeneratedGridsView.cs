using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public sealed class GeneratedGridsView : ContainedGridsView
{
	[SerializeField]
	private GridView _gridViewTemplate;

	private GridView[] m__E000 = new GridView[0];

	public override void Show(_EA40 compoundItem, _EB68 itemContext, _EAED inventoryController, FilterPanel filterPanel, ItemUiContext itemUiContext, bool magnify = false)
	{
		this.m__E000 = Array.ConvertAll(compoundItem.Grids, (_E9EF grid) => UnityEngine.Object.Instantiate(_gridViewTemplate, base.transform, worldPositionStays: false));
		Show(compoundItem, itemContext, this.m__E000, inventoryController, filterPanel, itemUiContext, magnify);
	}

	public override void Hide()
	{
		GridView[] array = this.m__E000;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Hide();
		}
		this.m__E000 = new GridView[0];
		base.Hide();
	}

	[CompilerGenerated]
	private GridView _E000(_E9EF grid)
	{
		return UnityEngine.Object.Instantiate(_gridViewTemplate, base.transform, worldPositionStays: false);
	}
}
