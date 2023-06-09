using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NLog;
using UnityEngine;

namespace AnimationEventSystem;

[Serializable]
public class AnimationEventsContainer : ICloneable
{
	private class _E000 : _E315
	{
		private new struct _E000
		{
			public int stateHash;

			public EUpdateType UpdateType;
		}

		private _E000 _E00D = new _E000
		{
			stateHash = 0
		};

		public _E000()
			: base(_ED3E._E000(123169), LoggerMode.Add)
		{
		}

		public void LogEventsCheck(IAnimator animator, in AnimatorStateInfoWrapper stateInfo, int layerIndex, EUpdateType updateType, float previousNormalizedTime)
		{
			if (_E000(in stateInfo, updateType))
			{
				_E00D.stateHash = stateInfo.fullPathHash;
				_E00D.UpdateType = updateType;
				LogLevel trace = LogLevel.Trace;
				string stateName = animator.GetStateName(in stateInfo);
				if (animator.IsInTransition(layerIndex))
				{
					AnimatorStateInfoWrapper stateInfo2 = animator.GetCurrentAnimatorStateInfo(layerIndex);
					AnimatorStateInfoWrapper stateInfo3 = animator.GetNextAnimatorStateInfo(layerIndex);
					Log(_ED3E._E000(123215), _ED3E._E000(123234), trace, Time.frameCount, stateName, updateType, stateInfo.normalizedTime, animator.GetStateName(in stateInfo2), animator.GetStateName(in stateInfo3), animator, previousNormalizedTime);
				}
				else
				{
					Log(_ED3E._E000(123352), _ED3E._E000(123391), trace, Time.frameCount, stateName, updateType, previousNormalizedTime, stateInfo.normalizedTime, animator);
				}
			}
		}

		private bool _E000(in AnimatorStateInfoWrapper stateInfo, EUpdateType updateType)
		{
			if (IsEnabled(LogLevel.Trace))
			{
				return true;
			}
			if (IsEnabled(LogLevel.Debug) && (_E00D.stateHash != stateInfo.fullPathHash || _E00D.UpdateType != updateType))
			{
				return true;
			}
			return false;
		}

		public void DebugAnimatorsSwitch(IAnimator previousAnimator, IAnimator animator, in AnimatorStateInfoWrapper stateInfo, int layerIndex, EUpdateType updateType)
		{
			if (IsEnabled(LogLevel.Debug))
			{
				string stateName = animator.GetStateName(in stateInfo);
				Log(_ED3E._E000(123413), _ED3E._E000(123413), LogLevel.Debug, Time.frameCount, stateName, updateType, previousAnimator, animator);
			}
		}

		public void LogEventTimeCheck(AnimationEvent animationEvent, float previousNormalizedTime, float normalizedTime)
		{
			if (IsEnabled(LogLevel.Debug))
			{
				Log(LogLevel.Trace, _ED3E._E000(123424), Time.frameCount, animationEvent, previousNormalizedTime, normalizedTime);
			}
		}
	}

	public enum EUpdateType
	{
		OnEnter,
		OnUpdate,
		OnExit
	}

	private const float ALMOST_ONE = 0.99999f;

	private const float A_BIT_GREATER_THAN_ONE = 1.00001f;

	private const float MIN_DIFF = 1E-05f;

	private static readonly _E000 Logger = new _E000();

	private bool _previousLoop;

	private float _previousNormalizedTime;

	private IAnimator _previousAnimator;

	private _E56F _eventsConsumer;

	private bool _hasEventsAndConsumerCache;

	public virtual void ResetCache()
	{
		_previousLoop = false;
		_previousNormalizedTime = 0f;
	}

	public void SetEventsConsumer([NotNull] _E56F consumer)
	{
		if (_eventsConsumer != null)
		{
			Logger.LogError(_ED3E._E000(124900), consumer, _eventsConsumer);
		}
		_eventsConsumer = consumer;
		_hasEventsAndConsumerCache = _E001();
	}

	public void UnsetEventsConsumer([NotNull] _E56F consumer)
	{
		if (_eventsConsumer != consumer)
		{
			Logger.LogError(_ED3E._E000(122913), consumer, _eventsConsumer);
		}
		_eventsConsumer = null;
		_hasEventsAndConsumerCache = _E001();
	}

	public object Clone()
	{
		return new AnimationEventsContainer();
	}

	public void OnStateEnter(IAnimator animator, in AnimatorStateInfoWrapper stateInfo, int layerIndex, List<AnimationEvent> animationEvents)
	{
		_E000(animator, in stateInfo, layerIndex, EUpdateType.OnEnter, animationEvents);
	}

	public void OnStateUpdate(IAnimator animator, in AnimatorStateInfoWrapper stateInfo, int layerIndex, List<AnimationEvent> animationEvents)
	{
		_E000(animator, in stateInfo, layerIndex, EUpdateType.OnUpdate, animationEvents);
	}

