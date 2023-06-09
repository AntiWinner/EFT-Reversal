using UnityEngine;

public class ThrowableSettings : MonoBehaviour
{
	[SerializeField]
	public Vector3 Offset;

	[Header("Threshold sqrMagnitude")]
	[SerializeField]
	public float VelocityTreshold = 0.005f;
}
