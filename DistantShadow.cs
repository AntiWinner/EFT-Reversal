using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Bsg.GameSettings;
using Comfort.Common;
using EFT.Impostors;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class DistantShadow : MonoBehaviour
{
	public enum ResolutionState
	{
		QUARTER,
		HALF,
		FULL
	}

	private class _E000
	{
		public Vector3 _cachedLightDirection = new Vector3(0f, -1f, 0f);

		public Vector3 _cachedShadowCameraPosition = new Vector3(0f, -1f, 0f);

		public Vector3 _cachedShadowCameraForward = new Vector3(0f, 0f, 0f);

		public Vector3 _cachedShadowCameraRight = new Vector3(0f, 0f, 0f);

		public Vector3 _cachedShadowCameraUp = new Vector3(0f, 0f, 0f);

		public Matrix4x4 _cachedGlobalShadowWorldToScreen = Matrix4x4.identity;

		public Matrix4x4 _cachedGlobalShadowWorldToView = Matrix4x4.identity;

		public Matrix4x4 _cachedGlobalShadowProjGPU = Matrix4x4.identity;

		public Matrix4x4 _cachedGlobalShadowProj = Matrix4x4.identity;

		public float _cachedNearPlane = 1f;

		public float _cachedFarPlane = 1f;

		public bool _buffersDataIsPropagated;

		public Matrix4x4[] _cachedGlobalShadowWorldToScreenData;

		public Matrix4x4[] _cachedGlobalShadowWorldToViewData;

		public Matrix4x4[] _cachedGlobalShadowProjData;

		public Vector4[] _cachedOffsetScaleData;

		public ComputeBuffer _cachedGlobalShadowWorldToScreenBuffer;

		public ComputeBuffer _cachedGlobalShadowWorldToViewBuffer;

		public ComputeBuffer _cachedGlobalShadowProjBuffer;

		public ComputeBuffer _cachedOffsetScaleBuffer;
	}

	private class _E001
	{
		public Mesh mesh;

		public Material material;

		public int submeshIdx;

		public Matrix4x4 drawTransform;
	}

	private static readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(87629));

	private static readonly int m__E001 = Shader.PropertyToID(_ED3E._E000(87673));

	private static readonly int m__E002 = Shader.PropertyToID(_ED3E._E000(87651));

	private static readonly int m__E003 = Shader.PropertyToID(_ED3E._E000(87691));

	private static readonly int m__E004 = Shader.PropertyToID(_ED3E._E000(87730));

	private static readonly int m__E005 = Shader.PropertyToID(_ED3E._E000(87775));

	private static readonly int m__E006 = Shader.PropertyToID(_ED3E._E000(87748));

	private static readonly int m__E007 = Shader.PropertyToID(_ED3E._E000(87791));

	private static readonly int m__E008 = Shader.PropertyToID(_ED3E._E000(87824));

	private static readonly int m__E009 = Shader.PropertyToID(_ED3E._E000(87864));

	private static readonly int m__E00A = Shader.PropertyToID(_ED3E._E000(87849));

	private static readonly int m__E00B = Shader.PropertyToID(_ED3E._E000(87895));

	private static readonly int m__E00C = Shader.PropertyToID(_ED3E._E000(87883));

	private static readonly int m__E00D = Shader.PropertyToID(_ED3E._E000(87930));

	private static readonly int m__E00E = Shader.PropertyToID(_ED3E._E000(87913));

	private static readonly int m__E00F = Shader.PropertyToID(_ED3E._E000(87963));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(87938));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(87980));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(87970));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(88019));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(19728));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(88005));

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(88045));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(88034));

	public Camera MasterCamera;

	private SSAA _E018;

	public bool EnableShadowsBlend = true;

	public bool EnableAdaptiveBias;

	public float AdaptiveBiasK = 0.0003f;

	public float ConstantBias = 0.001f;

	public float PCFShift = 0.0003f;

	public int ShadowPixelsSize = 4096;

	public bool DisableWind = true;

	public bool ShouldApplySettings = true;

	public float GlobalShadowHalfSize = 500f;

	public float OffsetTowardsLight = 100f;

	public float BlendTime = 0.5f;

	public float LightDirectionExtrapolateFactor = 1.5f;

	public float ObjectSizeFilter = 1f;

	public bool FixGlowEffect = true;

	public bool EnableDistantShadowKeyword = true;

	public bool EnableDistantShadowPCF;

	public bool EnableMultiviewTiles;

	public bool EnablePCFShift;

	public bool ReduceBlurSamples = true;

	public float BlurDepthThreshold = 1f;

	[NonSerialized]
	public bool InvalidateRenderersOnNextUpdate = true;

	private float _E019;

	private float _E01A;

	private float _E01B;

	private bool _E01C;

	public bool PreComputeMask;

	public bool EnableShadowmaskBlur = true;

	public bool EnableBlurMask = true;

	public bool EnableParallax;

	private float _E01D = 300000f;

	private bool _E01E = true;

	[Range(0.01f, 1f)]
	private float _E01F = 0.5f;

	public static int ObjectsToRenderInfo = 0;

	public static int ObjectsToRenderPerFrameInfo = 0;

	public static float LastTimeToUpdateInfo = 0f;

	public float LastTimeToUpdate;

	private float _E020;

	public float NearPlane = 0.1f;

	public float FarPlane = 500f;

	public int ObjectsPerFrame = 150;

	private int _E021 = 2;

	private int _E022 = 1;

	private float _E023;

	private const float _E024 = 10f;

	public bool UseGaussDistribution = true;

	public float SamplingRotation = 60f;

	private uint _E025 = 100u;

	private CommandBuffer _E026;

	private CommandBuffer _E027;

	private CommandBuffer _E028;

	private CommandBuffer _E029;

	private CommandBuffer _E02A;

	private CommandBuffer _E02B;

	private CommandBuffer _E02C;

	private CommandBuffer _E02D;

	private RenderTexture _E02E;

	private RenderTexture _E02F;

	private const GraphicsFormat _E030 = GraphicsFormat.R32_SFloat;

	private RenderTexture _E031;

	private RenderTexture _E032;

	private RenderTexture _E033;

	private RenderTexture _E034;

	private const GraphicsFormat _E035 = GraphicsFormat.R8_UNorm;

	private RenderTexture _E036;

	private RenderTexture _E037;

	private RenderTexture _E038;

	private RenderTexture _E039;

	private const int _E03A = 25;

	private ComputeBuffer _E03B;

	private int _E03C;

	private int _E03D = 1;

	private int _E03E = 2;

	private float _E03F;

	private int _E040;

	private int _E041;

	private string _E042 = _ED3E._E000(87349);

	private string _E043 = _ED3E._E000(87331);

	private string _E044 = _ED3E._E000(87375);

	private string _E045 = _ED3E._E000(87413);

	private string _E046 = _ED3E._E000(87455);

	private string _E047 = _ED3E._E000(87486);

	private string _E048 = _ED3E._E000(87471);

	private string _E049 = _ED3E._E000(87512);

	private string _E04A = _ED3E._E000(87497);

	private string _E04B = _ED3E._E000(87536);

	private string _E04C = _ED3E._E000(87520);

	private string _E04D = _ED3E._E000(87568);

	private string _E04E = _ED3E._E000(87559);

	private string _E04F = _ED3E._E000(87600);

	private int _E050 = Shader.PropertyToID(_ED3E._E000(87640));

	private Camera _E051;

	public ResolutionState CurrentMaskResolution;

	private int _E052 = 4;

	private List<_E000> _E053 = new List<_E000>();

	private float _E054;

	private float _E055;

	private Vector3 _E056 = new Vector3(0f, 1f, 0f);

	private RenderTexture[] _E057;

	private int _E058 = 3;

	private RenderTexture _E059;

	private List<_E001> _E05A = new List<_E001>();

	private MaterialPropertyBlock _E05B;

	private MaterialPropertyBlock _E05C;

	public Material DownscaleDepthMaterial;

	public Material ComputeShadowMaskMaterial;

	public Material UpscaleShadowMaskMaterial;

	public Material ShadowMaskBlurMaterial;

	private Mesh _E05D;

	private void Awake()
	{
		_E018 = GetComponent<SSAA>();
		if (ShouldApplySettings)
		{
			_E000();
		}
		_E00E();
		_E059 = _E057[_E03E];
		for (int i = 0; i < _E058; i++)
		{
			_E053.Add(new _E000());
		}
		_E00F();
		if (TOD_Sky.Instance == null)
		{
			Debug.LogWarning(_ED3E._E000(43785));
			base.gameObject.SetActive(value: false);
		}
	}

	private void _E000()
	{
		if (Singleton<_E7DE>.Instantiated)
		{
			GameSetting<int> shadowsQuality = Singleton<_E7DE>.Instance.Graphics.Settings.ShadowsQuality;
			if ((int)shadowsQuality == 0)
			{
				_E058 = 3;
				EnableShadowsBlend = true;
				ObjectsPerFrame = 10;
			}
			else if ((int)shadowsQuality == 1)
			{
				_E058 = 3;
				EnableShadowsBlend = true;
				ObjectsPerFrame = 20;
			}
			else if ((int)shadowsQuality == 2)
			{
				_E058 = 3;
				EnableShadowsBlend = true;
				ObjectsPerFrame = 35;
			}
			else if ((int)shadowsQuality == 3)
			{
				_E058 = 3;
				EnableShadowsBlend = true;
				ObjectsPerFrame = 50;
			}
			else
			{
				Debug.LogError(_ED3E._E000(43824));
			}
		}
	}

	private int _E001()
	{
		_E05A.Clear();
		foreach (LODGroup item in FindObjectsOfTypeAll<LODGroup>())
		{
			LOD[] lODs = item.GetLODs();
			if (lODs.Length < 1)
			{
				continue;
			}
			int num = lODs.Length - 1;
			Renderer[] renderers = lODs[num].renderers;
			num--;
			List<_E001> list = null;
			while (num >= 0 && list == null)
			{
				renderers = lODs[num].renderers;
				bool flag = false;
				for (int i = 0; i < renderers.Length; i++)
				{
					if (renderers[i] != null && renderers[i].shadowCastingMode != 0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					list = new List<_E001>();
					for (int j = 0; j < renderers.Length; j++)
					{
						if (!(renderers[j] != null) || renderers[j].shadowCastingMode == ShadowCastingMode.Off || !(renderers[j].sharedMaterial != null) || !renderers[j].gameObject.activeInHierarchy)
						{
							continue;
						}
						MeshFilter component = renderers[j].gameObject.GetComponent<MeshFilter>();
						if (component == null || component.sharedMesh == null || Mathf.Max(renderers[j].bounds.size.x, Mathf.Max(renderers[j].bounds.size.y, renderers[j].bounds.size.z)) < ObjectSizeFilter)
						{
							continue;
						}
						for (int k = 0; k < component.sharedMesh.subMeshCount; k++)
						{
							Material material = renderers[j].sharedMaterial;
							if (renderers[j].sharedMaterials != null && k < renderers[j].sharedMaterials.Length)
							{
								material = renderers[j].sharedMaterials[k];
							}
							if ((bool)material)
							{
								if (material.enableInstancing)
								{
									Matrix4x4[] matrices = new Matrix4x4[1] { renderers[j].gameObject.transform.localToWorldMatrix };
									_E026.DrawMeshInstanced(component.sharedMesh, k, material, 0, matrices);
								}
								else
								{
									_E026.DrawMesh(component.sharedMesh, renderers[j].gameObject.transform.localToWorldMatrix, material, k, 0);
								}
								_E001 obj = new _E001();
								obj.mesh = component.sharedMesh;
								obj.material = material;
								obj.submeshIdx = k;
								obj.drawTransform = renderers[j].gameObject.transform.localToWorldMatrix;
								list.Add(obj);
							}
						}
					}
				}
				num--;
			}
			if (list != null)
			{
				_E05A.AddRange(list);
			}
		}
		return _E05A.Count;
	}

	private Matrix4x4 _E002()
	{
		return Matrix4x4.Ortho(0f - GlobalShadowHalfSize, GlobalShadowHalfSize, 0f - GlobalShadowHalfSize, GlobalShadowHalfSize, 0f - NearPlane, 0f - FarPlane);
	}

	public static List<T> FindObjectsOfTypeAll<T>()
	{
		List<T> list = new List<T>();
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene sceneAt = SceneManager.GetSceneAt(i);
			if (sceneAt.isLoaded)
			{
				GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
				foreach (GameObject gameObject in rootGameObjects)
				{
					list.AddRange(gameObject.GetComponentsInChildren<T>(includeInactive: true));
				}
			}
		}
		return list;
	}

	private float _E003(double angleCosSpeed, double angleCos)
	{
		return (float)(-2428668480.0 * angleCosSpeed + 7136641.3 * angleCos - 1821580210.0 * angleCosSpeed * angleCos + 484576146000.0 * angleCosSpeed * angleCosSpeed + 4623877.04 * angleCos * angleCos + 3052001.66);
	}

	private int _E004()
	{
		if (!(_E018 != null))
		{
			return Screen.width;
		}
		return _E018.GetInputWidth();
	}

	private int _E005()
	{
		if (!(_E018 != null))
		{
			return Screen.height;
		}
		return _E018.GetInputHeight();
	}

	private void _E006(RenderTexture renderTexture)
	{
		renderTexture.Release();
		UnityEngine.Object.DestroyImmediate(renderTexture);
	}

	private RenderTexture _E007(RenderTexture initialRenderTexture, int width, int height, string name, GraphicsFormat format, bool UAV = false, int depth = 0)
	{
		if (initialRenderTexture != null)
		{
			_E006(initialRenderTexture);
		}
		RenderTexture renderTexture = new RenderTexture(width, height, depth, format);
		renderTexture.name = name;
		if (UAV)
		{
			if (renderTexture.IsCreated())
			{
				renderTexture.Release();
			}
			renderTexture.enableRandomWrite = UAV;
			renderTexture.Create();
		}
		return renderTexture;
	}

	private RenderTexture _E008(RenderTexture initialRenderTexture, int width, int height, string name, RenderTextureFormat format, bool UAV = false, int depth = 0)
	{
		if (initialRenderTexture != null)
		{
			_E006(initialRenderTexture);
		}
		RenderTexture renderTexture = new RenderTexture(width, height, depth, format);
		renderTexture.name = name;
		if (UAV)
		{
			if (renderTexture.IsCreated())
			{
				renderTexture.Release();
			}
			renderTexture.enableRandomWrite = UAV;
			renderTexture.Create();
		}
		return renderTexture;
	}

	private void OnDestroy()
	{
		_E02E?.Release();
		_E02E = null;
		_E031?.Release();
		_E031 = null;
		_E036?.Release();
		_E036 = null;
		_E037?.Release();
		_E037 = null;
		_E033?.Release();
		_E033 = null;
		_E02F?.Release();
		_E02F = null;
		_E032?.Release();
		_E032 = null;
		_E038?.Release();
		_E038 = null;
		_E039?.Release();
		_E039 = null;
		_E034?.Release();
		_E034 = null;
		for (int i = 0; i < _E057.Length; i++)
		{
			_E057[i]?.Release();
			_E057[i] = null;
		}
		_E026?.Dispose();
		_E026 = null;
		_E027?.Dispose();
		_E027 = null;
		_E028?.Dispose();
		_E028 = null;
		_E029?.Dispose();
		_E029 = null;
		_E02A?.Dispose();
		_E02A = null;
		_E02B?.Dispose();
		_E02B = null;
		_E02C?.Dispose();
		_E02C = null;
		_E02D?.Dispose();
		_E02D = null;
		_E03B?.Dispose();
		_E03B = null;
	}

	private void Update()
	{
		if (!MasterCamera.enabled)
		{
			return;
		}
		if (_E018 == null)
		{
			_E018 = MasterCamera.GetComponent<SSAA>();
		}
		if (!_E00F())
		{
			return;
		}
		if (_E022 == 0)
		{
			_E023 += Time.deltaTime;
		}
		if (_E03B == null)
		{
			Vector2[] data = new Vector2[25]
			{
				new Vector2(-0.74528f, 0.10120783f),
				new Vector2(-0.34528f, 0.38692212f),
				new Vector2(0.05472f, 0.6726364f),
				new Vector2(0.45472f, 0.95835066f),
				new Vector2(0.85472f, -0.7151187f),
				new Vector2(-0.66528f, -0.4294044f),
				new Vector2(-0.26528f, -0.14369012f),
				new Vector2(0.13472f, 0.14202416f),
				new Vector2(0.53472f, 0.42773843f),
				new Vector2(0.93472f, 0.71345276f),
				new Vector2(-0.96928f, 0.999167f),
				new Vector2(-0.56928f, -0.999881f),
				new Vector2(-0.16928f, -0.7141667f),
				new Vector2(0.23072f, -0.42845243f),
				new Vector2(0.63072f, -0.14273815f),
				new Vector2(-0.88928f, 0.14297614f),
				new Vector2(-0.48928f, 0.42869043f),
				new Vector2(-0.08928f, 0.7144047f),
				new Vector2(0.31072f, -0.95906466f),
				new Vector2(0.71072f, -0.6733504f),
				new Vector2(-0.80928f, -0.3876361f),
				new Vector2(-0.40928f, -0.10192182f),
				new Vector2(-0.00928f, 0.18379247f),
				new Vector2(0.39072f, 0.46950674f),
				new Vector2(0.79072f, 0.755221f)
			};
			int stride = Marshal.SizeOf(typeof(Vector2));
			_E03B = new ComputeBuffer(25, stride, ComputeBufferType.Default);
			_E03B.SetData(data);
		}
		int num = _E004();
		int num2 = _E005();
		switch (CurrentMaskResolution)
		{
		case ResolutionState.QUARTER:
			_E052 = 4;
			break;
		case ResolutionState.HALF:
			_E052 = 2;
			break;
		case ResolutionState.FULL:
			_E052 = 1;
			break;
		}
		int num3 = num / _E052;
		int num4 = num2 / _E052;
		if (_E02E == null || num3 != _E02E.width || num4 != _E02E.height)
		{
			_E02E = _E007(_E02E, num3, num4, _ED3E._E000(43900), GraphicsFormat.R32_SFloat, UAV: true);
			_E031 = _E007(_E031, num3, num4, _ED3E._E000(43890), GraphicsFormat.R8_UNorm, UAV: true);
			_E036 = _E007(_E036, num, num2, _ED3E._E000(43934), GraphicsFormat.R8_UNorm, UAV: true);
			_E037 = _E007(_E037, num, num2, _ED3E._E000(43919), GraphicsFormat.R8_UNorm);
			_E033 = _E007(_E033, num, num2, _ED3E._E000(43963), GraphicsFormat.R8_UNorm, UAV: true);
		}
		if (_E051 == null)
		{
			_E051 = _E8A8.Instance.OpticCameraManager.Camera;
			if (_E051 != null)
			{
				_E051.AddCommandBuffer(CameraEvent.BeforeGBuffer, _E02B);
				_E051.AddCommandBuffer(CameraEvent.AfterEverything, _E02A);
				_E051.AddCommandBuffer(CameraEvent.BeforeLighting, _E02D);
			}
		}
		if (_E051 != null && _E051.targetTexture != null && (_E02F == null || _E051.targetTexture.width / _E052 != _E02F.width || _E051.targetTexture.height / _E052 != _E02F.height))
		{
			_E02F = _E007(_E02F, _E051.targetTexture.width / _E052, _E051.targetTexture.height / _E052, _ED3E._E000(43950), GraphicsFormat.R32_SFloat, UAV: true);
			_E032 = _E007(_E032, _E051.targetTexture.width / _E052, _E051.targetTexture.height / _E052, _ED3E._E000(43993), GraphicsFormat.R8_UNorm, UAV: true);
			_E038 = _E007(_E038, _E051.targetTexture.width, _E051.targetTexture.height, _ED3E._E000(43970), GraphicsFormat.R8_UNorm, UAV: true);
			_E039 = _E007(_E039, _E051.targetTexture.width, _E051.targetTexture.height, _ED3E._E000(44008), GraphicsFormat.R8_UNorm);
			_E034 = _E007(_E034, _E051.targetTexture.width, _E051.targetTexture.height, _ED3E._E000(87057), GraphicsFormat.R8_UNorm, UAV: true);
		}
		_E00E();
		while (_E058 > _E053.Count)
		{
			_E053.Add(new _E000());
		}
		_E00D();
		bool flag = false;
		if (_E040 == 0 && _E041 == 0)
		{
			if (InvalidateRenderersOnNextUpdate && _E001() > 0)
			{
				InvalidateRenderersOnNextUpdate = false;
			}
			flag = true;
			_E03F = 0f;
			if (_E054 > 0f)
			{
				_E055 = Time.time - _E054;
				LastTimeToUpdateInfo = _E055;
				LastTimeToUpdate = _E055;
			}
			_E054 = Time.time;
			_E03C = _E03D;
			_E03D = _E03E;
			_E03E = (_E03E + 1) % _E057.Length;
			_E059 = _E057[_E03E];
			_E00C(_E03E);
			_E040 = _E05A.Count;
			_E041 = MonoBehaviourSingleton<ImpostorsRenderer>.Instance?.ImpostorsToRenderCount ?? 0;
		}
		int num5 = ObjectsPerFrame;
		if (_E021 > 0)
		{
			_E020 += Time.deltaTime;
			if (_E020 > TOD_Sky.Instance.Light.UpdateInterval)
			{
				_E021--;
				_E020 = 0f;
				num5 = _E040;
			}
			else
			{
				num5 = 1;
			}
			_E041 = 1;
		}
		ObjectsToRenderInfo = _E040 + _E041;
		ObjectsToRenderPerFrameInfo = num5;
		_E026.Clear();
		_E026.SetViewMatrix(_E053[_E03E]._cachedGlobalShadowWorldToView);
		Matrix4x4 cachedGlobalShadowProj = _E053[_E03E]._cachedGlobalShadowProj;
		_E026.SetProjectionMatrix(cachedGlobalShadowProj);
		_E026.SetRenderTarget(_E059);
		if (flag)
		{
			_E026.ClearRenderTarget(clearDepth: true, clearColor: false, Color.white);
		}
		int num6 = 0;
		while (num6 < num5 && _E040 > 0)
		{
			_E040--;
			_E001 obj = _E05A[_E040];
			_E026.DrawMesh(obj.mesh, obj.drawTransform, obj.material, obj.submeshIdx, 0);
			num6++;
			if (_E040 == 0)
			{
				_E041 = MonoBehaviourSingleton<ImpostorsRenderer>.Instance?.ImpostorsToRenderCount ?? 0;
			}
		}
		if (_E040 == 0 && _E041 > 0)
		{
			int num7 = num5 - num6;
			if (num7 > 0)
			{
				uint impostorsPerFrame = (uint)(num7 * _E025);
				MonoBehaviourSingleton<ImpostorsRenderer>.Instance?._E00C(MasterCamera, _E026, impostorsPerFrame, out _E041);
			}
		}
		int num8 = _E03D;
		int num9 = _E03C;
		Vector4 vector = new Vector4(1f / (float)_E057[0].width, 1f / (float)_E057[0].height, _E057[0].width, _E057[0].height);
		float y = ((QualitySettings.shadowCascades < 4) ? 1f : 0f);
		_E03F += Time.smoothDeltaTime;
		float num10 = Mathf.Min(BlendTime, Mathf.Max(_E055, 0.01f));
		Vector4 vector2 = new Vector4(0.25f, y, _E03F / num10, _E053[num8]._cachedFarPlane);
		if (PreComputeMask)
		{
			if (_E02C != null && DownscaleDepthMaterial != null && ComputeShadowMaskMaterial != null && UpscaleShadowMaskMaterial != null)
			{
				_E02C.Clear();
				_E009(_E02C, num8, num9, vector2, vector, new Rect(0f, 0f, _E02E.width, _E02E.height), new Rect(0f, 0f, _E036.width, _E036.height), CurrentMaskResolution, _E02E, _E031, _E033, _E036, _E037);
				if (_E051 != null && _E051.targetTexture != null)
				{
					_E02D.Clear();
					_E009(_E02D, num8, num9, vector2, vector, new Rect(0f, 0f, _E02F.width, _E02F.height), new Rect(0f, 0f, _E038.width, _E038.height), CurrentMaskResolution, _E02F, _E032, _E034, _E038, _E039);
				}
			}
			if (_E027 != null)
			{
				_E027.Clear();
			}
		}
		else if (_E027 != null)
		{
			_E027.Clear();
			_E027.SetGlobalTexture(_E050, _E057[num8]);
			_E027.SetGlobalTexture(DistantShadow.m__E000, _E057[num8]);
			_E027.SetGlobalMatrix(DistantShadow.m__E001, _E053[num8]._cachedGlobalShadowWorldToScreen);
			_E027.SetGlobalMatrix(DistantShadow.m__E002, _E053[num8]._cachedGlobalShadowWorldToView);
			_E027.SetGlobalMatrix(DistantShadow.m__E003, _E053[num8]._cachedGlobalShadowProjGPU);
			float x = (EnableAdaptiveBias ? 1f : 0f);
			float adaptiveBiasK = AdaptiveBiasK;
			Vector4 value = new Vector4(x, ConstantBias, adaptiveBiasK, PCFShift);
			_E027.SetGlobalVector(DistantShadow.m__E004, value);
			if (EnableShadowsBlend)
			{
				_E027.SetGlobalTexture(DistantShadow.m__E005, _E057[num9]);
				_E027.SetGlobalMatrix(DistantShadow.m__E006, _E053[num9]._cachedGlobalShadowWorldToScreen);
				_E027.SetGlobalMatrix(DistantShadow.m__E007, _E053[num9]._cachedGlobalShadowWorldToView);
				_E027.SetGlobalMatrix(DistantShadow.m__E008, _E053[num9]._cachedGlobalShadowProjGPU);
			}
			_E027.SetGlobalVector(DistantShadow.m__E009, vector2);
			_E027.SetGlobalVector(DistantShadow.m__E00A, vector);
			Vector4 value2 = new Vector4(SamplingRotation / 180f, 0f, 0f, 0f);
			_E027.SetGlobalVector(DistantShadow.m__E00B, value2);
			_E027.SetGlobalVector(DistantShadow.m__E00C, _E053[num8]._cachedLightDirection);
			_E027.SetGlobalVector(DistantShadow.m__E00D, -TOD_Sky.Instance.Components.LightTransform.forward);
		}
		if (TOD_Sky.Instance != null)
		{
			_E01B += Time.deltaTime;
			if (!_E01C)
			{
				_E01A = TOD_Sky.Instance.Components.LightTransform.forward.y;
				_E019 = 0f;
				_E01C = true;
			}
			if (_E01A != TOD_Sky.Instance.Components.LightTransform.forward.y)
			{
				_E019 = Mathf.Abs(_E01A - TOD_Sky.Instance.Components.LightTransform.forward.y) / _E01B;
				_E01B = 0f;
			}
			else if (_E01B > TOD_Sky.Instance.Light.UpdateInterval)
			{
				_E019 = 0f;
			}
			_E01A = TOD_Sky.Instance.Components.LightTransform.forward.y;
		}
	}

	private void _E009(CommandBuffer cmdBuf, int targetIdx, int targetIdxOld, Vector4 GlobalShadowProj, Vector4 TextureParams, Rect lowResViewport, Rect fullResViewport, ResolutionState resState, RenderTexture lowResDepth, RenderTexture lowResShadowMask, RenderTexture blurMask, RenderTexture hiResShadowMask, RenderTexture hiResShadowMaskBuf)
	{
		cmdBuf.SetGlobalTexture(_E050, _E057[targetIdx]);
		cmdBuf.SetGlobalTexture(DistantShadow.m__E000, _E057[targetIdx]);
		cmdBuf.SetGlobalMatrix(DistantShadow.m__E001, _E053[targetIdx]._cachedGlobalShadowWorldToScreen);
		cmdBuf.SetGlobalMatrix(DistantShadow.m__E002, _E053[targetIdx]._cachedGlobalShadowWorldToView);
		cmdBuf.SetGlobalMatrix(DistantShadow.m__E003, _E053[targetIdx]._cachedGlobalShadowProjGPU);
		float num = _E01D;
		if (_E01E)
		{
			float num2 = num;
			if (_E019 != 0f)
			{
				num2 = _E003(_E019, _E01A);
				_ = 0f;
				num2 = Mathf.Max(num2, 0f);
			}
			else
			{
				num2 = 0f;
			}
			_ = _E01D;
			num2 = Mathf.Min(num, _E01D);
			num = Mathf.Lerp(num, num2, _E01F);
		}
		num *= Mathf.Clamp01(_E023 / 10f);
		cmdBuf.SetGlobalFloat(DistantShadow.m__E00E, num);
		float x = (EnableAdaptiveBias ? 1f : 0f);
		float adaptiveBiasK = AdaptiveBiasK;
		cmdBuf.SetGlobalVector(value: new Vector4(x, ConstantBias, adaptiveBiasK, PCFShift), nameID: DistantShadow.m__E004);
		cmdBuf.SetGlobalVector(value: new Vector4(_E004(), _E005(), 1f / (float)_E004(), 1f / (float)_E005()), nameID: DistantShadow.m__E00F);
		if (EnableShadowsBlend)
		{
			cmdBuf.SetGlobalTexture(DistantShadow.m__E005, _E057[targetIdxOld]);
			cmdBuf.SetGlobalMatrix(DistantShadow.m__E006, _E053[targetIdxOld]._cachedGlobalShadowWorldToScreen);
			cmdBuf.SetGlobalMatrix(DistantShadow.m__E007, _E053[targetIdxOld]._cachedGlobalShadowWorldToView);
			cmdBuf.SetGlobalMatrix(DistantShadow.m__E008, _E053[targetIdxOld]._cachedGlobalShadowProjGPU);
		}
		cmdBuf.SetGlobalVector(DistantShadow.m__E009, GlobalShadowProj);
		cmdBuf.SetGlobalVector(DistantShadow.m__E00A, TextureParams);
		cmdBuf.SetGlobalVector(value: new Vector4(SamplingRotation / 180f, 0f, 0f, 0f), nameID: DistantShadow.m__E00B);
		cmdBuf.SetGlobalVector(DistantShadow.m__E00C, _E053[targetIdx]._cachedLightDirection);
		cmdBuf.SetGlobalVector(_E010, _E053[targetIdxOld]._cachedLightDirection);
		if (TOD_Sky.Instance != null)
		{
			cmdBuf.SetGlobalVector(DistantShadow.m__E00D, -TOD_Sky.Instance.Components.LightTransform.forward);
		}
		if (resState == ResolutionState.QUARTER || resState == ResolutionState.HALF)
		{
			cmdBuf.SetRenderTarget(lowResDepth);
			cmdBuf.SetViewport(lowResViewport);
			if (resState == ResolutionState.QUARTER)
			{
				cmdBuf.EnableShaderKeyword(_ED3E._E000(87041));
			}
			else
			{
				cmdBuf.EnableShaderKeyword(_ED3E._E000(87095));
			}
			cmdBuf.DrawMesh(_E05D, Matrix4x4.identity, DownscaleDepthMaterial);
		}
		if (resState == ResolutionState.FULL)
		{
			cmdBuf.SetRenderTarget(hiResShadowMask);
			cmdBuf.SetViewport(fullResViewport);
			cmdBuf.DisableShaderKeyword(_ED3E._E000(87082));
		}
		else
		{
			cmdBuf.SetRenderTarget(lowResShadowMask);
			cmdBuf.SetViewport(lowResViewport);
			cmdBuf.SetGlobalTexture(_E016, lowResDepth);
			cmdBuf.EnableShaderKeyword(_ED3E._E000(87082));
		}
		cmdBuf.SetGlobalBuffer(_E011, _E03B);
		cmdBuf.DrawMesh(_E05D, Matrix4x4.identity, ComputeShadowMaskMaterial, 0, 0);
		if (resState == ResolutionState.QUARTER || resState == ResolutionState.HALF)
		{
			cmdBuf.SetGlobalTexture(_E012, lowResShadowMask);
			if (EnableShadowmaskBlur && EnableBlurMask && resState == ResolutionState.QUARTER)
			{
				cmdBuf.SetRenderTarget(blurMask);
				cmdBuf.SetViewport(fullResViewport);
				cmdBuf.DrawMesh(_E05D, Matrix4x4.identity, ComputeShadowMaskMaterial, 0, 1);
				cmdBuf.SetGlobalTexture(_E013, blurMask);
			}
			cmdBuf.SetRenderTarget(hiResShadowMask);
			cmdBuf.SetViewport(fullResViewport);
			if (EnableBlurMask)
			{
				cmdBuf.ClearRenderTarget(clearDepth: false, clearColor: true, Color.white);
			}
			cmdBuf.DrawMesh(_E05D, Matrix4x4.identity, UpscaleShadowMaskMaterial);
		}
		if (EnableShadowmaskBlur)
		{
			if (FixGlowEffect)
			{
				cmdBuf.EnableShaderKeyword(_E04F);
			}
			cmdBuf.SetRenderTarget(hiResShadowMaskBuf);
			cmdBuf.SetGlobalFloat(_E017, BlurDepthThreshold);
			if (EnableBlurMask)
			{
				cmdBuf.ClearRenderTarget(clearDepth: false, clearColor: true, Color.white);
			}
			if (_E05B == null)
			{
				_E05B = new MaterialPropertyBlock();
			}
			_E05B.Clear();
			_E05B.SetTexture(_E014, hiResShadowMask);
			cmdBuf.DrawMesh(_E05D, Matrix4x4.identity, ShadowMaskBlurMaterial, 0, 0, _E05B);
			cmdBuf.SetRenderTarget(hiResShadowMask);
			if (EnableBlurMask)
			{
				cmdBuf.ClearRenderTarget(clearDepth: false, clearColor: true, Color.white);
			}
			if (_E05C == null)
			{
				_E05C = new MaterialPropertyBlock();
			}
			_E05C.Clear();
			_E05C.SetTexture(_E014, hiResShadowMaskBuf);
			cmdBuf.DrawMesh(_E05D, Matrix4x4.identity, ShadowMaskBlurMaterial, 0, 1, _E05C);
			cmdBuf.DisableShaderKeyword(_E04F);
		}
		if (PreComputeMask)
		{
			cmdBuf.SetGlobalTexture(_E015, hiResShadowMask);
		}
	}

	public void WarmupForFrames(int frames)
	{
		_E021 = frames;
	}

	private void _E00A(CommandBuffer cmdBuf, bool opticCamera)
	{
		if (EnableShadowsBlend)
		{
			cmdBuf.EnableShaderKeyword(_E042);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E042);
		}
		if (EnableAdaptiveBias)
		{
			cmdBuf.EnableShaderKeyword(_E043);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E043);
		}
		if (EnableDistantShadowKeyword)
		{
			cmdBuf.EnableShaderKeyword(_E044);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E044);
		}
		if (EnableDistantShadowPCF)
		{
			cmdBuf.EnableShaderKeyword(_E045);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E045);
		}
		if (EnableMultiviewTiles)
		{
			cmdBuf.EnableShaderKeyword(_E046);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E046);
		}
		if (EnablePCFShift)
		{
			cmdBuf.EnableShaderKeyword(_E047);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E047);
		}
		if (DisableWind)
		{
			cmdBuf.EnableShaderKeyword(_E048);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E048);
		}
		if (PreComputeMask)
		{
			cmdBuf.EnableShaderKeyword(_E04B);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E04B);
		}
		if (EnableParallax && _E022 == 0)
		{
			cmdBuf.EnableShaderKeyword(_E04C);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E04C);
		}
		if (UseGaussDistribution)
		{
			cmdBuf.EnableShaderKeyword(_E04A);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E04A);
		}
		if (ReduceBlurSamples)
		{
			cmdBuf.EnableShaderKeyword(_E04D);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E04D);
		}
		if (opticCamera)
		{
			cmdBuf.EnableShaderKeyword(_E049);
		}
		if (EnableShadowmaskBlur && EnableBlurMask && CurrentMaskResolution == ResolutionState.QUARTER)
		{
			cmdBuf.EnableShaderKeyword(_E04E);
		}
		else
		{
			cmdBuf.DisableShaderKeyword(_E04E);
		}
	}

	private void _E00B(CommandBuffer cmdBuf)
	{
		cmdBuf.DisableShaderKeyword(_E042);
		cmdBuf.DisableShaderKeyword(_E043);
		cmdBuf.DisableShaderKeyword(_E044);
		cmdBuf.DisableShaderKeyword(_E045);
		cmdBuf.DisableShaderKeyword(_E046);
		cmdBuf.DisableShaderKeyword(_E047);
		cmdBuf.DisableShaderKeyword(_E048);
		cmdBuf.DisableShaderKeyword(_E04B);
		cmdBuf.DisableShaderKeyword(_E04C);
		cmdBuf.DisableShaderKeyword(_E049);
		cmdBuf.DisableShaderKeyword(_E04E);
		cmdBuf.DisableShaderKeyword(_E04D);
	}

	private void _E00C(int idx)
	{
		if (_E021 == 0 && _E022 > 0)
		{
			_E022--;
		}
		_E053[idx]._cachedShadowCameraPosition = base.transform.position;
		_E053[idx]._cachedLightDirection = -base.transform.forward;
		Matrix4x4 matrix4x = _E002();
		_E053[idx]._cachedGlobalShadowWorldToScreen = GL.GetGPUProjectionMatrix(matrix4x, renderIntoTexture: false) * base.transform.worldToLocalMatrix;
		_E053[idx]._cachedGlobalShadowWorldToView = base.transform.worldToLocalMatrix;
		_E053[idx]._cachedGlobalShadowProjGPU = GL.GetGPUProjectionMatrix(matrix4x, renderIntoTexture: false);
		_E053[idx]._cachedGlobalShadowProj = matrix4x;
		_E053[idx]._cachedNearPlane = NearPlane;
		_E053[idx]._cachedFarPlane = FarPlane;
		_E053[idx]._cachedShadowCameraForward = base.transform.forward;
		_E053[idx]._cachedShadowCameraRight = base.transform.right;
		_E053[idx]._cachedShadowCameraUp = base.transform.up;
		_E053[idx]._buffersDataIsPropagated = false;
	}

	private void _E00D()
	{
		float num = 0f;
		Vector3 position = MasterCamera.transform.position;
		Vector3 forward = MasterCamera.transform.forward;
		if (TOD_Sky.Instance != null)
		{
			_E056 = TOD_Sky.Instance.LightDirectionExtrapolated(_E055 * LightDirectionExtrapolateFactor);
		}
		base.transform.position = position + _E056 * OffsetTowardsLight + num * forward;
		base.transform.LookAt(base.transform.position - _E056);
	}

	private void _E00E()
	{
		if (_E057 == null || _E057.Length != _E058)
		{
			_E057 = new RenderTexture[_E058];
			_E03C = 0;
			_E03D = 1;
			_E03E = 2 % _E058;
			_E040 = 0;
		}
		for (int i = 0; i < _E058; i++)
		{
			if (_E057[i] == null || _E057[i].width != ShadowPixelsSize || _E057[i].height != ShadowPixelsSize)
			{
				string text = _ED3E._E000(87135) + i;
				_E057[i] = _E008(_E057[i], ShadowPixelsSize, ShadowPixelsSize, text, RenderTextureFormat.Depth, UAV: false, 16);
				_E040 = 0;
			}
		}
	}

	private bool _E00F()
	{
		if (MasterCamera == null)
		{
			return false;
		}
		if (_E026 == null)
		{
			_E026 = new CommandBuffer();
			_E026.name = _ED3E._E000(87122);
			MasterCamera.AddCommandBuffer(CameraEvent.AfterEverything, _E026);
		}
		if (_E027 == null)
		{
			_E027 = new CommandBuffer();
			_E027.name = _ED3E._E000(87167);
			MasterCamera.AddCommandBuffer(CameraEvent.BeforeLighting, _E027);
		}
		if (_E028 == null)
		{
			_E028 = new CommandBuffer();
			_E028.name = _ED3E._E000(87146);
			MasterCamera.AddCommandBuffer(CameraEvent.AfterEverything, _E028);
			_E00B(_E028);
		}
		if (_E029 == null)
		{
			_E029 = new CommandBuffer();
			_E029.name = _ED3E._E000(87183);
			MasterCamera.AddCommandBuffer(CameraEvent.BeforeGBuffer, _E029);
		}
		if (_E02C == null)
		{
			_E02C = new CommandBuffer();
			_E02C.name = _ED3E._E000(87219);
			MasterCamera.AddCommandBuffer(CameraEvent.BeforeLighting, _E02C);
			_E02D = new CommandBuffer();
			_E02D.name = _ED3E._E000(87252);
		}
		if (_E02A == null)
		{
			_E02A = new CommandBuffer();
			_E02A.name = _ED3E._E000(87290);
			_E00B(_E02A);
		}
		if (_E02B == null)
		{
			_E02B = new CommandBuffer();
			_E02B.name = _ED3E._E000(87316);
		}
		if (_E05D == null)
		{
			_E05D = new Mesh();
			Vector3[] vertices = new Vector3[4]
			{
				new Vector3(-1f, -1f, 0f),
				new Vector3(-1f, 3f, 0f),
				new Vector3(3f, -1f, 0f),
				default(Vector3)
			};
			_E05D.vertices = vertices;
			Vector2[] uv = new Vector2[4]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 2f),
				new Vector2(2f, 0f),
				default(Vector2)
			};
			_E05D.uv = uv;
			int[] triangles = new int[6] { 0, 1, 2, 0, 0, 0 };
			_E05D.triangles = triangles;
		}
		_E029.Clear();
		_E00A(_E029, opticCamera: false);
		_E02B.Clear();
		_E00A(_E02B, opticCamera: true);
		return true;
	}

	public void DisableFeature()
	{
		if (_E026 != null)
		{
			_E026.Clear();
		}
		if (_E02C != null)
		{
			_E02C.Clear();
		}
		if (_E02D != null)
		{
			_E02D.Clear();
		}
		if (_E027 != null)
		{
			_E027.Clear();
		}
		if (_E029 != null)
		{
			_E00B(_E029);
		}
		if (_E02B != null)
		{
			_E00B(_E02B);
		}
		base.gameObject.SetActive(value: false);
	}

	public void EnableFeature()
	{
		if (EnableDistantShadowKeyword)
		{
			if (_E029 != null)
			{
				_E00A(_E029, opticCamera: false);
			}
			if (_E02B != null)
			{
				_E00A(_E02B, opticCamera: true);
			}
		}
		base.gameObject.SetActive(value: true);
	}
}
