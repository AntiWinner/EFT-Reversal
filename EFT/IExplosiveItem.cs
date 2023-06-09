using UnityEngine;

namespace EFT;

public interface IExplosiveItem
{
	Vector3 Blindness { get; }

	Vector3 Contusion { get; }

	Vector3 ArmorDistanceDistanceDamage { get; }

	float MinExplosionDistance { get; }

	float MaxExplosionDistance { get; }

	int FragmentsCount { get; }

	float GetStrength { get; }

	bool IsDummy { get; }

	_EA12 CreateFragment();
}
