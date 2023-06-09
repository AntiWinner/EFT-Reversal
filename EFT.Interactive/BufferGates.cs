using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public class BufferGates : Switch
{
	[SerializeField]
	private AnimationCurve _gatesMovementCurve;

	[SerializeField]
	private AnimationCurve _gatesCloseMovementCurve;

	[SerializeField]
	private GameObjectsToggleByDistance gatePartsDisabler;

	[SerializeField]
	private AudioClip _gatesOpenSound;

	[SerializeField]
	private AudioClip _gatesCloseSound;

	[SerializeField]
	private float _curveValueOnStartAutoClosing = 0.86f;

	private float _E01B;

	private float _E01C;

	private bool _E01D;

	private bool _E01E;

	internal new bool _E000 => _E01B >= 1f;

	protected override void ProcessAngleAsShift(float value)
	{
		if (_interaction.ResultState == EDoorState.Open)
		{
			_E000();
		}
		else if (_interaction.ResultState == EDoorState.Shut)
		{
			_E001();
		}
	}

	protected override void DoorStateChanged(EDoorState newState)
	{
		if (DoorState == EDoorState.Open)
		{
			DoorState = EDoorState.Shut;
		}
		if (DoorState == EDoorState.Shut)
		{
			gatePartsDisabler.ToggleCheck(toggle: false);
			if (_interaction != null && _interaction.ProgressTime < _interaction.Duration)
			{
				base.transform.localPosition = ShutPosition;
			}
		}
	}

	protected override void InitializeSmoothOpenInteraction(EDoorState state, bool confirmed)
	{
		_interaction.InitSmoothOpen(state, CurrentAngle, GetAngle(state), (state == EDoorState.Open) ? _gatesMovementCurve : _gatesCloseMovementCurve, confirmed);
		gatePartsDisabler.ToggleCheck(toggle: true);
	}

	protected override bool CanStartInteraction(EDoorState state, bool logFalse = false)
	{
		return DoorState != state;
	}

	private void _E000()
	{
		_E01B = _gatesMovementCurve.Evaluate(_interaction.ProgressTime);
		if (_E01C > _E01B)
		{
			if (_E01B <= _curveValueOnStartAutoClosing)
			{
				_E003();
			}
		}
		else
		{
			_E002();
		}
		_E01C = _E01B;
		base.transform.localPosition = Vector3.Lerp(ShutPosition, OpenPosition, _E01B);
	}

	private void _E001()
	{
		_E003();
		_E01B = 0f;
		float t = _gatesCloseMovementCurve.Evaluate(_interaction.ProgressTime);
		base.transform.localPosition = Vector3.Lerp(OpenPosition, ShutPosition, t);
	}

	private void _E002()
	{
		if (!_E01D && Singleton<BetterAudio>.Instantiated)
		{
			_E01D = true;
			_E01E = false;
			Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, _gatesOpenSound, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Collisions, 50, 1.1f, EOcclusionTest.Fast);
		}
	}

	private void _E003()
	{
		if (!_E01E && Singleton<BetterAudio>.Instantiated)
		{
			_E01E = true;
			_E01D = false;
			Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, _gatesCloseSound, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Collisions, 50, 1.1f, EOcclusionTest.Fast);
		}
	}
}
