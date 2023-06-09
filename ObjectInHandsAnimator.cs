using System;
using System.Collections.Generic;
using AnimationEventSystem;
using EFT;
using JetBrains.Annotations;

public abstract class ObjectInHandsAnimator
{
	public enum PlayerState
	{
		None,
		Idle,
		Jump,
		Sprint,
		Prone
	}

	public const int LACTION_DROP_BACKPACK_INDEX = 300;

	protected const string LACTIONS_LAYER_NAME = "LActions";

	private Func<IAnimator> _animatorGetter;

	protected int _animatorLayersCount;

	private readonly List<_E324> _eventsConsumers = new List<_E324>(2);

	private readonly HashSet<int> _existedParameters = new HashSet<int>();

	public int LACTIONS_LAYER_INDEX = -1;

	public IAnimator Animator => _animatorGetter();

	public bool IsInInteraction => _E326.GetBoolUseLeftHand(Animator);

	public bool IsInInventory => _E326.GetBoolInventory(Animator);

	public virtual Action FireEventAction => null;

	public void AnimatorEventHandler(int functionNameHash, AnimationEventParameter parameter)
	{
		_E325.AnimatorEventHandler(_eventsConsumers, functionNameHash, parameter);
	}

	public void Gesture(EGesture index)
	{
		_E326.TriggerGesture(Animator);
		_E326.SetUseLeftHand(Animator, useLeftHand: true);
		_E326.SetLActionIndex(Animator, 0);
		_E326.SetGestureIndex(Animator, (float)(index - 1));
	}

	public void ShowCompass(bool show)
	{
		if (show)
		{
			_E326.SetUseLeftHand(Animator, useLeftHand: true);
		}
		_E326.SetLActionIndex(Animator, show ? 404 : 0);
	}

	public void ShowRadioTransmitter(bool show)
	{
		if (show)
		{
			_E326.SetUseLeftHand(Animator, useLeftHand: true);
		}
		_E326.SetLActionIndex(Animator, show ? 404 : 0);
	}

	public void AddEventsConsumer([NotNull] _E324 eventsConsumer)
	{
		_eventsConsumers.Add(eventsConsumer);
	}

	public void RemoveEventsConsumer([NotNull] _E324 eventsConsumer)
	{
		_eventsConsumers.Remove(eventsConsumer);
	}

	public virtual void SetAnimatorGetter(Func<IAnimator> getter)
	{
		using (_ECC9.BeginSampleWithToken("ObjectInHandsAnimator:77.SetAnimatorGetter", "SetAnimatorGetter"))
		{
			_animatorGetter = getter;
			_animatorLayersCount = Animator.layerCount;
			_existedParameters.Clear();
			for (int i = 0; i < Animator.parameterCount; i++)
			{
				AnimatorParameterInfo parameter = Animator.GetParameter(i);
				_existedParameters.Add(parameter.nameHash);
			}
		}
	}

	protected bool HasParameter(int parameterHash)
	{
		return _existedParameters.Contains(parameterHash);
	}

	public void SetPlayerState(PlayerState state)
	{
		if (HasParameter(_E326.INT_PLAYERSTATE))
		{
			_E326.SetPlayerState(Animator, (int)state);
		}
	}

	public void SetPatrol(bool b)
	{
		_E326.SetPatrol(Animator, b);
	}

	public void SkipTime(float t)
	{
		Animator.Update(t);
	}

	public bool CurrentStateNameIs(int layer, string sname)
	{
		return Animator.GetCurrentAnimatorStateInfo(layer).IsName(sname);
	}

	public void SetLayerWeight(int layerid, int p2)
	{
		Animator.SetLayerWeight(layerid, p2);
	}

	public void SetLayerWeight(int layerId, float weight)
	{
		Animator.SetLayerWeight(layerId, weight);
	}

	public void SetLayerWeight(string layerName, int p2)
	{
		Animator.SetLayerWeight(Animator.GetLayerIndex(layerName), p2);
	}

	public void ResetLeftHand()
	{
		_E326.SetUseLeftHand(Animator, useLeftHand: false);
	}

	public void SetInventory(bool open)
	{
		_E326.SetInventory(Animator, open);
		ResetLeftHand();
	}

	public void SetActiveParam(bool active, bool resetLeftHand = true)
	{
		_E326.SetActive(Animator, active);
		if (resetLeftHand)
		{
			ResetLeftHand();
		}
	}

	public void SetFastHide(bool fastHide)
	{
		_E326.SetFastHide(Animator, fastHide);
	}

	public void SetQuickFire(bool quickFire)
	{
		_E326.SetQuickFire(Animator, quickFire);
	}

	public void SetPointOfViewOnSpawn(EPointOfView pointOfView)
	{
		_E326.SetThirdPerson(Animator, (pointOfView == EPointOfView.ThirdPerson) ? 1 : 0);
	}

	public void SetAnimationSpeed(float speed)
	{
		Animator.speed = speed;
	}

	public void SetLActionIndex(int actionIndex)
	{
		_E326.SetLActionIndex(Animator, actionIndex);
	}

	public void SetDeflected(bool b)
	{
		_E326.SetDeflected(Animator, b ? 1f : 0f);
	}

	public void SetMeleeSpeed(float speed)
	{
		_E326.SetSwingSpeed(Animator, speed);
	}

	public float GetAnimatorParameter(int hash)
	{
		return Animator.GetFloat(hash);
	}

	public float GetLeftHandProgress()
	{
		return Animator.GetFloat(_E326.FLOAT_LEFTHANDPROGRESS);
	}

	public float GetLayerWeight(int layerIndex)
	{
		if (layerIndex < 0 || layerIndex >= _animatorLayersCount)
		{
			return 0f;
		}
		return Animator.GetLayerWeight(layerIndex);
	}
}
