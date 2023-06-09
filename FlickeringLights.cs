using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
	public float MinIntensity = 0.8f;

	public float MaxIntensity = 1.2f;

	public float FlickerRate = 5f;

	private float _E000;

	private Light _E001;

	private void Start()
	{
		_E000 = Random.Range(0f, 100f);
		_E001 = GetComponent<Light>();
	}

	private void Update()
	{
		float t = Mathf.PerlinNoise(_E000, Time.time * FlickerRate);
		if (_E001 != null)
		{
			_E001.intensity = Mathf.Lerp(MinIntensity, MaxIntensity, t);
		}
	}
}
