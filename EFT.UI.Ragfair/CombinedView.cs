using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.HandBook;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI.Ragfair;

public sealed class CombinedView : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EBAB node;

		public CombinedView _003C_003E4__this;

		internal void _E000()
		{
			node.OnCountUpdated -= _003C_003E4__this._E000;
		}
	}

	[SerializeField]
	private CategoryView _categoryView;

	[SerializeField]
	private NodeBaseView _subcategoryView;

	private EViewListType _E32D;

	private _ECBD _E328;

	public NodeBaseView CategoryView => _categoryView;

	public NodeBaseView SubcategoryView => _subcategoryView;

	public NodeBaseView SelectedView
	{
		get
		{
			if (_categoryView.gameObject.activeSelf)
			{
				return CategoryView;
			}
			if (_subcategoryView.gameObject.activeSelf)
			{
				return SubcategoryView;
			}
			if (_categoryView.Node == null)
			{
				return SubcategoryView;
			}
			return CategoryView;
		}
	}

	public void Show([CanBeNull] _ECBD ragfair, NodeBaseView categoryView, NodeBaseView subcategoryView, _EBAB node, EViewListType viewListType, EWindowType windowType, Dictionary<string, NodeBaseView> viewNodes, string forbiddenItem, Action<NodeBaseView, string> onSelection, Action<NodeBaseView, string> onConfirmedSelection = null)
	{
		_E328 = ragfair;
		_E32D = viewListType;
		ShowGameObject();
		if (node.Data.Type == ENodeType.Category)
		{
			_categoryView.Show(_E328, categoryView, subcategoryView, node, _E32D, windowType, viewNodes, forbiddenItem, onSelection, onConfirmedSelection);
		}
		else
		{
			_subcategoryView.Show(_E328, categoryView, subcategoryView, node, _E32D, windowType, viewNodes, forbiddenItem, onSelection, onConfirmedSelection);
		}
		if (_E32D.IsUpdateChildStatus())
		{
			node.OnCountUpdated += _E000;
			UI.AddDisposable(delegate
			{
				node.OnCountUpdated -= _E000;
			});
		}
		_E000(node);
	}

	private void _E000(_EBAB node)
	{
		if (!(this == null))
		{
			if (_E328.FilterRule.HandbookId == node.Data?.Id)
			{
				base.gameObject.SetActive(value: true);
			}
			else
			{
				base.gameObject.SetActive(node.CanShowNodeView(_E32D));
			}
		}
	}

	public override void Close()
	{
		if (_categoryView.gameObject.activeSelf)
		{
			_categoryView.Close();
		}
		if (_subcategoryView.gameObject.activeSelf)
		{
			_subcategoryView.Close();
		}
		base.Close();
	}
}
