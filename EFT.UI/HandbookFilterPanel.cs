using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.HandBook;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class HandbookFilterPanel : FilterPanel
{
	private sealed class _E000 : _EC62
	{
		private readonly int m__E000;

		private readonly HandbookFilterPanel _E001;

		public _E000(HandbookFilterPanel panel, int nodeIndex)
		{
			_E001 = panel;
			m__E000 = nodeIndex;
		}

		public void Show()
		{
			_E001._E000(m__E000);
		}

		public Task<bool> TryHide()
		{
			return Task.FromResult(result: true);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public HandbookSpecialCategory special;

		internal bool _E000(HandbookData cat)
		{
			return cat.Id == special.PlaceAfter;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _EBAB node;

		public HierarchyFilterTab tab;

		public HandbookFilterPanel _003C_003E4__this;

		public HoverTrigger hover;

		internal bool _E000(HandbookSpecialCategory cat)
		{
			return cat.CategoryId == node.Data.Id;
		}

		internal void _E001(Sprite iconSprite)
		{
			tab.SetIcon(iconSprite);
		}

		internal void _E002(PointerEventData arg)
		{
			_003C_003E4__this._tooltip.Show(tab.name.Localized(), null, 0.25f, null, tab.Interactable);
			_003C_003E4__this._E001(hover, tab);
		}

		internal void _E003(PointerEventData arg)
		{
			_003C_003E4__this._tooltip.Close();
			_003C_003E4__this._E002(hover);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public KeyValuePair<FilterTab, _EBAB> tabToLoad;

		internal void _E000(Sprite iconSprite)
		{
			tabToLoad.Key.SetIcon(iconSprite);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _EBAB parentNode;

		public Predicate<_EBAB> _003C_003E9__0;

		internal bool _E000(_EBAB node)
		{
			if (node != null)
			{
				return node.Data.Id == parentNode.Data.Id;
			}
			return false;
		}
	}

	private readonly HashSet<string> _E106 = new HashSet<string>
	{
		_ED3E._E000(197311),
		_ED3E._E000(252034)
	};

	private const string _E107 = "5b47574386f77428ca22b33e";

	private int _E108 = -1;

	[SerializeField]
	private HierarchyFilterTab _tabTemplate;

	[SerializeField]
	private RectTransform _subTabsContainer;

	[SerializeField]
	private RectTransform _hiddenSubGroupsContainer;

	[SerializeField]
	private HandbookSpecialCategory[] _specialCategories;

	private HoverTrigger _E109;

	private _EBA8 _E081;

	private _E8AF _E10A;

	private List<_EBAB> _E10B;

	private int _E10C;

	private HierarchyFilterTab _E10D;

	private readonly Dictionary<string, HashSet<Item>> _E10E = new Dictionary<string, HashSet<Item>>();

	private readonly Dictionary<FilterTab, _EBAB> _E10F = new Dictionary<FilterTab, _EBAB>();

	private float _E110;

	private HoverTrigger _E111;

	protected override void Awake()
	{
		FilterTab[] filterTabs = _filterTabs;
		if (filterTabs != null && filterTabs.Length != 0)
		{
			return;
		}
		_tooltip = ItemUiContext.Instance.Tooltip;
		_E110 = float.MaxValue;
		if (_E109 == null)
		{
			_E109 = _subTabsContainer.gameObject.AddComponent<HoverTrigger>();
			_E109.OnHoverStart += delegate
			{
				_E001(_E109, null);
			};
			_E109.OnHoverEnd += delegate
			{
				_E002(_E109);
			};
		}
		_E10B = new List<_EBAB> { null };
		List<FilterTab> list = new List<FilterTab> { _E004(null, base.transform) };
		_E081 = Singleton<_EBA8>.Instance;
		List<HandbookData> list2 = (from cat in _E081.Categories
			where string.IsNullOrEmpty(cat.ParentId) && !_E106.Contains(cat.Id)
			orderby cat.Order descending
			select cat).ToList();
		HandbookSpecialCategory[] specialCategories = _specialCategories;
		foreach (HandbookSpecialCategory special in specialCategories)
		{
			_EBAB obj = _E081[special.CategoryId];
			if (obj == null)
			{
				continue;
			}
			int num = -1;
			if (!string.IsNullOrEmpty(special.PlaceAfter))
			{
				num = list2.FindIndex((HandbookData cat) => cat.Id == special.PlaceAfter) + 1;
			}
			if (num < 0)
			{
				num = list2.Count;
			}
			list2.Insert(num, obj.Data);
		}
		foreach (HandbookData item in list2)
		{
			_EBAB obj2 = _E081[item.Id];
			if (obj2 == null)
			{
				continue;
			}
			_E10E[obj2.Data.Id] = new HashSet<Item>();
			HierarchyFilterTab hierarchyFilterTab = _E004(obj2, base.transform);
			list.Add(hierarchyFilterTab);
			_E10B.Add(obj2);
			if (item.Id == _ED3E._E000(252061))
			{
				_E108 = _E10B.Count - 1;
			}
			List<Tab> list3 = new List<Tab>();
			if (obj2.Children != null && obj2.Children.Count > 0 && item.Id != _ED3E._E000(252061))
			{
				_EBAB[] array = (from child in obj2.Children
					where child.Data.Type == ENodeType.Category
					orderby child.Data.Name.Localized()
					select child).ToArray();
				foreach (_EBAB obj3 in array)
				{
					HierarchyFilterTab hierarchyFilterTab2 = _E004(obj3, _hiddenSubGroupsContainer);
					hierarchyFilterTab2.Init(hierarchyFilterTab);
					list3.Add(hierarchyFilterTab2);
					list.Add(hierarchyFilterTab2);
					_E10B.Add(obj3);
					_E10E[obj3.Data.Id] = new HashSet<Item>();
				}
			}
			hierarchyFilterTab.Init(list3);
		}
		_filterTabs = list.ToArray();
		Tab[] filterTabs2 = _filterTabs;
		_tabs = new _EC67(filterTabs2, _filterTabs[0], setAsLastSibling: false);
		for (int j = 0; j < _filterTabs.Length; j++)
		{
			int nodeIndex = j;
			if (_E10B[j] != null)
			{
				_filterTabs[j]._E001(active: false);
			}
			_filterTabs[j].Init(new _E000(this, nodeIndex));
		}
	}

	private void _E000(int index)
	{
		_E10C = index;
		FilterChanged();
	}

	private void _E001(HoverTrigger hoverTrigger, [CanBeNull] HierarchyFilterTab tab)
	{
		if (hoverTrigger == _E109)
		{
			_E110 = float.MaxValue;
			_E111 = hoverTrigger;
		}
		else
		{
			if (tab == null || !tab.Interactable)
			{
				return;
			}
			_E110 = float.MaxValue;
			HoverTrigger component = tab.transform.parent.GetComponent<HoverTrigger>();
			if (component != null)
			{
				_E111 = component;
				return;
			}
			if (_E10D != null && _E10D != tab)
			{
				_E003();
			}
			_E10D = tab;
			IEnumerable<Tab> subTabs = tab.SubTabs;
			if (subTabs == null)
			{
				return;
			}
			foreach (Tab item in subTabs)
			{
				item.transform.SetParent(_subTabsContainer, worldPositionStays: false);
			}
			RectTransform component2 = tab.GetComponent<RectTransform>();
			_subTabsContainer.anchoredPosition = new Vector2(_subTabsContainer.anchoredPosition.x, component2.anchoredPosition.y + component2.sizeDelta.y * component2.pivot.y);
		}
	}

	private void _E002(HoverTrigger hoverTrigger)
	{
		if (hoverTrigger == _E109)
		{
			_E111 = null;
		}
		if (_E10D != null && _E111 == null)
		{
			_E110 = Time.time + 0.2f;
		}
	}

	private void _E003()
	{
		if (_E10D == null || _E10D.SubTabs == null)
		{
			return;
		}
		foreach (Tab subTab in _E10D.SubTabs)
		{
			subTab.transform.SetParent(_hiddenSubGroupsContainer, worldPositionStays: false);
		}
		_subTabsContainer.anchoredPosition = new Vector2(_subTabsContainer.anchoredPosition.x, 0f);
		_E10D = null;
		_E110 = float.MaxValue;
	}

	private void Update()
	{
		if (_E110 < Time.time)
		{
			_E003();
			_E110 = float.MaxValue;
		}
	}

	private HierarchyFilterTab _E004([CanBeNull] _EBAB node, Transform parent)
	{
		HierarchyFilterTab tab = UnityEngine.Object.Instantiate(_tabTemplate, parent, worldPositionStays: false);
		string text = _ED3E._E000(252038);
		if (node != null)
		{
			text = node.Data.Name;
			HandbookSpecialCategory handbookSpecialCategory = _specialCategories.FirstOrDefault((HandbookSpecialCategory cat) => cat.CategoryId == node.Data.Id);
			if (handbookSpecialCategory != null && handbookSpecialCategory.CategoryIcon != null)
			{
				tab.SetIcon(handbookSpecialCategory.CategoryIcon);
			}
			else if (!string.IsNullOrEmpty(node.Data.Icon))
			{
				if (_ECBD.IconsLoader != null)
				{
					_ECBD.IconsLoader.GetIcon(node.Data.Icon, delegate(Sprite iconSprite)
					{
						tab.SetIcon(iconSprite);
					});
				}
				else
				{
					_E10F.Add(tab, node);
				}
			}
		}
		tab.name = text;
		HoverTrigger hover = tab.gameObject.AddComponent<HoverTrigger>();
		hover.OnHoverStart += delegate
		{
			_tooltip.Show(tab.name.Localized(), null, 0.25f, null, tab.Interactable);
			_E001(hover, tab);
		};
		hover.OnHoverEnd += delegate
		{
			_tooltip.Close();
			_E002(hover);
		};
		tab.gameObject.SetActive(value: true);
		return tab;
	}

	private void _E005()
	{
		foreach (KeyValuePair<FilterTab, _EBAB> tabToLoad in new Dictionary<FilterTab, _EBAB>(_E10F))
		{
			_ECBD.IconsLoader.GetIcon(tabToLoad.Value.Data.Icon, delegate(Sprite iconSprite)
			{
				tabToLoad.Key.SetIcon(iconSprite);
			});
			_E10F.Remove(tabToLoad.Key);
		}
	}

	public override void PreInit(_E8AF assortment, IEnumerable<Item> items)
	{
		_E10A = assortment;
		if (items == null)
		{
			return;
		}
		foreach (Item item in items)
		{
			RegisterItem(item);
		}
	}

	public override void Init()
	{
		_tooltip = ItemUiContext.Instance.Tooltip;
		if (_filterTabs == null || _filterTabs.Length == 0)
		{
			Awake();
		}
		if (_E10F.Count > 0)
		{
			_E005();
		}
		FilterTab tab = ((!RememberChoice || _E10C >= _E10B.Count) ? _filterTabs[0] : _filterTabs[_E10C]);
		_tabs.Show(tab, sendCallback: false);
	}

	public override void RegisterItem(Item item)
	{
		BarterScheme barterScheme = _E10A.GetSchemeForItem(item) ?? _E10A.GetSchemeForClone(item);
		if (barterScheme != null && !_EA10.IsCurrencyId(barterScheme[0][0]._tpl) && _E10E.TryGetValue(_E081[_ED3E._E000(252061)].Data.Id, out var value))
		{
			value.Add(item);
			_E006();
		}
		_EBAB obj = _E081[item.TemplateId];
		if (obj == null || obj.TopParent.Data.ParentId == _ED3E._E000(252061))
		{
			return;
		}
		for (_EBAB parent = obj.Parent; parent != null; parent = parent.Parent)
		{
			if (_E10E.TryGetValue(parent.Data.Id, out value))
			{
				value.Add(item);
			}
		}
		SetFilterTabAlpha(item);
	}

	public override void UnregisterItem(Item item)
	{
		BarterScheme barterScheme = _E10A.GetSchemeForItem(item) ?? _E10A.GetSchemeForClone(item);
		if (barterScheme != null && !_EA10.IsCurrencyId(barterScheme[0][0]._tpl) && _E10E.TryGetValue(_E081[_ED3E._E000(252061)].Data.Id, out var value))
		{
			value.Remove(item);
			_E006();
		}
		foreach (KeyValuePair<string, HashSet<Item>> item2 in _E10E)
		{
			item2.Value.Remove(item);
		}
		SetFilterTabAlpha(item);
	}

	private void _E006()
	{
		if (_E108 >= 0)
		{
			_filterTabs[_E108]._E001(_E10E[_E10B[_E108].Data.Id].Count > 0);
		}
	}

	protected override void SetFilterTabAlpha(Item item)
	{
		_EBAB obj = _E081[item.TemplateId];
		if (obj == null)
		{
			return;
		}
		_EBAB parentNode;
		for (parentNode = obj.Parent; parentNode != null; parentNode = parentNode.Parent)
		{
			int num = _E10B.FindIndex((_EBAB node) => node != null && node.Data.Id == parentNode.Data.Id);
			if (num >= 0)
			{
				_filterTabs[num]._E001(_E10E[_E10B[num].Data.Id].Count > 0);
			}
		}
	}

	public override IEnumerable<Item> GetFilteredItems(IEnumerable<Item> allItems)
	{
		if (_E10B == null || _E10C >= _E10B.Count || _E10B[_E10C] == null || !_E10E.ContainsKey(_E10B[_E10C].Data.Id))
		{
			return allItems;
		}
		return _E10E[_E10B[_E10C].Data.Id];
	}

	public override bool IsFilteredSingleItem(Item item)
	{
		return true;
	}

	public override void Hide()
	{
		foreach (KeyValuePair<string, HashSet<Item>> item in _E10E)
		{
			item.Value.Clear();
		}
		for (int i = 0; i < _filterTabs.Length; i++)
		{
			if (_E10B[i] != null)
			{
				_filterTabs[i]._E001(active: false);
			}
		}
		_E003();
		_E110 = float.MaxValue;
		base.Hide();
	}

	public override void ClearChoice()
	{
		_E10C = 0;
		base.ClearChoice();
	}

	[CompilerGenerated]
	private void _E007(PointerEventData arg)
	{
		_E001(_E109, null);
	}

	[CompilerGenerated]
	private void _E008(PointerEventData arg)
	{
		_E002(_E109);
	}

	[CompilerGenerated]
	private bool _E009(HandbookData cat)
	{
		if (string.IsNullOrEmpty(cat.ParentId))
		{
			return !_E106.Contains(cat.Id);
		}
		return false;
	}
}
