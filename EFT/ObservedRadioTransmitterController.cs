using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.NetworkPackets;
using UnityEngine;

namespace EFT;

internal sealed class ObservedRadioTransmitterController : RadioTransmitterController, ObservedPlayer._E004
{
	internal new class _E000 : Player.UsableItemController._E001
	{
		public _E000(ObservedRadioTransmitterController controller)
			: base(controller)
		{
		}

		protected override void _E006()
		{
			_E002.InitiateOperation<_E001>().Start();
		}
	}

	private new sealed class _E001 : RadioTransmitterController._E002
	{
		private readonly ObservedRadioTransmitterController _E08C;

		public _E001(RadioTransmitterController controller)
			: base(controller)
		{
			_E08C = controller as ObservedRadioTransmitterController;
		}

		public override void HideWeapon(Action onHidden, bool fastDrop)
		{
			State = Player.EOperationState.Finished;
			_E002.InitiateOperation<_E002>().Start(onHidden, fastDrop);
		}

		protected override void _E007(_EB75 oneItemOperation, Callback callback)
		{
			_E002.InitiateOperation<_E000>().Start(oneItemOperation.Item1, callback);
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			Queue<_E967> queue = _E08C._E04E;
			if (queue.Count != 0)
			{
				_E967 obj = queue.Dequeue();
				if (obj.ExamineWeapon)
				{
					ExamineWeapon();
				}
				_E08C.ApplyCompassPacket(obj.CompassPacket);
				if (obj.EnableInventoryPacket.EnableInventory)
				{
					SetInventoryOpened(obj.EnableInventoryPacket.InventoryStatus);
				}
				if (obj.ToggleAim)
				{
					_E002.SetAim(obj.IsAiming);
				}
				if (obj.HideItem)
				{
					HideWeapon(null, fastDrop: false);
				}
				if (obj.Gesture != 0)
				{
					_E001._E073(obj.Gesture);
				}
			}
		}
	}

	private new sealed class _E002 : RadioTransmitterController._E003
	{
		public _E002(ObservedRadioTransmitterController controller)
			: base(controller)
		{
		}
	}

	private new sealed class _E003 : _E004
	{
		public _E003(ObservedRadioTransmitterController controller)
			: base(controller)
		{
		}

		protected override void _E009()
		{
			_E001 obj = _E002.InitiateOperation<_E001>();
			obj.Start();
			_E00B();
			if (((_E005)this)._E009 != null)
			{
				obj.HideWeapon(((_E005)this)._E009, _E03E);
			}
		}
	}

	private readonly Queue<_E967> _E04E = new Queue<_E967>(100);

	public bool IsInIdleState()
	{
		return ((Player.UsableItemController)this)._E000 is _E001;
	}

	public void ProcessPlayerPacket(_E733 framePlayerInfo)
	{
		if (framePlayerInfo.HandsTypePacket != EHandsTypePacket.UsableItem)
		{
			Debug.LogError(_ED3E._E000(188658));
			return;
		}
		_E967 usableItemPacket = framePlayerInfo.UsableItemPacket;
		_E04E.Enqueue(usableItemPacket);
		if (usableItemPacket.Gesture != 0)
		{
			ShowGesture(usableItemPacket.Gesture);
		}
	}

	protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
	{
		return new Dictionary<Type, OperationFactoryDelegate>
		{
			{
				typeof(_E001),
				() => new _E001(this)
			},
			{
				typeof(_E003),
				() => new _E003(this)
			},
			{
				typeof(_E000),
				() => new _E000(this)
			},
			{
				typeof(_E002),
				() => new _E002(this)
			}
		};
	}

	protected override void _E014(Action callback)
	{
		InitiateOperation<_E003>().Start(callback);
	}

	[CompilerGenerated]
	private new Player._E00E _E000()
	{
		return new _E001(this);
	}

	[CompilerGenerated]
	private new Player._E00E _E001()
	{
		return new _E003(this);
	}

	[CompilerGenerated]
	private Player._E00E _E002()
	{
		return new _E000(this);
	}

	[CompilerGenerated]
	private Player._E00E _E003()
	{
		return new _E002(this);
	}
}
