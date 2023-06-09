using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationEventSystem;

[Serializable]
public class EventsCollection
{
	[SerializeField]
	private List<AnimationEvent> _animationEvents;

	public List<AnimationEvent> AnimationEvents => _animationEvents;

	public EventsCollection(IEnumerable<AnimationEvent> animationEvents)
	{
		_animationEvents = new List<AnimationEvent>(animationEvents);
	}

	private static void _E000(EventsCollection eventsCollection)
	{
		float max = 0.9899901f;
		List<AnimationEvent> animationEvents = eventsCollection._animationEvents;
		for (int i = 0; i < animationEvents.Count; i++)
		{
			AnimationEvent animationEvent = animationEvents[i];
			float num = Mathf.Clamp(animationEvent.Time, 0f, max);
			if (Math.Abs(num - animationEvent.Time) >= float.Epsilon)
			{
				animationEvent.Time = num;
			}
		}
	}
}
