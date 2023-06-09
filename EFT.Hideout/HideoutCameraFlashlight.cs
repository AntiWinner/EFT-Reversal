using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutCameraFlashlight : MonoBehaviour
{
	[Serializable]
	public struct TransitionSettings
	{
		public float TransitionTime;

		public Ease TransitionType;
	}

	[SerializeField]
	private Light _flashlight;

	[SerializeField]
	private TransitionSettings _turnOnTransition;

	[SerializeField]
	private TransitionSettings _turnOffTransition;

	[SerializeField]
	private bool _startsActivated;

	private bool _E000;

	private float _E001;

	[CompilerGenerated]
	private bool _E002;

	public bool IsActive
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	private void Awake()
	{
		_E001 = _flashlight.intensity;
		_E000 = _startsActivated;
		if (!_startsActivated)
		{
			_flashlight.intensity = 0f;
		}
	}

	private void OnEnable()
	{
		SetState(IsActive);
	}

	public void SetState(bool active)
	{
		if (!(_flashlight == null))
		{
			IsActive = active;
			if (!(base.gameObject == null) && base.gameObject.activeInHierarchy && active != _E000)
			{
				_E000 = active;
				TransitionSettings transitionSettings = (_E000 ? _turnOnTransition : _turnOffTransition);
				float endValue = (_E000 ? _E001 : 0f);
				_flashlight.DOKill();
				_flashlight.DOIntensity(endValue, transitionSettings.TransitionTime).SetEase(transitionSettings.TransitionType);
			}
		}
	}
}
