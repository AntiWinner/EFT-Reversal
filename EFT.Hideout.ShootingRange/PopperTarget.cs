using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

[RequireComponent(typeof(FoldingPopperTarget))]
public class PopperTarget : MonoBehaviour, _E84E
{
	[SerializeField]
	private int _row;

	[SerializeField]
	[Space]
	private PaperTarget _paperTarget;

	private FoldingPopperTarget _foldingPopperTarget;

	private CancellationTokenSource _cancellationToken;

	public int Row => _row;

	public bool Unfolded => _foldingPopperTarget.Unfolded;

	public bool Folded => _foldingPopperTarget.Folded;

	public bool Ready => _foldingPopperTarget.Unfolded;

	public float ReadyTime => _foldingPopperTarget.StateTime;

	public event Action<PopperTarget, int> OnHitTarget;

	private void Awake()
	{
		_foldingPopperTarget = GetComponent<FoldingPopperTarget>();
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
		this.OnHitTarget?.Invoke(this, segmentTarget);
	}

	public Task Fold(bool mute, CancellationToken token)
	{
		return _foldingPopperTarget.Fold(mute, token);
	}

	public Task Unfold(bool mute, CancellationToken token)
	{
		return _foldingPopperTarget.Unfold(mute, token);
	}

	public Task Rocking(CancellationToken token)
	{
		return _foldingPopperTarget.Rocking(token);
	}

	private void _E001()
	{
		_cancellationToken?.Cancel();
		_cancellationToken = new CancellationTokenSource();
	}

	private async void _E002()
	{
		_E001();
		await Fold(mute: false, _cancellationToken.Token);
	}

	private async void _E003()
	{
		_E001();
		await Unfold(mute: false, _cancellationToken.Token);
	}

	private async void _E004()
	{
		_E001();
		await Rocking(_cancellationToken.Token);
	}
}
