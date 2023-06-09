using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace AnimationEventSystem;

[_E506(typeof(_E574))]
public class AnimationEventsStateBehaviour : StateMachineBehaviour, _E572, IStateBehaviour
{
	public AnimationEventsContainer EventsContainer;

	public string FullName;

	public int FullNameHash;

	public AnimatorControllerStaticData EventsData;

	public int EventsListId = -1;

	private _E564 m__E000;

	[CanBeNull]
	public List<AnimationEvent> AnimationEvents => EventsData.GetEventsByIndex(EventsListId);

	AnimationEventsContainer _E572.EventsContainer
	{
		get
		{
			return EventsContainer;
		}
		set
		{
			EventsContainer = value;
		}
	}

	int _E572.FullNameHash => FullNameHash;

	private void _E000(Animator animator)
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = _E564.Create();
		}
		if ((object)animator != this.m__E000.Animator)
		{
			this.m__E000.SetAnimator(animator);
		}
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E000(animator);
		AnimationEventsContainer eventsContainer = EventsContainer;
		_E564 animator2 = this.m__E000;
		AnimatorStateInfoWrapper stateInfo2 = _E564.CreateAnimatorStateInfoWrapper(stateInfo);
		eventsContainer.OnStateEnter(animator2, in stateInfo2, layerIndex, AnimationEvents);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E000(animator);
		AnimationEventsContainer eventsContainer = EventsContainer;
		_E564 animator2 = this.m__E000;
		AnimatorStateInfoWrapper stateInfo2 = _E564.CreateAnimatorStateInfoWrapper(stateInfo);
		eventsContainer.OnStateUpdate(animator2, in stateInfo2, layerIndex, AnimationEvents);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E000(animator);
		AnimationEventsContainer eventsContainer = EventsContainer;
		_E564 animator2 = this.m__E000;
		AnimatorStateInfoWrapper stateInfo2 = _E564.CreateAnimatorStateInfoWrapper(stateInfo);
		eventsContainer.OnStateExit(animator2, in stateInfo2, layerIndex, AnimationEvents);
	}
}
