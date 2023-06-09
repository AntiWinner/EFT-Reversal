using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class PopperTargetControl : InteractiveShootingRange
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public HideoutPlayerOwner owner;

		public PopperTargetControl _003C_003E4__this;

		internal void _E000()
		{
			Singleton<_E815>.Instance?.ActivateWeapon(owner);
			_003C_003E4__this._E007();
		}

		internal void _E001()
		{
			Singleton<_E815>.Instance?.ActivateWeapon(owner);
			_003C_003E4__this._E009();
		}
	}

	[SerializeField]
	[Space]
	private RailTargets _railTargets;

	[SerializeField]
	private PopperTargets _popperTargets;

	[SerializeField]
	[Space]
	private SharedTargetControl _sharedTargetControl;

	[SerializeField]
	private _E843.Setting _popperTargetStandSettings;

	[SerializeField]
	private _E84B.Setting _shootTrainingSettings;

	[Space]
	[SerializeField]
	private ShootingScoreInterface _shootingScoreInterface;

	[SerializeField]
	private AudioSource _clickAudio;

	private _E843 m__E002;

	private _E84B m__E003;

	private _E842 m__E004;

	private _E844 m__E005;

	private _EC3F m__E006;

	private bool m__E007;

	private bool m__E008;

	private int m__E009;

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
		if (this.m__E003.Disabled)
		{
			obj.Actions.Add(new _EC3E
			{
				Name = _ED3E._E000(164689),
				Action = delegate
				{
					Singleton<_E815>.Instance?.ActivateWeapon(owner);
					_E007();
				}
			});
		}
		else
		{
			obj.Actions.Add(new _EC3E
			{
				Name = _ED3E._E000(164730),
				Action = _E008
			});
		}
		if (this.m__E002.Disabled)
		{
			obj.Actions.Add(new _EC3E
			{
				Name = _ED3E._E000(164764),
				Action = delegate
				{
					Singleton<_E815>.Instance?.ActivateWeapon(owner);
					_E009();
				}
			});
		}
		else
		{
			obj.Actions.Add(new _EC3E
			{
				Name = _ED3E._E000(164740),
				Action = _E00B
			});
		}
		if (this.m__E008)
		{
			obj.Actions.Add(new _EC3E
			{
				Name = _ED3E._E000(164781),
				Action = _E00D
			});
		}
		else
		{
			obj.Actions.Add(new _EC3E
			{
				Name = _ED3E._E000(164820),
				Action = _E00C
			});
		}
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
		this.m__E002 = new _E843(_popperTargets, _popperTargetStandSettings);
		this.m__E003 = new _E84B(_railTargets, _popperTargets, _shootTrainingSettings);
		this.m__E004 = new _E842(_popperTargets);
		this.m__E005 = new _E844(_popperTargets);
		this.m__E002.OnStart(_E000);
		this.m__E002.OnComplete(_E001);
		this.m__E002.OnCancel(_E001);
		this.m__E003.OnStart(_E002);
		this.m__E003.OnComplete(_E004);
		this.m__E003.OnCancel(_E003);
		this.m__E004.OnStart(_E005);
		this.m__E005.OnStart(_E000);
		this.m__E009 = Singleton<_E815>.Instance?.GetShootingRangeScore() ?? 0;
		_E00A();
	}

	private void _E000()
	{
		this.m__E008 = false;
		SetStateUpdateTime();
	}

	private void _E001()
	{
		SetStateUpdateTime();
	}

	private void _E002()
	{
		float duration = _shootTrainingSettings.Phases.Sum((_E84B.PhaseSetting v) => v.Duration);
		_shootingScoreInterface.StartCountdown(_shootTrainingSettings.Delay, duration);
		_E000();
	}

	private void _E003()
	{
		_shootingScoreInterface.StopCountdown();
		_E006();
		_E001();
	}

	private void _E004()
	{
		_shootingScoreInterface.StopCountdown();
		_E006();
		_E001();
	}

	private void _E005()
	{
		this.m__E008 = true;
		SetStateUpdateTime();
	}

	private void _E006()
	{
		if (this.m__E009 < this.m__E003.Score())
		{
			this.m__E009 = this.m__E003.Score();
			Singleton<_E815>.Instance.SetShootingRangeScore(this.m__E009);
		}
	}

	private void Update()
	{
		_shootingScoreInterface.SetBestScore(this.m__E009);
		_shootingScoreInterface.SetCurrentScore(this.m__E003.Score());
	}

	private void _E007()
	{
		if (_enabled)
		{
			_clickAudio.Play();
			_sharedTargetControl.Run(this.m__E003);
		}
	}

	private void _E008()
	{
		_clickAudio.Play();
		_sharedTargetControl.Complete();
	}

	private void _E009()
	{
		if (_enabled)
		{
			_clickAudio.Play();
			_sharedTargetControl.Run(this.m__E002);
		}
	}

	private void _E00A()
	{
		_sharedTargetControl.Run(this.m__E002);
	}

	private void _E00B()
	{
		_clickAudio.Play();
		_sharedTargetControl.Complete();
	}

	private void _E00C()
	{
		if (_enabled)
		{
			_clickAudio.Play();
			_sharedTargetControl.Run(this.m__E004);
		}
	}

	private void _E00D()
	{
		if (_enabled)
		{
			_clickAudio.Play();
			_sharedTargetControl.Run(this.m__E005);
		}
	}

	private void OnDestroy()
	{
		this.m__E003.Destroy();
		this.m__E002.Destroy();
	}
}
