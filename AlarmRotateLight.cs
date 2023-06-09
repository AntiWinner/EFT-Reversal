using UnityEngine;

[ExecuteInEditMode]
public class AlarmRotateLight : MonoBehaviour
{
	public float speed = 6f;

	private void Update()
	{
		base.transform.Rotate(0f, 0f, Time.deltaTime * speed * 40f);
	}
}
