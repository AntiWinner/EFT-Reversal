using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ExperienceSectionView : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	[SerializeField]
	private CustomTextMeshProUGUI _value;

	[SerializeField]
	private Image _icon;

	[Space]
	[SerializeField]
	private Transform _articlesContainer;

	[SerializeField]
	private ExperienceArticleView _articleTemplate;

	public void Show(_EC8D section)
	{
		ShowGameObject();
		_caption.text = section.Name;
		_value.text = section.Sum.ToThousandsString();
		if (_icon != null && section.Icon != null)
		{
			_icon.sprite = section.Icon;
		}
		UI.AddViewList(section.Articles, _articleTemplate, _articlesContainer, delegate(_EC8C article, ExperienceArticleView view)
		{
			view.Show(article);
		});
	}
}
