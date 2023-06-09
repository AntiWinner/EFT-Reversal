using UnityEngine;

namespace GPUInstancer;

public class ShieldImpact : MonoBehaviour
{
	private float _E000;

	private Material _E001;

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(77039));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(77027));

	private void Awake()
	{
		_E001 = base.transform.Find(_ED3E._E000(77050)).GetComponent<MeshRenderer>().material;
	}

	private void Update()
	{
		if (_E000 > 0f)
		{
			_E000 -= Time.deltaTime * 1000f;
			if (_E000 < 0f)
			{
				_E000 = 0f;
			}
			_E001.SetFloat(_E002, _E000);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		ContactPoint[] contacts = collision.contacts;
		foreach (ContactPoint contactPoint in contacts)
		{
			_E001.SetVector(_E003, base.transform.InverseTransformPoint(contactPoint.point));
			_E000 = 500f;
			_E001.SetFloat(_E002, _E000);
		}
	}
}
