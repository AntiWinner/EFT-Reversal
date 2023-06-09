using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.Weather;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[_E2E2(20001)]
public class WaterForSSRv2 : MonoBehaviour
{
	public class _E000
	{
		public Mesh Mesh;

		public Matrix4x4 Matrix;

		public _E000(Mesh mesh, Matrix4x4 matrix)
		{
			Mesh = mesh;
			Matrix = matrix;
		}
	}

	[Serializable]
	public class TextureBlendSetting
	{
		public float Scale = 0.1f;

		public Vector2 MovementDirection = Vector2.one;

		[Range(0f, 1f)]
		public float Blend = 0.5f;

		public Vector4 GetVals()
		{
			return new Vector4(MovementDirection.x, MovementDirection.y, Scale, Blend);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public int i;

		public WaterForSSRv2 _003C_003E4__this;

		internal bool _E000(_E000 x)
		{
			return x.Mesh == _003C_003E4__this.WaterPlanes[i].sharedMesh;
		}
	}

	[CompilerGenerated]
	private static Action<WaterForSSRv2> m__E000;

	[CompilerGenerated]
	private static Action<WaterForSSRv2> m__E001;

	public Shader WaterShader;

	public Texture2D RippleTexture;

	[Header("Textures")]
	[Space]
	public Texture Normals;

	public Texture NormalsDetails;

	public float NormalsDetailsMipMapBias;

	public Texture Foam;

	[Header("Layers Settings")]
	[Space(32f)]
	public TextureBlendSetting NormalsA;

	public TextureBlendSetting NormalsB;

	public TextureBlendSetting NormalsDetails0;

	[Space]
	public TextureBlendSetting FoamA;

	public TextureBlendSetting FoamB;

	[Space(32f)]
	[Header("Depth Settings")]
	public float BorderFade;

	public float BorderFadeDistStart;

	public float BorderFadeDistRange;

	public float DepthFade;

	public float DepthColorFade;

	public float DepthRefractions;

	[ColorUsage(false)]
	public Color DepthColorDeep;

	[ColorUsage(false)]
	public Color DepthColorShallow;

	[Space(32f)]
	[Header("Other")]
	public float Bumpiness;

	[Space]
	public float RippleScale;

	public float RippleBumpness;

	[Space]
	public float FoamSize;

	public float FoamIntensity;

	[ColorUsage(false)]
	public Color FoamColor;

	[Space]
	[Range(0f, 1f)]
	public float FresnelIntensity = 0.9f;

	public float FresnelPower = 5f;

	[Space]
	public float AdditionalCubemapReflectionMinWetting = 0.05f;

	public float AdditionalCubemapReflectionMaxWetting = 0.35f;

	public AnimationCurve ReflectionWettingFunc = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public Color ReflectionColor;

	public Color DiffuseColor;

	public MeshFilter[] WaterPlanes = new MeshFilter[0];

	private List<_E000> m__E002;

	private Material m__E003;

	private float m__E004;

	private float _E005 = -1f;

	private RenderTargetIdentifier[] _E006;

	private int _E007;

	private int _E008;

	private int _E009;

	private int _E00A;

	private int _E00B;

	private int _E00C;

	private int _E00D;

	private int _E00E;

	private int _E00F;

	private int _E010;

	private int _E011;

	private int _E012;

	private int _E013;

	private int _E014;

	private int _E015;

	private int _E016;

	private int _E017;

	private int _E018;

	private int _E019;

	private int _E01A;

	private int _E01B;

	private int _E01C;

	private int _E01D;

	private int _E01E;

