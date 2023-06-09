using EFT.GlobalEvents;
using UnityEngine;

namespace EFT.NPC;

public class AnimatorByEventsToggler : MonoBehaviour
{
	[SerializeField]
	private Animator _targetAnimator;

	[SerializeField]
	private string _triggerOnToggleOn;

	[SerializeField]
	private BaseEventFilter toggleOnEvent;

	[SerializeField]
	private BaseEventFilter toggleOffEvent;

	private void Awake()
	{
		toggleOnEvent.OnFilterPassed += _E000;
		toggleOffEvent.OnFilterPassed += _E001;
	}

	private void OnDestroy()
	{
		toggleOnEvent.OnFilterPassed -= _E000;
		toggleOffEvent.OnFilterPassed -= _E001;
	}

	private void _E000(BaseEventFilter filter, _EBAD invokedEvent)
	{
		_E002(toggle: true);
	}

	private void _E001(BaseEventFilter filter, _EBAD invokedEvent)
	{
		_E002(toggle: false);
	}

	private void _E002(bool toggle)
	{
		_targetAnimator.enabled = toggle;
		if (toggle && !string.IsNullOrEmpty(_triggerOnToggleOn))
		{
			_targetAnimator.SetTrigger(_triggerOnToggleOn);
		}
	}
}
