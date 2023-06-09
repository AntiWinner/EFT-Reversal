using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.Quests;

[Serializable]
public sealed class ConditionCounterManager
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E91F createData;

		internal bool _E000(_E920 x)
		{
			return x.Id == createData.id;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E920 counter;

		internal bool _E000(_E920 x)
		{
			return x.Id != counter.Id;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public string id;

		internal bool _E000(_E920 counter)
		{
			return counter.Id == id;
		}
	}

	private readonly _E933 _quest;

	private readonly List<_E920> _globalCounters;

	private readonly List<_E920> _counters = new List<_E920>();

	public ConditionCounterManager(_E933 quest, _E91E conditionCounters)
	{
		_quest = quest;
		_globalCounters = conditionCounters.Counters;
	}

	public void LoadConditionCounters()
	{
		if (_quest == null || (_quest.QuestStatus != EQuestStatus.Started && _quest.QuestStatus != EQuestStatus.AvailableForFinish) || _quest.CurrentStatusTransitions.Length == 0)
		{
			return;
		}
		List<_E91F> list = new List<_E91F>();
		foreach (KeyValuePair<EQuestStatus, _E91B> condition in _quest.Template.Conditions)
		{
			_E39D.Deconstruct(condition, out var _, out var value);
			foreach (Condition item in value)
			{
				if (item is ConditionCounterCreator conditionCounterCreator)
				{
					conditionCounterCreator.counter.RootCondition = conditionCounterCreator;
					list.Add(conditionCounterCreator.counter);
				}
			}
		}
		_E000(list);
	}

	private void _E000(IEnumerable<_E91F> templates)
	{
		foreach (_E91F createData in templates)
		{
			_E920 counter = _globalCounters.FirstOrDefault((_E920 x) => x.Id == createData.id);
			if (counter == null)
			{
				counter = new _E920
				{
					Id = createData.id
				};
				_globalCounters.Add(counter);
			}
			if (_counters.All((_E920 x) => x.Id != counter.Id))
			{
				_counters.Add(counter);
			}
			counter.Template = createData;
			counter.Quest = _quest;
		}
	}

	public void Clear()
	{
		foreach (_E920 counter in _counters)
		{
			_globalCounters.Remove(counter);
		}
		_counters.Clear();
	}

	[CanBeNull]
	public _E920 GetCounter(string id)
	{
		return _counters.FirstOrDefault((_E920 counter) => counter.Id == id);
	}

	[CanBeNull]
	private _E920 _E001(_E91A[] checks)
	{
		int identityOfArray = _E91A.GetIdentityOfArray(checks);
		foreach (_E920 counter in _counters)
		{
			if (counter.GetIdentity() == identityOfArray)
			{
				return counter;
			}
		}
		return null;
	}

	public void Test(int valueToAdd, params _E91A[] checks)
	{
		if (valueToAdd == 0)
		{
			return;
		}
		_E920 obj = _E001(checks);
		if (obj != null)
		{
			_E002(valueToAdd, obj, checks);
			return;
		}
		_E920[] array = _counters.Where((_E920 x) => x.Template != null).ToArray();
		for (int num = array.Length - 1; num >= 0; num--)
		{
			try
			{
				_E920 counter = array[num];
				_E002(valueToAdd, counter, checks);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
	}

	private static void _E002(int valueToAdd, _E920 counter, _E91A[] checks)
	{
		if (counter.Template.conditions.TestAll(counter.Quest, checks) && counter.Quest.CheckVisibilityStatus(counter.Template.RootCondition))
		{
			counter.Value += valueToAdd;
		}
	}

	public void ResetNullableConditions()
	{
		foreach (_E920 counter in _counters)
		{
			foreach (Condition condition in counter.Template.conditions)
			{
				if (condition is ConditionHit conditionHit && !((float)counter.Value >= counter.Template.RequiredValue) && conditionHit.resetOnSessionEnd)
				{
					counter.Value = 0;
				}
			}
		}
	}
}
