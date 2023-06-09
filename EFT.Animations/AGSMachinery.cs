using UnityEngine;

namespace EFT.Animations;

public class AGSMachinery : WeaponMachinery
{
	private Transform _E000;

	private Transform _E001;

	private Transform _E002;

	private Transform _E003;

	public override void SetTransforms(TransformLinks hierarchy)
	{
		AGSMachineryBones component = hierarchy.GetComponent<AGSMachineryBones>();
		if ((bool)component)
		{
			_E000 = component.Target;
			_E001 = component.Lever;
			_E002 = component.VerticalPart;
			_E003 = component.Scope;
		}
		else
		{
			_E000 = hierarchy.transform.FindTransform(_ED3E._E000(219752));
			_E001 = hierarchy.transform.FindTransform(_ED3E._E000(219805));
			_E002 = hierarchy.transform.FindTransform(_ED3E._E000(219776));
			_E003 = hierarchy.transform.FindTransform(_ED3E._E000(134727));
		}
	}

	public override void UpdateJoints()
	{
	}

	public override void OnRotation()
	{
		Vector3 position = _E001.position;
		Vector3 position2 = _E000.position;
		Vector3 localPosition = _E002.localPosition;
		float num = Vector3.Distance(position, position2);
		float num2 = Mathf.Asin(localPosition.y / num);
		_E002.localPosition = new Vector3(localPosition.x, localPosition.y, num);
		Vector3 vector = position2 - position;
		Vector3 vector2 = _E001.parent.InverseTransformDirection(vector.normalized);
		_E001.localRotation = Quaternion.Euler((num2 - Mathf.Asin(vector2.y)) * 57.29578f, 0f, 0f);
		_E003.rotation = Quaternion.LookRotation(Vector3.up, _E003.up);
	}
}
