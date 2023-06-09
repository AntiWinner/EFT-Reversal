using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI;

public sealed class WelcomeScreen : EftScreen<WelcomeScreen._E000, WelcomeScreen>
{
	public new sealed class _E000 : _EC90<_E000, WelcomeScreen>
	{
		public readonly _E796 Session;

		public override EEftScreenType ScreenType => EEftScreenType.Welcome;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public _E000(_E796 session)
		{
			Session = session;
		}

		public void SetButtonAvailability(bool value)
		{
			if (_E002 != null)
			{
				_E002._E004(value);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string localizationString;

		public WelcomeScreen _003C_003E4__this;

		internal void _E000(bool arg)
		{
			if (arg)
			{
				_003C_003E4__this._E005(localizationString);
			}
		}
	}

	[SerializeField]
	private DefaultUIButton _acceptButton;

	[SerializeField]
	private GameObject _loadingCaption;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private GameObject _descriptionText;

	[SerializeField]
	private LocalizationButtonCap _toggleTemplate;

	[SerializeField]
	private RectTransform _togglesContainer;

	private new const string m__E000 = "ru";

	private const string m__E001 = "en";

	private const string m__E002 = "fr";

	private const string m__E003 = "ge";

	private _E796 m__E004;

	private Dictionary<string, LocalizationButtonCap> m__E005;

	private CancellationTokenSource m__E006;

	private bool _E007;

	public static string DefaultLanguage => Application.systemLanguage switch
	{
		SystemLanguage.Dutch => _ED3E._E000(253529), 
		SystemLanguage.French => _ED3E._E000(253524), 
		SystemLanguage.Russian => _ED3E._E000(47615), 
		_ => _ED3E._E000(36786), 
	};

	public override void Show(_E000 controller)
	{
		Show(controller.Session);
	}

	private void Show(_E796 session)
	{
		this.m__E004 = session;
		ShowGameObject();
		if (this.m__E005 == null)
		{
			this.m__E005 = new Dictionary<string, LocalizationButtonCap>
			{
				{
					_ED3E._E000(253524),
					null
				},
				{
					_ED3E._E000(253529),
					null
				},
				{
					_ED3E._E000(47615),
					null
				},
				{
					_ED3E._E000(36786),
					null
				}
			};
			for (int num = this.m__E005.Count - 1; num >= 0; num--)
			{
				string localizationString = this.m__E005.ElementAt(num).Key;
				LocalizationButtonCap localizationButtonCap = Object.Instantiate(_toggleTemplate, _togglesContainer);
				localizationButtonCap.Show(delegate(bool arg)
				{
					if (arg)
					{
						_E005(localizationString);
					}
				});
				localizationButtonCap.gameObject.SetActive(value: true);
				this.m__E005[localizationString] = localizationButtonCap;
			}
			_acceptButton.OnClick.AddListener(async delegate
			{
				_E000(value: false);
				await Singleton<_E7DE>.Instance.Game.Save();
				ScreenController._E000();
			});
		}
		this.m__E006 = new CancellationTokenSource();
		_acceptButton.Interactable = false;
		_E001().HandleExceptions();
		_E000(value: true);
	}

	private void _E000(bool value)
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(!value);
		_acceptButton.gameObject.SetActive(value);
		foreach (KeyValuePair<string, LocalizationButtonCap> item in this.m__E005)
		{
			_E39D.Deconstruct(item, out var _, out var value2);
			value2.CanvasGroup.interactable = value;
		}
	}

	private async Task _E001()
	{
		_E002(value: true);
		for (int num = this.m__E005.Count - 1; num >= 0; num--)
		{
			var (text2, localizationButtonCap2) = this.m__E005.ElementAt(num);
			await _E003(text2);
			if (this.m__E006.IsCancellationRequested)
			{
				break;
			}
			string text3 = _E7AD._E010._E005(text2);
			localizationButtonCap2.SetLoadedStatus(text3, value: true);
			if (text2 == DefaultLanguage)
			{
				if (!_E007)
				{
					localizationButtonCap2.SetToggle(value: true);
					_E005(text2);
				}
				_E002(value: false);
				_acceptButton.Interactable = true;
			}
		}
	}

	private void _E002(bool value)
	{
		_descriptionText.SetActive(!value);
		_loader.SetActive(value);
	}

	private Task _E003(string locale)
	{
		return _E77F.ReloadBackendLocale(null, this.m__E004, locale);
	}

	private void _E004(bool value)
	{
		_loadingCaption.SetActive(!value);
		_acceptButton.gameObject.SetActive(value);
	}

	private void _E005(string locale)
	{
		Singleton<_E7DE>.Instance.Game.Settings.Language.Value = locale;
		_E7AD._E010.UpdateApplicationLanguage();
		if (locale != DefaultLanguage)
		{
			_E007 = true;
			_E002(value: false);
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		this.m__E006?.Cancel();
		base.Close();
	}

	[CompilerGenerated]
	private async void _E006()
	{
		_E000(value: false);
		await Singleton<_E7DE>.Instance.Game.Save();
		ScreenController._E000();
	}
}
