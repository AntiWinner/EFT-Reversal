using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

[RequireComponent(typeof(FoldingTarget))]
public class RailTarget : MonoBehaviour, _E84E
{
	public enum MovementSpeed
	{
		Low,
		Medium,
		High
	}

	private enum HorizontalDirection
	{
		Right,
		Left
	}

	private enum VerticalDirection
	{
		Forward,
		Backward
	}

	private enum MovementType
	{
		Default,
		Horizontal,
		Vertical
	}

	[Space]
	[SerializeField]
	private int _row;

	[Space]
	[SerializeField]
	private Transform _leftPoint;

	[SerializeField]
	private Transform _rightPoint;

	[SerializeField]
	private Transform _frontPoint;

	[SerializeField]
	[Space]
	private float _lowMovementSpeed = 1f;

	[SerializeField]
	private float _mediumMovementSpeed = 2f;

	[SerializeField]
	private float _highMovementSpeed = 3f;

	[SerializeField]
	private float _inspectMovementSpeed = 4f;

	[SerializeField]
	private PaperTarget _paperTarget;

	[SerializeField]
	[Space]
	private Transform _target;

	[SerializeField]
	private Transform _rail;

	[Space]
	[SerializeField]
	private AudioSource _shootingStartAudio;

	[SerializeField]
	private AudioSource _shootingStopAudio;

	[SerializeField]
	private AudioSource _shootingLoopAudio;

	[SerializeField]
	private AudioSource[] _inspectLoopAudios;

	private Vector3 _defaultTargetPosition;

	private Vector3 _defaultRailPosition;

	private HorizontalDirection _targetDirection;

	private VerticalDirection _railDirection;

	private int CurrentIndexMovementSpeed;

	private float[] _movementSpeeds;

	private MovementType _currentMovement;

	private Sequence _currentSequence;

	private FoldingTarget _foldingTarget;

	private readonly float _defaultShootingLoopPitch = 0.8f;

	private readonly float _shootingLoopPitchIncrease = 0.1f;

	public int Row => _row;

	public bool Unfolded => _foldingTarget.Unfolded;

	public bool Folded => _foldingTarget.Folded;

	public bool Ready => _foldingTarget.Unfolded;

	public event Action<RailTarget, TargetColliderType, int> OnHitTarget;

	private void Awake()
	{
		_foldingTarget = GetComponent<FoldingTarget>();
		_movementSpeeds = new float[3] { _lowMovementSpeed, _mediumMovementSpeed, _highMovementSpeed };
	}

	private void Start()
	{
		_defaultTargetPosition = _target.position;
		_defaultRailPosition = _rail.position;
	}

	protected void OnEnable()
	{
		if (!(_paperTarget == null))
		{
			_paperTarget.OnHitTarget += _E000;
		}
	}

	protected void OnDisable()
	{
		if (!(_paperTarget == null))
		{
			_paperTarget.OnHitTarget -= _E000;
		}
	}

	private void _E000(TargetColliderType targetColliderType, int segmentTarget)
	{
		this.OnHitTarget?.Invoke(this, targetColliderType, segmentTarget);
	}

	public void SetMovementSpeed(int currentMovementSpeed)
	{
		CurrentIndexMovementSpeed = currentMovementSpeed;
	}

	public Task Fold(bool mute, CancellationToken token)
	{
		return _foldingTarget.Fold(mute, token);
	}

	public Task Unfold(bool mute, CancellationToken token)
	{
		return _foldingTarget.Unfold(mute, token);
	}

	public Task Rocking(CancellationToken token)
	{
		return _foldingTarget.Rocking(token);
	}

	private Task _E001(CancellationToken token)
	{
		_currentSequence.Kill();
		_currentSequence = DOTween.Sequence();
		Sequence sequence = DOTween.Sequence();
		sequence.Append(_E004());
		sequence.Append(_E005());
		sequence.OnStart(_E002);
		sequence.OnKill(_E003);
		_currentSequence.Append(sequence);
		return _currentSequence.AsTask(token);
	}

	private void _E002()
	{
		_currentMovement = MovementType.Horizontal;
		ShootingStart(CurrentIndexMovementSpeed);
	}

	private void _E003()
	{
		ShootingStop();
	}

	public bool TryShooting(CancellationToken token, out Task task)
	{
		if (_currentMovement == MovementType.Vertical)
		{
			task = null;
			return false;
		}
		task = _E001(token);
		return true;
	}

	private Tween _E004()
	{
		Sequence sequence = DOTween.Sequence();
		Vector3 a = ((_currentMovement == MovementType.Horizontal) ? _target.position : _defaultTargetPosition);
		Vector3 b = ((_targetDirection == HorizontalDirection.Right) ? _rightPoint.position : _leftPoint.position);
		float num = _movementSpeeds[CurrentIndexMovementSpeed];
		float duration = Vector3.Distance(a, b) * (1f / num);
		if (_targetDirection == HorizontalDirection.Right)
		{
			sequence.AppendCallback(_E007);
			sequence.Append(_E006(duration));
		}
		else
		{
			sequence.AppendCallback(_E009);
			sequence.Append(_E008(duration));
		}
		return sequence;
	}

