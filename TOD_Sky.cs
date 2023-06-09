using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using Comfort.Common;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(TOD_Resources))]
[ExecuteInEditMode]
[RequireComponent(typeof(TOD_Components))]
public class TOD_Sky : MonoBehaviour
{
	private static List<TOD_Sky> m__E000 = new List<TOD_Sky>();

	private int m__E001 = -1;

	public TOD_ColorSpaceType ColorSpace;

	public TOD_ColorRangeType ColorRange;

	public TOD_SkyQualityType SkyQuality;

	public TOD_CloudQualityType CloudQuality = TOD_CloudQualityType.Bumped;

	public TOD_MeshQualityType MeshQuality = TOD_MeshQualityType.High;

	public TOD_CycleParameters Cycle;

	public TOD_WorldParameters World;

	public TOD_AtmosphereParameters Atmosphere;

	public TOD_DayParameters Day;

	public TOD_NightParameters Night;

	public TOD_SunParameters Sun;

	public TOD_MoonParameters Moon;

	public TOD_StarParameters Stars;

	public TOD_CloudParameters Clouds;

	public TOD_LightParameters Light;

	public TOD_FogParameters Fog;

	public TOD_AmbientParameters Ambient;

	public TOD_ReflectionParameters Reflection;

	[CompilerGenerated]
	private bool m__E002;

	[CompilerGenerated]
	private TOD_Components m__E003;

	[CompilerGenerated]
	private TOD_Resources m__E004;

	[CompilerGenerated]
	private bool m__E005;

	[CompilerGenerated]
	private bool m__E006;

	[CompilerGenerated]
	private float m__E007;

	[CompilerGenerated]
	private float m__E008;

	[CompilerGenerated]
	private float m__E009;

	[CompilerGenerated]
	private Vector3 m__E00A;

	[CompilerGenerated]
	private Vector3 m__E00B;

	[CompilerGenerated]
	private Vector3 m__E00C;

	[CompilerGenerated]
	private Vector3 m__E00D;

	[CompilerGenerated]
	private Vector3 m__E00E;

	[CompilerGenerated]
	private Vector3 m__E00F;

	[CompilerGenerated]
	private Color m__E010;

	[CompilerGenerated]
	private Color m__E011;

	[CompilerGenerated]
	private Color m__E012;

	[CompilerGenerated]
	private Color _E013;

	[CompilerGenerated]
	private Color _E014;

	[CompilerGenerated]
	private Color _E015;

	[CompilerGenerated]
	private Color _E016;

	[CompilerGenerated]
	private Color _E017;

	[CompilerGenerated]
	private Color _E018;

	[CompilerGenerated]
	private Color _E019;

	[CompilerGenerated]
	private Color _E01A;

	[CompilerGenerated]
	private Color _E01B;

	[CompilerGenerated]
	private Color _E01C;

	[CompilerGenerated]
	private ReflectionProbe _E01D;

	private float _E01E = float.MaxValue;

	private float _E01F = float.MaxValue;

	private float _E020 = float.MaxValue;

	private float _E021;

	private float _E022;

	private const int _E023 = 2;

	private Vector3 _E024;

	private Vector4 _E025;

	private Vector4 _E026;

	private Vector4 _E027;

	private Vector4 _E028;

	[Tooltip("sLerp debug rotation speed")]
	public float rotationSpeed = 1f;

	private const float _E029 = (float)Math.PI;

	private const float _E02A = (float)Math.PI * 2f;

	private float _E02B;

	private float _E02C;

	private float _E02D;

	private float _E02E;

	private Quaternion _E02F;

	private Quaternion _E030;

	public static List<TOD_Sky> Instances => TOD_Sky.m__E000;

	public static TOD_Sky Instance
	{
		get
		{
			if (TOD_Sky.m__E000.Count != 0)
			{
				return TOD_Sky.m__E000[TOD_Sky.m__E000.Count - 1];
			}
			return null;
		}
	}

