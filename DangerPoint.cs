using UnityEngine;

public class DangerPoint : MonoBehaviour
{
	public float Radius;

	[HideInInspector]
	public Vector3 Position;

	public void Awake()
	{
		Position = base.transform.position;
	}
}
