using UnityEngine;

public class RotateObject : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(Vector3.up * Time.deltaTime * 15f, Space.World);
	}
}
