using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.Interactive;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT;

public class PlayerOwner : InputNode, _E6D5
{
	protected enum EState
	{
		None,
		Started,
		Stopped
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public PlayerOwner _003C_003E4__this;

		public Item previousItem;

		public _E6D4 quickUseController;

		internal void _E000(Result<_E6D4> resultController)
		{
			_003C_003E4__this._E003(previousItem, quickUseController);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public PlayerOwner _003C_003E4__this;

		public Item previousItem;

		public _E6CC grenadeController;

		internal void _E000(Result<_E6CC> resultController)
		{
			_003C_003E4__this._E003(previousItem, grenadeController);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public PlayerOwner _003C_003E4__this;

		public Item previousItem;

		public _E6CF medsController;

		internal void _E000(Result<_E6D4> resultController)
		{
			_003C_003E4__this._E003(previousItem, medsController);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public PlayerOwner _003C_003E4__this;

		public Item previousItem;

		public _E6D2 grenadeQuickUseController;

		internal void _E000(Result<_E6D1<_EADF>> resultController)
		{
			_003C_003E4__this._E003(previousItem, grenadeQuickUseController);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public PlayerOwner _003C_003E4__this;

		public Item previousItem;

		public _E6D3 quickKnifeKickController;

		internal void _E000(Result<_E6D1<_EA60>> resultController)
		{
			_003C_003E4__this._E003(previousItem, quickKnifeKickController);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public WorldInteractiveObject worldInteractiveObject;

		internal bool _E000(KeyComponent x)
		{
			return x.Template.KeyId == worldInteractiveObject.KeyId;
		}
	}

	private static readonly WaitForSeconds m__E022 = new WaitForSeconds(3f);

	protected readonly _E3A4 OnDestroyCompositeDisposable = new _E3A4();

	[CompilerGenerated]
	private EState _E021;

	private _E7B7 m__E023;

	[CompilerGenerated]
	private _E7B4 _E024;

	[CompilerGenerated]
	private _E7B8 _E025;

	[CompilerGenerated]
	private Player _E026;

	[CompilerGenerated]
	private _E7FD _E020;

	[CanBeNull]
	public Item PreviousItem;

	private Coroutine _E027;

	private Action _E028;

	private Action _E029;

	protected EState State
	{
		[CompilerGenerated]
		get
		{
			return _E021;
		}
		[CompilerGenerated]
		private set
		{
			_E021 = value;
		}
	}

	public virtual _E7B4 PlayerInputTranslator
	{
		[CompilerGenerated]
		get
		{
			return _E024;
		}
		[CompilerGenerated]
		set
		{
			_E024 = value;
		}
	}

	public virtual _E7B8 HandsInputTranslator
	{
		[CompilerGenerated]
		get
		{
			return _E025;
		}
		[CompilerGenerated]
		set
		{
			_E025 = value;
		}
	}

	public Player Player
	{
		[CompilerGenerated]
		get
		{
			return _E026;
		}
		[CompilerGenerated]
		private set
		{
			_E026 = value;
		}
	}

	public _E7FD InputTree
	{
		[CompilerGenerated]
		get
		{
			return _E020;
		}
		[CompilerGenerated]
		private set
		{
			_E020 = value;
		}
	}

	protected virtual bool SetItemInHandsImmediately => true;

	internal static _E077 _E000<_E077>(Player player, _E7FD inputTree) where _E077 : PlayerOwner
	{
		_E077 val = player.gameObject.AddComponent<_E077>();
		val.Player = player;
		val.InputTree = inputTree;
		((PlayerOwner)val).m__E023 = new _E7B7(player);
		val.PlayerInputTranslator = val.PlayerInputTranslatorFactory(player);
		return val;
	}

	private void OnDestroy()
	{
		CleanupOnDestroy();
	}

	internal virtual void _E022()
	{
		Player.HandsChangedEvent += _E001;
		_E001(Player.HandsController);
		if (SetItemInHandsImmediately && (!Player.IsInBufferZone || Player.CanManipulateWithHandsInBufferZone) && Player.HandsController is Player.EmptyHandsController)
		{
			Player.SetFirstAvailableItem(delegate
			{
			});
		}
		InputTree.AddWithLowPriority(this);
		Player.PlayerHealthController?.Start();
		State = EState.Started;
		_E7F2 settings = Singleton<_E7DE>.Instance.Control.Settings;
		_E028 = settings.InvertedXAxis.Bind(delegate(bool x)
		{
			this.m__E023.InvertX = x;
		});
		_E029 = settings.InvertedYAxis.Bind(delegate(bool y)
		{
			this.m__E023.InvertY = y;
		});
	}

	internal virtual void _E023()
	{
		using (_ECC9.BeginSampleWithToken(_ED3E._E000(189090), _ED3E._E000(150596)))
		{
			if (State == EState.Started)
			{
				InputTree.Remove(this);
				State = EState.Stopped;
				_E028?.Invoke();
				_E028 = null;
				_E029?.Invoke();
				_E029 = null;
				Player.HandsChangedEvent -= _E001;
			}
		}
	}

	protected virtual _E7B4 PlayerInputTranslatorFactory(Player player)
	{
		return new _E7B6(player);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (_E004(command))
		{
			return ETranslateResult.BlockAll;
		}
		this.m__E023.TranslateCommand(command);
		HandsInputTranslator?.TranslateCommand(command);
		PlayerInputTranslator.TranslateCommand(command);
		return ETranslateResult.Ignore;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		this.m__E023.TranslateAxes(ref axes);
		if (HandsInputTranslator != null && axes != null)
		{
			HandsInputTranslator.TranslateAxes(ref axes);
		}
		PlayerInputTranslator.TranslateAxes(ref axes);
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.LockCursor;
	}

	private void _E001(_E6C7 controller)
	{
		ResetHands();
		if (controller != null)
		{
			if (controller is _E6CB obj)
			{
				_E6CB handsController = obj;
				SetHandsController(handsController);
				return;
			}
			if (controller is _E6C9 obj2)
			{
				_E6C9 handsController2 = obj2;
				SetHandsController(handsController2);
				return;
			}
			if (controller is _E6CC obj3)
			{
				_E6CC handsController3 = obj3;
				SetHandsController(handsController3);
				return;
			}
			if (controller is _E6CF obj4)
			{
				_E6CF handsController4 = obj4;
				SetHandsController(handsController4);
				return;
			}
			if (controller is _E6CD obj5)
			{
				_E6CD handsController5 = obj5;
				SetHandsController(handsController5);
				return;
			}
			if (controller is _E6D2 obj6)
			{
				_E6D2 handsController6 = obj6;
				SetHandsController(handsController6);
				return;
			}
			if (controller is _E6D3 obj7)
			{
				_E6D3 handsController7 = obj7;
				SetHandsController(handsController7);
				return;
			}
			if (controller is _E6D4 obj8)
			{
				_E6D4 quickUseController = obj8;
				_E002(quickUseController);
				return;
			}
			if (controller is _E6CE obj9)
			{
				_E6CE handsController8 = obj9;
				SetHandsController(handsController8);
				return;
			}
		}
		Debug.LogErrorFormat(_ED3E._E000(189134), controller);
	}

	private void _E002(_E6D4 quickUseController)
	{
		HandsInputTranslator = null;
		Item previousItem = PreviousItem;
		quickUseController.SetOnUsedCallback(delegate
		{
			_E003(previousItem, quickUseController);
		});
	}

	protected virtual void SetHandsController(_E6CB controller)
	{
		_E7BA handsInputTranslator = new _E7BA(Player, controller);
		HandsInputTranslator = handsInputTranslator;
		if (Player.MovementContext.StationaryWeapon == null || Player.MovementContext.StationaryWeapon.Item != controller.Item)
		{
			PreviousItem = controller.Item;
		}
	}

	protected virtual void SetHandsController(_E6CC grenadeController)
	{
		_E7B8 handsInputTranslator = GrenadeInputTranslatorFactory(grenadeController);
		HandsInputTranslator = handsInputTranslator;
		Item previousItem = PreviousItem;
		PreviousItem = grenadeController.Item;
		grenadeController.SetOnUsedCallback(delegate
		{
			_E003(previousItem, grenadeController);
		});
	}

	protected virtual void SetHandsController(_E6CE usableItemController)
	{
		_E7BD handsInputTranslator = new _E7BD(usableItemController);
		HandsInputTranslator = handsInputTranslator;
		PreviousItem = usableItemController.Item;
	}

	protected virtual _E7B8 GrenadeInputTranslatorFactory(_E6CC grenadeController)
	{
		return new _E7BB(grenadeController);
	}

	protected virtual void SetHandsController(_E6CD knifeController)
	{
		_E7BC handsInputTranslator = new _E7BC(knifeController);
		HandsInputTranslator = handsInputTranslator;
		PreviousItem = knifeController.Knife.Item;
	}

	protected virtual void SetHandsController(_E6C9 controller)
	{
		_E7BE handsInputTranslator = new _E7BE(controller);
		HandsInputTranslator = handsInputTranslator;
		PreviousItem = controller.Item;
	}

	protected virtual void SetHandsController(_E6CF medsController)
	{
		_E7BF handsInputTranslator = new _E7BF(medsController);
		HandsInputTranslator = handsInputTranslator;
		Item previousItem = ((Player.IsInBufferZone && !Player.CanManipulateWithHandsInBufferZone) ? null : PreviousItem);
		medsController.SetOnUsedCallback(delegate
		{
			_E003(previousItem, medsController);
		});
	}

	protected virtual void SetHandsController(_E6D2 grenadeQuickUseController)
	{
		HandsInputTranslator = null;
		Item previousItem = PreviousItem;
		grenadeQuickUseController.SetOnUsedCallback(delegate
		{
			_E003(previousItem, grenadeQuickUseController);
		});
	}

	protected virtual void SetHandsController(_E6D3 quickKnifeKickController)
	{
		HandsInputTranslator = null;
		Item previousItem = PreviousItem;
		quickKnifeKickController.SetOnUsedCallback(delegate
		{
			_E003(previousItem, quickKnifeKickController);
		});
	}

	private void _E003([CanBeNull] Item previousItem, object controller)
	{
		if (previousItem != null && (Player.HandsController == controller || Player.HandsController == null) && (!Player.IsInBufferZone || Player.CanManipulateWithHandsInBufferZone))
		{
			Player.SetInHands(previousItem, delegate
			{
			});
		}
		else
		{
			Player.SetEmptyHands(delegate
			{
			});
		}
	}

	protected virtual void ResetHands()
	{
		HandsInputTranslator = null;
	}

	private static bool _E004(ECommand command)
	{
		return false;
	}

	[CanBeNull]
	public KeyComponent GetKey(WorldInteractiveObject worldInteractiveObject)
	{
		return Player._E0DE.Inventory.Equipment.GetItemComponentsInChildren<KeyComponent>(onlyMerged: false).FirstOrDefault((KeyComponent x) => x.Template.KeyId == worldInteractiveObject.KeyId);
	}

	public IEnumerable<KeycardComponent> GetAllKeyCards()
	{
		return Player._E0DE.Inventory.Equipment.GetItemComponentsInChildren<KeycardComponent>(onlyMerged: false);
	}

	protected virtual void CleanupOnDestroy()
	{
		OnDestroyCompositeDisposable.Dispose();
	}

	[CompilerGenerated]
	private void _E005(bool x)
	{
		this.m__E023.InvertX = x;
	}

	[CompilerGenerated]
	private void _E006(bool y)
	{
		this.m__E023.InvertY = y;
	}
}
