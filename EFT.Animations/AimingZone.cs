using System;
using UnityEngine;

namespace EFT.Animations;

[Serializable]
public class AimingZone
{
	private readonly float _aimingZoneRadius;

	private readonly float _aimingSensitivity = 1f;

	private Vector2 _rotation;

	private Vector2 _cachedAimingRotation;

	public void Rotate(Vector2 deltaRotation)
	{
		_rotation = _cachedAimingRotation + deltaRotation * _aimingSensitivity;
		_cachedAimingRotation = ((_aimingZoneRadius > 0f) ? Vector2.ClampMagnitude(_rotation, _aimingZoneRadius) : Vector2.zero);
	}

	public void Reset()
	{
		_rotation = Vector2.zero;
	}

	public Vector3 GetAimingRotation()
	{
		return new Vector3(_cachedAimingRotation.y, _cachedAimingRotation.x, 0f);
	}

	public Vector2 GetBodyRotation()
	{
		return _rotation - _cachedAimingRotation;
	}
}
