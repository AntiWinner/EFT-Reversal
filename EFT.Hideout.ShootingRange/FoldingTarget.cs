using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class FoldingTarget : MonoBehaviour, _E84E
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public FoldingTarget _003C_003E4__this;

		public float percent;

		internal void _E000()
		{
			_003C_003E4__this.PlayFoldAudio(percent);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public FoldingTarget _003C_003E4__this;

		public float percent;

		internal void _E000()
		{
			_003C_003E4__this.PlayUnfoldAudio(percent);
		}
	}

	[SerializeField]
	[Range(0f, 1f)]
	private float _duration = 1f;

	[Space]
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Vector3 _targetUpRotation;

	[SerializeField]
	private Vector3 _targetDownRotation;

	[SerializeField]
	[Space]
	private AudioSource _foldAudio;

	[SerializeField]
	private AudioSource _unfoldAudio;

	private Quaternion m__E000;

	private Quaternion m__E001;

	private float _E002;

	private FoldingTargetState _E003;

	private AudioSource _E004;

	private Sequence _E005;

	public bool Unfolded => _E003 == FoldingTargetState.Unfold;

	public bool Folded => _E003 == FoldingTargetState.Fold;

	private void Awake()
	{
		this.m__E000 = Quaternion.Euler(_targetUpRotation);
		this.m__E001 = Quaternion.Euler(_targetDownRotation);
		_E002 = Quaternion.Angle(this.m__E000, this.m__E001);
	}

	public Task Fold(bool mute, CancellationToken token)
	{
		float percent = _E000(_target.localRotation, this.m__E001);
		float duration = _E001(percent);
		_E005.Kill();
		_E005 = DOTween.Sequence();
		_E005.AppendCallback(OnStartCommon);
		if (!mute)
		{
			_E005.AppendCallback(delegate
			{
				PlayFoldAudio(percent);
			});
		}
		_E005.Append(_target.DOLocalRotateQuaternion(this.m__E001, duration).SetEase(Ease.Linear));
		_E005.AppendCallback(OnCompleteFold);
		return _E005.AsTask(token);
	}

	public Task Unfold(bool mute, CancellationToken token)
	{
		float percent = _E000(_target.localRotation, this.m__E000);
		float duration = _E001(percent);
		_E005.Kill();
		_E005 = DOTween.Sequence();
		_E005.AppendCallback(OnStartCommon);
		if (!mute)
		{
			_E005.AppendCallback(delegate
			{
				PlayUnfoldAudio(percent);
			});
		}
		_E005.Append(_target.DOLocalRotateQuaternion(this.m__E000, duration).SetEase(Ease.Linear));
		_E005.AppendCallback(OnCompleteUnfold);
		return _E005.AsTask(token);
	}

	public Task Rocking(CancellationToken token)
	{
		return Task.CompletedTask;
	}

	public void OnStartCommon()
	{
		_E003 = FoldingTargetState.None;
	}

	public void OnCompleteFold()
	{
		_E003 = FoldingTargetState.Fold;
	}

	public void OnCompleteUnfold()
	{
		_E003 = FoldingTargetState.Unfold;
	}

	private float _E000(Quaternion leftRotation, Quaternion rightRotation)
	{
		return Quaternion.Angle(leftRotation, rightRotation) / _E002;
	}

	private float _E001(float percent)
	{
		return percent * _duration;
	}

	public void PlayFoldAudio(float volume)
	{
		PlayAudio(_foldAudio, volume);
	}

	public void PlayUnfoldAudio(float volume)
	{
		PlayAudio(_unfoldAudio, volume);
	}

	public void PlayAudio(AudioSource audioSource, float volume)
	{
		_E004?.Stop();
		_E004 = audioSource;
		_E004.volume = volume;
		_E004.Play();
	}
}
