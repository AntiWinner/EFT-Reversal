using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class RepairerParametersPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public RepairerParametersPanel _003C_003E4__this;

		public Item item;

		public IItemOwner itemOwner;

		internal bool _E000(_E8B2 trader)
		{
			return _003C_003E4__this._E009(trader, item, trader.Targets);
		}

		internal void _E001()
		{
			itemOwner.AddItemEvent -= _003C_003E4__this.ItemAddedHandler;
			itemOwner.RemoveItemEvent -= _003C_003E4__this.ItemRemovedHandler;
			_003C_003E4__this._E153.OnSuccessfulRepairChangedEvent -= _003C_003E4__this.UpdateRepairKitLabels;
			_003C_003E4__this._E153.OnSuccessfulRepairChangedEvent -= _003C_003E4__this.UpdateBuffState;
		}
	}

	[SerializeField]
	private DropDownBox _tradersDropDown;

	[SerializeField]
	private RankPanel _rankPanel;

	[SerializeField]
	private Image _avatar;

	[SerializeField]
	private GameObject _repairerStandingGameObject;

	[SerializeField]
	private TextMeshProUGUI _repairPriceRate;

	[SerializeField]
	private TextMeshProUGUI _repairerStanding;

	[SerializeField]
	private TextMeshProUGUI _repairSpeed;

	[SerializeField]
	private TextMeshProUGUI _repairQuality;

	[SerializeField]
	private TextMeshProUGUI _repairability;

	[SerializeField]
	private TextMeshProUGUI _enhancementChance;

	[SerializeField]
	private RepairWarningStatusPanel _repairWarningStatusPanel;

	[SerializeField]
	private ConditionCharacteristicsSlider _conditionSlider;

	[SerializeField]
	private TextMeshProUGUI _repairTime;

	[SerializeField]
	private TextMeshProUGUI _repairDp;

	[SerializeField]
	private LocalizedText _cashInStashLabel;

	[SerializeField]
	private TextMeshProUGUI _cashInStash;

	[SerializeField]
	private Image _cashInStashIcon;

	[SerializeField]
	private Sprite _repairPointsInStashSprite;

	[SerializeField]
	private LocalizedText _sumToPayLabel;

	[SerializeField]
	private TextMeshProUGUI _sumToPay;

	[SerializeField]
	private Image _sumToPayIcon;

	[SerializeField]
	private Sprite _repairPointsToPaySprite;

	[SerializeField]
	private TextMeshProUGUI _repairKitChargeText;

	[SerializeField]
	private Color _criticalColor;

	[SerializeField]
	private Color _defaultColor;

	private const string _E14F = "<color=#c5c3b4>{0}</color> {1}";

	[CompilerGenerated]
	private _E8CF _E150;

	private _EB10 _E151;

	private _EB10 _E152;

	private _E8D0 _E153;

	private List<_E8B2> _E154;

	private List<_E3C6> _E155;

	private Item _E085;

	private _EAE7 _E156;

	private Sprite _E157;

	private _EA8D _E158;

	private _EA8D _E159;

	private RepairableComponent _E15A;

	private _E3A4 _E15B = new _E3A4();

	private IEnumerable<Item> _E000 => _E156.Stash.Grid.ContainedItems.Keys;

	private float _E001 => _E085.RepairCost;

	private float _E002 => _conditionSlider.RealCurrentDurability;

	public _E8CF CurrentRepairer
	{
		[CompilerGenerated]
		get
		{
			return _E150;
		}
		[CompilerGenerated]
		private set
		{
			_E150 = value;
		}
	}

	private IEnumerable<_E8CF> _E003
	{
		get
		{
			foreach (_E3C6 item in _E155)
			{
				yield return item;
			}
			foreach (_E8B2 item2 in _E154)
			{
				yield return item2;
			}
		}
	}

	public float RepairAmount => _conditionSlider.Value;

	private void Awake()
	{
		_tradersDropDown.Bind(_E006);
		_conditionSlider.OnSliderValueChangedEvent += delegate
		{
			UpdateTraderLabels();
			UpdateRepairKitLabels();
			_E000();
		};
		_E157 = _cashInStashIcon.sprite;
	}

	public void Show(_E8D0 repairController, Item item, _EAE7 inventory, _EA8D draggedRepairKit)
	{
		_E153 = repairController;
		_E158 = draggedRepairKit;
		_E085 = item;
		_E156 = inventory;
		_E159 = draggedRepairKit;
		_E155 = repairController.GetSuitableRepairersCollections(item).ToList();
		_E154 = _E153.TraderRepairers.Where((_E8B2 trader) => _E009(trader, item, trader.Targets)).ToList();
		if (_E158 != null)
		{
			_E003(_E158);
		}
		CurrentRepairer = this._E003.FirstOrDefault();
		_E15A = _E085.GetItemComponent<RepairableComponent>();
		_E151 = _E085.Attributes.Find((_EB10 x) => x.Id.Equals(EItemAttributeId.Durability));
		_E152 = _E085.Attributes.Find((_EB10 x) => x.Id.Equals(EItemAttributeId.MaxDurability));
		if (_E15A == null)
		{
			throw new Exception(_ED3E._E000(247605));
		}
		_conditionSlider.Show(_E151, _E152, _E15A.TemplateDurability);
		List<string> values = this._E003.Select((_E8CF trader) => trader.LocalizedName).ToList();
		_tradersDropDown.Show(values);
		_tradersDropDown.UpdateValue(_E002(CurrentRepairer));
		IItemOwner itemOwner = _E085.Parent.GetOwner();
		itemOwner.AddItemEvent += ItemAddedHandler;
		itemOwner.RemoveItemEvent += ItemRemovedHandler;
		_E153.OnSuccessfulRepairChangedEvent += UpdateRepairKitLabels;
		UpdateBuffState();
		_E153.OnSuccessfulRepairChangedEvent += UpdateBuffState;
		UI.AddDisposable(delegate
		{
			itemOwner.AddItemEvent -= ItemAddedHandler;
			itemOwner.RemoveItemEvent -= ItemRemovedHandler;
			_E153.OnSuccessfulRepairChangedEvent -= UpdateRepairKitLabels;
			_E153.OnSuccessfulRepairChangedEvent -= UpdateBuffState;
		});
		UI.AddDisposable(_E15B);
		UI.AddDisposable(_conditionSlider);
		UI.AddDisposable(_tradersDropDown);
	}

	public void ItemAddedHandler(_EAF2 eventArgs)
	{
		if (eventArgs.Status != CommandStatus.Succeed)
		{
			return;
		}
		Item item = eventArgs.Item;
		UpdateTraderLabels();
		if (item is _EA8D obj)
		{
			if (obj == _E159)
			{
				_E158 = obj;
			}
			if (_E156.Stash.Contains(obj) || obj == _E158)
			{
				_E003(obj);
				UpdateRepairers();
				UpdateRepairKitLabels();
			}
		}
	}

	public void ItemRemovedHandler(_EAF3 eventArgs)
	{
		if (eventArgs.Status != CommandStatus.Succeed)
		{
			return;
		}
		UpdateTraderLabels();
		if (!(eventArgs.Item is _EA8D obj))
		{
			return;
		}
		if (obj == _E158)
		{
			_E158 = null;
		}
		foreach (_E3C6 item in _E155)
		{
			item.RemoveRepairKit(obj);
		}
		UpdateRepairKitLabels();
	}

	public void UpdateRepairers()
	{
		List<string> values = this._E003.Select((_E8CF trader) => trader.LocalizedName).ToList();
		_tradersDropDown.Show(values);
		_tradersDropDown.UpdateValue(_E002(CurrentRepairer));
		_conditionSlider.Show(_E151, _E152, _E15A.TemplateDurability);
	}

	private void _E000()
	{
		if (!CurrentRepairer.TryGetEnhancementChance(_E085, RepairAmount / (float)_E15A.TemplateDurability, out var enhancementChance))
		{
			_enhancementChance.text = "";
		}
		else
		{
			_enhancementChance.text = string.Format(_ED3E._E000(247642), _E00B(enhancementChance), _ED3E._E000(247673).Localized());
		}
	}

	public void UpdateBuffState()
	{
		var (isVisible, isVisible2) = _E001(_E085);
		_repairWarningStatusPanel.ShowCommonBuffInfo(isVisible);
		_repairWarningStatusPanel.ShowRareBuffInfo(isVisible2);
	}

	private (bool hasCommonBuff, bool hasRareBuff) _E001(Item item)
	{
		BuffComponent itemComponent = item.GetItemComponent<BuffComponent>();
		if (itemComponent != null && _E3A5<ERepairBuffType>.IsDefined(itemComponent.BuffType))
		{
			if (itemComponent.Rarity != 0)
			{
				return (false, true);
			}
			return (true, false);
		}
		return (false, false);
	}

	private int _E002(_E8CF repairer)
	{
		int num = 0;
		foreach (_E8CF item in this._E003)
		{
			if (item == repairer)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	private void _E003(_EA8D repairKit)
	{
		foreach (_E3C6 item in _E155)
		{
			if (item.AddRepairKit(repairKit))
			{
				return;
			}
		}
		_E155.Insert(0, _E153.CreateRepairKitsCollection(repairKit));
	}

	private void _E004()
	{
		Vector2 degradationValues = CurrentRepairer.GetDegradationValues(_E15A, _E085);
		float maxDurability = _E15A.MaxDurability;
		degradationValues.x = Mathf.Clamp(degradationValues.x, 0f, maxDurability);
		degradationValues.y = Mathf.Clamp(degradationValues.y, 0f, maxDurability);
		_conditionSlider.UpdateDegradationPrediction(degradationValues);
		_repairWarningStatusPanel.UpdateDegradationPrediction(degradationValues);
	}

	private static string _E005(RepairableComponent repairable, bool isRepairKit)
	{
		Vector2 repairDegradation = repairable.RepairDegradation;
		float num = (repairDegradation.x + repairDegradation.y) / 2f;
		if (repairDegradation.y - repairDegradation.x > 0.5f)
		{
			return _ED3E._E000(247650);
		}
		if (num < 0.05f)
		{
			return _ED3E._E000(247696);
		}
		if (num < 0.11f)
		{
			return _ED3E._E000(247690);
		}
		if (num < 0.19f)
		{
			return _ED3E._E000(247687);
		}
		if (num < 0.32f)
		{
			return _ED3E._E000(247743);
		}
		if (num < 0.55f)
		{
			return _ED3E._E000(247739);
		}
		if (!(num < 0.85f))
		{
			return _ED3E._E000(247650);
		}
		return _ED3E._E000(247724);
	}

	private void _E006(int index)
	{
		CurrentRepairer = this._E003.ElementAt(index);
		_E000();
		double repairPriceCoefficient = CurrentRepairer.GetRepairPriceCoefficient(_E085);
		_E15B.Dispose();
		CurrentRepairer.GetAndAssignAvatar(_avatar, _E15B.CancellationToken).HandleExceptions();
		_repairerStanding.text = CurrentRepairer.Standing.ToString(_ED3E._E000(253692));
		_repairSpeed.text = string.Format(_ED3E._E000(247642), _ED3E._E000(247717).Localized(), _ED3E._E000(247773).Localized());
		_repairTime.text = _ED3E._E000(27314);
		_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.ExceptionRepairItem, !_E009(CurrentRepairer, _E085, CurrentRepairer.Targets));
		_E8CF currentRepairer = CurrentRepairer;
		if (currentRepairer == null)
		{
			return;
		}
		if (!(currentRepairer is _E3C6 obj))
		{
			if (currentRepairer is _E8B2)
			{
				_repairability.text = string.Format(_ED3E._E000(247642), (_ED3E._E000(247762) + _E005(_E15A, isRepairKit: false)).Localized(), _ED3E._E000(247745).Localized());
				_E004();
				UpdateTraderLabels();
				_repairPriceRate.text = _E00C(repairPriceCoefficient);
				_repairQuality.text = string.Format(_ED3E._E000(247642), (_ED3E._E000(247799) + _E00D(CurrentRepairer.GetRepairQuality(_E085))).Localized(), _ED3E._E000(247784).Localized());
				_enhancementChance.text = string.Empty;
				_rankPanel.Show(CurrentRepairer.LoyaltyLevel, CurrentRepairer.MaxLoyaltyLevel);
				_repairerStandingGameObject.gameObject.SetActive(value: true);
				_cashInStashIcon.sprite = _E157;
				_sumToPayIcon.sprite = _E157;
				_cashInStashLabel.LocalizationKey = _ED3E._E000(245807);
				_sumToPayLabel.LocalizationKey = _ED3E._E000(245855);
			}
		}
		else
		{
			_E3C6 obj3 = obj;
			_repairability.text = string.Format(_ED3E._E000(247642), (_ED3E._E000(247762) + _E005(_E15A, isRepairKit: true)).Localized(), _ED3E._E000(247745).Localized());
			_E004();
			UpdateRepairKitLabels();
			_repairPriceRate.text = _E00C(repairPriceCoefficient);
			_repairQuality.text = string.Format(_ED3E._E000(247642), (_ED3E._E000(247799) + _E00D(obj3.GetRepairQualityForDisplay(_E085))).Localized(), _ED3E._E000(247784).Localized());
			_rankPanel.HideGameObject();
			_repairerStandingGameObject.gameObject.SetActive(value: false);
			_cashInStashIcon.sprite = _repairPointsInStashSprite;
			_sumToPayIcon.sprite = _repairPointsToPaySprite;
			_cashInStashLabel.LocalizationKey = _ED3E._E000(245791);
			_sumToPayLabel.LocalizationKey = _ED3E._E000(245760);
		}
	}

	public void UpdateRepairKitLabels()
	{
		if (CurrentRepairer is _E3C6 obj)
		{
			_repairDp.text = Math.Round(this._E002, 1) + _ED3E._E000(91195) + Math.Round(RepairAmount, 1) + _ED3E._E000(245834) + (Math.Round(this._E002, 1) + Math.Round(RepairAmount, 1));
			float repairPoints = obj.GetRepairPoints();
			_cashInStash.text = repairPoints.ToString();
			_E007(obj.SelectMinRepairKit(_E158));
			double repairPrice = obj.GetRepairPrice(_conditionSlider.Value, _E085);
			bool flag = repairPrice > (double)repairPoints;
			bool flag2 = !_conditionSlider.Value.Positive();
			bool flag3 = !_E153.IsRepairUnlocked(_E085);
			bool isVisible = _E153.IsEligibleForBuff(_E085);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.NotEnoughRepairPoints, flag, isWarningPanelVisible: false);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.NothingToRepair, flag2, isWarningPanelVisible: false);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.NotEnoughMoney, hasError: false);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.BrokenItem, _E151.Base() <= 1f);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.NoCorrespondingArea, flag3);
			if (!flag && !flag2 && !flag3)
			{
				_repairWarningStatusPanel.ShowDurabilityWarning(CurrentRepairer.GetRepairQuality(_E085));
				_repairWarningStatusPanel.ShowBuffPossibilityInfo(isVisible);
			}
			else
			{
				_repairWarningStatusPanel.ShowDurabilityWarning(-1f);
				_repairWarningStatusPanel.ShowBuffPossibilityInfo(isVisible: false);
			}
			_sumToPay.text = Math.Round(repairPrice, 2).ToString(CultureInfo.InvariantCulture);
			_sumToPay.color = (flag ? _criticalColor : _defaultColor);
		}
	}

	public void UpdateTraderLabels()
	{
		if (CurrentRepairer is _E8B2)
		{
			_repairDp.text = Math.Round(this._E002, 1) + _ED3E._E000(91195) + Math.Round(RepairAmount, 1) + _ED3E._E000(245834) + (Math.Round(this._E002, 1) + Math.Round(RepairAmount, 1));
			Dictionary<ECurrencyType, int> moneySums = _EB0E.GetMoneySums(this._E000);
			_cashInStash.text = moneySums[ECurrencyType.RUB].ToString();
			_E008();
			int num = _E00A(CurrentRepairer, _conditionSlider.Value);
			bool flag = num > moneySums[ECurrencyType.RUB];
			bool flag2 = !_conditionSlider.Value.Positive();
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.NotEnoughMoney, flag, isWarningPanelVisible: false);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.NothingToRepair, flag2, isWarningPanelVisible: false);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.NoCorrespondingArea, hasError: false);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.NotEnoughRepairPoints, hasError: false);
			_repairWarningStatusPanel.SetCriticalCondition(ERepairStatusWarning.BrokenItem, _E151.Base() <= 1f);
			_repairWarningStatusPanel.ShowDurabilityWarning((!flag && !flag2) ? CurrentRepairer.GetRepairQuality(_E085) : (-1f));
			_repairWarningStatusPanel.ShowBuffPossibilityInfo(isVisible: false);
			_sumToPay.text = num.ToString();
			_sumToPay.color = (flag ? _criticalColor : _defaultColor);
		}
	}

	private void _E007(_EA8D repairKit)
	{
		if (repairKit == null)
		{
			_E008();
			return;
		}
		float resource = repairKit.Resource;
		int maxRepairResource = repairKit.MaxRepairResource;
		float num = resource / (float)maxRepairResource;
		string text = _ED3E._E000(245824);
		double num2 = (((double)num > 0.5) ? Math.Floor(resource) : Math.Ceiling(resource));
		_repairKitChargeText.SetText(string.Format(_ED3E._E000(245880), text, num2, text, maxRepairResource));
	}

	private void _E008()
	{
		_repairKitChargeText.SetText("");
	}

	private bool _E009(_E8CF repairer, Item item, string[] excludedCategories)
	{
		if (repairer != null)
		{
			if (repairer is _E8B2)
			{
				return !ItemFilter.CheckItem(item, excludedCategories);
			}
			_E3C6 obj2;
			if ((obj2 = repairer as _E3C6) == null)
			{
				throw new NotImplementedException(_ED3E._E000(245902));
			}
		}
		return ItemFilter.CheckItem(item, excludedCategories);
	}

	public void UpdateConditionSlider()
	{
		_conditionSlider.SetSliderValues();
	}

	private int _E00A(_E8CF repairer, float count)
	{
		return (int)Math.Floor(((double)this._E001 + (double)this._E001 * repairer.GetRepairPriceCoefficient(_E085) / 100.0) * (double)count * (double)repairer.CurrencyCoefficient);
	}

	private string _E00B(double chance)
	{
		string text = _ED3E._E000(245935);
		text = ((chance < 0.25) ? (text + _ED3E._E000(245978)) : ((chance < 0.5) ? (text + _ED3E._E000(245971)) : ((!(chance < 0.75)) ? (text + _ED3E._E000(245959)) : (text + _ED3E._E000(245967)))));
		return text.Localized();
	}

	private string _E00C(double priceRate)
	{
		if (priceRate >= 100.0)
		{
			return _ED3E._E000(246012).Localized();
		}
		if (80.0 <= priceRate && priceRate < 100.0)
		{
			return _ED3E._E000(246005).Localized();
		}
		if (50.0 <= priceRate && priceRate < 80.0)
		{
			return _ED3E._E000(246001).Localized();
		}
		if (30.0 <= priceRate && priceRate < 50.0)
		{
			return _ED3E._E000(245998).Localized();
		}
		if (priceRate < 30.0)
		{
			return _ED3E._E000(245995).Localized();
		}
		return _ED3E._E000(246045).Localized();
	}

	private string _E00D(float quality)
	{
		if (quality >= 1.1f)
		{
			return _ED3E._E000(247724);
		}
		if (0.9f <= quality && quality < 1.1f)
		{
			return _ED3E._E000(246037);
		}
		if (0.8f <= quality && quality < 0.9f)
		{
			return _ED3E._E000(247687);
		}
		if (0.7f <= quality && quality < 0.8f)
		{
			return _ED3E._E000(246028);
		}
		if (!(quality <= 0.7f))
		{
			return _ED3E._E000(181789).Localized();
		}
		return _ED3E._E000(246025);
	}

	[CompilerGenerated]
	private void _E00E(float arg)
	{
		UpdateTraderLabels();
		UpdateRepairKitLabels();
		_E000();
	}
}
