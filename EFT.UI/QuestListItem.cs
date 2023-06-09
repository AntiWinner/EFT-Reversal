using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class QuestListItem : UIElement
{
	[SerializeField]
	private Button _questButton;

	[SerializeField]
	private Image _typeIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _title;

	[SerializeField]
	private Image _starIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _status;

	[SerializeField]
	private Image _lockedIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _timer;

	[SerializeField]
	private Image _timerIcon;

	[SerializeField]
	private Image _selectedIcon;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private GameObject _dailyBackground;

	[SerializeField]
	private Image _scavBackground;

	[SerializeField]
	private ColorMap _backColors;

	[SerializeField]
	private ColorMap _scavBackColors;

	[SerializeField]
	private ColorMap _textColors;

	public _E933 Quest;

	private Action<QuestListItem> _onSelect;

	private bool _isSelected;

	private IEnumerator _timerWorker;

	private CancellationTokenSource _timerCancellation;

	private void Awake()
	{
		_questButton.onClick.AddListener(delegate
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonBottomBarClick);
			_onSelect?.Invoke(this);
		});
	}

	public void Init(_E933 quest, Action<QuestListItem> onSelect)
	{
		Quest = quest;
		_onSelect = onSelect;
		ShowGameObject();
		UpdateView();
		Quest.OnStatusChanged += OnQuestStatusChanged;
	}

	private void UpdateView()
	{
		_title.text = Quest.Template.Name;
		_typeIcon.sprite = EFTHardSettings.Instance.StaticIcons.QuestIconTypeSprites[Quest.IconType];
		_starIcon.gameObject.SetActive(Quest.Template.KeyQuest);
		_status.gameObject.SetActive((Quest.QuestStatus != 0 && Quest.QuestStatus != EQuestStatus.AvailableForStart && Quest.QuestStatus != EQuestStatus.FailRestartable) || !Quest.IsViewed);
		if (_status.gameObject.activeSelf)
		{
			if (Quest.IsViewed)
			{
				_status.text = ("QuestStatus" + Quest.QuestStatus).Localized();
				_status.color = _textColors["Status" + Quest.QuestStatus];
			}
			else
			{
				_status.text = "QuestIsNew".Localized();
				_status.color = _textColors["IsNew"];
			}
			if (_isSelected)
			{
				_status.color = _textColors["Selected"];
			}
		}
		_lockedIcon.gameObject.SetActive(Quest.QuestStatus == EQuestStatus.Locked && !_isSelected);
		_scavBackground.gameObject.SetActive(Quest.Template.PlayerGroup.IsScav());
		_background.color = (_isSelected ? _backColors["Selected"] : _backColors[Quest.QuestStatus.ToStringNoBox()]);
		_scavBackground.color = (_isSelected ? _scavBackColors["Selected"] : _scavBackColors[Quest.QuestStatus.ToStringNoBox()]);
		_title.color = (_isSelected ? _textColors["Selected"] : _textColors["Title"]);
		_selectedIcon.gameObject.SetActive(_isSelected);
		_timer.color = (_isSelected ? _textColors["Selected"] : _textColors["Timer"]);
		_timerIcon.color = (_isSelected ? _textColors["Selected"] : Color.white);
		_lockedIcon.color = (_isSelected ? _textColors["Selected"] : Color.white);
		_timer.gameObject.SetActive(value: true);
		_timerIcon.gameObject.SetActive(value: true);
		UpdateDailyView(Quest as _E931, _isSelected);
		PrepareTimer();
	}

	private void PrepareTimer()
	{
		StopTimer();
		_timerCancellation = new CancellationTokenSource();
		UpdateTimer(_timerCancellation.Token).HandleExceptions();
	}

	private void UpdateDailyView(_E931 daily, bool isSelected)
	{
		if (daily == null)
		{
			return;
		}
		if (!isSelected)
		{
			switch (daily.QuestStatus)
			{
			case EQuestStatus.AvailableForStart:
				_timer.color = _textColors["IsNew"];
				break;
			case EQuestStatus.Started:
			case EQuestStatus.AvailableForFinish:
				_timer.color = _textColors["StatusStarted"];
				break;
			case EQuestStatus.Success:
				_timer.color = _textColors["StatusAvailableForFinish"];
				break;
			}
		}
		_timerIcon.gameObject.SetActive(value: false);
		_dailyBackground.SetActive(value: true);
	}

	public void SetSelected(bool selected)
	{
		_isSelected = selected;
		UpdateView();
	}

	private void OnQuestStatusChanged(_E933 quest, bool notify)
	{
		UpdateView();
	}

	private async Task UpdateTimer(CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			if ((!HandleDailyTimer(Quest as _E931) && !HandleDefaultTimer(Quest.FailTime)) || token.IsCancellationRequested)
			{
				StopTimer();
				_timer.gameObject.SetActive(value: false);
				_timerIcon.gameObject.SetActive(value: false);
				break;
			}
			await Task.Delay(TimeSpan.FromSeconds(1.0), token);
		}
	}

	private bool HandleDailyTimer(_E931 dailyQuest)
	{
		if (dailyQuest == null)
		{
			return false;
		}
		if (!dailyQuest.NeedCountdown)
		{
			return false;
		}
		if (dailyQuest.ExpirationDate - _E5AD.UtcNowUnixInt < 0)
		{
			return false;
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(dailyQuest.ExpirationDate - _E5AD.UtcNowUnixInt);
		if (_timer != null)
		{
			_timer.SetMonospaceText(timeSpan.DailyQuestFormat());
		}
		return true;
	}

	private bool HandleDefaultTimer(int timeToFail)
	{
		if (timeToFail <= 0 || Quest.QuestStatus != EQuestStatus.Started)
		{
			return false;
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(timeToFail);
		_timer.SetMonospaceText(timeSpan.DailyQuestFormat());
		return true;
	}

	private void StopTimer()
	{
		if (_timerCancellation != null)
		{
			_timerCancellation?.Cancel();
			_timerCancellation?.Dispose();
			_timerCancellation = null;
		}
	}

	public override void Close()
	{
		Quest.OnStatusChanged -= OnQuestStatusChanged;
		StopTimer();
		base.Close();
	}
}
