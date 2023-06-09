using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RainController : MonoBehaviour
{
	public enum ERainIntensity
	{
		None,
		Low,
		Med,
		High
	}

	[_E2BD(0f, 1f, -1f)]
	public Vector2 PuddleWaterLevel = new Vector2(0f, 0.95f);

	[SerializeField]
	private DepthPhotograper _depthPhotograper;

	[SerializeField]
	private RainSplashController _rainSplashController;

	[SerializeField]
	private RainFallDrops _rainFallDrops;

	[SerializeField]
	private WetRenderer _wetRenderer;

	[SerializeField]
	private RippleController _rippleController;

	[CompilerGenerated]
	private static bool m__E000;

	[CompilerGenerated]
	private static float m__E001;

	[CompilerGenerated]
	private static float _E002;

	[CompilerGenerated]
	private static Vector3 _E003;

	private RainScreenDrops _E004;

	private ScreenWater _E005;

	private Transform _E006;

	private static readonly List<RainCondensator> _E007 = new List<RainCondensator>();

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(45180));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(84182));

	public static bool IsCameraUnderRain
	{
		[CompilerGenerated]
		get
		{
			return RainController.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			RainController.m__E000 = value;
		}
	}

	public static float Intensity
	{
		[CompilerGenerated]
		get
		{
			return RainController.m__E001;
		}
		[CompilerGenerated]
		private set
		{
			RainController.m__E001 = value;
		}
	}

	public static float WettingIntensity
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	public static ERainIntensity IntensityType
	{
		get
		{
			if (Intensity > 0.7f)
			{
				return ERainIntensity.High;
			}
			if (Intensity > 0.2f)
			{
				return ERainIntensity.Med;
			}
			if (Intensity > 0.01f)
			{
				return ERainIntensity.Low;
			}
			return ERainIntensity.None;
		}
	}

	public static Vector3 FallingVectorV3
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		private set
		{
			_E003 = value;
		}
	}

	private void Awake()
	{
		_rainFallDrops.Init();
		_E8A8.Instance.OnCameraChanged += _E000;
	}

	public void SetFallingVector(Vector3 vec)
	{
		FallingVectorV3 = vec;
	}

	private void _E000()
	{
		Camera camera = _E8A8.Instance.Camera;
		_E006 = camera.transform;
		_E005 = _E8A8.Instance.EffectsController.ScreenWater;
		_E004 = _E8A8.Instance.EffectsController.RainScreenDrops;
		_E004.Init();
		_depthPhotograper.Render();
	}

	private void Update()
	{
		IsCameraUnderRain = _depthPhotograper.CheckIfCameraUnderRain(_E006);
		_E001();
	}

	public void DisableEffect()
	{
		Shader.DisableKeyword(_ED3E._E000(84185));
		if (_E004 != null)
		{
			_E004.enabled = false;
		}
		if (_E005 != null)
		{
			_E005.enabled = false;
		}
	}

	public void SetLyingWater(float laingWater)
	{
		Shader.SetGlobalFloat(_E008, Mathf.LerpUnclamped(PuddleWaterLevel.x, PuddleWaterLevel.y, laingWater));
	}

	public void SetIntensity(float value)
	{
		Shader.SetGlobalFloat(_E009, Intensity);
		Intensity = value;
		_rainFallDrops.Intensity = value;
		_rippleController.Intensity = value;
		_rainSplashController.Intensity = value;
		if (_E004 != null)
		{
			_E004.SetIntensity(value);
		}
		if (_E005 != null)
		{
			_E005.SetIntensity(value);
		}
		if (Mathf.Abs(Intensity) < Mathf.Epsilon)
		{
			Shader.DisableKeyword(_ED3E._E000(84185));
		}
		else
		{
			Shader.EnableKeyword(_ED3E._E000(84185));
		}
	}

	public void SetWettingIntensity(float rainIntensity, float fogginess, Vector2 mainWind, float t01)
	{
		float deltaTime = Time.deltaTime;
		_wetRenderer.Intensity += ((rainIntensity > 0f) ? (rainIntensity * deltaTime / 60f) : ((0f - t01) * (1f + 0.1f * (1f - fogginess) + 0.3f * mainWind.magnitude) * deltaTime / 180f));
		_wetRenderer.Intensity = Mathf.Clamp01(_wetRenderer.Intensity);
		WettingIntensity = _wetRenderer.Intensity;
	}

	public void SetMinAmbientAdditionCoef(float cloudness, float hour, float rain)
	{
		float minAmbientAdditionCoef = (Mathf.Max(Mathf.InverseLerp(0f, 1f, cloudness), Mathf.InverseLerp(0f, 12f, Mathf.Abs(hour - 12f))) + rain) / 2f * (Mathf.InverseLerp(12f, 0f, Mathf.Abs(hour - 12f)) / 2f + 0.5f);
		_rainFallDrops.MinAmbientAdditionCoef = minAmbientAdditionCoef;
	}

	public static void AddRainCondensator(RainCondensator rainCondensator)
	{
		if (!_E007.Contains(rainCondensator))
		{
			_E007.Add(rainCondensator);
		}
	}

	public static void RemoveRainCondensator(RainCondensator rainCondensator)
	{
		if (_E007.Contains(rainCondensator))
		{
			_E007.Remove(rainCondensator);
		}
	}

	private void _E001()
	{
		if (!(Intensity <= 0f))
		{
			for (int i = 0; i < _E007.Count; i++)
			{
				_E007[i].UpdateValues();
			}
		}
	}

	private void OnDestroy()
	{
		DisableEffect();
		_E8A8.Instance.OnCameraChanged -= _E000;
	}
}
