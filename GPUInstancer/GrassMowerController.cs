using UnityEngine;

namespace GPUInstancer;

public class GrassMowerController : MonoBehaviour
{
	public float engineTorque = 3500f;

	public float enginePower = 4000f;

	private Rigidbody m__E000;

	private float m__E001;

	private float _E002;

	private void Awake()
	{
		this.m__E000 = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		_E000();
		_E001();
	}

	private void _E000()
	{
		_E002 = Input.GetAxis(_ED3E._E000(23415));
		this.m__E001 = Input.GetAxis(_ED3E._E000(77053));
	}

	private void _E001()
	{
		this.m__E000.AddRelativeTorque(Vector3.up * _E002 * engineTorque * Time.deltaTime);
		this.m__E000.AddRelativeForce(Vector3.forward * this.m__E001 * enginePower * Time.deltaTime);
	}
}
