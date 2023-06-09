using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.Hideout;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.Screens;
using JetBrains.Annotations;
using UI.Hideout;
using UnityEngine;

namespace EFT;

public sealed class HideoutPlayerOwner : GamePlayerOwner
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public HideoutPlayerOwner _003C_003E4__this;

		public Action exitAction;

		internal void _E000()
		{
			_003C_003E4__this.Player.UpdateInteractionCast();
			exitAction();
		}
	}

	private const int _E00D = 50;

	[CompilerGenerated]
	private Action<bool> _E00E;

	[CompilerGenerated]
	private Action<HideoutArea> _E00F;

	[CompilerGenerated]
	private Action<HideoutArea> _E010;

	[CompilerGenerated]
	private Action _E011;

	[CompilerGenerated]
	private Action _E012;

	private bool _E013;

	private _E7B8 _E014;

	private _E7B4 _E015;

	private bool _E016 = true;

	private readonly Vector3 _E017 = new Vector3(0f, -500f, 0f);

	private Vector3 _E018;

	private RuntimeAnimatorController _E019;

	[CompilerGenerated]
	private bool _E01A = true;

	[CompilerGenerated]
	private HideoutPlayer _E01B;

	[CompilerGenerated]
	private bool _E01C;

	[CompilerGenerated]
	private bool _E01D;

	[CompilerGenerated]
	private HideoutArea _E01E;

	public bool FirstPersonMode
	{
		[CompilerGenerated]
		get
		{
			return _E01A;
		}
		[CompilerGenerated]
		private set
		{
			_E01A = value;
		}
	}

	public HideoutPlayer HideoutPlayer
	{
		[CompilerGenerated]
		get
		{
			return _E01B;
		}
		[CompilerGenerated]
		private set
		{
			_E01B = value;
		}
	}

	public bool InShootingRange
	{
		[CompilerGenerated]
		get
		{
			return _E01C;
		}
		[CompilerGenerated]
		private set
		{
			_E01C = value;
		}
	}

	public bool AvailableForInteractions
	{
		get
		{
			if (!InShootingRange)
			{
				return !HideoutPlayer.IsUpdateHideoutPlayerInventoryInProgress;
			}
			return false;
		}
	}

	public bool FlashLightState
	{
		[CompilerGenerated]
		get
		{
			return _E01D;
		}
		[CompilerGenerated]
		private set
		{
			_E01D = value;
		}
	}

	public HideoutArea HideoutArea
	{
		[CompilerGenerated]
		get
		{
			return _E01E;
		}
		[CompilerGenerated]
		private set
		{
			_E01E = value;
		}
	}

	protected override bool SetItemInHandsImmediately => false;

	public override _E7B4 PlayerInputTranslator
	{
		get
		{
			if (!InShootingRange)
			{
				return _E015;
			}
			return base.PlayerInputTranslator;
		}
		set
		{
			base.PlayerInputTranslator = value;
		}
	}

	public override _E7B8 HandsInputTranslator
	{
		get
		{
			if (!InShootingRange)
			{
				return _E014;
			}
			return base.HandsInputTranslator;
		}
		set
		{
			base.HandsInputTranslator = value;
		}
	}

	public event Action<bool> OnShootingRangeStatusChange
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E00E;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E00E, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E00E;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E00E, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<HideoutArea> OnSelectArea
	{
		[CompilerGenerated]
		add
		{
			Action<HideoutArea> action = _E00F;
			Action<HideoutArea> action2;
			do
			{
				action2 = action;
				Action<HideoutArea> value2 = (Action<HideoutArea>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E00F, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<HideoutArea> action = _E00F;
			Action<HideoutArea> action2;
			do
			{
				action2 = action;
				Action<HideoutArea> value2 = (Action<HideoutArea>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E00F, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<HideoutArea> OnSpecialAreaActionSelection
	{
		[CompilerGenerated]
		add
		{
			Action<HideoutArea> action = _E010;
			Action<HideoutArea> action2;
			do
			{
				action2 = action;
				Action<HideoutArea> value2 = (Action<HideoutArea>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E010, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<HideoutArea> action = _E010;
			Action<HideoutArea> action2;
			do
			{
				action2 = action;
				Action<HideoutArea> value2 = (Action<HideoutArea>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E010, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnToggleInfoIcons
	{
		[CompilerGenerated]
		add
		{
			Action action = _E011;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E011, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E011;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E011, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnExitQte
	{
		[CompilerGenerated]
		add
		{
			Action action = _E012;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E012, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E012;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E012, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void EnterShootingRange()
	{
		if (!HideoutPlayer.IsUpdateHideoutPlayerInventoryInProgress)
		{
			if (FlashLightState)
			{
				_E001();
			}
			_E000(status: true).HandleExceptions();
		}
	}

	public async Task ExitShootingRange()
	{
		while (HideoutPlayer.IsUpdateHideoutPlayerInventoryInProgress)
		{
			await Task.Yield();
		}
		await _E000(status: false);
		_E002();
	}

	public void SetPointOfView(bool firstPerson)
	{
		if (FirstPersonMode != firstPerson || _E016)
		{
			if (firstPerson)
			{
				base.Player.Speaker.ReplaceVoice(base.Player.Profile.Info.Side, base.Player.Profile.Info.Voice);
			}
			_E016 = false;
			FirstPersonMode = firstPerson;
			HideoutPlayer.VisorVisibility = FirstPersonMode;
			base.Player.BodyAnimatorCommon.enabled = FirstPersonMode;
			if (FirstPersonMode)
			{
				BattleUIScreenController.ShowScreen(EScreenState.Queued);
			}
			else if (!BattleUIScreenController.Closed)
			{
				BattleUIScreenController.CloseScreen();
			}
			GamePlayerOwner.SetIgnoreInput(!FirstPersonMode);
			base.Player.PointOfView = ((!FirstPersonMode) ? EPointOfView.FreeCamera : EPointOfView.FirstPerson);
			_E002();
			if (FirstPersonMode)
			{
				base.Player.Teleport(_E018);
				HideoutPlayer.SetPatrol(patrol: true);
			}
			else
			{
				_E018 = base.Player.Transform.position;
				base.Player.Teleport(_E017);
			}
			Singleton<_E815>.Instance.IsClientBusy = false;
		}
	}

	public void DecidePatrolStatus(bool force = false)
	{
		if (force || !HideoutPlayer.IsUpdateHideoutPlayerInventoryInProgress)
		{
			Vector2 rotation = base.Player.MovementContext.Rotation;
			bool flag = rotation.x < -50f || rotation.x > 50f;
			if (force || HideoutPlayer.IsInPatrol != flag)
			{
				HideoutPlayer.SetPatrol(flag);
			}
		}
	}

	public void SelectArea(HideoutArea area)
	{
		_E00F?.Invoke(area);
	}

	public void SelectSpecialAreaAction(HideoutArea area)
	{
		_E010?.Invoke(area);
	}

	public void ExitQte()
	{
		_E012?.Invoke();
	}

	public void HideoutAreaInteraction([CanBeNull] HideoutArea area, bool inSight)
	{
		if (area == HideoutArea || inSight)
		{
			HideoutArea = (inSight ? area : null);
			base.Player.SearchForInteractions();
		}
	}

	internal override void _E023()
	{
		if (base.State == EState.Started)
		{
			base._E023();
		}
	}

	protected override void Init()
	{
		HideoutPlayer = (HideoutPlayer)base.Player;
		HideoutPlayer.SetEnvironment(EnvironmentType.Indoor);
		_E014 = new _E7B9();
		_E015 = new _E7B5();
		_E000(status: false, forced: true).HandleExceptions();
		base.Player.MovementContext.BlockFirearms = true;
	}

	protected override void InitBattleUIScreen()
	{
		BattleUIScreenController = new BattleUIScreen._E002(this);
	}

	protected override void ShowBattleUIScreen()
	{
	}

	protected override void InteractionsChangedHandler()
	{
		_E633 interactableObject = base.Player.InteractableObject;
		object obj = interactableObject;
		if (obj == null)
		{
			interactableObject = base.Player.PlaceItemZone;
			obj = interactableObject;
			if (obj == null)
			{
				interactableObject = base.Player.ExfiltrationPoint;
				obj = interactableObject ?? HideoutArea;
			}
		}
		_EC3F availableHideoutActions = _E7C0.GetAvailableHideoutActions(this, (_E633)obj);
		availableHideoutActions?.DefaultSelected();
		AvailableInteractionState.Value = availableHideoutActions;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		switch ((int)command)
		{
		case 1:
			if (!HideoutPlayer.IsInPatrol)
			{
				break;
			}
			goto case 26;
		case 26:
		case 57:
		case 58:
		case 85:
			return ETranslateResult.Ignore;
		case 59:
			HideoutPlayer.ToggleNightVision();
			return ETranslateResult.Ignore;
		}
		ETranslateResult eTranslateResult = base.TranslateCommand(command);
		if (eTranslateResult != 0)
		{
			return eTranslateResult;
		}
		switch (command)
		{
		case ECommand.ToggleAlternativeShooting:
			if (!InShootingRange)
			{
				_E001();
			}
			return ETranslateResult.Block;
		case ECommand.EndAlternativeShooting:
			return ETranslateResult.Block;
		case ECommand.ToggleInfo:
			if (!InShootingRange)
			{
				_E011?.Invoke();
			}
			return ETranslateResult.Block;
		default:
			return ETranslateResult.Ignore;
		}
	}

	protected override bool TranslateInventoryScreenInput(ECommand command)
	{
		if (!command.IsCommand(ECommand.ToggleInventory))
		{
			return false;
		}
		if (InShootingRange)
		{
			return false;
		}
		ShowInventoryScreen(delegate
		{
			HideoutPlayer.SetInventoryOpened(opened: false);
		}, HideoutPlayer.HealthController, HideoutPlayer.OriginalInventory, HideoutPlayer._E0DC, HideoutPlayer.OriginalInventory.Inventory.Stash, InventoryScreen.EInventoryTab.Unchanged);
		base.Player.SetInventoryOpened(opened: true);
		return true;
	}

	protected override void ShowInventoryScreen(Action exitAction, _E9C4 healthController, _EAED controller, _E935 questController, _EA40 lootItem, InventoryScreen.EInventoryTab tab)
	{
		InventoryScreen._E002 obj = new InventoryScreen._E002(base.Session, healthController, controller, questController, new _EA40[1] { lootItem }, tab);
		obj.OnClose += delegate
		{
			base.Player.UpdateInteractionCast();
			exitAction();
		};
		obj.ShowScreen(EScreenState.Queued);
	}

	protected override bool TranslateExitScreenInput(ECommand command)
	{
		if (!command.IsCommand(ECommand.Escape))
		{
			return false;
		}
		if (base.Player.PointOfView != 0)
		{
			return true;
		}
		if (InShootingRange)
		{
			ExitShootingRange().HandleExceptions();
			return true;
		}
		if (!BattleUIScreenController.Closed)
		{
			BattleUIScreenController.CloseScreen();
			SelectArea(null);
			return true;
		}
		return base.TranslateExitScreenInput(command);
	}

	protected override _E7B4 PlayerInputTranslatorFactory(Player player)
	{
		return new _E7B1(player);
	}

	protected override _E7B8 GrenadeInputTranslatorFactory(_E6CC grenadeController)
	{
		return new _E7B2(grenadeController);
	}

	private async Task _E000(bool status, bool forced = false)
	{
		if (InShootingRange != status || forced)
		{
			InShootingRange = status;
			if (!InShootingRange)
			{
				await HideoutPlayer.ReleaseShootingRangeInventory();
			}
			else
			{
				await HideoutPlayer.UpdateHideoutPlayerInventory();
			}
			DecidePatrolStatus(force: true);
			_E00E?.Invoke(InShootingRange);
			base.Player.SearchForInteractions();
		}
	}

	private void _E001()
	{
		if (!(_E8A8.Instance.Flashlight == null))
		{
			FlashLightState = !FlashLightState;
			_E002();
		}
	}

	private void _E002()
	{
		if (!(_E8A8.Instance.Flashlight == null))
		{
			_E8A8.Instance.Flashlight.SetState(_E003());
		}
	}

	private bool _E003()
	{
		if (InShootingRange)
		{
			return false;
		}
		return FlashLightState;
	}

	private void _E004(bool hide)
	{
		HideoutPlayer.PlayerBones.HolsterPrimary.gameObject.SetActive(!hide);
		HideoutPlayer.PlayerBones.HolsterSecondary.gameObject.SetActive(!hide);
		Item containedItem = HideoutPlayer._E0DE.Inventory.Equipment.GetSlot(EquipmentSlot.Backpack).ContainedItem;
		PlayerBody._E000 slotViewByItem = HideoutPlayer.PlayerBody.GetSlotViewByItem(containedItem);
		if (slotViewByItem.ParentedModel.Value != null)
		{
			slotViewByItem.ParentedModel.Value.SetActive(!hide);
		}
	}

	public void PrepareWorkout(RuntimeAnimatorController animatorController, Vector3 playerPosition, Vector3 playerRotation, Transform lDumbbell, Transform rDumbbell, QteHandleData.PropsData leftDumbbellData, QteHandleData.PropsData rightDumbbellData)
	{
		HideoutPlayer.CustomAnimationsAreProcessing = true;
		_E004(hide: true);
		_E019 = HideoutPlayer._animators[0].runtimeAnimatorController;
		HideoutPlayer._animators[0].runtimeAnimatorController = animatorController;
		HideoutPlayer._animators[0].enabled = true;
		HideoutPlayer.PlayerBody.UpdatePlayerRenders(EPointOfView.ThirdPerson, HideoutPlayer.Side);
		HideoutPlayer.PlayerBody.PointOfView.Value = EPointOfView.FreeCamera;
		base.Player.Teleport(playerPosition);
		base.Player.Transform.rotation = Quaternion.Euler(playerRotation);
		_E006(lDumbbell, base.Player.PlayerBones.LeftPalm, leftDumbbellData);
		_E006(rDumbbell, base.Player.PlayerBones.RightPalm, rightDumbbellData);
		HideoutPlayer.SetPatrol(patrol: true);
	}

	public void StopWorkout()
	{
		HideoutPlayer.CustomAnimationsAreProcessing = false;
		_E004(hide: false);
		base.Player.Teleport(_E018);
		HideoutPlayer._animators[0].runtimeAnimatorController = _E019;
		HideoutPlayer._animators[0].Reset();
		_E342[] playerStates = (_E2B6.Config.UseSpiritPlayer ? HideoutPlayer.Spirit.BodyAnimatorWrapper.GetBehaviours<_E342>() : HideoutPlayer._animators[0].GetBehaviours<_E342>());
		HideoutPlayer.MovementContext.InitMovementStates(playerStates);
		DecidePatrolStatus(force: true);
	}

	[CompilerGenerated]
	private void _E005()
	{
		HideoutPlayer.SetInventoryOpened(opened: false);
	}

	[CompilerGenerated]
	internal static void _E006(Transform dumbbell, Transform parent, QteHandleData.PropsData transformData)
	{
		dumbbell.SetParent(parent);
		dumbbell.localPosition = transformData.Position;
		dumbbell.localRotation = Quaternion.Euler(transformData.Rotation);
	}
}
