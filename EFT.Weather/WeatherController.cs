using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.Weather;

public class WeatherController : MonoBehaviour
{
	public static WeatherController Instance;

	public CloudsController CloudsController;

	public LightningController LightningController;

	public WindController WindController;

	public RainController RainController;

	public Camera PlayerCamera;

	[Header("GlobalFog")]
	public Gradient GlobalFogColor;

	public float GlobalFogOvercast;

	public AnimationCurve GlobalFogY;

	public AnimationCurve GlobalFogStrength;

	[Header("Smoke")]
	public AnimationCurve SmokeDesaturation;

	[_E2BD(-0.5f, 1f, -1f)]
	[Header("Desaturate")]
	public Vector2 MinMaxAmmount;

	[Tooltip("bigger value summons lightning rarely")]
	[Space(16f)]
	public float LightningSummonBandWidth = 20f;

	public float MinLyingWater;

	public AnimationCurve FogMultyplyer;

	public ToDController TimeOfDayController;

	public bool isDrawDebugGui;

	public float CubemapDayMult = 0.55f;

	public float CubemapNightMult = -0.78f;

	public int OpticCameraResolution = 1024;

	[CompilerGenerated]
	private float m__E000;

	public WeatherDebug WeatherDebug;

	private IWeatherCurve m__E001 = new WeatherCurve();

	private _E8ED m__E002;

	private TOD_Scattering m__E003;

	private CustomGlobalFog m__E004;

	private CC_Sharpen m__E005;

	private static readonly int m__E006 = Shader.PropertyToID(_ED3E._E000(218599));

	private static readonly int m__E007 = Shader.PropertyToID(_ED3E._E000(218639));

	private static readonly int m__E008 = Shader.PropertyToID(_ED3E._E000(45168));

	private static readonly int m__E009 = Shader.PropertyToID(_ED3E._E000(45213));

	private static readonly int m__E00A = Shader.PropertyToID(_ED3E._E000(45202));

