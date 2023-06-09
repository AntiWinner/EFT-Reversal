using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public class GlobalChatButton : UIElement
{
	[SerializeField]
	private Button _runChatClientButton;

	[SerializeField]
	private CustomTextMeshProUGUI _runChatClientLabel;

	[SerializeField]
	private Image _runChatClientImage;

	[SerializeField]
	private Color _connectColor;

	[SerializeField]
	private Color _disconnectColor;

	private const int _E2E1 = 5;

	private bool _E2E2;

	private _E79D _E2C7;

	private Coroutine _E2E3;

	private void Awake()
	{
		_runChatClientButton.onClick.AddListener(delegate
		{
			if (_E2E3 != null)
			{
				StopCoroutine(_E2E3);
			}
			_E2E3 = StartCoroutine(_E000());
			if (!_E2E2)
			{
				_E001();
			}
			else
			{
				_E002();
			}
		});
		_E002();
	}

	public void Show(_E79D social)
	{
		_E2C7 = social;
		_runChatClientButton.interactable = true;
		ShowGameObject();
	}

	private IEnumerator _E000()
	{
		_runChatClientButton.interactable = false;
		yield return new WaitForSeconds(5f);
		_runChatClientButton.interactable = true;
	}

	private void _E001()
	{
		_runChatClientLabel.text = _ED3E._E000(230901).Localized();
		_runChatClientImage.color = _disconnectColor;
		_E2E2 = true;
	}

	private void _E002()
	{
		_runChatClientLabel.text = _ED3E._E000(230937).Localized();
		_runChatClientImage.color = _connectColor;
		_E2E2 = false;
	}

	public override void Close()
	{
		if (_E2E3 != null)
		{
			StopCoroutine(_E2E3);
			_E2E3 = null;
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E003()
	{
		if (_E2E3 != null)
		{
			StopCoroutine(_E2E3);
		}
		_E2E3 = StartCoroutine(_E000());
		if (!_E2E2)
		{
			_E001();
		}
		else
		{
			_E002();
		}
	}
}
