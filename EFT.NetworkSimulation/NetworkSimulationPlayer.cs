using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.CameraControl;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.NetworkSimulation;

internal class NetworkSimulationPlayer : ClientPlayer
{
	[CompilerGenerated]
	private new sealed class _E001
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public bool withNetwork;

		public Process<EmptyHandsController, _E6C9> process;

		public Action confirmCallback;

		internal EmptyHandsController _E000()
		{
			return ClientEmptyHandsController._E000(_003C_003E4__this);
		}

		internal void _E001()
		{
			if (withNetwork)
			{
				uint callbackId = _003C_003E4__this._E002(process._E001);
				_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateEmptyHands;
				_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			}
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E002
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public Weapon weapon;

		public Process<FirearmController, _E6CB> process;

		public Action confirmCallback;

		internal FirearmController _E000()
		{
			return ClientFirearmController._E000(_003C_003E4__this, weapon);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E002(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateFirearm;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = weapon.Id;
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E003
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public _EADF throwWeap;

		public Process<GrenadeController, _E6CC> process;

		public Action confirmCallback;

		internal GrenadeController _E000()
		{
			return ClientGrenadeController._E000(_003C_003E4__this, throwWeap);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E002(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateGrenade;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = throwWeap.Id;
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E004
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public _EA72 meds;

		public EBodyPart bodyPart;

		public int animationVariant;

		public Process<MedsController, _E6CF> process;

		public Action confirmCallback;

		internal MedsController _E000()
		{
			return ClientMedsController._E000(_003C_003E4__this, meds, bodyPart, 1f, animationVariant);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E002(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateMeds;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = meds.Id;
			_003C_003E4__this._handsChangePacket.MedsBodyPart = bodyPart;
			_003C_003E4__this._handsChangePacket.MedsAmount = 1f;
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E005
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public _EA48 foodDrink;

		public float amount;

		public int animationVariant;

		public Process<MedsController, _E6CF> process;

		public Action confirmCallback;

		internal MedsController _E000()
		{
			return ClientMedsController._E000(_003C_003E4__this, foodDrink, EBodyPart.Head, amount, animationVariant);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E002(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateMeds;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = foodDrink.Id;
			_003C_003E4__this._handsChangePacket.MedsBodyPart = EBodyPart.Head;
			_003C_003E4__this._handsChangePacket.MedsAmount = amount;
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E006
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public KnifeComponent knife;

		public Process<KnifeController, _E6CD> process;

		public Action confirmCallback;

		internal KnifeController _E000()
		{
			return ClientKnifeController._E000(_003C_003E4__this, knife);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E002(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateKnife;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = knife.Item.Id;
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E007<_E077> where _E077 : UsableItemController
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public Item item;

		public Process<_E077, _E6CE> process;

		public Action confirmCallback;

		internal _E077 _E000()
		{
			return UsableItemController._E000<_E077>(_003C_003E4__this, item);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E002(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateUsableItem;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = item.Id;
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E008
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public _EADF throwWeap;

		public Process<QuickGrenadeThrowController, _E6D2> process;

		public Action confirmCallback;

		internal QuickGrenadeThrowController _E000()
		{
			return ClientQuickGrenadeThrowController._E000(_003C_003E4__this, throwWeap);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E002(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickGrenadeThrow;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = throwWeap.Id;
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	[CompilerGenerated]
	private new sealed class _E009
	{
		public NetworkSimulationPlayer _003C_003E4__this;

		public KnifeComponent knife;

		public Process<QuickKnifeKickController, _E6D3> process;

		public Action confirmCallback;

		internal QuickKnifeKickController _E000()
		{
			return ClientQuickKnifeKickController._E000(_003C_003E4__this, knife);
		}

		internal void _E001()
		{
			uint callbackId = _003C_003E4__this._E002(process._E001);
			_003C_003E4__this._handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickKnifeKick;
			_003C_003E4__this._handsChangePacket.CallbackId = callbackId;
			_003C_003E4__this._handsChangePacket.ItemId = knife.Item.Id;
		}

		internal void _E002(IResult result)
		{
			NetworkSimulationPlayer._E001(result, confirmCallback);
		}
	}

	internal static async Task<NetworkSimulationPlayer> _E000(Profile profile, Vector3 position, _E62D frameIndexer, ClientPlayer._E000 dataSenderFactoryDelegate, EUpdateQueue updateQueue, EUpdateMode armsUpdateMode, EUpdateMode bodyUpdateMode)
	{
		NetworkSimulationPlayer networkSimulationPlayer = NetworkPlayer._E000<NetworkSimulationPlayer>(_E5D2.PLAYER_BUNDLE_NAME, 0, position, "", frameIndexer, updateQueue, armsUpdateMode, bodyUpdateMode, _E2B6.Config.CharacterController.ClientPlayerMode, () => 1f, () => 1f, isThirdPerson: false);
		networkSimulationPlayer._dataSender = dataSenderFactoryDelegate;
		networkSimulationPlayer._fbbik.solver.Quick = true;
		ClientPlayer._E002 inventoryController = new ClientPlayer._E002(networkSimulationPlayer, profile, default(MongoID));
		await networkSimulationPlayer.Init(Quaternion.identity, _ED3E._E000(60679), EPointOfView.FirstPerson, profile, inventoryController, new _E9CA(profile.Health, networkSimulationPlayer, inventoryController, profile.Skills, aiHealth: false), new _E757(), new _E942(profile, inventoryController, null), _E610.Default, EVoipState.NotAvailable);
		PlayerCameraController.Create(networkSimulationPlayer);
		networkSimulationPlayer._animators[0].enabled = bodyUpdateMode == EUpdateMode.Auto;
		networkSimulationPlayer._handsController = EmptyHandsController._E000<EmptyHandsController>(networkSimulationPlayer);
		networkSimulationPlayer._handsController.Spawn(1f, delegate
		{
		});
		return networkSimulationPlayer;
	}

	public override void Interact(IItemOwner loot, Callback callback)
	{
		string id = loot.RootItem.Id;
		callback.Succeed();
		_lootInteractionPacket.Interact = true;
		_lootInteractionPacket.LootId = id;
		_lootInteractionPacket.CallbackId = 0u;
	}

	protected override void DropCurrentController(Action callback, bool fastDrop, Item nextControllerItem = null)
	{
		_handsChangePacket.OperationType = ((!fastDrop) ? _E6E5.EOperationType.Drop : _E6E5.EOperationType.FastDrop);
		base.DropCurrentController(callback, fastDrop, nextControllerItem);
	}

	protected override void Proceed(bool withNetwork, Callback<_E6C9> callback, bool scheduled = true)
	{
		Func<EmptyHandsController> controllerFactory = () => ClientEmptyHandsController._E000(this);
		Process<EmptyHandsController, _E6C9> process = new Process<EmptyHandsController, _E6C9>(this, controllerFactory, null);
		Action confirmCallback = delegate
		{
			if (withNetwork)
			{
				uint callbackId = _E002(process._E001);
				_handsChangePacket.OperationType = _E6E5.EOperationType.CreateEmptyHands;
				_handsChangePacket.CallbackId = callbackId;
			}
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(Weapon weapon, Callback<_E6CB> callback, bool scheduled = true)
	{
		Func<FirearmController> controllerFactory = () => ClientFirearmController._E000(this, weapon);
		Process<FirearmController, _E6CB> process = new Process<FirearmController, _E6CB>(this, controllerFactory, weapon);
		Action confirmCallback = delegate
		{
			uint callbackId = _E002(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateFirearm;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = weapon.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(_EADF throwWeap, Callback<_E6CC> callback, bool scheduled = true)
	{
		Func<GrenadeController> controllerFactory = () => ClientGrenadeController._E000(this, throwWeap);
		Process<GrenadeController, _E6CC> process = new Process<GrenadeController, _E6CC>(this, controllerFactory, throwWeap);
		Action confirmCallback = delegate
		{
			uint callbackId = _E002(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateGrenade;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = throwWeap.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(_EA72 meds, EBodyPart bodyPart, Callback<_E6CF> callback, int animationVariant, bool scheduled = true)
	{
		Func<MedsController> controllerFactory = () => ClientMedsController._E000(this, meds, bodyPart, 1f, animationVariant);
		Process<MedsController, _E6CF> process = new Process<MedsController, _E6CF>(this, controllerFactory, meds);
		Action confirmCallback = delegate
		{
			uint callbackId = _E002(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateMeds;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = meds.Id;
			_handsChangePacket.MedsBodyPart = bodyPart;
			_handsChangePacket.MedsAmount = 1f;
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(_EA48 foodDrink, float amount, Callback<_E6CF> callback, int animationVariant, bool scheduled = true)
	{
		Func<MedsController> controllerFactory = () => ClientMedsController._E000(this, foodDrink, EBodyPart.Head, amount, animationVariant);
		Process<MedsController, _E6CF> process = new Process<MedsController, _E6CF>(this, controllerFactory, foodDrink);
		Action confirmCallback = delegate
		{
			uint callbackId = _E002(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateMeds;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = foodDrink.Id;
			_handsChangePacket.MedsBodyPart = EBodyPart.Head;
			_handsChangePacket.MedsAmount = amount;
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(KnifeComponent knife, Callback<_E6CD> callback, bool scheduled = true)
	{
		Func<KnifeController> controllerFactory = () => ClientKnifeController._E000(this, knife);
		Process<KnifeController, _E6CD> process = new Process<KnifeController, _E6CD>(this, controllerFactory, knife.Item);
		Action confirmCallback = delegate
		{
			uint callbackId = _E002(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateKnife;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = knife.Item.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	internal override void Proceed<T>(Item item, Callback<_E6CE> callback, bool scheduled = true)
	{
		Func<T> controllerFactory = () => UsableItemController._E000<T>(this, item);
		Process<T, _E6CE> process = new Process<T, _E6CE>(this, controllerFactory, item);
		Action confirmCallback = delegate
		{
			uint callbackId = _E002(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateUsableItem;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = item.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(_EADF throwWeap, Callback<_E6D2> callback, bool scheduled = true)
	{
		Func<QuickGrenadeThrowController> controllerFactory = () => ClientQuickGrenadeThrowController._E000(this, throwWeap);
		Process<QuickGrenadeThrowController, _E6D2> process = new Process<QuickGrenadeThrowController, _E6D2>(this, controllerFactory, throwWeap, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, skippable: false);
		Action confirmCallback = delegate
		{
			uint callbackId = _E002(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickGrenadeThrow;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = throwWeap.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	protected override void Proceed(KnifeComponent knife, Callback<_E6D3> callback, bool scheduled = true)
	{
		Func<QuickKnifeKickController> controllerFactory = () => ClientQuickKnifeKickController._E000(this, knife);
		Process<QuickKnifeKickController, _E6D3> process = new Process<QuickKnifeKickController, _E6D3>(this, controllerFactory, knife.Item, fastHide: false, AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, skippable: false);
		Action confirmCallback = delegate
		{
			uint callbackId = _E002(process._E001);
			_handsChangePacket.OperationType = _E6E5.EOperationType.CreateQuickKnifeKick;
			_handsChangePacket.CallbackId = callbackId;
			_handsChangePacket.ItemId = knife.Item.Id;
		};
		process._E000(delegate(IResult result)
		{
			_E001(result, confirmCallback);
		}, callback, scheduled);
	}

	private static void _E001(IResult result, Action confirmAction)
	{
		if (result.Succeed)
		{
			confirmAction();
		}
	}

	private uint _E002(Action<bool> _)
	{
		return 0u;
	}
}
