using UnityEngine;

namespace EFT.UI.Tutorial;

public class KeyBindingView : MonoBehaviour
{
	[SerializeField]
	private Transform _keyCombinationContainer;

	[SerializeField]
	private KeyView _keyView;

	[SerializeField]
	private CustomTextMeshProUGUI _keyBindingDescriptionText;

	internal void _E000(_EC82 keyBinding)
	{
		_keyBindingDescriptionText.text = keyBinding._E001;
		_EC81[] array = keyBinding._E000;
		foreach (_EC81 key in array)
		{
			GameObject obj = Object.Instantiate(_keyView.gameObject, _keyCombinationContainer);
			obj.SetActive(value: true);
			obj.GetComponent<KeyView>()._E002(key);
		}
	}
}
