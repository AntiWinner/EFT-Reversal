using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bsg.GameSettings;
using Comfort.Common;
using EFT.Settings.Graphics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Settings;

public sealed class GraphicsSettingsTab : SettingsTab
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public GraphicsSettingsTab _003C_003E4__this;

		public _E3A4 unsubs;

		public _E469<_E7E4, byte> displayProxy;

		public _E469<_E7E4, AspectRatio> aspectRatioProxy;

		public _E469<_E7E4, EftResolution> resolutionProxy;

		public _E469<_E7E4, FullScreenMode> fullScreenProxy;

		internal void _E000(_E7E4 displaySettings)
		{
			_003C_003E4__this._E277.DisplaySettings.Value = displaySettings;
			_003C_003E4__this._E00F();
		}

		internal void _E001(_E7E4 settings)
		{
			_E002();
		}

		internal void _E002()
		{
			_E002 CS_0024_003C_003E8__locals0 = new _E001();
			unsubs.Dispose();
			CS_0024_003C_003E8__locals0.displays = EftDisplay.Infos();
			ReadOnlyCollection<byte> variants = Array.AsReadOnly(CS_0024_003C_003E8__locals0.displays.Select((EftDisplay._E001 info) => info.Index).ToArray());
			_E3A4 disposable = SettingDropDown.BindTwoWaySettingDropDown(_003C_003E4__this._displaysDropdown, displayProxy, variants, delegate(byte index)
			{
				CS_0024_003C_003E8__locals0 = new _E002
				{
					index = index
				};
				return CS_0024_003C_003E8__locals0.displays.First((EftDisplay._E001 info) => info.Index == CS_0024_003C_003E8__locals0.index).FriendlyName;
			});
			unsubs.AddDisposable(disposable);
			disposable = SettingDropDown.BindTwoWaySettingDropDown(_003C_003E4__this._aspectRatio, aspectRatioProxy, AspectRatio.SortedAspectRatios, (AspectRatio x) => x.ToString());
			unsubs.AddDisposable(disposable);
			disposable = SettingDropDown.BindTwoWaySettingDropDown(_003C_003E4__this._resolutionBox, resolutionProxy, EftResolution.SortedResolutions, (EftResolution x) => x.ToString());
			unsubs.AddDisposable(disposable);
			disposable = SettingDropDown.BindTwoWaySettingDropDown(_003C_003E4__this._fullscreenDropdown, fullScreenProxy, _E272, (FullScreenMode mode) => _E273[_E272.IndexOf(mode)].Localized());
			unsubs.AddDisposable(disposable);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public List<EftDisplay._E001> displays;

		internal string _E000(byte index)
		{
			_E002 CS_0024_003C_003E8__locals0 = new _E002
			{
				index = index
			};
			return displays.First((EftDisplay._E001 info) => info.Index == CS_0024_003C_003E8__locals0.index).FriendlyName;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public byte index;

		internal bool _E000(EftDisplay._E001 info)
		{
			return info.Index == index;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public GameSetting<_E7E4> displaySettings;

		public _E7E4 oldSettings;

		public GraphicsSettingsTab _003C_003E4__this;

		public EDLSSMode oldDlssMode;

		internal void _E000()
		{
			_E001().HandleExceptions();
		}

		internal async Task _E001()
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.CloseErrorScreen();
			await displaySettings.SetValue(oldSettings);
			_003C_003E4__this._E278.DLSSMode.Value = oldDlssMode;
			_003C_003E4__this._E277.DLSSMode.TakeValueFrom(_003C_003E4__this._E278.DLSSMode);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public ReadOnlyCollection<string> labels;

		public DropDownBox dropdown;

		public GameSetting<int> setting;

		public GraphicsSettingsTab _003C_003E4__this;

		internal IEnumerable<string> _E000()
		{
			return ((IReadOnlyList<string>)labels).Localized(EStringCase.None);
		}

		internal void _E001(int x)
		{
			dropdown.UpdateValue(Mathf.Clamp(x, 0, labels.Count), sendCallback: false);
		}

		internal void _E002(int index)
		{
			setting.Value = index;
			_003C_003E4__this._E00E();
		}
	}

	[CompilerGenerated]
	private sealed class _E006<_E077>
	{
		public GameSetting<_E077> setting;

		public ReadOnlyCollection<_E077> variants;

		public GraphicsSettingsTab _003C_003E4__this;

		public SelectSlider slider;

		public Func<_E077, string> stringConverter;

		internal void _E000(int index)
		{
			setting.Value = variants[index];
			_003C_003E4__this._E00E();
		}

		internal void _E001(_E077 newValue)
		{
			if (SettingSelectSlider.UpdateSliderValue(slider, newValue, variants))
			{
				slider._E001(stringConverter(newValue));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public UpdatableToggle toggle;

		public GameSetting<bool> setting;

		public GraphicsSettingsTab _003C_003E4__this;

		internal void _E000(bool x)
		{
			toggle.UpdateValue(x, sendCallback: false);
		}

		internal void _E001(bool arg)
		{
			setting.Value = arg;
			_003C_003E4__this._E00E();
		}
	}

	[CompilerGenerated]
	private sealed class _E008<_E077> where _E077 : struct, Enum
	{
		public GameSetting<_E077> setting;

		public bool canSetCustomLabel;

		public GraphicsSettingsTab _003C_003E4__this;

		public DropDownBox dropdown;

		internal void _E000(int index)
		{
			setting.Value = _E3A5<_E077>.Values[index];
			if (canSetCustomLabel)
			{
				_003C_003E4__this._E00E();
			}
		}

		internal void _E001(_E077 newValue)
		{
			dropdown.UpdateValue(_E3A5<_E077>.IndexOf(setting), sendCallback: false);
		}
	}

	[CompilerGenerated]
	private sealed class _E009
	{
		public GraphicsSettingsTab _003C_003E4__this;

		public SettingDropDown dlssControl;

		internal void _E000(ESamplingMode _)
		{
			_E003();
		}

		internal void _E001(EFSRMode _)
		{
			_E003();
		}

		internal void _E002(EFSR2Mode _)
		{
			_E003();
		}

		internal void _E003()
		{
			if (_003C_003E4__this._E277.FSREnabled)
			{
				dlssControl.Blocker.StartBlock(_ED3E._E000(256883));
			}
			else if (_003C_003E4__this._E277.FSR2Enabled)
			{
				dlssControl.Blocker.StartBlock(_ED3E._E000(256913));
			}
			else
			{
				dlssControl.Blocker.SetBlock(!_E7EB.Is1XSampling(_003C_003E4__this._E277.SuperSampling), _ED3E._E000(234515));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E00A
	{
		public GraphicsSettingsTab _003C_003E4__this;

		public SettingDropDown fsrControl;

		internal void _E000(ESamplingMode _)
		{
			_E005();
		}

		internal void _E001(EAntialiasingMode _)
		{
			_E005();
		}

		internal void _E002(EDLSSMode _)
		{
			_E005();
		}

		internal void _E003(EFSR2Mode _)
		{
			_E005();
		}

		internal void _E004(_E7E4 _)
		{
			_E005();
		}

		internal void _E005()
		{
			if (_003C_003E4__this._E277.DLSSEnabled)
			{
				fsrControl.Blocker.StartBlock(_ED3E._E000(256844));
				return;
			}
			if (_003C_003E4__this._E277.FSR2Enabled)
			{
				fsrControl.Blocker.StartBlock(_ED3E._E000(256913));
				return;
			}
			EftResolution resolution = _003C_003E4__this._E277.DisplaySettings.Value.Resolution;
			if ((long)resolution.Width * (long)resolution.Height <= 2071680)
			{
				fsrControl.Blocker.StartBlock(_ED3E._E000(234539));
				return;
			}
			GameSetting<EAntialiasingMode> antiAliasing = _003C_003E4__this._E277.AntiAliasing;
			if ((EAntialiasingMode)antiAliasing != EAntialiasingMode.TAA_Low && (EAntialiasingMode)antiAliasing != EAntialiasingMode.TAA_High)
			{
				fsrControl.Blocker.StartBlock(_ED3E._E000(234561));
			}
			else
			{
				fsrControl.Blocker.SetBlock(!_E7EB.Is1XSampling(_003C_003E4__this._E277.SuperSampling), _ED3E._E000(234515));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E00B
	{
		public GraphicsSettingsTab _003C_003E4__this;

		public SettingDropDown fsr2Control;

		internal void _E000(ESamplingMode _)
		{
			_E004();
		}

		internal void _E001(EDLSSMode _)
		{
			_E004();
		}

		internal void _E002(EFSRMode _)
		{
			_E004();
		}

		internal void _E003(_E7E4 _)
		{
			_E004();
		}

		internal void _E004()
		{
			if (_003C_003E4__this._E277.DLSSEnabled)
			{
				fsr2Control.Blocker.StartBlock(_ED3E._E000(256844));
			}
			else if (_003C_003E4__this._E277.FSREnabled)
			{
				fsr2Control.Blocker.StartBlock(_ED3E._E000(256883));
			}
			else
			{
				fsr2Control.Blocker.SetBlock(!_E7EB.Is1XSampling(_003C_003E4__this._E277.SuperSampling), _ED3E._E000(234515));
			}
		}
	}

	private const string _E261 = "Settings/Graphics/Ultra";

	private const string _E262 = "Settings/Graphics/High";

	private const string _E263 = "Settings/Graphics/Medium";

	private const string _E264 = "Settings/Graphics/Low";

	private const string _E265 = "Settings/Graphics/VeryLow";

	private const string _E266 = "Settings/Graphics/Only1xSamplingAllowed";

	private const string _E267 = "Settings/Graphics/OnlyTaaAllowed";

	private const string _E268 = "Settings/Graphics/DLSSLockThis";

	private const string _E269 = "Settings/Graphics/FSRLockThis";

	private const string _E26A = "Settings/Graphics/FSRTooLowResolution";

	private const string _E26B = "Settings/Graphics/FSR2LockThis";

	private const long _E26C = 2071680L;

	private static readonly ReadOnlyCollection<float> _E26D = Array.AsReadOnly(new float[6] { 400f, 1000f, 1500f, 2000f, 2500f, 3000f });

	private static readonly ReadOnlyCollection<int> _E26E = Array.AsReadOnly(new int[4] { 16, 32, 48, 64 });

	private static readonly ReadOnlyCollection<int> _E26F = Array.AsReadOnly(new int[4] { 256, 512, 768, 1024 });

	private static readonly ReadOnlyCollection<float> _E270 = Array.AsReadOnly(new float[5] { 2f, 2.5f, 3f, 3.5f, 4f });

	private static readonly ReadOnlyCollection<float> _E271 = Array.AsReadOnly(new float[31]
	{
		0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f,
		1f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f,
		2f, 2.1f, 2.2f, 2.3f, 2.4f, 2.5f, 2.6f, 2.7f, 2.8f, 2.9f,
		3f
	});

	private static readonly ReadOnlyCollection<FullScreenMode> _E272 = Array.AsReadOnly(new FullScreenMode[3]
	{
		FullScreenMode.ExclusiveFullScreen,
		FullScreenMode.FullScreenWindow,
		FullScreenMode.Windowed
	});

	private static readonly ReadOnlyCollection<string> _E273 = Array.AsReadOnly(new string[3]
	{
		_ED3E._E000(256728),
		_ED3E._E000(256708),
		_ED3E._E000(256757)
	});

	private static readonly ReadOnlyCollection<string> _E274 = Array.AsReadOnly(new string[3]
	{
		_ED3E._E000(256750),
		_ED3E._E000(256788),
		_ED3E._E000(256829)
	});

	private static readonly ReadOnlyCollection<string> _E275 = Array.AsReadOnly(new string[4]
	{
		_ED3E._E000(256750),
		_ED3E._E000(256788),
		_ED3E._E000(256829),
		_ED3E._E000(256804)
	});

	[SerializeField]
	private DropDownBox _resolutionBox;

	[SerializeField]
	private UpdatableToggle _multimonitorSupport;

	[SerializeField]
	private DropDownBox _aspectRatio;

	[SerializeField]
	private UpdatableToggle _vSync;

	[SerializeField]
	private DropDownBox _fullscreenDropdown;

	[SerializeField]
	private DropDownBox _displaysDropdown;

	[SerializeField]
	private SelectSlider _graphicsQuality;

	[SerializeField]
	private DropDownBox _textureQuality;

	[SerializeField]
	private DropDownBox _shadowsQuality;

	[Space]
	[SerializeField]
	private DropDownBox _superSampling;

	[SerializeField]
	private UiElementBlocker _superSamplingBlocker;

	[SerializeField]
	[Space]
	private SelectSlider _objectLodQuality;

	[SerializeField]
	private SelectSlider _overallVisibility;

	[SerializeField]
	private SelectSlider _mipStreamingBuffersSize;

	[SerializeField]
	private SelectSlider _mipStreamingDiskUsageIntensity;

	[SerializeField]
	private DropDownBox _ssao;

	[SerializeField]
	private SelectSlider _sharpen;

	[SerializeField]
	private DropDownBox _screenSpaceReflections;

	[SerializeField]
	private DropDownBox _anisotropicFiltering;

	[SerializeField]
	[Space]
	private DropDownBox _antiAliasing;

	[SerializeField]
	private UiElementBlocker _antiAliasingBlocker;

	[Space]
	[SerializeField]
	private DropDownBox _nvidiaReflexTypeDropdown;

	[SerializeField]
	private UiElementBlocker[] _vSyncBlockers;

	[SerializeField]
	private UiElementBlocker[] _inRaidBlockers;

	[SerializeField]
	private UiElementBlocker[] _NVidiaReflexBlockers;

	[SerializeField]
	private UiElementBlocker[] _reflexNotAvailableBlockers;

	[SerializeField]
	private UiElementBlocker[] _mipStreamingParametersBlockers;

	[SerializeField]
	private FloatSlider _lobbyFramerate;

	[SerializeField]
	private FloatSlider _gameFramerate;

	[SerializeField]
	private UpdatableToggle _bloom;

	[SerializeField]
	private UpdatableToggle _zBlur;

	[SerializeField]
	private UpdatableToggle _highQualityColor;

	[SerializeField]
	private UpdatableToggle _chromaticAberrations;

	[SerializeField]
	private UpdatableToggle _noise;

	[SerializeField]
	private UpdatableToggle _grassShadow;

	[SerializeField]
	private UpdatableToggle _mipStreaming;

	[SerializeField]
	[Space]
	private Transform _otherSettingsContainer;

	[SerializeField]
	private SettingDropDown _dropDownPrefab;

	[SerializeField]
	private ScrollRect _settingsScroll;

	[SerializeField]
	private TextMeshProUGUI _recommendedVRAMValueText;

	private VRamUsageWrapper _E276;

	private _E7EB _E277;

	private _E7EB _E278;

	private bool _E279;

	public void Show(_E7EB graphicsSettings)
	{
		_E279 = false;
		_E277 = graphicsSettings;
		_E7DE._E000<_E7EB, _E7E9> graphics = Singleton<_E7DE>.Instance.Graphics;
		_E278 = graphics.Settings;
		UiElementBlocker[] inRaidBlockers = _inRaidBlockers;
		for (int i = 0; i < inRaidBlockers.Length; i++)
		{
			inRaidBlockers[i].SetBlock(_E7A3.InRaid, _ED3E._E000(256164));
		}
		_E00B();
		_E00C();
		_E000();
		SettingsTab.BindFloatSliderToSetting(_lobbyFramerate, _E277.LobbyFramerate, _E7E9.MinFramerateLimit, _E7E9.MaxFramerateLobbyLimit);
		SettingsTab.BindFloatSliderToSetting(_gameFramerate, _E277.GameFramerate, _E7E9.MinFramerateLimit, _E7E9.MaxFramerateGameLimit);
		_E00D();
		_E013(_sharpen, _E277.Sharpen, _E271, (float x) => x.ToString(_ED3E._E000(215469)));
		UI.SubscribeState(_E277.Sharpen, delegate
		{
			_E00F();
		});
		_E013(_objectLodQuality, _E277.LodBias, _E270, (float x) => x.ToString(_ED3E._E000(215469)));
		UI.SubscribeState(_E277.LodBias, delegate
		{
			_E00F();
		});
		_E013(_overallVisibility, _E277.OverallVisibility, _E26D, (float x) => x.ToString(_ED3E._E000(215469)));
		UI.SubscribeState(_E277.OverallVisibility, delegate
		{
			_E00F();
		});
		_E013(_mipStreamingBuffersSize, _E277.MipStreamingBufferSize, _E26E, (int x) => x.ToString(_ED3E._E000(215469)));
		UI.SubscribeState(_E277.MipStreamingBufferSize, delegate
		{
			_E00F();
		});
		_E013(_mipStreamingDiskUsageIntensity, _E277.MipStreamingIOCount, _E26F, (int x) => x.ToString(_ED3E._E000(215469)));
		UI.SubscribeState(_E277.MipStreamingIOCount, delegate
		{
			_E00F();
		});
		_E015(_ssao, _E277.Ssao);
		UI.SubscribeState(_E277.Ssao, delegate
		{
			_E00F();
		});
		_E015(_screenSpaceReflections, _E277.SSR);
		UI.SubscribeState(_E277.SSR, delegate
		{
			_E00F();
		});
		_E015(_anisotropicFiltering, _E277.AnisotropicFiltering);
		UI.SubscribeState(_E277.AnisotropicFiltering, delegate
		{
			_E00F();
		});
		_E015(_superSampling, _E277.SuperSampling);
		UI.SubscribeState(_E277.SuperSampling, delegate
		{
			_E00F();
		});
		_E015(_antiAliasing, _E277.AntiAliasing);
		UI.SubscribeState(_E277.AntiAliasing, delegate
		{
			_E00F();
		});
		_E014(_grassShadow, _E277.GrassShadow);
		UI.SubscribeState(_E277.GrassShadow, delegate
		{
			_E00F();
		});
		_E014(_zBlur, _E277.ZBlur);
		UI.SubscribeState(_E277.ZBlur, delegate
		{
			_E00F();
		});
		_E014(_chromaticAberrations, _E277.ChromaticAberrations);
		UI.SubscribeState(_E277.ChromaticAberrations, delegate
		{
			_E00F();
		});
		_E014(_noise, _E277.Noise);
		UI.SubscribeState(_E277.Noise, delegate
		{
			_E00F();
		});
		SettingsTab.ShowFakeToggle(_multimonitorSupport);
		SettingsTab.ShowFakeToggle(_bloom);
		SettingsTab.BindToggleToSetting(_vSync, _E277.VSync);
		UI.BindState(_E277.VSync, _E011);
		_E014(_highQualityColor, _E277.HighQualityColor);
		UI.SubscribeState(_E277.HighQualityColor, delegate
		{
			_E016();
		});
		UI.SubscribeState(_E277.HighQualityColor, delegate
		{
			_E00F();
		});
		_E00A();
		_E014(_mipStreaming, _E277.MipStreaming);
		UI.SubscribeState(_E277.MipStreaming, delegate
		{
			_E017();
		});
		UI.SubscribeState(_E277.MipStreaming, delegate
		{
			_E00F();
		});
		UI.SubscribeState(_E277.MipStreaming, delegate
		{
			_E00A();
		});
		_E009();
		_E015(_nvidiaReflexTypeDropdown, _E277.NVidiaReflex, canSetCustomLabel: false);
		UI.BindState(_E277.NVidiaReflex, _E010);
		int siblingIndex = _superSampling.transform.parent.GetSiblingIndex();
		SettingDropDown settingDropDown = CreateControl(_dropDownPrefab, _otherSettingsContainer);
		settingDropDown.GetOrCreateTooltip().SetMessageText(_ED3E._E000(256255));
		settingDropDown.transform.SetSiblingIndex(siblingIndex + 1);
		settingDropDown.BindToEnum(_E277.DLSSMode, twoWays: true, _E006, _E00E);
		_E018(settingDropDown);
		SettingDropDown settingDropDown2 = CreateControl(_dropDownPrefab, _otherSettingsContainer);
		settingDropDown2.GetOrCreateTooltip().SetMessageText(_ED3E._E000(256281));
		settingDropDown2.transform.SetSiblingIndex(siblingIndex + 2);
		settingDropDown2.BindToEnum(_E277.FSRMode, twoWays: true, _E007, _E00E);
		_E019(settingDropDown2);
		SettingDropDown settingDropDown3 = CreateControl(_dropDownPrefab, _otherSettingsContainer);
		settingDropDown3.GetOrCreateTooltip().SetMessageText(_ED3E._E000(256314));
		settingDropDown3.transform.SetSiblingIndex(siblingIndex + 3);
		settingDropDown3.BindToEnum(_E277.FSR2Mode, twoWays: true, _E008, _E00E);
		_E01A(settingDropDown3);
		_E01B();
		UI.SubscribeState(_E277.DLSSMode, delegate
		{
			_E00F();
		});
		UI.SubscribeState(_E277.FSRMode, delegate
		{
			_E00F();
		});
		UI.SubscribeState(_E277.FSR2Mode, delegate
		{
			_E00F();
		});
		UI.SubscribeState(_E277.SuperSampling, delegate
		{
			_E00F();
		});
		_settingsScroll.verticalNormalizedPosition = 1f;
		_E00F();
	}

	public override Task TakeSettingsFrom(_E7DE settingsManager)
	{
		_E277.TakeSettingsFrom(settingsManager.Graphics.Default);
		return Task.CompletedTask;
	}

	private void _E000()
	{
		_E000 CS_0024_003C_003E8__locals0 = new _E000();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.displayProxy = new _E469<_E7E4, byte>(_E277.DisplaySettings, (_E7E4 settings) => settings.Display, _E002);
		CS_0024_003C_003E8__locals0.aspectRatioProxy = new _E469<_E7E4, AspectRatio>(_E277.DisplaySettings, (_E7E4 settings) => settings.AspectRatio, _E005);
		CS_0024_003C_003E8__locals0.resolutionProxy = new _E469<_E7E4, EftResolution>(_E277.DisplaySettings, (_E7E4 settings) => settings.Resolution, _E004);
		CS_0024_003C_003E8__locals0.fullScreenProxy = new _E469<_E7E4, FullScreenMode>(_E277.DisplaySettings, (_E7E4 settings) => settings.FullScreenMode, _E003);
		UI.SubscribeState(_E278.DisplaySettings, delegate(_E7E4 displaySettings)
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E277.DisplaySettings.Value = displaySettings;
			CS_0024_003C_003E8__locals0._003C_003E4__this._E00F();
		});
		CS_0024_003C_003E8__locals0.unsubs = new _E3A4();
		UI.AddDisposable(CS_0024_003C_003E8__locals0.unsubs);
		UI.AddDisposable(_E278.DisplaySettings.Bind(delegate
		{
			CS_0024_003C_003E8__locals0._E002();
		}));
	}

	private async Task _E001(_E7E4 newSettings)
	{
		_E003 CS_0024_003C_003E8__locals0 = new _E003();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.displaySettings = _E278.DisplaySettings;
		CS_0024_003C_003E8__locals0.oldSettings = CS_0024_003C_003E8__locals0.displaySettings.Value;
		CS_0024_003C_003E8__locals0.oldDlssMode = _E277.DLSSMode.Value;
		await CS_0024_003C_003E8__locals0.displaySettings.SetValue(newSettings);
		_E277.DLSSMode.TakeValueFrom(_E278.DLSSMode);
		newSettings = CS_0024_003C_003E8__locals0.displaySettings.Value;
		if (!EqualityComparer<_E7E4>.Default.Equals(CS_0024_003C_003E8__locals0.oldSettings, newSettings) && !(ItemUiContext.Instance == null))
		{
			string description = _ED3E._E000(256986).Localized();
			ItemUiContext.Instance.ShowMessageWindow(description, delegate
			{
				Singleton<_E7DE>.Instance.Graphics.Save().HandleExceptions();
			}, delegate
			{
				CS_0024_003C_003E8__locals0._E001().HandleExceptions();
			}, _ED3E._E000(256992).Localized(), 15f);
		}
	}

	private Task _E002(byte displayIndex)
	{
		if (_E278.DisplaySettings.Value.Display == displayIndex)
		{
			return Task.CompletedTask;
		}
		_E7E4 displaySettingsPreset = _E278.GetDisplaySettingsPreset(displayIndex);
		return _E001(displaySettingsPreset);
	}

	private Task _E003(FullScreenMode mode)
	{
		if (_E278.DisplaySettings.Value.FullScreenMode == mode)
		{
			return Task.CompletedTask;
		}
		_E7E4 displaySettingsPreset = _E278.GetDisplaySettingsPreset(mode);
		return _E001(displaySettingsPreset);
	}

	private Task _E004(EftResolution resolution)
	{
		_E7E4 value = _E278.DisplaySettings.Value;
		if (value.Resolution == resolution)
		{
			return Task.CompletedTask;
		}
		value.Resolution = resolution;
		return _E001(value);
	}

	private Task _E005(AspectRatio ratio)
	{
		_E7E4 value = _E278.DisplaySettings.Value;
		if (value.AspectRatio.Equals(ratio))
		{
			return Task.CompletedTask;
		}
		value.AspectRatio = ratio;
		return _E001(value);
	}

	private bool _E006(int modeIndex)
	{
		return _E277.IsDLSSModeAllowed(_E3A5<EDLSSMode>.Values.ElementAt(modeIndex));
	}

	private bool _E007(int modeIndex)
	{
		return _E277.IsFSRModeAllowed(_E3A5<EFSRMode>.Values.ElementAt(modeIndex));
	}

	private bool _E008(int modeIndex)
	{
		return _E277.IsFSR2ModeAllowed(_E3A5<EFSR2Mode>.Values.ElementAt(modeIndex));
	}

	private void _E009()
	{
		bool flag = _E8AE.IsReflexAvailable();
		UiElementBlocker[] reflexNotAvailableBlockers = _reflexNotAvailableBlockers;
		for (int i = 0; i < reflexNotAvailableBlockers.Length; i++)
		{
			reflexNotAvailableBlockers[i].SetBlock(!flag, _ED3E._E000(256340));
		}
	}

	private void _E00A()
	{
		bool value = _E277.MipStreaming.Value;
		UiElementBlocker[] mipStreamingParametersBlockers = _mipStreamingParametersBlockers;
		for (int i = 0; i < mipStreamingParametersBlockers.Length; i++)
		{
			mipStreamingParametersBlockers[i].SetBlock(!value, _ED3E._E000(256369));
		}
	}

	private void _E00B()
	{
		_graphicsQuality.Show(() => ((IReadOnlyList<string>)new string[5]
		{
			_ED3E._E000(256944),
			_ED3E._E000(256750),
			_ED3E._E000(256788),
			_ED3E._E000(256829),
			_ED3E._E000(256804)
		}).Localized(EStringCase.None));
		UI.AddDisposable(_graphicsQuality);
		int indexFromQuality = _E7E7.GetIndexFromQuality(_E277.GraphicsQuality);
		_graphicsQuality.UpdateValue(indexFromQuality, sendCallback: false);
		_graphicsQuality.Bind(delegate(int index)
		{
			_E277.GraphicsQuality.Value = _E7E7.GetQualityFromIndex(index);
		});
		if (!_E277.GraphicsQuality.Value.HasValue)
		{
			_graphicsQuality._E001(_ED3E._E000(256389).Localized());
		}
		UI.SubscribeState(_E277.GraphicsQuality, delegate(int? quality)
		{
			if (quality.HasValue)
			{
				_E7E7.GetPreset(quality.Value).Apply(_E277);
			}
			else
			{
				_graphicsQuality.UpdateValue(0, sendCallback: false);
				_graphicsQuality._E001(_ED3E._E000(256389).Localized());
			}
			_E00F();
		});
	}

	private void _E00C()
	{
		_textureQuality.Interactable = !_E7A3.InRaid;
		_E012(_textureQuality, _E277.TextureQuality, _E274);
		UI.SubscribeState(_E277.TextureQuality, delegate
		{
			_E017();
			_E00F();
		});
	}

	private void _E00D()
	{
		_E012(_shadowsQuality, _E277.ShadowsQuality, _E275);
		UI.SubscribeState(_E277.ShadowsQuality, delegate
		{
			_E00F();
		});
	}

	private void _E00E()
	{
		_E277.GraphicsQuality.Value = null;
	}

	private void _E00F()
	{
		int num = 0;
		if (_E276 == null)
		{
			Camera[] allCameras = Camera.allCameras;
			Camera camera = null;
			Camera[] array = allCameras;
			foreach (Camera camera2 in array)
			{
				if (camera2.name == _ED3E._E000(256444))
				{
					camera = camera2;
					break;
				}
			}
			if (camera != null)
			{
				EnvironmentUIRoot componentInParent = camera.gameObject.GetComponentInParent<EnvironmentUIRoot>();
				if (componentInParent != null)
				{
					_E276 = componentInParent.VRamWrapper;
				}
			}
		}
		if (_E276 != null)
		{
			_E276.Update();
			num = (int)(_E276.localBudget / 1024uL / 1024uL);
		}
		int recommendedVRAM = _E7E7.GetRecommendedVRAM(_E277);
		if (recommendedVRAM == -1)
		{
			_recommendedVRAMValueText.text = _ED3E._E000(18502) + _ED3E._E000(181789).Localized() + _ED3E._E000(124720) + ((num != 0) ? num.ToString() : _ED3E._E000(256435));
			_recommendedVRAMValueText.color = Color.gray;
			return;
		}
		_recommendedVRAMValueText.text = _ED3E._E000(18502) + recommendedVRAM + _ED3E._E000(124720) + ((num != 0) ? num.ToString() : _ED3E._E000(256435));
		if (num == 0)
		{
			_recommendedVRAMValueText.color = new Color(0.357f, 0.357f, 0.357f);
		}
		else
		{
			_recommendedVRAMValueText.color = ((num < recommendedVRAM) ? new Color(0.451f, 0.243f, 0.235f) : new Color(0.278f, 0.502f, 0.396f));
		}
	}

	private void _E010(ENvidiaReflexMode mode)
	{
		bool flag = mode != ENvidiaReflexMode.Off;
		UiElementBlocker[] nVidiaReflexBlockers = _NVidiaReflexBlockers;
		for (int i = 0; i < nVidiaReflexBlockers.Length; i++)
		{
			nVidiaReflexBlockers[i].SetBlock(flag, _ED3E._E000(256424));
		}
		if (!flag)
		{
			_E011(_E277.VSync);
		}
	}

	private void _E011(bool isVSyncOn)
	{
		UiElementBlocker[] vSyncBlockers = _vSyncBlockers;
		for (int i = 0; i < vSyncBlockers.Length; i++)
		{
			vSyncBlockers[i].SetBlock(isVSyncOn, _ED3E._E000(256499));
		}
	}

	[Obsolete("Replace this with SettingDropDown")]
	private void _E012(DropDownBox dropdown, GameSetting<int> setting, ReadOnlyCollection<string> labels)
	{
		dropdown.Show(() => ((IReadOnlyList<string>)labels).Localized(EStringCase.None));
		UI.BindState(setting, delegate(int x)
		{
			dropdown.UpdateValue(Mathf.Clamp(x, 0, labels.Count), sendCallback: false);
		});
		dropdown.Bind(delegate(int index)
		{
			setting.Value = index;
			_E00E();
		});
		RegisterDropDown(dropdown);
	}

	[Obsolete("Replace this with SettingSelectSlider")]
	private void _E013<_E077>(SelectSlider slider, GameSetting<_E077> setting, ReadOnlyCollection<_E077> variants, Func<_E077, string> stringConverter)
	{
		slider.Show(variants.Select(stringConverter).ToArray());
		UI.AddDisposable(slider);
		UI.BindState(setting, delegate(_E077 newValue)
		{
			if (SettingSelectSlider.UpdateSliderValue(slider, newValue, variants))
			{
				slider._E001(stringConverter(newValue));
			}
		});
		slider.Bind(delegate(int index)
		{
			setting.Value = variants[index];
			_E00E();
		});
	}

	[Obsolete("Replace this with SettingToggle")]
	private void _E014(UpdatableToggle toggle, GameSetting<bool> setting)
	{
		UI.BindState(setting, delegate(bool x)
		{
			toggle.UpdateValue(x, sendCallback: false);
		});
		toggle.Bind(delegate(bool arg)
		{
			setting.Value = arg;
			_E00E();
		});
	}

	[Obsolete("Replace this with SettingDropDown")]
	private void _E015<_E077>(DropDownBox dropdown, GameSetting<_E077> setting, bool canSetCustomLabel = true) where _E077 : struct, Enum
	{
		dropdown.Show(() => _E3A5<_E077>.GetLocalizedNames());
		UI.BindState(setting, delegate
		{
			dropdown.UpdateValue(_E3A5<_E077>.IndexOf(setting), sendCallback: false);
		});
		dropdown.Bind(delegate(int index)
		{
			setting.Value = _E3A5<_E077>.Values[index];
			if (canSetCustomLabel)
			{
				_E00E();
			}
		});
		RegisterDropDown(dropdown);
	}

	private static void _E016()
	{
		if (_E7A3.InRaid && ItemUiContext.Instance != null)
		{
			ItemUiContext.Instance.ShowMessageWindow(_ED3E._E000(256574).Localized(), null, null, null, 0f, forceShow: true);
		}
	}

	private void _E017()
	{
		if (!_E279 && !(ItemUiContext.Instance == null))
		{
			ItemUiContext.Instance.ShowMessageWindow(_ED3E._E000(256629).Localized(), null, null, null, 0f, forceShow: true);
			_E279 = true;
		}
	}

	private void _E018(SettingDropDown dlssControl)
	{
		_E009 CS_0024_003C_003E8__locals0 = new _E009();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.dlssControl = dlssControl;
		if (!DLSSWrapper.IsDLSSSupported())
		{
			CS_0024_003C_003E8__locals0.dlssControl.Blocker.StartBlock(_ED3E._E000(256701));
			return;
		}
		UI.SubscribeState(_E277.SuperSampling, delegate
		{
			CS_0024_003C_003E8__locals0._E003();
		});
		UI.BindState(_E277.FSRMode, delegate
		{
			CS_0024_003C_003E8__locals0._E003();
		});
		UI.BindState(_E277.FSR2Mode, delegate
		{
			CS_0024_003C_003E8__locals0._E003();
		});
	}

	private void _E019(SettingDropDown fsrControl)
	{
		_E00A CS_0024_003C_003E8__locals0 = new _E00A();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.fsrControl = fsrControl;
		UI.SubscribeState(_E277.SuperSampling, delegate
		{
			CS_0024_003C_003E8__locals0._E005();
		});
		UI.SubscribeState(_E277.AntiAliasing, delegate
		{
			CS_0024_003C_003E8__locals0._E005();
		});
		UI.SubscribeState(_E277.DLSSMode, delegate
		{
			CS_0024_003C_003E8__locals0._E005();
		});
		UI.SubscribeState(_E277.FSR2Mode, delegate
		{
			CS_0024_003C_003E8__locals0._E005();
		});
		UI.BindState(_E277.DisplaySettings, delegate
		{
			CS_0024_003C_003E8__locals0._E005();
		});
	}

	private void _E01A(SettingDropDown fsr2Control)
	{
		_E00B CS_0024_003C_003E8__locals0 = new _E00B();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.fsr2Control = fsr2Control;
		UI.SubscribeState(_E277.SuperSampling, delegate
		{
			CS_0024_003C_003E8__locals0._E004();
		});
		UI.SubscribeState(_E277.DLSSMode, delegate
		{
			CS_0024_003C_003E8__locals0._E004();
		});
		UI.SubscribeState(_E277.FSRMode, delegate
		{
			CS_0024_003C_003E8__locals0._E004();
		});
		UI.BindState(_E277.DisplaySettings, delegate
		{
			CS_0024_003C_003E8__locals0._E004();
		});
	}

	private void _E01B()
	{
		UI.SubscribeState(_E277.DLSSMode, delegate
		{
			_E039();
		});
		UI.BindState(_E277.FSRMode, delegate
		{
			_E039();
		});
		UI.BindState(_E277.FSR2Mode, delegate
		{
			_E039();
		});
	}

	[CompilerGenerated]
	private void _E01C(float _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E01D(float _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E01E(float _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E01F(int _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E020(int _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E021(ESSAOMode _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E022(ESSRMode _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E023(AnisotropicFiltering _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E024(ESamplingMode _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E025(EAntialiasingMode _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E026(bool _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E027(bool _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E028(bool _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E029(bool _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E02A(bool _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E02B(bool _)
	{
		_E017();
	}

	[CompilerGenerated]
	private void _E02C(bool _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E02D(bool _)
	{
		_E00A();
	}

	[CompilerGenerated]
	private void _E02E(EDLSSMode _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E02F(EFSRMode _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E030(EFSR2Mode _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E031(ESamplingMode _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E032(int index)
	{
		_E277.GraphicsQuality.Value = _E7E7.GetQualityFromIndex(index);
	}

	[CompilerGenerated]
	private void _E033(int? quality)
	{
		if (quality.HasValue)
		{
			_E7E7.GetPreset(quality.Value).Apply(_E277);
		}
		else
		{
			_graphicsQuality.UpdateValue(0, sendCallback: false);
			_graphicsQuality._E001(_ED3E._E000(256389).Localized());
		}
		_E00F();
	}

	[CompilerGenerated]
	private void _E034(int _)
	{
		_E017();
		_E00F();
	}

	[CompilerGenerated]
	private void _E035(int _)
	{
		_E00F();
	}

	[CompilerGenerated]
	private void _E036(EDLSSMode _)
	{
		_E039();
	}

	[CompilerGenerated]
	private void _E037(EFSRMode _)
	{
		_E039();
	}

	[CompilerGenerated]
	private void _E038(EFSR2Mode _)
	{
		_E039();
	}

	[CompilerGenerated]
	private void _E039()
	{
		if (_E277.DLSSEnabled)
		{
			_superSamplingBlocker.StartBlock(_ED3E._E000(256844));
			_antiAliasingBlocker.StartBlock(_ED3E._E000(256844));
			return;
		}
		bool fSREnabled = _E277.FSREnabled;
		_superSamplingBlocker.SetBlock(fSREnabled, _ED3E._E000(256883));
		_antiAliasingBlocker.SetBlock(fSREnabled, _ED3E._E000(256883));
		bool fSR2Enabled = _E277.FSR2Enabled;
		_superSamplingBlocker.SetBlock(fSR2Enabled, _ED3E._E000(256913));
		_antiAliasingBlocker.SetBlock(fSR2Enabled, _ED3E._E000(256913));
	}
}
