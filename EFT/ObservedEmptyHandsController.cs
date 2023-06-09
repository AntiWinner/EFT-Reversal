using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EFT;

internal sealed class ObservedEmptyHandsController : Player.EmptyHandsController, ObservedPlayer._E004
{
	private new class _E000 : _E002
	{
		private readonly ObservedEmptyHandsController _E082;

		public _E000(Player.EmptyHandsController controller)
			: base(controller)
		{
			_E082 = controller as ObservedEmptyHandsController;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			Queue<_E94E> queue = _E082._E04E;
			if (queue.Count != 0)
			{
				_E94E obj = queue.Dequeue();
				_E082.ApplyCompassPacket(obj.CompassPacket);
				if (obj.Gesture != 0)
				{
					_E001._E073(obj.Gesture);
				}
			}
		}
	}

	private readonly Queue<_E94E> _E04E = new Queue<_E94E>(100);

	internal static ObservedEmptyHandsController _E000(ObservedPlayer player)
	{
		return Player.EmptyHandsController._E000<ObservedEmptyHandsController>(player);
	}

	internal static Task<ObservedEmptyHandsController> _E001(ObservedPlayer player)
	{
		return Player.EmptyHandsController._E001<ObservedEmptyHandsController>(player);
	}

	bool ObservedPlayer._E004.IsInIdleState()
	{
		return base._E003 is _E000;
	}

	void ObservedPlayer._E004.ProcessPlayerPacket(_E733 framePlayerInfo)
	{
		_E94E emptyHandPacket = framePlayerInfo.EmptyHandPacket;
		_E04E.Enqueue(emptyHandPacket);
	}

	protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
	{
		Dictionary<Type, OperationFactoryDelegate> operationFactoryDelegates = base.GetOperationFactoryDelegates();
		operationFactoryDelegates[typeof(_E002)] = () => new _E000(this);
		return operationFactoryDelegates;
	}

	protected override bool CanChangeCompassState(bool newState)
	{
		return false;
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
}
