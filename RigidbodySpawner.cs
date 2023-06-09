using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[DisallowMultipleComponent]
public class RigidbodySpawner : MonoBehaviour
{
	public float mass;

	public float drag;

	public float angularDrag;

	public bool useGravity;

	public bool isKinematic;

	public RigidbodyInterpolation interpolation;

	public CollisionDetectionMode collisionDetectionMode;

	[_E376(typeof(RigidbodyConstraints))]
	public RigidbodyConstraints constraints;

	private Rigidbody _E000;

	[CompilerGenerated]
	private Action<RigidbodySpawner, Rigidbody> _E001;

	[CompilerGenerated]
	private Action<RigidbodySpawner> _E002;

	public Rigidbody Rigidbody => _E000;

	public event Action<RigidbodySpawner, Rigidbody> SpawnEvent
	{
		[CompilerGenerated]
		add
		{
			Action<RigidbodySpawner, Rigidbody> action = _E001;
			Action<RigidbodySpawner, Rigidbody> action2;
			do
			{
				action2 = action;
				Action<RigidbodySpawner, Rigidbody> value2 = (Action<RigidbodySpawner, Rigidbody>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<RigidbodySpawner, Rigidbody> action = _E001;
			Action<RigidbodySpawner, Rigidbody> action2;
			do
			{
				action2 = action;
				Action<RigidbodySpawner, Rigidbody> value2 = (Action<RigidbodySpawner, Rigidbody>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<RigidbodySpawner> RemoveEvent
	{
		[CompilerGenerated]
		add
		{
			Action<RigidbodySpawner> action = _E002;
			Action<RigidbodySpawner> action2;
			do
			{
				action2 = action;
				Action<RigidbodySpawner> value2 = (Action<RigidbodySpawner>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<RigidbodySpawner> action = _E002;
			Action<RigidbodySpawner> action2;
			do
			{
				action2 = action;
				Action<RigidbodySpawner> value2 = (Action<RigidbodySpawner>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public Rigidbody Create()
	{
		if (_E000 == null)
		{
			_E000 = base.gameObject.AddComponent<Rigidbody>();
			_E000.mass = mass;
			_E000.drag = drag;
			_E000.angularDrag = angularDrag;
			_E000.useGravity = useGravity;
			_E000.isKinematic = isKinematic;
			_E000.interpolation = interpolation;
			_E000.collisionDetectionMode = collisionDetectionMode;
			_E000.constraints = constraints;
		}
		if (_E001 != null)
		{
			_E001(this, _E000);
		}
		return _E000;
	}

	public void Remove()
	{
		if (_E000 != null)
		{
			UnityEngine.Object.Destroy(_E000);
			_E000 = null;
			if (_E002 != null)
			{
				_E002(this);
			}
		}
	}
}
