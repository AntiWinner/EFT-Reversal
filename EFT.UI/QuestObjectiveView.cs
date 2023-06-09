using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.Quests;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class QuestObjectiveView : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E933 quest;

		public _E935 questController;

		public _EAE6 controller;

		internal void _E000(Condition item, QuestObjectiveView view)
		{
			view.Show(quest, item, questController, controller, EFTHardSettings.Instance.StaticIcons.GetQuestIcon(item), item.IsNecessary, childCondition: true);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public TaskCompletionSource<Item[]> taskSource;

		internal void _E000()
		{
			taskSource.SetResult(null);
		}
	}

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TextMeshProUGUI _description;

	[SerializeField]
	private Image _progress;

	[SerializeField]
	private GameObject _progressBlock;

	[SerializeField]
	private DefaultUIButton _handoverButton;

	[SerializeField]
	private Image _doneIcon;

	[SerializeField]
	private TextMeshProUGUI _counter;

	[SerializeField]
	private Image _back;

	[SerializeField]
	private ColorMap _backColors;

	[SerializeField]
	private HandoverQuestItemsWindow _handoverItemsWindow;

	[SerializeField]
	private QuestObjectiveView _childObjectivePrefab;

	[SerializeField]
	private RectTransform _container;

	private _E933 _E199;

	private Condition _E1B6;

	private _E928 _E1B7;

	private Profile _E0B7;

	private _E935 _E19B;

	private _EC71<Condition, QuestObjectiveView> _E07E;

	private bool _E1B8;

	private _EAE6 _E0A3;

	private void Awake()
	{
		if (_handoverButton != null)
		{
			_handoverButton.OnClick.AddListener(_E001);
		}
	}

	public void Show(_E933 quest, Condition condition, _E935 questController, _EAE6 controller, [CanBeNull] Sprite icon, bool isNecessary, bool childCondition = false)
	{
		ShowGameObject();
		_E199 = quest;
		_E1B6 = condition;
		_E19B = questController;
		_E0B7 = _E19B.Profile;
		_E0A3 = controller;
		_E1B7 = _E199.ConditionHandlers[_E1B6];
		_counter.gameObject.SetActive(value: false);
		_progressBlock.gameObject.SetActive(value: false);
		_E1B7.OnConditionChanged += _E000;
		_E07E?.Dispose();
		if (_childObjectivePrefab != null)
		{
			_E07E = UI.AddDisposable(new _EC71<Condition, QuestObjectiveView>(condition.ChildConditions.BindWhere(quest.CheckVisibilityStatus), _childObjectivePrefab, _container, delegate(Condition item, QuestObjectiveView view)
			{
				view.Show(quest, item, questController, controller, EFTHardSettings.Instance.StaticIcons.GetQuestIcon(item), item.IsNecessary, childCondition: true);
			}));
		}
		_description.fontSize = (isNecessary ? 18 : 16);
		_counter.fontSize = (isNecessary ? 18 : 16);
		_description.fontStyle = (isNecessary ? FontStyles.Bold : FontStyles.Normal);
		_counter.fontStyle = _description.fontStyle;
		_description.text = (childCondition ? (_ED3E._E000(261680).Localized() + _ED3E._E000(18502)) : "") + _E1B6.FormattedDescription;
		_icon.sprite = icon;
		_icon.gameObject.SetActive(icon != null);
		if (childCondition)
		{
			_progressBlock.SetActive(value: false);
		}
		_E000(_E1B7);
	}

	private void _E000(_E928 conditionHandler)
	{
		if (_back == null || _doneIcon == null)
		{
			return;
		}
		EQuestStatus questStatus = _E199.QuestStatus;
		bool flag = _E199.IsConditionDone(_E1B6);
		_back.color = ((flag && questStatus != EQuestStatus.Success && questStatus != EQuestStatus.Fail) ? _backColors[_ED3E._E000(56108)] : _backColors[_ED3E._E000(261675)]);
		_doneIcon.gameObject.SetActive(flag);
		if (_handoverButton != null)
		{
			Condition condition = conditionHandler.Condition;
			if (condition != null)
			{
				if (!(condition is ConditionHandoverItem conditionHandoverItem))
				{
					if (condition is ConditionWeaponAssembly conditionWeaponAssembly)
					{
						ConditionWeaponAssembly conditionWeaponAssembly2 = conditionWeaponAssembly;
						int count = _E0B7.Inventory.GetWeaponAssembly(conditionWeaponAssembly2, displayLog: true).Count;
						_handoverButton.gameObject.SetActive(_E199.QuestStatus == EQuestStatus.Started && !flag && (float)count >= conditionWeaponAssembly2.value);
					}
				}
				else
				{
					ConditionHandoverItem conditionHandoverItem2 = conditionHandoverItem;
					if (Singleton<_E63B>.Instance.ItemTemplates.ContainsKey(conditionHandoverItem2.target.First()))
					{
						bool active;
						if (Singleton<_E63B>.Instance.ItemTemplates[conditionHandoverItem2.target.First()] is _EA75)
						{
							Dictionary<ECurrencyType, int> moneySums = _EB0E.GetMoneySums(_E0B7.Inventory.Stash.Grid.ContainedItems.Keys);
							ECurrencyType currencyTypeById = _EA10.GetCurrencyTypeById(conditionHandoverItem2.target[0]);
							active = _E199.QuestStatus == EQuestStatus.Started && !flag && moneySums[currencyTypeById] > 0;
						}
						else
						{
							int num = _E935.GetItemsForCondition(_E0A3.Inventory, conditionHandoverItem2).Length;
							active = _E199.QuestStatus == EQuestStatus.Started && !flag && num > 0;
						}
						_handoverButton.gameObject.SetActive(active);
					}
					else
					{
						ConditionHandoverItem conditionHandoverItem3 = conditionHandoverItem;
						Debug.LogError(_ED3E._E000(261666) + conditionHandoverItem3.target.First());
						_handoverButton.gameObject.SetActive(value: false);
					}
				}
			}
		}
		bool flag2 = _E1B6 is ConditionCounterCreator || _E1B6 is ConditionFindItem || _E1B6 is ConditionHandoverItem || _E1B6 is ConditionStatisticsCounter || _E1B6 is ConditionLevel || _E1B6 is ConditionSkill || _E1B6 is ConditionExperience || _E1B6 is ConditionSellItemToTrader || _E1B6 is ConditionPlaceBeacon || _E1B6 is ConditionLeaveItemAtLocation || _E1B6 is _E351;
		_progressBlock.gameObject.SetActive(flag2 && _E1B6.value > 1f);
		_counter.gameObject.SetActive(flag2 && _E1B6.value > 1f);
		if (flag2)
		{
			float a = 0f;
			if (flag)
			{
				a = _E1B6.value;
			}
			else if (_E1B7.HasGetter())
			{
				a = _E1B7.CurrentValue;
			}
			a = Mathf.Min(a, _E1B6.value);
			_counter.text = string.Format(_ED3E._E000(182604), a, _E1B6.value);
			_progress.fillAmount = a / _E1B6.value;
		}
	}

	private void _E001()
	{
		_E002(_E199).HandleExceptions();
	}

	private async Task _E002(_E933 selectedQuest)
	{
		if (_E1B8)
		{
			return;
		}
		_E1B8 = true;
		Item[] array = null;
		ConditionItem conditionItem = null;
		try
		{
			Condition condition = _E1B6;
			if (condition == null)
			{
				goto IL_00cd;
			}
			if (!(condition is ConditionHandoverItem conditionHandoverItem))
			{
				if (!(condition is ConditionWeaponAssembly conditionWeaponAssembly))
				{
					goto IL_00cd;
				}
				ConditionWeaponAssembly conditionWeaponAssembly2 = conditionWeaponAssembly;
				conditionItem = conditionWeaponAssembly2;
				array = _E0B7.Inventory.GetWeaponAssembly(conditionWeaponAssembly2).ToArray();
			}
			else
			{
				ConditionHandoverItem conditionHandoverItem2 = conditionHandoverItem;
				conditionItem = conditionHandoverItem2;
				array = _E935.GetItemsForCondition(_E0A3.Inventory, conditionHandoverItem2);
			}
			if (array.IsNullOrEmpty())
			{
				Debug.LogError(_ED3E._E000(261769));
				return;
			}
			if (!array.All((Item item) => item.QuestItem || _EA10.IsCurrencyId(item.TemplateId)))
			{
				TaskCompletionSource<Item[]> taskSource = new TaskCompletionSource<Item[]>();
				_handoverItemsWindow.Show(_E1B6.value, _E1B7.CurrentValue, array, _E0B7, _E0A3 as _EAED, taskSource.SetResult, delegate
				{
					taskSource.SetResult(null);
				});
				array = await taskSource.Task;
				return;
			}
			return;
			IL_00cd:
			Debug.LogError(string.Format(_ED3E._E000(261757), _E1B6.GetType()));
		}
		finally
		{
			if (conditionItem != null && !array.IsNullOrEmpty())
			{
				await _E19B.HandoverItem(selectedQuest, conditionItem, array, runNetworkTransaction: true);
			}
			_E1B8 = false;
		}
	}

	public override void Close()
	{
		if (_E1B7 != null)
		{
			_E1B7.OnConditionChanged -= _E000;
		}
		if (_handoverItemsWindow != null && _handoverItemsWindow.gameObject.activeSelf)
		{
			_handoverItemsWindow.Close();
		}
		base.Close();
	}
}
