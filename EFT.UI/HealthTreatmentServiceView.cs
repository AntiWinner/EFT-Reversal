using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.Health;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class HealthTreatmentServiceView : ServiceView
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public HealthTreatmentServiceView _003C_003E4__this;

		public MultiLineTooltip tooltip;

		internal void _E000(PointerEventData arg)
		{
			if (_003C_003E4__this._E126 != null)
			{
				tooltip.Show(_003C_003E4__this._E126);
				_003C_003E4__this._E1E4 = true;
			}
		}

		internal void _E001(PointerEventData arg)
		{
			if (tooltip != null && tooltip.isActiveAndEnabled)
			{
				tooltip.Close();
				_003C_003E4__this._E1E4 = false;
			}
		}
	}

	private const string _E1D8 = "UI/HealthTreatmentScreen_TreatAll";

	private const string _E1D9 = "UI/HealthTreatmentScreen_TreatNone";

	private const string _E1DA = "UI/HealthTreatmentScreen_NotEnoughMoney";

	private const string _E1DB = "UI/HealthTreatmentScreen_NoTreatmentSelected";

	private const string _E1DC = "UI/HealthTreatmentScreen_Description";

	private const string _E1DD = "UI/HealthTreatmentScreen_TrialDescription";

	private static readonly List<_EC84> _E1DE = new List<_EC84>
	{
		new _EC86()
	};

	[SerializeField]
	private InventoryScreenHealthPanel _bodyPreview;

	[SerializeField]
	private RectTransform _healthConditionsContainer;

	[SerializeField]
	private RectTransform _damageList;

	[SerializeField]
	private RectTransform _nothingToHealMessage;

	[SerializeField]
	private RectTransform _loadingIndicator;

	[SerializeField]
	private HealthTreatmentFactorView _healthFactorTemplate;

	[SerializeField]
	private HealthTreatmentEffectView _healthEffectTemplate;

	[SerializeField]
	private TextMeshProUGUI _quickHealNote;

	[SerializeField]
	private TextMeshProUGUI _regenTimeField;

	[SerializeField]
	private TextMeshProUGUI _cashInStashField;

	[SerializeField]
	private TextMeshProUGUI _costTotalField;

	[SerializeField]
	private TextMeshProUGUI _treatAllField;

	[SerializeField]
	private CanvasGroup _regenTimeCanvasGroup;

	[SerializeField]
	private UpdatableToggle _selectAllToggle;

	[SerializeField]
	private DefaultUIButton _applyButton;

	private _E8B2 _E1BB;

	private Profile _E0B7;

	private _EAED _E092;

	private _E981 _E0AE;

	private _E796 _E17F;

	private int _E1DF;

	private int _E1E0;

	private bool _E1E1;

	private bool _E1E2;

	private double _E1E3;

	private bool _E1E4;

	private _EC80 _E126;

	private _ECEF<_EC8A> _E1E5 = new _ECEF<_EC8A>();

	private IEnumerable<Item> _E000 => _E092.Inventory.Stash.Grid.ContainedItems.Keys.SelectMany((Item x) => x.GetAllItems());

	private void Awake()
	{
		MultiLineTooltip tooltip = ItemUiContext.Instance.MultiLineTooltip;
		HoverTrigger orAddComponent = _costTotalField.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate
		{
			if (_E126 != null)
			{
				tooltip.Show(_E126);
				_E1E4 = true;
			}
		};
		orAddComponent.OnHoverEnd += delegate
		{
			if (tooltip != null && tooltip.isActiveAndEnabled)
			{
				tooltip.Close();
				_E1E4 = false;
			}
		};
	}

	public static string FormatRublesString(float value)
	{
		value = (int)Math.Ceiling(value);
		return value.SplitCurrencyNumber(_ED3E._E000(18502)) + _ED3E._E000(260492);
	}

	public void Preview(Profile profile, _EAED inventoryController, _E981 healthController)
	{
		ShowGameObject();
		_E000(profile, inventoryController, healthController);
		_loadingIndicator.gameObject.SetActive(value: true);
		_damageList.gameObject.SetActive(value: false);
		_nothingToHealMessage.gameObject.SetActive(value: false);
		_applyButton.Interactable = false;
	}

	public override void Show(_E8B2 trader, Profile profile, _EAED inventoryController, _E981 healthController, _E934 quests, ItemUiContext context, _E796 session)
	{
		_E1BB = trader;
		_E0B7 = profile;
		_E092 = inventoryController;
		_E0AE = healthController;
		_E17F = session;
		_E74F skills = _E0B7.Skills;
		_E1E3 = 1.0 - ((double)(float)skills.CharismaHealingDiscount + (double)(Math.Min(skills.Charisma.Level, 50) * (_E1BB.LoyaltyLevel - 1)) / 3.0 * (double)skills.Settings.Charisma.BonusSettings.LevelBonusSettings.HealthRestoreTraderDiscount);
		ShowGameObject();
		_E000(_E0B7, _E092, _E0AE);
		_E1E5.Clear();
		while (_healthConditionsContainer.childCount > 0)
		{
			UnityEngine.Object.DestroyImmediate(_healthConditionsContainer.GetChild(0).gameObject);
		}
		_selectAllToggle.UpdateValue(value: false);
		UI.AddDisposable(new _EC6F<_EC8A, HealthTreatmentView>(_E1E5, _EC8A.Comparer, (_EC8A treatmentWrapper) => treatmentWrapper.TreatmentView, (_EC8A treatmentWrapper) => _healthConditionsContainer, delegate(_EC8A treatmentWrapper, HealthTreatmentView treatmentView)
		{
			treatmentWrapper.ShowViewAction(_E0AE, _E1BB.Info, treatmentView);
		}));
		_E008(_E003(_E0B7, healthController, _E1BB.Info));
		_E007(_E002(_E0B7, healthController, _E1BB.Info));
		_selectAllToggle.onValueChanged.AddListener(_E00B);
		_applyButton.OnClick.AddListener(_E001);
		_applyButton.Interactable = true;
		_loadingIndicator.gameObject.SetActive(value: false);
		_quickHealNote.text = _ED3E._E000(260489).Localized();
		_E5CB._E025._E001 healPrice = Singleton<_E5CB>.Instance.Health.HealPrice;
		if (healPrice.IsFastHealFree(profile))
		{
			_quickHealNote.text += string.Format(_ED3E._E000(260518).Localized(), healPrice.TrialLevels, healPrice.TrialRaids);
		}
		UI.AddDisposable(delegate
		{
			_selectAllToggle.onValueChanged.RemoveListener(_E00B);
		});
		UI.AddDisposable(delegate
		{
			_applyButton.OnClick.RemoveListener(_E001);
		});
		_E005();
		_E004();
		_E00B(selected: true);
	}

	private void _E000(Profile profile, _EAED inventoryController, _E981 healthController)
	{
		UI.Dispose();
		UI.AddDisposable(_bodyPreview);
		_bodyPreview.Show(healthController, inventoryController.Inventory, profile.Skills, profile.Stats.DamageHistory);
		_E1E4 = false;
	}

	public override bool CheckAvailable(_E8B2 trader)
	{
		return trader.Settings.Medic;
	}

	private void _E001()
	{
		_E0AE.ApplyRegenerationImmediately();
		if (_E1DF > _E1E0)
		{
			return;
		}
		_E98C obj = new _E98C();
		foreach (EBodyPart realBodyPart in _E9C6.RealBodyParts)
		{
			obj.BodyParts.Add(realBodyPart, new _E98B());
		}
		float num = 0f;
		List<_EC8A> list = new List<_EC8A>();
		foreach (_EC8A item in _E1E5)
		{
			if (item.Active && item.Selected)
			{
				item.Store(obj, out var cost);
				num += cost;
				list.Add(item);
			}
		}
		foreach (_EC8A item2 in list)
		{
			item2.Apply();
		}
		int num2 = (int)Math.Ceiling((double)num * _E1E3);
		List<Item> list2 = (from item in this._E000
			where _EA10.CurrencyIndex[ECurrencyType.RUB].HasId(item.TemplateId)
			orderby item.StackObjectsCount
			select item).ToList();
		List<_E557> list3 = new List<_E557>();
		foreach (Item item3 in list2)
		{
			if (num <= 0f)
			{
				break;
			}
			_E557 obj2 = new _E557(item3, item3.StackObjectsCount);
			if (num2 >= item3.StackObjectsCount)
			{
				num2 -= item3.StackObjectsCount;
				_EB29.Remove(item3, _E092);
			}
			else
			{
				item3.StackObjectsCount -= num2;
				obj2.count = num2;
				num = 0f;
			}
			list3.Add(obj2);
		}
		_E005();
		_E17F.RestoreHealth(obj, list3, _E1BB.Settings.Id);
	}

	public static IEnumerable<_EC83> GetHealthObservers(Profile profile, _E981 healthController, Profile._E001 trader)
	{
		List<_EC83> list = new List<_EC83>();
		list.AddRange(_E002(profile, healthController, trader));
		list.AddRange(_E003(profile, healthController, trader));
		return list;
	}

	private static List<_EC89> _E002(Profile profile, _E981 healthController, Profile._E001 trader)
	{
		List<_EC89> list = new List<_EC89>();
		foreach (Type treatableEffectType in _E981.TreatableEffectTypes)
		{
			_EC89 item = new _EC89(treatableEffectType, profile, healthController, trader);
			list.Add(item);
		}
		return list;
	}

	private static List<_EC84> _E003(Profile profile, _E981 healthController, Profile._E001 trader)
	{
		List<_EC84> list = new List<_EC84>();
		foreach (_EC84 item2 in _E1DE)
		{
			_EC84 item = item2.Clone(profile, healthController, trader);
			list.Add(item);
		}
		return list;
	}

	private void _E004()
	{
		float overallHealthRegenTime = _E0AE.GetOverallHealthRegenTime();
		_regenTimeCanvasGroup.alpha = (overallHealthRegenTime.Positive() ? 1f : 0f);
		if (overallHealthRegenTime.Positive())
		{
			string text = _ED3E._E000(59488);
			if (!float.IsInfinity(overallHealthRegenTime))
			{
				text = TimeSpan.FromSeconds(overallHealthRegenTime).TraderFormat();
			}
			_regenTimeField.SetMonospaceText(text);
		}
	}

	private void _E005()
	{
		Dictionary<ECurrencyType, int> moneySums = _EB0E.GetMoneySums(_E092.Inventory.Stash.Grid.ContainedItems.Keys);
		_E1E0 = moneySums[ECurrencyType.RUB];
		_E006();
	}

	private void _E006()
	{
		bool flag = _E1DF > _E1E0;
		string text = FormatRublesString(_E1E0);
		if (flag)
		{
			text = _ED3E._E000(103088) + text + _ED3E._E000(59467);
		}
		_damageList.gameObject.SetActive(!_E1E1);
		_nothingToHealMessage.gameObject.SetActive(_E1E1);
		_applyButton.Interactable = !flag && !_E1E1 && !_E1E2;
		_cashInStashField.SetMonospaceText(text);
		string tooltip = string.Empty;
		if (flag)
		{
			tooltip = _ED3E._E000(260600);
		}
		else if (_E1E1 || _E1E2)
		{
			tooltip = _ED3E._E000(260624);
		}
		_applyButton.SetDisabledTooltip(tooltip);
	}

	private void _E007(List<_EC89> effectObservers)
	{
		foreach (_EC89 effectObserver in effectObservers)
		{
			_EC8A treatment = new _EC8A(effectObserver, _healthEffectTemplate);
			_E009(treatment);
		}
	}

	private void _E008(List<_EC84> factorObservers)
	{
		foreach (_EC84 factorObserver in factorObservers)
		{
			_EC8A treatment = new _EC8A(factorObserver, _healthFactorTemplate);
			_E009(treatment);
		}
	}

	private void _E009(_EC8A treatment)
	{
		_E1E5.Add(treatment);
		UI.AddDisposable(treatment);
		UI.AddDisposable(treatment.Subscribe(_E00C));
		UI.AddDisposable(treatment.OnSelect.Subscribe(_E00C));
		_E00C();
	}

	private void _E00A(_EC8A treatment)
	{
		_E1E5.Remove(treatment);
		treatment.Unsubscribe(_E00C);
		treatment.Dispose();
		_E00C();
	}

	private void _E00B(bool selected)
	{
		foreach (_EC8A item in _E1E5)
		{
			item.Select(selected);
		}
	}

	private void _E00C()
	{
		bool selected = true;
		_E1DF = 0;
		int num = 0;
		_E1E1 = true;
		_E1E2 = true;
		foreach (_EC8A item in _E1E5)
		{
			if (item.Active)
			{
				_E1E1 = false;
				if (item.Selected)
				{
					_E1DF += (int)Math.Ceiling((double)item.CurrentTreatmentCost * _E1E3);
					num += (int)Math.Ceiling(item.CurrentTreatmentCost);
					_E1E2 = false;
				}
				else
				{
					selected = false;
				}
			}
		}
		if (!_E1E3.ApproxEquals(1.0))
		{
			_E126 = new _EC80(ECurrencyType.RUB, ECharismaDiscountType.PostRaidHealingCharismaDiscount, num, _E1DF, _E1E0);
			if (_E1E4)
			{
				ItemUiContext.Instance.MultiLineTooltip.Show(_E126);
			}
		}
		else
		{
			_E126 = null;
		}
		_E00D(selected, sentCallback: false);
		_costTotalField.SetMonospaceText(FormatRublesString(_E1DF));
		_E006();
		_E004();
	}

	private void _E00D(bool selected, bool sentCallback)
	{
		_selectAllToggle.UpdateValue(selected, sentCallback);
		_treatAllField.text = (selected ? _ED3E._E000(260679) : _ED3E._E000(260645)).Localized();
	}

	[CompilerGenerated]
	private Transform _E00E(_EC8A treatmentWrapper)
	{
		return _healthConditionsContainer;
	}

	[CompilerGenerated]
	private void _E00F(_EC8A treatmentWrapper, HealthTreatmentView treatmentView)
	{
		treatmentWrapper.ShowViewAction(_E0AE, _E1BB.Info, treatmentView);
	}

	[CompilerGenerated]
	private void _E010()
	{
		_selectAllToggle.onValueChanged.RemoveListener(_E00B);
	}

	[CompilerGenerated]
	private void _E011()
	{
		_applyButton.OnClick.RemoveListener(_E001);
	}
}
