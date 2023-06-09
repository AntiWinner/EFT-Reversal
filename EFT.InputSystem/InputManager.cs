using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.InputSystem;

public sealed class InputManager : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E804 current;

		internal bool _E000(_E804 groupCombination)
		{
			if (groupCombination != null)
			{
				return groupCombination.KeysHash == current.KeysHash;
			}
			return false;
		}
	}

	public static readonly EKeyPress[,] UpdateInputMatrix = new EKeyPress[2, 4]
	{
		{
			EKeyPress.None,
			EKeyPress.Up,
			EKeyPress.Up,
			EKeyPress.None
		},
		{
			EKeyPress.Down,
			EKeyPress.Hold,
			EKeyPress.Hold,
			EKeyPress.Down
		}
	};

	[SerializeField]
	public bool SuperSmooth;

	[SerializeField]
	public bool DeliverInputOnUpdates;

	public Action<bool> CursorVisibilityChanged;

	private static GameObject m__E000;

	private bool m__E001;

	private static _E807 m__E002;

	private bool m__E003;

	private readonly List<ECommand> m__E004 = new List<ECommand>(256);

	private readonly List<ECommand> m__E005 = new List<ECommand>();

	private readonly float[] _E006 = new float[_E3A5<EAxis>.Count];

	[CompilerGenerated]
	private _E806 _E007;

	private ECursorResult _E008;

	private bool _E009;

	private Action _E00A;

	public _E806 TranslateDelegate
	{
		[CompilerGenerated]
		get
		{
			return _E007;
		}
		[CompilerGenerated]
		set
		{
			_E007 = value;
		}
	}

	public static InputManager Create(KeyGroup[] keyGroups, AxisGroup[] settingsAxisGroups, float doubleClickTime, bool deliverInputOnUpdates)
	{
		return _E000(new GameObject(_ED3E._E000(168839)), keyGroups, settingsAxisGroups, doubleClickTime, deliverInputOnUpdates);
	}

	private static InputManager _E000(GameObject gameObject, KeyGroup[] keyGroups, AxisGroup[] settingsAxisGroups, float doubleClickTime, bool deliverInputOnUpdates)
	{
		InputManager inputManager = gameObject.AddComponent<InputManager>();
		inputManager._E001(keyGroups, settingsAxisGroups, doubleClickTime);
		inputManager.DeliverInputOnUpdates = deliverInputOnUpdates;
		return inputManager;
	}

	public static void UpdateBindings(KeyGroup[] keyGroups, AxisGroup[] settingsAxisGroups, float doubleClickTimeout)
	{
		InputManager.m__E002?.UpdateBindings(keyGroups, settingsAxisGroups, doubleClickTimeout);
	}

	private void _E001(KeyGroup[] keyGroups, AxisGroup[] settingsAxisGroups, float doubleClickTime)
	{
		InputManager.m__E002 = new _E807();
		InputManager.m__E002.SetCombinations(doubleClickTime, new _E805(EGameKey.Shoot, ECommand.ToggleShooting, ECommand.EndShooting), new _E805(EGameKey.Aim, ECommand.ToggleAlternativeShooting, ECommand.EndAlternativeShooting)
		{
			OnSetKeys = delegate(_E804 combination, IEnumerable<_E804> allCombinations)
			{
				combination.IsImportant = combination.Type == EPressType.Continuous;
			}
		}, new _E805(EGameKey.HighThrow, ECommand.TryHighThrow, ECommand.FinishHighThrow)
		{
			OnSetKeys = delegate(_E804 combination, IEnumerable<_E804> allCombinations)
			{
				combination.IsImportant = combination.Type == EPressType.Continuous;
			}
		}, new _E805(EGameKey.LowThrow, ECommand.TryLowThrow, ECommand.FinishLowThrow)
		{
			OnSetKeys = delegate(_E804 combination, IEnumerable<_E804> allCombinations)
			{
				combination.IsImportant = combination.Type == EPressType.Continuous;
			}
		}, new _E804(EGameKey.ChangeAimScope, ECommand.ChangeScope), new _E804(EGameKey.ChangeAimScopeMagnification, ECommand.ChangeScopeMagnification), new _E804(EGameKey.Next, ECommand.ScrollNext), new _E804(EGameKey.NextWalkPose, ECommand.NextWalkPose), new _E804(EGameKey.NextMagazine, ECommand.NextMagazine), new _E804(EGameKey.Previous, ECommand.ScrollPrevious), new _E804(EGameKey.PreviousWalkPose, ECommand.PreviousWalkPose), new _E804(EGameKey.PreviousMagazine, ECommand.PreviousMagazine), new _E805(EGameKey.Interact, ECommand.BeginInteracting, ECommand.EndInteracting), new _E804(EGameKey.ThrowGrenade, ECommand.ThrowGrenade), new _E804(EGameKey.ReloadWeapon, ECommand.ReloadWeapon), new _E804(EGameKey.QuickReloadWeapon, ECommand.QuickReloadWeapon), new _E804(EGameKey.DropBackpack, ECommand.DropBackpack), new _E804(EGameKey.ShootingMode, ECommand.ChangeWeaponMode), new _E804(EGameKey.ForceAutoWeaponMode, ECommand.ForceAutoWeaponMode), new _E804(EGameKey.CheckFireMode, ECommand.CheckFireMode, 10), new _E805(EGameKey.Sprint, ECommand.ToggleSprinting, ECommand.EndSprinting, -2), new _E805(EGameKey.Breath, ECommand.ToggleBreathing, ECommand.EndBreathing), new _E804(EGameKey.ExamineWeapon, ECommand.ExamineWeapon), new _E804(EGameKey.FoldStock, ECommand.FoldStock, 11), new _E804(EGameKey.Tactical, ECommand.ToggleTacticalDevice), new _E804(EGameKey.NextTacticalDevice, ECommand.NextTacticalDevice), new _E804(EGameKey.CheckAmmo, ECommand.CheckAmmo, 12), new _E804(EGameKey.CheckChamber, ECommand.CheckChamber, 13), new _E804(EGameKey.ChamberUnload, ECommand.ChamberUnload, 14), new _E804(EGameKey.UnloadMagazine, ECommand.UnloadMagazine, 15), new _E805(EGameKey.Walk, ECommand.ToggleWalk, ECommand.EndWalk), new _E804(EGameKey.ToggleGoggles, ECommand.ToggleGoggles), new _E804(EGameKey.WatchTime, ECommand.DisplayTimer), new _E804(EGameKey.WatchTimerAndExits, ECommand.DisplayTimerAndExits), new _E804(EGameKey.Inventory, ECommand.ToggleInventory), new _E804(EGameKey.Knife, ECommand.SelectKnife), new _E804(EGameKey.QuickKnife, ECommand.QuickKnifeKick), new _E804(EGameKey.Jump, ECommand.Jump), new _E804(EGameKey.Nidnod, ECommand.ResetLookDirection), new _E804(EGameKey.PrimaryWeaponFirst, ECommand.SelectFirstPrimaryWeapon), new _E804(EGameKey.PrimaryWeaponSecond, ECommand.SelectSecondPrimaryWeapon), new _E804(EGameKey.SecondaryWeapon, ECommand.SelectSecondaryWeapon), new _E804(EGameKey.ChangePointOfView, ECommand.ChangePointOfView), new _E804(EGameKey.Escape, ECommand.Escape), new _E805(EGameKey.Duck, ECommand.ToggleDuck, ECommand.RestorePose)
		{
			OnSetKeys = delegate(_E804 current, IEnumerable<_E804> allCombinations)
			{
				_E804[] array = allCombinations.Where((_E804 groupCombination) => groupCombination != null && groupCombination.KeysHash == current.KeysHash).ToArray();
				if (array.Length != 0)
				{
					_E804[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].CalculateFullHash = current.Type == EPressType.Release;
					}
				}
			}
		}, new _E804(EGameKey.Prone, ECommand.ToggleProne), new _E804(EGameKey.Slot4, ECommand.SelectFastSlot4), new _E804(EGameKey.Slot5, ECommand.SelectFastSlot5), new _E804(EGameKey.Slot6, ECommand.SelectFastSlot6), new _E804(EGameKey.Slot7, ECommand.SelectFastSlot7), new _E804(EGameKey.Slot8, ECommand.SelectFastSlot8), new _E804(EGameKey.Slot9, ECommand.SelectFastSlot9), new _E804(EGameKey.Slot0, ECommand.SelectFastSlot0), new _E805(EGameKey.LeanLockLeft, ECommand.ToggleLeanLeft, ECommand.EndLeanLeft), new _E805(EGameKey.LeanLockRight, ECommand.ToggleLeanRight, ECommand.EndLeanRight), new _E805(EGameKey.BlindShootAbove, ECommand.ToggleBlindAbove, ECommand.BlindShootEnd, 99), new _E805(EGameKey.BlindShootRight, ECommand.ToggleBlindRight, ECommand.BlindShootEnd, 99), new _E805(EGameKey.StepLeft, ECommand.ToggleStepLeft, ECommand.ReturnFromLeftStep), new _E805(EGameKey.StepRight, ECommand.ToggleStepRight, ECommand.ReturnFromRightStep), new _E805(EGameKey.Mumble, ECommand.MumbleToggle, ECommand.MumbleEnd), new _E805(EGameKey.MumbleDropdown, ECommand.ToggleMumbleDropdown, ECommand.MumbleDropdownHide), new _E804(EGameKey.MumbleQuick, ECommand.QuickMumbleStart), new _E804(EGameKey.F1, ECommand.F1), new _E804(EGameKey.F2, ECommand.F2), new _E804(EGameKey.F3, ECommand.F3), new _E804(EGameKey.F4, ECommand.F4), new _E804(EGameKey.F5, ECommand.F5), new _E804(EGameKey.F6, ECommand.F6), new _E804(EGameKey.F7, ECommand.F7), new _E804(EGameKey.F8, ECommand.F8), new _E804(EGameKey.F9, ECommand.F9), new _E804(EGameKey.F10, ECommand.F10), new _E804(EGameKey.F11, ECommand.F11), new _E804(EGameKey.F12, ECommand.F12), new _E804(EGameKey.DoubleF1, ECommand.DoubleF1), new _E804(EGameKey.DoubleF2, ECommand.DoubleF2), new _E804(EGameKey.DoubleF3, ECommand.DoubleF3), new _E804(EGameKey.DoubleF4, ECommand.DoubleF4), new _E804(EGameKey.DoubleF5, ECommand.DoubleF5), new _E804(EGameKey.DoubleF6, ECommand.DoubleF6), new _E804(EGameKey.DoubleF7, ECommand.DoubleF7), new _E804(EGameKey.DoubleF8, ECommand.DoubleF8), new _E804(EGameKey.DoubleF9, ECommand.DoubleF9), new _E804(EGameKey.DoubleF10, ECommand.DoubleF10), new _E804(EGameKey.DoubleF11, ECommand.DoubleF11), new _E804(EGameKey.DoubleF12, ECommand.DoubleF12), new _E804(EGameKey.OpticCalibrationSwitchUp, ECommand.OpticCalibrationSwitchUp), new _E804(EGameKey.OpticCalibrationSwitchDown, ECommand.OpticCalibrationSwitchDown), new _E804(EGameKey.MakeScreenshot, ECommand.MakeScreenshot), new _E804(EGameKey.Enter, ECommand.EnterHideout), new _E804(EGameKey.ToggleInfo, ECommand.ToggleInfo), new _E804(EGameKey.Console, ECommand.ShowConsole), new _E804(EGameKey.ThrowItem, ECommand.ThrowItem), new _E804(EGameKey.ToggleVoip, ECommand.ToggleVoip), new _E805(EGameKey.PushToTalk, ECommand.ToggleTalk, ECommand.StopTalk));
		InputManager.m__E002.SetAxis(new _E7FF(EAxis.MoveX), new _E7FF(EAxis.MoveY), new _E7FF(EAxis.LookX, EGameKey.Nidnod), new _E7FF(EAxis.LookY, EGameKey.Nidnod), new _E7FF(EAxis.TurnX), new _E7FF(EAxis.TurnY), new _E7FF(EAxis.LeanX));
		UpdateBindings(keyGroups, settingsAxisGroups, doubleClickTime);
	}

	private void Awake()
	{
		if (InputManager.m__E000 != null)
		{
			Debug.LogError(_ED3E._E000(168888) + InputManager.m__E000.transform.GetFullPath(withSceneName: true) + _ED3E._E000(168920));
			base.gameObject.SetActive(value: false);
			return;
		}
		InputManager.m__E000 = base.gameObject;
		this.m__E001 = true;
		_E00A = _EBAF.Instance.SubscribeOnEvent(delegate(_EBC4 raisedEvent)
		{
			_E002(raisedEvent);
		});
	}

	private void Start()
	{
		foreach (EAxis value in _E3A5<EAxis>.Values)
		{
			_E006[(int)value] = 0f;
		}
		CursorVisibilityChanged?.Invoke(obj: true);
	}

	private void Update()
	{
		InputManager.m__E002.UpdateInput(this.m__E004, _E006, Time.deltaTime);
		bool flag = _E008 != ECursorResult.LockCursor;
		flag |= _E009;
		if (Cursor.visible != flag)
		{
			CursorVisibilityChanged?.Invoke(flag);
		}
		if (SuperSmooth)
		{
			_E004(this.m__E005, _E006);
		}
		this.m__E003 = true;
		if (DeliverInputOnUpdates)
		{
			_E003();
		}
	}

	private void FixedUpdate()
	{
		if (!DeliverInputOnUpdates)
		{
			_E003();
		}
	}

	private void _E002(_EBC4 calledEvent)
	{
		_E009 = calledEvent.Show;
	}

	private void _E003()
	{
		if (!this.m__E003)
		{
			this.m__E004.Clear();
		}
		this.m__E003 = false;
		_E004(this.m__E004, _E006);
	}

	private void _E004(List<ECommand> commandsList, float[] axesList)
	{
		_E008 = ECursorResult.Ignore;
		if (TranslateDelegate != null)
		{
			float[] axes = axesList;
			TranslateDelegate(commandsList, ref axes, ref _E008);
		}
	}

	private void OnDestroy()
	{
		_E00A();
		if (this.m__E001)
		{
			InputManager.m__E000 = null;
		}
	}

	[CompilerGenerated]
	private void _E005(_EBC4 raisedEvent)
	{
		_E002(raisedEvent);
	}
}
