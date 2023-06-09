using UnityEngine;

public class TOD_Weather : MonoBehaviour
{
	[Tooltip("Time to fade from one weather type to the other.")]
	public float FadeTime = 10f;

	[Tooltip("Currently selected cloud type.")]
	public TOD_CloudType Clouds;

	[Tooltip("Currently selected weather type.")]
	public TOD_WeatherType Weather;

	private float _E000;

	private float _E001;

	private float _E002;

	private float _E003;

	private float _E004;

	private float _E005;

	private float _E006;

	private TOD_Sky _E007;

	protected void Start()
	{
		_E007 = GetComponent<TOD_Sky>();
		_E003 = (_E000 = _E007.Clouds.Brightness);
		_E004 = (_E001 = _E007.Clouds.Density);
		_E005 = (_E002 = _E007.Atmosphere.Fogginess);
		_E006 = _E007.Clouds.Sharpness;
	}

	protected void Update()
	{
		if (Clouds != 0 || Weather != 0)
		{
			switch (Clouds)
			{
			case TOD_CloudType.Custom:
				_E004 = _E007.Clouds.Density;
				_E006 = _E007.Clouds.Sharpness;
				break;
			case TOD_CloudType.None:
				_E004 = 0f;
				_E006 = 1f;
				break;
			case TOD_CloudType.Few:
				_E004 = _E001;
				_E006 = 5f;
				break;
			case TOD_CloudType.Scattered:
				_E004 = _E001;
				_E006 = 3f;
				break;
			case TOD_CloudType.Broken:
				_E004 = _E001;
				_E006 = 1f;
				break;
			case TOD_CloudType.Overcast:
				_E004 = _E001;
				_E006 = 0.1f;
				break;
			}
			switch (Weather)
			{
			case TOD_WeatherType.Custom:
				_E003 = _E007.Clouds.Brightness;
				_E005 = _E007.Atmosphere.Fogginess;
				break;
			case TOD_WeatherType.Clear:
				_E003 = _E000;
				_E005 = _E002;
				break;
			case TOD_WeatherType.Storm:
				_E003 = 0.3f;
				_E005 = 1f;
				break;
			case TOD_WeatherType.Dust:
				_E003 = _E000;
				_E005 = 0.5f;
				break;
			case TOD_WeatherType.Fog:
				_E003 = _E000;
				_E005 = 1f;
				break;
			}
			float t = Time.deltaTime / FadeTime;
			_E007.Clouds.Brightness = Mathf.Lerp(_E007.Clouds.Brightness, _E003, t);
			_E007.Clouds.Density = Mathf.Lerp(_E007.Clouds.Density, _E004, t);
			_E007.Clouds.Sharpness = Mathf.Lerp(_E007.Clouds.Sharpness, _E006, t);
			_E007.Atmosphere.Fogginess = Mathf.Lerp(_E007.Atmosphere.Fogginess, _E005, t);
		}
	}
}
