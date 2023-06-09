using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.Hideout;

public sealed class HideoutScreenOverlay : UIScreen
{
	private const string _E048 = "hideout/Turn night vision on";

	private const string _E049 = "hideout/Turn night vision off";

	private const string _E04A = "hideout/Turn generator on";

	private const string _E04B = "hideout/Turn generator off";

	private const string _E04C = "hideout/Generator not constructed";

	private const string _E04D = "hideout/Welcome title";

	private const string _E04E = "hideout/Welcome";

	private const float _E04F = 4f;

	private const int _E050 = 10;

	private const int _E045 = 100;

	[CompilerGenerated]
	private Action _E051;

	[CompilerGenerated]
	private Action<ELightingLevel> _E052;

	[SerializeField]
	private DefaultUIButton _enterHideoutButtonSpawner;

	[SerializeField]
	private DefaultUIButton _returnToMenuButtonSpawner;

	[SerializeField]
	private ComplementaryButton _nightVisionButton;

	[SerializeField]
	private ComplementaryButton _generatorButton;

	[SerializeField]
	private AreasPanel _areasPanel;

	[SerializeField]
	private DelayTypeWindow _delayTypeWindow;

	[SerializeField]
	private AreaScreenSubstrate _areaSubstrateTemplate;

	[SerializeField]
	private RectTransform _areaScreenContainer;

	[SerializeField]
	private ChangeLightButton _changeLightButton;

	[SerializeField]
	private ChangeLightPanel _changeLightPanel;

	private HideoutCameraController _E046;

	private AreaScreenSubstrate _E053;

	private HideoutPlayer _E030;

	private _E796 _E031;

	private AreaData _E02E;

	private HideoutScreenRear _E047;

	private float[] _E054;

	private bool _E055 = true;

	private bool _E056;

	private _E80F _E057;

	private AreaData _E058;

