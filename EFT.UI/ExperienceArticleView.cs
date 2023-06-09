using UnityEngine;

namespace EFT.UI;

public sealed class ExperienceArticleView : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	[SerializeField]
	private CustomTextMeshProUGUI _value;

	public void Show(_EC8C article)
	{
		ShowGameObject();
		_caption.text = article.Name;
		_value.text = article.Value.ToThousandsString();
	}
}
