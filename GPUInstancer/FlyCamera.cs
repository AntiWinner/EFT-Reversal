using UnityEngine;

namespace GPUInstancer;

public class FlyCamera : MonoBehaviour
{
	public float mainSpeed = 10f;

	public float shiftSpeed = 30f;

	public float rotationSpeed = 5f;

	private Vector3 m__E000;

	private Vector3 _E001;

	private void Start()
	{
		this.m__E000 = Vector3.zero;
		_E001 = base.transform.rotation.eulerAngles;
	}

	private void Update()
	{
		if (Input.GetMouseButton(1))
		{
			_E001.x -= Input.GetAxis(_ED3E._E000(23366)) * rotationSpeed;
			_E001.y += Input.GetAxis(_ED3E._E000(23374)) * rotationSpeed;
			base.transform.eulerAngles = _E001;
		}
		_E000();
		base.transform.Translate(this.m__E000);
	}

	private void _E000()
	{
		this.m__E000.x = 0f;
		this.m__E000.y = 0f;
		this.m__E000.z = 0f;
		if (Input.GetKey(KeyCode.W))
		{
			this.m__E000.z += 1f;
		}
		if (Input.GetKey(KeyCode.S))
		{
			this.m__E000.z -= 1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			this.m__E000.x -= 1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			this.m__E000.x += 1f;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			this.m__E000.y -= 1f;
		}
		if (Input.GetKey(KeyCode.E))
		{
			this.m__E000.y += 1f;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.m__E000 *= Time.deltaTime * shiftSpeed;
		}
		else
		{
			this.m__E000 *= Time.deltaTime * mainSpeed;
		}
	}
}
