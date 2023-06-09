using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
	public float Distance = 16f;

	public float XSpeed = 250f;

	public float YSpeed = 120f;

	public float YMinLimit = -20f;

	public float YMaxLimit = 80f;

	[Range(0f, 50000f)]
	public float Force = 10000f;

	public Transform Target;

	private float m__E000;

	private float m__E001;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.m__E000 = eulerAngles.y;
		this.m__E001 = eulerAngles.x;
		_E000();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			StartCoroutine(_E002());
		}
		if ((bool)Target && Input.GetKey(KeyCode.Mouse1))
		{
			_E000();
		}
	}

	private void _E000()
	{
		this.m__E000 += (float)((double)Input.GetAxis(_ED3E._E000(23374)) * (double)XSpeed * 0.0199999995529652);
		this.m__E001 -= (float)((double)Input.GetAxis(_ED3E._E000(23366)) * (double)YSpeed * 0.0199999995529652);
		this.m__E001 = _E001(this.m__E001, YMinLimit, YMaxLimit);
		Quaternion quaternion = Quaternion.Euler(this.m__E001, this.m__E000, 0f);
		Vector3 position = quaternion * new Vector3(0f, 0f, 0f - Distance) + Target.position;
		base.transform.rotation = quaternion;
		base.transform.position = position;
	}

	private static float _E001(float angle, float min, float max)
	{
		if ((double)angle < -360.0)
		{
			angle += 360f;
		}
		if ((double)angle > 360.0)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	private IEnumerator _E002()
	{
		Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		if (!Physics.Raycast(ray, out var hitInfo, 100f))
		{
			yield break;
		}
		Rigidbody attachedRigidbody = hitInfo.collider.attachedRigidbody;
		if (attachedRigidbody != null)
		{
			attachedRigidbody.AddForceAtPosition(Force * ray.direction, hitInfo.point);
		}
		yield return new WaitForSeconds(0.1f);
		if ((bool)hitInfo.collider)
		{
			Component component = hitInfo.collider.GetComponent(typeof(BoxFracture));
			if (component is BoxFracture)
			{
				(component as BoxFracture).Destroy(hitInfo.point, 1f);
			}
		}
	}
}
