using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace EFT.Interactive;

[RequireComponent(typeof(Collider))]
public class ColliderReporter : MonoBehaviour, IPhysicsTrigger
{
	[CompilerGenerated]
	private Action<ColliderReporter, Collider> m__E000;

	[CompilerGenerated]
	private Action<ColliderReporter, Collider> _E001;

	[NonSerialized]
	public List<MonoBehaviour> Owners = new List<MonoBehaviour>(2);

	[NonSerialized]
	public bool IsInverse;

	[NonSerialized]
	public bool Entered;

	[CompilerGenerated]
	private readonly string _E002 = _ED3E._E000(206327);

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
	}

	public event Action<ColliderReporter, Collider> OnTriggerEnterEvent
	{
		[CompilerGenerated]
		add
		{
			Action<ColliderReporter, Collider> action = this.m__E000;
			Action<ColliderReporter, Collider> action2;
			do
			{
				action2 = action;
				Action<ColliderReporter, Collider> value2 = (Action<ColliderReporter, Collider>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ColliderReporter, Collider> action = this.m__E000;
			Action<ColliderReporter, Collider> action2;
			do
			{
				action2 = action;
				Action<ColliderReporter, Collider> value2 = (Action<ColliderReporter, Collider>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<ColliderReporter, Collider> OnTriggerExitEvent
	{
		[CompilerGenerated]
		add
		{
			Action<ColliderReporter, Collider> action = _E001;
			Action<ColliderReporter, Collider> action2;
			do
			{
				action2 = action;
				Action<ColliderReporter, Collider> value2 = (Action<ColliderReporter, Collider>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ColliderReporter, Collider> action = _E001;
			Action<ColliderReporter, Collider> action2;
			do
			{
				action2 = action;
				Action<ColliderReporter, Collider> value2 = (Action<ColliderReporter, Collider>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		Entered = true;
		if (this.m__E000 != null)
		{
			this.m__E000(this, col);
		}
	}

	public void OnTriggerExit(Collider col)
	{
		Entered = false;
		if (_E001 != null)
		{
			_E001(this, col);
		}
	}

	public void AddOwner(MonoBehaviour disablerCullingObject)
	{
		if (!Owners.Contains(disablerCullingObject))
		{
			Owners.Add(disablerCullingObject);
		}
	}

	public void RemoveOwner(MonoBehaviour disablerCullingObject)
	{
		Owners.Remove(disablerCullingObject);
	}

	[Conditional("UNITY_EDITOR")]
	private void _E000()
	{
		for (int num = Owners.Count - 1; num >= 0; num--)
		{
			if (Owners[num] == null)
			{
				Owners.RemoveAt(num);
			}
		}
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
