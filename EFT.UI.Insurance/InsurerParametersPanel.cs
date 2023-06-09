using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Insurance;

public sealed class InsurerParametersPanel : UIElement
{
	public const string NO_SELECTED_ITEMS = "Trading/NoSelectedItems";

	[CompilerGenerated]
	private Action<bool> _E30A;

	[SerializeField]
	private DropDownBox _tradersDropDown;

	[SerializeField]
	private RankPanel _rankPanel;

	[SerializeField]
	private Image _avatar;

	[SerializeField]
	private TextMeshProUGUI _minPayment;

	[SerializeField]
	private TextMeshProUGUI _returnRate;

	[SerializeField]
	private TextMeshProUGUI _repairerStanding;

	[SerializeField]
	private DefaultUIButton _insureButton;

	[SerializeField]
	private TextMeshProUGUI _returnTime;

	[SerializeField]
	private TextMeshProUGUI _cashInStash;

	[SerializeField]
	private TextMeshProUGUI _sumToPay;

	[SerializeField]
	private TradeItemType _sellTypeTemplate;

	[SerializeField]
	private RectTransform _sellTypesContainer;

	[SerializeField]
	private Color _criticalColor;

	[SerializeField]
	private Color _defaultColor;

	private _EAE7 _E156;

	private _E74F _E135;

	private _EC79<EItemType, TradeItemType> _E30B;

	private _ECB1 _E18F;

	private Action _E30C;

	private Action _E30D;

	private Action _E30E;

	private int _E30F;

	private int _E310;

	private Dictionary<ECurrencyType, int> _E311 = new Dictionary<ECurrencyType, int>();

	private int _E312;

	private bool _E313;

	private double _E314 = 1.0;

	private List<_E8B2> _E315;

	private List<string> _E316;

	private _E3A4 _E15B = new _E3A4();

	private MultiLineTooltip _E02A;

	private _EC80 _E126;

	private bool _E000
	{
		get
		{
			return _E313;
		}
		set
		{
			if (_E313 != value)
			{
				_E313 = value;
				_E30A?.Invoke(!value);
			}
		}
	}

	private Dictionary<Item, LocationInGrid>.KeyCollection _E001 => _E156.Stash.Grid.ContainedItems.Keys;

	private _E5CB.TraderInsurance _E002 => _E18F.SelectedInsurer.Settings.Insurance;

	public event Action<bool> OnInsuranceAvailableChanged
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E30A;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E30A, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E30A;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E30A, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Awake()
	{
		_E02A = ItemUiContext.Instance.MultiLineTooltip;
		HoverTrigger orAddComponent = _sumToPay.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate
		{
			if (_E126 != null)
			{
				_E02A.Show(_E126);
			}
		};
		orAddComponent.OnHoverEnd += delegate
		{
			if (_E02A != null && _E02A.isActiveAndEnabled)
			{
				_E02A.Close();
			}
		};
	}

	private void OnDisable()
	{
		if (_E02A != null && _E02A.isActiveAndEnabled)
		{
			_E02A.Close();
		}
	}

	public void Show(_EAE7 inventory, _ECB1 insurance, Action updateItemsPrices, _E74F skills)
	{
		ShowGameObject();
		_tradersDropDown.Bind(_E000);
		_E135 = skills;
		_E156 = inventory;
		_E18F = insurance;
		_E315 = _E18F.Insurers.ToList();
		_E316 = _E315.Select((_E8B2 trader) => trader.Id).ToList();
		_E30C = updateItemsPrices;
		_tradersDropDown.Show(_E315.Select((_E8B2 trader) => trader.Settings.Nickname.Localized()));
		_tradersDropDown.UpdateValue(_E316.IndexOf(_E18F.SelectedInsurerId));
		_E314 = _E135.CalculateCharismaInsuranceDiscount(_E18F.SelectedInsurer.Info);
	}

	public void UpdateInsureButtonStatus(_ECB1.EInsuranceError? error)
	{
		string tooltip = string.Empty;
		if (!error.HasValue && this._E000)
		{
			error = _ECB1.EInsuranceError.NotEnoughMoney;
		}
		switch (error)
		{
		case _ECB1.EInsuranceError.InvalidItem:
			tooltip = _ED3E._E000(252830);
			break;
		case _ECB1.EInsuranceError.NotEnoughMoney:
			tooltip = _ED3E._E000(232963);
			break;
		case _ECB1.EInsuranceError.NothingToInsure:
			tooltip = _ED3E._E000(232996);
			break;
		}
		_insureButton.SetDisabledTooltip(tooltip);
		_insureButton.Interactable = !error.HasValue;
	}

