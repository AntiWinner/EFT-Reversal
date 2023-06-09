using System;
using Comfort.Common;
using EFT.InputSystem;
using UnityEngine;

namespace EFT.UI;

public sealed class UnknownErrorScreen : UIInputNode
{
	private const string _E05A = "ERROR";

	private Action _E05B;

	[SerializeField]
	private CustomTextMeshProUGUI _errorHeader;

	[SerializeField]
	private CustomTextMeshProUGUI _errorMessage;

	[SerializeField]
	private CustomTextMeshProUGUI _errorDescription;

	[SerializeField]
	private DefaultUIButton _closeButton;

	[SerializeField]
	private DefaultUIButton _copyButton;

	private void Awake()
	{
		_copyButton.OnClick.AddListener(_E001);
		_closeButton.OnClick.AddListener(_E000);
	}

	public void Show(string header, string message, string fullDescription, Action acceptCallback = null)
	{
		ShowGameObject();
		_E05B = acceptCallback;
		if (Singleton<GUISounds>.Instantiated)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
		}
		_errorHeader.text = (string.IsNullOrEmpty(header) ? _ED3E._E000(249938) : header);
		_errorMessage.text = message;
		_errorDescription.text = fullDescription;
		ItemUiContext.Instance.CloseAllWindows();
		MonoBehaviourSingleton<PreloaderUI>.Instance.ClearErrorQueue();
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		axes = null;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			_E000();
		}
		return ETranslateResult.BlockAll;
	}

	private void Update()
	{
		if (base.isActiveAndEnabled)
		{
			UIEventSystem.Instance.Enable();
		}
	}

	private void _E000()
	{
		_E05B?.Invoke();
		Close();
	}

	private void _E001()
	{
		GUIUtility.systemCopyBuffer = _errorDescription.text;
	}
}
