using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Tutorial;

public class KeyView : MonoBehaviour
{
	[SerializeField]
	private Image _keyImage;

	[SerializeField]
	private CustomTextMeshProUGUI _keyText;

	private void _E000(Sprite imageSprite)
	{
		_keyImage.sprite = imageSprite;
		_keyImage.SetNativeSize();
	}

	private void _E001(string text)
	{
		_keyText.text = text;
	}

	internal void _E002(_EC81 key)
	{
		_E000(key._E000);
		_E001(key._E001);
	}
}
