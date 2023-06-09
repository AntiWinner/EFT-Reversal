using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class QuestObjectivesView : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E928 condition;

		public QuestObjectivesView _003C_003E4__this;

		internal void _E000()
		{
			condition.OnConditionChanged -= _003C_003E4__this._E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public QuestObjectivesView _003C_003E4__this;

		public _E91B conditions;

		public _E933 quest;

		public Predicate<Condition> _003C_003E9__1;

		public Action<Condition, QuestObjectiveView> _003C_003E9__2;

		internal void _E000()
		{
			_003C_003E4__this._E07E?.Dispose();
			_003C_003E4__this._E07E = _003C_003E4__this.UI.AddDisposable(new _EC71<Condition, QuestObjectiveView>(conditions.BindWhere((Condition x) => !QuestObjectivesView._E002(conditions).Contains(x) && quest.CheckVisibilityStatus(x)), _003C_003E4__this._objectivePrefab, _003C_003E4__this._container, delegate(Condition condition, QuestObjectiveView view)
			{
				view.Show(quest, condition, _003C_003E4__this._E19B, _003C_003E4__this._E19A, EFTHardSettings.Instance.StaticIcons.GetQuestIcon(condition), condition.IsNecessary);
			}));
		}

		internal bool _E001(Condition x)
		{
			if (!QuestObjectivesView._E002(conditions).Contains(x))
			{
				return quest.CheckVisibilityStatus(x);
			}
			return false;
		}

		internal void _E002(Condition condition, QuestObjectiveView view)
		{
			view.Show(quest, condition, _003C_003E4__this._E19B, _003C_003E4__this._E19A, EFTHardSettings.Instance.StaticIcons.GetQuestIcon(condition), condition.IsNecessary);
		}
	}

	[SerializeField]
	private QuestObjectiveView _objectivePrefab;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private GameObject _failtyBlock;

	[SerializeField]
	private TextMeshProUGUI _timer;

	[SerializeField]
	private Image _penaltyIcon;

	private _E935 _E19B;

	private _E91B _E1B9 = new _E91B();

	private _EAE6 _E19A;

	private _E933 _E199;

	private IEnumerator _E19C;

	private _EC79<Condition, QuestObjectiveView> _E07E;

	private readonly List<Action> _E1BA = new List<Action>();

	public void Show(_E935 questController, _EAE6 itemController, _E91B conditions, _E933 quest)
	{
		ShowGameObject();
		_E19B = questController;
		_E1B9 = conditions;
		_E19A = itemController;
		_E199 = quest;
		_E001(conditions);
		_E1BA.Clear();
		foreach (Condition condition2 in conditions)
		{
			_E928 condition = _E199.ConditionHandlers[condition2];
			condition.OnConditionChanged += _E000;
			_E1BA.Add(delegate
			{
				condition.OnConditionChanged -= _E000;
			});
		}
		if (quest.QuestStatus == EQuestStatus.Started)
		{
			_E19C = this.StartBehaviourTimer(1f, repeatable: true, _E003);
		}
		_E003();
		_penaltyIcon.gameObject.SetActive(quest.Template.Rewards[EQuestStatus.Fail].Count > 0);
	}

	private void _E000(_E928 obj)
	{
		_E001(_E1B9);
	}

	private void _E001(_E91B conditions)
	{
		if (this == null || base.gameObject == null || !base.gameObject.activeSelf)
		{
			Debug.LogWarning(_ED3E._E000(261843));
			return;
		}
		_E933 quest = _E199;
		this.WaitOneFrame(delegate
		{
			_E07E?.Dispose();
			_E07E = UI.AddDisposable(new _EC71<Condition, QuestObjectiveView>(conditions.BindWhere((Condition x) => !_E002(conditions).Contains(x) && quest.CheckVisibilityStatus(x)), _objectivePrefab, _container, delegate(Condition condition, QuestObjectiveView view)
			{
				view.Show(quest, condition, _E19B, _E19A, EFTHardSettings.Instance.StaticIcons.GetQuestIcon(condition), condition.IsNecessary);
			}));
		});
	}

	private static _ED07<Condition> _E002(_E91B questConditions)
	{
		List<_ED07<Condition>> list = questConditions.Select((Condition x) => x.ChildConditions).ToList();
		_ED07<Condition> obj = new _ED07<Condition>();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			_ED07<Condition> obj2 = list[num];
			for (int num2 = obj2.Count - 1; num2 >= 0; num2--)
			{
				Condition newItem = obj2[num2];
				obj.Add(newItem);
			}
		}
		return obj;
	}

	private void _E003()
	{
		int failTime = _E199.FailTime;
		bool flag = _E199.QuestStatus != EQuestStatus.AvailableForFinish && _E199.QuestStatus != EQuestStatus.Success && failTime > 0;
		_failtyBlock.gameObject.SetActive(flag);
		if (flag)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(failTime);
			_timer.SetMonospaceText(timeSpan.DailyQuestFormat());
		}
		else
		{
			this.StopBehaviourTimer(ref _E19C);
		}
	}

	public override void Close()
	{
		foreach (Action item in _E1BA)
		{
			item();
		}
		if (_E07E != null)
		{
			_E07E.Dispose();
		}
		_E1BA.Clear();
		base.Close();
	}
}
