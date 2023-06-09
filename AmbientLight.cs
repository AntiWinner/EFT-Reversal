using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[_E2E2(19000)]
[ExecuteInEditMode]
public class AmbientLight : OnRenderObjectConnector
{
	private class _E000
	{
		public CommandBuffer CbStencil;

		public CommandBuffer CbAmbient;

		public RenderTexture Texture;

		public bool HDR;

		public bool WasRendering;
	}

	[Serializable]
	public class CullingSettings
	{
		public float Distance = 100f;

		public float FadeLength = 5f;

		private float _maxSqrtDistance;

		private float _minSqrtDistance;

		private float _invSqrtDistRange;

		public void Update()
		{
			_maxSqrtDistance = Distance * Distance;
			_minSqrtDistance = (Distance - FadeLength) * (Distance - FadeLength);
			_invSqrtDistRange = 1f / (_minSqrtDistance - _maxSqrtDistance);
		}

		public bool PassCulling(float sqrtDistance, out float fadeValue)
		{
			if (sqrtDistance > _maxSqrtDistance)
			{
				fadeValue = 0f;
				return false;
			}
			if (sqrtDistance < _minSqrtDistance)
			{
				fadeValue = 1f;
				return true;
			}
			fadeValue = (sqrtDistance - _maxSqrtDistance) * _invSqrtDistRange;
			return true;
		}
	}

	private readonly Dictionary<Camera, _E000> m__E00C = new Dictionary<Camera, _E000>();

	private static readonly HashSet<AnalyticSource> m__E00D = new HashSet<AnalyticSource>();

	private static readonly SortedSet<AnalyticSource> m__E00E = new SortedSet<AnalyticSource>(new _E40E());

	private static readonly HashSet<DynamicAnalyticSource> _E00F = new HashSet<DynamicAnalyticSource>();

	private static readonly SortedSet<DynamicAnalyticSource> _E010 = new SortedSet<DynamicAnalyticSource>(new _E40E());

	private static readonly HashSet<StencilShadow> _E011 = new HashSet<StencilShadow>();

	private static readonly SortedSet<StencilShadow> _E012 = new SortedSet<StencilShadow>(new _E40F());

	public static bool UseSortedSources = true;

	[_E3EC("Use method SetSH to set spherical harmonics")]
	public Shader DepthWriteShader;

	public Shader ClearStencilShader;

	public Shader WriteStencilShader;

	public Shader ScreenAmbientShader;

	[FormerlySerializedAs("AnalSourceShader")]
	public Shader AnalyticSourceShader;

	[Space]
	public float AmbientBlur;

	[Header("Reflections")]
	public float ReflectionIntensity;

	public float ReflectionIntensitySSR = 1f;

	public float RenderDelay = 0.05f;

	public int QubemapResolution = 128;

	public LayerMask CullingMask;

	[Range(0f, 1f)]
	public float ReflectionBottomShade;

	public AnimationCurve _reflectionWettingFunc = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

	private Material _E013;

	private Material _E014;

	private Material _E015;

	private Material m__E000;

	private Material _E016;

	private MaterialPropertyBlock _E017;

	private static Mesh m__E002;

	private static Mesh _E018;

	private static readonly Matrix4x4 m__E003 = Matrix4x4.identity;

	private static int[] m__E006;

	private static int[] m__E007;

	private static int m__E008;

	private bool _E019;

	private readonly List<UnityEngine.Object> _E01A = new List<UnityEngine.Object>();

	private RenderTexture _E01B;

	private int _E01C = -1;

	private Camera _E01D;

