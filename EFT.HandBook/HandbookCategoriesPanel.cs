using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.UI;
using EFT.UI.Ragfair;
using UnityEngine;

namespace EFT.HandBook;

public sealed class HandbookCategoriesPanel : BrowseCategoriesPanel
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public HandbookCategoriesPanel _003C_003E4__this;

		public List<string> questItems;

		public Func<_EBAB, bool> _003C_003E9__3;

		public Action<NodeBaseView, string> _003C_003E9__4;

		internal CombinedView _E000(_EBAB arg)
		{
			return _003C_003E4__this.CombinedCategoryView;
		}

		internal Transform _E001(_EBAB arg)
		{
			return _003C_003E4__this.CategoryViewsContainer;
		}

		internal void _E002(_EBAB item, CombinedView view)
		{
			if (item.Data.Type == ENodeType.Category)
			{
				if (item.Data.Id == _ED3E._E000(197311))
				{
					item.SetChildrenCount(item.Children.Count((_EBAB x) => questItems.Contains(x.Data.Item.TemplateId)));
				}
				view.Show(_003C_003E4__this.Ragfair, _003C_003E4__this.CombinedCategoryView.CategoryView, _003C_003E4__this.CombinedCategoryView.SubcategoryView, item, EViewListType.Handbook, EWindowType.Handbook, _003C_003E4__this.ViewNodes, string.Empty, delegate(NodeBaseView nodeView, string id)
				{
					_003C_003E4__this._entitiesPanel.ShowEntity(nodeView.Node);
					_003C_003E4__this.OnSelection(nodeView, id);
				});
				_003C_003E4__this.ViewNodes.Add(item.Data.Id, view.SelectedView);
			}
			else
			{
				_003C_003E4__this._entitiesPanel.AddToFilteredList(item);
			}
		}

		internal bool _E003(_EBAB x)
		{
			return questItems.Contains(x.Data.Item.TemplateId);
		}

		internal void _E004(NodeBaseView nodeView, string id)
		{
			_003C_003E4__this._entitiesPanel.ShowEntity(nodeView.Node);
			_003C_003E4__this.OnSelection(nodeView, id);
		}
	}

	[SerializeField]
	private EntitiesPanel _entitiesPanel;

	protected override void Awake()
	{
		SearchInputField.onValueChanged.AddListener(async delegate(string arg)
		{
			await Filter(arg);
		});
	}

	public async Task Show(_ECBD ragfair, _EBA8 handbook, _EBAC nodes, _EBAC filteredNodes, ItemUiContext itemUiContext, List<string> questItems, Action<NodeBaseView, string> onSelection)
	{
		Show(ragfair, handbook, nodes, filteredNodes, itemUiContext.ContextMenu, EViewListType.Handbook, EWindowType.Handbook, onSelection);
		_entitiesPanel.Show(base.Handbook, itemUiContext, questItems);
		ViewList = new _EC6E<_EBAB, CombinedView>();
		UI.AddDisposable(ViewList);
		await ViewList.Init(base.FilteredNodes.AsBindableList(), _EC6A.Instance, (_EBAB arg) => CombinedCategoryView, (_EBAB arg) => CategoryViewsContainer, delegate(_EBAB item, CombinedView view)
		{
			if (item.Data.Type == ENodeType.Category)
			{
				if (item.Data.Id == _ED3E._E000(197311))
				{
					item.SetChildrenCount(item.Children.Count((_EBAB x) => questItems.Contains(x.Data.Item.TemplateId)));
				}
				view.Show(base.Ragfair, CombinedCategoryView.CategoryView, CombinedCategoryView.SubcategoryView, item, EViewListType.Handbook, EWindowType.Handbook, ViewNodes, string.Empty, delegate(NodeBaseView nodeView, string id)
				{
					_entitiesPanel.ShowEntity(nodeView.Node);
					OnSelection(nodeView, id);
				});
				ViewNodes.Add(item.Data.Id, view.SelectedView);
			}
			else
			{
				_entitiesPanel.AddToFilteredList(item);
			}
		});
	}

	protected override void FilterHandler()
	{
		_entitiesPanel.ClearEntity();
		base.FilterHandler();
	}

	public override void Close()
	{
		_entitiesPanel.Close();
		base.Close();
	}

	[CompilerGenerated]
	private async void _E000(string arg)
	{
		await Filter(arg);
	}
}
