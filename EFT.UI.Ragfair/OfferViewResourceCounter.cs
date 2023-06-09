using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class OfferViewResourceCounter : UIElement
{
	[SerializeField]
	private Image _resourceIcon;

	[SerializeField]
	private TextMeshProUGUI _resourceCount;

	public void Show(Sprite icon, string resource)
	{
		_resourceIcon.sprite = icon;
		_resourceCount.text = resource;
		base.gameObject.SetActive(value: true);
	}
}
