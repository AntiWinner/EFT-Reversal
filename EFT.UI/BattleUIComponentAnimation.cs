using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace EFT.UI;

public sealed class BattleUIComponentAnimation : MonoBehaviour
{
	public const float VISIBILITY_DURATION = 3f;

	private const float _E000 = 0.5f;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	private CancellationTokenSource _E001;

	public async Task Show(bool autoHide, float delaySeconds = 0f)
	{
		CancellationTokenSource cancellationTokenSource = StopAnimation();
		if (delaySeconds.Positive())
		{
			await Task.Delay((int)(delaySeconds * 1000f));
			if (cancellationTokenSource.IsCancellationRequested)
			{
				return;
			}
		}
		await _canvasGroup.DOFade(1f, 0.5f);
		if (!cancellationTokenSource.IsCancellationRequested && autoHide)
		{
			await Task.Delay(3000);
			if (!cancellationTokenSource.IsCancellationRequested)
			{
				await Hide();
			}
		}
	}

	public async Task Hide(float delaySeconds = 0f)
	{
		CancellationTokenSource cancellationTokenSource = StopAnimation();
		if (delaySeconds.Positive())
		{
			await Task.Delay((int)(delaySeconds * 1000f));
			if (cancellationTokenSource.IsCancellationRequested)
			{
				return;
			}
		}
		await _canvasGroup.DOFade(0f, 0.5f);
	}

	public void Close()
	{
		StopAnimation();
		_canvasGroup.alpha = 0f;
	}

	public CancellationTokenSource StopAnimation()
	{
		this._E001?.Cancel(throwOnFirstException: false);
		this._E001 = new CancellationTokenSource();
		_canvasGroup.DOKill();
		return this._E001;
	}
}
