using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class ProgressSpinner : UIElement
{
	[SerializeField]
	private Image _fillImage;

	[SerializeField]
	private Image _backgroundImage;

	public void Show()
	{
		ShowGameObject();
		SetProgress(0f);
	}

	public void SetProgress(float progress)
	{
		if (_fillImage != null)
		{
			_fillImage.fillAmount = progress;
		}
	}

	public void SetColor(Color color)
	{
		_fillImage.color = color;
		_backgroundImage.color = color;
	}
}