	private void _E000(int index)
	{
		_E18F.SelectedInsurerId = _E316[index];
		_E5CB.TraderSettings settings = _E18F.SelectedInsurer.Settings;
		Profile._E001 info = _E18F.SelectedInsurer.Info;
		BonusController instance = Singleton<BonusController>.Instance;
		_E314 = _E135.CalculateCharismaInsuranceDiscount(_E18F.SelectedInsurer.Info);
		_E30F = (int)Math.Ceiling(instance.Calculate(EBonusType.InsuranceReturnTime, this._E002.MinReturnHours));
		_E310 = (int)Math.Ceiling(instance.Calculate(EBonusType.InsuranceReturnTime, this._E002.MaxReturnHours));
		_E30C();
		_E15B.Dispose();
		settings.GetAndAssignAvatar(_avatar, _E15B.CancellationToken).HandleExceptions();
		_minPayment.text = this._E002.MinPayment.ToString();
		_returnRate.text = _E001(_E30F, _E310).Localized();
		_repairerStanding.text = info.Standing.ToString(_ED3E._E000(253692));
		_rankPanel.Show(info.LoyaltyLevel, info.MaxLoyaltyLevel);
		IEnumerable<EItemType> items = settings.Insurance.ExcludedCategories.Select((string x) => ItemViewFactory.GetItemType(_EA59.TypeTable[x])).Distinct();
		foreach (Transform item in _sellTypesContainer.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		_E30B?.Dispose();
		_E30B = new _EC79<EItemType, TradeItemType>(items, (EItemType arg) => _sellTypeTemplate, (EItemType arg) => _sellTypesContainer, delegate(EItemType itemType, TradeItemType itemTypeView)
		{
			itemTypeView.Show(itemType);
		});
	}

	public void UpdateLabels(int moneyToPay, bool updateMoneyToPay = true, bool updateMoneySums = true)
	{
		if (updateMoneySums)
		{
			_E311 = _EB0E.GetMoneySums(this._E001);
			if (_cashInStash != null)
			{
				_cashInStash.text = _E311[ECurrencyType.RUB].ToString();
			}
		}
		if (updateMoneyToPay)
		{
			_E312 = moneyToPay;
			_E126 = (_E314.ApproxEquals(1.0) ? null : new _EC80(ECurrencyType.RUB, ECharismaDiscountType.InsuranceCharismaDiscount, (int)((double)_E312 / _E314), _E312, _E311[ECurrencyType.RUB]));
			if (_sumToPay != null)
			{
				_sumToPay.text = ((moneyToPay >= 0) ? _E312.ToString() : _ED3E._E000(59476));
			}
			if (_returnTime != null)
			{
				_returnTime.text = string.Format(_ED3E._E000(233036), _E30F, _E310, _ED3E._E000(103409).Localized());
			}
		}
		this._E000 = _E312 > _E311[ECurrencyType.RUB];
		if (_sumToPay != null)
		{
			_sumToPay.color = (this._E000 ? _criticalColor : _defaultColor);
		}
	}

	private string _E001(int start, int end)
	{
		if (end > 24)
		{
			return _ED3E._E000(233025);
		}
		if (start >= 12)
		{
			return _ED3E._E000(233083);
		}
		if (start >= 6 && end < 12)
		{
			return _ED3E._E000(261675);
		}
		if (start >= 2 && end < 6)
		{
			return _ED3E._E000(233072);
		}
		if (end <= 6)
		{
			return _ED3E._E000(233069);
		}
		return _ED3E._E000(246045);
	}

	public override void Close()
	{
		_E15B.Dispose();
		_tradersDropDown.Hide();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002(PointerEventData arg)
	{
		if (_E126 != null)
		{
			_E02A.Show(_E126);
		}
	}

	[CompilerGenerated]
	private void _E003(PointerEventData arg)
	{
		if (_E02A != null && _E02A.isActiveAndEnabled)
		{
			_E02A.Close();
		}
	}

	[CompilerGenerated]
	private TradeItemType _E004(EItemType arg)
	{
		return _sellTypeTemplate;
	}

	[CompilerGenerated]
	private Transform _E005(EItemType arg)
	{
		return _sellTypesContainer;
	}
}
