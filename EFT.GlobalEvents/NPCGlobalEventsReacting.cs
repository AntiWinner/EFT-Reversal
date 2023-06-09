using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace EFT.GlobalEvents;

public class NPCGlobalEventsReacting : MonoBehaviour
{
	[Serializable]
	public class AnimationInt
	{
		public string paramName;

		public bool randomizeValue;

		public int paramValue;

		public int minValue;

		public int maxValue;

		public int GetValue()
		{
			if (randomizeValue)
			{
				return UnityEngine.Random.Range(minValue, maxValue + 1);
			}
			return paramValue;
		}
	}

	[SerializeField]
	private List<EventFilterAnimationRelation> eventsFilters = new List<EventFilterAnimationRelation>();

	private Dictionary<BaseEventFilter, ReactionOnEvent> m__E000 = new Dictionary<BaseEventFilter, ReactionOnEvent>();

	[CompilerGenerated]
	private Action<int, ReactionOnEvent> _E001;

	public event Action<int, ReactionOnEvent> OnNeedReactOnEvent
	{
		[CompilerGenerated]
		add
		{
			Action<int, ReactionOnEvent> action = _E001;
			Action<int, ReactionOnEvent> action2;
			do
			{
				action2 = action;
				Action<int, ReactionOnEvent> value2 = (Action<int, ReactionOnEvent>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<int, ReactionOnEvent> action = _E001;
			Action<int, ReactionOnEvent> action2;
			do
			{
				action2 = action;
				Action<int, ReactionOnEvent> value2 = (Action<int, ReactionOnEvent>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		this.m__E000.Clear();
		foreach (EventFilterAnimationRelation eventsFilter in eventsFilters)
		{
			eventsFilter.eventFilter.OnFilterPassed += _E000;
			this.m__E000[eventsFilter.eventFilter] = eventsFilter.reaction;
		}
	}

	private void OnDestroy()
	{
		_E001 = null;
	}

	private void _E000(BaseEventFilter filter, _EBAD passedEvent)
	{
		_E001?.Invoke(passedEvent.ID, this.m__E000[filter]);
	}
}
