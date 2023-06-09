using UnityEngine;

namespace GPUInstancer;

public class SpaceshipController : MonoBehaviour
{
	public float engineTorque = 1500f;

	public float enginePower = 4500f;

	private Rigidbody m__E000;

	private float m__E001;

	private float m__E002;

	private float _E003;

	private float _E004;

	private ParticleSystem.EmissionModule _E005;

	private ParticleSystem.EmissionModule _E006;

	private Light _E007;

	private float _E008;

	private float _E009;

	private void Awake()
	{
		this.m__E000 = GetComponent<Rigidbody>();
		_E005 = base.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
		_E008 = _E005.rateOverTime.constant;
		_E006 = base.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>()
			.emission;
		_E009 = _E006.rateOverTime.constant;
		Transform transform = base.transform.Find(_ED3E._E000(74712));
		if ((bool)transform)
		{
			_E007 = transform.GetComponent<Light>();
		}
	}

	private void FixedUpdate()
	{
		_E000();
		_E001();
		_E002();
	}

	private void _E000()
	{
		_E004 = Input.GetAxis(_ED3E._E000(23415));
		this.m__E002 = Input.GetAxis(_ED3E._E000(77053));
		_E003 = Input.GetAxis(_ED3E._E000(23422));
		this.m__E001 = (Input.GetKey(KeyCode.Q) ? 1f : (Input.GetKey(KeyCode.E) ? (-1f) : 0f));
	}

	private void _E001()
	{
		this.m__E000.AddRelativeTorque(Vector3.up * _E004 * engineTorque * Time.deltaTime);
		this.m__E000.AddRelativeTorque(Vector3.right * _E003 * engineTorque * Time.deltaTime);
		this.m__E000.AddRelativeTorque(Vector3.forward * this.m__E001 * engineTorque * Time.deltaTime);
		this.m__E000.AddRelativeForce(Vector3.forward * this.m__E002 * enginePower * Time.deltaTime);
	}

	private void _E002()
	{
		_E005.rateOverTime = _E008 * this.m__E002;
		_E006.rateOverTime = Mathf.Lerp(0.5f * _E009, _E009, this.m__E002);
		if ((bool)_E007)
		{
			_E007.intensity = Mathf.Clamp01(0.5f + this.m__E002);
		}
	}
}
