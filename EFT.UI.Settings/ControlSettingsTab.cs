using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Gestures;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class ControlSettingsTab : SettingsTab
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public _ED0E<_ED08>._E002 token;

		internal void _E000()
		{
			token.Release();
		}
	}

	[SerializeField]
	private CommandKeyPair _commandKeyPairTemplate;

	[SerializeField]
	private CommandAxisPair _commandAxisPairTemplate;

	[SerializeField]
	private RectTransform _commandsContainer;

	[SerializeField]
	private FloatSlider _mouseSensitivity;

	[SerializeField]
	private FloatSlider _mouseSensitivityAiming;

	[SerializeField]
	private FloatSlider _doubleClickTimeout;

	[SerializeField]
	private UpdatableToggle _invertedXAxis;

	[SerializeField]
	private UpdatableToggle _invertedYAxis;

	[Space]
	[SerializeField]
	private UIAnimatedToggleSpawner _controlButton;

	[SerializeField]
	private UIAnimatedToggleSpawner _gesturesButton;

	[SerializeField]
	[Space]
	private GameObject _controlPanel;

	[SerializeField]
	private GameObject _gesturesPanel;

	[SerializeField]
	private GesturesMenu _gesturesMenu;

	private readonly List<GameObject> _E24B = new List<GameObject>();

	private List<CommandKeyPair> _E24C;

	private List<CommandAxisPair> _E24D;

	private _E7F2 _E24E;

	private _E7E0 _E24F;

	private readonly EGameKey[] _E250 = new EGameKey[29]
	{
		EGameKey.F1,
		EGameKey.F2,
		EGameKey.F3,
		EGameKey.F4,
		EGameKey.F5,
		EGameKey.F6,
		EGameKey.F7,
		EGameKey.F8,
		EGameKey.F9,
		EGameKey.F10,
		EGameKey.F11,
		EGameKey.F12,
		EGameKey.DoubleF1,
		EGameKey.DoubleF2,
		EGameKey.DoubleF3,
		EGameKey.DoubleF4,
		EGameKey.DoubleF5,
		EGameKey.DoubleF6,
		EGameKey.DoubleF7,
		EGameKey.DoubleF8,
		EGameKey.DoubleF9,
		EGameKey.DoubleF10,
		EGameKey.DoubleF11,
		EGameKey.DoubleF12,
		EGameKey.Escape,
		EGameKey.Enter,
		EGameKey.HighThrow,
		EGameKey.LowThrow,
		EGameKey.ChangePointOfView
	};

	private readonly EGameKey[] _E251 = new EGameKey[0];

	private readonly EAxis[] _E252 = new EAxis[4]
	{
		EAxis.LookX,
		EAxis.LookY,
		EAxis.TurnX,
		EAxis.TurnY
	};

	private _ECAA _E253;

	private _E76C _E254;

	private bool _E255;

	private _E72F _E256;

	[CompilerGenerated]
	private bool _E257;

	public bool CanPressEscape
	{
		[CompilerGenerated]
		get
		{
			return _E257;
		}
		[CompilerGenerated]
		private set
		{
			_E257 = value;
		}
	}

	private void Awake()
	{
		_controlButton.SpawnedObject.onValueChanged.AddListener(delegate(bool isOn)
		{
			_controlPanel.SetActive(isOn);
		});
		_gesturesButton.SpawnedObject.onValueChanged.AddListener(delegate(bool isOn)
		{
			_gesturesPanel.SetActive(isOn);
			if (isOn)
			{
				_E005().HandleExceptions();
			}
		});
	}

	public void Show(_E7F2 controlSettings, _E7E0 soundSettings, _ECAA gesturesStorage, _E72F profileInfo)
	{
		_E24E = controlSettings;
		_E24F = soundSettings;
		_E253 = gesturesStorage;
		_E256 = profileInfo;
		_controlButton.SpawnedObject.isOn = true;
		_gesturesButton.SpawnedObject.isOn = false;
		_E255 = false;
		SettingsTab.BindToggleToSetting(_invertedXAxis, _E24E.InvertedXAxis);
		SettingsTab.BindToggleToSetting(_invertedYAxis, _E24E.InvertedYAxis);
		SettingsTab.BindFloatSliderToSetting(_mouseSensitivity, _E24E.MouseSensitivity, 0.1f, 5f);
		SettingsTab.BindFloatSliderToSetting(_mouseSensitivityAiming, _E24E.MouseAimingSensitivity, 0.1f, 5f);
		SettingsTab.BindFloatSliderToSetting(_doubleClickTimeout, _E24E.DoubleClickTimeout, 0.3f, 1f);
		_E000();
		UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(_E000));
		_controlButton._E001 = true;
		_gesturesButton.SetActive(!_E7A3.InRaid);
	}

	private void _E000()
	{
		CanPressEscape = true;
		_E004();
		AxisGroup[] array = _E24E.UserAxisBindings.Value.Where((AxisGroup x) => !_E252.Contains(x.axisName)).ToArray();
		_E24D = new List<CommandAxisPair>(array.Length);
		AxisGroup[] array2 = array;
		foreach (AxisGroup axis in array2)
		{
			_E001(axis, positive: true);
			_E001(axis, positive: false);
		}
		Dictionary<string, EPressType> pressTypes = _E3A5<EPressType>.Values.ToDictionary((EPressType pressType) => pressType.Localized(EStringCase.None), (EPressType pressType) => pressType);
		KeyGroup[] array3 = _E24E.UserKeyBindings.Value.Where((KeyGroup x) => !_E250.Contains(x.keyName)).ToArray();
		_E24C = new List<CommandKeyPair>(array3.Length);
		KeyGroup[] array4 = array3;
		foreach (KeyGroup keyGroup in array4)
		{
			CommandKeyPair commandKeyPair = UnityEngine.Object.Instantiate(_commandKeyPairTemplate, _commandsContainer, worldPositionStays: false);
			commandKeyPair.name = keyGroup.keyName.ToStringNoBox();
			commandKeyPair.gameObject.SetActive(value: true);
			commandKeyPair.Show(keyGroup, pressTypes, !_E251.Contains(keyGroup.keyName));
			commandKeyPair._E002 = (Action)Delegate.Combine(commandKeyPair._E002, (Action)delegate
			{
				CanPressEscape = false;
			});
			commandKeyPair._E003 = (Func<KeyGroup, InputSource, bool, bool>)Delegate.Combine(commandKeyPair._E003, new Func<KeyGroup, InputSource, bool, bool>(_E002));
			_E24B.Add(commandKeyPair.gameObject);
			_E24C.Add(commandKeyPair);
		}
	}

	public override Task TakeSettingsFrom(_E7DE settingsManager)
	{
		_E24E.TakeSettingsFrom(settingsManager.Control.Default);
		_E253.Reset();
		return Task.CompletedTask;
	}

	protected override void OnTabSelected()
	{
		foreach (CommandKeyPair item in _E24C)
		{
			EGameKey keyName = item.KeyGroup.keyName;
			if ((uint)(keyName - 102) <= 1u)
			{
				_E009(item.gameObject, !_E24F.VoipEnabled, _ED3E._E000(192208));
			}
		}
	}

	private void _E001(AxisGroup axis, bool positive)
	{
		CommandAxisPair commandAxisPair = UnityEngine.Object.Instantiate(_commandAxisPairTemplate, _commandsContainer, worldPositionStays: false);
		commandAxisPair.name = axis.axisName.ToStringNoBox();
		commandAxisPair.gameObject.SetActive(value: true);
		commandAxisPair.Show(axis, positive);
		commandAxisPair._E002 = (Action)Delegate.Combine(commandAxisPair._E002, (Action)delegate
		{
			CanPressEscape = false;
		});
		commandAxisPair._E003 = (Action<AxisGroup, InputSource, bool>)Delegate.Combine(commandAxisPair._E003, new Action<AxisGroup, InputSource, bool>(_E003));
		_E24B.Add(commandAxisPair.gameObject);
		_E24D.Add(commandAxisPair);
	}

	private bool _E002(KeyGroup keyGroup, InputSource input, bool full)
	{
		if (input.IsEmpty)
		{
			return true;
		}
		bool flag = true;
		foreach (CommandKeyPair item in _E24C)
		{
			if (item.KeyGroup.Equals(keyGroup))
			{
				continue;
			}
			int num = item.ConflictIndex(input);
			if (num < 0)
			{
				continue;
			}
			bool flag2 = keyGroup.keyName.IsPressTypeSelectionAllowed() && !keyGroup.variants.Any((InputSource variant) => variant.isAxis) && item.DropdownAvailable;
			if (!flag2 || keyGroup.pressType == item.KeyGroup.pressType)
			{
				flag = !flag2;
				if (full || !flag2)
				{
					item.ResetInput(num);
				}
			}
		}
		if (full)
		{
			foreach (CommandAxisPair item2 in _E24D)
			{
				int num2 = item2.ConflictIndex(input);
				if (num2 >= 0)
				{
					item2.ResetInput(num2);
				}
			}
		}
		if (full || flag)
		{
			CanPressEscape = true;
		}
		return flag;
	}

	private void _E003(AxisGroup axisGroup, InputSource input, bool positive)
	{
		if (input.IsEmpty)
		{
			return;
		}
		foreach (CommandAxisPair item in _E24D)
		{
			if (!item.AxisGroup.Equals(axisGroup) || positive != item.Positive)
			{
				int num = item.ConflictIndex(input);
				if (num >= 0)
				{
					item.ResetInput(num);
				}
			}
		}
		foreach (CommandKeyPair item2 in _E24C)
		{
			int num2 = item2.ConflictIndex(input);
			if (num2 >= 0)
			{
				item2.ResetInput(num2);
			}
		}
		CanPressEscape = true;
	}

	public override void Close()
	{
		foreach (CommandKeyPair item in _E24C)
		{
			item.Close();
		}
		if (_E254 != null)
		{
			_E254.OnPhraseTold -= _E007;
			_E254 = null;
		}
		_gesturesMenu.Close();
		_E004();
		base.Close();
	}

	private void _E004()
	{
		foreach (GameObject item in _E24B)
		{
			UnityEngine.Object.Destroy(item);
		}
		_E24B.Clear();
	}

	private async Task _E005()
	{
		if (_E7A3.InRaid || _E254 != null)
		{
			return;
		}
		_E254 = new _E76C();
		await _E006(_E254, _E256.Voice);
		HashSet<EPhraseTrigger> availablePhrases = new HashSet<EPhraseTrigger>(_E254.PhrasesBanks.Keys);
		_gesturesMenu.Init(_E253, availablePhrases);
		_gesturesMenu.HideOnlySituationalGroups = true;
		_gesturesMenu.Show();
		UI.AddDisposable(_gesturesMenu.OnItemSelected.Subscribe(delegate((int actionId, bool aggressive) x)
		{
			_E008(x.actionId);
		}));
		foreach (EPhraseTrigger key in GesturesQuickPanel.PhrasePriorities.Keys)
		{
			_gesturesMenu.SetPhraseActive(key, active: false);
		}
		_E255 = true;
	}

	private async Task _E006(_E76C speaker, string voice)
	{
		_ED0E<_ED08>._E002 token = Singleton<_ED0A>.Instance.Retain(new string[1] { _E5D2.TakePhrasePath(voice) });
		await _E612.LoadBundles(token);
		UI.AddDisposable(delegate
		{
			token.Release();
		});
		speaker.Init(_E253.Side, 0, voice, registerInSpeakerManager: false);
		speaker.OnPhraseTold += _E007;
	}

	private static void _E007(EPhraseTrigger arg1, TaggedClip clip, TagBank arg3, _E76C arg4)
	{
		if (Singleton<GUISounds>.Instantiated)
		{
			Singleton<GUISounds>.Instance.ForcePlaySound(clip.Clip).HandleExceptions();
		}
	}

	private void _E008(int item)
	{
		if (_E255)
		{
			EPhraseTrigger ePhraseTrigger = (EPhraseTrigger)item;
			if (ePhraseTrigger == EPhraseTrigger.MumblePhrase)
			{
				ePhraseTrigger = ((UnityEngine.Random.value > 0.5f) ? EPhraseTrigger.OnFight : EPhraseTrigger.OnMutter);
			}
			_E254.PlayDirectRandom(ePhraseTrigger);
		}
	}

	private static void _E009(GameObject objectToLock, bool locked, string reason)
	{
		objectToLock.GetOrAddComponent<CanvasGroup>();
		HoverTooltipArea orAddComponent = objectToLock.GetOrAddComponent<HoverTooltipArea>();
		orAddComponent.SetUnlockStatus(!locked);
		orAddComponent.SetMessageText(reason);
	}

	[CompilerGenerated]
	private void _E00A(bool isOn)
	{
		_controlPanel.SetActive(isOn);
	}

	[CompilerGenerated]
	private void _E00B(bool isOn)
	{
		_gesturesPanel.SetActive(isOn);
		if (isOn)
		{
			_E005().HandleExceptions();
		}
	}

	[CompilerGenerated]
	private bool _E00C(AxisGroup x)
	{
		return !_E252.Contains(x.axisName);
	}

	[CompilerGenerated]
	private bool _E00D(KeyGroup x)
	{
		return !_E250.Contains(x.keyName);
	}

	[CompilerGenerated]
	private void _E00E()
	{
		CanPressEscape = false;
	}

	[CompilerGenerated]
	private void _E00F()
	{
		CanPressEscape = false;
	}

	[CompilerGenerated]
	private void _E010((int actionId, bool aggressive) x)
	{
		_E008(x.actionId);
	}
}
