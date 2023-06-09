using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class NotesTask : UIElement
{
	private const string _E195 = "default";

	[SerializeField]
	private QuestProgressView _progressView;

	[SerializeField]
	private List<Image> _backgroundImages;

	[SerializeField]
	private List<TextMeshProUGUI> _labels;

	[SerializeField]
	private Image _statusBackground;

	[SerializeField]
	private Image _traderAvatar;

	[SerializeField]
	private Image _typeIcon;

	[SerializeField]
	private TextMeshProUGUI _taskLabel;

	[SerializeField]
	private TextMeshProUGUI _statusLabel;

	[SerializeField]
	private TextMeshProUGUI _locationLabel;

	[SerializeField]
	private TextMeshProUGUI _timerLabel;

	[SerializeField]
	private ColorMap _colorMap;

	[SerializeField]
	private float _notAvailableAlphaMultiplier = 0.5f;

	[SerializeField]
	private GameObject[] _dailyQuestObjects;

	[SerializeField]
	private Image ScavBackground;

	[CompilerGenerated]
	private bool _E196;

	private readonly Dictionary<TextMeshProUGUI, Color> _E197 = new Dictionary<TextMeshProUGUI, Color>();

	private NotesTaskDescriptionShort _E198;

	private _E933 _E199;

	private _E796 _E17F;

	private _EAE6 _E19A;

	private _E935 _E19B;

	private IEnumerator _E19C;

	private bool _E000
	{
		[CompilerGenerated]
		get
		{
			return _E196;
		}
		[CompilerGenerated]
		set
		{
			_E196 = value;
		}
	}

	private void Awake()
	{
		foreach (TextMeshProUGUI label in _labels)
		{
			_E197.Add(label, label.color);
		}
		GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E000();
			}
			else
			{
				_E001();
			}
		});
	}

	internal void Show(_E933 quest, _E796 session, _EAE6 controller, _E935 questController, NotesTaskDescriptionShort description, bool availability)
	{
		ShowGameObject();
		if (!quest.NecessaryConditions.Any())
		{
			Debug.LogWarning(_ED3E._E000(258279));
			return;
		}
		_E199 = quest;
		_E17F = session;
		_E19A = controller;
		_E19B = questController;
		_typeIcon.sprite = EFTHardSettings.Instance.StaticIcons.QuestIconTypeSprites[quest.IconType];
		_taskLabel.text = quest.Template.Name;
		_locationLabel.text = (quest.Template.LocationId + _ED3E._E000(70087)).Localized();
		_progressView.Show(quest);
		_E198 = description;
		this._E000 = availability;
		switch (quest.QuestStatus)
		{
		case EQuestStatus.Started:
			_statusBackground.color = _E004(_ED3E._E000(258306));
			_statusLabel.text = _ED3E._E000(258361).Localized();
			_statusLabel.color = _E004(_ED3E._E000(258340));
			break;
		case EQuestStatus.AvailableForFinish:
			_statusBackground.color = _E004(_ED3E._E000(258392));
			_statusLabel.text = _ED3E._E000(258385).Localized();
			_statusLabel.color = _E004(_ED3E._E000(258428));
			break;
		case EQuestStatus.MarkedAsFailed:
			_statusBackground.color = _E004(_ED3E._E000(192542));
			_statusLabel.text = _ED3E._E000(258418).Localized();
			_statusLabel.color = _E004(_ED3E._E000(258402));
			break;
		case EQuestStatus.Locked:
		case EQuestStatus.AvailableForStart:
		case EQuestStatus.Success:
		case EQuestStatus.Fail:
			Debug.LogError(_ED3E._E000(258454));
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		_E002(_E004(_ED3E._E000(64467)));
		_E003(selected: false);
		_E931 obj = quest as _E931;
		bool flag = obj != null;
		if (flag && obj.NeedCountdown)
		{
			_timerLabel.gameObject.SetActive(value: true);
			_E19C = this.StartBehaviourTimer(1f, repeatable: true, _E005);
			_E005();
		}
		else
		{
			_timerLabel.gameObject.SetActive(value: false);
		}
		GameObject[] dailyQuestObjects = _dailyQuestObjects;
		for (int i = 0; i < dailyQuestObjects.Length; i++)
		{
			dailyQuestObjects[i].SetActive(flag);
		}
		ScavBackground.gameObject.SetActive(quest.Template.PlayerGroup.IsScav());
		ScavBackground.color = _E004(_ED3E._E000(258467));
		Singleton<_E5CB>.Instance.TradersSettings[_E199.Template.TraderId].GetAndAssignAvatar(_traderAvatar, base.CancellationToken).HandleExceptions();
	}

	private void _E000()
	{
		_E002(_E004(_ED3E._E000(258519)));
		ScavBackground.color = _E004(_ED3E._E000(258504));
		_E003(selected: true);
		_E198.transform.SetParent(base.transform);
		_E198.Show(_E199, _E19A, _E19B, _E17F);
	}

	private void _E001()
	{
		_E002(_E004(_ED3E._E000(64467)));
		ScavBackground.color = _E004(_ED3E._E000(258467));
		_E003(selected: false);
		if (_E198.transform.IsChildOf(base.transform))
		{
			_E198.Close();
		}
	}

	private void _E002(Color color)
	{
		foreach (Image backgroundImage in _backgroundImages)
		{
			backgroundImage.color = color;
		}
	}

	private void _E003(bool selected)
	{
		foreach (TextMeshProUGUI label in _labels)
		{
			Color color = (selected ? Color.black : (this._E000 ? _E197[label] : (_E197[label] * Mathf.Clamp01(_notAvailableAlphaMultiplier))));
			label.color = color;
		}
	}

	private Color _E004(string key)
	{
		Color result = _colorMap[key];
		if (!this._E000)
		{
			result.a *= Mathf.Clamp01(_notAvailableAlphaMultiplier);
		}
		return result;
	}

	private void _E005()
	{
		if (_E199 is _E931 obj && obj.NeedCountdown)
		{
			int num = obj.ExpirationDate - _E5AD.UtcNowUnixInt;
			if (num > 0)
			{
				_timerLabel.text = new TimeSpan(0, 0, num).DailyQuestFormat();
				return;
			}
		}
		this.StopBehaviourTimer(ref _E19C);
		_E006();
	}

	private void _E006()
	{
		_statusBackground.color = _E004(_ED3E._E000(192542));
		_statusLabel.text = _ED3E._E000(258418).Localized();
		_statusLabel.color = _E004(_ED3E._E000(258402));
		_timerLabel.gameObject.SetActive(value: false);
	}

	public override void Close()
	{
		if (_E198.transform.IsChildOf(base.transform))
		{
			_E198.Close();
		}
		_progressView.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E007(bool arg)
	{
		if (arg)
		{
			_E000();
		}
		else
		{
			_E001();
		}
	}
}
