using UnityEngine;

[ExecuteInEditMode]
public class DynamicFakeGI : MonoBehaviour
{
	[SerializeField]
	private float intensityMultiplier = 1f;

	[SerializeField]
	private float lightSubtract = 0.25f;

	[SerializeField]
	private Color hue = new Color(1f, 1f, 1f, 1f);

	[Range(0f, 1f)]
	public float minMaxWeatherInfluenceAmount;

	private Light _E000;

	private AreaLight _E001;

	private Color _E002;

	private float _E003;

	private float _E004;

	private float _E005;

	private float _E006;

	private void Start()
	{
		if (!_E001)
		{
			_E001 = GetComponent<AreaLight>();
		}
		if (!_E000)
		{
			_E000 = GetComponent<Light>();
		}
	}

	private void Update()
	{
		if (!(TOD_Sky.Instance == null))
		{
			_E006 = Mathf.Clamp(Mathf.Lerp(TOD_Sky.Instance.AmbientColor.grayscale - lightSubtract, 1f, 1f - minMaxWeatherInfluenceAmount) * intensityMultiplier, 0f, 500f);
			Color.RGBToHSV(hue, out _E003, out _E004, out _E005);
			_E002 = Color.HSVToRGB(_E003, _E004, _E006);
			if ((bool)_E001)
			{
				_E001.m_Color = _E002;
			}
			else
			{
				_E000.color = _E002;
			}
		}
	}
}
