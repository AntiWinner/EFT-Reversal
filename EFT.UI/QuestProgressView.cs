using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class QuestProgressView : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _percentages;

	[SerializeField]
	private Image _progress;

	public void Show(_E933 quest)
	{
		var (num, num2) = quest.Progress;
		if (num.ApproxEquals(0f))
		{
			_progress.fillAmount = 1f;
			_percentages.text = _ED3E._E000(261918);
		}
		else
		{
			float num3 = num2 / num;
			_progress.fillAmount = num3;
			_percentages.text = Mathf.FloorToInt(num3 * 100f).ToString(_ED3E._E000(27314));
		}
	}
}
