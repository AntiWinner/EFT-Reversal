using System;
using System.Collections.Generic;
using System.Linq;
using EFT.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class QuestsListView : UIElement
{
	private const string LIST_SHOW_LOCKED_KEY = "quests_list_ShowLocked";

	[SerializeField]
	private RectTransform _questListContainer;

	[SerializeField]
	private QuestListItem _questListItemPrefab;

	[SerializeField]
	private CustomTextMeshProUGUI _questsCounterText;

	[SerializeField]
	private Toggle _toggleShowCompleted;

	[SerializeField]
	private Toggle _toggleShowLocked;

	private _E935 _questController;

	private QuestView _questView;

	private _E796 _backendSession;

	private _EC71<_E933, QuestListItem> _questBindableViewList;

	private QuestListItem _questListItemSelected;

	private readonly List<EQuestStatus> _questListExcludeFilter = new List<EQuestStatus>();

	private _EAE6 _itemController;

	private _E8B2 _trader;

	private bool _listShowCompleted;

	private static bool? _listShowLocked;

	private static bool ListShowLocked
	{
		get
		{
			if (!_listShowLocked.HasValue)
			{
				_listShowLocked = _E762.GetBool("quests_list_ShowLocked");
			}
			return _listShowLocked.Value;
		}
		set
		{
			if (_listShowLocked != value)
			{
				_listShowLocked = value;
				_E762.SetBool("quests_list_ShowLocked", _listShowLocked.GetValueOrDefault());
			}
		}
	}

	private void Awake()
	{
		_toggleShowCompleted.onValueChanged.AddListener(OnToggleShowCompleted);
		_toggleShowLocked.onValueChanged.AddListener(OnToggleShowLocked);
	}

	public void Show(_E796 backendSession, _EAE6 controller, _E935 questController, _E8B2 trader, QuestView questView)
	{
		ShowGameObject();
		_backendSession = backendSession;
		_questController = questController;
		_questView = questView;
		_itemController = controller;
		_trader = trader;
		string traderId = trader.Id;
		_toggleShowCompleted.isOn = _listShowCompleted;
		_toggleShowLocked.isOn = ListShowLocked;
		_ED05<_E933> obj = _questController.Quests.BindWhere((_E933 x) => x.IsVisible && x.Template != null && x.Template.TraderId == traderId);
		_questBindableViewList = new _EC71<_E933, QuestListItem>(obj, _questListItemPrefab, _questListContainer, delegate(_E933 item, QuestListItem view)
		{
			if (item.QuestStatus == EQuestStatus.Locked || item.QuestStatus == EQuestStatus.Started)
			{
				item.CheckForStatusChange(fromServer: false, canFail: true);
			}
			view.Init(item, OnQuestSelected);
			int questsSuccessCount = _questController.Quests.GetQuestsSuccessCount(traderId);
			int questsCount = _questController.Quests.GetQuestsCount(traderId);
			questsCount = Math.Max(questsSuccessCount, questsCount);
			_questsCounterText.text = $"{questsSuccessCount}/{questsCount}";
		});
		_questBindableViewList.OnRemove += OnRemoveQuestFromList;
		UI.AddDisposable(_questBindableViewList);
		UpdateVisibility();
		if (!obj.Any())
		{
			_questsCounterText.text = "0/0";
		}
	}

	private void OnRemoveQuestFromList(_E933 removedQuest)
	{
		if (removedQuest.Id == _questView.QuestId)
		{
			AutoSelectQuest();
		}
	}

	private void UpdateVisibility()
	{
		if (_questBindableViewList == null)
		{
			return;
		}
		foreach (var (_, questListItem2) in _questBindableViewList)
		{
			questListItem2.gameObject.SetActive(!_questListExcludeFilter.Contains(questListItem2.Quest.QuestStatus));
		}
		if ((_questListItemSelected != null && !_questListItemSelected.gameObject.activeSelf) || _questListItemSelected == null)
		{
			AutoSelectQuest();
		}
	}

	private void AutoSelectQuest()
	{
		bool flag = false;
		using (IEnumerator<KeyValuePair<_E933, QuestListItem>> enumerator = _questBindableViewList.Where((KeyValuePair<_E933, QuestListItem> item) => item.Value.gameObject.activeSelf).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				var (_, view) = enumerator.Current;
				OnQuestSelected(view);
				flag = true;
			}
		}
		if (!flag)
		{
			_questListItemSelected = null;
			_questView.gameObject.SetActive(value: false);
		}
	}

	private void OnQuestSelected(QuestListItem view)
	{
		if (!(_questListItemSelected == view))
		{
			if (_questListItemSelected != null)
			{
				_questListItemSelected.SetSelected(selected: false);
			}
			_questListItemSelected = view;
			_questListItemSelected.SetSelected(selected: true);
			_questView.gameObject.SetActive(value: true);
			_questView.Show(_backendSession, _itemController, _questController, view.Quest, _trader);
			view.Quest.IsViewed = true;
		}
	}

	private void OnToggleShowCompleted(bool show)
	{
		_listShowCompleted = show;
		if (show)
		{
			_questListExcludeFilter.Remove(EQuestStatus.Success);
			_questListExcludeFilter.Remove(EQuestStatus.Fail);
		}
		else
		{
			_questListExcludeFilter.Add(EQuestStatus.Success);
			_questListExcludeFilter.Add(EQuestStatus.Fail);
		}
		UpdateVisibility();
	}

	private void OnToggleShowLocked(bool show)
	{
		ListShowLocked = show;
		if (show)
		{
			_questListExcludeFilter.Remove(EQuestStatus.Locked);
		}
		else
		{
			_questListExcludeFilter.Add(EQuestStatus.Locked);
		}
		UpdateVisibility();
	}

	public override void Close()
	{
		_listShowCompleted = false;
		_questBindableViewList = null;
		base.Close();
	}
}
