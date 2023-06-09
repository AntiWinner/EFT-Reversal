using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.Quests;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class QuestView : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _title;

	[SerializeField]
	private TextMeshProUGUI _status;

	[SerializeField]
	private GameObject _rewardListPrefab;

	[SerializeField]
	private GameObject _initialsContainer;

	[SerializeField]
	private GameObject _rewardsContainer;

	[SerializeField]
	private NotesTaskDescription _descriptionPanel;

	[SerializeField]
	private DelayTypeWindow _delayTypeWindow;

	[SerializeField]
	private QuestRequirementsView _requirementsBlock;

	[SerializeField]
	private QuestObjectivesView _objectivesBlock;

	[SerializeField]
	private SpriteMap _buttonIcons;

	[SerializeField]
	private DefaultUIButton _button;

	[SerializeField]
	private DefaultUIButton _buttonReRoll;

	[SerializeField]
	private TextMeshProUGUI _buttonTitle;

	private MultiLineTooltip _multiLineTooltip;

	private _E935 _questController;

	private _E933 _quest;

	private _E796 _backendSession;

	private _E8B2 _trader;

	private bool _performingAction;

	private _EAE6 _itemController;

	private Dictionary<ECurrencyType, int> _moneySums = new Dictionary<ECurrencyType, int>();

	private float _charismaDiscount;

	private Queue<_E937> _newQuestsQueue = new Queue<_E937>();

	public string QuestId => _quest?.Id;

	public void Show(_E796 backendSession, _EAE6 controller, _E935 questController, _E933 quest, _E8B2 trader)
	{
		_trader = trader;
		_quest = quest;
		_questController = questController;
		_backendSession = backendSession;
		_itemController = controller;
		_multiLineTooltip = ItemUiContext.Instance.MultiLineTooltip;
		_charismaDiscount = 1f - (float)backendSession.Profile?.Skills.CharismaDailyQuestsRerollDiscount;
		UI.AddDisposable(delegate
		{
			_multiLineTooltip.Close();
		});
		_quest.OnStatusChanged += OnQuestStatusChanged;
		UpdateView();
	}

	private void OnQuestStatusChanged(_E933 quest, bool notify)
	{
		UpdateView();
	}

	private void UpdateView()
	{
		_descriptionPanel.Show(_quest, _backendSession);
		ShowButtonBlock();
		if (_quest.QuestStatus == EQuestStatus.AvailableForStart || _quest.QuestStatus == EQuestStatus.FailRestartable)
		{
			_requirementsBlock.Close();
			_objectivesBlock.Close();
			ShowObjectivesBlock(_quest is _E931);
			_rewardsContainer.gameObject.SetActive(value: false);
			_initialsContainer.gameObject.SetActive(value: false);
		}
		else
		{
			ShowRequirementsBlock(_quest.QuestStatus == EQuestStatus.Locked);
			ShowObjectivesBlock(_quest.QuestStatus != EQuestStatus.Locked);
			ShowInitialBlock(_quest.QuestStatus != 0 && _quest.Template.Rewards[EQuestStatus.Started].Count > 0);
			ShowRewardsBlock(_quest.QuestStatus != 0 && _quest.Template.Rewards[EQuestStatus.Success].Count > 0);
		}
	}

	private void ShowButtonBlock()
	{
		_buttonTitle.text = _title.text + ((_quest.QuestStatus != EQuestStatus.AvailableForStart && _quest.QuestStatus != EQuestStatus.FailRestartable) ? (" (" + _status.text + ")") : "");
		_button.gameObject.SetActive(value: true);
		_button.OnClick.RemoveAllListeners();
		_buttonReRoll.gameObject.SetActive(_quest.IsChangeAllowed);
		_buttonReRoll.Interactable = IsChangeRequirementsSatisfied();
		UI.SubscribeEvent(_buttonReRoll.OnMouseOver, ShowReRollTooltip);
		UI.SubscribeEvent(_buttonReRoll.OnMouseOut, _multiLineTooltip.HideGameObject);
		UI.SubscribeEvent(_buttonReRoll.OnClick, delegate
		{
			ShowChangeQuestConfirmation().HandleExceptions();
		});
		switch (_quest.QuestStatus)
		{
		case EQuestStatus.AvailableForFinish:
			_button.OnClick.AddListener(FinishQuest);
			_button.SetIcon(_buttonIcons["done"]);
			_button.SetHeaderText("Complete", 36);
			break;
		case EQuestStatus.AvailableForStart:
			_button.OnClick.AddListener(StartQuest);
			_button.SetIcon(_buttonIcons["accept"]);
			_button.SetHeaderText("Accept", 36);
			break;
		case EQuestStatus.FailRestartable:
			_button.OnClick.AddListener(StartQuest);
			_button.SetIcon(_buttonIcons["accept"]);
			_button.SetHeaderText("Restart", 36);
			break;
		default:
			_button.gameObject.SetActive(value: false);
			break;
		}
	}

	private bool IsChangeRequirementsSatisfied()
	{
		if (!_quest.IsChangeAllowed)
		{
			return false;
		}
		_E938 changeRequirement = QuestChangeRequirement();
		if (changeRequirement.ChangeCosts.All(CheckChangeItemsRequirement))
		{
			return CheckChangeStandingRequirement(changeRequirement);
		}
		return false;
	}

	private _E938 QuestChangeRequirement()
	{
		return _E93B.Instance.GetChangeRequirements(_quest.Template.Id);
	}

	private bool CheckChangeStandingRequirement(_E938 changeRequirement)
	{
		return _trader.Info.Standing >= changeRequirement.ChangeStandingCost;
	}

	private bool CheckChangeItemsRequirement(_E939 questChangeCost)
	{
		return GetTotalCurrency(questChangeCost) >= GetDiscountedPrice(questChangeCost.Count);
	}

	private int GetTotalCurrency(_E939 questChangeCost)
	{
		if (_EA10.TryGetCurrencyType(questChangeCost.TemplateId, out var type))
		{
			_moneySums = _EB0E.GetMoneySums(_itemController.Inventory.Stash.Grid.ContainedItems.Keys);
			return _moneySums[type];
		}
		Debug.LogError("Invalid currency template " + questChangeCost.TemplateId);
		return 0;
	}

	private async Task ShowChangeQuestConfirmation()
	{
		if (!_quest.IsChangeAllowed || _performingAction)
		{
			return;
		}
		_performingAction = true;
		_EC7A arguments = new _EC7A(_quest as _E931, _quest.Template.ChangeQuestText, new _EC80(QuestChangeRequirement(), _charismaDiscount));
		_EC7B context = ItemUiContext.Instance.ShowChangeQuestConfirm(arguments);
		Task acceptTask = context.AcceptResult;
		await Task.WhenAny(context.WindowResult, acceptTask);
		if (acceptTask.Status == TaskStatus.RanToCompletion)
		{
			if ((await _backendSession.QuestChange(_quest.Id)).Succeed)
			{
				ExpireQuestLocal();
			}
			context.Close();
		}
		_performingAction = false;
	}

	private void ExpireQuestLocal()
	{
		_E931 obj = _quest as _E931;
		obj.SetStatus(EQuestStatus.Expired, notify: true, fromServer: false);
		obj.SetExpired();
	}

	private void ShowReRollTooltip()
	{
		if (_quest.IsChangeAllowed)
		{
			_multiLineTooltip.Show(QuestChangePriceInfos());
		}
	}

	private _EC80 QuestChangePriceInfos()
	{
		_E938 obj = QuestChangeRequirement();
		string standing = string.Empty;
		int num = 0;
		int charismaNewPrice = 0;
		ECurrencyType currencyType = ECurrencyType.RUB;
		int totalCurrency = 0;
		if (obj.ChangeCosts != null && obj.ChangeCosts.Any())
		{
			_E939[] changeCosts = obj.ChangeCosts;
			for (int i = 0; i < changeCosts.Length; i++)
			{
				_E939 questChangeCost = changeCosts[i];
				if (_EA10.TryGetCurrencyType(questChangeCost.TemplateId, out var type))
				{
					currencyType = type;
					num = questChangeCost.Count;
					charismaNewPrice = GetDiscountedPrice(num);
					totalCurrency = GetTotalCurrency(questChangeCost);
				}
			}
		}
		if (obj.ChangeStandingCost > 0.0)
		{
			standing = obj.ChangeStandingCost.ToString("-0.##");
		}
		return new _EC80(currencyType, ECharismaDiscountType.QuestRerollCharismaDiscount, num, charismaNewPrice, totalCurrency, standing);
	}

	private int GetDiscountedPrice(int price)
	{
		return (int)((float)price * _charismaDiscount);
	}

	private void ShowInitialBlock(bool show)
	{
		_initialsContainer.DestroyAllChildren(onlyActive: true);
		_initialsContainer.gameObject.SetActive(show);
		if (show)
		{
			_initialsContainer.InstantiatePrefab<QuestRewardList>(_rewardListPrefab).Init("QuestInitialsEquipment".Localized(), _quest.Template.Rewards[EQuestStatus.Started], _quest.QuestStatus >= EQuestStatus.Started);
		}
	}

	private void ShowRewardsBlock(bool show)
	{
		_rewardsContainer.DestroyAllChildren(onlyActive: true);
		_rewardsContainer.gameObject.SetActive(show);
		if (show)
		{
			QuestRewardList questRewardList = _rewardsContainer.InstantiatePrefab<QuestRewardList>(_rewardListPrefab);
			string info = null;
			switch (_quest.QuestStatus)
			{
			case EQuestStatus.AvailableForFinish:
				info = "QuestRecieveRequared".Localized();
				break;
			case EQuestStatus.Success:
				info = "QuestRewardsRecieved".Localized();
				break;
			}
			questRewardList.Init("QuestRewardsTitle".Localized(), _quest.Template.Rewards[EQuestStatus.Success], _quest.QuestStatus == EQuestStatus.AvailableForFinish || _quest.QuestStatus == EQuestStatus.Success, info);
			List<_E936> list = _quest.Template.Rewards[EQuestStatus.Fail];
			if (list.Count > 0)
			{
				_rewardsContainer.InstantiatePrefab<QuestRewardList>(_rewardListPrefab).Init("<color=red>" + "Penalties".Localized() + "</color>", list, _quest.QuestStatus == EQuestStatus.AvailableForFinish || _quest.QuestStatus == EQuestStatus.Success);
			}
		}
	}

	private void ShowRequirementsBlock(bool show)
	{
		_requirementsBlock.gameObject.SetActive(show);
		if (show)
		{
			_requirementsBlock.Show(_quest, _quest.Template.Conditions[EQuestStatus.AvailableForStart]);
		}
	}

	private void ShowObjectivesBlock(bool show)
	{
		_objectivesBlock.gameObject.SetActive(show);
		if (show)
		{
			_objectivesBlock.Show(_questController, _itemController, _quest.Template.Conditions[EQuestStatus.AvailableForFinish], _quest);
		}
	}

	private void StartQuest()
	{
		StartQuest(_quest).HandleExceptions();
	}

	private async Task StartQuest(_E933 selectedQuest)
	{
		if (_performingAction)
		{
			return;
		}
		_performingAction = true;
		_questController.OnNewQuestsAdded += LinkedQuestHandler;
		IResult result = await _questController.AcceptQuest(selectedQuest, runNetworkTransaction: true);
		_questController.OnNewQuestsAdded -= LinkedQuestHandler;
		_performingAction = false;
		if (result.Failed)
		{
			if (EBackendErrorCode.NoFreeSpaceForRewards.EqualsToInt(result.ErrorCode))
			{
				Debug.LogError("(no free space) failed to change quest status to started: " + selectedQuest.Id);
			}
			else
			{
				Debug.LogError("fail to change quest status to started: " + selectedQuest.Id);
			}
		}
	}

	private void LinkedQuestHandler(_E933 quest)
	{
		_newQuestsQueue.Enqueue(quest.Template);
		ShowQuestMessagesRecursively();
	}

	private void FinishQuest()
	{
		FinishQuest(_quest).HandleExceptions();
	}

	private async Task FinishQuest(_E933 quest)
	{
		if (!_performingAction)
		{
			_performingAction = true;
			_ECD8<_EB52> obj = await _questController.FinishQuest(quest, runNetworkTransaction: true);
			_performingAction = false;
			if (!obj.Failed)
			{
				_E937 template = quest.Template;
				ShowQuestMessage(template, template.SuccessMessageText);
			}
		}
	}

	private void ShowQuestMessage(_E937 questTemplate, string message, Action callback = null)
	{
		_delayTypeWindow.Show(questTemplate.Name, message, questTemplate.TraderId, delegate
		{
			_delayTypeWindow.FinishAnimating();
			callback?.Invoke();
		});
	}

	private void ShowQuestMessagesRecursively()
	{
		if (_newQuestsQueue.Any())
		{
			_E937 obj = _newQuestsQueue.Dequeue();
			ShowQuestMessage(obj, obj.Description, ShowQuestMessagesRecursively);
		}
	}

	public override void Close()
	{
		_objectivesBlock.Close();
		_requirementsBlock.Close();
		_descriptionPanel.Close();
		if (_quest != null)
		{
			_quest.OnStatusChanged -= OnQuestStatusChanged;
		}
		base.Close();
	}
}
