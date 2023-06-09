using UnityEngine;

namespace EFT.Animations;

public class UtesMachinery : WeaponMachinery
{
	[SerializeField]
	private Transform _hingeJoint1;

	[SerializeField]
	private Transform _hingeJoint2;

	[SerializeField]
	private Transform _hingeJoint2DirectionTransform;

	[SerializeField]
	private Transform _weaponJoint;

	[SerializeField]
	private Transform _verticalJoint;

	public override void SetTransforms(TransformLinks hierarchy)
	{
	}

	public override void UpdateJoints()
	{
		Vector3 position = _weaponJoint.position;
		Vector3 vector = _hingeJoint1.parent.InverseTransformPoint(position) - _hingeJoint1.localPosition;
		Vector2 normalized = new Vector2(vector.x, vector.y).normalized;
		_hingeJoint1.localRotation = Quaternion.Euler(0f, 0f, (0f - Mathf.Asin(normalized.x)) * 57.29578f);
		Vector3 vector2 = _verticalJoint.position - position;
		Vector3 vector3 = Quaternion.Inverse(_hingeJoint2.rotation) * (_hingeJoint2DirectionTransform.position - _hingeJoint2.position);
		_hingeJoint2.SetPositionAndRotation(position, Quaternion.identity * Quaternion.FromToRotation(vector3.normalized, vector2.normalized));
	}

	public override void OnRotation()
	{
	}
}
