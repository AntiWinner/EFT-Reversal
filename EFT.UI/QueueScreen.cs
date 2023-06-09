using TMPro;
using UnityEngine;

namespace EFT.UI;

public class QueueScreen : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _queuePlace;

	[SerializeField]
	private DefaultUIButton _exitButton;

	private void Awake()
	{
		_exitButton.OnClick.AddListener(delegate
		{
			Application.Quit();
		});
	}

	public void SetQueue(int place, bool showExitButton)
	{
		ShowGameObject();
		_queuePlace.text = _ED3E._E000(248336).Localized() + _ED3E._E000(248320) + place + _ED3E._E000(47210);
		_exitButton.gameObject.SetActive(showExitButton);
	}
}
