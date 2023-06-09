using System;
using Comfort.Common;
using EFT.ClientItems.ClientSpecItems;

namespace EFT;

internal class RadioTransmitterController : Player.UsableItemController
{
	protected new class _E000 : Player.UsableItemController._E001
	{
		public _E000(RadioTransmitterController controller)
			: base(controller)
		{
		}

		public override void SetAiming(bool isAiming)
		{
		}

		protected override void _E006()
		{
			_E002.InitiateOperation<_E002>().Start();
		}
	}

	public new sealed class _E001 : Player.UsableItemController._E002
	{
		public _E001(RadioTransmitterController controller)
			: base(controller)
		{
		}
	}

	internal new class _E002 : Player.UsableItemController._E003
	{
		public _E002(RadioTransmitterController controller)
			: base(controller)
		{
		}

		public override void SetAiming(bool isAiming)
		{
		}

		public override void HideWeapon(Action onHidden, bool fastDrop)
		{
			State = Player.EOperationState.Finished;
			_E002.InitiateOperation<_E003>().Start(onHidden, fastDrop);
		}

		protected override void _E007(_EB75 oneItemOperation, Callback callback)
		{
			_E002.InitiateOperation<_E000>().Start(oneItemOperation.Item1, callback);
		}
	}

	protected new class _E003 : Player.UsableItemController._E004
	{
		public _E003(RadioTransmitterController controller)
			: base(controller)
		{
		}
	}

	protected new class _E004 : _E005
	{
		public _E004(RadioTransmitterController controller)
			: base(controller)
		{
		}

		protected override void _E009()
		{
			_E002 obj = _E002.InitiateOperation<_E002>();
			obj.Start();
			_E00B();
			if (base._E009 != null)
			{
				obj.HideWeapon(base._E009, _E03E);
			}
		}
	}

	private RadioTransmitterView _E04D;

	protected override void _E013(Player player, WeaponPrefab weaponPrefab)
	{
		base._E013(player, weaponPrefab);
		if (_E04D == null)
		{
			_E04D = weaponPrefab.GetComponentInChildren<RadioTransmitterView>();
			_E04D.Initialiaze(player);
		}
		_E009();
		_E036.AfterGetFromPoolInit(player.ProceduralWeaponAnimation, null, player.IsYourPlayer);
		BaseSoundPlayer component = _controllerObject.GetComponent<BaseSoundPlayer>();
		if (component != null)
		{
			component.Init(this, player.PlayerBones.WeaponRoot, player);
		}
	}

	protected override void _E015(EPlayerState previousstate, EPlayerState nextstate)
	{
	}

	public override void SetAim(bool value)
	{
	}
}
