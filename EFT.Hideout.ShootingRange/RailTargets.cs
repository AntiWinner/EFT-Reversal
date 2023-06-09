using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

[Serializable]
public class RailTargets : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public RailTargets _003C_003E4__this;

		public CancellationToken token;

		internal Task _E000(RailTarget target)
		{
			return _003C_003E4__this.Fold(target, mute: true, token);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public RailTargets _003C_003E4__this;

		public CancellationToken token;

		internal Task _E000(RailTarget target)
		{
			return _003C_003E4__this.Unfold(target, mute: true, token);
		}
	}

	[SerializeField]
	private RailTarget[] _targets;

	[Space]
	[SerializeField]
	private AudioSource _foldAudio;

	[SerializeField]
	private AudioSource _unfoldAudio;

	private CancellationTokenSource _cancellationToken;

	public RailTarget[] Targets => _targets;

	public event Action<RailTarget, TargetColliderType, int> OnHitTarget;

	protected void OnEnable()
	{
		RailTarget[] targets = _targets;
		for (int i = 0; i < targets.Length; i++)
		{
			targets[i].OnHitTarget += _E000;
		}
	}

	protected void OnDisable()
	{
		RailTarget[] targets = _targets;
		for (int i = 0; i < targets.Length; i++)
		{
			targets[i].OnHitTarget -= _E000;
		}
	}

	private void _E000(RailTarget target, TargetColliderType targetColliderType, int segmentTarget)
	{
		this.OnHitTarget?.Invoke(target, targetColliderType, segmentTarget);
	}

	public Task Shooting(CancellationToken token)
	{
		return Shooting(_targets, token);
	}

	public Task Shooting(RailTarget[] targets, CancellationToken token)
	{
		return Task.WhenAll(_E001(targets, token));
	}

	private Task[] _E001(RailTarget[] targets, CancellationToken token)
	{
		List<Task> list = new List<Task>();
		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].TryShooting(token, out var task))
			{
				list.Add(task);
			}
		}
		return list.ToArray();
	}

	public Task ResetShooting(CancellationToken token)
	{
		return ResetShooting(_targets, token);
	}

	public Task ResetShooting(RailTarget[] targets, CancellationToken token)
	{
		return Task.WhenAll(_E002(targets, token));
	}

	private Task[] _E002(RailTarget[] targets, CancellationToken token)
	{
		List<Task> list = new List<Task>();
		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].TryResetShooting(token, out var tween))
			{
				list.Add(tween);
			}
		}
		return list.ToArray();
	}

	public Task Inspect(CancellationToken token)
	{
		return Inspect(_targets, token);
	}

	public Task Inspect(RailTarget[] targets, CancellationToken token)
	{
		return Task.WhenAll(_E003(targets, token));
	}

	private Task[] _E003(RailTarget[] targets, CancellationToken token)
	{
		List<Task> list = new List<Task>();
		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].TryInspect(token, out var task))
			{
				list.Add(task);
			}
		}
		return list.ToArray();
	}

	public Task ResetInspect(CancellationToken token)
	{
		return ResetInspect(_targets, token);
	}

	public Task ResetInspect(RailTarget[] targets, CancellationToken token)
	{
		return Task.WhenAll(_E004(targets, token));
	}

	private Task[] _E004(RailTarget[] targets, CancellationToken token)
	{
		List<Task> list = new List<Task>();
		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].TryResetInspect(token, out var tween))
			{
				list.Add(tween);
			}
		}
		return list.ToArray();
	}

	public Task RestoreInspect(CancellationToken token)
	{
		return RestoreInspect(_targets, token);
	}

	public Task RestoreInspect(RailTarget[] targets, CancellationToken token)
	{
		return Task.WhenAll(_E005(targets, token));
	}

	private Task[] _E005(RailTarget[] targets, CancellationToken token)
	{
		List<Task> list = new List<Task>();
		for (int i = 0; i < targets.Length; i++)
		{
			if (targets[i].TryRestoreInspect(token, out var tween))
			{
				list.Add(tween);
			}
		}
		return list.ToArray();
	}

	public void SetSpeed(int value)
	{
		SetSpeed(_targets, value);
	}

	public void SetSpeed(RailTarget[] targets, int value)
	{
		for (int i = 0; i < targets.Length; i++)
		{
			targets[i].SetMovementSpeed(value);
		}
	}

	public Task Fold(CancellationToken token)
	{
		return Task.WhenAll(_E006(token));
	}

	public Task Fold(_E84E target, bool mute, CancellationToken token)
	{
		return target.Fold(mute, token);
	}

	private Task[] _E006(CancellationToken token)
	{
		if (_targets.Any((RailTarget target) => !target.Folded))
		{
			_foldAudio.Play();
		}
		return _targets.Select((RailTarget target) => Fold(target, mute: true, token)).ToArray();
	}

	public Task Unfold(CancellationToken token)
	{
		return Task.WhenAll(_E007(token));
	}

	public Task Unfold(_E84E target, bool mute, CancellationToken token)
	{
		return target.Unfold(mute, token);
	}

	private Task[] _E007(CancellationToken token)
	{
		if (_targets.Any((RailTarget target) => !target.Unfolded))
		{
			_unfoldAudio.Play();
		}
		return _targets.Select((RailTarget target) => Unfold(target, mute: true, token)).ToArray();
	}

	private void _E008()
	{
		_cancellationToken?.Cancel();
		_cancellationToken = new CancellationTokenSource();
	}

	private async void _E009()
	{
		_E008();
		await Fold(_cancellationToken.Token);
	}

	private async void _E00A()
	{
		_E008();
		await Unfold(_cancellationToken.Token);
	}

	private async void _E00B()
	{
		_E008();
		await Shooting(_cancellationToken.Token);
	}

	private async void _E00C()
	{
		_E008();
		await ResetShooting(_cancellationToken.Token);
	}

	private async void _E00D()
	{
		_E008();
		await Inspect(_cancellationToken.Token);
	}

	private async void _E00E()
	{
		_E008();
		await ResetInspect(_cancellationToken.Token);
	}
}
