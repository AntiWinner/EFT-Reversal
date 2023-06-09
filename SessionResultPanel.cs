using UnityEngine;
using UnityEngine.UI;

public class SessionResultPanel : MonoBehaviour
{
	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private Sprite _glow;

	[SerializeField]
	private Image _iconImage;

	[SerializeField]
	private Image _glowImage;

	private void Awake()
	{
		_iconImage.sprite = _icon;
		_glowImage.sprite = _glow;
	}
}
