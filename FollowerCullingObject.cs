using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[ExecuteInEditMode]
public class FollowerCullingObject : CullingObject
{
	[CompilerGenerated]
	private Action<bool> _E016;

	private Func<Transform> _E017;

	public event Action<bool> OnVisibilityChanged
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E016;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E016, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E016;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E016, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Init(Func<Transform> transformGetter)
	{
		_E017 = transformGetter;
	}

	public void SetParams(float radius, Vector3 shift, float cullDistance)
	{
		base.Radius = radius;
		base.Shift = shift;
		CullDistance = cullDistance;
		if (CullingManager.Instance != null)
		{
			CullingManager.Instance.UpdateSphere(this);
		}
	}

	protected override Transform GetTransform()
	{
		return _E017();
	}

	public override void SetVisibility(bool isVisible)
	{
		if (base.IsVisible != isVisible)
		{
			base.SetVisibility(isVisible);
			_E016?.Invoke(isVisible);
			if (CullingManager.Instance != null)
			{
				CullingManager.Instance.UpdateSphere(this);
			}
		}
	}
}
