using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.HandBook;
using EFT.UI.Ragfair;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class RagfairCategoriesPanel : BrowseCategoriesPanel
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string buffer;

		public RagfairCategoriesPanel _003C_003E4__this;

		internal void _E000()
		{
			_003C_003E4__this.SearchInputField.text = buffer;
			_003C_003E4__this.SearchInputField.Select();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public RagfairCategoriesPanel _003C_003E4__this;

		public EViewListType viewListType;

		public EWindowType windowType;

		public string forbiddenItem;

		public Action<NodeBaseView, string> onCreation;

		public _EBAC originalNodes;

		public _ECBD ragfair;

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
			if (!node.Data.Id.Contains(_ED3E._E000(197311)))
			{
				view.Show(_003C_003E4__this.Ragfair, _003C_003E4__this.CombinedCategoryView.CategoryView, _003C_003E4__this.CombinedCategoryView.SubcategoryView, node, viewListType, windowType, _003C_003E4__this.ViewNodes, forbiddenItem, _003C_003E4__this.OnSelection);
				_003C_003E4__this.ViewNodes.Add(node.Data.Id, view.SelectedView);
				onCreation?.Invoke((node.Data.Type == ENodeType.Item) ? view.SubcategoryView : view.CategoryView, node.Data.Id);
			}
		}

		internal void _E003()
		{
			originalNodes.ItemRemoved -= _003C_003E4__this._E002;
			originalNodes.ItemAdded -= _003C_003E4__this._E003;
			originalNodes.AllItemsRemoved -= _003C_003E4__this._E004;
		}

		internal void _E004()
		{
			_003C_003E4__this.ViewList.OnRemove -= _003C_003E4__this._E005;
			ragfair.CancellableFilters.ItemAdded -= _003C_003E4__this._E000;
			ragfair.CancellableFilters.ItemRemoved -= _003C_003E4__this._E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _EBAB value;

		public _E001 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			value.OnCountUpdated -= CS_0024_003C_003E8__locals1._003C_003E4__this._E001;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _EBAB node;

		internal bool _E000(KeyValuePair<string, _EBAB> x)
		{
			if (x.Key == node.Data.Id)
			{
				return x.Value != node;
			}
			return false;
		}
	}

	private Action<string> _E1CD;

	private string _E1CE;

	protected override string FilterString => base.Ragfair.FilterRule.HandbookId;

	protected override bool DisplayLoadingStatus => true;

	protected override void Awake()
	{
		base.Awake();
		SearchInputField.gameObject.AddComponent<ClickTrigger>().Init(delegate(PointerEventData eventData)
		{
			string buffer = GUIUtility.systemCopyBuffer;
			if (!string.IsNullOrEmpty(buffer) && buffer.StartsWith(_ED3E._E000(60677)))
			{
				if (base.ContextMenu.gameObject.activeSelf)
				{
					base.ContextMenu.Close();
				}
				else if (eventData.button == PointerEventData.InputButton.Right)
				{
					base.ContextMenu.Show(eventData.position, new _ECBE(base.ContextMenu.Close, null, null, delegate
					{
						SearchInputField.text = buffer;
						SearchInputField.Select();
					}));
				}
			}
		});
	}

	public async Task Show(string forbiddenItem, _ECBD ragfair, _EBA8 handbook, [CanBeNull] _EBAC originalNodes, _EBAC nodes, _EBAC filteredNodes, SimpleContextMenu contextMenu, EViewListType viewListType, EWindowType windowType, Action<NodeBaseView, string> onSelection, Action<NodeBaseView, string> onCreation = null, Action<string> onFindById = null)
	{
		_E1CD = onFindById;
		Show(ragfair, handbook, nodes, filteredNodes, contextMenu, viewListType, windowType, onSelection);
		ViewList = new _EC6E<_EBAB, CombinedView>();
		UI.AddDisposable(ViewList);
		await ViewList.Init(base.FilteredNodes.AsBindableList(), _EC6A.Instance, (_EBAB arg) => CombinedCategoryView, (_EBAB arg) => CategoryViewsContainer, delegate(_EBAB node, CombinedView view)
		{
			if (!node.Data.Id.Contains(_ED3E._E000(197311)))
			{
				view.Show(base.Ragfair, CombinedCategoryView.CategoryView, CombinedCategoryView.SubcategoryView, node, viewListType, windowType, ViewNodes, forbiddenItem, OnSelection);
				ViewNodes.Add(node.Data.Id, view.SelectedView);
				onCreation?.Invoke((node.Data.Type == ENodeType.Item) ? view.SubcategoryView : view.CategoryView, node.Data.Id);
			}
		});
		if (originalNodes != null)
		{
			originalNodes.ItemRemoved += _E002;
			originalNodes.ItemAdded += _E003;
			originalNodes.AllItemsRemoved += _E004;
			UI.AddDisposable(delegate
			{
				originalNodes.ItemRemoved -= _E002;
				originalNodes.ItemAdded -= _E003;
				originalNodes.AllItemsRemoved -= _E004;
			});
			foreach (KeyValuePair<string, _EBAB> originalNode in originalNodes)
			{
				var (_, value) = originalNode;
				value.OnCountUpdated += _E001;
				UI.AddDisposable(delegate
				{
					value.OnCountUpdated -= _E001;
				});
			}
		}
		ViewList.OnRemove += _E005;
		ragfair.CancellableFilters.ItemAdded += _E000;
		ragfair.CancellableFilters.ItemRemoved += _E000;
		UI.AddDisposable(delegate
		{
			ViewList.OnRemove -= _E005;
			ragfair.CancellableFilters.ItemAdded -= _E000;
			ragfair.CancellableFilters.ItemRemoved -= _E000;
		});
		SearchInputField.onEndEdit.AddListener(_E006);
	}

	public void ClearIdFilter()
	{
		_E1CE = string.Empty;
		SearchInputField.text = string.Empty;
	}

	private void _E000(_ECC0 cancellableFilter)
	{
		Filter(SearchInputField.text).HandleExceptions();
	}

	private void _E001(_EBAB node)
	{
		_E39D.Deconstruct(base.FilteredNodes.FirstOrDefault((KeyValuePair<string, _EBAB> x) => x.Key == node.Data.Id && x.Value != node), out var _, out var value);
		value?.SetChildrenCount(node.Count);
	}

	private void _E002(_EBAB node)
	{
		if (base.FilteredNodes.ContainsKey(node.Data.Id))
		{
			base.FilteredNodes.Remove(node.Data.Id);
		}
	}

	private void _E003(_EBAB node)
	{
		if (!base.FilteredNodes.ContainsKey(node.Data.Id))
		{
			base.FilteredNodes.Add(node.Data.Id, node);
		}
	}

	private void _E004()
	{
		base.FilteredNodes.Clear();
	}

	public void SetInputFieldText(string text)
	{
		SearchInputField.text = text;
	}

	private void _E005(_EBAB node)
	{
		ViewNodes.Remove(node.Data.Id);
		if (ViewNodes.Count > 0)
		{
			NodeBaseView value = ViewNodes.First().Value;
			OnSelection(value, value.Node.Data.Id);
		}
		else
		{
			OnSelection(null, string.Empty);
		}
	}

	private async void _E006(string arg)
	{
		if (arg.StartsWith(_ED3E._E000(60677)))
		{
			_E1CE = arg.Remove(0, 1);
			await Task.Yield();
			if (!string.IsNullOrEmpty(_E1CE))
			{
				_E1CD?.Invoke(_E1CE);
				_E1CE = string.Empty;
			}
		}
	}

	protected override bool Allowed(_EBAB node)
	{
		if (node.Parent == null)
		{
			return true;
		}
		return !node.Parent.Data.Id.Contains(_ED3E._E000(197311));
	}

	public override void Close()
	{
		SearchInputField.onEndEdit.RemoveListener(_E006);
		_E1CE = string.Empty;
		SearchInputField.text = string.Empty;
		ViewNodes.Clear();
		base.Close();
	}

	[CompilerGenerated]
	private void _E007(PointerEventData eventData)
	{
		string buffer = GUIUtility.systemCopyBuffer;
		if (string.IsNullOrEmpty(buffer) || !buffer.StartsWith(_ED3E._E000(60677)))
		{
			return;
		}
		if (base.ContextMenu.gameObject.activeSelf)
		{
			base.ContextMenu.Close();
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			base.ContextMenu.Show(eventData.position, new _ECBE(base.ContextMenu.Close, null, null, delegate
			{
				SearchInputField.text = buffer;
				SearchInputField.Select();
			}));
		}
	}
}
