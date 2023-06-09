using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.Hideout;
using EFT.InventoryLogic;
using EFT.Trading;
using EFT.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class HandoverExchangeableItemsWindow : HandoverItemsWindow
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TaskCompletionSource<_E7D8> source;

		internal void _E000(_E7D8 acceptResult)
		{
			source.SetResult(acceptResult);
		}

		internal void _E001()
		{
			source.SetResult(null);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Predicate<Item> allowedItemPredicate;

		internal bool _E000(Item x)
		{
			return !allowedItemPredicate(x);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _E8B1 requisite;

		internal bool _E000(_E8B1 r)
		{
			return r.IsEqual(requisite);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public Item item;

		internal bool _E000(_E8B1 requisite)
		{
			return requisite.IsRequired(item);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _E8B1 requisite;

		internal bool _E000(Item item)
		{
			return requisite.IsRequired(item);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public Item item;

		internal bool _E000(_E8B1 requisite)
		{
			if (!requisite.Enough)
			{
				return requisite.IsRequired(item);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public Item item;

		internal bool _E000(_E8B1 requisite)
		{
			return requisite.IsPreparedForTrade(item);
		}
	}

	[SerializeField]
	private HandoverRequirementBarterIcon _barterIcon;

	[SerializeField]
	private RectTransform _barterContainer;

	[SerializeField]
	private TextMeshProUGUI _expirationLabel;

	[SerializeField]
	private TMP_InputField _inputField;

	[SerializeField]
	private TextMeshProUGUI _maxCountLabel;

	[SerializeField]
	private Button _allItemsButton;

	[SerializeField]
	private GameObject _countPanel;

	[SerializeField]
	private GameObject _toolMarker;

	private SimpleTooltip _E010;

	[NonSerialized]
	public Dictionary<_E7D3, int> Goods;

	private Dictionary<_E7D3, List<_E8B1>> _E011;

	private Dictionary<_E8B1, HandoverRequirementBarterIcon> _E012;

	private _EC79<_E8B1, HandoverRequirementBarterIcon> _E013;

	private List<_E8B1> _E014 = new List<_E8B1>();

	private Action<_E7D8> m__E001;

	private List<Item> _E015 = new List<Item>();

	private EMemberCategory _E016;

	private EExchangeableWindowType _E017;

	private DateTime _E018;

	private IEnumerator _E019;

	private readonly HashSet<string> _E01A = new HashSet<string>();

	private ICollection<Item> _E01B = new List<Item>();

	private ICollection<Item> _E01C = new List<Item>();

	private TimeSpan _E000 => _E018 - _E5AD.UtcNow;

	protected override void Awake()
	{
		base.Awake();
		_inputField.onValueChanged.AddListener(delegate(string arg)
		{
			if (base.gameObject.activeSelf)
			{
				_E3EF.RagfairValidation(arg, _inputField, ref Goods, SetAcceptActive, delegate
				{
					_E003();
					UpdateItems(_E015);
				});
			}
		});
		_allItemsButton.onClick.AddListener(delegate
		{
			int num = Goods.Min((KeyValuePair<_E7D3, int> x) => x.Key.CurrentItemCount);
			_inputField.text = num.ToString();
		});
	}

	public Task<_E7D8> SelectItemsAsync(EExchangeableWindowType windowType, Dictionary<_E7D3, int> goods, bool displayInputField, Profile profile, _EAED controller, Predicate<Item> allowedItemPredicate = null)
	{
		TaskCompletionSource<_E7D8> source = new TaskCompletionSource<_E7D8>();
		Show(windowType, goods, displayInputField, profile, controller, delegate(_E7D8 acceptResult)
		{
			source.SetResult(acceptResult);
		}, delegate
		{
			source.SetResult(null);
		}, allowedItemPredicate);
		return source.Task;
	}

	public void Show(EExchangeableWindowType windowType, Dictionary<_E7D3, int> goods, bool displayInputField, Profile profile, _EAED controller, Action<_E7D8> acceptAction, Action cancelAction, Predicate<Item> allowedItemPredicate = null)
	{
		Goods = goods;
		_E011 = new Dictionary<_E7D3, List<_E8B1>>();
		_E017 = windowType;
		this.m__E001 = acceptAction;
		foreach (_E8B1 item2 in _E014)
		{
			item2.Clear();
		}
		_E015.Clear();
		profile.Inventory.Stash.GetAllAssembledItemsNonAlloc(_E015);
		profile.Inventory.QuestStashItems.GetAllAssembledItemsNonAlloc(_E015);
		profile.Inventory.QuestRaidItems.GetAllAssembledItemsNonAlloc(_E015);
		if (allowedItemPredicate != null)
		{
			_E015.RemoveAll((Item x) => !allowedItemPredicate(x));
		}
		_E01A.Clear();
		_E008();
		foreach (KeyValuePair<_E7D3, int> good in Goods)
		{
			_E39D.Deconstruct(good, out var key, out var i);
			_E7D3 obj = key;
			List<_E8B1> list = new List<_E8B1>();
			_E011.Add(obj, list);
			IExchangeRequirement[] requirements = obj.Requirements;
			for (i = 0; i < requirements.Length; i++)
			{
				IExchangeRequirement exchangeRequirement = requirements[i];
				Item item = exchangeRequirement.Item;
				int level = 0;
				EDogtagExchangeSide side = EDogtagExchangeSide.Any;
				IExchangeRequirement exchangeRequirement2 = exchangeRequirement;
				if (exchangeRequirement2 != null)
				{
					if (!(exchangeRequirement2 is HandoverRequirement handoverRequirement))
					{
						if (exchangeRequirement2 is ToolRequirement toolRequirement)
						{
							ToolRequirement toolRequirement2 = toolRequirement;
							_E01A.Add(toolRequirement2.TemplateId);
						}
					}
					else
					{
						level = handoverRequirement.Level;
						side = handoverRequirement.Side;
					}
				}
				_E8B1 requisite = new _E8B1(item, exchangeRequirement.IntCount, level, side, exchangeRequirement.IsEncoded, exchangeRequirement.OnlyFunctional);
				_E8B1 obj2 = _E014.FirstOrDefault((_E8B1 r) => r.IsEqual(requisite));
				if (obj2 != null)
				{
					obj2.Consume(requisite);
					continue;
				}
				_E014.Add(requisite);
				requisite.OnChangedFromInventory += _E007;
				list.Add(requisite);
			}
		}
		_E003();
		_E010 = ItemUiContext.Instance.Tooltip;
		_E016 = ((!Goods.Any((KeyValuePair<_E7D3, int> x) => x.Key.MemberType != EMemberCategory.Trader)) ? EMemberCategory.Trader : EMemberCategory.Default);
		_E018 = Goods.Min((KeyValuePair<_E7D3, int> x) => x.Key.EndTime);
		_E012 = new Dictionary<_E8B1, HandoverRequirementBarterIcon>();
		if (displayInputField)
		{
			_inputField.text = _ED3E._E000(27343);
			_maxCountLabel.text = _ED3E._E000(124720) + Goods.Min((KeyValuePair<_E7D3, int> x) => x.Key.CurrentItemCount);
		}
		else
		{
			_inputField.text = string.Empty;
			_maxCountLabel.text = string.Empty;
		}
		if (_E017 == EExchangeableWindowType.Hideout)
		{
			_expirationLabel.text = string.Empty;
		}
		_EB66 itemContext;
		string title;
		switch (_E017)
		{
		case EExchangeableWindowType.Hideout:
			itemContext = new _EB66(EItemViewType.Hideout, this);
			title = _ED3E._E000(231983).Localized();
			break;
		case EExchangeableWindowType.Ragfair:
			itemContext = new _EB66(EItemViewType.Ragfair, this);
			title = _ED3E._E000(232024).Localized();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		Show(title, _ED3E._E000(232001).Localized(), profile, controller, itemContext, cancelAction);
		if (_E016 != EMemberCategory.Trader)
		{
			_E004();
			_E019 = this.StartBehaviourTimer(1f, repeatable: true, _E004);
		}
		else
		{
			_expirationLabel.text = string.Empty;
		}
		_countPanel.gameObject.SetActive(!Goods.Any((KeyValuePair<_E7D3, int> x) => x.Key.SellInOnePiece) && displayInputField);
		UpdateItems(_E015);
	}

	public override void Accept()
	{
		_E7D8 obj = new _E7D8();
		foreach (KeyValuePair<_E7D3, List<_E8B1>> item in _E011)
		{
			_E39D.Deconstruct(item, out var key, out var value);
			_E7D3 obj2 = key;
			List<_E8B1> source = value;
			int count = Goods[obj2];
			List<_E7D4> items = source.SelectMany((_E8B1 requisite) => requisite.PreparedItems.Select((TradingItemReference reference) => reference.AsItemReference())).ToList();
			obj.Add(new _E7D7(obj2, count, items));
		}
		this.m__E001?.Invoke(obj);
		if (!base.isActiveAndEnabled)
		{
			_E009();
		}
		base.Accept();
	}

	protected override void UpdateItems(ICollection<Item> playerItems)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		ClearSelectedList();
		_E013?.Dispose();
		_E012.Clear();
		_E013 = UI.AddViewList(_E014, _barterIcon, _barterContainer, delegate(_E8B1 requisite, HandoverRequirementBarterIcon panel)
		{
			panel.Show(requisite, _E010);
			_E012.Add(requisite, panel);
		});
		_E01B = playerItems.Where((Item item) => _E014.Any((_E8B1 requisite) => requisite.IsRequired(item))).ToList();
		_E014.AutoFill(playerItems, _E001);
		_E01C = new List<Item>();
		foreach (_E8B1 requisite2 in _E014)
		{
			if (!_E01B.Any((Item item) => requisite2.IsRequired(item)))
			{
				_E01C.Add(requisite2.RequiredItem.CloneItem());
			}
		}
		base.UpdateItems(_E01C.Concat(_E01B).ToArray());
		_E000();
		_E002();
	}

	private void _E000()
	{
		foreach (ItemView item in base.GridView._E002)
		{
			if (item is GridItemView gridItemView && _E01A.Contains(item.Item.TemplateId))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(_toolMarker);
				gridItemView.AddCustomObjectToInfoPanel(gameObject.RectTransform(), Vector2.zero);
				gameObject.SetActive(value: true);
			}
		}
	}

	protected override bool IsSelected(Item item)
	{
		return ItemsList.Contains(item);
	}

	private bool _E001(Item item)
	{
		string tooltip;
		return IsActive(item, out tooltip);
	}

	protected override bool IsActive(Item item, out string tooltip)
	{
		tooltip = string.Empty;
		if (_E01C.Contains(item))
		{
			tooltip = string.Format(_ED3E._E000(260834) + _ED3E._E000(232034).Localized() + _ED3E._E000(47210), item.ShortName.Localized());
			return false;
		}
		if (!_E01B.Contains(item))
		{
			return false;
		}
		if (_E016 == EMemberCategory.Trader)
		{
			return true;
		}
		return _ECBD.CanUseForBarterExchange(item, out tooltip);
	}

	private void _E002()
	{
		List<Item> list = _E014.SelectMany((_E8B1 x) => x.PreparedItems.Select((TradingItemReference itemReference) => itemReference.Item)).ToList();
		foreach (Item item in _E01B)
		{
			if (list.Contains(item))
			{
				SelectView(item);
				ItemsList.Add(item);
			}
			else
			{
				DeselectView(item);
			}
		}
		SetAcceptActive(_E014.All((_E8B1 x) => x.Enough));
		foreach (HandoverRequirementBarterIcon value in _E012.Values)
		{
			value.UpdateView();
		}
	}

	protected override void AutoSelectButtonPressedHandler()
	{
		UpdateItems(_E015);
	}

	private void _E003()
	{
		foreach (KeyValuePair<_E7D3, int> good in Goods)
		{
			_E7D3 key = good.Key;
			int value = good.Value;
			foreach (_E8B1 item in _E011[key])
			{
				item.Quantity = (key.SellInOnePiece ? 1 : value);
			}
		}
	}

	private void _E004()
	{
		if (this._E000.TotalSeconds <= 0.0)
		{
			Close();
		}
		else
		{
			_expirationLabel.text = this._E000.RagfairDateFormatLong();
		}
	}

	protected override void TrySelectItemToHandover(Item item)
	{
		if (_E001(item))
		{
			if (_E014.IsPreparedForTrade(item))
			{
				_E006(item);
			}
			else
			{
				_E005(item);
			}
			SetAcceptActive(_E014.All((_E8B1 x) => x.Enough));
		}
	}

	private void _E005(Item item)
	{
		if (!_E014.IsPreparedForTrade(item))
		{
			List<_E8B1> list = (from requisite in _E014
				where !requisite.Enough && requisite.IsRequired(item)
				orderby requisite.Level descending, requisite.RequiredItemsCount
				select requisite).ToList();
			if (list.Count != 0)
			{
				_E8B1 obj = list[0];
				obj.PrepareItem(item);
				_E012[obj].UpdateView();
				SelectView(item);
				ItemsList.Add(item);
			}
		}
	}

	private void _E006(Item item)
	{
		_E8B1 obj = _E014.Find((_E8B1 requisite) => requisite.IsPreparedForTrade(item));
		if (obj != null)
		{
			obj.RemoveItem(item);
			if (base.isActiveAndEnabled)
			{
				_E012[obj].UpdateView();
				DeselectView(item);
			}
			ItemsList.Remove(item);
		}
	}

	protected override void ClearSelectedList()
	{
		for (int num = ItemsList.Count - 1; num >= 0; num--)
		{
			_E006(ItemsList[num]);
		}
		base.ClearSelectedList();
	}

	private void _E007()
	{
		_E002();
	}

	private void _E008()
	{
		foreach (_E8B1 item in _E014)
		{
			item.OnChangedFromInventory -= _E007;
			item.Clear();
		}
		_E014.Clear();
	}

	private void _E009()
	{
		ClearSelectedList();
		_E008();
		if (_E016 != EMemberCategory.Trader)
		{
			this.StopBehaviourTimer(ref _E019);
		}
		_E012?.Clear();
		this.m__E001 = null;
	}

	public override void Close()
	{
		_E009();
		base.Close();
	}

	[CompilerGenerated]
	private void _E00A(string arg)
	{
		if (base.gameObject.activeSelf)
		{
			_E3EF.RagfairValidation(arg, _inputField, ref Goods, SetAcceptActive, delegate
			{
				_E003();
				UpdateItems(_E015);
			});
		}
	}

	[CompilerGenerated]
	private void _E00B()
	{
		_E003();
		UpdateItems(_E015);
	}

	[CompilerGenerated]
	private void _E00C()
	{
		int num = Goods.Min((KeyValuePair<_E7D3, int> x) => x.Key.CurrentItemCount);
		_inputField.text = num.ToString();
	}

	[CompilerGenerated]
	private void _E00D(_E8B1 requisite, HandoverRequirementBarterIcon panel)
	{
		panel.Show(requisite, _E010);
		_E012.Add(requisite, panel);
	}

	[CompilerGenerated]
	private bool _E00E(Item item)
	{
		return _E014.Any((_E8B1 requisite) => requisite.IsRequired(item));
	}
}
