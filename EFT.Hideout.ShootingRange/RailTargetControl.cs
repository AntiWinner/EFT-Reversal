using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class RailTargetControl : InteractiveShootingRange
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public HideoutPlayerOwner owner;

		public RailTargetControl _003C_003E4__this;

		internal void _E000()
		{
			Singleton<_E815>.Instance?.ActivateWeapon(owner);
			_003C_003E4__this._E000();
		}
	}

	[Space]
	[SerializeField]
	private RailTargets _targets;

	[SerializeField]
	private Transform _switch;

	[Space]
	[SerializeField]
	private SharedTargetControl _sharedTargetControl;

	[SerializeField]
	[Space]
	private AudioSource _clickAudio;

	[SerializeField]
	private AudioSource _switchAudio;

	private _EC3F m__E006;

	private const int _E00A = 2;

	private int _E00B;

	private bool _E00C;

	private bool m__E007;

	private _E849 _E00D;

	private _E84A _E00E;

	private _E848 _E00F;

	private _E847 _E010;

	private Vector3[] _E011;

	public override void Disable()
	{
		base.Disable();
		_sharedTargetControl.Cancel();
	}

	public void TurnOnEnergy()
	{
		this.m__E007 = true;
	}

	public void TurnOffEnergy()
	{
		this.m__E007 = false;
		_sharedTargetControl.Cancel();
	}

	public override _EC3F InteractionStates(HideoutPlayerOwner owner)
	{
		_EC3F obj = base.InteractionStates(owner);
		if (!_enabled || !this.m__E007)
		{
			return obj;
		}
		obj.Actions.Add(new _EC3E
		{
			Name = string.Format(_ED3E._E000(164801).Localized(), (!_E00C) ? 1 : (_E005() + 1)),
			Action = delegate
			{
				Singleton<_E815>.Instance?.ActivateWeapon(owner);
				_E000();
			}
		});
		obj.Actions.Add(new _EC3E
		{
			Name = _ED3E._E000(164843),
			Action = _E001
		});
		obj.Actions.Add(new _EC3E
		{
			Name = _ED3E._E000(166936),
			Action = _E002
		});
		obj.Actions.Add(new _EC3E
		{
			Name = _ED3E._E000(166920),
			Action = _E003
		});
		if (this.m__E006 == null)
		{
			obj.InitSelected();
		}
		else
		{
			int index = this.m__E006.Actions.IndexOf(this.m__E006.SelectedAction);
			obj.SelectedAction = obj.Actions[index];
		}
		this.m__E006 = obj;
		return obj;
	}

	private void Start()
	{
		_E00D = new _E849(_targets);
		_E00E = new _E84A(_targets);
		_E00F = new _E848(_targets);
		_E010 = new _E847(_targets);
		_E011 = new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 30f),
			new Vector3(0f, 0f, 60f),
			new Vector3(0f, 0f, 90f)
		};
	}

	private async void _E000()
	{
		if (_enabled)
		{
			_switchAudio.Play();
			_E004();
			SetStateUpdateTime();
			_sharedTargetControl.Run(_E00D);
		}
	}

	private async void _E001()
	{
		if (_enabled)
		{
			_clickAudio.Play();
			_E006();
			SetStateUpdateTime();
			_sharedTargetControl.Run(_E00E);
		}
	}

	private async void _E002()
	{
		if (_enabled)
		{
			_clickAudio.Play();
			_E006();
			SetStateUpdateTime();
			_sharedTargetControl.Run(_E00F);
		}
	}

	private void _E003()
	{
		if (_enabled)
		{
			_clickAudio.Play();
			_E006();
			SetStateUpdateTime();
			_sharedTargetControl.Run(_E010);
		}
	}

	private void _E004()
	{
		if (_E00C)
		{
			_E00B = _E005();
			_E007();
		}
		_E00C = true;
		_E008();
	}

	private int _E005()
	{
		int num = _E00B + 1;
		if (num > 2)
		{
			return 0;
		}
		return num;
	}

	private void _E006()
	{
		_E00B = 0;
		_E00C = false;
		_E007();
		_E008();
	}

	private void _E007()
	{
		_targets.SetSpeed(_E00B);
	}

	private void _E008()
	{
		if (_E00C)
		{
			_switch.localRotation = Quaternion.Euler(_E011[_E00B + 1]);
		}
		else
		{
			_switch.localRotation = Quaternion.Euler(_E011[0]);
		}
	}
}
