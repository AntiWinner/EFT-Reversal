using System;
using DG.Tweening;
using EFT.Communications;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class NotificationView : UIElement, IPointerClickHandler, IEventSystemHandler
{
	private const float ANIMATION_PREPARE_TIME = 0.3f;

	private static readonly int _speed = Animator.StringToHash("Speed");

	private static readonly int _autoComplete = Animator.StringToHash("AutoComplete");

	private static readonly int _forcedComplete = Animator.StringToHash("ForcedComplete");

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private CustomTextMeshProUGUI _text;

	[SerializeField]
	private LayoutElement _layout;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private Animator _animator;

	private _E856 _notification;

	private float _height;

	public bool IsHiding { get; private set; }

	private new RectTransform RectTransform => (RectTransform)base.transform;

	public event Action<NotificationView> OnHideComplete;

	public void Init(_E856 notification, Sprite sprite, Color backgroundColor)
	{
		_notification = notification;
		_icon.sprite = sprite;
		_text.color = _notification.TextColor ?? _defaultColor;
		_text.text = _notification.Description;
		_layout.DOKill();
		_layout.preferredHeight = -1f;
		LayoutRebuilder.ForceRebuildLayoutImmediate(_container);
		_layout.preferredHeight = 0f;
		_height = RectTransform.rect.height;
		_background.color = backgroundColor;
		_canvasGroup.interactable = true;
		float value = 1f;
		switch (_notification.Duration)
		{
		case ENotificationDurationType.Long:
			value = 0.5f;
			break;
		case ENotificationDurationType.Infinite:
			_animator.SetBool(_autoComplete, value: false);
			break;
		}
		_animator.SetFloat(_speed, value);
		UI.Dispose();
		if (_notification is _E8A1 obj)
		{
			UI.AddDisposable(obj.OnRemove.Subscribe(delegate
			{
				HideNotification(invokeCallback: false);
			}));
		}
		StartAnimation();
	}

	public void OnAnimationDone()
	{
		StopAnimation();
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		HideNotification(eventData.button == PointerEventData.InputButton.Left);
	}

	public bool HasSameNotification(_E856 other)
	{
		return _notification.Equals(other);
	}

	private void OnDisable()
	{
		_notification = null;
	}

	private void StartAnimation()
	{
		IsHiding = false;
		_animator.enabled = true;
		if (_layout.preferredHeight < 0f)
		{
			_layout.preferredHeight = 0f;
		}
		_layout.DOKill();
		_layout.DOPreferredSize(new Vector2(-1f, _height), 0.3f);
	}

	private void StopAnimation()
	{
		IsHiding = true;
		_layout.DOKill();
		_layout.DOPreferredSize(new Vector2(-1f, 0f), 0.3f).OnComplete(delegate
		{
			_animator.enabled = false;
			this.OnHideComplete?.Invoke(this);
		});
	}

	private void HideNotification(bool invokeCallback)
	{
		IsHiding = true;
		_canvasGroup.interactable = false;
		if (_notification is _E8A1 obj)
		{
			UI.Dispose();
			if (invokeCallback)
			{
				obj.OnClick();
			}
		}
		if (_animator.enabled)
		{
			_animator.SetTrigger(_forcedComplete);
		}
		else
		{
			StopAnimation();
		}
	}
}
