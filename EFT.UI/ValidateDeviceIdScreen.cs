using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI;

public sealed class ValidateDeviceIdScreen : EftScreen<ValidateDeviceIdScreen._E000, ValidateDeviceIdScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, ValidateDeviceIdScreen>
	{
		[CompilerGenerated]
		private new Action<string> m__E000;

		public override EEftScreenType ScreenType => EEftScreenType.ValidateDeviceId;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public event Action<string> OnValidateDeviceId
		{
			[CompilerGenerated]
			add
			{
				Action<string> action = this.m__E000;
				Action<string> action2;
				do
				{
					action2 = action;
					Action<string> value2 = (Action<string>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<string> action = this.m__E000;
				Action<string> action2;
				do
				{
					action2 = action;
					Action<string> value2 = (Action<string>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		internal void _E000(string deviceId)
		{
			this.m__E000?.Invoke(deviceId);
		}

		public void ErrorShow(string err)
		{
			if (!base.Closed)
			{
				_E002._E001(err);
			}
		}
	}

	[SerializeField]
	private CustomTextMeshProInputField _deviceIdInputField;

	[SerializeField]
	private CustomTextMeshProUGUI _errorLabel;

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _cancelButton;

	private void Awake()
	{
		_nextButton.OnClick.AddListener(delegate
		{
			_nextButton.gameObject.SetActive(value: false);
			_E000();
		});
		_cancelButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_deviceIdInputField.onValueChanged.AddListener(delegate(string arg)
		{
			_deviceIdInputField.text = arg.ToUpper();
		});
	}

	public override void Show(_E000 controller)
	{
		Show();
	}

	private void Show()
	{
		_deviceIdInputField.text = "";
		_E001(string.Empty);
		_nextButton.gameObject.SetActive(value: true);
		Display();
	}

	private void _E000()
	{
		ScreenController._E000(_deviceIdInputField.text);
	}

	private void _E001(string err)
	{
		_nextButton.gameObject.SetActive(value: true);
		_errorLabel.text = err;
	}

	private void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !string.IsNullOrEmpty(_deviceIdInputField.text))
		{
			_E000();
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	[CompilerGenerated]
	private void _E002()
	{
		_nextButton.gameObject.SetActive(value: false);
		_E000();
	}

	[CompilerGenerated]
	private void _E003()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E004(string arg)
	{
		_deviceIdInputField.text = arg.ToUpper();
	}
}
