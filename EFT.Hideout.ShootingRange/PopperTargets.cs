using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

[Serializable]
public class PopperTargets : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public PopperTargets _003C_003E4__this;

		public CancellationToken token;

		internal Task _E000(PopperTarget target)
		{
			return _003C_003E4__this.Fold(target, mute: true, token);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public PopperTargets _003C_003E4__this;

		public CancellationToken token;

		internal Task _E000(PopperTarget target)
		{
			return _003C_003E4__this.Unfold(target, mute: true, token);
		}
	}

	[SerializeField]
	private int _rows;

	[SerializeField]
	private int _cols;

	[SerializeField]
	[Space]
	private PopperTarget[] _targets;

	[Space]
	[SerializeField]
	private AudioSource _foldAudio;

	[SerializeField]
	private AudioSource _unfoldAudio;

	private CancellationTokenSource _cancellationToken;

	public PopperTarget[] Targets => _targets;

	public int Cols => _cols;

	public event Action<PopperTarget, int> OnHitTarget;

	protected void OnEnable()
	{
		PopperTarget[] targets = _targets;
		for (int i = 0; i < targets.Length; i++)
		{
			targets[i].OnHitTarget += _E000;
		}
	}

	protected void OnDisable()
	{
		PopperTarget[] targets = _targets;
		for (int i = 0; i < targets.Length; i++)
		{
			targets[i].OnHitTarget -= _E000;
		}
	}

	private void _E000(PopperTarget target, int segmentTarget)
	{
		this.OnHitTarget?.Invoke(target, segmentTarget);
	}

	public Task Fold(CancellationToken token)
	{
		return Task.WhenAll(_E001(token));
	}

	public Task Fold(_E84E target, bool mute, CancellationToken token)
	{
		return target.Fold(mute, token);
	}

	private Task[] _E001(CancellationToken token)
	{
		if (_targets.Any((PopperTarget target) => !target.Folded))
		{
			_foldAudio.Play();
		}
		return _targets.Select((PopperTarget target) => Fold(target, mute: true, token)).ToArray();
	}

	public Task Unfold(CancellationToken token)
	{
		return Task.WhenAll(_E002(token));
	}

	public Task Unfold(_E84E target, bool mute, CancellationToken token)
	{
		return target.Unfold(mute, token);
	}

	private Task[] _E002(CancellationToken token)
	{
		if (_targets.Any((PopperTarget target) => !target.Unfolded))
		{
			_unfoldAudio.Play();
		}
		return _targets.Select((PopperTarget target) => Unfold(target, mute: true, token)).ToArray();
	}

	private void _E003()
	{
		_cancellationToken?.Cancel();
		_cancellationToken = new CancellationTokenSource();
	}

	private async void _E004()
	{
		_E003();
		await Fold(_cancellationToken.Token);
	}

	private async void _E005()
	{
		_E003();
		await Unfold(_cancellationToken.Token);
	}
}
