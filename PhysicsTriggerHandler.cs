using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class PhysicsTriggerHandler : MonoBehaviour, IPhysicsTrigger
{
	[CompilerGenerated]
	private Action<Collider> _E000;

	[CompilerGenerated]
	private Action<Collider> _E001;

	public Collider trigger;

	public string Description => _ED3E._E000(55490);

	public event Action<Collider> OnTriggerEnter
	{
		[CompilerGenerated]
		add
		{
			Action<Collider> action = _E000;
			Action<Collider> action2;
			do
			{
				action2 = action;
				Action<Collider> value2 = (Action<Collider>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Collider> action = _E000;
			Action<Collider> action2;
			do
			{
				action2 = action;
				Action<Collider> value2 = (Action<Collider>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<Collider> OnTriggerExit
	{
		[CompilerGenerated]
		add
		{
			Action<Collider> action = _E001;
			Action<Collider> action2;
			do
			{
				action2 = action;
				Action<Collider> value2 = (Action<Collider>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Collider> action = _E001;
			Action<Collider> action2;
			do
			{
				action2 = action;
				Action<Collider> value2 = (Action<Collider>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		if (trigger == null)
		{
			trigger = GetComponent<Collider>();
		}
	}

	void IPhysicsTrigger.OnTriggerEnter(Collider col)
	{
		if (base.enabled)
		{
			_E000?.Invoke(col);
		}
	}

	void IPhysicsTrigger.OnTriggerExit(Collider col)
	{
		if (base.enabled)
		{
			_E001?.Invoke(col);
		}
	}

	private void OnDestroy()
	{
		_E000 = null;
		_E001 = null;
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
