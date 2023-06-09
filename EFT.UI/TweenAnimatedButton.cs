using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class TweenAnimatedButton : SerializedMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerUpHandler, IPointerClickHandler, IPointerDownHandler
{
	[SerializeField]
	private _EC42 _animation;

	[SerializeField]
	private bool _interactable = true;

	[CompilerGenerated]
	private Action _E000;

	[CompilerGenerated]
	private Action _E001;

	[CompilerGenerated]
	private Action _E002;

	private bool _E003;

	private bool _E004;

	public bool Interactable
	{
		get
		{
			return _interactable;
		}
		set
		{
			if (_interactable != value)
			{
				_interactable = value;
				if (_interactable)
				{
					_animation.TransitionToState(_E004 ? EButtonAnimationState.Highlighted : EButtonAnimationState.Normal);
					return;
				}
				_animation.TransitionToState(EButtonAnimationState.Disabled);
				_E003 = false;
				_E004 = false;
			}
		}
	}

	public event Action OnClick
	{
		[CompilerGenerated]
		add
		{
			Action action = _E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnMouseEnter
	{
		[CompilerGenerated]
		add
		{
			Action action = _E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnMouseExit
	{
		[CompilerGenerated]
		add
		{
			Action action = _E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void OnEnable()
	{
		_animation.SetState((!_interactable) ? EButtonAnimationState.Disabled : EButtonAnimationState.Normal);
	}

	private void OnDisable()
	{
		_animation.Stop();
		_E003 = false;
		_E004 = false;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && (_E003 || _E004))
		{
			_animation.TransitionToState(EButtonAnimationState.Normal);
			_E003 = false;
			_E004 = false;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!_E004 && Interactable)
		{
			_E004 = true;
			_E001?.Invoke();
			if (!_E003)
			{
				_animation.TransitionToState(EButtonAnimationState.Highlighted);
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_E004 && Interactable)
		{
			_E004 = false;
			_E002?.Invoke();
			if (!_E003)
			{
				_animation.TransitionToState(EButtonAnimationState.Normal);
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!_E003 && Interactable)
		{
			_E003 = true;
			_animation.TransitionToState(EButtonAnimationState.Pressed);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (_E003 && Interactable)
		{
			_E003 = false;
			_animation.TransitionToState(_E004 ? EButtonAnimationState.Highlighted : EButtonAnimationState.Normal);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_interactable)
		{
			_E000?.Invoke();
		}
	}
}
