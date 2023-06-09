using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class VolumetricFog : MonoBehaviour
{
	private struct _E000
	{
		public int x;

		public int y;

		public int z;

		public _E000(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	private struct _E001
	{
		public Vector3 pos;

		public float range;

		public Vector3 color;

		private float _E000;
	}

	private struct _E002
	{
		public Vector3 start;

		public float range;

		public Vector3 end;

		public float radius;

		public Vector3 color;

		private float _E000;
	}

	private struct _E003
	{
		public Matrix4x4 mat;

		public Vector4 pos;

		public Vector3 color;

		public float bounded;
	}

	private struct _E004
	{
		public Vector3 pos;

		public float radius;

		public Vector3 axis;

		public float stretch;

		public float density;

		public float noiseAmount;

		public float noiseSpeed;

		public float noiseScale;

		public float feather;

		public float blend;

		public float padding1;

		public float padding2;
	}

	private Material m__E000;

	[HideInInspector]
	public Shader m_DebugShader;

	[HideInInspector]
	public Shader m_ShadowmapShader;

	[HideInInspector]
	public ComputeShader m_InjectLightingAndDensity;

	[HideInInspector]
	public ComputeShader m_Scatter;

	private Material m__E001;

	[HideInInspector]
	public Shader m_ApplyToOpaqueShader;

	private Material m__E002;

	[HideInInspector]
	public Shader m_BlurShadowmapShader;

	[HideInInspector]
	public Texture2D m_Noise;

	[HideInInspector]
	public bool m_Debug;

	[HideInInspector]
	[Range(0f, 1f)]
	public float m_Z = 1f;

	[Header("Size")]
	[_E3FB(0.1f)]
	public float m_NearClip = 0.1f;

	[_E3FB(0.1f)]
	public float m_FarClipMax = 100f;

	[Header("Fog Density")]
	[FormerlySerializedAs("m_Density")]
	public float m_GlobalDensityMult = 1f;

	private _E000 m__E003 = new _E000(16, 2, 16);

	private _E000 m__E004 = new _E000(32, 2, 1);

	private RenderTexture m__E005;

	private RenderTexture m__E006;

	private _E000 m__E007 = new _E000(160, 90, 128);

	private Camera m__E008;

	public float m_ConstantFog;

	public float m_HeightFogAmount;

	public float m_HeightFogExponent;

	public float m_HeightFogOffset;

	[Tooltip("Noise multiplies with constant fog and height fog, but not with fog ellipsoids.")]
	[Range(0f, 1f)]
	public float m_NoiseFogAmount;

	public float m_NoiseFogScale = 1f;

	public Wind m_Wind;

	[Range(0f, 0.999f)]
	public float m_Anisotropy;

	[Header("Lights")]
	[FormerlySerializedAs("m_Intensity")]
	public float m_GlobalIntensityMult = 1f;

	[_E3FB(0f)]
	public float m_AmbientLightIntensity;

	public Color m_AmbientLightColor = Color.white;

	private _E001[] m__E009;

	private ComputeBuffer m__E00A;

	private _E002[] m__E00B;

	private ComputeBuffer m__E00C;

	private _E003[] m__E00D;

	private ComputeBuffer m__E00E;

	private _E004[] m__E00F;

	private ComputeBuffer m__E010;

	private ComputeBuffer m__E011;

	private FogLight m__E012;

	private float[] m__E013;

	private float[] m__E014;

	private float[] _E015;

	private float[] _E016;

	private float[] _E017;

	private static readonly Vector2[] _E018 = new Vector2[4]
	{
		new Vector2(0f, 0f),
		new Vector2(1f, 0f),
		new Vector2(1f, 1f),
		new Vector2(0f, 1f)
	};

	private static float[] _E019 = new float[16];

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(42947));

	private static readonly int _E01B = Shader.PropertyToID(_ED3E._E000(41035));

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(41232));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(19728));

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(41235));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(41277));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(41254));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(40996));

	private Camera _E022
	{
		get
		{
			if (this.m__E008 == null)
			{
				this.m__E008 = GetComponent<Camera>();
			}
			return this.m__E008;
		}
	}

	private float _E023 => Mathf.Max(0f, m_NearClip);

	private float _E024 => Mathf.Min(_E022.farClipPlane, m_FarClipMax);

	private void _E000(ref ComputeBuffer buffer)
	{
		if (buffer != null)
		{
			buffer.Release();
		}
		buffer = null;
	}

	private void OnDestroy()
	{
		_E001();
	}

	private void OnDisable()
	{
		_E001();
	}

	private void _E001()
	{
		Object.DestroyImmediate(this.m__E005);
		Object.DestroyImmediate(this.m__E006);
		_E000(ref this.m__E00A);
		_E000(ref this.m__E00C);
		_E000(ref this.m__E00E);
		_E000(ref this.m__E010);
		_E000(ref this.m__E011);
		this.m__E005 = null;
		this.m__E006 = null;
	}

	private void _E002()
	{
		m_GlobalDensityMult = Mathf.Max(m_GlobalDensityMult, 0f);
		m_ConstantFog = Mathf.Max(m_ConstantFog, 0f);
		m_HeightFogAmount = Mathf.Max(m_HeightFogAmount, 0f);
	}

	private void _E003(int kernel)
	{
		int num = ((this.m__E00A != null) ? this.m__E00A.count : 0);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(42725), num);
		if (num == 0)
		{
			m_InjectLightingAndDensity.SetBuffer(kernel, _ED3E._E000(42775), this.m__E011);
			return;
		}
		if (this.m__E009 == null || this.m__E009.Length != num)
		{
			this.m__E009 = new _E001[num];
		}
		HashSet<FogLight> hashSet = LightManager<FogLight>.Get();
		int num2 = 0;
		HashSet<FogLight>.Enumerator enumerator = hashSet.GetEnumerator();
		while (enumerator.MoveNext())
		{
			FogLight current = enumerator.Current;
			if (!(current == null) && current.type == LightOverride.Type.Point && current.isOn)
			{
				Light light = current.light;
				this.m__E009[num2].pos = light.transform.position;
				float num3 = light.range * current.m_RangeMult;
				this.m__E009[num2].range = 1f / (num3 * num3);
				this.m__E009[num2].color = new Vector3(light.color.r, light.color.g, light.color.b) * light.intensity * current.m_IntensityMult;
				num2++;
			}
		}
		this.m__E00A.SetData(this.m__E009);
		m_InjectLightingAndDensity.SetBuffer(kernel, _ED3E._E000(42775), this.m__E00A);
	}

	private void _E004(int kernel)
	{
		int num = ((this.m__E00C != null) ? this.m__E00C.count : 0);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(42756), num);
		if (num == 0)
		{
			m_InjectLightingAndDensity.SetBuffer(kernel, _ED3E._E000(42805), this.m__E011);
			return;
		}
		if (this.m__E00B == null || this.m__E00B.Length != num)
		{
			this.m__E00B = new _E002[num];
		}
		HashSet<FogLight> hashSet = LightManager<FogLight>.Get();
		int num2 = 0;
		HashSet<FogLight>.Enumerator enumerator = hashSet.GetEnumerator();
		while (enumerator.MoveNext())
		{
			FogLight current = enumerator.Current;
			if (!(current == null) && current.type == LightOverride.Type.Tube && current.isOn)
			{
				TubeLight tubeLight = current.tubeLight;
				Transform transform = tubeLight.transform;
				Vector3 position = transform.position;
				Vector3 vector = 0.5f * transform.up * tubeLight.m_Length;
				this.m__E00B[num2].start = position + vector;
				this.m__E00B[num2].end = position - vector;
				float num3 = tubeLight.m_Range * current.m_RangeMult;
				this.m__E00B[num2].range = 1f / (num3 * num3);
				this.m__E00B[num2].color = new Vector3(tubeLight.m_Color.r, tubeLight.m_Color.g, tubeLight.m_Color.b) * tubeLight.m_Intensity * current.m_IntensityMult;
				this.m__E00B[num2].radius = tubeLight.m_Radius;
				num2++;
			}
		}
		this.m__E00C.SetData(this.m__E00B);
		m_InjectLightingAndDensity.SetBuffer(kernel, _ED3E._E000(42805), this.m__E00C);
	}

	private void _E005(int kernel)
	{
		int num = ((this.m__E00E != null) ? this.m__E00E.count : 0);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(42793), num);
		if (num == 0)
		{
			m_InjectLightingAndDensity.SetBuffer(kernel, _ED3E._E000(42842), this.m__E011);
			m_InjectLightingAndDensity.SetTexture(kernel, _ED3E._E000(42830), Texture2D.whiteTexture);
			return;
		}
		if (this.m__E00D == null || this.m__E00D.Length != num)
		{
			this.m__E00D = new _E003[num];
		}
		HashSet<FogLight> hashSet = LightManager<FogLight>.Get();
		int num2 = -1;
		int num3 = 0;
		HashSet<FogLight>.Enumerator enumerator = hashSet.GetEnumerator();
		while (enumerator.MoveNext())
		{
			FogLight current = enumerator.Current;
			if (current == null || current.type != LightOverride.Type.Area || !current.isOn)
			{
				continue;
			}
			AreaLight areaLight = current.areaLight;
			this.m__E00D[num3].mat = areaLight.GetProjectionMatrix(linearZ: true);
			this.m__E00D[num3].pos = areaLight.GetPosition();
			this.m__E00D[num3].color = new Vector3(areaLight.m_Color.r, areaLight.m_Color.g, areaLight.m_Color.b) * areaLight.m_Intensity * current.m_IntensityMult;
			this.m__E00D[num3].bounded = (current.m_Bounded ? 1 : 0);
			if (current.m_Shadows)
			{
				RenderTexture blurredShadowmap = areaLight.GetBlurredShadowmap();
				if (blurredShadowmap != null)
				{
					m_InjectLightingAndDensity.SetTexture(kernel, _ED3E._E000(42830), blurredShadowmap);
					m_InjectLightingAndDensity.SetFloat(_ED3E._E000(42874), current.m_ESMExponent);
					num2 = num3;
				}
			}
			num3++;
		}
		this.m__E00E.SetData(this.m__E00D);
		m_InjectLightingAndDensity.SetBuffer(kernel, _ED3E._E000(42842), this.m__E00E);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(42848), (num2 < 0) ? hashSet.Count : num2);
		if (num2 < 0)
		{
			m_InjectLightingAndDensity.SetTexture(kernel, _ED3E._E000(42830), Texture2D.whiteTexture);
		}
	}

	private void _E006(int kernel)
	{
		int num = 0;
		HashSet<FogEllipsoid> hashSet = LightManager<FogEllipsoid>.Get();
		HashSet<FogEllipsoid>.Enumerator enumerator = hashSet.GetEnumerator();
		while (enumerator.MoveNext())
		{
			FogEllipsoid current = enumerator.Current;
			if (current != null && current.enabled && current.gameObject.activeSelf)
			{
				num++;
			}
		}
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(42888), num);
		if (num == 0)
		{
			m_InjectLightingAndDensity.SetBuffer(kernel, _ED3E._E000(42932), this.m__E011);
			return;
		}
		if (this.m__E00F == null || this.m__E00F.Length != num)
		{
			this.m__E00F = new _E004[num];
		}
		int num2 = 0;
		HashSet<FogEllipsoid>.Enumerator enumerator2 = hashSet.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			FogEllipsoid current2 = enumerator2.Current;
			if (!(current2 == null) && current2.enabled && current2.gameObject.activeSelf)
			{
				Transform transform = current2.transform;
				this.m__E00F[num2].pos = transform.position;
				this.m__E00F[num2].radius = current2.m_Radius * current2.m_Radius;
				this.m__E00F[num2].axis = -transform.up;
				this.m__E00F[num2].stretch = 1f / current2.m_Stretch - 1f;
				this.m__E00F[num2].density = current2.m_Density;
				this.m__E00F[num2].noiseAmount = current2.m_NoiseAmount;
				this.m__E00F[num2].noiseSpeed = current2.m_NoiseSpeed;
				this.m__E00F[num2].noiseScale = current2.m_NoiseScale;
				this.m__E00F[num2].feather = 1f - current2.m_Feather;
				this.m__E00F[num2].blend = ((current2.m_Blend != 0) ? 1 : 0);
				num2++;
			}
		}
		this.m__E010.SetData(this.m__E00F);
		m_InjectLightingAndDensity.SetBuffer(kernel, _ED3E._E000(42932), this.m__E010);
	}

	private FogLight _E007()
	{
		HashSet<FogLight> hashSet = LightManager<FogLight>.Get();
		FogLight result = null;
		HashSet<FogLight>.Enumerator enumerator = hashSet.GetEnumerator();
		while (enumerator.MoveNext())
		{
			FogLight current = enumerator.Current;
			if (current == null || current.type != LightOverride.Type.Directional || !current.isOn)
			{
				continue;
			}
			result = current;
			break;
		}
		return result;
	}

	private void OnPreRender()
	{
		this.m__E012 = _E007();
		if (this.m__E012 != null)
		{
			this.m__E012.UpdateDirectionalShadowmap();
		}
	}

	private void _E008(int kernel)
	{
		if (this.m__E013 == null || this.m__E013.Length != 3)
		{
			this.m__E013 = new float[3];
		}
		if (this.m__E014 == null || this.m__E014.Length != 3)
		{
			this.m__E014 = new float[3];
		}
		if (this.m__E012 == null)
		{
			this.m__E013[0] = 0f;
			this.m__E013[1] = 0f;
			this.m__E013[2] = 0f;
			m_InjectLightingAndDensity.SetFloats(_ED3E._E000(42923), this.m__E013);
			return;
		}
		this.m__E012.SetUpDirectionalShadowmapForSampling(this.m__E012.m_Shadows, m_InjectLightingAndDensity, kernel);
		Light light = this.m__E012.light;
		Vector4 vector = light.color;
		vector *= light.intensity * this.m__E012.m_IntensityMult;
		this.m__E013[0] = vector.x;
		this.m__E013[1] = vector.y;
		this.m__E013[2] = vector.z;
		m_InjectLightingAndDensity.SetFloats(_ED3E._E000(42923), this.m__E013);
		Vector3 forward = light.GetComponent<Transform>().forward;
		this.m__E014[0] = forward.x;
		this.m__E014[1] = forward.y;
		this.m__E014[2] = forward.z;
		m_InjectLightingAndDensity.SetFloats(_ED3E._E000(42970), this.m__E014);
	}

	private void _E009(int kernel)
	{
		_E002();
		_E012();
		_E00F();
		float num = (_E024 - _E023) * 0.01f;
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(16719), m_GlobalDensityMult * 0.128f * num);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(35970), m_GlobalIntensityMult);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(42959), m_Anisotropy);
		m_InjectLightingAndDensity.SetTexture(kernel, _ED3E._E000(42947), this.m__E005);
		m_InjectLightingAndDensity.SetTexture(kernel, _ED3E._E000(42993), m_Noise);
		if (_E015 == null || _E015.Length != 4)
		{
			_E015 = new float[4];
		}
		if (_E016 == null || _E016.Length != 3)
		{
			_E016 = new float[3];
		}
		if (_E017 == null || _E017.Length != 3)
		{
			_E017 = new float[3];
		}
		_E015[0] = m_ConstantFog;
		_E015[1] = m_HeightFogExponent;
		_E015[2] = m_HeightFogOffset;
		_E015[3] = m_HeightFogAmount;
		m_InjectLightingAndDensity.SetFloats(_ED3E._E000(42984), _E015);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(42979), m_NoiseFogAmount);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(40979), m_NoiseFogScale);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(40962), (m_Wind == null) ? 0f : m_Wind.m_Speed);
		Vector3 vector = ((m_Wind == null) ? Vector3.forward : m_Wind.transform.forward);
		_E016[0] = vector.x;
		_E016[1] = vector.y;
		_E016[2] = vector.z;
		m_InjectLightingAndDensity.SetFloats(_ED3E._E000(41013), _E016);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(41006), Time.time);
		m_InjectLightingAndDensity.SetFloat(_ED3E._E000(40996), _E023 / _E024);
		Color color = m_AmbientLightColor * m_AmbientLightIntensity * 0.1f;
		_E017[0] = color.r;
		_E017[1] = color.g;
		_E017[2] = color.b;
		m_InjectLightingAndDensity.SetFloats(_ED3E._E000(41045), _E017);
		_E003(kernel);
		_E004(kernel);
		_E005(kernel);
		_E008(kernel);
	}

	private void _E00A()
	{
		int num = 0;
		_E009(num);
		m_InjectLightingAndDensity.Dispatch(num, this.m__E007.x / this.m__E003.x, this.m__E007.y / this.m__E003.y, this.m__E007.z / this.m__E003.z);
		m_Scatter.SetTexture(0, _ED3E._E000(42947), this.m__E005);
		m_Scatter.SetTexture(0, _ED3E._E000(41035), this.m__E006);
		m_Scatter.Dispatch(0, this.m__E007.x / this.m__E004.x, this.m__E007.y / this.m__E004.y, 1);
	}

	private void _E00B(RenderTexture src, RenderTexture dest)
	{
		_E014(ref this.m__E000, m_DebugShader);
		this.m__E000.SetTexture(_E01A, this.m__E005);
		this.m__E000.SetTexture(_E01B, this.m__E006);
		this.m__E000.SetFloat(_E01C, m_Z);
		this.m__E000.SetTexture(_E01D, src);
		Graphics.Blit(src, dest, this.m__E000);
	}

	private void _E00C(int width, int height)
	{
		Shader.SetGlobalTexture(_E01B, this.m__E006);
		Shader.SetGlobalVector(_E01E, new Vector4(1f / (float)width, 1f / (float)height, width, height));
		Shader.SetGlobalVector(_E01F, new Vector4(1f / (float)this.m__E007.x, 1f / (float)this.m__E007.y, 1f / (float)this.m__E007.z, 0f));
		Shader.SetGlobalFloat(_E020, _E022.farClipPlane / _E024);
		Shader.SetGlobalFloat(_E021, _E023 / _E024);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (!CheckSupport())
		{
			Debug.LogError(GetUnsupportedErrorMessage());
			_E3A1.BlitOrCopy(src, dest);
			base.enabled = false;
			return;
		}
		if (m_Debug)
		{
			_E00B(src, dest);
			return;
		}
		_E00A();
		_E014(ref this.m__E001, m_ApplyToOpaqueShader);
		this.m__E001.SetTexture(_E01D, src);
		_E00C(src.width, src.height);
		Graphics.Blit(src, dest, this.m__E001);
		_E00D(enable: true);
	}

	private void OnPostRender()
	{
		_E00D(enable: false);
	}

	private void _E00D(bool enable)
	{
		if (enable)
		{
			Shader.EnableKeyword(_ED3E._E000(41082));
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(41082));
		}
	}

	private Vector3 _E00E(Camera c, Transform t, Vector3 p)
	{
		return t.InverseTransformPoint(c.ViewportToWorldPoint(p));
	}

	private void _E00F()
	{
		float z = _E024;
		Vector3 position = _E022.transform.position;
		Vector2[] array = _E018;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = _E022.ViewportToWorldPoint(new Vector3(array[i].x, array[i].y, z)) - position;
			_E019[i * 4] = vector.x;
			_E019[i * 4 + 1] = vector.y;
			_E019[i * 4 + 2] = vector.z;
			_E019[i * 4 + 3] = 0f;
		}
		m_InjectLightingAndDensity.SetVector(_ED3E._E000(41065), position);
		m_InjectLightingAndDensity.SetFloats(_ED3E._E000(41116), _E019);
	}

	private void _E010(ref RenderTexture volume)
	{
		if (!volume)
		{
			volume = new RenderTexture(this.m__E007.x, this.m__E007.y, 0, RuntimeUtilities.defaultHDRRenderTextureFormat);
			volume.volumeDepth = this.m__E007.z;
			volume.dimension = TextureDimension.Tex3D;
			volume.enableRandomWrite = true;
			volume.Create();
		}
	}

	private void _E011(ref ComputeBuffer buffer, int count, int stride)
	{
		if (buffer == null || buffer.count != count)
		{
			if (buffer != null)
			{
				buffer.Release();
				buffer = null;
			}
			if (count > 0)
			{
				buffer = new ComputeBuffer(count, stride);
			}
		}
	}

	private void _E012()
	{
		_E010(ref this.m__E005);
		_E010(ref this.m__E006);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		HashSet<FogLight>.Enumerator enumerator = LightManager<FogLight>.Get().GetEnumerator();
		while (enumerator.MoveNext())
		{
			FogLight current = enumerator.Current;
			if (current == null)
			{
				continue;
			}
			bool isOn = current.isOn;
			switch (current.type)
			{
			case LightOverride.Type.Point:
				if (isOn)
				{
					num++;
				}
				break;
			case LightOverride.Type.Tube:
				if (isOn)
				{
					num2++;
				}
				break;
			case LightOverride.Type.Area:
				if (isOn)
				{
					num3++;
				}
				break;
			}
		}
		_E011(ref this.m__E00A, num, Marshal.SizeOf(typeof(_E001)));
		_E011(ref this.m__E00C, num2, Marshal.SizeOf(typeof(_E002)));
		_E011(ref this.m__E00E, num3, Marshal.SizeOf(typeof(_E003)));
		_E011(ref this.m__E011, 1, 4);
	}

	private void _E013(ref RenderTexture rt)
	{
		if (!(rt == null))
		{
			RenderTexture.ReleaseTemporary(rt);
			rt = null;
		}
	}

	private void _E014(ref Material material, Shader shader)
	{
		if (!material)
		{
			if (!shader)
			{
				Debug.LogError(_ED3E._E000(42416));
				return;
			}
			material = new Material(shader);
			material.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawFrustum(Vector3.zero, _E022.fieldOfView, _E024, _E023, _E022.aspect);
	}

	public static bool CheckSupport()
	{
		return SystemInfo.supportsComputeShaders;
	}

	public static string GetUnsupportedErrorMessage()
	{
		return string.Concat(_ED3E._E000(41105), SystemInfo.graphicsDeviceType, _ED3E._E000(41244), SystemInfo.graphicsDeviceVersion);
	}
}
