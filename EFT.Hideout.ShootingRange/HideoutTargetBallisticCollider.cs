using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.Ballistics;
using UnityEngine;

namespace EFT.Hideout.ShootingRange;

public class HideoutTargetBallisticCollider : BallisticCollider
{
	[Space]
	[SerializeField]
	private DecalSystem _decal;

	[SerializeField]
	[Space]
	private TargetColliderType _targetType;

	[SerializeField]
	private float[] _sizes;

	[SerializeField]
	[Space]
	private Vector3 _direction = Vector3.forward;

	private const float _E011 = 0.001f;

	public const int MISS_SHOT_TARGET = int.MaxValue;

	public const int PERFECT_SHOT_TARGET = 0;

	[CompilerGenerated]
	private Action<TargetColliderType, int> _E012;

	private List<int> _E013 = new List<int>();

	private Vector3 _E000 => base.transform.rotation * _direction;

	public event Action<TargetColliderType, int> OnHitTarget
	{
		[CompilerGenerated]
		add
		{
			Action<TargetColliderType, int> action = _E012;
			Action<TargetColliderType, int> action2;
			do
			{
				action2 = action;
				Action<TargetColliderType, int> value2 = (Action<TargetColliderType, int>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E012, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<TargetColliderType, int> action = _E012;
			Action<TargetColliderType, int> action2;
			do
			{
				action2 = action;
				Action<TargetColliderType, int> value2 = (Action<TargetColliderType, int>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E012, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public override _E6FF ApplyHit(_EC23 damageInfo, _EC22 shotID)
	{
		base.ApplyHit(damageInfo, shotID);
		Vector3 vector = damageInfo.HitNormal * 0.001f;
		Vector3 position = damageInfo.HitPoint + vector;
		_decal.Add(position, damageInfo.HitNormal);
		_E013.Add(_E000(Vector3.Distance(base.transform.position, damageInfo.HitPoint)));
		return null;
	}

	private void FixedUpdate()
	{
		if (_E013.Any())
		{
			_E012?.Invoke(_targetType, _E013.Min());
			_E013.Clear();
		}
	}

	private int _E000(float distance)
	{
		if (_targetType == TargetColliderType.None)
		{
			return int.MaxValue;
		}
		for (int i = 0; i < _sizes.Length; i++)
		{
			if (distance < _sizes[i])
			{
				return i;
			}
		}
		return int.MaxValue;
	}

	private void OnDrawGizmos()
	{
		if (_targetType != 0)
		{
			DrawGizmosCircles(base.transform.position);
		}
	}

	public void DrawGizmosCircles(Vector3 pos)
	{
		Gizmos.color = Color.red;
		float[] sizes = _sizes;
		foreach (float num in sizes)
		{
			DrawGizmosCircle(pos, this._E000, num, 32);
		}
	}

	public void DrawGizmosCircle(Vector3 pos, Vector3 normal, float radius, int numSegments)
	{
		Vector3 rhs = ((normal.x < normal.z) ? new Vector3(1f, 0f, 0f) : new Vector3(0f, 0f, 1f));
		Vector3 normalized = Vector3.Cross(normal, rhs).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, normal).normalized;
		Vector3 from = pos + normalized * radius;
		float num = (float)Math.PI * 2f / (float)numSegments;
		for (int i = 0; i < numSegments; i++)
		{
			float f = ((i == numSegments - 1) ? 0f : ((float)(i + 1) * num));
			Vector3 vector = new Vector3(Mathf.Sin(f) * base.transform.localScale.x, 0f, Mathf.Cos(f) * base.transform.localScale.z) * radius;
			Vector3 vector2 = pos + normalized2 * vector.x + normalized * vector.z;
			Gizmos.DrawLine(from, vector2);
			from = vector2;
		}
	}
}
