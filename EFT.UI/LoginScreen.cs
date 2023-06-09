using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class LoginScreen : EftScreen<LoginScreen._E000, LoginScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, LoginScreen>
	{
		[CompilerGenerated]
		private new Action<string, string, bool, Action<string, double>> m__E000;

		[CompilerGenerated]
		private Action m__E001;

		public readonly string Login;

		public readonly string Password;

		public readonly bool SavePassword;

		public override EEftScreenType ScreenType => EEftScreenType.Login;

		protected override bool MainEnvironment => true;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		public event Action<string, string, bool, Action<string, double>> OnLogin
		{
			[CompilerGenerated]
			add
			{
				Action<string, string, bool, Action<string, double>> action = this.m__E000;
				Action<string, string, bool, Action<string, double>> action2;
				do
				{
					action2 = action;
					Action<string, string, bool, Action<string, double>> value2 = (Action<string, string, bool, Action<string, double>>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<string, string, bool, Action<string, double>> action = this.m__E000;
				Action<string, string, bool, Action<string, double>> action2;
				do
				{
					action2 = action;
					Action<string, string, bool, Action<string, double>> value2 = (Action<string, string, bool, Action<string, double>>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRestore
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m__E001;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m__E001;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public _E000(string login, string password, bool savePassword)
		{
			Login = login;
			Password = password;
			SavePassword = savePassword;
		}

		internal void _E000(string login, string password, bool savePassword, Action<string, double> onError)
		{
			this.m__E000?.Invoke(login, password, savePassword, onError);
		}

		internal void _E001()
		{
			this.m__E001?.Invoke();
		}
	}

	[SerializeField]
	private QueueScreen _queuePanel;

	[SerializeField]
	private CustomTextMeshProInputField _loginInput;

	[SerializeField]
	private CustomTextMeshProInputField _passwordInput;

	[SerializeField]
	private Toggle _passwordToggle;

	[SerializeField]
	private CustomTextMeshProUGUI _errorLabel;

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _exitButton;

	[SerializeField]
	private GameObject _blockPanel;

	private new string m__E000;

	private bool m__E001;

	private void Awake()
	{
		_nextButton.OnClick.AddListener(_E001);
		_exitButton.OnClick.AddListener(delegate
		{
			Application.Quit();
		});
	}

	private void _E000(bool value)
	{
		_blockPanel.SetActive(value);
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Login, controller.Password, controller.SavePassword);
	}

	private void Show(string login, string password, bool saveToggle)
	{
		Display();
		SetNextButtonStatus(status: true);
		_loginInput.text = login;
		this.m__E000 = password;
		_passwordInput.text = password;
		_passwordToggle.isOn = saveToggle;
		_errorLabel.text = "";
		_E000(value: false);
	}

	private void _E001()
	{
		if (this.m__E001)
		{
			SetNextButtonStatus(status: false);
			if (this.m__E000 != _passwordInput.text)
			{
				this.m__E000 = _passwordInput.text;
				this.m__E000 = _E2C2.GetMd5Hash(this.m__E000);
			}
			ScreenController._E000(_loginInput.text, this.m__E000, _passwordToggle.isOn, _E003);
		}
	}

	private void _E002(string login, string password)
	{
		if (this.m__E001)
		{
			SetNextButtonStatus(status: false);
			_loginInput.text = login;
			_passwordInput.text = password;
			this.m__E000 = password;
			this.m__E000 = _E2C2.GetMd5Hash(this.m__E000);
			ScreenController._E000(login, this.m__E000, savePassword: false, _E003);
		}
	}

	private void _E003(string err, double banTime)
	{
		SetNextButtonStatus(status: true);
		_E000(value: false);
		_errorLabel.text = err.Localized() + _E004(banTime);
		_queuePanel.HideGameObject();
	}

	private static string _E004(double unix)
	{
		if (unix <= 0.0)
		{
			return string.Empty;
		}
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unix).ToLocalTime();
		return string.Concat(_ED3E._E000(2540), _ED3E._E000(248930).Localized(), _ED3E._E000(18502), dateTime, _ED3E._E000(2540), _ED3E._E000(248971).Localized());
	}

	public void SetNextButtonStatus(bool status)
	{
		_nextButton.gameObject.SetActive(status);
		this.m__E001 = status;
	}

	private void Update()
	{
		ConsoleScreen consoleScreen = MonoBehaviourSingleton<PreloaderUI>.Instance?.Console;
		if ((!(consoleScreen != null) || !consoleScreen.IsConsoleVisible) && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && !string.IsNullOrEmpty(_loginInput.text) && !string.IsNullOrEmpty(_passwordInput.text))
		{
			_E001();
		}
		else if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (_loginInput.isFocused)
			{
				_passwordInput.Select();
			}
			else if (_passwordInput.isFocused)
			{
				_loginInput.Select();
			}
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}
}
