using UnityEngine;

namespace EFT.Animations;

public abstract class WeaponMachinery : MonoBehaviour
{
	public abstract void SetTransforms(TransformLinks hierarchy);

	public abstract void UpdateJoints();

	public abstract void OnRotation();
}
