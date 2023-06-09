using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using EFT.Hideout;
using UnityEngine;

namespace EFT.Interactive;

public sealed class GarlandSwitcher : InteractiveAmbianceObject<GarlandSwitcher.GarlandPattern>, _EBFD
{
	public enum ELightType
	{
		None = 0,
		Green = 1,
		Blue = 2,
		Red = 3,
		Any = 100
	}

	public enum EPatternType
	{
		Linear,
		Random,
		OnOff
	}

	private sealed class _E000
	{
		[CompilerGenerated]
		private sealed class _E000
		{
			public Action callback;

			internal void _E000()
			{
				callback();
			}
		}

		private Action m__E000;

		public void Run(float time, Action callback)
		{
			this.m__E000 = delegate
			{
				callback();
			};
			_E000(time).HandleExceptions();
		}

		public void Reset()
		{
			this.m__E000 = null;
		}

		private async Task _E000(float time)
		{
			Action action = this.m__E000;
			await TasksExtensions.Delay(time);
			if (action == this.m__E000)
			{
				this.m__E000?.Invoke();
			}
		}
	}

	[Serializable]
	public sealed class GarlandPattern
	{
		public bool Active;

		public float Speed = 0.5f;
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public GarlandSwitcher _003C_003E4__this;

		public int index;

		internal void _E000()
		{
			// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
			if (!(_003C_003E4__this == null))
			{
				if (index >= _003C_003E4__this._order.Count)
				{
					index = 0;
				}
				_003C_003E4__this._E002(_003C_003E4__this._order[index]);
				int num = index + 1;
				index = num;
				_003C_003E4__this.m__E005.Run(_003C_003E4__this.m__E002, _E000);
			}
		}
	}

	[SerializeField]
	private Dictionary<ELightType, List<Light>> _lights;

	[SerializeField]
	private Dictionary<ELightType, int> _materialIndexes;

	[SerializeField]
	private GameObject _garland;

	[SerializeField]
	private List<ELightType> _order;

	public float Intensity = 0.3f;

	public float EmissionPower = 3.01f;

	private Dictionary<ELightType, Material> m__E000;

	private bool m__E001 = true;

	private float m__E002 = 0.5f;

	private EPatternType m__E003;

	private Turnable.EState m__E004 = Turnable.EState.Off;

	private _E000 m__E005 = new _E000();

	public new bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			SetEnable(value);
			Switch(this.m__E004);
		}
	}

	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		this.m__E004 = ((status == ELightStatus.Working) ? Turnable.EState.On : Turnable.EState.Off);
		if (!(await base.PerformInteraction(status)) || !base.gameObject.activeInHierarchy || !Patterns.TryGetValue(status, out var value))
		{
			return false;
		}
		Switch(value.Value.Active ? Turnable.EState.On : Turnable.EState.Off);
		return true;
	}

	public void Switch(Turnable.EState switchTo)
	{
		this.m__E004 = switchTo;
		if (!Enabled)
		{
			switchTo = Turnable.EState.Off;
		}
		if (this.m__E000 == null)
		{
			this.m__E000 = new Dictionary<ELightType, Material>();
			foreach (KeyValuePair<ELightType, int> materialIndex in _materialIndexes)
			{
				_E39D.Deconstruct(materialIndex, out var key, out var value);
				ELightType key2 = key;
				int num = value;
				Material value2 = _garland.GetComponent<Renderer>().materials[num];
				this.m__E000.Add(key2, value2);
			}
		}
		if (!this.m__E001 && (switchTo == Turnable.EState.On || switchTo == Turnable.EState.TurningOn || switchTo == Turnable.EState.ConstantFlickering))
		{
			switchTo = Turnable.EState.Off;
		}
		switch (switchTo)
		{
		case Turnable.EState.On:
			_E000(this.m__E003, this.m__E002);
			break;
		case Turnable.EState.TurningOn:
		case Turnable.EState.TurningOff:
		case Turnable.EState.Off:
		case Turnable.EState.Destroyed:
		case Turnable.EState.ConstantFlickering:
			_E003();
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(212501), switchTo, null);
		}
	}

	private void _E000(EPatternType pattern, float speed)
	{
		this.m__E002 = speed;
		this.m__E003 = pattern;
		if (Enabled && pattern == EPatternType.Linear)
		{
			_E001();
		}
	}

	private void _E001()
	{
		_E002 obj = new _E002();
		obj._003C_003E4__this = this;
		obj.index = 0;
		obj._E000();
	}

	private void _E002(ELightType color)
	{
		ELightType key;
		foreach (KeyValuePair<ELightType, List<Light>> light in _lights)
		{
			_E39D.Deconstruct(light, out key, out var value);
			ELightType eLightType = key;
			List<Light> list = value;
			bool active = color == eLightType || color == ELightType.Any;
			foreach (Light item in list)
			{
				_E004(item, active);
			}
		}
		foreach (KeyValuePair<ELightType, Material> item2 in this.m__E000)
		{
			_E39D.Deconstruct(item2, out key, out var value2);
			ELightType eLightType2 = key;
			Material material = value2;
			bool active2 = color == eLightType2 || color == ELightType.Any;
			_E005(material, active2);
		}
	}

	private void _E003()
	{
		this.m__E005.Reset();
		_E002(ELightType.None);
	}

	private void _E004(Light light, bool active)
	{
		light.DOKill();
		light.DOIntensity(active ? Intensity : 0f, this.m__E002);
	}

	private void _E005(Material material, bool active)
	{
		material.DOKill();
		material.DOFloat(active ? EmissionPower : 0f, _ED3E._E000(205934), this.m__E002);
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task<bool> _E006(ELightStatus status)
	{
		return base.PerformInteraction(status);
	}
}
