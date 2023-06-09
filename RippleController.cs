using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class RippleController : MonoBehaviour
{
	[Serializable]
	public class Cycle
	{
		public float CucleSpeed = 0.4f;

		[_E2BD(0.0001f, 1f, -1f)]
		public Vector2 CucleLengthRelationMinMax = new Vector2(0.01f, 1f);

		[_E2BD(0f, 1f, -1f)]
		public Vector2 IntensityMinMax = new Vector2(0f, 0.1f);

		private float _time;

		private float _lastCucleLengthRelation = 1f;

		public Vector4 Update(float dt, float intensity)
		{
			_time += dt * CucleSpeed;
			float num = 1f / Mathf.Lerp(CucleLengthRelationMinMax.x, CucleLengthRelationMinMax.y, intensity);
			_time = _E000(_time, _lastCucleLengthRelation, num);
			_lastCucleLengthRelation = num;
			return new Vector4(_time, _lastCucleLengthRelation, Mathf.InverseLerp(IntensityMinMax.x, IntensityMinMax.y, intensity));
		}

		private static float _E000(float currentTime, float lastCucleLengthRelation, float newCucleLengthRelation)
		{
			return currentTime * (newCucleLengthRelation / lastCucleLengthRelation);
		}
	}

	private static int m__E000;

	private static int m__E001;

	private static int _E002;

	private static int _E003;

	private static int _E004;

	private static int _E005;

	private static int _E006;

	private static int _E007;

	private static int _E008;

	private static int _E009;

	private static int _E00A;

	private static bool _E00B;

	[CompilerGenerated]
	private float _E00C;

	public Texture2D RippleTexture;

	[Header("Water Ripples")]
	public Cycle WaterCucle;

	public int RippleWaveCount = 3;

	public float RippleWaveFreq = 3f;

	[Header("Environment Ripples")]
	public Cycle EnvironmentCucle;

	[Range(0f, 1f)]
	public float WetDropsSpecular = 0.576f;

	[Range(0f, 1f)]
	public float WetDropsGlossness = 0.553f;

	[Range(0f, 1f)]
	public float WetDropsAlbedo = 0.248f;

	[Range(0f, 1f)]
	public float WetDropsNormal = 1f;

	[Range(0f, 3f)]
	public float RippleFakeLightIntensity = 0.4f;

	[Range(0f, 5f)]
	public float RainIntensityRippleScale = 2f;

	[SerializeField]
	private bool _forceTriplanar = true;

	[SerializeField]
	private bool _useFakeRippleLight = true;

	public float Intensity
	{
		[CompilerGenerated]
		get
		{
			return _E00C;
		}
		[CompilerGenerated]
		set
		{
			_E00C = value;
		}
	}

	private static void _E000()
	{
		if (!_E00B)
		{
			RippleController.m__E000 = Shader.PropertyToID(_ED3E._E000(84367));
			RippleController.m__E001 = Shader.PropertyToID(_ED3E._E000(84409));
			_E002 = Shader.PropertyToID(_ED3E._E000(84388));
			_E003 = Shader.PropertyToID(_ED3E._E000(84428));
			_E004 = Shader.PropertyToID(_ED3E._E000(84419));
			_E005 = Shader.PropertyToID(_ED3E._E000(84460));
			_E006 = Shader.PropertyToID(_ED3E._E000(84510));
			_E007 = Shader.PropertyToID(_ED3E._E000(84493));
			_E008 = Shader.PropertyToID(_ED3E._E000(84540));
			_E009 = Shader.PropertyToID(_ED3E._E000(84518));
			_E00A = Shader.PropertyToID(_ED3E._E000(84552));
			_E00B = true;
		}
	}

	private void Start()
	{
		_E000();
		if (_forceTriplanar)
		{
			Shader.EnableKeyword(_ED3E._E000(84606));
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(84606));
		}
		if (_useFakeRippleLight)
		{
			Shader.EnableKeyword(_ED3E._E000(84592));
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(84592));
		}
	}

	private void OnDestroy()
	{
		Shader.DisableKeyword(_ED3E._E000(84606));
		Shader.DisableKeyword(_ED3E._E000(84592));
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		Vector4 waterCycle = WaterCucle.Update(deltaTime, Intensity);
		float dt = (RainController.IsCameraUnderRain ? deltaTime : 0f);
		Vector4 environmentCycle = EnvironmentCucle.Update(dt, Intensity);
		_E001(deltaTime, waterCycle, environmentCycle);
	}

	private void _E001(float dt, Vector4 waterCycle, Vector4 environmentCycle)
	{
		Shader.SetGlobalVector(RippleController.m__E000, waterCycle);
		Shader.SetGlobalVector(RippleController.m__E001, new Vector4(RippleWaveCount, RippleWaveFreq));
		Shader.SetGlobalVector(_E002, environmentCycle);
		Shader.SetGlobalTexture(_E003, RippleTexture);
		Shader.SetGlobalFloat(_E004, WetDropsSpecular);
		Shader.SetGlobalFloat(_E005, WetDropsGlossness);
		Shader.SetGlobalFloat(_E006, WetDropsAlbedo);
		Shader.SetGlobalFloat(_E007, WetDropsNormal);
		Shader.SetGlobalFloat(_E008, RippleFakeLightIntensity);
		Shader.SetGlobalFloat(_E009, RainIntensityRippleScale);
		if (TOD_Sky.Instance != null)
		{
			Shader.SetGlobalVector(_E00A, -TOD_Sky.Instance.SunDirection);
		}
	}
}
