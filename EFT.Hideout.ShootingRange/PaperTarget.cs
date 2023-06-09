using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class PaperTarget : MonoBehaviour
{
	[SerializeField]
	private DecalSystem _decal;

	[SerializeField]
	private HideoutTargetBallisticCollider[] _colliders;

	[CompilerGenerated]
	private Action<TargetColliderType, int> m__E000;

	public event Action<TargetColliderType, int> OnHitTarget
	{
		[CompilerGenerated]
		add
		{
			Action<TargetColliderType, int> action = this.m__E000;
			Action<TargetColliderType, int> action2;
			do
			{
				action2 = action;
				Action<TargetColliderType, int> value2 = (Action<TargetColliderType, int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<TargetColliderType, int> action = this.m__E000;
			Action<TargetColliderType, int> action2;
			do
			{
				action2 = action;
				Action<TargetColliderType, int> value2 = (Action<TargetColliderType, int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected void OnEnable()
	{
		HideoutTargetBallisticCollider[] colliders = _colliders;
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].OnHitTarget += _E000;
		}
	}

	protected void OnDisable()
	{
		HideoutTargetBallisticCollider[] colliders = _colliders;
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].OnHitTarget -= _E000;
		}
	}

	private void _E000(TargetColliderType targetColliderType, int segmentTarget)
	{
		this.m__E000?.Invoke(targetColliderType, segmentTarget);
	}

	public void Replace()
	{
		_decal.Clear();
	}
}