	public void OnStateExit(IAnimator animator, in AnimatorStateInfoWrapper stateInfo, int layerIndex, List<AnimationEvent> animationEvents)
	{
		_E000(animator, in stateInfo, layerIndex, EUpdateType.OnExit, animationEvents);
	}

	private void _E000(IAnimator animator, in AnimatorStateInfoWrapper stateInfo, int layerIndex, EUpdateType updateType, List<AnimationEvent> animationEvents)
	{
		if (updateType == EUpdateType.OnExit && _previousAnimator != animator && _previousAnimator != null)
		{
			Logger.DebugAnimatorsSwitch(_previousAnimator, animator, in stateInfo, layerIndex, updateType);
			_previousAnimator = animator;
			return;
		}
		_previousAnimator = animator;
		Logger.LogEventsCheck(animator, in stateInfo, layerIndex, updateType, _previousNormalizedTime);
		if (!_hasEventsAndConsumerCache)
		{
			return;
		}
		int num = (int)_previousNormalizedTime;
		int num2 = (int)stateInfo.normalizedTime;
		float previousNormalizedTime = ((_previousLoop || num <= 0) ? (_previousNormalizedTime - (float)num) : 1f);
		float normalizedTime = stateInfo.normalizedTime - (float)num2;
		if (animator.IsInTransition(layerIndex) && animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash != stateInfo.fullPathHash)
		{
			_E002(animator, in stateInfo, previousNormalizedTime, 0.99999f, animationEvents);
			_previousNormalizedTime = 0.99999f;
			return;
		}
		if (stateInfo.loop)
		{
			if (updateType == EUpdateType.OnExit && num2 == 1 && (num2 < num || _previousNormalizedTime > stateInfo.normalizedTime))
			{
				_E002(animator, in stateInfo, previousNormalizedTime, 1.00001f, animationEvents);
			}
			else if (num == num2)
			{
				_E002(animator, in stateInfo, previousNormalizedTime, normalizedTime, animationEvents);
			}
			else if (num < num2)
			{
				_E002(animator, in stateInfo, previousNormalizedTime, 1.00001f, animationEvents);
				if (updateType != EUpdateType.OnExit)
				{
					_E002(animator, in stateInfo, 0f, normalizedTime, animationEvents);
				}
			}
			_previousNormalizedTime = stateInfo.normalizedTime;
		}
		else
		{
			float num3 = Mathf.Clamp01(_previousNormalizedTime);
			float num4 = Mathf.Clamp01(stateInfo.normalizedTime);
			if (num == 0 && num2 == 0)
			{
				if (num3 != num4)
				{
					_E002(animator, in stateInfo, num3, num4, animationEvents);
				}
			}
			else if (num == 0 && num2 > 0 && num3 < 1f)
			{
				_E002(animator, in stateInfo, previousNormalizedTime, 1.00001f, animationEvents);
			}
			_previousNormalizedTime = Mathf.Clamp(stateInfo.normalizedTime, 0f, 0.99999f);
			if (updateType == EUpdateType.OnExit)
			{
				_previousNormalizedTime = 0f;
			}
		}
		_previousLoop = stateInfo.loop;
	}

	private bool _E001()
	{
		if (_eventsConsumer == null)
		{
			return false;
		}
		return true;
	}

	private void _E002(IAnimator animator, in AnimatorStateInfoWrapper stateInfo, float previousNormalizedTime, float normalizedTime, List<AnimationEvent> animationEvents)
	{
		if (animator == null)
		{
			Debug.LogError(_ED3E._E000(123025));
			return;
		}
		if (animationEvents == null)
		{
			Debug.LogError(string.Format(_ED3E._E000(123050), _ED3E._E000(123135), _ED3E._E000(123119), animator));
			return;
		}
		int count = animationEvents.Count;
		for (int i = 0; i < count; i++)
		{
			AnimationEvent animationEvent = animationEvents[i];
			if (animationEvent == null)
			{
				Debug.LogError(string.Format(_ED3E._E000(123104), _ED3E._E000(123186), i, _ED3E._E000(123119), animator));
			}
			else if (animationEvent.IsTimeToFire(previousNormalizedTime, normalizedTime))
			{
				Logger.LogEventTimeCheck(animationEvent, previousNormalizedTime, normalizedTime);
				_E003(animationEvent, animator, in stateInfo, normalizedTime);
			}
		}
	}

	private void _E003(AnimationEvent @event, IAnimator animator, in AnimatorStateInfoWrapper stateInfo, float normalizedTime)
	{
		if (_eventsConsumer != null)
		{
			_eventsConsumer.ReceiveEvent(@event, animator, in stateInfo, normalizedTime);
		}
	}
}
