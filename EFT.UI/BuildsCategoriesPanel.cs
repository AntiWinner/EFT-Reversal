using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.UI.Ragfair;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

public sealed class BuildsCategoriesPanel : BrowseCategoriesPanel
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public BuildsCategoriesPanel _003C_003E4__this;

		public EViewListType viewListType;

		public EWindowType windowType;

		public Action<NodeBaseView, string> onConfirmedSelection;

		internal CombinedView _E000(_EBAB arg)
		{
			return _003C_003E4__this.CombinedCategoryView;
		}

		internal Transform _E001(_EBAB arg)
		{
			return _003C_003E4__this.CategoryViewsContainer;
		}

		internal void _E002(_EBAB node, CombinedView view)
		{
			view.Show(_003C_003E4__this.Ragfair, _003C_003E4__this.CombinedCategoryView.CategoryView, _003C_003E4__this.CombinedCategoryView.SubcategoryView, node, viewListType, windowType, _003C_003E4__this.ViewNodes, string.Empty, _003C_003E4__this.OnSelection, onConfirmedSelection);
			if (node.Data.Id == _ED3E._E000(197189) && node.Parent == null)
			{
				((CategoryView)view.CategoryView).OpenCategory();
			}
			_003C_003E4__this.ViewNodes.Add(node.Data.Id, view.SelectedView);
		}
	}

	public async Task Show(_ECBD ragfair, _EBA8 handbook, _EBAC nodes, _EBAC filteredNodes, [CanBeNull] SimpleContextMenu contextMenu, EViewListType viewListType, EWindowType windowType, Action<NodeBaseView, string> onSelection, Action<NodeBaseView, string> onConfirmedSelection)
	{
		Show(ragfair, handbook, nodes, filteredNodes, contextMenu, viewListType, windowType, onSelection);
		ViewList = new _EC6E<_EBAB, CombinedView>();
		UI.AddDisposable(ViewList);
		await ViewList.Init(base.FilteredNodes.AsBindableList(), _EC6A.Instance, (_EBAB arg) => CombinedCategoryView, (_EBAB arg) => CategoryViewsContainer, delegate(_EBAB node, CombinedView view)
		{
			view.Show(base.Ragfair, CombinedCategoryView.CategoryView, CombinedCategoryView.SubcategoryView, node, viewListType, windowType, ViewNodes, string.Empty, OnSelection, onConfirmedSelection);
			if (node.Data.Id == _ED3E._E000(197189) && node.Parent == null)
			{
				((CategoryView)view.CategoryView).OpenCategory();
			}
			ViewNodes.Add(node.Data.Id, view.SelectedView);
		});
	}
}
