using UnityEngine;

namespace EFT.UI;

public class SplashScreen : MonoBehaviour
{
	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
