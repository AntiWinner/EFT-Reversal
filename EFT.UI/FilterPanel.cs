using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class FilterPanel : UIElement
{
	private enum EFilterItemType
	{
		None,
		All,
		Ammo,
		Barter,
		Container,
		Food,
		Goggles,
		Rig,
		Armor,
		Grenade,
		Info,
		Keys,
		Knife,
		Magazine,
		Meds,
		Mod,
		Special,
		Weapon,
		FoundInRaid
	}

	private class _E000
	{
		[CompilerGenerated]
		private readonly EFilterItemType m__E000;

		public EFilterItemType Type
		{
			[CompilerGenerated]
			get
			{
				return m__E000;
			}
		}

		public _E000(EFilterItemType filterType)
		{
			m__E000 = filterType;
		}

		public virtual bool Check(Item item)
		{
			return true;
		}
	}

	private sealed class _E001 : _E000
	{
		public _E001(EFilterItemType type)
			: base(type)
		{
		}

		public override bool Check(Item item)
		{
			return item.MarkedAsSpawnedInSession;
		}
	}

	private sealed class _E002 : _E000
	{
		private readonly Type[] _E001;

		public _E002(EFilterItemType filterType, params Type[] types)
			: base(filterType)
		{
			_E001 = types;
		}

		public override bool Check(Item item)
		{
			return _E000(item, _E001);
		}
	}

	private sealed class _E003 : _EC63
	{
		private readonly _E000 _E000;

		private readonly FilterPanel _E001;

		public _E003(FilterPanel panel, _E000 rule)
		{
			_E000 = rule;
			_E001 = panel;
		}

		public override void Show()
		{
			_E001._E001(_E000);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public Type itemType;

		internal bool _E000(Type x)
		{
			return x.IsAssignableFrom(itemType);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public _E000 filterRule;

		public int index;

		public FilterPanel _003C_003E4__this;

		internal void _E000(PointerEventData arg)
		{
			_003C_003E4__this._tooltip.Show(filterRule.Type.ToString().Localized(), null, 0.25f, null, _003C_003E4__this._filterTabs[index].Interactable);
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public _E000 filterRule;

		internal bool _E000(Item x)
		{
			return filterRule.Check(x);
		}
	}

	private static readonly _E000[] _E0F7 = new _E000[18]
	{
		new _E000(EFilterItemType.All),
		new _E002(EFilterItemType.Weapon, typeof(Weapon)),
		new _E002(EFilterItemType.Magazine, typeof(_EA6A)),
		new _E002(EFilterItemType.Ammo, typeof(_EA12), typeof(AmmoBox)),
		new _E002(EFilterItemType.Meds, typeof(_EA72)),
		new _E002(EFilterItemType.Food, typeof(_EA48)),
		new _E002(EFilterItemType.Knife, typeof(_EA60)),
		new _E002(EFilterItemType.Mod, typeof(Mod)),
		new _E002(EFilterItemType.Grenade, typeof(_EADF)),
		new _E002(EFilterItemType.Barter, typeof(_EA1E)),
		new _E002(EFilterItemType.Rig, typeof(_EAE1)),
		new _E002(EFilterItemType.Goggles, typeof(_EAE3)),
		new _E002(EFilterItemType.Container, typeof(_EA91), typeof(_EA95)),
		new _E002(EFilterItemType.Armor, typeof(_EAAC)),
		new _E002(EFilterItemType.Info, typeof(_EAC1)),
		new _E002(EFilterItemType.Keys, typeof(_EA5B)),
		new _E002(EFilterItemType.Special, typeof(_EA9A)),
		new _E001(EFilterItemType.FoundInRaid)
	};

	[CompilerGenerated]
	private Action _E0F8;

	[SerializeField]
	protected FilterTab[] _filterTabs;

	public bool RememberChoice;

	protected SimpleTooltip _tooltip;

	protected _EC67 _tabs;

	private readonly Dictionary<EFilterItemType, FilterTab> _E0F9 = _E3A5<EFilterItemType>.GetDictWith<FilterTab>();

	private readonly List<Item> _E0FA = new List<Item>();

	[CompilerGenerated]
	private _E000 _E0FB;

	private _E000 _E000
	{
		[CompilerGenerated]
		get
		{
			return _E0FB;
		}
		[CompilerGenerated]
		set
		{
			_E0FB = value;
		}
	}

	public event Action CurrentFilterChanged
	{
		[CompilerGenerated]
		add
		{
			Action action = _E0F8;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E0F8, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E0F8;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E0F8, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private static bool _E000(Item item, Type[] types)
	{
		Type itemType = item.GetType();
		return types.Any((Type x) => x.IsAssignableFrom(itemType));
	}

	protected virtual void Awake()
	{
		if (_tabs != null)
		{
			return;
		}
		_tooltip = ItemUiContext.Instance.Tooltip;
		Tab[] filterTabs = _filterTabs;
		_tabs = new _EC67(filterTabs, _filterTabs[0], setAsLastSibling: false);
		for (int i = 0; i < _E0F7.Length; i++)
		{
			int index = i;
			_E000 filterRule = _E0F7[i];
			_E0F9.Add(filterRule.Type, _filterTabs[index]);
			_filterTabs[index].Init(new _E003(this, filterRule));
			if (_filterTabs.Length != _E0F7.Length || _tooltip == null)
			{
				Debug.LogError(_ED3E._E000(253864));
				continue;
			}
			HoverTrigger hoverTrigger = _filterTabs[index].gameObject.AddComponent<HoverTrigger>();
			hoverTrigger.OnHoverStart += delegate
			{
				_tooltip.Show(filterRule.Type.ToString().Localized(), null, 0.25f, null, _filterTabs[index].Interactable);
			};
			hoverTrigger.OnHoverEnd += delegate
			{
				_tooltip.Close();
			};
		}
	}

	protected void FilterChanged()
	{
		_E0F8?.Invoke();
	}

	public virtual void PreInit(_E8AF assortment, [CanBeNull] IEnumerable<Item> items)
	{
	}

	public virtual void Init()
	{
		if (_tabs == null)
		{
			Awake();
		}
		FilterTab[] filterTabs = _filterTabs;
		for (int i = 0; i < filterTabs.Length; i++)
		{
			filterTabs[i]._E001(active: false);
		}
		if (this._E000 == null)
		{
			this._E000 = _E0F7.First();
		}
		_tabs.Show(RememberChoice ? _E0F9[this._E000.Type] : _filterTabs[0]);
	}

	public virtual void RegisterItem(Item item)
	{
		if (!_E0FA.Contains(item))
		{
			_E0FA.Add(item);
			SetFilterTabAlpha(item);
		}
	}

	public virtual void UnregisterItem(Item item)
	{
		if (_E0FA.Remove(item))
		{
			SetFilterTabAlpha(item);
		}
	}

	public void ClearAll()
	{
		_E0FA.Clear();
		foreach (KeyValuePair<EFilterItemType, FilterTab> item in _E0F9)
		{
			_E39D.Deconstruct(item, out var _, out var value);
			value._E001(active: false);
		}
	}

	protected virtual void SetFilterTabAlpha(Item item)
	{
		_E000[] array = _E0F7;
		foreach (_E000 filterRule in array)
		{
			if (filterRule.Check(item))
			{
				_E0F9[filterRule.Type]._E001(_E0FA.Any((Item x) => filterRule.Check(x)));
			}
		}
	}

	public virtual bool IsFilteredSingleItem(Item item)
	{
		return this._E000.Check(item);
	}

	public virtual IEnumerable<Item> GetFilteredItems(IEnumerable<Item> allItems)
	{
		if (this._E000.Type.Equals(EFilterItemType.All))
		{
			return _E0FA;
		}
		return _E0FA.Where((Item item) => this._E000.Check(item));
	}

	public virtual void Hide()
	{
		_tabs.TryHide();
		if (_tooltip != null)
		{
			_tooltip.Close();
		}
	}

	public virtual void ClearChoice()
	{
		RememberChoice = false;
		this._E000 = _E0F7.First();
	}

	private void _E001(_E000 filter)
	{
		this._E000 = filter;
		FilterChanged();
	}

	[CompilerGenerated]
	private void _E002(PointerEventData arg)
	{
		_tooltip.Close();
	}

	[CompilerGenerated]
	private bool _E003(Item item)
	{
		return this._E000.Check(item);
	}
}
