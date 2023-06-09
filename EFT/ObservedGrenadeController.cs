using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT;

internal sealed class ObservedGrenadeController : Player.GrenadeController, ObservedPlayer._E004
{
	private new class _E000 : Player.GrenadeController._E002
	{
		private readonly ObservedGrenadeController _E007;

		public _E000(Player.GrenadeController controller)
			: base(controller)
		{
			_E007 = controller as ObservedGrenadeController;
		}

		public override void Update(float deltaTime)
		{
			ObservedGrenadeController._E004.ProcessPackets(_E007, this);
			base.Update(deltaTime);
		}

		public override void FastForward()
		{
		}
	}

	private new class _E001 : Player.GrenadeController._E003
	{
		private readonly ObservedGrenadeController _E007;

		public _E001(Player.GrenadeController controller)
			: base(controller)
		{
			_E007 = controller as ObservedGrenadeController;
		}

		public override void Update(float deltaTime)
		{
			Queue<_E953> queue = _E007._E04E;
			if (queue.Count != 0)
			{
				_E953 obj = queue.Dequeue();
				if (obj.ExamineWeapon)
				{
					ExamineWeapon();
				}
				if (obj.PullRingForHighThrow)
				{
					PullRingForHighThrow();
				}
				if (obj.PullRingForLowThrow)
				{
					PullRingForLowThrow();
				}
				if (obj.EnableInventoryPacket.EnableInventory)
				{
					SetInventoryOpened(obj.EnableInventoryPacket.InventoryStatus);
				}
				_E002.ApplyCompassPacket(obj.CompassPacket);
				if (obj.LowThrow)
				{
					_E002._E000.LowThrow();
				}
				if (obj.HighThrow)
				{
					_E002._E000.HighThrow();
				}
				if (obj.HideGrenade)
				{
					_E002._E000.HideGrenade(delegate
					{
					}, fastHide: false);
				}
				if (obj.DiscardThrow)
				{
					_E002._E000.PutGrenadeBack();
				}
			}
			base.Update(deltaTime);
		}
	}

	private new class _E002 : Player.GrenadeController._E004
	{
		private readonly ObservedGrenadeController _E007;

		public _E002(Player.GrenadeController controller)
			: base(controller)
		{
			_E007 = controller as ObservedGrenadeController;
		}

		public override void Update(float deltaTime)
		{
			ObservedGrenadeController._E004.ProcessPackets(_E007, this);
			base.Update(deltaTime);
		}

		public override void FastForward()
		{
		}
	}

	private new class _E003 : _E005
	{
		public override bool CanRemove()
		{
			return true;
		}

		public _E003(Player.GrenadeController controller)
			: base(controller)
		{
		}
	}

	private new class _E004
	{
		public static void ProcessPackets(ObservedGrenadeController controller, Player.GrenadeController._E001 operation)
		{
			Queue<_E953> queue = controller._E04E;
			if (queue.Count != 0)
			{
				_E953 obj = queue.Dequeue();
				if (obj.LowThrow)
				{
					operation.LowThrow();
				}
				if (obj.HighThrow)
				{
					operation.HighThrow();
				}
				if (obj.HideGrenade)
				{
					operation.HideGrenade(delegate
					{
					}, fastHide: false);
				}
				if (obj.EnableInventoryPacket.EnableInventory)
				{
					operation.SetInventoryOpened(obj.EnableInventoryPacket.InventoryStatus);
				}
				if (obj.DiscardThrow)
				{
					controller._E008();
				}
			}
		}
	}

	private readonly Queue<_E953> _E04E = new Queue<_E953>(100);

	internal new static ObservedGrenadeController _E000(ObservedPlayer player, _EADF item)
	{
		return Player.GrenadeController._E000<ObservedGrenadeController>(player, item);
	}

	internal static Task<ObservedGrenadeController> _E001(ObservedPlayer player, _EADF item)
	{
		return Player.GrenadeController._E001<ObservedGrenadeController>(player, item);
	}

	protected override void _E00F(float timeSinceSafetyLevelRemoved, Vector3 position, Quaternion rotation, Vector3 force, bool lowThrow)
	{
		base._E00F(timeSinceSafetyLevelRemoved, position, rotation, force, lowThrow);
	}

	protected override bool CanChangeCompassState(bool newState)
	{
		return false;
	}

	public override bool CanRemove()
	{
		return true;
	}

	protected override void OnCanUsePropChanged(bool canUse)
	{
	}

	public override void SetCompassState(bool active)
	{
	}

	internal static ObservedGrenadeController _E002(ObservedPlayer player, string itemId)
	{
		_EADF obj = (string.IsNullOrEmpty(itemId) ? null : (player._E0DE.Inventory.Equipment.FindItem(itemId) as _EADF));
		if (obj != null)
		{
			return _E000(player, obj);
		}
		throw new Exception(_ED3E._E000(192615));
	}

	bool ObservedPlayer._E004.IsInIdleState()
	{
		return base._E000 is _E001;
	}

	void ObservedPlayer._E004.ProcessPlayerPacket(_E733 framePlayerInfo)
	{
		_E953 grenadePacket = framePlayerInfo.GrenadePacket;
		_E04E.Enqueue(grenadePacket);
		_E954 grenadeThrowData = framePlayerInfo.GrenadePacket.GrenadeThrowData;
		if (grenadeThrowData.HasThrowData)
		{
			base._E00F(0f, grenadeThrowData.ThrowGreanadePosition, grenadeThrowData.ThrowGrenadeRotation, grenadeThrowData.ThrowForce, grenadeThrowData.LowThrow);
		}
		if (framePlayerInfo.GrenadePacket.Gesture != 0)
		{
			ShowGesture(framePlayerInfo.GrenadePacket.Gesture);
		}
	}

	protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
	{
		Dictionary<Type, OperationFactoryDelegate> operationFactoryDelegates = base.GetOperationFactoryDelegates();
		operationFactoryDelegates[typeof(Player.GrenadeController._E003)] = () => new _E001(this);
		operationFactoryDelegates[typeof(_E005)] = () => new _E003(this);
		operationFactoryDelegates[typeof(Player.GrenadeController._E002)] = () => new _E000(this);
		operationFactoryDelegates[typeof(Player.GrenadeController._E004)] = () => new _E002(this);
		return operationFactoryDelegates;
	}

	[CompilerGenerated]
	private new Player._E00E _E003()
	{
		return new _E001(this);
	}

	[CompilerGenerated]
	private new Player._E00E _E004()
	{
		return new _E003(this);
	}

	[CompilerGenerated]
	private new Player._E00E _E005()
	{
		return new _E000(this);
	}

	[CompilerGenerated]
	private new Player._E00E _E006()
	{
		return new _E002(this);
	}
}
