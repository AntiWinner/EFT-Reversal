using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.Screens;
using JetBrains.Annotations;
using RootMotion.FinalIK;
using UnityEngine;

namespace EFT;

public class GamePlayerOwner : PlayerOwner
{
	[CompilerGenerated]
	private new sealed class _E000<_E0A9> where _E0A9 : GamePlayerOwner
	{
		public Player player;

		public _E0A9 owner;

		internal void _E000()
		{
			player.PossibleInteractionsChanged -= owner.InteractionsChangedHandler;
		}

		internal void _E001(_EAF3 removeItemEventArgs)
		{
			owner.ClearInteractionState();
		}

		internal void _E002()
		{
			player._E0DE.RemoveItemEvent -= delegate
			{
				owner.ClearInteractionState();
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public GamePlayerOwner _003C_003E4__this;

		public Weapon weapon;

		public _EA6A currentMagazine;

		internal void _E000(Item item)
		{
			_003C_003E4__this._E004(weapon, item);
		}

		internal bool _E001(_EA12 ammo)
		{
			if (ammo.StackObjectsCount > 0 && currentMagazine.CheckCompatibility(ammo))
			{
				return _003C_003E4__this.Player._E0DE.Examined(ammo);
			}
			return false;
		}

		internal bool _E002(_EA12 ammo)
		{
			if (ammo.StackObjectsCount > 0 && weapon.Chambers[0].CanAccept(ammo))
			{
				return _003C_003E4__this.Player._E0DE.Examined(ammo);
			}
			return false;
		}

		internal bool _E003(_EA12 ammo)
		{
			if (ammo.StackObjectsCount > 0 && weapon.Chambers[0].CanAccept(ammo))
			{
				return _003C_003E4__this.Player._E0DE.Examined(ammo);
			}
			return false;
		}

		internal bool _E004(_EA12 ammo)
		{
			if (ammo.StackObjectsCount > 0 && weapon.Chambers[0].CanAccept(ammo))
			{
				return _003C_003E4__this.Player._E0DE.Examined(ammo);
			}
			return false;
		}

		internal void _E005(Item item)
		{
			_003C_003E4__this._E004(weapon, item);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public _EA62 underbarrelWeapon;

		public _E002 CS_0024_003C_003E8__locals1;

		internal bool _E000(_EA12 ammo)
		{
			if (ammo.StackObjectsCount > 0 && underbarrelWeapon.Chamber.CanAccept(ammo))
			{
				return CS_0024_003C_003E8__locals1._003C_003E4__this.Player._E0DE.Examined(ammo);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _EB20 magazineSlotItemAddress;

		public _E002 CS_0024_003C_003E8__locals2;

		internal bool _E000(_EA6A mag)
		{
			return _EB29.CheckMoveIgnoringTargetItem(mag, magazineSlotItemAddress, CS_0024_003C_003E8__locals2._003C_003E4__this.Player._E0DE).OrElse(elseValue: false);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public _EB20 magazineSlotItemAddress;

		public _E002 CS_0024_003C_003E8__locals3;

		internal bool _E000(_EA6A mag)
		{
			return _EB29.CheckMoveIgnoringTargetItem(mag, magazineSlotItemAddress, CS_0024_003C_003E8__locals3._003C_003E4__this.Player._E0DE).OrElse(elseValue: false);
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public _EA6A currentMagazine;

		internal _EB22 _E000(_E9EF grid)
		{
			return grid.FindLocationForItem(currentMagazine);
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public _EA12 containedAmmo;

		internal _EB22 _E000(_E9EF grid)
		{
			return grid.FindLocationForItem(containedAmmo);
		}
	}

	[CompilerGenerated]
	private sealed class _E008
	{
		public GamePlayerOwner _003C_003E4__this;

		public Action callback;

		internal void _E000()
		{
			_003C_003E4__this.Player.SetInventoryOpened(opened: false);
			callback();
		}
	}

	[CompilerGenerated]
	private sealed class _E009
	{
		public GamePlayerOwner _003C_003E4__this;

		public Action exitAction;

		internal void _E000()
		{
			_003C_003E4__this.Player.UpdateInteractionCast();
			exitAction();
		}
	}

	private new static bool m__E000;

	[CompilerGenerated]
	private static bool m__E001;

	[CompilerGenerated]
	private static bool m__E002;

	private _E554.Location m__E003;

	private BattleUIPanelExtraction m__E004;

	private ExtractionTimersPanel m__E005;

	private _E6CB m__E006;

	private bool m__E007;

	private _ECB1 m__E008;

	private bool m__E009;

	[CompilerGenerated]
	private _E796 _E00A;

	protected BattleUIScreen._E000 BattleUIScreenController;

	private _E629 _E00B;

	private static Player _E00C;

	public _ECF5<_EC3F> AvailableInteractionState = new _ECF5<_EC3F>();

	public static bool IgnoreInputInNPCDialog
	{
		[CompilerGenerated]
		get
		{
			return GamePlayerOwner.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			GamePlayerOwner.m__E001 = value;
		}
	}

	public static bool IgnoreInputWithKeepResetLook
	{
		[CompilerGenerated]
		get
		{
			return GamePlayerOwner.m__E002;
		}
		[CompilerGenerated]
		private set
		{
			GamePlayerOwner.m__E002 = value;
		}
	}

	protected _E796 Session
	{
		[CompilerGenerated]
		get
		{
			return _E00A;
		}
		[CompilerGenerated]
		private set
		{
			_E00A = value;
		}
	}

	public static Player MyPlayer => _E00C;

	public static TPlayerOwner Create<TPlayerOwner>(Player player, _E7FD inputTree, _ECB1 insurance, _E796 session, CommonUI commonUI, PreloaderUI preloaderUI, GameUI gameUI, _E629 gameDateTime, [CanBeNull] _E554.Location location) where TPlayerOwner : GamePlayerOwner
	{
		TPlayerOwner owner = PlayerOwner._E000<TPlayerOwner>(player, inputTree);
		owner._E00B = gameDateTime;
		_E00C = player;
		player.HasGamePlayerOwner = true;
		player.POM.On();
		((GamePlayerOwner)owner).m__E004 = gameUI.BattleUiPanelExtraction;
		((GamePlayerOwner)owner).m__E005 = gameUI.TimerPanel;
		owner.m__E008 = insurance;
		owner.Session = session;
		((GamePlayerOwner)owner).m__E003 = location;
		owner._children.Add(commonUI.MenuScreen);
		owner._children.Add(commonUI.ReconnectionScreen);
		owner._children.Add(commonUI.InventoryScreen);
		owner._children.Add(commonUI.WeaponModdingScreen);
		owner._children.Add(commonUI.EditBuildScreen);
		owner._children.Add(commonUI.SettingsScreen);
		owner._children.Add(commonUI.HandbookScreen);
		owner._children.Add(commonUI.TraderDialogScreen);
		owner._children.Add(gameUI.BattleUiScreen.AmmoSelector);
		owner._children.Add(gameUI.BattleUIGesturesMenu);
		owner._children.Add(gameUI.BattleUiScreen.ActionPanel);
		owner._children.Add(gameUI.GesturesQuickPanel.DropdownPanel);
		owner._children.Add(gameUI.PostFXPreview);
		owner._children.Add(commonUI.ChatScreen);
		owner._children.Add(ItemUiContext.Instance);
		owner._children.Add(preloaderUI.Console);
		owner._children.Add(preloaderUI.ErrorScreenInputNode);
		owner.CheckDuplicateChildren();
		commonUI.InventoryScreen.RecacheChildren();
		owner.InitBattleUIScreen();
		player.PossibleInteractionsChanged += owner.InteractionsChangedHandler;
		owner.OnDestroyCompositeDisposable.AddDisposable(delegate
		{
			player.PossibleInteractionsChanged -= owner.InteractionsChangedHandler;
		});
		player._E0DE.RemoveItemEvent += delegate
		{
			owner.ClearInteractionState();
		};
		owner.OnDestroyCompositeDisposable.AddDisposable(delegate
		{
			player._E0DE.RemoveItemEvent -= delegate
			{
				owner.ClearInteractionState();
			};
		});
		player.Grounder.solver.quality = Grounding.Quality.Fastest;
		if (Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Instance.SetProtagonist(player);
		}
		owner.Init();
		return owner;
	}

	internal static GamePlayerOwner _E000(Player player, _E7FD inputTree, _ECB1 insurance, _E796 session, CommonUI commonUI, PreloaderUI preloaderUI, GameUI gameUI, _E629 gameDateTime, [CanBeNull] _E554.Location location)
	{
		return Create<GamePlayerOwner>(player, inputTree, insurance, session, commonUI, preloaderUI, gameUI, gameDateTime, location);
	}

	protected virtual void Init()
	{
	}

	internal override void _E022()
	{
		ShowBattleUIScreen();
		base._E022();
		if (this.m__E006 == null)
		{
			Debug.Log(this.m__E006);
		}
	}

	public static void SetIgnoreInput(bool ignore)
	{
		GamePlayerOwner.m__E000 = ignore;
	}

	public static void SetIgnoreInputInNPCDialog(bool ignore)
	{
		IgnoreInputInNPCDialog = ignore;
	}

	public static void SetIgnoreInputWithKeepResetLook(bool ignore)
	{
		IgnoreInputWithKeepResetLook = ignore;
	}

	protected virtual void InitBattleUIScreen()
	{
		BattleUIScreenController = new BattleUIScreen._E001(this, _E00B, this.m__E003);
	}

	protected virtual void ShowBattleUIScreen()
	{
		BattleUIScreenController.OnClose += delegate
		{
			this.m__E005.Hide();
			MonoBehaviourSingleton<PreloaderUI>.Instance.RaidInfoVisibility = false;
		};
		BattleUIScreenController.OnShow += delegate
		{
			this.m__E005.Reveal();
			MonoBehaviourSingleton<PreloaderUI>.Instance.RaidInfoVisibility = true;
		};
		BattleUIScreenController.ShowScreen(EScreenState.Root);
	}

	protected override void CleanupOnDestroy()
	{
		_E00C = null;
		_E023();
		base.CleanupOnDestroy();
	}

	internal override void _E023()
	{
		if (base.State != EState.Started)
		{
			return;
		}
		using (_ECC9.ReleaseBeginSampleWithToken(_ED3E._E000(192662), _ED3E._E000(150596)))
		{
			foreach (UIInputNode item in from child in _children.OfType<UIInputNode>()
				where child != null && child.gameObject.activeSelf && !(child is ConsoleScreen)
				select child)
			{
				item.Close();
			}
			StaticManager.Instance.StartCoroutine(_E001());
			SetIgnoreInput(ignore: false);
			SetIgnoreInputInNPCDialog(ignore: false);
			SetIgnoreInputWithKeepResetLook(ignore: false);
			base._E023();
		}
	}

	private static IEnumerator _E001()
	{
		yield return null;
		yield return null;
		_EC92.Instance.ToggleScreen(EEftScreenType.BattleUI);
	}

	public void CloseObjectivesPanel()
	{
		this.m__E004.Close();
	}

	public void ShowObjectivesPanel(string text, float time)
	{
		this.m__E004.Show(text, time);
	}

	private void LateUpdate()
	{
		base.Player.InteractionRaycast();
		_E002();
	}

	private void _E002()
	{
		if (base.Player.InteractableObject is Door && base.Player.HealthController.IsAlive)
		{
			bool flag = (base.Player.InteractableObject as Door).IsBreachAngle(base.Player.Position);
			if (flag && !this.m__E009)
			{
				this.m__E009 = true;
				InteractionsChangedHandler();
			}
			else if (!flag && this.m__E009)
			{
				this.m__E009 = false;
				InteractionsChangedHandler();
			}
		}
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		if (GamePlayerOwner.m__E000)
		{
			return;
		}
		if (!IgnoreInputWithKeepResetLook)
		{
			if (IgnoreInputInNPCDialog)
			{
				if (base.Player.IsLooking || base.Player.IsResettingLook || axes[4] != 0f || axes[5] != 0f)
				{
					axes[0] = 0f;
					axes[1] = 0f;
					base.TranslateAxes(ref axes);
				}
			}
			else
			{
				base.TranslateAxes(ref axes);
			}
		}
		else if (base.Player.IsResettingLook)
		{
			base.TranslateAxes(ref axes);
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if ((IgnoreInputInNPCDialog || IgnoreInputWithKeepResetLook) && command != ECommand.ResetLookDirection && command != ECommand.DisplayTimer && command != ECommand.DisplayTimerAndExits)
		{
			return ETranslateResult.BlockAll;
		}
		if (GamePlayerOwner.m__E000 && command != ECommand.ChangePointOfView && command != ECommand.ToggleTalk && command != ECommand.StopTalk)
		{
			return ETranslateResult.BlockAll;
		}
		if (!base.Player.HealthController.IsAlive)
		{
			return ETranslateResult.BlockAll;
		}
		base.Player.CurrentState.Cancel();
		if (TranslateInventoryScreenInput(command))
		{
			return ETranslateResult.BlockAll;
		}
		if (TranslateExitScreenInput(command))
		{
			return ETranslateResult.BlockAll;
		}
		if (_E003(command))
		{
			return ETranslateResult.BlockAll;
		}
		switch (command)
		{
		case ECommand.DisplayTimer:
			this.m__E005.ShowTimer(showExits: false);
			break;
		case ECommand.DisplayTimerAndExits:
			this.m__E005.ShowTimer(showExits: true, updateExits: true);
			break;
		}
		base.TranslateCommand(command);
		return ETranslateResult.Ignore;
	}

	protected virtual bool TranslateInventoryScreenInput(ECommand command)
	{
		if (!command.IsCommand(ECommand.ToggleInventory) || !_EC92.Instance.CheckCurrentScreen(EEftScreenType.BattleUI))
		{
			return false;
		}
		ShowInventoryScreen(delegate
		{
			base.Player.SetInventoryOpened(opened: false);
		}, base.Player.HealthController, base.Player._E0DE, base.Player._E0DC, null, InventoryScreen.EInventoryTab.Unchanged);
		base.Player.SetInventoryOpened(opened: true);
		return true;
	}

	protected virtual bool TranslateExitScreenInput(ECommand command)
	{
		if (!command.IsCommand(ECommand.Escape))
		{
			return false;
		}
		new MenuScreen._E003().ShowScreen(EScreenState.Queued);
		return true;
	}

	private bool _E003(ECommand command)
	{
		if (this.m__E006 == null)
		{
			return false;
		}
		Weapon weapon = this.m__E006.Item;
		if (weapon.IsUnderBarrelDeviceActive)
		{
			if ((!command.IsCommand(ECommand.NextMagazine) && !command.IsCommand(ECommand.PreviousMagazine)) || !BattleUIScreenController.AmmoSelectorAvailable || !this.m__E006.CanStartReload())
			{
				return false;
			}
			_EA62 underbarrelWeapon = weapon.GetUnderbarrelWeapon();
			List<Item> list = new List<Item>((from ammo in base.Player._E0DE.GetReachableItemsOfType<_EA12>()
				where ammo.StackObjectsCount > 0 && underbarrelWeapon.Chamber.CanAccept(ammo) && base.Player._E0DE.Examined(ammo)
				select ammo).Cast<Item>().ToList());
			if (list != null && list.Count > 0)
			{
				BattleUIScreenController.ShowReloadPanel(weapon, list, this.m__E008, delegate(Item item)
				{
					_E004(weapon, item);
				});
				return true;
			}
			return false;
		}
		_EA6A currentMagazine = weapon.GetCurrentMagazine();
		if ((!command.IsCommand(ECommand.NextMagazine) && !command.IsCommand(ECommand.PreviousMagazine)) || !BattleUIScreenController.AmmoSelectorAvailable || !this.m__E006.CanStartReload() || (weapon.ReloadMode != 0 && weapon.ReloadMode != Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport && weapon.ReloadMode != Weapon.EReloadMode.OnlyBarrel && (weapon.ReloadMode != Weapon.EReloadMode.InternalMagazine || currentMagazine != null) && (weapon.ReloadMode != Weapon.EReloadMode.InternalMagazine || currentMagazine == null || currentMagazine.Count >= currentMagazine.MaxCount)))
		{
			return false;
		}
		List<Item> list2 = null;
		if (weapon.ReloadMode == Weapon.EReloadMode.ExternalMagazine)
		{
			Slot magazineSlot = weapon.GetMagazineSlot();
			_EB20 magazineSlotItemAddress2 = new _EB20(magazineSlot);
			list2 = (from mag in (from mag in base.Player._E0DE.GetReachableItemsOfType<_EA6A>()
					where mag.Count > 0
					select mag).ToArray()
				where _EB29.CheckMoveIgnoringTargetItem(mag, magazineSlotItemAddress2, base.Player._E0DE).OrElse(elseValue: false)
				where mag.CheckAction(null)
				select mag).Cast<Item>().ToList();
		}
		else if (weapon.ReloadMode == Weapon.EReloadMode.InternalMagazine && currentMagazine != null)
		{
			list2 = ((!(weapon is _EAD1)) ? (from ammo in base.Player._E0DE.GetReachableItemsOfType<_EA12>()
				where ammo.StackObjectsCount > 0 && weapon.Chambers[0].CanAccept(ammo) && base.Player._E0DE.Examined(ammo)
				where ammo.CheckAction(null)
				select ammo).Cast<Item>().ToList() : (from ammo in base.Player._E0DE.GetReachableItemsOfType<_EA12>()
				where ammo.StackObjectsCount > 0 && currentMagazine.CheckCompatibility(ammo) && base.Player._E0DE.Examined(ammo)
				where ammo.CheckAction(null)
				select ammo).Cast<Item>().ToList());
		}
		else if (weapon.ReloadMode == Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport || (weapon.ReloadMode == Weapon.EReloadMode.InternalMagazine && currentMagazine == null))
		{
			List<Item> collection = (from ammo in base.Player._E0DE.GetReachableItemsOfType<_EA12>()
				where ammo.StackObjectsCount > 0 && weapon.Chambers[0].CanAccept(ammo) && base.Player._E0DE.Examined(ammo)
				where ammo.CheckAction(null)
				select ammo).Cast<Item>().ToList();
			Slot magazineSlot2 = weapon.GetMagazineSlot();
			_EB20 magazineSlotItemAddress = new _EB20(magazineSlot2);
			list2 = new List<Item>((from mag in (from mag in base.Player._E0DE.GetReachableItemsOfType<_EA6A>()
					where mag.Count > 0
					select mag).ToArray()
				where _EB29.CheckMoveIgnoringTargetItem(mag, magazineSlotItemAddress, base.Player._E0DE).OrElse(elseValue: false)
				where mag.CheckAction(null)
				select mag).Cast<Item>().ToList());
			if (currentMagazine != null)
			{
				list2.AddRange(collection);
			}
		}
		else if (weapon.ReloadMode == Weapon.EReloadMode.OnlyBarrel)
		{
			list2 = new List<Item>((from ammo in base.Player._E0DE.GetReachableItemsOfType<_EA12>()
				where ammo.StackObjectsCount > 0 && weapon.Chambers[0].CanAccept(ammo) && base.Player._E0DE.Examined(ammo)
				select ammo).Cast<Item>().ToList());
		}
		if (list2 != null && list2.Count > 0)
		{
			BattleUIScreenController.ShowReloadPanel(weapon, list2, this.m__E008, delegate(Item item)
			{
				_E004(weapon, item);
			});
			return true;
		}
		return false;
	}

	private void _E004(Weapon weapon, Item item)
	{
		if (this.m__E006 == null)
		{
			return;
		}
		_EA6A obj = item as _EA6A;
		_EA12 obj2 = item as _EA12;
		if (item == null || item.Owner != base.Player._E0DE)
		{
			return;
		}
		if (obj != null && (weapon.ReloadMode == Weapon.EReloadMode.ExternalMagazine || weapon.ReloadMode == Weapon.EReloadMode.ExternalMagazineWithInternalReloadSupport || (weapon.ReloadMode == Weapon.EReloadMode.InternalMagazine && weapon.GetCurrentMagazine() == null)))
		{
			_EA6A currentMagazine = weapon.GetCurrentMagazine();
			_EB20 to = new _EB20(weapon.GetMagazineSlot());
			if (_EB29.CheckMoveIgnoringTargetItem(obj, to, base.Player._E0DE).OrElse(elseValue: false))
			{
				_EB22 gridItemAddress = ((currentMagazine != null) ? (from grid in base.Player._E0DE.Inventory.Equipment.GetPrioritizedGridsForUnloadedObject()
					select grid.FindLocationForItem(currentMagazine) into g
					where g != null
					orderby g.Grid.GridWidth.Value * g.Grid.GridHeight.Value
					select g).FirstOrDefault((_EB22 x) => x != null) : null);
				this.m__E006.ReloadMag(obj, gridItemAddress, null);
			}
		}
		else if (obj2 != null && weapon.IsUnderBarrelDeviceActive)
		{
			this.m__E006.ReloadGrenadeLauncher(new _E9CF(new List<_EA12> { obj2 }), null);
		}
		else if (obj2 != null && weapon.SupportsInternalReload)
		{
			if (weapon is _EAD1)
			{
				this.m__E006.ReloadRevolverDrum(new _E9CF(new List<_EA12> { obj2 }), null, quickReload: false);
			}
			else
			{
				this.m__E006.ReloadWithAmmo(new _E9CF(new List<_EA12> { obj2 }), null);
			}
		}
		else if (obj2 != null && weapon.ReloadMode == Weapon.EReloadMode.OnlyBarrel)
		{
			Slot slot = (weapon.IsMultiBarrel ? weapon.FirstFreeChamberSlot : weapon.Chambers[0]);
			if (slot != null)
			{
				_EA12 containedAmmo;
				_EB22 placeToPutContainedAmmoMagazine = (((containedAmmo = slot.ContainedItem as _EA12) != null && !containedAmmo.IsUsed) ? (from grid in base.Player._E0DE.Inventory.Equipment.GetPrioritizedGridsForUnloadedObject()
					select grid.FindLocationForItem(containedAmmo) into g
					where g != null
					orderby g.Grid.GridWidth.Value * g.Grid.GridHeight.Value
					select g).FirstOrDefault((_EB22 x) => x != null) : null);
				this.m__E006.ReloadBarrels(new _E9CF(new List<_EA12> { obj2 }), placeToPutContainedAmmoMagazine, null);
			}
		}
		else
		{
			Debug.LogError(string.Concat(_ED3E._E000(192703), weapon.ReloadMode, _ED3E._E000(2540), item));
		}
	}

	public void CloseInventoryIfOpen()
	{
		_EC92.Instance.ToggleScreen(EEftScreenType.Inventory);
	}

	public void ShowInventoryScreenLoot(_EA40 loot, Action callback)
	{
		base.Player.SetInventoryOpened(opened: true);
		ShowInventoryScreen(delegate
		{
			base.Player.SetInventoryOpened(opened: false);
			callback();
		}, base.Player.HealthController, base.Player._E0DE, base.Player._E0DC, loot, InventoryScreen.EInventoryTab.Gear);
	}

	protected virtual void ShowInventoryScreen(Action exitAction, _E9C4 healthController, _EAED controller, _E935 questController, [CanBeNull] _EA40 lootItem, InventoryScreen.EInventoryTab tab)
	{
		if (!_EC92.Instance.CheckCurrentScreen(EEftScreenType.BattleUI))
		{
			Debug.Log(_ED3E._E000(192681));
			exitAction();
			return;
		}
		InventoryScreen._E001 obj = new InventoryScreen._E001(Session, base.Player.Profile, healthController, controller, questController, (lootItem == null) ? null : new _EA40[1] { lootItem }, tab);
		obj.OnClose += delegate
		{
			base.Player.UpdateInteractionCast();
			exitAction();
		};
		obj.ShowScreen(EScreenState.Queued);
	}

	protected virtual void InteractionsChangedHandler()
	{
		_E633 interactableObject = base.Player.InteractableObject;
		object obj = interactableObject;
		if (obj == null)
		{
			interactableObject = base.Player.PlaceItemZone;
			obj = interactableObject ?? base.Player.ExfiltrationPoint;
		}
		_EC3F availableActions = _E7C0.GetAvailableActions(this, (_E633)obj);
		availableActions?.InitSelected();
		AvailableInteractionState.Value = availableActions;
	}

	public void ClearInteractionState()
	{
		AvailableInteractionState.Value = null;
	}

	public void DisplayPreloaderUiNotification(string message)
	{
		_E857.DisplayMessageNotification(message);
	}

	protected override void SetHandsController(_E6CB controller)
	{
		base.SetHandsController(controller);
		this.m__E006 = controller;
		base.Player.SetInventoryOpened(_EC92.Instance.CheckCurrentScreen(EEftScreenType.Inventory));
	}

	protected override void SetHandsController(_E6CC grenadeController)
	{
		base.SetHandsController(grenadeController);
		base.Player.SetInventoryOpened(_EC92.Instance.CheckCurrentScreen(EEftScreenType.Inventory));
	}

	protected override void SetHandsController(_E6C9 controller)
	{
		base.SetHandsController(controller);
		this.m__E006 = null;
		base.Player.SetInventoryOpened(_EC92.Instance.CheckCurrentScreen(EEftScreenType.Inventory));
	}

	protected override void ResetHands()
	{
		this.m__E006 = null;
	}

	[CompilerGenerated]
	private void _E005()
	{
		this.m__E005.Hide();
		MonoBehaviourSingleton<PreloaderUI>.Instance.RaidInfoVisibility = false;
	}

	[CompilerGenerated]
	private void _E006()
	{
		this.m__E005.Reveal();
		MonoBehaviourSingleton<PreloaderUI>.Instance.RaidInfoVisibility = true;
	}

	[CompilerGenerated]
	private void _E007()
	{
		base.Player.SetInventoryOpened(opened: false);
	}
}