	public event Action OnReturnToHomeScreen
	{
		[CompilerGenerated]
		add
		{
			Action action = _E051;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E051, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E051;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E051, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<ELightingLevel> OnLightingLevelChanged
	{
		[CompilerGenerated]
		add
		{
			Action<ELightingLevel> action = _E052;
			Action<ELightingLevel> action2;
			do
			{
				action2 = action;
				Action<ELightingLevel> value2 = (Action<ELightingLevel>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E052, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ELightingLevel> action = _E052;
			Action<ELightingLevel> action2;
			do
			{
				action2 = action;
				Action<ELightingLevel> value2 = (Action<ELightingLevel>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E052, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_E053 = UnityEngine.Object.Instantiate(_areaSubstrateTemplate, _areaScreenContainer);
		RecacheChildren();
		_enterHideoutButtonSpawner.OnClick.AddListener(_E004);
		_returnToMenuButtonSpawner.OnClick.AddListener(_E007);
		_nightVisionButton.Show(delegate
		{
			_E002();
		});
		_nightVisionButton.SetTooltipMessages(() => _ED3E._E000(164374), () => _ED3E._E000(164404));
		_generatorButton.Show(delegate(bool arg)
		{
			Singleton<GUISounds>.Instance.PlayUISound(arg ? EUISoundType.GeneratorTurnOn : EUISoundType.GeneratorTurnOff);
			_E000(arg);
		}, null, null, unlocked: true, 10);
		_generatorButton.SetTooltipMessages(() => _ED3E._E000(164441), () => _ED3E._E000(164476));
		_changeLightPanel.OnLightningSelected += delegate(ELightingLevel lightingLevel)
		{
			_E052?.Invoke(lightingLevel);
		};
	}

	private void _E000(bool isOn)
	{
		_E031.ToggleArea(EAreaType.Generator, isOn);
		_E058.IsActive = isOn;
		_E057.SetSwitchedStatus(isOn);
	}

	public void Show(HideoutPlayer player, bool returnToFirstPerson, _E796 session, AreaData[] areaDatas, HideoutScreenRear hideoutRear)
	{
		UIEventSystem.Instance.Enable();
		_E047 = hideoutRear;
		_E046 = hideoutRear.HideoutCameraController;
		_E030 = player;
		_E031 = session;
		_E056 = returnToFirstPerson;
		_E058 = areaDatas.FirstOrDefault((AreaData data) => data.Template.Type == EAreaType.Generator);
		_E815 instance = Singleton<_E815>.Instance;
		_E057 = instance.EnergyController;
		ItemUiContext.Instance.Configure(_E030._E0DE, session.Profile, session, session.InsuranceCompany, session.RagFair, null, null, new _EA40[1] { session.Profile.Inventory.Stash }, EItemUiContextType.Hideout, ECursorResult.Ignore);
		ShowGameObject();
		if (!_E762.GetBool(_E762.HideoutWelcomeShown))
		{
			_E762.SetBool(_E762.HideoutWelcomeShown, value: true);
			_delayTypeWindow.Show(_ED3E._E000(164272).Localized(), _ED3E._E000(164318).Localized(), null, delegate
			{
				_delayTypeWindow.FinishAnimating();
			});
		}
		_areasPanel.Show(areaDatas, delegate(AreaPanel arg)
		{
			AreaSelected(arg.Data, wait: true).HandleExceptions();
		});
		if (_E058.CurrentLevel == 0)
		{
			_generatorButton.SetTooltipMessages(() => _ED3E._E000(164454), () => _ED3E._E000(164454));
		}
		_E058.LevelUpdated.Bind(delegate
		{
			if (_E058.CurrentLevel > 0)
			{
				_generatorButton.SetTooltipMessages(() => _ED3E._E000(164441), () => _ED3E._E000(164476));
			}
			_generatorButton.SetSelectedStatus(_E058.CurrentLevel > 0 && ((_E058.Template.AreaBehaviour as GeneratorBehaviour)?.SwitchedOn ?? false));
			_generatorButton.SetUnlockStatus(_E058.CurrentLevel > 0);
		});
		_generatorButton.SetSelectedStatus(_E058.IsActive);
		_nightVisionButton.SetUnlockStatus(isUnlocked: true);
		_E053.Init(_E030, _E031, _E003, ReturnToPreviousState);
		_E055 = true;
		UI.AddDisposable(_E030.NightVisionObserver.Changed.Subscribe(_E001));
		UI.AddDisposable(_E030.ThermalVisionObserver.Changed.Subscribe(_E001));
		_E001();
		_changeLightButton.Show();
	}

	public void ClearLightLevels()
	{
		_changeLightPanel.ClearLightLevels();
	}

	public void AddLightingLevel(ELightingLevel level, bool switchToNewLevel)
	{
		_changeLightPanel.AddLightingLevel(level, switchToNewLevel);
	}

	public void SetCurrentLightingLevel(ELightingLevel level)
	{
		_changeLightPanel.SetCurrentLightingLevel(level);
	}

	public async Task AreaSelected(AreaData data, bool wait)
	{
		if (!_E055 || _E047.Closed)
		{
			return;
		}
		_E055 = false;
		wait = wait && _E02E != data;
		_E005(data);
		if (_E055)
		{
			return;
		}
		if (_E02E == null)
		{
			_E055 = true;
			return;
		}
		if (wait)
		{
			await TasksExtensions.Delay(0.5f);
		}
		if (_E047.Closed)
		{
			_E055 = true;
			return;
		}
		_E053.SelectArea(data);
		_E046.AreaSelected = true;
		_E055 = true;
	}

	public async Task SpecialAreaActionSelected(AreaData data, bool wait)
	{
		if (!_E055 || _E047.Closed)
		{
			return;
		}
		_E055 = false;
		wait = wait && _E02E != data;
		_E006(data);
		if (_E055)
		{
			return;
		}
		if (_E02E == null)
		{
			_E055 = true;
			return;
		}
		if (wait)
		{
			await TasksExtensions.Delay(0.5f);
		}
		if (_E047.Closed)
		{
			_E055 = true;
			return;
		}
		_E053.SelectArea(data);
		_E046.AreaSelected = true;
		_E055 = true;
	}

	private void _E001()
	{
		_nightVisionButton.SetSelectedStatus(_E030.NightVisionActive);
	}

	private void _E002()
	{
		_E030.ToggleNightVision();
	}

	private static void _E003(AreaData areaData)
	{
		switch (areaData.Status)
		{
		case EAreaStatus.ReadyToConstruct:
		case EAreaStatus.ReadyToInstallConstruct:
		case EAreaStatus.ReadyToUpgrade:
		case EAreaStatus.ReadyToInstallUpgrade:
			areaData.UpgradeAction().HandleExceptions();
			break;
		case EAreaStatus.NotSet:
		case EAreaStatus.LockedToConstruct:
		case EAreaStatus.Constructing:
		case EAreaStatus.LockedToUpgrade:
		case EAreaStatus.Upgrading:
		case EAreaStatus.NoFutureUpgrades:
		case EAreaStatus.AutoUpgrading:
			throw new Exception(_ED3E._E000(164302) + areaData.Status);
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void ReturnToPreviousState()
	{
		if (!_E047.Closed)
		{
			if (_E056)
			{
				_E004();
				return;
			}
			_E053.Close();
			_E005(null);
		}
	}

	private void _E004()
	{
		_E005(null);
		Close();
		_E046.SetActiveState(state: false);
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetMenuChatBarVisibility(visible: false);
		_E047.FirstPerson = true;
		_E00E().HandleExceptions();
	}

	private void _E005(AreaData area)
	{
		bool flag = area != null;
		if (_E02E != null)
		{
			if (!flag)
			{
				_E046.SetAnimationTime(_E02E.Template.CameraTimePosition);
			}
			_E02E.SetSelectedStatus(isSelected: false);
			_E02E.AreaCamera.Priority = 0;
			if ((bool)_E02E.SpecialActionCamera)
			{
				_E02E.SpecialActionCamera.Priority = 0;
			}
			_E02E = null;
		}
		_E02E = area;
		if (flag)
		{
			_E02E.SetSelectedStatus(isSelected: true);
			_E02E.AreaCamera.Priority = 100;
			_areasPanel.SelectArea(_E02E);
		}
		else
		{
			_E046.AreaSelected = false;
		}
		_areasPanel.SetSelectedStatus(flag);
		_returnToMenuButtonSpawner.gameObject.SetActive(!flag);
	}

	private void _E006(AreaData area)
	{
		bool flag = area != null;
		if (_E02E != null)
		{
			if (!flag)
			{
				_E046.SetAnimationTime(_E02E.Template.CameraTimePosition);
			}
			_E02E.SetSelectedStatus(isSelected: false);
			_E02E.SpecialActionCamera.Priority = 0;
			_E02E.AreaCamera.Priority = 0;
			_E02E = null;
		}
		_E02E = area;
		if (flag)
		{
			_E02E.SpecialActionCamera.Priority = 100;
		}
		else
		{
			_E046.AreaSelected = false;
		}
		_areasPanel.SetSelectedStatus(flag);
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (!_E055)
		{
			return ETranslateResult.Block;
		}
		float value = 0f;
		switch (command)
		{
		case ECommand.EnterHideout:
			_E004();
			return ETranslateResult.Block;
		case ECommand.Escape:
			if (_changeLightPanel.gameObject.activeSelf)
			{
				_changeLightButton.PointerExit();
				_changeLightPanel.Hide();
				return ETranslateResult.Block;
			}
			if (_E053.gameObject.activeSelf)
			{
				ReturnToPreviousState();
				return ETranslateResult.Block;
			}
			_E007();
			return ETranslateResult.Block;
		case ECommand.ToggleGoggles:
			_E002();
			break;
		case ECommand.ScrollNext:
			value = -4f;
			break;
		case ECommand.ScrollPrevious:
			value = 4f;
			break;
		}
		if (!value.IsZero() && !_E053.gameObject.activeSelf)
		{
			_E046.Zoom(value);
		}
		return ETranslateResult.Ignore;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		if (!_E053.gameObject.activeSelf)
		{
			float value = axes[0];
			if (!value.IsZero())
			{
				_E046.MoveCamera(value.Positive() ? EMovementDirection.Right : EMovementDirection.Left);
			}
			float[] array = _E054;
			float num = ((array != null) ? array[1] : 0f);
			float num2 = axes[1];
			if (!num2.IsZero() && !Math.Abs(num - num2).IsZero())
			{
				_E046.SwitchCamera();
			}
			_E054 = axes.ToArray();
			axes = null;
		}
	}

	private void _E007()
	{
		if (_E046.CameraIsInThirdPerson)
		{
			_E051?.Invoke();
		}
	}

	public new void Close()
	{
		_E055 = true;
		_E02E = null;
		_areasPanel.Close();
		if (_E046 != null)
		{
			_E046.Zoom(null);
			_nightVisionButton.SetSelectedStatus(_E046.NightVisionState);
		}
		if (_E053.gameObject.activeSelf)
		{
			_E053.Close();
		}
		_changeLightPanel.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E008(bool arg)
	{
		_E002();
	}

	[CompilerGenerated]
	private void _E009(bool arg)
	{
		Singleton<GUISounds>.Instance.PlayUISound(arg ? EUISoundType.GeneratorTurnOn : EUISoundType.GeneratorTurnOff);
		_E000(arg);
	}

	[CompilerGenerated]
	private void _E00A(ELightingLevel lightingLevel)
	{
		_E052?.Invoke(lightingLevel);
	}

	[CompilerGenerated]
	private void _E00B()
	{
		_delayTypeWindow.FinishAnimating();
	}

	[CompilerGenerated]
	private void _E00C(AreaPanel arg)
	{
		AreaSelected(arg.Data, wait: true).HandleExceptions();
	}

	[CompilerGenerated]
	private void _E00D()
	{
		if (_E058.CurrentLevel > 0)
		{
			_generatorButton.SetTooltipMessages(() => _ED3E._E000(164441), () => _ED3E._E000(164476));
		}
		_generatorButton.SetSelectedStatus(_E058.CurrentLevel > 0 && ((_E058.Template.AreaBehaviour as GeneratorBehaviour)?.SwitchedOn ?? false));
		_generatorButton.SetUnlockStatus(_E058.CurrentLevel > 0);
	}

	[CompilerGenerated]
	private async Task _E00E()
	{
		await TasksExtensions.Delay(0.8f);
		if (!base.gameObject.activeSelf)
		{
			_E047.SetPointOfView(firstPerson: true);
			_E046.SetActiveState(state: false);
		}
	}
}
