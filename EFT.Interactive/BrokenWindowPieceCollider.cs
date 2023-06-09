using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace EFT.Interactive;

public sealed class BrokenWindowPieceCollider : MonoBehaviour, _E31B, IPhysicsTrigger
{
	[CompilerGenerated]
	private Action<Vector3, Vector3> _E000;

	public BoxCollider Collider;

	private float _E001;

	public string Description => _ED3E._E000(206016);

	public event Action<Vector3, Vector3> OnPlayerCollision
	{
		[CompilerGenerated]
		add
		{
			Action<Vector3, Vector3> action = _E000;
			Action<Vector3, Vector3> action2;
			do
			{
				action2 = action;
				Action<Vector3, Vector3> value2 = (Action<Vector3, Vector3>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Vector3, Vector3> action = _E000;
			Action<Vector3, Vector3> action2;
			do
			{
				action2 = action;
				Action<Vector3, Vector3> value2 = (Action<Vector3, Vector3>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void OnTriggerStay(Collider col)
	{
		if (!(Time.time < _E001) && Physics.CheckBox(Collider.bounds.center, Collider.bounds.extents, base.transform.rotation, _E37B.PlayerMask))
		{
			_E001 = Time.time + 0.2f;
			_E000?.Invoke(col.transform.position, base.transform.position - col.transform.position);
			_E000 = null;
		}
	}

	public void OnTriggerEnter(Collider col)
	{
	}

	public void OnTriggerExit(Collider col)
	{
	}

	public void UnsubscibeAction()
	{
		_E000 = null;
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
