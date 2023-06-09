using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.CameraControl;

namespace EFT;

internal class PortableRangeFinderController : Player.UsableItemController
{
	internal new class _E000 : Player.UsableItemController._E001
	{
		public _E000(PortableRangeFinderController controller)
			: base(controller)
		{
		}

		public override void SetAiming(bool isAiming)
		{
			base.SetAiming(isAiming);
			if (_E001.PointOfView == EPointOfView.FirstPerson)
			{
				PortableRangeFinderController obj = _E002 as PortableRangeFinderController;
				obj._E04B.enabled = isAiming;
				obj._E04C.gameObject.SetActive(isAiming);
			}
		}

		protected override void _E006()
		{
			_E002.InitiateOperation<_E002>().Start();
		}
	}

	public new sealed class _E001 : Player.UsableItemController._E002
	{
		public _E001(PortableRangeFinderController controller)
			: base(controller)
		{
		}
	}

	internal new class _E002 : Player.UsableItemController._E003
	{
		public _E002(PortableRangeFinderController controller)
			: base(controller)
		{
		}

		public override void SetAiming(bool isAiming)
		{
			base.SetAiming(isAiming);
			if (_E001.PointOfView == EPointOfView.FirstPerson)
			{
				PortableRangeFinderController obj = _E002 as PortableRangeFinderController;
				obj._E04B.enabled = isAiming;
				obj._E04C.gameObject.SetActive(isAiming);
			}
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

	internal new class _E003 : Player.UsableItemController._E004
	{
		public _E003(PortableRangeFinderController controller)
			: base(controller)
		{
		}

		public override void Start(Action onHidden, bool fastDrop)
		{
			base.Start(onHidden, fastDrop);
			PortableRangeFinderController obj = _E002 as PortableRangeFinderController;
			obj._E04B.enabled = false;
			obj._E04C.gameObject.SetActive(value: false);
		}
	}

	internal new class _E004 : _E005
	{
		public _E004(PortableRangeFinderController controller)
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

	private const float _E04A = 2f;

	private OpticSight _E04B;

	private TacticalRangeFinderController _E04C;

	protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
	{
		return new Dictionary<Type, OperationFactoryDelegate>
		{
			{
				typeof(_E004),
				() => new _E004(this)
			},
			{
				typeof(_E002),
				() => new _E002(this)
			},
			{
				typeof(_E003),
				() => new _E003(this)
			},
			{
				typeof(_E000),
				() => new _E000(this)
			}
		};
	}

	protected override void _E013(Player player, WeaponPrefab weaponPrefab)
	{
		base._E013(player, weaponPrefab);
		_E04B = weaponPrefab.GetComponentInChildren<OpticSight>();
		_E04C = _E04B.GetComponentInChildren<TacticalRangeFinderController>(includeInactive: true);
		player.ProceduralWeaponAnimation.ManualSetVariables(2f, 0f, 0f, 0f);
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

	protected override void _E014(Action callback)
	{
		InitiateOperation<_E004>().Start(callback);
	}

	[CompilerGenerated]
	private new Player._E00E _E000()
	{
		return new _E004(this);
	}

	[CompilerGenerated]
	private new Player._E00E _E001()
	{
		return new _E002(this);
	}

	[CompilerGenerated]
	private Player._E00E _E002()
	{
		return new _E003(this);
	}

	[CompilerGenerated]
	private Player._E00E _E003()
	{
		return new _E000(this);
	}
}
