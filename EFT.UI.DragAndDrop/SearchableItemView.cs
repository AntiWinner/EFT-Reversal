using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public sealed class SearchableItemView : MonoBehaviour
{
	[SerializeField]
	private SearchableView _searchableView;

	[SerializeField]
	private ContainedGridsView _containedGridsTemplate;

	[SerializeField]
	private Transform _gridsContainer;

	[SerializeField]
	private GridViewMagnifier _viewMagnifier;

	private ContainedGridsView m__E000;

	private _EB68 _E001;

	private _EA40 _E002;

	private _EAED _E003;

	private FilterPanel _E004;

	private ItemUiContext _E005;

	private bool _E006;

	private void Awake()
	{
		_E006 = _viewMagnifier != null;
	}

	public void Show(_EA40 compoundItem, _EB68 itemContext, _EAED inventoryController, [CanBeNull] FilterPanel filterPanel, ItemUiContext itemUiContext)
	{
		_E001 = itemContext;
		_E002 = compoundItem;
		_E003 = inventoryController;
		_E004 = filterPanel;
		_E005 = itemUiContext;
		this.m__E000 = ContainedGridsView.CreateGrids(_E002, _containedGridsTemplate);
		if (!(this.m__E000 == null))
		{
			this.m__E000.transform.SetParent(_gridsContainer, worldPositionStays: false);
			if (_E002 is _EA91 item)
			{
				_searchableView.Configure(_E003, searchButtonDisplay: true);
				_searchableView.Show(item, _E000);
			}
			ShowContents();
			if (_viewMagnifier != null && this.m__E000.GridViews != null && this.m__E000.GridViews.Length != 0)
			{
				_viewMagnifier.InitGridView(this.m__E000.GridViews[0]);
			}
		}
	}

	public void ShowContents()
	{
		if (_E002 is _EA91)
		{
			_searchableView.ShowContents();
		}
		else
		{
			_E000();
		}
	}

	private void _E000()
	{
		if (!(this.m__E000 == null) && !this.m__E000.gameObject.activeSelf)
		{
			this.m__E000.Show(_E002, _E001, _E003, _E004, _E005, _E006);
		}
	}

	public void HideContents()
	{
		if (this.m__E000 != null)
		{
			this.m__E000.Hide();
		}
	}

	public void Hide()
	{
		if (_E002 is _EA91 && _E003 != null)
		{
			_searchableView.Close();
		}
		HideContents();
	}

	public void Close()
	{
		Hide();
		if (this.m__E000 != null)
		{
			Object.DestroyImmediate(this.m__E000.gameObject);
			this.m__E000 = null;
		}
		_E002 = null;
		_E003 = null;
		_E004 = null;
		_E005 = null;
		_E001 = null;
	}
}
