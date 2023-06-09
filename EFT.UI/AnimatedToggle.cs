using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class AnimatedToggle : Toggle
{
	[SerializeField]
	private string _onTrigger = _ED3E._E000(205981);

	[SerializeField]
	private string _offTrigger = _ED3E._E000(255223);

	[CompilerGenerated]
	private Action m__E000;

	public string OnTrigger
	{
		get
		{
			return _onTrigger;
		}
		set
		{
			_onTrigger = value;
		}
	}

	public string OffTrigger
	{
		get
		{
			return _offTrigger;
		}
		set
		{
			_offTrigger = value;
		}
	}

	internal bool _E001
	{
		get
		{
			return base.isOn;
		}
		set
		{
			base.isOn = value;
			if (base.transition == Transition.Animation)
			{
				TriggerAnimation(value);
			}
		}
	}

	public event Action OnMouseDown
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		this.m__E000?.Invoke();
		base.OnPointerDown(eventData);
	}

	protected override void Awake()
	{
		base.Awake();
		if (!base.isOn)
		{
			base.animator.ResetTrigger(OnTrigger);
			base.animator.ResetTrigger(OffTrigger);
		}
		onValueChanged.AddListener(ToggleSilent);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		ToggleSilent(base.isOn);
	}

	public void ToggleSilent(bool value)
	{
		SetIsOnWithoutNotify(value);
		if (base.transition == Transition.Animation)
		{
			TriggerAnimation(value);
		}
	}

	public void TriggerAnimation(bool value)
	{
		_E000(value ? OnTrigger : OffTrigger);
	}

	private void _E000(string triggerName)
	{
		if (!(base.animator == null) && base.animator.enabled && base.animator.isActiveAndEnabled && !(base.animator.runtimeAnimatorController == null) && !string.IsNullOrEmpty(triggerName))
		{
			base.animator.ResetTrigger(OnTrigger);
			base.animator.ResetTrigger(OffTrigger);
			base.animator.SetTrigger(triggerName);
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (base.group.allowSwitchOff || !base.isOn)
		{
			base.OnPointerClick(eventData);
		}
	}

	protected override void InstantClearState()
	{
		if (base.transition == Transition.Animation)
		{
			_E000(OffTrigger);
		}
		base.InstantClearState();
	}
}
