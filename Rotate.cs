using UnityEngine;

public class Rotate : MonoBehaviour
{
	public float Speed = 10f;

	public void Update()
	{
		base.transform.Rotate(Vector3.up, Speed * Time.deltaTime);
	}
}
