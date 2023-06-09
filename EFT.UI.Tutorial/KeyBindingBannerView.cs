using UnityEngine;

namespace EFT.UI.Tutorial;

public class KeyBindingBannerView : MonoBehaviour
{
	public Transform VerticalLayoutTransform;

	[SerializeField]
	private KeyBindingView _keyBindingView;

	private readonly int m__E000 = 4;

	internal void _E000(_EC82[] keyBindings)
	{
		Transform transform = VerticalLayoutTransform;
		for (int i = 0; i < keyBindings.Length; i++)
		{
			if (i % this.m__E000 == 0)
			{
				transform = Object.Instantiate(VerticalLayoutTransform, base.transform);
				transform.gameObject.SetActive(value: true);
			}
			GameObject obj = Object.Instantiate(_keyBindingView.gameObject, transform);
			obj.SetActive(value: true);
			obj.GetComponent<KeyBindingView>()._E000(keyBindings[i]);
		}
	}
}
