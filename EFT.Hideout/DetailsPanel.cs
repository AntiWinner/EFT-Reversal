using EFT.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

public class DetailsPanel : UIElement
{
	[SerializeField]
	private Image _colorPanel;

	[SerializeField]
	private Image _detailsIcon;

	[SerializeField]
	private TextMeshProUGUI _detailsStatus;

	public void SetIcon(Sprite icon)
	{
		_detailsIcon.sprite = icon;
		_detailsIcon.SetNativeSize();
	}

	public void SetBackgroundColor(Color color)
	{
		_colorPanel.color = color;
	}

	public void SetText(string text)
	{
		_detailsStatus.SetMonospaceText(text);
	}
}
