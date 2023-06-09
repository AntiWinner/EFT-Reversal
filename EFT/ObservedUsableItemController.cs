using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.InventoryLogic;

namespace EFT;

internal sealed class ObservedUsableItemController : Player.UsableItemController, ObservedPlayer._E004
{
	private new sealed class _E000 : _E003
	{
		private readonly ObservedUsableItemController _E08C;

		public _E000(Player.UsableItemController controller)
			: base(controller)
		{
			_E08C = controller as ObservedUsableItemController;
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
					_E002.Hide();
				}
				if (obj.Gesture != 0)
				{
					_E001._E073(obj.Gesture);
				}
			}
		}
	}

	private readonly Queue<_E967> _E04E = new Queue<_E967>(100);

	internal new static ObservedUsableItemController _E000(ObservedPlayer player, Item knife)
	{
		return Player.UsableItemController._E000<ObservedUsableItemController>(player, knife);
	}

	internal new static Task<ObservedUsableItemController> _E001(ObservedPlayer player, Item item)
	{
		return Player.UsableItemController._E001<ObservedUsableItemController>(player, item);
	}

	bool ObservedPlayer._E004.IsInIdleState()
	{
		return base._E000 is _E000;
	}

	void ObservedPlayer._E004.ProcessPlayerPacket(_E733 framePlayerInfo)
	{
		_E967 usableItemPacket = framePlayerInfo.UsableItemPacket;
		_E04E.Enqueue(usableItemPacket);
		if (usableItemPacket.Gesture != 0)
		{
			ShowGesture(usableItemPacket.Gesture);
		}
	}

	protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
	{
		Dictionary<Type, OperationFactoryDelegate> operationFactoryDelegates = base.GetOperationFactoryDelegates();
		operationFactoryDelegates[typeof(_E003)] = () => new _E000(this);
		return operationFactoryDelegates;
	}

	[CompilerGenerated]
	private Player._E00E _E002()
	{
		return new _E000(this);
	}
}
