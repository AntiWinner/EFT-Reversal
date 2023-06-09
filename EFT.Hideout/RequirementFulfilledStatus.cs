using EFT.UI;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.Hideout;

public sealed class RequirementFulfilledStatus : UIElement
{
	[SerializeField]
	private Image _requirementIcon;

	[SerializeField]
	private Sprite _fulfilledSprite;

	[SerializeField]
	private Sprite _failedSprite;

	public void Show(bool fulfilled)
	{
		ShowGameObject();
		_requirementIcon.sprite = (fulfilled ? _fulfilledSprite : _failedSprite);
	}
}
