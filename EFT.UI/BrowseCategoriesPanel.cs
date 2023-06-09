using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EFT.UI.Ragfair;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public class BrowseCategoriesPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public string id;

		public Func<_EBAB, bool> _003C_003E9__3;

		internal bool _E000(NodeBaseView nodeView)
		{
			return nodeView.Node.Children.Flatten((_EBAB y) => y.Children).Any((_EBAB childNode) => childNode.Data.Id == id);
		}

		internal bool _E001(_EBAB childNode)
		{
			return childNode.Data.Id == id;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public BrowseCategoriesPanel _003C_003E4__this;

		public bool searchCanceled;

		public string value;

		public Func<_EBAB, bool> _003C_003E9__4;

		internal void _E000()
		{
			// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
			_003C_003E4__this._E000 -= _E000;
			searchCanceled = true;
			_003C_003E4__this._E1CC = false;
			_003C_003E4__this._E001(status: false);
		}

		internal bool _E001(_EBAB x)
		{
			if (_003C_003E4__this.Allowed(x))
			{
				return x.Data.Name.Localized().IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
			}
			return false;
		}

		internal _EBAB _E002(_EBAB x)
		{
			return x.CreateDummy(_003C_003E4__this._E002 == EViewListType.Handbook);
		}

		internal bool _E003(_EBAB node)
		{
			return !_003C_003E4__this.FilteredNodes.ContainsKey(node.Data.Id);
		}
	}

	protected const string QUEST_CATEGORY_ID = "5b619f1a86f77450a702a6f3";

	private const int _E1BE = 3;

	private const float _E1BF = 1f;

	private const int _E1C0 = 10;

	[SerializeField]
	protected TMP_InputField SearchInputField;

	[SerializeField]
	protected CombinedView CombinedCategoryView;

	[SerializeField]
	protected RectTransform CategoryViewsContainer;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private GameObject _searchIcon;

	[CompilerGenerated]
	private Action _E1C1;

	[CompilerGenerated]
	private Action _E1C2;

	[CompilerGenerated]
	private _EBAC _E1C3;

	[CompilerGenerated]
	private SimpleContextMenu _E1C4;

	[CompilerGenerated]
	private _ECBD _E1C5;

	[CompilerGenerated]
	private _EBA8 _E1C6;

	[CompilerGenerated]
	private _EBAC _E1C7;

	[CompilerGenerated]
	private EViewListType _E1C8;

	[CompilerGenerated]
	private EWindowType _E1C9;

	protected readonly Dictionary<string, NodeBaseView> ViewNodes = new Dictionary<string, NodeBaseView>();

	protected _EC6E<_EBAB, CombinedView> ViewList;

	protected Action<NodeBaseView, string> OnSelection;

	private bool _E1CA;

	private string _E1CB;

	private bool _E1CC;

	protected virtual string FilterString => string.Empty;

	protected _EBAC FilteredNodes
	{
		[CompilerGenerated]
		get
		{
			return _E1C3;
		}
		[CompilerGenerated]
		private set
		{
			_E1C3 = value;
		}
	}

	protected SimpleContextMenu ContextMenu
	{
		[CompilerGenerated]
		get
		{
			return _E1C4;
		}
		[CompilerGenerated]
		private set
		{
			_E1C4 = value;
		}
	}

	protected _ECBD Ragfair
	{
		[CompilerGenerated]
		get
		{
			return _E1C5;
		}
		[CompilerGenerated]
		private set
		{
			_E1C5 = value;
		}
	}

	protected _EBA8 Handbook
	{
		[CompilerGenerated]
		get
		{
			return _E1C6;
		}
		[CompilerGenerated]
		private set
		{
			_E1C6 = value;
		}
	}

	protected virtual bool DisplayLoadingStatus => false;

	private _EBAC _E001
	{
		[CompilerGenerated]
		get
		{
			return _E1C7;
		}
		[CompilerGenerated]
		set
		{
			_E1C7 = value;
		}
	}

	private EViewListType _E002
	{
		[CompilerGenerated]
		get
		{
			return _E1C8;
		}
		[CompilerGenerated]
		set
		{
			_E1C8 = value;
		}
	}

	public EWindowType WindowType
	{
		[CompilerGenerated]
		get
		{
			return _E1C9;
		}
		[CompilerGenerated]
		set
		{
			_E1C9 = value;
		}
	}

	private event Action _E000
	{
		[CompilerGenerated]
		add
		{
			Action action = _E1C1;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E1C1, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E1C1;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E1C1, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnFiltered
	{
		[CompilerGenerated]
		add
		{
			Action action = _E1C2;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E1C2, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E1C2;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E1C2, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected virtual void Awake()
	{
		SearchInputField.onValueChanged.AddListener(async delegate(string arg)
		{
			await Filter(arg);
		});
	}

	protected void Show(_ECBD ragfair, _EBA8 handbook, _EBAC nodes, _EBAC filteredNodes, [CanBeNull] SimpleContextMenu contextMenu, EViewListType viewListType, EWindowType windowType, Action<NodeBaseView, string> onSelection)
	{
		Ragfair = ragfair;
		Handbook = handbook;
		this._E001 = nodes;
		FilteredNodes = filteredNodes;
		ContextMenu = contextMenu;
		this._E002 = viewListType;
		WindowType = windowType;
		OnSelection = onSelection;
		if (_loader != null)
		{
			_loader.SetActive(value: false);
		}
		ShowGameObject();
	}

	[CanBeNull]
	public NodeBaseView SelectNode(string id)
	{
		if (!ViewNodes.TryGetValue(id, out var value))
		{
			return _E000(id, ViewNodes.Values);
		}
		if (!value.gameObject.activeSelf)
		{
			return _E000(id, ViewNodes.Values);
		}
		value.SelectView();
		return value;
	}

	[CanBeNull]
	private NodeBaseView _E000(string id, [CanBeNull] IEnumerable<NodeBaseView> selection)
	{
		if (selection == null)
		{
			return null;
		}
		CategoryView categoryView = selection.Where((NodeBaseView nodeView) => nodeView.Node != null).FirstOrDefault((NodeBaseView nodeView) => nodeView.Node.Children.Flatten((_EBAB y) => y.Children).Any((_EBAB childNode) => childNode.Data.Id == id)) as CategoryView;
		if (categoryView == null)
		{
			return null;
		}
		IEnumerable<NodeBaseView> selection2 = categoryView.OpenCategory();
		if (!ViewNodes.TryGetValue(id, out var value))
		{
			return _E000(id, selection2);
		}
		if (!value.gameObject.activeSelf)
		{
			return _E000(id, selection2);
		}
		value.SelectView();
		return value;
	}

	protected async Task Filter(string value)
	{
		_E001 CS_0024_003C_003E8__locals0 = new _E001();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.value = value;
		CS_0024_003C_003E8__locals0.searchCanceled = false;
		_E1C1?.Invoke();
		this._E000 += delegate
		{
			// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
			CS_0024_003C_003E8__locals0._003C_003E4__this._E000 -= CS_0024_003C_003E8__locals0._E000;
			CS_0024_003C_003E8__locals0.searchCanceled = true;
			CS_0024_003C_003E8__locals0._003C_003E4__this._E1CC = false;
			CS_0024_003C_003E8__locals0._003C_003E4__this._E001(status: false);
		};
		if (CS_0024_003C_003E8__locals0.value.StartsWith(_ED3E._E000(60677)))
		{
			return;
		}
		if (CS_0024_003C_003E8__locals0.value.Length < 3)
		{
			if (!string.IsNullOrEmpty(_E1CB) && !_E1CA)
			{
				FilterHandler();
				FilteredNodes.AddRange(this._E001);
				SelectNode(FilterString);
				ViewList.OrderViewList();
				_E1CA = true;
			}
			return;
		}
		_E001(status: true);
		await TasksExtensions.Delay(1f);
		if (_E1CC | CS_0024_003C_003E8__locals0.searchCanceled)
		{
			return;
		}
		_E1CC = true;
		_E1CB = CS_0024_003C_003E8__locals0.value;
		IEnumerable<_EBAB> source = from x in this._E001.Values.Flatten((_EBAB x) => x.Children)
			where CS_0024_003C_003E8__locals0._003C_003E4__this.Allowed(x) && x.Data.Name.Localized().IndexOf(CS_0024_003C_003E8__locals0.value, StringComparison.OrdinalIgnoreCase) >= 0
			select x.CreateDummy(CS_0024_003C_003E8__locals0._003C_003E4__this._E002 == EViewListType.Handbook);
		FilterHandler();
		int num = 0;
		foreach (_EBAB item in source.Where((_EBAB node) => !CS_0024_003C_003E8__locals0._003C_003E4__this.FilteredNodes.ContainsKey(node.Data.Id)))
		{
			FilteredNodes.Add(item.Data.Id, item);
			int num2 = num + 1;
			num = num2;
			if (num > 10)
			{
				num = 0;
				await Task.Yield();
				if (CS_0024_003C_003E8__locals0.searchCanceled)
				{
					return;
				}
			}
		}
		_E1CA = false;
		_E001(status: false);
		_E1CC = false;
	}

	private void _E001(bool status)
	{
		if (DisplayLoadingStatus)
		{
			_loader.SetActive(status);
			_searchIcon.SetActive(!status);
		}
	}

	protected virtual void FilterHandler()
	{
		FilteredNodes.Clear();
		ViewNodes.Clear();
		_E1C2?.Invoke();
	}

	protected virtual bool Allowed(_EBAB node)
	{
		return true;
	}

	public override void Close()
	{
		_E1C1?.Invoke();
		SearchInputField.text = string.Empty;
		FilteredNodes?.Clear();
		ViewNodes.Clear();
		base.Close();
	}

	[CompilerGenerated]
	private async void _E002(string arg)
	{
		await Filter(arg);
	}
}