	public float SunHeight
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		set
		{
			this.m__E000 = value;
		}
	}

	public IWeatherCurve WeatherCurve
	{
		get
		{
			if (!WeatherDebug.Enabled)
			{
				return this.m__E001;
			}
			return WeatherDebug;
		}
	}

	private TOD_Sky _E00B => TOD_Sky.Instance;

	public void ReInit()
	{
		if (this.m__E002 == null)
		{
			this.m__E002 = new _E8ED
			{
				LightningController = LightningController
			};
		}
		CloudsController.Refresh();
		TimeOfDayController.Update();
		this.m__E002.Update();
		_E005();
		_E003();
	}

	internal void _E000(_E8EB[] nodes)
	{
		this.m__E001 = new WeatherCurve(nodes);
		WeatherDebug.Enabled = false;
		WeatherDebug.CopyParams(this.m__E001);
		CloudsController.CloudPosition = Vector2.zero;
		this._E00B.Components.Animation.CloudPosition = Vector2.zero;
		Debug.Log(nodes.ToPrettyJson());
	}

	public void SetWeatherForce(_E8EB end)
	{
		this.m__E001 = new WeatherCurve(this.m__E001, end);
		WeatherDebug.Enabled = false;
		WeatherDebug.CopyParams(this.m__E001);
	}

	private void Awake()
	{
		Instance = this;
		WeatherDebug.SavePreset();
		this.m__E002 = new _E8ED
		{
			LightningController = LightningController
		};
		_E002();
		_E8A8.Instance.OnCameraChanged += _E001;
		base.gameObject.AddComponent<WeatherEventController>();
		_E003();
	}

	private void _E001()
	{
		Camera camera = _E8A8.Instance.Camera;
		this.m__E004 = camera.GetComponent<CustomGlobalFog>();
		this.m__E003 = camera.GetComponent<TOD_Scattering>();
		this.m__E005 = camera.GetComponent<CC_Sharpen>();
		CloudsController.SetPlayer(camera.transform);
		if (this.m__E003 != null)
		{
			this.m__E003.sky = TOD_Sky.Instance;
		}
	}

	public void OnDestroy()
	{
		Instance = null;
		_E004();
		if (RainController != null)
		{
			RainController.DisableEffect();
		}
		_E8A8.Instance.OnCameraChanged -= _E001;
	}

	private void LateUpdate()
	{
		TimeOfDayController.Update();
		this.m__E002.Update();
		_E005();
	}

	private void _E002()
	{
	}

	private void _E003()
	{
		Shader.EnableKeyword(_ED3E._E000(218614));
		Shader.SetGlobalFloat(WeatherController.m__E006, CubemapNightMult);
		Shader.SetGlobalFloat(WeatherController.m__E007, CubemapDayMult);
	}

	private void _E004()
	{
		Shader.DisableKeyword(_ED3E._E000(218614));
	}

	private void _E005()
	{
		if (!(this._E00B == null))
		{
			float num = (SunHeight = this._E00B.SunDirection.y);
			float num2 = num;
			float t = num2 * 0.5f + 0.5f;
			float fog = WeatherCurve.Fog * FogMultyplyer.Evaluate(num2);
			float fogginess = Mathf.InverseLerp(0f, 0.4f, WeatherCurve.Cloudiness) * 0.5f + Mathf.InverseLerp(0.1f, 0.4f, WeatherCurve.Cloudiness) * 0.5f;
			TimeOfDayController.ContrastMinMax.y = 1.5f - Mathf.Clamp(WeatherCurve.Cloudiness, -1f, 0.4f) * 0.5f;
			_E006(num2, t);
			_E007(fog);
			_E008();
			_E009(fog);
			_E00A();
			_E00B(t, fogginess);
			_E00C();
			_E00D(fogginess);
			_E00E(fogginess);
			_E00F();
		}
	}

	private void _E006(float t, float t01)
	{
		if (!(this.m__E004 == null))
		{
			this.m__E004.FogColor = ToDController.SaturateColor(GlobalFogColor.Evaluate(t01), 1f - this._E00B.Atmosphere.Fogginess);
			this.m__E004.FogColor *= 1f - this._E00B.Atmosphere.Fogginess * GlobalFogOvercast;
			this.m__E004.FogY = GlobalFogY.Evaluate(t);
			this.m__E004.FogStrength = GlobalFogStrength.Evaluate(t);
		}
	}

	private void _E007(float fog)
	{
		float num = this._E00B.Components.Time.GameDateTime?.TimeFactor ?? 1f;
		CloudsController.CloudPosition += -WeatherCurve.Wind * Time.deltaTime * 0.02f * (WindController.CloudWindMultiplier / num);
		CloudsController.FogDensity = fog;
		CloudsController.Density = WeatherCurve.Cloudiness;
		this._E00B.Components.Animation.CloudPosition += WeatherCurve.TopWind * Time.deltaTime * 0.02f;
	}

	private void _E008()
	{
		WindController.SetWind(WeatherCurve.Wind);
	}

	private void _E009(float fog)
	{
		if (!(this.m__E003 == null))
		{
			this.m__E003.GlobalDensity = fog;
		}
	}

	private void _E00A()
	{
		Vector2 vector = WeatherCurve.Wind * WindController.RainWindMultiplier;
		Vector3 fallingVector = new Vector3(vector.x, -25f, vector.y);
		RainController.SetIntensity(WeatherCurve.Rain);
		RainController.SetLyingWater(Mathf.Max(MinLyingWater, RainController.WettingIntensity));
		RainController.SetFallingVector(fallingVector);
		RainController.SetMinAmbientAdditionCoef(WeatherCurve.Cloudiness, this._E00B.Cycle.Hour, WeatherCurve.Rain);
	}

	private void _E00B(float t01, float fogginess)
	{
		RainController.SetWettingIntensity(WeatherCurve.Rain, fogginess, WeatherCurve.Wind, t01);
	}

	private void _E00C()
	{
		this.m__E002.GeneralProbability = WeatherCurve.LightningThunderProbability;
	}

	private void _E00D(float fogginess)
	{
		this._E00B.Atmosphere.Fogginess = Mathf.Clamp01(fogginess);
	}

	private void _E00E(float fogginess)
	{
		if (!(this.m__E005 == null))
		{
			this.m__E005.WeatherDesaturate = Mathf.Lerp(MinMaxAmmount.x, MinMaxAmmount.y, fogginess);
		}
	}

	private void _E00F()
	{
		Shader.SetGlobalColor(WeatherController.m__E008, TimeOfDayController.TopHorizontSkyColor);
		Shader.SetGlobalFloat(WeatherController.m__E009, 0f);
		Shader.SetGlobalColor(WeatherController.m__E00A, Color.clear);
	}
}
