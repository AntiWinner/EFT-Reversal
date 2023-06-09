using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
	private const float _E006 = 0.5f;

	[CompilerGenerated]
	private Action<Throwable> _E007;

	[CompilerGenerated]
	private Action<Throwable> _E008;

	protected Rigidbody Rigidbody;

	protected Vector3 Velocity;

	protected float IgnoreCollisionTrackingTimer;

	private int _E009;

	private const int _E00A = 180;

	private bool _E00B = true;

	private bool _E00C;

	private ThrowableSettings _E00D;

	private float _E00E;

	private byte _E00F;

	private Coroutine _E010;

	private const float _E011 = 0.2f;

	public byte CollisionNumber
	{
		get
		{
			return _E00F;
		}
		set
		{
			if (value > _E00F)
			{
				_E00F = value;
				if (Time.time > IgnoreCollisionTrackingTimer)
				{
					OnCollisionHandler();
				}
			}
		}
	}

	public abstract int Id { get; }

	public abstract bool HasNetData { get; }

	public event Action<Throwable> DestroyEvent
	{
		[CompilerGenerated]
		add
		{
			Action<Throwable> action = _E007;
			Action<Throwable> action2;
			do
			{
				action2 = action;
				Action<Throwable> value2 = (Action<Throwable>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E007, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Throwable> action = _E007;
			Action<Throwable> action2;
			do
			{
				action2 = action;
				Action<Throwable> value2 = (Action<Throwable>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E007, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<Throwable> VelocityBelowThreshold
	{
		[CompilerGenerated]
		add
		{
			Action<Throwable> action = _E008;
			Action<Throwable> action2;
			do
			{
				action2 = action;
				Action<Throwable> value2 = (Action<Throwable>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E008, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Throwable> action = _E008;
			Action<Throwable> action2;
			do
			{
				action2 = action;
				Action<Throwable> value2 = (Action<Throwable>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E008, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public virtual void Init(ThrowableSettings throwableSettings)
	{
		_E00D = throwableSettings;
	}

	public virtual void OnCollisionHandler()
	{
		IgnoreCollisionTrackingTimer = Time.time + 0.5f;
	}

	protected virtual void OnDestroy()
	{
		_E00C = true;
		_E007?.Invoke(this);
	}

	private void FixedUpdate()
	{
		if (_E00E > 0f)
		{
			Velocity = ((Rigidbody == null) ? Vector3.zero : Rigidbody.velocity);
		}
		if (!(Velocity.sqrMagnitude > _E00D.VelocityTreshold) && _E00B)
		{
			_E009++;
			if (_E009 >= 180)
			{
				_E00B = false;
				_E008?.Invoke(this);
			}
		}
		else
		{
			_E009 = 0;
		}
		_E00E += Time.fixedDeltaTime;
	}

	public virtual _E5C5 GetNetPacket()
	{
		_E5C5 obj = default(_E5C5);
		obj.Id = Id;
		obj.Position = base.transform.position;
		obj.Rotation = base.transform.rotation;
		obj.CollisionNumber = CollisionNumber;
		_E5C5 result = obj;
		if (Rigidbody != null && !_E00C)
		{
			result.Velocity = Rigidbody.velocity;
			result.AngularVelocity = Rigidbody.angularVelocity;
		}
		else
		{
			result.Done = true;
		}
		return result;
	}

	public virtual void ApplyNetPacket(_E5C5 packet)
	{
		if (_E010 != null)
		{
			StopCoroutine(_E010);
		}
		CollisionNumber = packet.CollisionNumber;
		_E010 = StartCoroutine(_E000(packet));
		if (packet.Done)
		{
			OnDoneFromNet();
		}
	}

	private IEnumerator _E000(_E5C5 packet)
	{
		while (!packet.Done && (packet.Position - base.transform.position).sqrMagnitude > 1E-06f)
		{
			yield return new WaitForFixedUpdate();
			Vector3 position = Vector3.Lerp(base.transform.position, packet.Position, 0.2f);
			Quaternion rotation = Quaternion.Lerp(base.transform.rotation, packet.Rotation, 0.2f);
			base.transform.SetPositionAndRotation(position, rotation);
			if (CollisionNumber == packet.CollisionNumber)
			{
				if (Rigidbody != null)
				{
					Rigidbody.velocity = Vector3.Lerp(Rigidbody.velocity, packet.Velocity, 0.2f);
					Rigidbody.angularVelocity = Vector3.Lerp(Rigidbody.angularVelocity, packet.AngularVelocity, 0.2f);
				}
			}
			else
			{
				_E001(packet);
			}
		}
		if (packet.Done)
		{
			base.transform.SetPositionAndRotation(packet.Position, packet.Rotation);
			_E001(packet);
		}
	}

	private void _E001(_E5C5 packet)
	{
		if (Rigidbody != null)
		{
			Rigidbody.velocity = packet.Velocity;
			Rigidbody.angularVelocity = packet.AngularVelocity;
		}
	}

	protected virtual void OnDoneFromNet()
	{
	}
}
