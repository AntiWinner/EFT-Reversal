using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class SetNicknameScreen : EftScreen<SetNicknameScreen._E000, SetNicknameScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, SetNicknameScreen>
	{
		[CompilerGenerated]
		private new Action<string> m__E000;

		[CompilerGenerated]
		private Action<ENicknameError> _E001;

		public readonly string ReservedNickname;

		public override EEftScreenType ScreenType => EEftScreenType.SetNickname;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public event Action<string> OnNickNameSubmitted
		{
			[CompilerGenerated]
			add
			{
				Action<string> action = m__E000;
				Action<string> action2;
				do
				{
					action2 = action;
					Action<string> value2 = (Action<string>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<string> action = m__E000;
				Action<string> action2;
				do
				{
					action2 = action;
					Action<string> value2 = (Action<string>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action<ENicknameError> OnNicknameError
		{
			[CompilerGenerated]
			add
			{
				Action<ENicknameError> action = _E001;
				Action<ENicknameError> action2;
				do
				{
					action2 = action;
					Action<ENicknameError> value2 = (Action<ENicknameError>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E001, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<ENicknameError> action = _E001;
				Action<ENicknameError> action2;
				do
				{
					action2 = action;
					Action<ENicknameError> value2 = (Action<ENicknameError>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E001, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public _E000(string reservedNickname)
		{
			ReservedNickname = reservedNickname;
		}

		public void SubmitNickname(string nickname)
		{
			m__E000?.Invoke(nickname);
		}

		public void ShowNicknameError(ENicknameError error)
		{
			_E001?.Invoke(error);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E000 controller;

		public SetNicknameScreen _003C_003E4__this;

		internal void _E000()
		{
			controller.OnNicknameError -= _003C_003E4__this._E000;
		}
	}

	[SerializeField]
	private ValidationInputField _nicknameInput;

	[SerializeField]
	private DefaultUIButton _nextButton;

	private void Awake()
	{
		_nextButton.OnClick.AddListener(delegate
		{
			_E001(value: false);
			ScreenController.SubmitNickname(_nicknameInput.text);
		});
		_nicknameInput.HasError.Bind(delegate(bool hasError)
		{
			_nextButton.Interactable = !hasError;
		});
	}

	public override void Show(_E000 controller)
	{
		Show(controller.ReservedNickname);
		controller.OnNicknameError += _E000;
		UI.AddDisposable(delegate
		{
			controller.OnNicknameError -= _E000;
		});
	}

	private void _E000(ENicknameError error)
	{
		_nicknameInput.ShowError(error);
		_E001(value: true);
	}

	private void Show(string reservedNickname)
	{
		UI.Dispose();
		ShowGameObject();
		if (string.IsNullOrEmpty(_nicknameInput.text) && !string.IsNullOrEmpty(reservedNickname))
		{
			_nicknameInput.text = reservedNickname;
		}
		_E001(value: true);
	}

	private void _E001(bool value)
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(!value);
		_nextButton.gameObject.SetActive(value);
		_nicknameInput.interactable = value;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E001(value: false);
		ScreenController.SubmitNickname(_nicknameInput.text);
	}

	[CompilerGenerated]
	private void _E003(bool hasError)
	{
		_nextButton.Interactable = !hasError;
	}
}
