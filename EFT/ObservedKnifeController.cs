using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT;

internal sealed class ObservedKnifeController : Player.KnifeController, ObservedPlayer._E004
{
	private new sealed class _E000 : Player.KnifeController._E001
	{
		private readonly ObservedKnifeController _E00D;

		public _E000(Player.KnifeController controller)
			: base(controller)
		{
			_E00D = controller as ObservedKnifeController;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			Queue<_E959> queue = _E00D._E04E;
			if (queue.Count == 0)
			{
				return;
			}
			_E959 obj = queue.Dequeue();
			if (obj.ExamineWeapon)
			{
				ExamineWeapon();
			}
			if (obj.MakeKnifeKick)
			{
				if (obj.AlternativeKick)
				{
					MakeAlternativeKick();
				}
				else
				{
					MakeKnifeKick();
				}
			}
			if (obj.BrakeCombo)
			{
				Debug.Log(_ED3E._E000(188592).Red());
				BrakeCombo();
			}
			_E00D.ApplyCompassPacket(obj.CompassPacket);
			if (obj.Gesture != 0)
			{
				_E005._E073(obj.Gesture);
			}
			if (obj.EnableInventoryPacket.EnableInventory)
			{
				SetInventoryOpened(obj.EnableInventoryPacket.InventoryStatus);
			}
		}

		public override bool MakeKnifeKick()
		{
			State = Player.EOperationState.Finished;
			_E002.InitiateOperation<_E002>().Start(Player.EKickType.Slash);
			return true;
		}

		public override bool MakeAlternativeKick()
		{
			State = Player.EOperationState.Finished;
			_E002.InitiateOperation<_E002>().Start(Player.EKickType.Stab);
			return true;
		}
	}

	private new sealed class _E001 : _E002
	{
		private readonly ObservedKnifeController _E00D;

		public _E001(Player.KnifeController controller)
			: base(controller)
		{
			_E00D = controller as ObservedKnifeController;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			Queue<_E959> queue = _E00D._E04E;
			if (queue.Count != 0 && queue.Dequeue().BrakeCombo)
			{
				Debug.Log(_ED3E._E000(188625).Green());
				BrakeCombo();
			}
		}
	}

	private readonly Queue<_E959> _E04E = new Queue<_E959>(100);

	protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
	{
		Dictionary<Type, OperationFactoryDelegate> operationFactoryDelegates = base.GetOperationFactoryDelegates();
		operationFactoryDelegates[typeof(Player.KnifeController._E001)] = () => new _E000(this);
		operationFactoryDelegates[typeof(_E002)] = () => new _E001(this);
		return operationFactoryDelegates;
	}

	internal new static ObservedKnifeController _E000(ObservedPlayer player, KnifeComponent knife)
	{
		return Player.KnifeController._E000<ObservedKnifeController>(player, knife);
	}

	internal new static Task<ObservedKnifeController> _E001(ObservedPlayer player, KnifeComponent knife)
	{
		return Player.KnifeController._E001<ObservedKnifeController>(player, knife);
	}

	bool ObservedPlayer._E004.IsInIdleState()
	{
		return base._E000 is _E000;
	}

	void ObservedPlayer._E004.ProcessPlayerPacket(_E733 framePlayerInfo)
	{
		_E959 knifePacket = framePlayerInfo.KnifePacket;
		_E04E.Enqueue(knifePacket);
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

	[CompilerGenerated]
	private Player._E00E _E002()
	{
		return new _E000(this);
	}

	[CompilerGenerated]
	private Player._E00E _E003()
	{
		return new _E001(this);
	}
}
