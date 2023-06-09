using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace ChartAndGraph;

public abstract class ChartItemEffect : MonoBehaviour
{
	internal int _E000;

	private CharItemEffectController m__E001;

	[CompilerGenerated]
	private Action<ChartItemEffect> _E002;

	protected CharItemEffectController Controller
	{
		get
		{
			if (this.m__E001 == null)
			{
				this.m__E001 = GetComponent<CharItemEffectController>();
				if (this.m__E001 == null)
				{
					this.m__E001 = base.gameObject.AddComponent<CharItemEffectController>();
				}
			}
			return this.m__E001;
		}
	}

	internal abstract Vector3 _E007 { get; }

	internal abstract Quaternion _E008 { get; }

	internal abstract Vector3 _E009 { get; }

	public event Action<ChartItemEffect> Deactivate
	{
		[CompilerGenerated]
		add
		{
			Action<ChartItemEffect> action = _E002;
			Action<ChartItemEffect> action2;
			do
			{
				action2 = action;
				Action<ChartItemEffect> value2 = (Action<ChartItemEffect>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ChartItemEffect> action = _E002;
			Action<ChartItemEffect> action2;
			do
			{
				action2 = action;
				Action<ChartItemEffect> value2 = (Action<ChartItemEffect>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected void RaiseDeactivated()
	{
		if (_E002 != null)
		{
			_E002(this);
		}
	}

	private void _E000()
	{
		CharItemEffectController controller = Controller;
		if (controller != null)
		{
			controller.Register(this);
		}
	}

	private void _E001()
	{
		CharItemEffectController controller = Controller;
		if (controller != null)
		{
			controller.Unregister(this);
		}
	}

	protected virtual void OnDisable()
	{
		_E001();
	}

	protected virtual void OnEnable()
	{
		_E000();
	}

	protected virtual void Start()
	{
		_E000();
	}

	protected virtual void Destroy()
	{
		_E001();
	}

	public abstract void TriggerIn(bool deactivateOnEnd);

	public abstract void TriggerOut(bool deactivateOnEnd);
}
