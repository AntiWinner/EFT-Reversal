using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class FoldingPopperTarget : MonoBehaviour, _E84E
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public FoldingPopperTarget _003C_003E4__this;

		public float targetPercent;

		internal void _E000()
		{
			_003C_003E4__this.PlayFoldAudio(targetPercent);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public FoldingPopperTarget _003C_003E4__this;

		public float targetPercent;

		internal void _E000()
		{
			_003C_003E4__this.PlayUnfoldAudio(targetPercent);
		}
	}

	[Range(0f, 1f)]
	[SerializeField]
	private float _duration = 1f;

	[SerializeField]
	[Space]
	private Transform _target;

	[SerializeField]
	private AnimationCurve _targetUpEase;

	[SerializeField]
	private AnimationCurve _targetDownEase;

	[SerializeField]
	private Vector3 _targetUpRotation;

	[SerializeField]
	private Vector3 _targetDownRotation;

	[SerializeField]
	[Space]
	private Transform _roller;

	[SerializeField]
	private AnimationCurve _rollerUpEase;

	[SerializeField]
	private AnimationCurve _rollerDownEase;

	[SerializeField]
	private Vector3 _rollerUpRotation;

	[SerializeField]
	private Vector3 _rollerDownRotation;

	[SerializeField]
	[Space]
	private AudioSource[] _foldAudios;

	[SerializeField]
	private AudioSource _unfoldAudio;

	[SerializeField]
	private AudioSource _rockingAudio;

	private Quaternion m__E000;

	private Quaternion m__E001;

	private Quaternion _E002;

	private Quaternion _E003;

	private float _E004;

	private float _E005;

	private FoldingTargetState _E006 = FoldingTargetState.Unfold;

	private float _E007;

	private AudioSource _E008;

	private Sequence _E009;

	public bool Folded => _E006 == FoldingTargetState.Fold;

	public bool Unfolded => _E006 == FoldingTargetState.Unfold;

	public float StateTime => Time.time - _E007;

	private void Awake()
	{
		this.m__E000 = Quaternion.Euler(_targetUpRotation);
		this.m__E001 = Quaternion.Euler(_targetDownRotation);
		_E002 = Quaternion.Euler(_rollerUpRotation);
		_E003 = Quaternion.Euler(_rollerDownRotation);
		_E004 = Quaternion.Angle(this.m__E000, this.m__E001);
		_E005 = Quaternion.Angle(_E002, _E003);
	}

	public Task Fold(bool mute, CancellationToken token)
	{
		float targetPercent = _E000(_target.localRotation, this.m__E001, _E004);
		float percent = _E000(_roller.localRotation, _E003, _E005);
		float duration = _E001(targetPercent);
		float duration2 = _E001(percent);
		_E009.Kill();
		_E009 = DOTween.Sequence();
		_E009.AppendCallback(OnStartCommon);
		if (!mute)
		{
			_E009.AppendCallback(delegate
			{
				PlayFoldAudio(targetPercent);
			});
		}
		_E009.Append(_target.DOLocalRotateQuaternion(Quaternion.Euler(_targetDownRotation), duration).SetEase(_targetDownEase));
		_E009.Join(_roller.DOLocalRotateQuaternion(Quaternion.Euler(_rollerDownRotation), duration2).SetEase(_rollerDownEase));
		_E009.AppendCallback(OnCompleteFold);
		return _E009.AsTask(token);
	}

	public Task Unfold(bool mute, CancellationToken token)
	{
		float targetPercent = _E000(_target.localRotation, this.m__E000, _E004);
		float percent = _E000(_roller.localRotation, _E002, _E005);
		float duration = _E001(targetPercent);
		float duration2 = _E001(percent);
		_E009.Kill();
		_E009 = DOTween.Sequence();
		_E009.AppendCallback(OnStartCommon);
		if (!mute)
		{
			_E009.AppendCallback(delegate
			{
				PlayUnfoldAudio(targetPercent);
			});
		}
		_E009.Append(_target.DOLocalRotateQuaternion(Quaternion.Euler(_targetUpRotation), duration).SetEase(_targetUpEase));
		_E009.Join(_roller.DOLocalRotateQuaternion(Quaternion.Euler(_rollerUpRotation), duration2).SetEase(_rollerUpEase));
		_E009.AppendCallback(OnCompleteUnfold);
		return _E009.AsTask(token);
	}

	public Task Rocking(CancellationToken token)
	{
		_E009.Kill();
		_E009 = DOTween.Sequence();
		_E009.AppendCallback(PlayRockingAudio);
		_E009.Append(_target.DOLocalRotateQuaternion(this.m__E000 * Quaternion.Euler(10f, 0f, 0f), 0.05f).SetEase(Ease.Linear));
		_E009.Join(_roller.DOLocalRotateQuaternion(_E002 * Quaternion.Euler(10f, 0f, 0f), 0.05f).SetEase(Ease.Linear));
		_E009.Append(_target.DOLocalRotateQuaternion(this.m__E000, 0.05f).SetEase(Ease.Linear));
		_E009.Join(_roller.DOLocalRotateQuaternion(_E002, 0.05f).SetEase(Ease.Linear));
		return _E009.AsTask(token);
	}

	public void OnStartCommon()
	{
		_E006 = FoldingTargetState.None;
		_E007 = Time.time;
	}

	public void OnCompleteFold()
	{
		_E006 = FoldingTargetState.Fold;
		_E007 = Time.time;
	}

	public void OnCompleteUnfold()
	{
		_E006 = FoldingTargetState.Unfold;
		_E007 = Time.time;
	}

	private float _E000(Quaternion leftRotation, Quaternion rightRotation, float defaultAngle)
	{
		return Quaternion.Angle(leftRotation, rightRotation) / defaultAngle;
	}

	private float _E001(float percent)
	{
		return percent * _duration;
	}

	public void PlayFoldAudio(float volume)
	{
		PlayAudio(_foldAudios[Random.Range(0, 2)], volume);
	}

	public void PlayUnfoldAudio(float volume)
	{
		PlayAudio(_unfoldAudio, volume);
	}

	public void PlayRockingAudio()
	{
		PlayAudio(_rockingAudio, 1f);
	}

	public void PlayAudio(AudioSource audioSource, float volume)
	{
		_E008?.Stop();
		_E008 = audioSource;
		_E008.volume = volume;
		_E008.Play();
	}
}
