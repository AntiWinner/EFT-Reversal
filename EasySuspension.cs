using UnityEngine;

[ExecuteInEditMode]
public class EasySuspension : MonoBehaviour
{
	[Range(0.1f, 20f)]
	[Tooltip("Natural frequency of the suspension springs. Describes bounciness of the suspension.")]
	public float naturalFrequency = 10f;

	[Tooltip("Damping ratio of the suspension springs. Describes how fast the spring returns back after a bounce. ")]
	[Range(0f, 3f)]
	public float dampingRatio = 0.8f;

	[Range(-1f, 1f)]
	[Tooltip("The distance along the Y axis the suspension forces application point is offset below the center of mass")]
	public float forceShift = 0.03f;

	[Tooltip("Adjust the length of the suspension springs according to the natural frequency and damping ratio. When off, can cause unrealistic suspension bounce.")]
	public bool setSuspensionDistance = true;

	private Rigidbody _E000;

	public void SetRigidbody(Rigidbody rigidbody)
	{
		_E000 = rigidbody;
	}

	private void Update()
	{
		if (_E000 == null)
		{
			return;
		}
		WheelCollider[] componentsInChildren = GetComponentsInChildren<WheelCollider>();
		foreach (WheelCollider wheelCollider in componentsInChildren)
		{
			JointSpring suspensionSpring = wheelCollider.suspensionSpring;
			float num = Mathf.Sqrt(wheelCollider.sprungMass);
			suspensionSpring.spring = num * naturalFrequency * num * naturalFrequency;
			suspensionSpring.damper = 2f * dampingRatio * Mathf.Sqrt(suspensionSpring.spring * wheelCollider.sprungMass);
			wheelCollider.suspensionSpring = suspensionSpring;
			Vector3 vector = base.transform.InverseTransformPoint(wheelCollider.transform.position);
			float num2 = _E000.centerOfMass.y - vector.y + wheelCollider.radius;
			wheelCollider.forceAppPointDistance = num2 - forceShift;
			if (suspensionSpring.targetPosition > 0f && setSuspensionDistance)
			{
				wheelCollider.suspensionDistance = wheelCollider.sprungMass * Physics.gravity.magnitude / (suspensionSpring.targetPosition * suspensionSpring.spring);
			}
		}
	}
}