	private float _E01E;

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(89562));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(89552));

	private static readonly int m__E004 = Shader.PropertyToID(_ED3E._E000(41354));

	private static readonly int m__E005 = Shader.PropertyToID(_ED3E._E000(41404));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(89551));

	private static readonly int m__E00B = Shader.PropertyToID(_ED3E._E000(41449));

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(89596));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(89585));

	private static readonly int _E024 = Shader.PropertyToID(_ED3E._E000(89630));

	private static readonly int _E025 = Shader.PropertyToID(_ED3E._E000(89605));

	private static readonly string _E026 = _ED3E._E000(89646);

	private static readonly string _E027 = _ED3E._E000(89693);

	private static readonly string _E028 = _ED3E._E000(89677);

	private RenderTexture _E001
	{
		get
		{
			if (_E01B != null)
			{
				return _E01B;
			}
			_E01B = new RenderTexture(QubemapResolution, QubemapResolution, 0, RenderTextureFormat.ARGBHalf)
			{
				name = _ED3E._E000(89298),
				dimension = TextureDimension.Cube,
				useMipMap = true,
				hideFlags = HideFlags.DontSave,
				autoGenerateMips = true
			};
			Shader.SetGlobalTexture(_E025, _E01B);
			return _E01B;
		}
	}

	private bool _E009
	{
		get
		{
			if (_E8A8.Exist)
			{
				return _E8A8.Instance.GetSSREnabled();
			}
			return false;
		}
	}

	public void SetReflectionIntensity(float value)
	{
		ReflectionIntensity = value;
		_E002();
	}

	public static void AddAnalyticSource(AnalyticSource source)
	{
		AmbientLight.m__E00D.Add(source);
		AmbientLight.m__E00E.Add(source);
	}

	public static void RemoveAnalyticSource(AnalyticSource source)
	{
		AmbientLight.m__E00D.Remove(source);
		AmbientLight.m__E00E.Remove(source);
	}

	public static void AddDynamicAnalyticSource(DynamicAnalyticSource source)
	{
		_E00F.Add(source);
		_E010.Add(source);
	}

	public static void RemoveDynamicAnalyticSource(DynamicAnalyticSource source)
	{
		_E00F.Remove(source);
		_E010.Remove(source);
	}

	public static void AddStencilShadow(StencilShadow shadow)
	{
		_E011.Add(shadow);
		_E012.Add(shadow);
	}

	public static void RemoveStencilShadow(StencilShadow shadow)
	{
		_E011.Remove(shadow);
		_E012.Remove(shadow);
	}

	public void Update()
	{
		foreach (DynamicAnalyticSource item in _E00F)
		{
			item.UpdateDynamicValues();
		}
	}

	public void LateUpdate()
	{
		if (!_E019)
		{
			_E00B();
		}
		_E000();
		_E001();
		_E002();
	}

	public override void ManualOnRenderObject(Camera currentCamera)
	{
		base.ManualOnRenderObject(currentCamera);
		if (!AmbientLight.m__E002)
		{
			AmbientLight.m__E002 = GetQuadMesh();
			_E018 = _E00D();
		}
		if (!this.m__E00C.TryGetValue(currentCamera, out var value))
		{
			value = new _E000
			{
				CbStencil = _E42E.FindOrCreate(currentCamera, CameraEvent.BeforeLighting, _ED3E._E000(89343)),
				CbAmbient = _E42E.FindOrCreate(currentCamera, CameraEvent.AfterLighting, _ED3E._E000(89326)),
				Texture = _E00A(currentCamera.pixelWidth, currentCamera.pixelHeight, currentCamera.name)
			};
			_E005(value, currentCamera);
			this.m__E00C.Add(currentCamera, value);
			_E019 = false;
		}
		if (value.Texture.width != currentCamera.pixelWidth || value.Texture.height != currentCamera.pixelHeight)
		{
			UnityEngine.Object.DestroyImmediate(value.Texture);
			value.Texture = _E00A(currentCamera.pixelWidth, currentCamera.pixelHeight, currentCamera.name);
			_E005(value, currentCamera);
		}
		value.WasRendering = true;
		if (value.HDR != currentCamera.allowHDR)
		{
			value.HDR = currentCamera.allowHDR;
			_E005(value, currentCamera);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		foreach (KeyValuePair<Camera, _E000> item in this.m__E00C)
		{
			Camera camera = item.Key as Camera;
			if (!(camera == null))
			{
				_E000 obj = this.m__E00C[camera];
				this.m__E00C.Remove(camera);
				camera.RemoveCommandBuffer(CameraEvent.BeforeLighting, obj.CbStencil);
				camera.RemoveCommandBuffer(CameraEvent.AfterLighting, obj.CbAmbient);
				obj.CbAmbient.Dispose();
				obj.CbStencil.Dispose();
				UnityEngine.Object.DestroyImmediate(obj.Texture);
			}
		}
		_E019 = false;
		_E01A.Clear();
	}

	private void _E000()
	{
		foreach (KeyValuePair<Camera, _E000> item in this.m__E00C)
		{
			Camera key = item.Key;
			_E000 value = item.Value;
			if (!value.WasRendering || !key)
			{
				_E01A.Add(key);
				continue;
			}
			_E005(value, key);
			value.WasRendering = false;
			_E003(value, key);
		}
	}

	private void _E001()
	{
		if (!(_E01E < Time.realtimeSinceStartup) && RenderDelay != 0f)
		{
			return;
		}
		if (_E01D == null)
		{
			_E01D = GetComponent<Camera>();
			if (_E01D == null)
			{
				_E01D = base.gameObject.AddComponent<Camera>();
			}
			_E01D.clearFlags = CameraClearFlags.Color;
			_E01D.backgroundColor = new Color(0f, 0f, 0f, 1f);
			_E01D.renderingPath = RenderingPath.VertexLit;
			_E01D.nearClipPlane = 0.1f;
			_E01D.farClipPlane = 0.2f;
			_E01D.useOcclusionCulling = false;
			_E01D.allowHDR = true;
			_E01D.enabled = false;
			_E01D.targetTexture = this._E001;
			_E01D.cullingMask = CullingMask;
		}
		if (_E01C < 0)
		{
			_E01D.RenderToCubemap(this._E001, 63);
		}
		else
		{
			if (_E01C == 6)
			{
				_E01C = 0;
			}
			_E01D.RenderToCubemap(this._E001, 1 << _E01C);
		}
		Shader.SetGlobalTexture(_E025, this._E001);
		_E01E = Time.realtimeSinceStartup + RenderDelay;
		_E01C++;
	}

	private void _E002()
	{
		float value = (this._E009 ? 1f : (_reflectionWettingFunc.Evaluate(RainController.WettingIntensity) * ReflectionIntensity));
		_E017.SetFloat(_E023, value);
	}

	private void _E003(_E000 camSettings, Camera currentCamera)
	{
		CommandBuffer cbStencil = camSettings.CbStencil;
		cbStencil.Clear();
		if (_E00E(currentCamera.cullingMask))
		{
			_E004(cbStencil, camSettings.Texture, _ED3E._E000(89373));
			Vector3 position = currentCamera.transform.position;
			Plane[] frustrumPlanes = _E379.CalculateFrustumPlanesNonAlloc(currentCamera);
			_E006(cbStencil, position, frustrumPlanes);
			_E008(cbStencil, position, frustrumPlanes);
		}
	}

	private void _E004(CommandBuffer commandBuffer, RenderTexture renderTexture, string shaderTextureName)
	{
		commandBuffer.Blit(BuiltinRenderTextureType.GBuffer0, renderTexture, _E013);
		commandBuffer.SetRenderTarget(renderTexture);
		commandBuffer.SetGlobalTexture(shaderTextureName, renderTexture);
		commandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 0f));
		commandBuffer.SetGlobalTexture(shaderTextureName, renderTexture);
	}

	private void _E005(_E000 camSettings, Camera currentCamera)
	{
		if (this.m__E000 == null)
		{
			Debug.Log(_ED3E._E000(89356));
		}
		CommandBuffer cbAmbient = camSettings.CbAmbient;
		cbAmbient.Clear();
		if (_E00E(currentCamera.cullingMask))
		{
			if (currentCamera.allowHDR)
			{
				cbAmbient.SetGlobalFloat(AmbientLight.m__E004, 1f);
				cbAmbient.SetGlobalFloat(AmbientLight.m__E005, 1f);
			}
			else
			{
				cbAmbient.SetGlobalFloat(AmbientLight.m__E004, 2f);
				cbAmbient.SetGlobalFloat(AmbientLight.m__E005, 0f);
			}
			cbAmbient.SetRenderTarget(BuiltinRenderTextureType.CurrentActive, BuiltinRenderTextureType.CurrentActive);
			cbAmbient.DrawMesh(AmbientLight.m__E002, Matrix4x4.identity, this.m__E000, 0, 0, _E017);
		}
	}

	private void _E006(CommandBuffer cmdBuf, Vector3 camPos, Plane[] frustrumPlanes)
	{
		cmdBuf.BeginSample(_E026);
		IEnumerable<StencilShadow> enumerable2;
		if (!UseSortedSources)
		{
			IEnumerable<StencilShadow> enumerable = _E011;
			enumerable2 = enumerable;
		}
		else
		{
			IEnumerable<StencilShadow> enumerable = _E012;
			enumerable2 = enumerable;
		}
		foreach (StencilShadow item in enumerable2)
		{
			if (!item || !item.gameObject || !item.isActiveAndEnabled)
			{
				_E01A.Add(item);
				continue;
			}
			if (!item.Renderer)
			{
				item.Awake();
			}
			Bounds bounds = item.Bounds;
			if (GeometryUtility.TestPlanesAABB(frustrumPlanes, bounds))
			{
				_E007(cmdBuf, item, camPos);
			}
		}
		if (_E01A.Count > 0)
		{
			for (int i = 0; i < _E01A.Count; i++)
			{
				StencilShadow stencilShadow = _E01A[i] as StencilShadow;
				if (stencilShadow != null)
				{
					_E011.Remove(stencilShadow);
					_E012.Remove(stencilShadow);
				}
			}
			_E01A.Clear();
		}
		cmdBuf.EndSample(_E026);
	}

	private bool _E007(CommandBuffer cmdBuf, StencilShadow ss, Vector3 camPos, bool disableColorPass = false)
	{
		Bounds bounds = ss.Bounds;
		if (!ss.Culling.PassCulling((bounds.center - camPos).sqrMagnitude, out var fadeValue))
		{
			return false;
		}
		cmdBuf.BeginSample(_E026 + _ED3E._E000(89403));
		cmdBuf.DrawMesh(AmbientLight.m__E002, AmbientLight.m__E003, _E014);
		cmdBuf.DrawRenderer(ss.Renderer, _E015, 0, 0);
		cmdBuf.DrawRenderer(ss.Renderer, _E015, 0, 1);
		if (!disableColorPass)
		{
			cmdBuf.SetGlobalColor(_ED3E._E000(89393), ss.Ambient * fadeValue);
			cmdBuf.DrawMesh(AmbientLight.m__E002, AmbientLight.m__E003, _E015, 0, 2);
		}
		cmdBuf.EndSample(_E026 + _ED3E._E000(89403));
		return true;
	}

	private void _E008(CommandBuffer cmdBuf, Vector3 camPos, Plane[] frustrumPlanes)
	{
		IEnumerable<AnalyticSource> enumerable2;
		if (!UseSortedSources)
		{
			IEnumerable<AnalyticSource> enumerable = AmbientLight.m__E00D;
			enumerable2 = enumerable;
		}
		else
		{
			IEnumerable<AnalyticSource> enumerable = AmbientLight.m__E00E;
			enumerable2 = enumerable;
		}
		IEnumerable<AnalyticSource> enumerable3;
		if (!UseSortedSources)
		{
			IEnumerable<AnalyticSource> enumerable = _E00F;
			enumerable3 = enumerable;
		}
		else
		{
			IEnumerable<AnalyticSource> enumerable = _E010;
			enumerable3 = enumerable;
		}
		IEnumerable<AnalyticSource> enumerable4 = enumerable3;
		cmdBuf.BeginSample(_E027);
		foreach (AnalyticSource item in enumerable2)
		{
			_E009(item, cmdBuf, camPos, frustrumPlanes);
		}
		cmdBuf.EndSample(_E027);
		cmdBuf.BeginSample(_E028);
		foreach (DynamicAnalyticSource item2 in enumerable4)
		{
			_E009(item2, cmdBuf, camPos, frustrumPlanes);
		}
		cmdBuf.EndSample(_E028);
	}

	private void _E009(AnalyticSource source, CommandBuffer cmdBuf, Vector3 camPos, Plane[] frustrumPlanes)
	{
		if (GeometryUtility.TestPlanesAABB(frustrumPlanes, source.Bounds) && source.Culling.PassCulling((source.Bounds.center - camPos).sqrMagnitude, out var fadeValue))
		{
			cmdBuf.BeginSample(_E027 + _ED3E._E000(89403));
			if (source.Bounds.Contains(camPos))
			{
				cmdBuf.SetGlobalFloat(_E01F, 1f);
				cmdBuf.SetGlobalFloat(_E020, 5f);
			}
			else
			{
				cmdBuf.SetGlobalFloat(_E01F, 2f);
				cmdBuf.SetGlobalFloat(_E020, 2f);
			}
			if (source.LinkedStencilShadow != null)
			{
				_E007(cmdBuf, source.LinkedStencilShadow, camPos, disableColorPass: true);
			}
			int shaderPass = source.ShaderPath + ((source.LinkedStencilShadow != null) ? 3 : 0);
			source.MaterialPropertyBlock.SetFloat(_E021, fadeValue);
			cmdBuf.DrawMesh(_E018, source.LocalToWorldMatrix, _E016, 0, shaderPass, source.MaterialPropertyBlock);
			cmdBuf.EndSample(_E027 + _ED3E._E000(89403));
		}
	}

	private static RenderTexture _E00A(int width, int height, string name = "")
	{
		return new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32)
		{
			name = _ED3E._E000(89438) + name,
			filterMode = FilterMode.Point
		};
	}

	private void _E00B()
	{
		_E01E = 0f;
		_E013 = new Material(DepthWriteShader);
		_E014 = new Material(ClearStencilShader);
		_E015 = new Material(WriteStencilShader);
		this.m__E000 = new Material(ScreenAmbientShader);
		_E016 = new Material(AnalyticSourceShader);
		_E017 = new MaterialPropertyBlock();
		AmbientLight.m__E006 = new int[3]
		{
			Shader.PropertyToID(_ED3E._E000(41398)),
			Shader.PropertyToID(_ED3E._E000(41388)),
			Shader.PropertyToID(_ED3E._E000(41386))
		};
		AmbientLight.m__E007 = new int[3]
		{
			Shader.PropertyToID(_ED3E._E000(41376)),
			Shader.PropertyToID(_ED3E._E000(41438)),
			Shader.PropertyToID(_ED3E._E000(41428))
		};
		AmbientLight.m__E008 = Shader.PropertyToID(_ED3E._E000(41426));
		_E017.SetFloat(AmbientLight.m__E00B, 1f / AmbientBlur);
		_E002();
		Shader.SetGlobalFloat(_E024, 2f - ReflectionBottomShade);
		foreach (KeyValuePair<Camera, _E000> item in this.m__E00C)
		{
			if ((bool)item.Key)
			{
				_E005(item.Value, item.Key);
			}
		}
		_E014.SetFloat(_ED3E._E000(42517), 64f);
		_E019 = true;
	}

	private void OnValidate()
	{
		_E00B();
	}

	protected override void Start()
	{
		base.Start();
		_E00B();
	}

	private void OnEnable()
	{
		_E00B();
	}

	private void OnDisable()
	{
		foreach (KeyValuePair<Camera, _E000> item in this.m__E00C)
		{
			item.Value.CbAmbient.Clear();
			item.Value.CbStencil.Clear();
			UnityEngine.Object.DestroyImmediate(item.Value.Texture);
		}
		this.m__E00C.Clear();
		_E019 = false;
		UnityEngine.Object.DestroyImmediate(_E01B);
	}

	public void SetSH(SphericalHarmonicsL2 sh)
	{
		if (!_E019)
		{
			_E00B();
		}
		Shader.SetGlobalVector(AmbientLight.m__E008, new Vector4(sh[0, 8], sh[1, 8], sh[2, 8], 1f));
		for (int i = 0; i < 3; i++)
		{
			Shader.SetGlobalVector(AmbientLight.m__E006[i], new Vector4(sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]));
			Shader.SetGlobalVector(AmbientLight.m__E007[i], new Vector4(sh[i, 4], sh[i, 5], sh[i, 6] * 3f, sh[i, 7]));
		}
		Color colorTop = GetColorTop(sh);
		Shader.SetGlobalVector(_E022, new Vector3(colorTop.r, colorTop.g, colorTop.b));
	}

	public Color GetColorTop(SphericalHarmonicsL2 sh)
	{
		Color result = default(Color);
		for (int i = 0; i < 3; i++)
		{
			result[i] = sh[i, 1] + (sh[i, 0] - sh[i, 6]);
		}
		result += new Color(0f - sh[0, 8], 0f - sh[1, 8], 0f - sh[2, 8]);
		if (QualitySettings.activeColorSpace != 0)
		{
			result = new Color(Mathf.LinearToGammaSpace(result.r), Mathf.LinearToGammaSpace(result.g), Mathf.LinearToGammaSpace(result.b));
		}
		return result;
	}

	public Color GetColor(SphericalHarmonicsL2 sh, Vector3 normal)
	{
		Vector4 b = new Vector4(normal.x, normal.y, normal.z, 1f);
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < 3; i++)
		{
			zero[i] = Vector4.Dot(new Vector4(sh[i, 3], sh[i, 1], sh[i, 2], sh[i, 0] - sh[i, 6]), b);
		}
		Vector4 b2 = new Vector4(b.x * b.y, b.y * b.z, b.z * b.z, b.z * b.x);
		Vector3 zero2 = Vector3.zero;
		for (int j = 0; j < 3; j++)
		{
			zero2[j] = Vector4.Dot(new Vector4(sh[j, 4], sh[j, 5], sh[j, 6] * 3f, sh[j, 7]), b2);
		}
		float num = b.x * b.x - b.y * b.y;
		Vector3 vector = new Vector3(sh[0, 8], sh[1, 8], sh[2, 8]) * num;
		Vector3 vector2 = zero + zero2 + vector;
		return new Color(vector2.x, vector2.y, vector2.z);
	}

	public static Mesh GetQuadMesh()
	{
		float z = 0.1f;
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(-1f, -1f, z),
			new Vector3(1f, -1f, z),
			new Vector3(-1f, 1f, z),
			new Vector3(1f, 1f, z)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] triangles = new int[6] { 0, 1, 3, 3, 2, 0 };
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.name = _ED3E._E000(89464);
		mesh.RecalculateBounds();
		mesh.name = _ED3E._E000(41328);
		return mesh;
	}

	private static Mesh _E00C()
	{
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(-1f, -1f, -0.1f),
			new Vector3(-1f, 1f, -0.1f),
			new Vector3(1f, 1f, -0.1f),
			new Vector3(1f, -1f, -0.1f)
		};
		int[] triangles = new int[6] { 0, 2, 1, 0, 3, 2 };
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(3f, 0f),
			new Vector2(2f, 0f),
			new Vector2(1f, 0f)
		};
		Mesh obj = new Mesh
		{
			name = _ED3E._E000(89441),
			vertices = vertices,
			uv = uv,
			triangles = triangles
		};
		Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 2.1474836E+09f);
		obj.bounds = bounds;
		return obj;
	}

	private static Mesh _E00D()
	{
		float num = 1f;
		Vector3[] vertices = new Vector3[8]
		{
			new Vector3(num, num, 0f - num),
			new Vector3(0f - num, num, 0f - num),
			new Vector3(0f - num, num, num),
			new Vector3(num, num, num),
			new Vector3(num, 0f - num, 0f - num),
			new Vector3(0f - num, 0f - num, 0f - num),
			new Vector3(0f - num, 0f - num, num),
			new Vector3(num, 0f - num, num)
		};
		int[] array = new int[36]
		{
			0, 1, 2, 0, 2, 3, 0, 4, 5, 0,
			5, 1, 1, 5, 6, 1, 6, 2, 2, 6,
			7, 2, 7, 3, 3, 7, 4, 3, 4, 0,
			4, 7, 6, 4, 6, 5
		};
		_ = array.Length;
		int[] array2 = new int[0];
		for (int i = 0; i < 0; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				int num2 = i * array.Length + j;
				array2[num2] = array[j];
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = array;
		mesh.name = _ED3E._E000(89481);
		mesh.RecalculateBounds();
		mesh.name = _ED3E._E000(89522);
		return mesh;
	}

	private bool _E00E(int cullingMask)
	{
		int layer = base.gameObject.layer;
		if (cullingMask == (cullingMask | (1 << layer)))
		{
			return true;
		}
		return false;
	}
}
