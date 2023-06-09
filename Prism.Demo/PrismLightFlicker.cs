using UnityEngine;

namespace Prism.Demo;

[RequireComponent(typeof(Light))]
public class PrismLightFlicker : MonoBehaviour
{
	[Range(-2f, 2f)]
	public float offset = 0.3f;

	[Range(0f, 1f)]
	public float flickerChance = 0.395f;

	[Range(0f, 5f)]
	public float minAliveTime = 0.04f;

	[Range(0f, 5f)]
	public float maxAliveTime = 0.52f;

	[Range(0f, 5f)]
	public float flickerSpeed = 2f;

	private float m__E000;

	private Light _E001;

	private float _E002;

	private float _E003;

	private float _E004;

	private void Start()
	{
		_E001 = GetComponent<Light>();
		_E002 = _E001.intensity;
	}

	private static float _E000(float start, float end, float t)
	{
		end -= start;
		return end * t * t * t + start;
	}

	private void Update()
	{
		_E001.intensity = _E000(_E001.intensity, _E003, _E004);
		_E004 += Time.deltaTime * flickerSpeed;
		_E004 = Mathf.Min(_E004, 1f);
		if (!(Time.time < this.m__E000))
		{
			float num = Random.Range(0f, 1f);
			if (num > flickerChance)
			{
				num += offset;
				_E003 = _E002 * num;
				this.m__E000 = Time.time + Random.Range(minAliveTime, maxAliveTime);
				_E004 = 0f;
			}
		}
	}
}
