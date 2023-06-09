using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public abstract class BaseUiWindow : UIElement
{
	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private Button _closeButton;

	public bool IsActive => base.gameObject.activeSelf;

	protected virtual void Awake()
	{
		_closeButton.onClick.AddListener(Close);
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: false);
	}
}
