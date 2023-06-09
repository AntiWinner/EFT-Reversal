using UnityEngine;

public class OpticCameraData : MonoBehaviour
{
	public float Fov;

	[ContextMenu("ReInit")]
	public void ReInit()
	{
		Camera component = GetComponent<Camera>();
		Fov = component.fieldOfView;
	}
}
