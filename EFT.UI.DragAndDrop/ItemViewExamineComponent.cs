using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public sealed class ItemViewExamineComponent : SerializedMonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _progressLabel;

	[SerializeField]
	private Slider _progressSlider;

	private float m__E000;

	private CancellationTokenSource m__E001;

	public void Show(float duration)
	{
		_progressSlider.maxValue = 1f;
		base.gameObject.SetActive(value: true);
		this.m__E000 = duration;
		_E000().HandleExceptions();
	}

	private async Task _E000()
	{
		this.m__E001 = new CancellationTokenSource();
		Task task = Task.Delay((int)(this.m__E000 * 1000f));
		double utcNowUnix = _E5AD.UtcNowUnix;
		_E001(0f);
		do
		{
			await Task.Yield();
			if (this.m__E001.IsCancellationRequested)
			{
				break;
			}
			double num = (_E5AD.UtcNowUnix - utcNowUnix) / (double)this.m__E000;
			_E001(Mathf.Clamp01((float)num));
		}
		while (!task.IsCompleted);
	}

	private void _E001(float progress)
	{
		int num = Mathf.RoundToInt(progress * 100f);
		_progressLabel.text = num.ToString();
		_progressSlider.value = progress;
	}

	public void Destroy()
	{
		this.m__E001?.Cancel();
		Object.Destroy(base.gameObject);
	}
}