	private Tween _E005()
	{
		Sequence sequence = DOTween.Sequence();
		float num = _movementSpeeds[CurrentIndexMovementSpeed];
		float duration = Vector3.Distance(_rightPoint.position, _leftPoint.position) * (1f / num);
		Tween t = _E006(duration);
		Tween t2 = _E008(duration);
		if (_targetDirection == HorizontalDirection.Right)
		{
			sequence.AppendCallback(_E009);
			sequence.Append(t2);
			sequence.AppendCallback(_E007);
			sequence.Append(t);
		}
		else
		{
			sequence.AppendCallback(_E007);
			sequence.Append(t);
			sequence.AppendCallback(_E009);
			sequence.Append(t2);
		}
		sequence.SetLoops(int.MaxValue);
		return sequence;
	}

	private Tween _E006(float duration)
	{
		return _target.DOMove(_rightPoint.position, duration).SetEase(Ease.Linear);
	}

	private void _E007()
	{
		_targetDirection = HorizontalDirection.Right;
	}

	private Tween _E008(float duration)
	{
		return _target.DOMove(_leftPoint.position, duration).SetEase(Ease.Linear);
	}

	private void _E009()
	{
		_targetDirection = HorizontalDirection.Left;
	}

	private Task _E00A(CancellationToken token)
	{
		_currentSequence.Kill();
		float num = _movementSpeeds[CurrentIndexMovementSpeed];
		float duration = Vector3.Distance(_target.position, _defaultTargetPosition) * (1f / num);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(_target.DOMove(_defaultTargetPosition, duration).SetEase(Ease.Linear).OnStart(_E00B)
			.OnComplete(_E00C)
			.OnKill(_E00D));
		_currentSequence = sequence;
		return _currentSequence.AsTask(token);
	}

	private void _E00B()
	{
		ShootingStart(CurrentIndexMovementSpeed);
	}

	private void _E00C()
	{
		_targetDirection = HorizontalDirection.Right;
		_currentMovement = MovementType.Default;
		ShootingStop();
	}

	private void _E00D()
	{
		ShootingStop();
	}

	public bool TryResetShooting(CancellationToken token, out Task tween)
	{
		if (_currentMovement == MovementType.Horizontal)
		{
			tween = _E00A(token);
			return true;
		}
		tween = null;
		return false;
	}

	private Task _E00E(CancellationToken token)
	{
		_currentSequence.Kill();
		float duration = Vector3.Distance(_rail.position, _frontPoint.position) * (1f / _inspectMovementSpeed);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(_rail.DOMove(_frontPoint.position, duration).SetEase(Ease.Linear).OnStart(_E00F)
			.OnComplete(_E010)
			.OnKill(_E011));
		_currentSequence = sequence;
		return _currentSequence.AsTask(token);
	}

	private void _E00F()
	{
		_railDirection = VerticalDirection.Backward;
		_currentMovement = MovementType.Vertical;
		InspectStart();
	}

	private void _E010()
	{
		InspectStop();
	}

	private void _E011()
	{
		InspectStop();
	}

	public bool TryInspect(CancellationToken token, out Task task)
	{
		if (_currentMovement == MovementType.Horizontal)
		{
			task = null;
			return false;
		}
		task = _E00E(token);
		return true;
	}

	private Task _E012(CancellationToken token)
	{
		_currentSequence.Kill();
		float duration = Vector3.Distance(_rail.position, _defaultRailPosition) * (1f / _inspectMovementSpeed);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(_rail.DOMove(_defaultRailPosition, duration).SetEase(Ease.Linear).OnStart(_E013)
			.OnComplete(_E014)
			.OnKill(_E015));
		_currentSequence = sequence;
		return _currentSequence.AsTask(token);
	}

	private void _E013()
	{
		_railDirection = VerticalDirection.Forward;
		InspectStart();
	}

	private void _E014()
	{
		_currentMovement = MovementType.Default;
		InspectStop();
	}

	private void _E015()
	{
		InspectStop();
	}

	public bool TryResetInspect(CancellationToken token, out Task tween)
	{
		if (_currentMovement == MovementType.Vertical)
		{
			tween = _E012(token);
			return true;
		}
		tween = null;
		return false;
	}

	public Task RestoreInspect(CancellationToken token)
	{
		if (_railDirection == VerticalDirection.Forward)
		{
			return _E012(token);
		}
		return _E00E(token);
	}

	public bool TryRestoreInspect(CancellationToken token, out Task tween)
	{
		if (_currentMovement == MovementType.Vertical)
		{
			tween = RestoreInspect(token);
			return true;
		}
		tween = null;
		return false;
	}

	public void ShootingStart(int speed)
	{
		_shootingLoopAudio.pitch = _defaultShootingLoopPitch + _shootingLoopPitchIncrease * (float)speed;
		_shootingStartAudio.Play();
		_shootingLoopAudio.Play();
	}

	public void ShootingStop()
	{
		_shootingLoopAudio.Stop();
		_shootingStopAudio.Play();
	}

	public void InspectStart()
	{
		_shootingStartAudio.Play();
		AudioSource[] inspectLoopAudios = _inspectLoopAudios;
		for (int i = 0; i < inspectLoopAudios.Length; i++)
		{
			inspectLoopAudios[i].Play();
		}
	}

	public void InspectStop()
	{
		AudioSource[] inspectLoopAudios = _inspectLoopAudios;
		for (int i = 0; i < inspectLoopAudios.Length; i++)
		{
			inspectLoopAudios[i].Stop();
		}
		_shootingStopAudio.Play();
	}
}