	public bool Initialized
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E002 = value;
		}
	}

	public bool Headless => Camera.allCamerasCount == 0;

	public TOD_Components Components
	{
		[CompilerGenerated]
		get
		{
			return this.m__E003;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E003 = value;
		}
	}

	public TOD_Resources Resources
	{
		[CompilerGenerated]
		get
		{
			return this.m__E004;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E004 = value;
		}
	}

	public bool IsDay
	{
		[CompilerGenerated]
		get
		{
			return this.m__E005;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E005 = value;
		}
	}

	public bool IsNight
	{
		[CompilerGenerated]
		get
		{
			return this.m__E006;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E006 = value;
		}
	}

	public float Radius => Components.DomeTransform.lossyScale.y;

	public float Diameter => Components.DomeTransform.lossyScale.y * 2f;

	public float LerpValue
	{
		[CompilerGenerated]
		get
		{
			return this.m__E007;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E007 = value;
		}
	}

	public float SunZenith
	{
		[CompilerGenerated]
		get
		{
			return this.m__E008;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E008 = value;
		}
	}

	public float MoonZenith
	{
		[CompilerGenerated]
		get
		{
			return this.m__E009;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E009 = value;
		}
	}

	public float LightZenith => Mathf.Min(SunZenith, MoonZenith);

	public float LightIntensity => Components.LightSource.intensity;

	public Vector3 SunDirection
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00A;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00A = value;
		}
	}

	public Vector3 MoonDirection
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00B;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00B = value;
		}
	}

	public Vector3 LightDirection
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00C;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00C = value;
		}
	}

	public Vector3 LocalSunDirection
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00D;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00D = value;
		}
	}

	public Vector3 LocalMoonDirection
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00E;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00E = value;
		}
	}

	public Vector3 LocalLightDirection
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00F;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00F = value;
		}
	}

	public Color SunLightColor
	{
		[CompilerGenerated]
		get
		{
			return this.m__E010;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E010 = value;
		}
	}

	public Color MoonLightColor
	{
		[CompilerGenerated]
		get
		{
			return this.m__E011;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E011 = value;
		}
	}

	public Color LightColor => Components.LightSource.color;

	public Color SunRayColor
	{
		[CompilerGenerated]
		get
		{
			return this.m__E012;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E012 = value;
		}
	}

	public Color MoonRayColor
	{
		[CompilerGenerated]
		get
		{
			return _E013;
		}
		[CompilerGenerated]
		private set
		{
			_E013 = value;
		}
	}

	public Color RayColor
	{
		[CompilerGenerated]
		get
		{
			return _E014;
		}
		[CompilerGenerated]
		private set
		{
			_E014 = value;
		}
	}

	public Color SunSkyColor
	{
		[CompilerGenerated]
		get
		{
			return _E015;
		}
		[CompilerGenerated]
		private set
		{
			_E015 = value;
		}
	}

	public Color MoonSkyColor
	{
		[CompilerGenerated]
		get
		{
			return _E016;
		}
		[CompilerGenerated]
		private set
		{
			_E016 = value;
		}
	}

	public Color SunMeshColor
	{
		[CompilerGenerated]
		get
		{
			return _E017;
		}
		[CompilerGenerated]
		private set
		{
			_E017 = value;
		}
	}

	public Color MoonMeshColor
	{
		[CompilerGenerated]
		get
		{
			return _E018;
		}
		[CompilerGenerated]
		private set
		{
			_E018 = value;
		}
	}

	public Color CloudColor
	{
		[CompilerGenerated]
		get
		{
			return _E019;
		}
		[CompilerGenerated]
		private set
		{
			_E019 = value;
		}
	}

	public Color GroundColor
	{
		[CompilerGenerated]
		get
		{
			return _E01A;
		}
		[CompilerGenerated]
		private set
		{
			_E01A = value;
		}
	}

	public Color AmbientColor
	{
		[CompilerGenerated]
		get
		{
			return _E01B;
		}
		[CompilerGenerated]
		private set
		{
			_E01B = value;
		}
	}

	public Color MoonHaloColor
	{
		[CompilerGenerated]
		get
		{
			return _E01C;
		}
		[CompilerGenerated]
		private set
		{
			_E01C = value;
		}
	}

	public ReflectionProbe Probe
	{
		[CompilerGenerated]
		get
		{
			return _E01D;
		}
		[CompilerGenerated]
		private set
		{
			_E01D = value;
		}
	}

	public Vector3 OrbitalToUnity(float radius, float theta, float phi)
	{
		float num = Mathf.Sin(theta);
		float num2 = Mathf.Cos(theta);
		float num3 = Mathf.Sin(phi);
		float num4 = Mathf.Cos(phi);
		Vector3 result = default(Vector3);
		result.z = radius * num * num4;
		result.y = radius * num2;
		result.x = radius * num * num3;
		return result;
	}

	public Vector3 OrbitalToLocal(float theta, float phi)
	{
		float num = Mathf.Sin(theta);
		float y = Mathf.Cos(theta);
		float num2 = Mathf.Sin(phi);
		float num3 = Mathf.Cos(phi);
		Vector3 result = default(Vector3);
		result.z = num * num3;
		result.y = y;
		result.x = num * num2;
		return result;
	}

	public Color SampleAtmosphere(Vector3 direction, bool directLight = true)
	{
		Vector3 dir = Components.DomeTransform.InverseTransformDirection(direction);
		Color color = _E010(dir, directLight);
		color = _E00D(color);
		return _E00F(color);
	}

	public Color SampleAtmosphereRaw(Vector3 direction, bool directLight = true)
	{
		Vector3 dir = Components.DomeTransform.InverseTransformDirection(direction);
		return _E010(dir, directLight);
	}

	public SphericalHarmonicsL2 RenderToSphericalHarmonics()
	{
		SphericalHarmonicsL2 result = default(SphericalHarmonicsL2);
		bool directLight = true;
		Color linear = AmbientColor.linear;
		linear *= 0f;
		Vector3 vector = new Vector3(0.61237246f, 0.5f, 0.61237246f);
		Vector3 up = Vector3.up;
		Color linear2 = SampleAtmosphere(up, directLight).linear;
		result.AddDirectionalLight(up, linear2, 0.42857143f);
		Vector3 direction = new Vector3(0f - vector.x, vector.y, 0f - vector.z);
		Color linear3 = SampleAtmosphere(direction, directLight).linear;
		result.AddDirectionalLight(direction, linear3, 0.2857143f);
		Vector3 direction2 = new Vector3(vector.x, vector.y, 0f - vector.z);
		Color linear4 = SampleAtmosphere(direction2, directLight).linear;
		result.AddDirectionalLight(direction2, linear4, 0.2857143f);
		Vector3 direction3 = new Vector3(0f - vector.x, vector.y, vector.z);
		Color linear5 = SampleAtmosphere(direction3, directLight).linear;
		result.AddDirectionalLight(direction3, linear5, 0.2857143f);
		Vector3 direction4 = new Vector3(vector.x, vector.y, vector.z);
		Color linear6 = SampleAtmosphere(direction4, directLight).linear;
		result.AddDirectionalLight(direction4, linear6, 0.2857143f);
		Vector3 left = Vector3.left;
		Color linear7 = SampleAtmosphere(left, directLight).linear;
		result.AddDirectionalLight(left, linear7, 1f / 7f);
		Vector3 right = Vector3.right;
		Color linear8 = SampleAtmosphere(right, directLight).linear;
		result.AddDirectionalLight(right, linear8, 1f / 7f);
		Vector3 back = Vector3.back;
		Color linear9 = SampleAtmosphere(back, directLight).linear;
		result.AddDirectionalLight(back, linear9, 1f / 7f);
		Vector3 forward = Vector3.forward;
		Color linear10 = SampleAtmosphere(forward, directLight).linear;
		result.AddDirectionalLight(forward, linear10, 1f / 7f);
		Vector3 direction5 = new Vector3(0f - vector.x, 0f - vector.y, 0f - vector.z);
		result.AddDirectionalLight(direction5, linear, 0.2857143f);
		Vector3 direction6 = new Vector3(vector.x, 0f - vector.y, 0f - vector.z);
		result.AddDirectionalLight(direction6, linear, 0.2857143f);
		Vector3 direction7 = new Vector3(0f - vector.x, 0f - vector.y, vector.z);
		result.AddDirectionalLight(direction7, linear, 0.2857143f);
		Vector3 direction8 = new Vector3(vector.x, 0f - vector.y, vector.z);
		result.AddDirectionalLight(direction8, linear, 0.2857143f);
		Vector3 down = Vector3.down;
		result.AddDirectionalLight(down, linear, 0.42857143f);
		return result;
	}

	public void RenderToCubemap(RenderTexture targetTexture = null)
	{
		if (!Probe)
		{
			Probe = new GameObject().AddComponent<ReflectionProbe>();
			Probe.name = base.gameObject.name + _ED3E._E000(18607);
			Probe.mode = ReflectionProbeMode.Realtime;
		}
		if (this.m__E001 < 0 || Probe.IsFinishedRendering(this.m__E001))
		{
			float num = float.MaxValue;
			Probe.transform.position = Components.DomeTransform.position;
			Probe.size = new Vector3(num, num, num);
			Probe.intensity = RenderSettings.reflectionIntensity;
			Probe.clearFlags = Reflection.ClearFlags;
			Probe.backgroundColor = Color.black;
			Probe.cullingMask = Reflection.CullingMask;
			Probe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
			Probe.timeSlicingMode = Reflection.TimeSlicing;
			this.m__E001 = Probe.RenderProbe(targetTexture);
		}
	}

	public Color SampleFogColor(bool directLight = true)
	{
		Vector3 vector = Vector3.forward;
		if (Components.Camera != null)
		{
			vector = Quaternion.Euler(0f, Components.Camera.transform.rotation.eulerAngles.y, 0f) * vector;
		}
		Color color = SampleAtmosphere(Vector3.Lerp(vector, Vector3.up, Fog.HeightBias).normalized, directLight);
		return new Color(color.r, color.g, color.b, 1f);
	}

	public Color SampleSkyColor()
	{
		Vector3 sunDirection = SunDirection;
		sunDirection.y = Mathf.Abs(sunDirection.y);
		Color color = SampleAtmosphere(sunDirection.normalized, directLight: false);
		return new Color(color.r, color.g, color.b, 1f);
	}

	public Color SampleEquatorColor()
	{
		Vector3 sunDirection = SunDirection;
		sunDirection.y = 0f;
		Color color = SampleAtmosphere(sunDirection.normalized, directLight: false);
		return new Color(color.r, color.g, color.b, 1f);
	}

	public void UpdateFog()
	{
		switch (Fog.Mode)
		{
		case TOD_FogType.Color:
			RenderSettings.fogColor = SampleFogColor(directLight: false);
			break;
		case TOD_FogType.Directional:
			RenderSettings.fogColor = SampleFogColor();
			break;
		case TOD_FogType.None:
			break;
		}
	}

	public void UpdateAmbient()
	{
	}

	public void UpdateReflection()
	{
	}

	public void LoadParameters(string xml)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(TOD_Parameters));
		XmlTextReader xmlReader = new XmlTextReader(new StringReader(xml));
		(xmlSerializer.Deserialize(xmlReader) as TOD_Parameters).ToSky(this);
	}

	private void _E000()
	{
		if (!Headless)
		{
			Mesh mesh = null;
			Mesh mesh2 = null;
			Mesh mesh3 = null;
			Mesh mesh4 = null;
			Mesh mesh5 = null;
			Mesh mesh6 = null;
			switch (MeshQuality)
			{
			case TOD_MeshQualityType.Low:
				mesh = Resources.IcosphereLow;
				mesh2 = Resources.IcosphereLow;
				mesh3 = Resources.IcosphereLow;
				mesh4 = Resources.HalfIcosphereLow;
				mesh5 = Resources.Quad;
				mesh6 = Resources.SphereLow;
				break;
			case TOD_MeshQualityType.Medium:
				mesh = Resources.IcosphereMedium;
				mesh2 = Resources.IcosphereMedium;
				mesh3 = Resources.IcosphereLow;
				mesh4 = Resources.HalfIcosphereMedium;
				mesh5 = Resources.Quad;
				mesh6 = Resources.SphereMedium;
				break;
			case TOD_MeshQualityType.High:
				mesh = Resources.IcosphereHigh;
				mesh2 = Resources.IcosphereHigh;
				mesh3 = Resources.IcosphereLow;
				mesh4 = Resources.HalfIcosphereHigh;
				mesh5 = Resources.Quad;
				mesh6 = Resources.SphereHigh;
				break;
			}
			mesh5.bounds = new Bounds(Vector3.zero, Vector3.one * 1000000f);
			if ((bool)Components.SpaceRenderer && Components.SpaceMaterial != Resources.SpaceMaterial)
			{
				TOD_Components components = Components;
				Material spaceMaterial2 = (Components.SpaceRenderer.sharedMaterial = Resources.SpaceMaterial);
				components.SpaceMaterial = spaceMaterial2;
			}
			if ((bool)Components.AtmosphereRenderer && Components.AtmosphereMaterial != Resources.AtmosphereMaterial)
			{
				TOD_Components components2 = Components;
				Material spaceMaterial2 = (Components.AtmosphereRenderer.sharedMaterial = Resources.AtmosphereMaterial);
				components2.AtmosphereMaterial = spaceMaterial2;
			}
			if ((bool)Components.ClearRenderer && Components.ClearMaterial != Resources.ClearMaterial)
			{
				TOD_Components components3 = Components;
				Material spaceMaterial2 = (Components.ClearRenderer.sharedMaterial = Resources.ClearMaterial);
				components3.ClearMaterial = spaceMaterial2;
			}
			if ((bool)Components.CloudRenderer && Components.CloudMaterial != Resources.CloudMaterial)
			{
				TOD_Components components4 = Components;
				Material spaceMaterial2 = (Components.CloudRenderer.sharedMaterial = Resources.CloudMaterial);
				components4.CloudMaterial = spaceMaterial2;
			}
			if ((bool)Components.ShadowProjector && Components.ShadowMaterial != Resources.ShadowMaterial)
			{
				TOD_Components components5 = Components;
				Material spaceMaterial2 = (Components.ShadowProjector.material = Resources.ShadowMaterial);
				components5.ShadowMaterial = spaceMaterial2;
			}
			if ((bool)Components.SunRenderer && Components.SunMaterial != Resources.SunMaterial)
			{
				TOD_Components components6 = Components;
				Material spaceMaterial2 = (Components.SunRenderer.sharedMaterial = Resources.SunMaterial);
				components6.SunMaterial = spaceMaterial2;
			}
			if ((bool)Components.MoonRenderer && Components.MoonMaterial != Resources.MoonMaterial)
			{
				TOD_Components components7 = Components;
				Material spaceMaterial2 = (Components.MoonRenderer.sharedMaterial = Resources.MoonMaterial);
				components7.MoonMaterial = spaceMaterial2;
			}
			if ((bool)Components.SpaceMeshFilter && Components.SpaceMeshFilter.sharedMesh != mesh)
			{
				Components.SpaceMeshFilter.mesh = mesh;
			}
			if ((bool)Components.AtmosphereMeshFilter && Components.AtmosphereMeshFilter.sharedMesh != mesh2)
			{
				Components.AtmosphereMeshFilter.mesh = mesh2;
			}
			if ((bool)Components.ClearMeshFilter && Components.ClearMeshFilter.sharedMesh != mesh3)
			{
				Components.ClearMeshFilter.mesh = mesh3;
			}
			if ((bool)Components.CloudMeshFilter && Components.CloudMeshFilter.sharedMesh != mesh4)
			{
				Components.CloudMeshFilter.mesh = mesh4;
			}
			if ((bool)Components.SunMeshFilter && Components.SunMeshFilter.sharedMesh != mesh5)
			{
				Components.SunMeshFilter.mesh = mesh5;
			}
			if ((bool)Components.MoonMeshFilter && Components.MoonMeshFilter.sharedMesh != mesh6)
			{
				Components.MoonMeshFilter.mesh = mesh6;
			}
		}
	}

	private void _E001()
	{
		if (!Headless)
		{
			UpdateFog();
			if (!Application.isPlaying || _E01F >= Ambient.UpdateInterval)
			{
				_E01F = 0f;
				UpdateAmbient();
			}
			else
			{
				_E01F += Time.deltaTime;
			}
			if (!Application.isPlaying || _E020 >= Reflection.UpdateInterval)
			{
				_E020 = 0f;
				UpdateReflection();
			}
			else
			{
				_E020 += Time.deltaTime;
			}
		}
	}

	private void _E002()
	{
		if (!Headless)
		{
			if ((bool)Resources.CloudMaterial)
			{
				_E007(Resources.CloudMaterial);
				_E004(Resources.CloudMaterial);
				_E005(Resources.CloudMaterial);
			}
			if ((bool)Resources.BillboardMaterial)
			{
				_E007(Resources.BillboardMaterial);
				_E004(Resources.BillboardMaterial);
				_E005(Resources.BillboardMaterial);
			}
			if ((bool)Resources.ShadowMaterial)
			{
				_E007(Resources.ShadowMaterial);
			}
			if ((bool)Resources.AtmosphereMaterial)
			{
				_E006(Resources.AtmosphereMaterial);
				_E004(Resources.AtmosphereMaterial);
				_E005(Resources.AtmosphereMaterial);
			}
			if ((bool)Resources.SkyboxMaterial)
			{
				_E004(Resources.SkyboxMaterial);
				_E005(Resources.SkyboxMaterial);
			}
		}
	}

	private void _E003()
	{
		if (!Headless)
		{
			Vector4 value = Components.Animation.CloudUV + Components.Animation.OffsetUV;
			Vector4 value2 = new Vector4(Clouds.Scale1.x, Clouds.Scale1.y, Clouds.Scale2.x, Clouds.Scale2.y);
			float value3 = Clouds.ShadowStrength * Mathf.Clamp01(1f - LightZenith / 90f);
			Shader.SetGlobalColor(Resources.ID_SunSkyColor, SunSkyColor);
			Shader.SetGlobalColor(Resources.ID_MoonSkyColor, MoonSkyColor);
			Shader.SetGlobalColor(Resources.ID_SunCloudColor, CloudColor * SunLightColor);
			Shader.SetGlobalColor(Resources.ID_MoonCloudColor, CloudColor * MoonLightColor);
			Shader.SetGlobalColor(Resources.ID_SunLightColor, SunLightColor);
			Shader.SetGlobalColor(Resources.ID_MoonLightColor, MoonLightColor);
			Shader.SetGlobalColor(Resources.ID_SunMeshColor, SunMeshColor);
			Shader.SetGlobalColor(Resources.ID_MoonMeshColor, MoonMeshColor);
			Shader.SetGlobalColor(Resources.ID_CloudColor, CloudColor);
			Shader.SetGlobalColor(Resources.ID_GroundColor, GroundColor);
			Shader.SetGlobalColor(Resources.ID_AmbientColor, AmbientColor);
			Shader.SetGlobalColor(Resources.ID_MoonHaloColor, MoonHaloColor);
			Shader.SetGlobalVector(Resources.ID_SunDirection, SunDirection);
			Shader.SetGlobalVector(Resources.ID_MoonDirection, MoonDirection);
			Shader.SetGlobalVector(Resources.ID_LightDirection, LightDirection);
			Shader.SetGlobalVector(Resources.ID_LocalSunDirection, LocalSunDirection);
			Shader.SetGlobalVector(Resources.ID_LocalMoonDirection, LocalMoonDirection);
			Shader.SetGlobalVector(Resources.ID_LocalLightDirection, LocalLightDirection);
			Shader.SetGlobalFloat(Resources.ID_Contrast, Atmosphere.Contrast);
			Shader.SetGlobalFloat(Resources.ID_Brightness, Atmosphere.Brightness);
			Shader.SetGlobalFloat(Resources.ID_ScatteringBrightness, Atmosphere.ScatteringBrightness);
			Shader.SetGlobalFloat(Resources.ID_Fogginess, Atmosphere.Fogginess);
			Shader.SetGlobalFloat(Resources.ID_Directionality, Atmosphere.Directionality);
			Shader.SetGlobalFloat(Resources.ID_MoonHaloPower, 1f / Moon.HaloSize);
			Shader.SetGlobalFloat(Resources.ID_CloudDensity, Clouds.Density);
			Shader.SetGlobalFloat(Resources.ID_CloudSharpness, Clouds.Sharpness);
			Shader.SetGlobalFloat(Resources.ID_CloudShadow, value3);
			Shader.SetGlobalVector(Resources.ID_CloudScale, value2);
			Shader.SetGlobalVector(Resources.ID_CloudUV, value);
			Shader.SetGlobalFloat(Resources.ID_SpaceTiling, Stars.Tiling);
			Shader.SetGlobalFloat(Resources.ID_SpaceBrightness, Stars.Brightness * (1f - Atmosphere.Fogginess) * (1f - LerpValue));
			Shader.SetGlobalFloat(Resources.ID_SunMeshContrast, 1f / Mathf.Max(0.001f, Sun.MeshContrast));
			Shader.SetGlobalFloat(Resources.ID_SunMeshBrightness, Sun.MeshBrightness * (1f - Atmosphere.Fogginess));
			Shader.SetGlobalFloat(Resources.ID_MoonMeshContrast, 1f / Mathf.Max(0.001f, Moon.MeshContrast));
			Shader.SetGlobalFloat(Resources.ID_MoonMeshBrightness, Moon.MeshBrightness * (1f - Atmosphere.Fogginess));
			Shader.SetGlobalVector(Resources.ID_kBetaMie, _E024);
			Shader.SetGlobalVector(Resources.ID_kSun, _E025);
			Shader.SetGlobalVector(Resources.ID_k4PI, _E026);
			Shader.SetGlobalVector(Resources.ID_kRadius, _E027);
			Shader.SetGlobalVector(Resources.ID_kScale, _E028);
			Shader.SetGlobalMatrix(Resources.ID_World2Sky, Components.DomeTransform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(Resources.ID_Sky2World, Components.DomeTransform.localToWorldMatrix);
			if ((bool)Components.ShadowProjector)
			{
				float farClipPlane = Radius * 2f;
				float radius = Radius;
				Components.ShadowProjector.farClipPlane = farClipPlane;
				Components.ShadowProjector.orthographicSize = radius;
			}
		}
	}

	private void _E004(Material material)
	{
		switch (ColorSpace)
		{
		case TOD_ColorSpaceType.Auto:
			if (QualitySettings.activeColorSpace == UnityEngine.ColorSpace.Linear)
			{
				material.EnableKeyword(_ED3E._E000(18649));
				material.DisableKeyword(_ED3E._E000(18640));
			}
			else
			{
				material.DisableKeyword(_ED3E._E000(18649));
				material.EnableKeyword(_ED3E._E000(18640));
			}
			break;
		case TOD_ColorSpaceType.Linear:
			material.EnableKeyword(_ED3E._E000(18649));
			material.DisableKeyword(_ED3E._E000(18640));
			break;
		case TOD_ColorSpaceType.Gamma:
			material.DisableKeyword(_ED3E._E000(18649));
			material.EnableKeyword(_ED3E._E000(18640));
			break;
		}
	}

	private void _E005(Material material)
	{
		switch (ColorRange)
		{
		case TOD_ColorRangeType.Auto:
			if ((bool)Components.Camera && Components.Camera.HDR)
			{
				material.EnableKeyword(_ED3E._E000(18638));
				material.DisableKeyword(_ED3E._E000(18634));
			}
			else
			{
				material.DisableKeyword(_ED3E._E000(18638));
				material.EnableKeyword(_ED3E._E000(18634));
			}
			break;
		case TOD_ColorRangeType.HDR:
			material.EnableKeyword(_ED3E._E000(18638));
			material.DisableKeyword(_ED3E._E000(18634));
			break;
		case TOD_ColorRangeType.LDR:
			material.DisableKeyword(_ED3E._E000(18638));
			material.EnableKeyword(_ED3E._E000(18634));
			break;
		}
	}

	private void _E006(Material material)
	{
		switch (SkyQuality)
		{
		case TOD_SkyQualityType.PerVertex:
			material.EnableKeyword(_ED3E._E000(18630));
			material.DisableKeyword(_ED3E._E000(18681));
			break;
		case TOD_SkyQualityType.PerPixel:
			material.DisableKeyword(_ED3E._E000(18630));
			material.EnableKeyword(_ED3E._E000(18681));
			break;
		}
	}

	private void _E007(Material material)
	{
		switch (CloudQuality)
		{
		case TOD_CloudQualityType.Fastest:
			material.EnableKeyword(_ED3E._E000(18675));
			material.DisableKeyword(_ED3E._E000(18667));
			material.DisableKeyword(_ED3E._E000(18659));
			break;
		case TOD_CloudQualityType.Density:
			material.DisableKeyword(_ED3E._E000(18675));
			material.EnableKeyword(_ED3E._E000(18667));
			material.DisableKeyword(_ED3E._E000(18659));
			break;
		case TOD_CloudQualityType.Bumped:
			material.DisableKeyword(_ED3E._E000(18675));
			material.DisableKeyword(_ED3E._E000(18667));
			material.EnableKeyword(_ED3E._E000(18659));
			break;
		}
	}

	private float _E008(float inCos)
	{
		float num = 1f - inCos;
		return 0.25f * Mathf.Exp(-0.00287f + num * (0.459f + num * (3.83f + num * (-6.8f + num * 5.25f))));
	}

	private float _E009(float eyeCos, float eyeCos2)
	{
		return _E024.x * (1f + eyeCos2) / Mathf.Pow(_E024.y + _E024.z * eyeCos, 1.5f);
	}

	private float _E00A(float eyeCos2)
	{
		return 0.75f + 0.75f * eyeCos2;
	}

	private Color _E00B(Vector3 dir)
	{
		return Color.Lerp(MoonSkyColor, Color.black, dir.y);
	}

	private Color _E00C(Vector3 dir)
	{
		return MoonHaloColor * Mathf.Pow(Mathf.Max(0f, Vector3.Dot(dir, LocalMoonDirection)), 1f / Moon.HaloSize);
	}

	private Color _E00D(Color color)
	{
		return new Color(1f - Mathf.Pow(2f, (0f - Atmosphere.Brightness) * color.r), 1f - Mathf.Pow(2f, (0f - Atmosphere.Brightness) * color.g), 1f - Mathf.Pow(2f, (0f - Atmosphere.Brightness) * color.b), color.a);
	}

	private Color _E00E(Color color)
	{
		return new Color(color.r * color.r, color.g * color.g, color.b * color.b, color.a);
	}

	private Color _E00F(Color color)
	{
		return new Color(Mathf.Sqrt(color.r), Mathf.Sqrt(color.g), Mathf.Sqrt(color.b), color.a);
	}

	private Color _E010(Vector3 dir, bool directLight = true)
	{
		dir.y = Mathf.Clamp01(dir.y);
		float x = _E027.x;
		float y = _E027.y;
		float w = _E027.w;
		float x2 = _E028.x;
		float z = _E028.z;
		float w2 = _E028.w;
		float x3 = _E026.x;
		float y2 = _E026.y;
		float z2 = _E026.z;
		float w3 = _E026.w;
		float x4 = _E025.x;
		float y3 = _E025.y;
		float z3 = _E025.z;
		float w4 = _E025.w;
		Vector3 rhs = new Vector3(0f, x + w2, 0f);
		float num = Mathf.Sqrt(w + y * dir.y * dir.y - y) - x * dir.y;
		float num2 = Mathf.Exp(z * (0f - w2));
		float inCos = Vector3.Dot(dir, rhs) / (x + w2);
		float num3 = num2 * _E008(inCos);
		float num4 = num / 2f;
		float num5 = num4 * x2;
		Vector3 vector = dir * num4;
		Vector3 rhs2 = new Vector3(rhs.x + vector.x * 0.5f, rhs.y + vector.y * 0.5f, rhs.z + vector.z * 0.5f);
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		for (int i = 0; i < 2; i++)
		{
			float magnitude = rhs2.magnitude;
			float num9 = 1f / magnitude;
			float num10 = Mathf.Exp(z * (x - magnitude));
			float num11 = num10 * num5;
			float inCos2 = Vector3.Dot(dir, rhs2) * num9;
			float inCos3 = Vector3.Dot(LocalSunDirection, rhs2) * num9;
			float num12 = num3 + num10 * (_E008(inCos3) - _E008(inCos2));
			float num13 = Mathf.Exp((0f - num12) * (x3 + w3));
			float num14 = Mathf.Exp((0f - num12) * (y2 + w3));
			float num15 = Mathf.Exp((0f - num12) * (z2 + w3));
			num6 += num13 * num11;
			num7 += num14 * num11;
			num8 += num15 * num11;
			rhs2.x += vector.x;
			rhs2.y += vector.y;
			rhs2.z += vector.z;
		}
		float num16 = SunSkyColor.r * num6 * x4;
		float num17 = SunSkyColor.g * num7 * y3;
		float num18 = SunSkyColor.b * num8 * z3;
		float num19 = SunSkyColor.r * num6 * w4;
		float num20 = SunSkyColor.g * num7 * w4;
		float num21 = SunSkyColor.b * num8 * w4;
		float num22 = 0f;
		float num23 = 0f;
		float num24 = 0f;
		float num25 = Vector3.Dot(LocalSunDirection, dir);
		float eyeCos = num25 * num25;
		float num26 = _E00A(eyeCos);
		num22 += num26 * num16;
		num23 += num26 * num17;
		num24 += num26 * num18;
		if (directLight)
		{
			float num27 = _E009(num25, eyeCos);
			num22 += num27 * num19;
			num23 += num27 * num20;
			num24 += num27 * num21;
		}
		Color color = _E00B(dir);
		num22 += color.r;
		num23 += color.g;
		num24 += color.b;
		if (directLight)
		{
			Color color2 = _E00C(dir);
			num22 += color2.r;
			num23 += color2.g;
			num24 += color2.b;
		}
		num22 = Mathf.Lerp(num22, CloudColor.r, Atmosphere.Fogginess);
		num23 = Mathf.Lerp(num23, CloudColor.g, Atmosphere.Fogginess);
		num24 = Mathf.Lerp(num24, CloudColor.b, Atmosphere.Fogginess);
		num22 = Mathf.Lerp(num22, AmbientColor.r, 0.5f);
		num23 = Mathf.Lerp(num23, AmbientColor.g, 0.5f);
		num24 = Mathf.Lerp(num24, AmbientColor.b, 0.5f);
		num22 = Mathf.Pow(num22 * Atmosphere.Brightness, Atmosphere.Contrast);
		num23 = Mathf.Pow(num23 * Atmosphere.Brightness, Atmosphere.Contrast);
		num24 = Mathf.Pow(num24 * Atmosphere.Brightness, Atmosphere.Contrast);
		return new Color(num22, num23, num24, 1f);
	}

	protected void OnEnable()
	{
		Components = GetComponent<TOD_Components>();
		Components.Initialize();
		Resources = GetComponent<TOD_Resources>();
		Resources.Initialize();
		LateUpdate();
		TOD_Sky.m__E000.Add(this);
		Initialized = true;
	}

	protected void OnDisable()
	{
		TOD_Sky.m__E000.Remove(this);
		if ((bool)Probe)
		{
			UnityEngine.Object.Destroy(Probe.gameObject);
		}
	}

	protected void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		Vector2 mainTextureScale = Resources.BillboardMaterial.mainTextureScale;
		int num = Mathf.RoundToInt(1f / mainTextureScale.x);
		int num2 = Mathf.RoundToInt(1f / mainTextureScale.y);
		Mesh[] array = new Mesh[2 * num * num2];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				array[i * num + j] = TOD_Resources.CreateQuad(new Vector2(j, i), new Vector2(j + 1, i + 1));
			}
		}
		for (int k = 0; k < num2; k++)
		{
			for (int l = 0; l < num; l++)
			{
				array[num * num2 + k * num + l] = TOD_Resources.CreateQuad(new Vector2(l + 1, k), new Vector2(l, k + 1));
			}
		}
		for (int m = 0; m < Clouds.Billboards; m++)
		{
			GameObject obj = new GameObject(_ED3E._E000(18714) + m);
			obj.transform.parent = Components.Billboards.transform;
			float num3 = UnityEngine.Random.Range(0.3f, 0.4f);
			obj.transform.localScale = new Vector3(num3, num3 * 0.5f, 1f);
			float f = (float)Math.PI * 2f * ((float)m / (float)Clouds.Billboards);
			obj.transform.localPosition = 0.95f * new Vector3(Mathf.Sin(f), UnityEngine.Random.Range(0.1f, 0.2f), Mathf.Cos(f)).normalized;
			obj.transform.LookAt(Components.DomeTransform.position);
			obj.AddComponent<MeshFilter>().sharedMesh = array[UnityEngine.Random.Range(0, array.Length)];
			obj.AddComponent<MeshRenderer>().sharedMaterial = Resources.BillboardMaterial;
		}
	}

	protected void LateUpdate()
	{
		_E011();
		_E012();
		_E000();
		_E001();
		_E002();
		_E003();
	}

	private void _E011()
	{
		float num = 0f - Atmosphere.Directionality;
		float num2 = num * num;
		_E024.x = 1.5f * ((1f - num2) / (2f + num2));
		_E024.y = 1f + num2;
		_E024.z = 2f * num;
		float num3 = 0.002f * Atmosphere.MieMultiplier;
		float num4 = 0.002f * Atmosphere.RayleighMultiplier;
		float x = num4 * 40f * 5.2701645f;
		float y = num4 * 40f * 9.473284f;
		float z = num4 * 40f * 19.643803f;
		float w = num3 * 40f;
		_E025.x = x;
		_E025.y = y;
		_E025.z = z;
		_E025.w = w;
		float x2 = num4 * 4f * (float)Math.PI * 5.2701645f;
		float y2 = num4 * 4f * (float)Math.PI * 9.473284f;
		float z2 = num4 * 4f * (float)Math.PI * 19.643803f;
		float w2 = num3 * 4f * (float)Math.PI;
		_E026.x = x2;
		_E026.y = y2;
		_E026.z = z2;
		_E026.w = w2;
		_E027.x = 1f;
		_E027.y = 1f;
		_E027.z = 1.025f;
		_E027.w = 1.050625f;
		_E028.x = 40.00004f;
		_E028.y = 0.25f;
		_E028.z = 160.00015f;
		_E028.w = 0.0001f;
	}

	public Vector3 LightDirectionExtrapolated(float addTime)
	{
		Vector3 vector = -Components.LightTransform.forward;
		float num = _E01E * rotationSpeed;
		float t = num + addTime * rotationSpeed;
		if (!Singleton<_E7DE>.Instantiated || (int)Singleton<_E7DE>.Instance.Graphics.Settings.ShadowsQuality < 3)
		{
			num = (Time.time - _E022) / (_E021 + 0.0001f);
			t = num + addTime / (_E021 + 0.0001f);
		}
		if ((double)num > 1.3)
		{
			return vector;
		}
		Quaternion rotation = Quaternion.Slerp(_E02F, _E030, num);
		return Quaternion.SlerpUnclamped(_E02F, _E030, t) * Quaternion.Inverse(rotation) * vector;
	}

	private void _E012()
	{
		_E01E += Time.deltaTime;
		if (!Singleton<_E7DE>.Instantiated || (int)Singleton<_E7DE>.Instance.Graphics.Settings.ShadowsQuality > 2)
		{
			Components.LightTransform.rotation = Quaternion.Slerp(_E02F, _E030, _E01E * rotationSpeed);
		}
		if (_E01E < Light.UpdateInterval && Application.isPlaying)
		{
			return;
		}
		float f = (float)Math.PI / 180f * World.Latitude;
		float num = Mathf.Sin(f);
		float num2 = Mathf.Cos(f);
		float longitude = World.Longitude;
		float num3 = (float)Math.PI / 2f;
		int year = Cycle.Year;
		int month = Cycle.Month;
		int day = Cycle.Day;
		float num4 = Cycle.Hour - World.UTC;
		float num5 = (float)(367 * year - 7 * (year + (month + 9) / 12) / 4 + 275 * month / 9 + day - 730530) + num4 / 24f;
		float num6 = 23.4393f - 3.563E-07f * num5;
		float f2 = (float)Math.PI / 180f * num6;
		float num7 = Mathf.Sin(f2);
		float num8 = Mathf.Cos(f2);
		float num9 = 282.9404f + 4.70935E-05f * num5;
		float num10 = 0.016709f - 1.151E-09f * num5;
		float num11 = 356.047f + 0.98560023f * num5;
		float num12 = (float)Math.PI / 180f * num11;
		float num13 = Mathf.Sin(num12);
		float num14 = Mathf.Cos(num12);
		float f3 = num12 + num10 * num13 * (1f + num10 * num14);
		float num15 = Mathf.Sin(f3);
		float num16 = Mathf.Cos(f3) - num10;
		float num17 = Mathf.Sqrt(1f - num10 * num10) * num15;
		float num18 = 57.29578f * Mathf.Atan2(num17, num16);
		float num19 = Mathf.Sqrt(num16 * num16 + num17 * num17);
		float num20 = num18 + num9;
		float f4 = (float)Math.PI / 180f * num20;
		float num21 = Mathf.Sin(f4);
		float num22 = Mathf.Cos(f4);
		float num23 = num19 * num22;
		float num24 = num19 * num21;
		float num25 = num23;
		float num26 = num24 * num8;
		float y = num24 * num7;
		float num27 = Mathf.Atan2(num26, num25);
		float f5 = Mathf.Atan2(y, Mathf.Sqrt(num25 * num25 + num26 * num26));
		float num28 = Mathf.Sin(f5);
		float num29 = Mathf.Cos(f5);
		float num30 = num18 + num9 + 180f + 15f * num4;
		float num31 = (float)Math.PI / 180f * (num30 + longitude);
		float f6 = num31 - num27;
		float num32 = Mathf.Sin(f6);
		float num33 = Mathf.Cos(f6) * num29;
		float num34 = num32 * num29;
		float num35 = num28;
		float num36 = num33 * num - num35 * num2;
		float num37 = num34;
		float y2 = num33 * num2 + num35 * num;
		float num38 = Mathf.Atan2(num37, num36) + (float)Math.PI;
		float num39 = Mathf.Atan2(y2, Mathf.Sqrt(num36 * num36 + num37 * num37));
		float num40 = num3 - num39;
		float num41 = 0f - num38;
		float num82;
		float num83;
		if (Moon.Position == TOD_MoonPositionType.Realistic)
		{
			float num42 = 125.1228f - 0.05295381f * num5;
			float num43 = 5.1454f;
			float num44 = 318.0634f + 0.16435732f * num5;
			float num45 = 0.0549f;
			float num46 = 115.3654f + 13.064993f * num5;
			float f7 = (float)Math.PI / 180f * num42;
			float num47 = Mathf.Sin(f7);
			float num48 = Mathf.Cos(f7);
			float f8 = (float)Math.PI / 180f * num43;
			float num49 = Mathf.Sin(f8);
			float num50 = Mathf.Cos(f8);
			float num51 = (float)Math.PI / 180f * num46;
			float num52 = Mathf.Sin(num51);
			float num53 = Mathf.Cos(num51);
			float f9 = num51 + num45 * num52 * (1f + num45 * num53);
			float num54 = Mathf.Sin(f9);
			float num55 = Mathf.Cos(f9);
			float num56 = 60.2666f * (num55 - num45);
			float num57 = 60.2666f * (Mathf.Sqrt(1f - num45 * num45) * num54);
			float num58 = 57.29578f * Mathf.Atan2(num57, num56);
			float num59 = Mathf.Sqrt(num56 * num56 + num57 * num57);
			float num60 = num58 + num44;
			float f10 = (float)Math.PI / 180f * num60;
			float num61 = Mathf.Sin(f10);
			float num62 = Mathf.Cos(f10);
			float num63 = num59 * (num48 * num62 - num47 * num61 * num50);
			float num64 = num59 * (num47 * num62 + num48 * num61 * num50);
			float num65 = num59 * (num61 * num49);
			float num66 = num63;
			float num67 = num64;
			float num68 = num65;
			float num69 = num66;
			float num70 = num67 * num8 - num68 * num7;
			float y3 = num67 * num7 + num68 * num8;
			float num71 = Mathf.Atan2(num70, num69);
			float f11 = Mathf.Atan2(y3, Mathf.Sqrt(num69 * num69 + num70 * num70));
			float num72 = Mathf.Sin(f11);
			float num73 = Mathf.Cos(f11);
			float f12 = num31 - num71;
			float num74 = Mathf.Sin(f12);
			float num75 = Mathf.Cos(f12) * num73;
			float num76 = num74 * num73;
			float num77 = num72;
			float num78 = num75 * num - num77 * num2;
			float num79 = num76;
			float y4 = num75 * num2 + num77 * num;
			float num80 = Mathf.Atan2(num79, num78) + (float)Math.PI;
			float num81 = Mathf.Atan2(y4, Mathf.Sqrt(num78 * num78 + num79 * num79));
			num82 = num3 - num81;
			num83 = 0f - num80;
		}
		else
		{
			num82 = num40 - (float)Math.PI;
			num83 = num41;
		}
		SunZenith = 57.29578f * num40;
		MoonZenith = 57.29578f * num82;
		Quaternion quaternion = Quaternion.Euler(90f - World.Latitude, 0f, 0f) * Quaternion.Euler(0f, World.Longitude, 0f) * Quaternion.Euler(0f, num31 * 57.29578f, 0f);
		if (Stars.Position == TOD_StarsPositionType.Rotating)
		{
			Components.SpaceTransform.localRotation = quaternion;
		}
		else
		{
			Components.SpaceTransform.localRotation = Quaternion.identity;
		}
		Vector3 localPosition = OrbitalToLocal(num40, num41);
		Components.SunTransform.localPosition = localPosition;
		Components.SunTransform.LookAt(Components.DomeTransform.position, Components.SunTransform.up);
		Vector3 localPosition2 = OrbitalToLocal(num82, num83);
		Vector3 worldUp = quaternion * -Vector3.right;
		Components.MoonTransform.localPosition = localPosition2;
		Components.MoonTransform.LookAt(Components.DomeTransform.position, worldUp);
		float num84 = 2f * Mathf.Tan((float)Math.PI / 90f * Sun.MeshSize);
		Vector3 localScale = new Vector3(num84, num84, num84);
		Components.SunTransform.localScale = localScale;
		float num85 = 2f * Mathf.Tan((float)Math.PI / 180f * Moon.MeshSize);
		Vector3 localScale2 = new Vector3(num85, num85, num85);
		Components.MoonTransform.localScale = localScale2;
		bool flag = Components.SunTransform.localPosition.y > 0f - num84;
		Components.SunRenderer.enabled = flag;
		bool flag2 = Components.MoonTransform.localPosition.y > 0f - num85;
		Components.MoonRenderer.enabled = flag2;
		bool flag3 = Clouds.Density > 0f;
		Components.CloudRenderer.enabled = flag3;
		bool flag4 = Components.ShadowMaterial != null && Clouds.ShadowStrength != 0f;
		Components.ShadowProjector.enabled = flag4;
		bool flag5 = true;
		Components.SpaceRenderer.enabled = flag5;
		bool flag6 = true;
		Components.AtmosphereRenderer.enabled = flag6;
		bool flag7 = Components.Rays != null;
		Components.ClearRenderer.enabled = flag7;
		LerpValue = Mathf.InverseLerp(110f, 80f, SunZenith);
		float time = 1f - LerpValue;
		float num86 = 1f - Atmosphere.Fogginess;
		float num87 = ((Moon.Position == TOD_MoonPositionType.Realistic) ? Mathf.Clamp01((90f - num82 * 57.29578f) / 5f) : Mathf.Clamp01((90f + num82 * 57.29578f) / 5f));
		float num88 = Mathf.Clamp01(num86 * (LerpValue - 0.1f) / 0.9f);
		float num89 = Mathf.Clamp01(num86 * num87 * (0.1f - LerpValue) / 0.1f);
		num89 = Mathf.Clamp01(num86 * num87);
		float multiplier = Day.ColorMultiplier * num88;
		SunLightColor = _E05B.MulRGB(Day.LightColor.Evaluate(time), multiplier);
		float multiplier2 = Night.ColorMultiplier * num89;
		MoonLightColor = _E05B.MulRGB(Night.LightColor.Evaluate(time), multiplier2);
		float multiplier3 = Day.ColorMultiplier * num88;
		SunRayColor = _E05B.MulRGB(Day.RayColor.Evaluate(time), multiplier3);
		float multiplier4 = 0.25f * Night.ColorMultiplier * num89;
		MoonRayColor = _E05B.MulRGB(Night.RayColor.Evaluate(time), multiplier4);
		float colorMultiplier = Day.ColorMultiplier;
		SunSkyColor = _E05B.MulRGB(Day.SkyColor.Evaluate(time), colorMultiplier);
		float multiplier5 = 0.25f * Night.ColorMultiplier;
		MoonSkyColor = _E05B.MulRGB(Night.SkyColor.Evaluate(time), multiplier5);
		float colorMultiplier2 = Day.ColorMultiplier;
		SunMeshColor = _E05B.MulRGB(Sun.MeshColor.Evaluate(time), colorMultiplier2);
		float colorMultiplier3 = Night.ColorMultiplier;
		MoonMeshColor = _E05B.MulRGB(Moon.MeshColor.Evaluate(time), colorMultiplier3);
		float multiplier6 = Day.ColorMultiplier * Clouds.Brightness;
		Color b = _E05B.MulRGB(Day.CloudColor.Evaluate(time), multiplier6);
		float multiplier7 = 0.25f * Night.ColorMultiplier * Clouds.Brightness;
		Color a = _E05B.MulRGB(Night.CloudColor.Evaluate(time), multiplier7);
		CloudColor = Color.Lerp(a, b, LerpValue);
		float colorMultiplier4 = Day.ColorMultiplier;
		Color b2 = _E05B.MulRGB(Day.AmbientColor.Evaluate(time), colorMultiplier4);
		float multiplier8 = 0.25f * Night.ColorMultiplier;
		Color a2 = _E05B.MulRGB(Night.AmbientColor.Evaluate(time), multiplier8);
		GroundColor = Color.Lerp(a2, b2, LerpValue);
		float colorMultiplier5 = Day.ColorMultiplier;
		Color b3 = _E05B.MulRGB(Day.AmbientColor.Evaluate(time), colorMultiplier5);
		float multiplier9 = 0.5f * Night.ColorMultiplier;
		Color a3 = _E05B.MulRGB(Night.AmbientColor.Evaluate(time), multiplier9);
		AmbientColor = Color.Lerp(a3, b3, LerpValue);
		float multiplier10 = 0.25f * Night.ColorMultiplier * num87;
		MoonHaloColor = _E05B.MulRGB(Moon.HaloColor.Evaluate(time), multiplier10);
		float shadowStrength;
		float intensity;
		if (LerpValue > 0.1f)
		{
			IsDay = true;
			IsNight = false;
			shadowStrength = Day.ShadowStrength;
			intensity = Mathf.Lerp(0f, Day.LightIntensity, num88);
			_ = SunLightColor;
			RayColor = SunRayColor;
		}
		else
		{
			IsDay = false;
			IsNight = true;
			shadowStrength = Night.ShadowStrength;
			intensity = Mathf.Lerp(0f, Night.LightIntensity, num89);
			_ = MoonLightColor;
			RayColor = MoonRayColor;
		}
		Components.LightSource.intensity = intensity;
		Components.LightSource.shadowStrength = shadowStrength;
		if (!Singleton<_E7DE>.Instantiated || (int)Singleton<_E7DE>.Instance.Graphics.Settings.ShadowsQuality < 3)
		{
			_E01E = 0f;
			Vector3 vector = (IsNight ? OrbitalToLocal(Mathf.Min(num82, (1f - Light.MinimumHeight) * (float)Math.PI / 2f), num83) : OrbitalToLocal(Mathf.Min(num40, (1f - Light.MinimumHeight) * (float)Math.PI / 2f), num41));
			Components.LightTransform.rotation = Quaternion.LookRotation(Components.DomeTransform.position - vector);
			if (Quaternion.Angle(_E030, Components.LightTransform.rotation) > 1f)
			{
				_E02F = _E030;
				_E030 = Components.LightTransform.rotation;
				_E021 = Time.time - _E022;
				_E022 = Time.time;
			}
		}
		else
		{
			Vector3 vector2 = (IsNight ? OrbitalToLocal(Mathf.Min(num82, (1f - Light.MinimumHeight) * (float)Math.PI / 2f), num83) : OrbitalToLocal(Mathf.Min(num40, (1f - Light.MinimumHeight) * (float)Math.PI / 2f), num41));
			Vector3 vector3 = (IsNight ? OrbitalToLocal(Mathf.Min(_E02D, (1f - Light.MinimumHeight) * (float)Math.PI / 2f), _E02E) : OrbitalToLocal(Mathf.Min(_E02B, (1f - Light.MinimumHeight) * (float)Math.PI / 2f), _E02C));
			if (_E02B != num40 || _E02C != num41 || _E02D != num82 || _E02E != num83)
			{
				_E02B = num40;
				_E02C = num41;
				_E02D = num82;
				_E02E = num83;
				_E030 = Quaternion.LookRotation(Components.DomeTransform.position - vector2);
				_E02F = Quaternion.LookRotation(Components.DomeTransform.position - vector3);
				rotationSpeed = 1f / Mathf.Max(_E01E, 0.001f);
				_E01E = 0f;
			}
		}
		SunDirection = -Components.SunTransform.forward;
		LocalSunDirection = Components.DomeTransform.InverseTransformDirection(SunDirection);
		MoonDirection = -Components.MoonTransform.forward;
		LocalMoonDirection = Components.DomeTransform.InverseTransformDirection(MoonDirection);
		LightDirection = Vector3.Lerp(MoonDirection, SunDirection, LerpValue * LerpValue);
		LocalLightDirection = Components.DomeTransform.InverseTransformDirection(LightDirection);
	}
}
