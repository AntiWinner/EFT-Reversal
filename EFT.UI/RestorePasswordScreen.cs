using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI;

public sealed class RestorePasswordScreen : EftScreen<RestorePasswordScreen._E000, RestorePasswordScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, RestorePasswordScreen>
	{
		[CompilerGenerated]
		private new Action<string> m__E000;

		public override EEftScreenType ScreenType => EEftScreenType.RestorePassword;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public event Action<string> OnRestore
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

		internal void _E000(string login)
		{
			this.m__E000?.Invoke(login);
		}
	}

	[SerializeField]
	private ValidationInputField _login;

	[SerializeField]
	private DefaultUIButton _acceptButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	private void Awake()
	{
		_acceptButton.OnClick.AddListener(delegate
		{
			ScreenController._E000(_login.text);
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
	}

	public override void Show(_E000 controller)
	{
		Show();
	}

	private void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public override void Close()
	{
		_login.text = string.Empty;
		base.Close();
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
	private void _E000()
	{
		ScreenController._E000(_login.text);
	}

	[CompilerGenerated]
	private void _E001()
	{
		ScreenController.CloseScreen();
	}
}
