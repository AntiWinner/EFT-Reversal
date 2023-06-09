using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cutscene;
using EFT.GlobalEvents;
using UnityEngine;

namespace EFT.NPC;

public class NPCReactionController : MonoBehaviour
{
	private enum EProcessQueueState
	{
		Waiting,
		HandlingQueue
	}

	[Serializable]
	public class ObjectWithID
	{
		public int ID;

		public GameObject gameObject;
	}

	[SerializeField]
	private Animator _npcAnimator;

	[SerializeField]
	private NPCGlobalEventsReacting _eventsReacting;

	[SerializeField]
	private NPCAnimationsEventReceiver _animatorEventsReceiver;

	[SerializeField]
	private List<ObjectWithID> _objectsForToggleVisibility = new List<ObjectWithID>();

	private LipSyncPlayer m__E000;

	private _E385<_E850> m__E001 = new _E385<_E850>(4, () => new _E850());

	private Queue<_E850> m__E002 = new Queue<_E850>();

	private EProcessQueueState m__E003;

	private WaitForEndOfFrame m__E004 = new WaitForEndOfFrame();

	private _E850 m__E005;

	private Coroutine m__E006;

	private Dictionary<int, GameObject> m__E007 = new Dictionary<int, GameObject>();

	private bool m__E008;

	private Action m__E009;

	[HideInInspector]
	public int CurrentIdleAnimationNumber;

	public Animator TestAnimator => _npcAnimator;

	private void Awake()
	{
		_animatorEventsReceiver.Initialize(_npcAnimator);
		_animatorEventsReceiver.OnCurrentAnimationEnded += _E002;
		_animatorEventsReceiver.OnNeedChangeObjectVisibility += _E004;
		this.m__E006 = StartCoroutine(_E005());
		_eventsReacting.OnNeedReactOnEvent += AddReactionInQueue;
		this.m__E000 = LipSyncPlayer.GetPlayer(_npcAnimator.gameObject);
		this.m__E000.OnStartPlay += _E000;
		this.m__E000.OnStopPlay += _E001;
		foreach (ObjectWithID item in _objectsForToggleVisibility)
		{
			this.m__E007.Add(item.ID, item.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (this.m__E006 != null)
		{
			StopCoroutine(this.m__E006);
			this.m__E006 = null;
		}
		this.m__E001.Dispose();
	}

	public void AddReactionInQueue(int eventID, ReactionOnEvent eventReaction)
	{
		_E850 obj = this.m__E001.Withdraw();
		obj.SetUp(_E84F.Instance.GetActionOnEndFeedback(eventID), eventReaction.animationBoolNames, eventReaction.animationTriggerNames, eventReaction.animationInt, eventReaction.lipSyncData, eventReaction.clearReactionQueue);
		this.m__E002.Enqueue(obj);
	}

	private void _E000()
	{
		this.m__E008 = true;
	}

	private void _E001()
	{
		this.m__E008 = false;
		this.m__E009?.Invoke();
		this.m__E009 = null;
	}

	private void _E002()
	{
		if (this.m__E005 == null)
		{
			return;
		}
		foreach (string animationBoolName in this.m__E005.AnimationBoolNames)
		{
			_npcAnimator.SetBool(animationBoolName, value: false);
		}
		if (this.m__E005.AudioReactionEnded && this.m__E008)
		{
			this.m__E009 = delegate
			{
				_E006();
			};
		}
		else
		{
			_E006();
		}
	}

	private void _E003()
	{
		if (this.m__E005.OnEndAudioReaction())
		{
			bool clearReactionQueue = this.m__E005.ClearReactionQueue;
			this.m__E001.Return(this.m__E005);
			this.m__E005 = null;
			if (clearReactionQueue)
			{
				this.m__E002.Clear();
			}
		}
	}

	private void _E004(int objectID, bool visible)
	{
		if (this.m__E007.TryGetValue(objectID, out var value))
		{
			value.SetActive(visible);
		}
	}

	private IEnumerator _E005()
	{
		while (true)
		{
			switch (this.m__E003)
			{
			case EProcessQueueState.Waiting:
				if (this.m__E002.Count > 0)
				{
					this.m__E003 = EProcessQueueState.HandlingQueue;
				}
				break;
			case EProcessQueueState.HandlingQueue:
				if (this.m__E005 != null)
				{
					break;
				}
				if (this.m__E002.Count > 0 && !this.m__E008)
				{
					this.m__E005 = this.m__E002.Dequeue();
					if (!this.m__E005.HaveAnyData)
					{
						if (!string.IsNullOrEmpty(this.m__E005.AnimationInt.paramName))
						{
							_npcAnimator.SetInteger(this.m__E005.AnimationInt.paramName, this.m__E005.AnimationInt.GetValue());
						}
						this.m__E001.Return(this.m__E005);
						this.m__E005 = null;
						break;
					}
					if (!string.IsNullOrEmpty(this.m__E005.AnimationInt.paramName))
					{
						_npcAnimator.SetInteger(this.m__E005.AnimationInt.paramName, this.m__E005.AnimationInt.GetValue());
					}
					foreach (string animationBoolName in this.m__E005.AnimationBoolNames)
					{
						_npcAnimator.SetBool(animationBoolName, value: true);
					}
					foreach (string animationTriggerName in this.m__E005.AnimationTriggerNames)
					{
						_npcAnimator.SetTrigger(animationTriggerName);
					}
					if (this.m__E005.LipSyncData.HaveData)
					{
						LipSyncPlayer.Play(this.m__E005.LipSyncData, this.m__E000.gameObject, delegate(string clipName)
						{
							this.m__E005.AudioReactionName = clipName;
						}, delegate
						{
							_E003();
						});
					}
				}
				else
				{
					this.m__E003 = EProcessQueueState.Waiting;
				}
				break;
			}
			yield return this.m__E004;
		}
	}

	public void TestChangeIdleAnimationNumber()
	{
		CurrentIdleAnimationNumber++;
		if (CurrentIdleAnimationNumber >= 3)
		{
			CurrentIdleAnimationNumber = 0;
		}
		_npcAnimator.SetInteger(_ED3E._E000(166973), CurrentIdleAnimationNumber);
	}

	[CompilerGenerated]
	private void _E006()
	{
		if (this.m__E005 != null && this.m__E005.OnEndAnimReaction())
		{
			bool clearReactionQueue = this.m__E005.ClearReactionQueue;
			this.m__E001.Return(this.m__E005);
			this.m__E005 = null;
			if (clearReactionQueue)
			{
				this.m__E002.Clear();
			}
		}
	}

	[CompilerGenerated]
	private void _E007()
	{
		_E006();
	}

	[CompilerGenerated]
	private void _E008(string clipName)
	{
		this.m__E005.AudioReactionName = clipName;
	}

	[CompilerGenerated]
	private void _E009()
	{
		_E003();
	}
}