	public static event Action<WaterForSSRv2> OnAdd
	{
		[CompilerGenerated]
		add
		{
			Action<WaterForSSRv2> action = WaterForSSRv2.m__E000;
			Action<WaterForSSRv2> action2;
			do
			{
				action2 = action;
				Action<WaterForSSRv2> value2 = (Action<WaterForSSRv2>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref WaterForSSRv2.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<WaterForSSRv2> action = WaterForSSRv2.m__E000;
			Action<WaterForSSRv2> action2;
			do
			{
				action2 = action;
				Action<WaterForSSRv2> value2 = (Action<WaterForSSRv2>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref WaterForSSRv2.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<WaterForSSRv2> OnRemove
	{
		[CompilerGenerated]
		add
		{
			Action<WaterForSSRv2> action = WaterForSSRv2.m__E001;
			Action<WaterForSSRv2> action2;
			do
			{
				action2 = action;
				Action<WaterForSSRv2> value2 = (Action<WaterForSSRv2>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref WaterForSSRv2.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<WaterForSSRv2> action = WaterForSSRv2.m__E001;
			Action<WaterForSSRv2> action2;
			do
			{
				action2 = action;
				Action<WaterForSSRv2> value2 = (Action<WaterForSSRv2>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref WaterForSSRv2.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Start()
	{
		OnValidate();
		if (WaterForSSRv2.m__E000 != null)
		{
			WaterForSSRv2.m__E000(this);
		}
	}

	private void OnEnable()
	{
		OnValidate();
		if (WaterForSSRv2.m__E000 != null)
		{
			WaterForSSRv2.m__E000(this);
		}
	}

	private void Update()
	{
		if (!(this.m__E003 == null))
		{
			float t = 0f;
			if (WeatherController.Instance != null)
			{
				t = ReflectionWettingFunc.Evaluate(RainController.Intensity);
			}
			this.m__E003.SetFloat(_E01A, Mathf.Lerp(AdditionalCubemapReflectionMinWetting, AdditionalCubemapReflectionMaxWetting, t));
		}
	}

	private void OnValidate()
	{
		if (float.IsNaN(NormalsA.Blend) || float.IsNaN(NormalsB.Blend) || float.IsNaN(NormalsDetails0.Blend))
		{
			NormalsA.Blend = (NormalsB.Blend = (NormalsDetails0.Blend = 0.5f));
		}
		float num = NormalsA.Blend + NormalsB.Blend + NormalsDetails0.Blend;
		num = 1f / num;
		NormalsA.Blend *= num;
		NormalsB.Blend *= num;
		NormalsDetails0.Blend *= num;
		if (float.IsNaN(FoamA.Blend) || float.IsNaN(FoamB.Blend))
		{
			FoamA.Blend = (FoamB.Blend = 0.5f);
		}
		num = FoamA.Blend + FoamB.Blend;
		num = 1f / num;
		FoamA.Blend *= num;
		FoamB.Blend *= num;
		_E000();
		_E002();
		_E003();
	}

	private void _E000()
	{
		_E007 = Shader.PropertyToID(_ED3E._E000(85638));
		_E008 = Shader.PropertyToID(_ED3E._E000(85966));
		_E009 = Shader.PropertyToID(_ED3E._E000(44835));
		_E00A = Shader.PropertyToID(_ED3E._E000(85683));
		_E00B = Shader.PropertyToID(_ED3E._E000(85667));
		_E00C = Shader.PropertyToID(_ED3E._E000(85716));
		_E00D = Shader.PropertyToID(_ED3E._E000(85711));
		_E00E = Shader.PropertyToID(_ED3E._E000(85755));
		_E00F = Shader.PropertyToID(_ED3E._E000(85742));
		_E010 = Shader.PropertyToID(_ED3E._E000(85790));
		_E011 = Shader.PropertyToID(_ED3E._E000(85769));
		_E012 = Shader.PropertyToID(_ED3E._E000(85763));
		_E013 = Shader.PropertyToID(_ED3E._E000(85814));
		_E014 = Shader.PropertyToID(_ED3E._E000(85799));
		_E015 = Shader.PropertyToID(_ED3E._E000(85845));
		_E016 = Shader.PropertyToID(_ED3E._E000(85832));
		_E017 = Shader.PropertyToID(_ED3E._E000(85827));
		_E018 = Shader.PropertyToID(_ED3E._E000(85878));
		_E019 = Shader.PropertyToID(_ED3E._E000(85865));
		_E01A = Shader.PropertyToID(_ED3E._E000(85916));
		_E01B = Shader.PropertyToID(_ED3E._E000(85889));
		_E01C = Shader.PropertyToID(_ED3E._E000(85942));
		_E01D = Shader.PropertyToID(_ED3E._E000(85926));
		_E01E = Shader.PropertyToID(_ED3E._E000(85979));
	}

	private void OnDisable()
	{
		if (WaterForSSRv2.m__E001 != null)
		{
			WaterForSSRv2.m__E001(this);
		}
	}

	private void OnDestroy()
	{
		if (WaterForSSRv2.m__E001 != null)
		{
			WaterForSSRv2.m__E001(this);
		}
	}

	public void InitBuffer(CommandBuffer buffer, Camera currentCamera)
	{
		_E003();
		_E001(currentCamera);
		RenderBuffer(buffer, currentCamera);
	}

	private void _E001(Camera currentCamera)
	{
		if (_E006 == null)
		{
			_E006 = new RenderTargetIdentifier[4]
			{
				BuiltinRenderTextureType.GBuffer0,
				BuiltinRenderTextureType.GBuffer1,
				BuiltinRenderTextureType.GBuffer2,
				currentCamera.allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3
			};
		}
		_E006[3] = (currentCamera.allowHDR ? BuiltinRenderTextureType.CameraTarget : BuiltinRenderTextureType.GBuffer3);
	}

	public void RenderBuffer(CommandBuffer buffer, Camera currentCamera)
	{
		if (this.m__E003 == null)
		{
			_E003();
		}
		_E004();
		if (_E006 == null)
		{
			_E001(currentCamera);
		}
		buffer.GetTemporaryRT(_E007, -1, -1, 16, FilterMode.Point, RenderTextureFormat.ARGB32);
		buffer.Blit(BuiltinRenderTextureType.GBuffer0, _E007);
		buffer.SetGlobalTexture(_E007, _E007);
		buffer.GetTemporaryRT(_E008, -1, -1, 16, FilterMode.Point, RenderTextureFormat.ARGB32);
		buffer.Blit(BuiltinRenderTextureType.GBuffer2, _E008);
		buffer.SetGlobalTexture(_E008, _E008);
		buffer.SetRenderTarget(_E006, BuiltinRenderTextureType.CameraTarget);
		for (int i = 0; i < this.m__E002.Count; i++)
		{
			buffer.DrawMesh(this.m__E002[i].Mesh, this.m__E002[i].Matrix, this.m__E003, 0, 0);
		}
		buffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.ResolvedDepth);
		for (int j = 0; j < this.m__E002.Count; j++)
		{
			buffer.DrawMesh(this.m__E002[j].Mesh, this.m__E002[j].Matrix, this.m__E003, 0, 1);
		}
		buffer.ReleaseTemporaryRT(_E007);
		buffer.ReleaseTemporaryRT(_E008);
	}

	public bool IsCorrectLayer(int cullingMask)
	{
		if (WaterPlanes.Length == 0)
		{
			return false;
		}
		MeshFilter meshFilter = WaterPlanes[0];
		if (meshFilter == null)
		{
			return false;
		}
		int layer = meshFilter.gameObject.layer;
		if (cullingMask == (cullingMask | (1 << layer)))
		{
			return true;
		}
		return false;
	}

	private void _E002()
	{
		if (WaterPlanes.Length == 0)
		{
			return;
		}
		if (this.m__E002 == null)
		{
			this.m__E002 = new List<_E000>();
			for (int j = 0; j < WaterPlanes.Length; j++)
			{
				if (WaterPlanes[j] != null)
				{
					_E000 item = new _E000(WaterPlanes[j].sharedMesh, WaterPlanes[j].gameObject.transform.localToWorldMatrix);
					this.m__E002.Add(item);
				}
			}
			return;
		}
		int i;
		for (i = 0; i < WaterPlanes.Length; i++)
		{
			if (WaterPlanes[i] != null)
			{
				this.m__E002.Find((_E000 x) => x.Mesh == WaterPlanes[i].sharedMesh).Matrix = WaterPlanes[i].gameObject.transform.localToWorldMatrix;
			}
		}
	}

	private void _E003()
	{
		if (this.m__E003 == null)
		{
			this.m__E003 = new Material(WaterShader);
		}
		NormalsDetails.mipMapBias = NormalsDetailsMipMapBias;
		this.m__E003.SetTexture(_E009, Normals);
		this.m__E003.SetTexture(_E00A, NormalsDetails);
		this.m__E003.SetTexture(_E00B, Foam);
		this.m__E003.SetVector(_E00C, new Vector4(DepthFade, DepthRefractions, DepthColorFade, BorderFade));
		this.m__E003.SetVector(_E00D, new Vector4(BorderFadeDistStart, 1f / BorderFadeDistRange));
		this.m__E003.SetFloat(_E00E, Bumpiness);
		this.m__E003.SetColor(_E00F, DepthColorDeep);
		this.m__E003.SetColor(_E010, DepthColorShallow);
		this.m__E003.SetVector(_E011, new Vector4(FoamSize, 1f / FoamSize, FoamIntensity));
		this.m__E003.SetColor(_E012, FoamColor);
		this.m__E003.SetColor(_E013, ReflectionColor);
		this.m__E003.SetColor(_E014, DiffuseColor);
		this.m__E003.SetVector(_E015, NormalsA.GetVals());
		this.m__E003.SetVector(_E016, NormalsB.GetVals());
		this.m__E003.SetVector(_E017, NormalsDetails0.GetVals());
		this.m__E003.SetVector(_E018, FoamA.GetVals());
		this.m__E003.SetVector(_E019, FoamB.GetVals());
		_E004();
		this.m__E003.SetFloat(_E01B, RippleScale);
		this.m__E003.SetFloat(_E01C, RippleBumpness);
		this.m__E003.SetVector(_E01D, new Vector4(FresnelIntensity, FresnelPower));
		this.m__E003.SetTexture(_E01E, RippleTexture);
	}

	private void _E004()
	{
		if (WeatherController.Instance != null)
		{
			this.m__E004 = ReflectionWettingFunc.Evaluate(RainController.Intensity);
		}
		if (!(Math.Abs(this.m__E004 - _E005) < 0.01f))
		{
			_E005 = this.m__E004;
			this.m__E003.SetFloat(_E01A, Mathf.Lerp(AdditionalCubemapReflectionMinWetting, AdditionalCubemapReflectionMaxWetting, this.m__E004));
		}
	}
}
