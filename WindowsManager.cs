using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EFT.BlitDebug;
using EFT.Interactive;
using GPUInstancer;
using Koenigz.PerfectCulling.EFT;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

[ExecuteAlways]
public class WindowsManager : MonoBehaviour
{
	private class _E000
	{
		public CommandBuffer CmdBufCulling;

		public CommandBuffer CmdBufDrawing;

		public CommandBuffer CmdBufBeforeTransparent;

		public ComputeBuffer FrustumPlanes;

		public ComputeBuffer FrustumPNFactors;

		public ComputeBuffer DrawArgsBuffer;

		public ComputeBuffer InstancesIDsAfterFrustumCulling;

		public ComputeBuffer InstancesIDsCount;

		public ComputeBuffer OcclusionComputeGroupsCountBuffer;

		public ComputeBuffer CameraProjViewMatrixBuffer;

		public ComputeBuffer CameraParameters;

		public List<Matrix4x4> ProjViewMatrix = new List<Matrix4x4>();

		public GPUInstancerHiZOcclusionGenerator HiZGenerator;

		public RenderTexture MomentsRT;

		public RenderTexture MomentsRT1;

		public RenderTexture TransmittanceSumRcp;

		public RenderTexture StubDepth;

		public RenderTexture OpaqueRT;

		public int RenderTexturesWidth;

		public int RenderTexturesHeight;

		public RenderTargetIdentifier[] _oitRTIs = new RenderTargetIdentifier[2];

		public RenderTargetIdentifier _oitSumRcp;

		public RenderTargetIdentifier _stubDepthRTI;

		public RenderTargetIdentifier _opaqueRTI;

		public RenderTargetIdentifier[] _rtisToClear = new RenderTargetIdentifier[3];

		public SSAA _ssaa;

		public SSAAOptic _ssaaOptic;

		public void Release()
		{
			CmdBufDrawing?.Release();
			CmdBufDrawing = null;
			CmdBufBeforeTransparent?.Release();
			CmdBufBeforeTransparent = null;
			CmdBufCulling?.Release();
			CmdBufCulling = null;
			InstancesIDsAfterFrustumCulling?.Release();
			InstancesIDsAfterFrustumCulling = null;
			InstancesIDsCount?.Release();
			InstancesIDsCount = null;
			OcclusionComputeGroupsCountBuffer?.Release();
			OcclusionComputeGroupsCountBuffer = null;
			CameraProjViewMatrixBuffer?.Release();
			CameraProjViewMatrixBuffer = null;
			CameraParameters?.Release();
			CameraParameters = null;
			FrustumPlanes?.Release();
			FrustumPlanes = null;
			FrustumPNFactors?.Release();
			FrustumPNFactors = null;
			DrawArgsBuffer?.Release();
			DrawArgsBuffer = null;
			ReleaseRenderTextures();
		}

		public void ReleaseRenderTextures()
		{
			if (MomentsRT != null)
			{
				MomentsRT.Release();
				UnityEngine.Object.DestroyImmediate(MomentsRT);
			}
			if (MomentsRT1 != null)
			{
				MomentsRT1.Release();
				UnityEngine.Object.DestroyImmediate(MomentsRT1);
			}
			if (TransmittanceSumRcp != null)
			{
				TransmittanceSumRcp.Release();
				UnityEngine.Object.DestroyImmediate(TransmittanceSumRcp);
			}
			if (StubDepth != null)
			{
				StubDepth.Release();
				UnityEngine.Object.DestroyImmediate(StubDepth);
			}
			if (OpaqueRT != null)
			{
				OpaqueRT.Release();
				UnityEngine.Object.DestroyImmediate(OpaqueRT);
			}
		}

		public int GetCameraWidth(Camera camera)
		{
			if (!(_ssaa != null))
			{
				return camera.pixelWidth;
			}
			return _ssaa.GetInputWidth();
		}

		public int GetCameraHeight(Camera camera)
		{
			if (!(_ssaa != null))
			{
				return camera.pixelHeight;
			}
			return _ssaa.GetInputHeight();
		}

		public bool ResolutionMismatched(Camera camera)
		{
			if (GetCameraWidth(camera) != RenderTexturesWidth)
			{
				return true;
			}
			if (GetCameraHeight(camera) != RenderTexturesHeight)
			{
				return true;
			}
			return false;
		}
	}

	[Serializable]
	private struct MeshOffsets
	{
		public int VerticesOffset;

		public int IndicesOffset;

		public int MeshIndex;
	}

	[Serializable]
	private struct TexturesOffsets
	{
		public int MainTexOffset;

		public int SpecTexOffset;

		public int ReflectionCubeOffset;
	}

	[Serializable]
	private struct WindowVertex
	{
		public Vector3 vertex;

		public Vector3 normal;

		public Vector2 uv;
	}

	[Serializable]
	private struct LightProperties
	{
		public Vector4 pos;

		public Vector3 dir;

		public Vector3 color;

		public Matrix4x4 worldToLight;
	}

	[Serializable]
	private struct MaterialParameters
	{
		public float GlobalReflectionStrength;

		public float CustomFogStrength;

		public float ScatterStrength;

		public float Shininess;

		public Vector4 MainColor;

		public Vector4 SpecularColor;

		public Vector4 ReflectionColor;

		public Vector4 MainTexTilingOffset;

		public Vector4 SpecTexTilingOffset;

		public MaterialParameters(Material material, string mainTexName, string specTexName)
		{
			this = default(MaterialParameters);
			Init(material, mainTexName, specTexName);
		}

		public void Init(Material material, string mainTexName, string specTexName)
		{
			Color color = material.GetColor(_ED3E._E000(36528));
			Color color2 = material.GetColor(_ED3E._E000(96236));
			Color color3 = material.GetColor(_ED3E._E000(45489));
			float @float = material.GetFloat(_ED3E._E000(98047));
			float float2 = material.GetFloat(_ED3E._E000(98017));
			float float3 = material.GetFloat(_ED3E._E000(98060));
			float float4 = material.GetFloat(_ED3E._E000(95531));
			Vector2 textureOffset = material.GetTextureOffset(mainTexName);
			Vector2 textureScale = material.GetTextureScale(mainTexName);
			Vector2 textureOffset2 = material.GetTextureOffset(specTexName);
			Vector2 textureScale2 = material.GetTextureScale(specTexName);
			GlobalReflectionStrength = @float;
			CustomFogStrength = float2;
			ScatterStrength = float3;
			Shininess = float4;
			MainColor = new Vector4(color.r, color.g, color.b, color.a);
			SpecularColor = new Vector4(color2.r, color2.g, color2.b, color2.a);
			ReflectionColor = new Vector4(color3.r, color3.g, color3.b, color3.a);
			MainTexTilingOffset = new Vector4(textureScale.x, textureScale.y, textureOffset.x, textureOffset.y);
			SpecTexTilingOffset = new Vector4(textureScale2.x, textureScale2.y, textureOffset2.x, textureOffset2.y);
		}
	}

	[Serializable]
	private class GeometryBuffers
	{
		public List<WindowVertex> _allVertices = new List<WindowVertex>();

		public List<int> _allTriangles = new List<int>();

		public void Clear()
		{
			_allVertices.Clear();
			_allTriangles.Clear();
		}
	}

	[Serializable]
	private class InstancesBuffers
	{
		public List<Matrix4x4> _allTransforms = new List<Matrix4x4>();

		public List<Vector3> _allBounds = new List<Vector3>();

		public List<int> _meshIndex = new List<int>();

		[SerializeField]
		private List<TexturesOffsets> _texturesOffsets = new List<TexturesOffsets>();

		[SerializeField]
		private int _texturesOffsetsCount;

		public List<TexturesOffsets> TexturesOffsetsList => _texturesOffsets;

		public int TexturesOffsetsCount => _texturesOffsetsCount;

		public void Clear()
		{
			_allTransforms.Clear();
			_allBounds.Clear();
			_meshIndex.Clear();
			_texturesOffsets.Clear();
			_texturesOffsetsCount = 0;
		}

		public void AddTextureOffsets(TexturesOffsets offsets)
		{
			_texturesOffsets.Add(offsets);
			_texturesOffsetsCount = _texturesOffsets.Count;
		}
	}

	private class _E001
	{
		public ComputeBuffer _texturesOffsetsBuffer;

		public ComputeBuffer _allVerticesBuffer;

		public ComputeBuffer _verticesIDs;

		public ComputeBuffer _allTransformsBuffer;

		public ComputeBuffer _allBoundsBuffer;

		public ComputeBuffer _allTrianglesBuffer;

		public ComputeBuffer _argsBuffer;

		public ComputeBuffer _verticesInstances;

		public ComputeBuffer _verticesLocalOffsets;

		public ComputeBuffer _instancesMeshesIndices;

		public ComputeBuffer _instanceIdToStartVertexId;

		public ComputeBuffer _meshesOffsets;

		public ComputeBuffer _meshesVerticesCounts;

		public ComputeBuffer _lightsProperties;

		public ComputeBuffer _materialsProperties;

		public ComputeBuffer _instancesEnable;

		private void _E000(ref ComputeBuffer buf)
		{
			buf?.Release();
			buf = null;
		}

		public void Release()
		{
			_E000(ref _texturesOffsetsBuffer);
			_E000(ref _allVerticesBuffer);
			_E000(ref _verticesIDs);
			_E000(ref _allTransformsBuffer);
			_E000(ref _allBoundsBuffer);
			_E000(ref _allTrianglesBuffer);
			_E000(ref _argsBuffer);
			_E000(ref _verticesInstances);
			_E000(ref _verticesLocalOffsets);
			_E000(ref _instancesMeshesIndices);
			_E000(ref _instanceIdToStartVertexId);
			_E000(ref _meshesOffsets);
			_E000(ref _meshesVerticesCounts);
			_E000(ref _lightsProperties);
			_E000(ref _materialsProperties);
			_E000(ref _instancesEnable);
		}
	}

	private class _E002
	{
		public bool Enabled;

		public string PieceId;

		public int PieceIdHash;

		public int InstanceOffset;

		public int VerticesOffset;

		public int VerticesCount;

		public int VerticesInstancesOffset;

		public int VerticesInstancesCount;

		public int TrianglesOffset;

		public int TrianglesCount;

		public int VerticesLocalOffsetsOffset;

		public int VerticesLocalOffsetsCount;

		public int InstanceMeshIndexOffset;

		public int MeshOffsetOffset;

		public int InstanceIdToStartVertexIdOffset;

		public int MeshesVerticesCountOffset;

		public _E002()
		{
			Enabled = true;
		}

		public _E002(_E002 other)
		{
			Enabled = other.Enabled;
			PieceId = other.PieceId;
			PieceIdHash = other.PieceIdHash;
			InstanceOffset = other.InstanceOffset;
			VerticesOffset = other.VerticesOffset;
			VerticesCount = other.VerticesCount;
			VerticesInstancesOffset = other.VerticesInstancesOffset;
			VerticesInstancesCount = other.VerticesInstancesCount;
			TrianglesOffset = other.TrianglesOffset;
			TrianglesCount = other.TrianglesCount;
			VerticesLocalOffsetsOffset = other.VerticesLocalOffsetsOffset;
			VerticesLocalOffsetsCount = other.VerticesLocalOffsetsCount;
			InstanceMeshIndexOffset = other.InstanceMeshIndexOffset;
			MeshOffsetOffset = other.MeshOffsetOffset;
			InstanceIdToStartVertexIdOffset = other.InstanceIdToStartVertexIdOffset;
			MeshesVerticesCountOffset = other.MeshesVerticesCountOffset;
		}

		public _E002(_E002 other, string pieceId, int pieceIdHash)
		{
			Enabled = other.Enabled;
			PieceId = pieceId;
			PieceIdHash = pieceIdHash;
			InstanceOffset = other.InstanceOffset;
			VerticesOffset = other.VerticesOffset;
			VerticesCount = other.VerticesCount;
			VerticesInstancesOffset = other.VerticesInstancesOffset;
			VerticesInstancesCount = other.VerticesInstancesCount;
			TrianglesOffset = other.TrianglesOffset;
			TrianglesCount = other.TrianglesCount;
			VerticesLocalOffsetsOffset = other.VerticesLocalOffsetsOffset;
			VerticesLocalOffsetsCount = other.VerticesLocalOffsetsCount;
			InstanceMeshIndexOffset = other.InstanceMeshIndexOffset;
			MeshOffsetOffset = other.MeshOffsetOffset;
			InstanceIdToStartVertexIdOffset = other.InstanceIdToStartVertexIdOffset;
			MeshesVerticesCountOffset = other.MeshesVerticesCountOffset;
		}

		public void Reset()
		{
			Enabled = false;
			PieceId = "";
			PieceIdHash = 0;
			InstanceOffset = 0;
			VerticesOffset = 0;
			VerticesCount = 0;
			VerticesInstancesOffset = 0;
			VerticesInstancesCount = 0;
			TrianglesOffset = 0;
			TrianglesCount = 0;
			VerticesLocalOffsetsOffset = 0;
			VerticesLocalOffsetsCount = 0;
			InstanceMeshIndexOffset = 0;
			MeshOffsetOffset = 0;
			InstanceIdToStartVertexIdOffset = 0;
			MeshesVerticesCountOffset = 0;
		}
	}

	[Serializable]
	private class MyKeyValuePair<KeyType, ValueType>
	{
		public KeyType key;

		public ValueType value;
	}

	[Serializable]
	private class BreakerIdInstanceIdPair : MyKeyValuePair<string, int>
	{
		public static List<BreakerIdInstanceIdPair> ListFromDictionary(Dictionary<string, int> dict)
		{
			List<BreakerIdInstanceIdPair> list = new List<BreakerIdInstanceIdPair>();
			foreach (KeyValuePair<string, int> item in dict)
			{
				list.Add(new BreakerIdInstanceIdPair
				{
					key = item.Key,
					value = item.Value
				});
			}
			return list;
		}

		public static Dictionary<string, int> DictFromList(List<BreakerIdInstanceIdPair> list)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (BreakerIdInstanceIdPair item in list)
			{
				dictionary[item.key] = item.value;
			}
			return dictionary;
		}
	}

	[Serializable]
	private class BreakerTextureOffsetsPair : MyKeyValuePair<int, TexturesOffsets>
	{
		public static List<BreakerTextureOffsetsPair> ListFromDictionary(Dictionary<int, TexturesOffsets> dict)
		{
			List<BreakerTextureOffsetsPair> list = new List<BreakerTextureOffsetsPair>();
			foreach (KeyValuePair<int, TexturesOffsets> item in dict)
			{
				list.Add(new BreakerTextureOffsetsPair
				{
					key = item.Key,
					value = item.Value
				});
			}
			return list;
		}

		public static Dictionary<int, TexturesOffsets> DictFromList(List<BreakerTextureOffsetsPair> list)
		{
			Dictionary<int, TexturesOffsets> dictionary = new Dictionary<int, TexturesOffsets>();
			foreach (BreakerTextureOffsetsPair item in list)
			{
				dictionary[item.key] = item.value;
			}
			return dictionary;
		}
	}

	private struct _E003
	{
		public string breakerId;

		public string pieceId;

		public MeshRenderer renderer;

		public Mesh mesh;

		public Material material;
	}

	private static WindowsManager m__E000 = null;

	private Dictionary<int, _E000> m__E001 = new Dictionary<int, _E000>();

	private Dictionary<int, Camera> m__E002 = new Dictionary<int, Camera>();

	public Material WindowsMaterial;

	public Material CopyCubemapMaterial;

	public Material MBOITBlitOpaque;

	public ComputeShader FrustumCullShader;

	public ComputeShader OcclusionCullShader;

	public int OcclusionAccuracy = 1;

	public float OcclusionOffset;

	public bool IsActive = true;

	public List<Renderer> RenderersForOITMoments = new List<Renderer>();

	public static bool EnableFrustumCulling = true;

	public static bool EnableFrustumOcclusionCulling = true;

	public static bool EnableMBOIT = true;

	public static bool EnableIsManageFunction = true;

	public static bool MBOITEnergyConservative = true;

	private bool m__E003 = true;

	private bool m__E004 = true;

	private bool m__E005 = true;

	[SerializeField]
	private List<int> _verticesInstances = new List<int>();

	[SerializeField]
	private List<int> _verticesLocalOffsets = new List<int>();

	private Dictionary<Mesh, MeshOffsets> m__E006 = new Dictionary<Mesh, MeshOffsets>();

	[SerializeField]
	private List<MeshOffsets> _meshesOffsetsList = new List<MeshOffsets>();

	[SerializeField]
	private List<int> _meshesVerticesCount = new List<int>();

	[SerializeField]
	private List<int> _instanceIdToStartVertexId = new List<int>();

	private List<Texture2D> m__E007 = new List<Texture2D>();

	private List<Cubemap> m__E008 = new List<Cubemap>();

	[SerializeField]
	private List<LightProperties> _lightsProperties = new List<LightProperties>();

	[SerializeField]
	private List<MaterialParameters> _materialsParameters = new List<MaterialParameters>();

	private Vector4[] m__E009 = new Vector4[6];

	private Vector3 m__E00A;

	public static int PiecesInstancesRingSize = 2000;

	public static int PiecesVerticesRingSize = 20000;

	public static int PiecesIndicesRingSize = 100000;

	private Dictionary<int, _E002> m__E00B = new Dictionary<int, _E002>();

	private Queue<_E002> m__E00C = new Queue<_E002>();

	private _E002 m__E00D;

	private _E002 m__E00E;

	private _E002 m__E00F;

	[SerializeField]
	private InstancesBuffers _instancesGeometry = new InstancesBuffers();

	[SerializeField]
	private GeometryBuffers _geometryData = new GeometryBuffers();

	private _E001 m__E010 = new _E001();

	private Light m__E011;

	[SerializeField]
	private Texture2DArray _texture2DArray;

	[SerializeField]
	private CubemapArray _cubemapArray;

	private List<int> m__E012 = new List<int>();

	private Dictionary<string, int> m__E013 = new Dictionary<string, int>();

	[SerializeField]
	private List<BreakerIdInstanceIdPair> _breakerIdInstanceIdList = new List<BreakerIdInstanceIdPair>();

	[SerializeField]
	private List<BreakerTextureOffsetsPair> _breakersTexturesOffsetsList = new List<BreakerTextureOffsetsPair>();

	private Dictionary<int, TexturesOffsets> m__E014 = new Dictionary<int, TexturesOffsets>();

	private Dictionary<int, _E003> m__E015 = new Dictionary<int, _E003>();

	private List<string> m__E016 = new List<string>();

	private List<MaterialParameters> m__E017 = new List<MaterialParameters>();

	private List<MeshOffsets> m__E018 = new List<MeshOffsets>();

	private List<int> m__E019 = new List<int>();

	private List<WindowVertex> m__E01A = new List<WindowVertex>();

	private List<int> m__E01B = new List<int>();

	private WindowVertex m__E01C;

	private List<int> m__E01D = new List<int>();

	private List<int> _E01E = new List<int>();

	private List<int> _E01F = new List<int>();

	private _E002 _E020 = new _E002();

	private List<Matrix4x4> _E021 = new List<Matrix4x4>();

	private List<Vector3> _E022 = new List<Vector3>();

	private List<int> _E023 = new List<int>();

	private List<TexturesOffsets> _E024 = new List<TexturesOffsets>();

	private List<int> _E025 = new List<int>();

	private static Vector3 _E026 = new Vector3(-1f, 1f, 1f).normalized;

	private static Vector3 _E027 = new Vector3(-1f, 1f, -1f).normalized;

	private static Vector3 _E028 = new Vector3(1f, 1f, 1f).normalized;

	private static Vector3 _E029 = new Vector3(1f, -1f, 1f).normalized;

	private static Vector3[] _E02A = new Vector3[4] { _E026, _E027, _E028, _E029 };

	private static _E313[] _E02B = new _E313[4]
	{
		new _E313(0, 7),
		new _E313(1, 6),
		new _E313(4, 3),
		new _E313(5, 2)
	};

	private static Vector3[] _E02C = new Vector3[8]
	{
		new Vector3(1f, 0f, 0f),
		new Vector3(1f, 0f, 1f),
		new Vector3(1f, 1f, 0f),
		new Vector3(1f, 1f, 1f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0f, 0f, 1f),
		new Vector3(0f, 1f, 0f),
		new Vector3(0f, 1f, 1f)
	};

	private static Vector3[] _E02D = new Vector3[12];

	private static float[] _E02E = new float[4];

	private List<int> _E02F = new List<int>();

	public static WindowsManager Instance => WindowsManager.m__E000;

	public static bool InstanceIsActive()
	{
		if (WindowsManager.m__E000 == null)
		{
			return false;
		}
		return WindowsManager.m__E000.IsActive;
	}

	private void Awake()
	{
		WindowsManager.m__E000 = this;
		this.m__E013 = BreakerIdInstanceIdPair.DictFromList(_breakerIdInstanceIdList);
		this.m__E014 = BreakerTextureOffsetsPair.DictFromList(_breakersTexturesOffsetsList);
		_E00C(ref _lightsProperties);
	}

	private void Start()
	{
		TOD_Sky instance = TOD_Sky.Instance;
		this.m__E011 = instance.Components.LightSource;
		if (Application.isPlaying)
		{
			CollectAndDisableMeshRenderers(collect: true);
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E006));
		}
	}

	public void RunAwake()
	{
	}

	public void EnableMeshRenderers()
	{
		_E002(enable: true);
	}

	public void DisableMeshRenderers()
	{
		_E002(enable: false);
	}

	public void RunAwakeForSelected()
	{
	}

	private void OnDestroy()
	{
		if (WindowsManager.m__E000 == this)
		{
			WindowsManager.m__E000 = null;
		}
	}

	private void OnDisable()
	{
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E006));
	}

	private void _E000()
	{
		EnableFrustumCulling = true;
		EnableFrustumOcclusionCulling = false;
	}

	private void _E001()
	{
		CrossSceneCullingGroupPreProcess[] array = UnityEngine.Object.FindObjectsOfType<CrossSceneCullingGroupPreProcess>();
		for (int i = 0; i < array.Length; i++)
		{
			GuidReference[] cullingGroups = array[i].CullingGroups;
			foreach (GuidReference guidReference in cullingGroups)
			{
				if (!(guidReference.gameObject == null))
				{
					PerfectCullingCrossSceneContentMeshes component = guidReference.GetComponent<PerfectCullingCrossSceneContentMeshes>();
					if (!(component == null))
					{
						component.FillWindows();
					}
				}
			}
		}
	}

	public void CollectAndDisableMeshRenderers(bool collect)
	{
		if (collect)
		{
			_E001();
		}
		_E002(enable: false);
	}

	private void _E002(bool enable)
	{
		CrossSceneCullingGroupPreProcess[] array = UnityEngine.Object.FindObjectsOfType<CrossSceneCullingGroupPreProcess>();
		for (int i = 0; i < array.Length; i++)
		{
			GuidReference[] cullingGroups = array[i].CullingGroups;
			foreach (GuidReference guidReference in cullingGroups)
			{
				if (guidReference.gameObject == null)
				{
					continue;
				}
				PerfectCullingCrossSceneContentMeshes component = guidReference.GetComponent<PerfectCullingCrossSceneContentMeshes>();
				if (!(component == null) && component.Windows != null)
				{
					Renderer[] windows = component.Windows;
					for (int k = 0; k < windows.Length; k++)
					{
						windows[k].enabled = enable;
					}
				}
			}
		}
	}

	public void CollectAndEnableMeshRenderers(bool collect)
	{
		if (collect)
		{
			_E001();
		}
		_E002(enable: true);
	}

	private void _E003()
	{
		EnableFrustumCulling = true;
		EnableFrustumOcclusionCulling = true;
	}

	private void _E004()
	{
		EnableFrustumCulling = false;
		EnableFrustumOcclusionCulling = false;
	}

	private void _E005()
	{
		ClearRenderData(releaseTextures: true);
	}

	public void ClearRenderData(bool releaseTextures)
	{
		_E008();
		_E009(releaseTextures);
	}

	public void Discard()
	{
		WindowsManager.m__E000 = null;
		_E002(enable: true);
		if (RenderersForOITMoments != null)
		{
			foreach (Renderer renderersForOITMoment in RenderersForOITMoments)
			{
				if (renderersForOITMoment != null)
				{
					renderersForOITMoment.enabled = true;
				}
			}
		}
		_E007();
		_E008();
		_E009();
	}

	private void _E006(Camera camera)
	{
		if (!IsActive)
		{
			return;
		}
		_E8A8 instance = _E8A8.Instance;
		bool flag = false;
		if (instance != null)
		{
			bool num = instance.Camera != null && instance.Camera.GetInstanceID() == camera.GetInstanceID();
			bool flag2 = instance.OpticCameraManager != null && instance.OpticCameraManager.Camera != null && instance.OpticCameraManager.Camera.GetInstanceID() == camera.GetInstanceID();
			flag = num || flag2;
		}
		if (!flag || WindowsMaterial == null || _instancesGeometry._allTransforms.Count == 0)
		{
			return;
		}
		if (this.m__E003 != EnableFrustumCulling)
		{
			_E008();
			_E009();
			this.m__E003 = EnableFrustumCulling;
		}
		if (this.m__E004 != EnableFrustumOcclusionCulling)
		{
			_E008();
			_E009();
			this.m__E004 = EnableFrustumOcclusionCulling;
		}
		if (this.m__E005 != EnableMBOIT)
		{
			_E008();
			_E009();
			this.m__E005 = EnableMBOIT;
		}
		if (!this.m__E001.ContainsKey(camera.GetInstanceID()))
		{
			_E000 crd = new _E000();
			CommandBuffer commandBuffer = new CommandBuffer();
			commandBuffer.name = _ED3E._E000(97381) + camera.name + _ED3E._E000(97438);
			crd.CmdBufDrawing = commandBuffer;
			crd.CmdBufBeforeTransparent = new CommandBuffer();
			crd.CmdBufBeforeTransparent.name = _ED3E._E000(97434) + camera.name;
			crd.CmdBufCulling = new CommandBuffer();
			crd.CmdBufCulling.name = _ED3E._E000(97422) + camera.name;
			crd._ssaa = camera.GetComponent<SSAA>();
			crd._ssaaOptic = camera.GetComponent<SSAAOptic>();
			crd.ProjViewMatrix.Add(camera.projectionMatrix * camera.worldToCameraMatrix);
			crd.HiZGenerator = camera.gameObject.GetComponent<GPUInstancerHiZOcclusionGenerator>();
			if (crd.HiZGenerator == null)
			{
				if (_E4BF.gpuiSettings == null)
				{
					_E4BF.gpuiSettings = GPUInstancerSettings.GetDefaultGPUInstancerSettings();
				}
				_E4BF.gpuiSettings.SetDefultBindings();
				crd.HiZGenerator = camera.gameObject.AddComponent<GPUInstancerHiZOcclusionGenerator>();
				crd.HiZGenerator.Initialize(camera);
			}
			camera.AddCommandBuffer(CameraEvent.BeforeGBuffer, crd.CmdBufCulling);
			camera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, crd.CmdBufBeforeTransparent);
			camera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, crd.CmdBufDrawing);
			camera.SortCommandBuffers(CameraEvent.AfterForwardAlpha);
			this.m__E002.Add(camera.GetInstanceID(), camera);
			_E01D(ref crd, camera);
			this.m__E001.Add(camera.GetInstanceID(), crd);
			return;
		}
		_E000 crd2 = this.m__E001[camera.GetInstanceID()];
		if (crd2.ResolutionMismatched(camera))
		{
			crd2.ReleaseRenderTextures();
			_E01D(ref crd2, camera);
		}
		_E013();
		if (crd2.FrustumPlanes != null)
		{
			Plane[] array = GeometryUtility.CalculateFrustumPlanes(camera);
			for (int i = 0; i < 6; i++)
			{
				this.m__E009[i] = new Vector4(array[i].normal.x, array[i].normal.y, array[i].normal.z, array[i].distance);
			}
			crd2.FrustumPlanes.SetData(this.m__E009);
		}
		if (crd2.FrustumPNFactors != null)
		{
			_E019(camera, ref _E02D);
			crd2.FrustumPNFactors.SetData(_E02D);
		}
		crd2.ProjViewMatrix[0] = camera.projectionMatrix * camera.worldToCameraMatrix;
		if (crd2.CameraProjViewMatrixBuffer != null)
		{
			crd2.CameraProjViewMatrixBuffer.SetData(crd2.ProjViewMatrix);
		}
		if (crd2.CameraParameters != null)
		{
			_E02E[0] = camera.nearClipPlane;
			_E02E[1] = camera.farClipPlane;
			_E02E[2] = camera.farClipPlane - camera.nearClipPlane;
			crd2.CameraParameters.SetData(_E02E);
		}
		_E00D(ref _lightsProperties);
		this.m__E010._lightsProperties.SetData(_lightsProperties);
	}

	private void _E007()
	{
		this.m__E00B.Clear();
		this.m__E00C.Clear();
		this.m__E00D = null;
		this.m__E00E = null;
		_instancesGeometry.Clear();
		_geometryData.Clear();
		this.m__E006.Clear();
		_meshesOffsetsList.Clear();
		_meshesVerticesCount.Clear();
		_instanceIdToStartVertexId.Clear();
		_verticesInstances.Clear();
		_verticesLocalOffsets.Clear();
		this.m__E007.Clear();
		this.m__E008.Clear();
		_lightsProperties.Clear();
		_materialsParameters.Clear();
		this.m__E013.Clear();
		_breakerIdInstanceIdList.Clear();
		this.m__E014.Clear();
		_breakersTexturesOffsetsList.Clear();
	}

	private void _E008()
	{
		foreach (KeyValuePair<int, _E000> item in this.m__E001)
		{
			Camera camera = this.m__E002[item.Key];
			if ((bool)camera)
			{
				camera.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, item.Value.CmdBufDrawing);
				camera.RemoveCommandBuffer(CameraEvent.BeforeGBuffer, item.Value.CmdBufCulling);
				camera.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, item.Value.CmdBufBeforeTransparent);
			}
			item.Value.Release();
		}
		this.m__E001.Clear();
		this.m__E002.Clear();
	}

	private void _E009(bool releaseTextures = true)
	{
		this.m__E010.Release();
		this.m__E00D = null;
		this.m__E00E = null;
		if (releaseTextures)
		{
			if (_texture2DArray != null)
			{
				_texture2DArray = null;
			}
			if (_cubemapArray != null)
			{
				_cubemapArray = null;
			}
		}
	}

	public static bool IsManagedByWindowsManager(Renderer renderer)
	{
		if (!EnableIsManageFunction)
		{
			return false;
		}
		MeshRenderer meshRenderer = renderer as MeshRenderer;
		if (meshRenderer == null)
		{
			return false;
		}
		return _E00A(meshRenderer);
	}

	private static bool _E00A(MeshRenderer renderer)
	{
		if (renderer.gameObject.GetComponentInParent<Trunk>() != null)
		{
			return false;
		}
		if (renderer.gameObject.GetComponentInParent<Door>() != null)
		{
			return false;
		}
		Material[] sharedMaterials = renderer.sharedMaterials;
		if (sharedMaterials.Length != 1)
		{
			return false;
		}
		if (!_E012(sharedMaterials[0]))
		{
			return false;
		}
		if (renderer.GetComponent<MeshFilter>().sharedMesh == null)
		{
			return false;
		}
		return true;
	}

	private List<MeshRenderer> _E00B(MeshRenderer[] meshRenderers)
	{
		List<MeshRenderer> list = new List<MeshRenderer>();
		int num = 0;
		if (!Application.isPlaying)
		{
			_ = num % 20 == 0;
		}
		else
			_ = 0;
		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			num++;
			if (_E00A(meshRenderer))
			{
				list.Add(meshRenderer);
			}
		}
		return list;
	}

	private void _E00C(ref List<LightProperties> props)
	{
		props.Clear();
		props.Add(default(LightProperties));
		_E00D(ref props);
	}

	private void _E00D(ref List<LightProperties> props)
	{
		if (!(this.m__E011 == null))
		{
			LightProperties value = props[0];
			value.pos = this.m__E011.transform.position;
			value.pos.w = 1f;
			value.dir = this.m__E011.transform.forward;
			this.m__E00A.x = this.m__E011.color.r;
			this.m__E00A.y = this.m__E011.color.g;
			this.m__E00A.z = this.m__E011.color.b;
			value.color = this.m__E011.intensity * this.m__E00A;
			value.worldToLight = this.m__E011.transform.worldToLocalMatrix;
			props[0] = value;
		}
	}

	private bool _E00E(_E002 last, _E002 desiredSize)
	{
		int num = last.InstanceOffset + 1 + desiredSize.InstanceOffset;
		if (num > this.m__E010._texturesOffsetsBuffer.count)
		{
			return false;
		}
		if (num > this.m__E010._allTransformsBuffer.count)
		{
			return false;
		}
		if (num * 2 > this.m__E010._allBoundsBuffer.count)
		{
			return false;
		}
		if (num > this.m__E010._materialsProperties.count)
		{
			return false;
		}
		if (num > this.m__E010._instancesEnable.count)
		{
			return false;
		}
		if (last.VerticesOffset + last.VerticesCount + desiredSize.VerticesOffset > this.m__E010._allVerticesBuffer.count)
		{
			return false;
		}
		if (last.VerticesInstancesOffset + last.VerticesInstancesCount + desiredSize.VerticesInstancesOffset > this.m__E010._verticesInstances.count)
		{
			return false;
		}
		if (last.TrianglesOffset + last.TrianglesCount + desiredSize.TrianglesOffset > this.m__E010._allTrianglesBuffer.count)
		{
			return false;
		}
		if (last.VerticesLocalOffsetsOffset + last.VerticesLocalOffsetsCount + desiredSize.VerticesLocalOffsetsOffset > this.m__E010._verticesLocalOffsets.count)
		{
			return false;
		}
		if (last.InstanceMeshIndexOffset + 1 + desiredSize.InstanceMeshIndexOffset > this.m__E010._instancesMeshesIndices.count)
		{
			return false;
		}
		if (last.MeshOffsetOffset + 1 + desiredSize.MeshOffsetOffset > this.m__E010._meshesOffsets.count)
		{
			return false;
		}
		if (last.InstanceIdToStartVertexIdOffset + 1 + desiredSize.InstanceIdToStartVertexIdOffset > this.m__E010._instanceIdToStartVertexId.count)
		{
			return false;
		}
		if (last.MeshesVerticesCountOffset + 1 + desiredSize.MeshesVerticesCountOffset > this.m__E010._meshesVerticesCounts.count)
		{
			return false;
		}
		return true;
	}

	private bool _E00F(_E002 bufferHead, _E002 first, _E002 desiredSize)
	{
		if (bufferHead.InstanceOffset + desiredSize.InstanceOffset > first.InstanceOffset)
		{
			return false;
		}
		if (bufferHead.VerticesOffset + desiredSize.VerticesOffset > first.VerticesOffset)
		{
			return false;
		}
		if (bufferHead.VerticesInstancesOffset + desiredSize.VerticesInstancesOffset > first.VerticesInstancesOffset)
		{
			return false;
		}
		if (bufferHead.TrianglesOffset + desiredSize.TrianglesOffset > first.TrianglesOffset)
		{
			return false;
		}
		if (bufferHead.VerticesLocalOffsetsOffset + desiredSize.VerticesLocalOffsetsOffset > first.VerticesLocalOffsetsOffset)
		{
			return false;
		}
		if (bufferHead.InstanceMeshIndexOffset + desiredSize.InstanceMeshIndexOffset > first.InstanceMeshIndexOffset)
		{
			return false;
		}
		if (bufferHead.MeshOffsetOffset + desiredSize.MeshOffsetOffset > first.MeshOffsetOffset)
		{
			return false;
		}
		if (bufferHead.InstanceIdToStartVertexIdOffset + desiredSize.InstanceIdToStartVertexIdOffset > first.InstanceIdToStartVertexIdOffset)
		{
			return false;
		}
		if (bufferHead.MeshesVerticesCountOffset + desiredSize.MeshesVerticesCountOffset > first.MeshesVerticesCountOffset)
		{
			return false;
		}
		return true;
	}

	private bool _E010(_E002 left, _E002 right, _E002 desiredSize)
	{
		if (left.InstanceOffset + 1 + desiredSize.InstanceOffset > right.InstanceOffset)
		{
			return false;
		}
		if (left.VerticesOffset + left.VerticesCount + desiredSize.VerticesOffset > right.VerticesOffset)
		{
			return false;
		}
		if (left.VerticesInstancesOffset + left.VerticesInstancesCount + desiredSize.VerticesInstancesOffset > right.VerticesInstancesOffset)
		{
			return false;
		}
		if (left.TrianglesOffset + left.TrianglesCount + desiredSize.TrianglesOffset > right.TrianglesOffset)
		{
			return false;
		}
		if (left.VerticesLocalOffsetsOffset + left.VerticesLocalOffsetsCount + desiredSize.VerticesLocalOffsetsOffset > right.VerticesLocalOffsetsOffset)
		{
			return false;
		}
		if (left.InstanceMeshIndexOffset + 1 + desiredSize.InstanceMeshIndexOffset > right.InstanceMeshIndexOffset)
		{
			return false;
		}
		if (left.MeshOffsetOffset + 1 + desiredSize.MeshOffsetOffset > right.MeshOffsetOffset)
		{
			return false;
		}
		if (left.InstanceIdToStartVertexIdOffset + 1 + desiredSize.InstanceIdToStartVertexIdOffset > right.InstanceIdToStartVertexIdOffset)
		{
			return false;
		}
		if (left.MeshesVerticesCountOffset + 1 + desiredSize.MeshesVerticesCountOffset > right.MeshesVerticesCountOffset)
		{
			return false;
		}
		return true;
	}

	private _E002 _E011(string pieceId, int pieceIdHash, _E002 desiredSizes)
	{
		if (this.m__E00D == null)
		{
			this.m__E00E = new _E002
			{
				PieceId = pieceId,
				PieceIdHash = pieceIdHash,
				InstanceOffset = _instancesGeometry.TexturesOffsetsCount,
				VerticesOffset = _geometryData._allVertices.Count,
				VerticesInstancesOffset = _verticesInstances.Count,
				TrianglesOffset = _geometryData._allTriangles.Count,
				VerticesLocalOffsetsOffset = _verticesLocalOffsets.Count,
				InstanceMeshIndexOffset = _instancesGeometry._meshIndex.Count,
				MeshOffsetOffset = _meshesOffsetsList.Count,
				InstanceIdToStartVertexIdOffset = _instanceIdToStartVertexId.Count,
				MeshesVerticesCountOffset = _meshesVerticesCount.Count
			};
			this.m__E00D = this.m__E00E;
			this.m__E00F = new _E002(this.m__E00D);
			return this.m__E00D;
		}
		if (this.m__E00D.InstanceOffset > this.m__E00E.InstanceOffset)
		{
			if (_E00E(this.m__E00D, desiredSizes))
			{
				return this.m__E00D = new _E002
				{
					PieceId = pieceId,
					PieceIdHash = pieceIdHash,
					InstanceOffset = this.m__E00D.InstanceOffset + 1,
					VerticesOffset = this.m__E00D.VerticesOffset + this.m__E00D.VerticesCount,
					VerticesInstancesOffset = this.m__E00D.VerticesInstancesOffset + this.m__E00D.VerticesInstancesCount,
					TrianglesOffset = this.m__E00D.TrianglesOffset + this.m__E00D.TrianglesCount,
					VerticesLocalOffsetsOffset = this.m__E00D.VerticesLocalOffsetsOffset + this.m__E00D.VerticesLocalOffsetsCount,
					InstanceMeshIndexOffset = this.m__E00D.InstanceMeshIndexOffset + 1,
					MeshOffsetOffset = this.m__E00D.MeshOffsetOffset + 1,
					InstanceIdToStartVertexIdOffset = this.m__E00D.InstanceIdToStartVertexIdOffset + 1,
					MeshesVerticesCountOffset = this.m__E00D.MeshesVerticesCountOffset + 1
				};
			}
			if (_E00F(this.m__E00F, this.m__E00E, desiredSizes))
			{
				return this.m__E00D = new _E002(this.m__E00F, pieceId, pieceIdHash);
			}
			while (this.m__E00E != null && !_E00F(this.m__E00F, this.m__E00E, desiredSizes))
			{
				_E002 obj = this.m__E00C.Dequeue();
				this.m__E00B.Remove(obj.PieceIdHash);
				if (this.m__E00C.Count == 0)
				{
					this.m__E00E = null;
					this.m__E00D = null;
				}
				else
				{
					this.m__E00E = this.m__E00C.Peek();
				}
			}
			_E002 obj2 = new _E002(this.m__E00F, pieceId, pieceIdHash);
			if (this.m__E00E == null)
			{
				if (!_E00E(this.m__E00F, desiredSizes))
				{
					return null;
				}
				this.m__E00E = obj2;
			}
			this.m__E00D = obj2;
			this.m__E00F = new _E002(this.m__E00D);
			return this.m__E00D;
		}
		if (_E010(this.m__E00D, this.m__E00E, desiredSizes))
		{
			return this.m__E00D = new _E002
			{
				PieceId = pieceId,
				PieceIdHash = pieceIdHash,
				InstanceOffset = this.m__E00D.InstanceOffset + 1,
				VerticesOffset = this.m__E00D.VerticesOffset + this.m__E00D.VerticesCount,
				VerticesInstancesOffset = this.m__E00D.VerticesInstancesOffset + this.m__E00D.VerticesInstancesCount,
				TrianglesOffset = this.m__E00D.TrianglesOffset + this.m__E00D.TrianglesCount,
				VerticesLocalOffsetsOffset = this.m__E00D.VerticesLocalOffsetsOffset + this.m__E00D.VerticesLocalOffsetsCount,
				InstanceMeshIndexOffset = this.m__E00D.InstanceMeshIndexOffset + 1,
				MeshOffsetOffset = this.m__E00D.MeshOffsetOffset + 1,
				InstanceIdToStartVertexIdOffset = this.m__E00D.InstanceIdToStartVertexIdOffset + 1,
				MeshesVerticesCountOffset = this.m__E00D.MeshesVerticesCountOffset + 1
			};
		}
		while (!_E010(this.m__E00D, this.m__E00E, desiredSizes) && this.m__E00D.InstanceOffset < this.m__E00E.InstanceOffset)
		{
			_E002 obj3 = this.m__E00C.Dequeue();
			this.m__E00B.Remove(obj3.PieceIdHash);
			if (this.m__E00C.Count == 0)
			{
				this.m__E00E = null;
				this.m__E00D = null;
			}
			else
			{
				this.m__E00E = this.m__E00C.Peek();
			}
		}
		if (this.m__E00D.InstanceOffset > this.m__E00E.InstanceOffset)
		{
			return _E011(pieceId, pieceIdHash, desiredSizes);
		}
		return this.m__E00D = new _E002
		{
			PieceId = pieceId,
			PieceIdHash = pieceIdHash,
			InstanceOffset = this.m__E00D.InstanceOffset + 1,
			VerticesOffset = this.m__E00D.VerticesOffset + this.m__E00D.VerticesCount,
			VerticesInstancesOffset = this.m__E00D.VerticesInstancesOffset + this.m__E00D.VerticesInstancesCount,
			TrianglesOffset = this.m__E00D.TrianglesOffset + this.m__E00D.TrianglesCount,
			VerticesLocalOffsetsOffset = this.m__E00D.VerticesLocalOffsetsOffset + this.m__E00D.VerticesLocalOffsetsCount,
			InstanceMeshIndexOffset = this.m__E00D.InstanceMeshIndexOffset + 1,
			MeshOffsetOffset = this.m__E00D.MeshOffsetOffset + 1,
			InstanceIdToStartVertexIdOffset = this.m__E00D.InstanceIdToStartVertexIdOffset + 1,
			MeshesVerticesCountOffset = this.m__E00D.MeshesVerticesCountOffset + 1
		};
	}

	private static bool _E012(Material material)
	{
		if (material == null)
		{
			return false;
		}
		if (!(material != null) || !(material.shader != null) || material.shader.name == null)
		{
			return false;
		}
		if (material.shader.name != _ED3E._E000(97411))
		{
			return false;
		}
		return true;
	}

	public bool AddPiece(string breakerId, string pieceId, MeshRenderer renderer, Mesh mesh, Material material)
	{
		if (!IsActive)
		{
			return false;
		}
		if (!_E012(material))
		{
			return false;
		}
		int hashCode = pieceId.GetHashCode();
		if (this.m__E010._allBoundsBuffer == null)
		{
			this.m__E015.Add(hashCode, new _E003
			{
				breakerId = breakerId,
				pieceId = pieceId,
				renderer = renderer,
				mesh = mesh,
				material = material
			});
			return true;
		}
		if (this.m__E00B.ContainsKey(hashCode))
		{
			Debug.LogError(_ED3E._E000(97494) + hashCode.ToString() + _ED3E._E000(97475) + this.m__E00B[hashCode].PieceIdHash + _ED3E._E000(97525) + pieceId);
			return true;
		}
		_E020.Reset();
		if (this.m__E017.Count == 1)
		{
			MaterialParameters value = this.m__E017[0];
			value.Init(material, _ED3E._E000(19728), _ED3E._E000(97553));
			this.m__E017[0] = value;
		}
		else
		{
			this.m__E017.Add(new MaterialParameters(material, _ED3E._E000(19728), _ED3E._E000(97553)));
		}
		_E020.InstanceOffset = 1;
		_E020.MeshOffsetOffset = 1;
		int[] triangles = mesh.triangles;
		if (this.m__E019.Count == 1)
		{
			this.m__E019[0] = triangles.Length;
		}
		else
		{
			this.m__E019.Add(triangles.Length);
		}
		_E020.MeshesVerticesCountOffset = 1;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		this.m__E01A.Clear();
		this.m__E01B.Clear();
		for (int i = 0; i < vertices.Length; i++)
		{
			this.m__E01C.vertex = vertices[i];
			this.m__E01C.normal = normals[i];
			this.m__E01C.uv = uv[i];
			this.m__E01A.Add(this.m__E01C);
		}
		this.m__E01B.AddRange(triangles);
		_E020.VerticesOffset = vertices.Length;
		_E020.TrianglesOffset = triangles.Length;
		_E020.InstanceIdToStartVertexIdOffset = 1;
		_E020.VerticesInstancesOffset = triangles.Length;
		_E020.VerticesLocalOffsetsOffset = triangles.Length;
		_E021.Clear();
		_E022.Clear();
		_E023.Clear();
		renderer.enabled = false;
		material.GetTexture(_ED3E._E000(19728));
		material.GetTexture(_ED3E._E000(97553));
		material.GetTexture(_ED3E._E000(84062));
		int hashCode2 = breakerId.GetHashCode();
		TexturesOffsets texturesOffsets2;
		if (!this.m__E014.ContainsKey(hashCode2))
		{
			TexturesOffsets texturesOffsets = default(TexturesOffsets);
			texturesOffsets.MainTexOffset = 0;
			texturesOffsets.SpecTexOffset = 0;
			texturesOffsets.ReflectionCubeOffset = 0;
			texturesOffsets2 = texturesOffsets;
		}
		else
		{
			texturesOffsets2 = this.m__E014[hashCode2];
		}
		TexturesOffsets item = texturesOffsets2;
		_E024.Clear();
		_E024.Add(item);
		_E025.Clear();
		_E025.Add(1);
		_E002 obj = _E011(pieceId, hashCode, _E020);
		if (obj == null)
		{
			Debug.LogError(_ED3E._E000(97546));
			return false;
		}
		this.m__E010._materialsProperties.SetData(this.m__E017, 0, obj.InstanceOffset, 1);
		if (this.m__E018.Count == 0)
		{
			this.m__E018.Add(new MeshOffsets
			{
				VerticesOffset = obj.VerticesOffset,
				IndicesOffset = obj.TrianglesOffset,
				MeshIndex = obj.InstanceMeshIndexOffset
			});
		}
		else
		{
			MeshOffsets value2 = this.m__E018[0];
			value2.VerticesOffset = obj.VerticesOffset;
			value2.IndicesOffset = obj.TrianglesOffset;
			value2.MeshIndex = obj.InstanceMeshIndexOffset;
			this.m__E018[0] = value2;
		}
		this.m__E010._meshesOffsets.SetData(this.m__E018, 0, obj.MeshOffsetOffset, 1);
		this.m__E010._meshesVerticesCounts.SetData(this.m__E019, 0, obj.MeshesVerticesCountOffset, 1);
		this.m__E010._allVerticesBuffer.SetData(this.m__E01A, 0, obj.VerticesOffset, vertices.Length);
		obj.VerticesCount = vertices.Length;
		this.m__E010._allTrianglesBuffer.SetData(this.m__E01B, 0, obj.TrianglesOffset, triangles.Length);
		obj.TrianglesCount = triangles.Length;
		if (this.m__E01D.Count == 0)
		{
			this.m__E01D.Add(obj.VerticesInstancesOffset);
		}
		else
		{
			this.m__E01D[0] = obj.VerticesInstancesOffset;
		}
		this.m__E010._instanceIdToStartVertexId.SetData(this.m__E01D, 0, obj.InstanceIdToStartVertexIdOffset, 1);
		_E01E.Clear();
		_E01F.Clear();
		int instanceOffset = obj.InstanceOffset;
		for (int j = 0; j < triangles.Length; j++)
		{
			_E01E.Add(instanceOffset);
			_E01F.Add(j);
		}
		this.m__E010._verticesInstances.SetData(_E01E, 0, obj.VerticesInstancesOffset, triangles.Length);
		obj.VerticesInstancesCount = triangles.Length;
		this.m__E010._verticesLocalOffsets.SetData(_E01F, 0, obj.VerticesLocalOffsetsOffset, triangles.Length);
		obj.VerticesLocalOffsetsCount = triangles.Length;
		_E021.Add(renderer.localToWorldMatrix);
		this.m__E010._allTransformsBuffer.SetData(_E021, 0, obj.InstanceOffset, 1);
		_E023.Add(obj.MeshOffsetOffset);
		this.m__E010._instancesMeshesIndices.SetData(_E023, 0, obj.InstanceMeshIndexOffset, 1);
		_E022.Add(renderer.bounds.min);
		_E022.Add(renderer.bounds.max);
		this.m__E010._allBoundsBuffer.SetData(_E022, 0, obj.InstanceOffset * 2, 2);
		this.m__E010._texturesOffsetsBuffer.SetData(_E024, 0, obj.InstanceOffset, 1);
		this.m__E010._instancesEnable.SetData(_E025, 0, obj.InstanceOffset, 1);
		this.m__E00D.VerticesCount = obj.VerticesCount;
		this.m__E00D.VerticesInstancesCount = obj.VerticesInstancesCount;
		this.m__E00D.TrianglesCount = obj.TrianglesCount;
		this.m__E00D.VerticesLocalOffsetsCount = obj.VerticesLocalOffsetsCount;
		this.m__E00B.Add(hashCode, this.m__E00D);
		this.m__E00C.Enqueue(this.m__E00D);
		return true;
	}

	private void _E013()
	{
		if (this.m__E015.Count > 0)
		{
			foreach (KeyValuePair<int, _E003> item in this.m__E015)
			{
				_E003 value = item.Value;
				AddPiece(value.breakerId, value.pieceId, value.renderer, value.mesh, value.material);
			}
			this.m__E015.Clear();
		}
		if (this.m__E016.Count <= 0)
		{
			return;
		}
		foreach (string item2 in this.m__E016)
		{
			BreakWindow(item2);
		}
		this.m__E016.Clear();
	}

	public void UpdatePieceTransform(string pieceId, Transform transform)
	{
		int hashCode = pieceId.GetHashCode();
		if (this.m__E00B.ContainsKey(hashCode))
		{
			_E002 obj = this.m__E00B[hashCode];
			_E021.Clear();
			_E021.Add(transform.localToWorldMatrix);
			this.m__E010._allTransformsBuffer.SetData(_E021, 0, obj.InstanceOffset, 1);
		}
	}

	private void _E014()
	{
		if (this.m__E00C.Count != 0)
		{
			do
			{
				_E002 obj = this.m__E00C.Dequeue();
				this.m__E00B.Remove(obj.PieceIdHash);
			}
			while (this.m__E00C.Count > 0 && !this.m__E00C.Peek().Enabled);
			if (this.m__E00C.Count == 0)
			{
				this.m__E00E = null;
				this.m__E00D = null;
			}
			else
			{
				this.m__E00E = this.m__E00C.Peek();
			}
		}
	}

	public void RemovePiece(string pieceId)
	{
		if (!IsActive)
		{
			return;
		}
		int hashCode = pieceId.GetHashCode();
		if (this.m__E015.Count > 0 && this.m__E015.ContainsKey(hashCode))
		{
			this.m__E015.Remove(hashCode);
		}
		else if (this.m__E00B.ContainsKey(hashCode))
		{
			_E002 obj = this.m__E00B[hashCode];
			obj.Enabled = false;
			_E025.Clear();
			_E025.Add(0);
			this.m__E010._instancesEnable.SetData(_E025, 0, obj.InstanceOffset, 1);
			if (obj == this.m__E00E)
			{
				_E014();
			}
		}
	}

	private ComputeBuffer _E015<_E077>(List<_E077> data, int tailReserveCount)
	{
		if (data.Count == 0)
		{
			Debug.LogError(_ED3E._E000(97628));
			return null;
		}
		ComputeBuffer computeBuffer = new ComputeBuffer(data.Count + tailReserveCount, Marshal.SizeOf<_E077>());
		computeBuffer.SetData(data.ToArray(), 0, 0, data.Count);
		return computeBuffer;
	}

	private ComputeBuffer _E016<_E077>(_E077[] data, int tailReserveCount, string name = "", ComputeBufferType bufferType = ComputeBufferType.Default)
	{
		if (data.Length == 0)
		{
			Debug.LogError(_ED3E._E000(97628));
			return null;
		}
		ComputeBuffer computeBuffer = new ComputeBuffer(data.Length + tailReserveCount, Marshal.SizeOf<_E077>(), bufferType);
		computeBuffer.SetData(data, 0, 0, data.Length);
		computeBuffer.name = name;
		return computeBuffer;
	}

	private static Texture2DArray _E017(List<Texture2D> textures, string name, int firstMipLevel)
	{
		Texture2D texture2D = ((textures.Count > 1) ? textures[1] : textures[0]);
		int num = texture2D.width >> firstMipLevel;
		int num2 = texture2D.height >> firstMipLevel;
		Texture2DArray texture2DArray = new Texture2DArray(num, num2, textures.Count, TextureFormat.DXT5, mipChain: true, linear: false)
		{
			name = name,
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Repeat,
			anisoLevel = 1
		};
		int mipmapCount = texture2DArray.mipmapCount;
		RenderTexture[] array = new RenderTexture[texture2D.mipmapCount];
		for (int i = 0; i < texture2D.mipmapCount; i++)
		{
			int width = Math.Max(1, num >> i);
			int height = Math.Max(1, num2 >> i);
			array[i] = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
			array[i].antiAliasing = 1;
			array[i].Create();
		}
		RenderTexture active = RenderTexture.active;
		for (int j = 1; j < textures.Count; j++)
		{
			Texture2D texture2D2 = textures[j];
			if (texture2D2 == null)
			{
				Debug.LogError(string.Format(_ED3E._E000(97623), j));
				UnityEngine.Object.DestroyImmediate(texture2DArray);
				texture2DArray = null;
				break;
			}
			for (int k = firstMipLevel; k < mipmapCount; k++)
			{
				Graphics.Blit(texture2D2, array[k - firstMipLevel]);
				Graphics.SetRenderTarget(array[k - firstMipLevel], 0);
				RenderTexture.active = array[k - firstMipLevel];
				int width2 = array[k - firstMipLevel].width;
				int height2 = array[k - firstMipLevel].height;
				Texture2D texture2D3 = new Texture2D(width2, height2, TextureFormat.ARGB32, mipChain: false);
				texture2D3.ReadPixels(new Rect(0f, 0f, width2, height2), 0, 0);
				texture2D3.Apply();
				texture2D3.Compress(highQuality: false);
				Graphics.CopyTexture(texture2D3, 0, 0, texture2DArray, j, k - firstMipLevel);
				RenderTexture.active = null;
			}
		}
		RenderTexture.active = active;
		RenderTexture[] array2 = array;
		for (int l = 0; l < array2.Length; l++)
		{
			array2[l].Release();
		}
		return texture2DArray;
	}

	private static CubemapArray _E018(List<Cubemap> textures, string name, int firstMipLevel, Material copyMaterial)
	{
		if (copyMaterial == null)
		{
			return null;
		}
		Cubemap cubemap = textures[1];
		int num = cubemap.width >> firstMipLevel;
		int num2 = cubemap.height >> firstMipLevel;
		CubemapArray cubemapArray = new CubemapArray(num, textures.Count, TextureFormat.DXT5, mipChain: true, linear: false)
		{
			name = name,
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Repeat,
			anisoLevel = 1
		};
		int mipmapCount = cubemapArray.mipmapCount;
		RenderTexture[] array = new RenderTexture[cubemap.mipmapCount];
		for (int i = 0; i < mipmapCount; i++)
		{
			array[i] = new RenderTexture(num >> i, num2 >> i, 0, RenderTextureFormat.ARGB32);
			array[i].antiAliasing = 1;
			array[i].Create();
		}
		RenderTexture active = RenderTexture.active;
		for (int j = 1; j < textures.Count; j++)
		{
			Cubemap cubemap2 = textures[j];
			if (cubemap2 == null)
			{
				Debug.LogError(string.Format(_ED3E._E000(97623), j));
				UnityEngine.Object.Destroy(cubemapArray);
				cubemapArray = null;
				break;
			}
			for (int k = firstMipLevel; k < mipmapCount; k++)
			{
				for (int l = 0; l < 6; l++)
				{
					copyMaterial.SetInt(_ED3E._E000(97660), l);
					DebugGraphics.Blit(cubemap2, array[k - firstMipLevel], copyMaterial, 0);
					Graphics.SetRenderTarget(array[k - firstMipLevel], 0);
					RenderTexture.active = array[k - firstMipLevel];
					Texture2D texture2D = new Texture2D(array[k - firstMipLevel].width, array[k - firstMipLevel].height, TextureFormat.ARGB32, mipChain: false);
					texture2D.ReadPixels(new Rect(0f, 0f, array[k - firstMipLevel].width, array[k - firstMipLevel].height), 0, 0);
					texture2D.Apply();
					texture2D.Compress(highQuality: false);
					Graphics.CopyTexture(texture2D, 0, 0, cubemapArray, j * 6 + l, k - firstMipLevel);
					RenderTexture.active = null;
				}
			}
		}
		RenderTexture.active = active;
		RenderTexture[] array2 = array;
		for (int m = 0; m < array2.Length; m++)
		{
			array2[m].Release();
		}
		return cubemapArray;
	}

	private static void _E019(Camera camera, ref Vector3[] result)
	{
		Plane[] array = GeometryUtility.CalculateFrustumPlanes(camera);
		for (int i = 0; i < 6; i++)
		{
			int num = 0;
			float num2 = Vector3.Dot(_E02A[0], array[i].normal);
			float num3 = Mathf.Abs(num2);
			for (int j = 1; j < 4; j++)
			{
				float num4 = Vector3.Dot(_E02A[j], array[i].normal);
				float num5 = Mathf.Abs(num4);
				if (num5 > num3)
				{
					num2 = num4;
					num3 = num5;
					num = j;
				}
			}
			_E313 obj = _E02B[num];
			if (num2 < 0f)
			{
				obj = new _E313(obj.Y, obj.X);
			}
			result[i * 2] = _E02C[obj.X];
			result[i * 2 + 1] = _E02C[obj.Y];
		}
	}

	private void _E01A(RenderTexture renderTexture)
	{
		renderTexture.Release();
		UnityEngine.Object.DestroyImmediate(renderTexture);
	}

	private RenderTexture _E01B(RenderTexture initialRenderTexture, int width, int height, string name, GraphicsFormat format, bool UAV = false, int depth = 0)
	{
		if (initialRenderTexture != null)
		{
			_E01A(initialRenderTexture);
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

	private RenderTexture _E01C(RenderTexture initialRenderTexture, int width, int height, string name, RenderTextureFormat format, bool UAV = false, int depth = 0)
	{
		if (initialRenderTexture != null)
		{
			_E01A(initialRenderTexture);
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

	private void _E01D(ref _E000 crd, Camera camera)
	{
		crd.CmdBufCulling.Clear();
		crd.CmdBufBeforeTransparent.Clear();
		crd.CmdBufDrawing.Clear();
		if (this.m__E010._verticesInstances == null)
		{
			this.m__E010._verticesInstances = _E015(_verticesInstances, PiecesVerticesRingSize);
		}
		if (this.m__E010._verticesLocalOffsets == null)
		{
			this.m__E010._verticesLocalOffsets = _E015(_verticesLocalOffsets, PiecesVerticesRingSize);
		}
		if (this.m__E010._instancesMeshesIndices == null)
		{
			this.m__E010._instancesMeshesIndices = _E015(_instancesGeometry._meshIndex, PiecesInstancesRingSize);
		}
		if (this.m__E010._allVerticesBuffer == null)
		{
			this.m__E010._allVerticesBuffer = _E015(_geometryData._allVertices, PiecesVerticesRingSize);
		}
		if (this.m__E010._verticesIDs == null)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < _verticesInstances.Count + PiecesVerticesRingSize * 3; i++)
			{
				list.Add(i);
			}
			this.m__E010._verticesIDs = _E015(list, 0);
		}
		if (this.m__E010._allTransformsBuffer == null)
		{
			this.m__E010._allTransformsBuffer = _E015(_instancesGeometry._allTransforms, PiecesInstancesRingSize);
		}
		if (this.m__E010._allTrianglesBuffer == null)
		{
			this.m__E010._allTrianglesBuffer = _E015(_geometryData._allTriangles, PiecesIndicesRingSize);
		}
		if (this.m__E010._allBoundsBuffer == null)
		{
			this.m__E010._allBoundsBuffer = _E015(_instancesGeometry._allBounds, PiecesInstancesRingSize * 2);
		}
		if (this.m__E010._meshesOffsets == null)
		{
			this.m__E010._meshesOffsets = _E015(_meshesOffsetsList, PiecesInstancesRingSize);
		}
		if (this.m__E010._meshesVerticesCounts == null)
		{
			this.m__E010._meshesVerticesCounts = _E015(_meshesVerticesCount, PiecesVerticesRingSize);
		}
		if (this.m__E010._instanceIdToStartVertexId == null)
		{
			this.m__E010._instanceIdToStartVertexId = _E015(_instanceIdToStartVertexId, PiecesInstancesRingSize);
		}
		if (this.m__E010._texturesOffsetsBuffer == null)
		{
			this.m__E010._texturesOffsetsBuffer = _E015(_instancesGeometry.TexturesOffsetsList, PiecesInstancesRingSize);
		}
		if (this.m__E010._materialsProperties == null)
		{
			this.m__E010._materialsProperties = _E015(_materialsParameters, PiecesInstancesRingSize);
		}
		if (this.m__E010._lightsProperties == null)
		{
			this.m__E010._lightsProperties = _E015(_lightsProperties, 0);
		}
		if (this.m__E010._instancesEnable == null)
		{
			if (this.m__E012.Count != _materialsParameters.Count)
			{
				this.m__E012.Clear();
				for (int j = 0; j < _materialsParameters.Count; j++)
				{
					this.m__E012.Add(1);
				}
				for (int k = _materialsParameters.Count; k < _materialsParameters.Count + PiecesInstancesRingSize; k++)
				{
					this.m__E012.Add(0);
				}
			}
			this.m__E010._instancesEnable = _E015(this.m__E012, 0);
		}
		if (crd.DrawArgsBuffer == null)
		{
			int[] data = new int[4]
			{
				_geometryData._allTriangles.Count,
				1,
				0,
				0
			};
			crd.DrawArgsBuffer = _E016(data, 0, _ED3E._E000(97381) + camera.name + _ED3E._E000(97658), ComputeBufferType.DrawIndirect);
		}
		if (crd.FrustumPlanes == null)
		{
			Plane[] array = GeometryUtility.CalculateFrustumPlanes(camera);
			for (int l = 0; l < 6; l++)
			{
				this.m__E009[l] = new Vector4(array[l].normal.x, array[l].normal.y, array[l].normal.z, array[l].distance);
			}
			crd.FrustumPlanes = _E016(this.m__E009, 0);
		}
		if (crd.FrustumPNFactors == null)
		{
			_E019(camera, ref _E02D);
			crd.FrustumPNFactors = _E016(_E02D, 0);
		}
		if (crd.InstancesIDsAfterFrustumCulling == null)
		{
			int[] array2 = new int[this.m__E010._allTransformsBuffer.count + PiecesInstancesRingSize];
			for (int m = 0; m < array2.Length; m++)
			{
				array2[m] = m;
			}
			crd.InstancesIDsAfterFrustumCulling = _E016(array2, 0);
		}
		if (crd.InstancesIDsCount == null)
		{
			int[] data2 = new int[3] { 0, 1, 1 };
			crd.InstancesIDsCount = _E016(data2, 0, _ED3E._E000(97381) + camera.name + _ED3E._E000(97644), ComputeBufferType.DrawIndirect);
		}
		if (crd.OcclusionComputeGroupsCountBuffer == null)
		{
			int[] data3 = new int[3] { 1, 1, 1 };
			crd.OcclusionComputeGroupsCountBuffer = _E016(data3, 0, _ED3E._E000(97381) + camera.name + _ED3E._E000(97695), ComputeBufferType.DrawIndirect);
		}
		if (crd.CameraProjViewMatrixBuffer == null)
		{
			crd.CameraProjViewMatrixBuffer = _E015(crd.ProjViewMatrix, 0);
		}
		if (crd.CameraParameters == null)
		{
			_E02E[0] = camera.nearClipPlane;
			_E02E[1] = camera.farClipPlane;
			_E02E[2] = camera.farClipPlane - camera.nearClipPlane;
			crd.CameraParameters = _E016(_E02E, 0);
		}
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97669), this.m__E010._verticesInstances);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97710), this.m__E010._verticesLocalOffsets);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97746), this.m__E010._verticesIDs);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97789), this.m__E010._instancesMeshesIndices);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97763), this.m__E010._meshesOffsets);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97793), this.m__E010._meshesVerticesCounts);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97828), this.m__E010._allVerticesBuffer);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97876), this.m__E010._allBoundsBuffer);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97866), this.m__E010._allTransformsBuffer);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97910), this.m__E010._allTrianglesBuffer);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97895), this.m__E010._texturesOffsetsBuffer);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97934), this.m__E010._instanceIdToStartVertexId);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97969), this.m__E010._materialsProperties);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(98013), crd.CameraParameters);
		crd.CmdBufDrawing.SetGlobalBuffer(_ED3E._E000(97998), this.m__E010._lightsProperties);
		crd.CmdBufDrawing.SetGlobalFloat(_ED3E._E000(98047), 0.2f);
		crd.CmdBufDrawing.SetGlobalFloat(_ED3E._E000(98017), 1f);
		crd.CmdBufDrawing.SetGlobalFloat(_ED3E._E000(98060), 1f);
		crd.CmdBufDrawing.SetGlobalTexture(_ED3E._E000(98109), _texture2DArray);
		crd.CmdBufDrawing.SetGlobalTexture(_ED3E._E000(98092), _cubemapArray);
		if (this.m__E005)
		{
			int cameraWidth = crd.GetCameraWidth(camera);
			int cameraHeight = crd.GetCameraHeight(camera);
			if (crd.MomentsRT == null)
			{
				crd.MomentsRT = _E01B(null, cameraWidth, cameraHeight, _ED3E._E000(98083), GraphicsFormat.R16G16B16A16_SFloat, UAV: true);
				crd._oitRTIs[0] = new RenderTargetIdentifier(crd.MomentsRT);
				crd._rtisToClear[0] = crd._oitRTIs[0];
			}
			if (crd.MomentsRT1 == null)
			{
				crd.MomentsRT1 = _E01B(null, cameraWidth, cameraHeight, _ED3E._E000(98133), GraphicsFormat.R16_SFloat, UAV: true);
				crd._oitRTIs[1] = new RenderTargetIdentifier(crd.MomentsRT1);
				crd._rtisToClear[1] = crd._oitRTIs[1];
			}
			if (crd.TransmittanceSumRcp == null)
			{
				crd.TransmittanceSumRcp = _E01B(null, cameraWidth, cameraHeight, _ED3E._E000(98120), GraphicsFormat.R16_SFloat, UAV: true);
				crd._oitSumRcp = new RenderTargetIdentifier(crd.TransmittanceSumRcp);
				crd._rtisToClear[2] = crd._oitSumRcp;
			}
			if (crd.OpaqueRT == null)
			{
				crd.OpaqueRT = _E01B(null, cameraWidth, cameraHeight, _ED3E._E000(98164), GraphicsFormat.R16G16B16A16_SFloat, UAV: true);
				crd._opaqueRTI = new RenderTargetIdentifier(crd.OpaqueRT);
				crd.RenderTexturesWidth = cameraWidth;
				crd.RenderTexturesHeight = cameraHeight;
			}
			if (crd.StubDepth == null)
			{
				crd.StubDepth = _E01C(null, cameraWidth, cameraHeight, _ED3E._E000(98154), RenderTextureFormat.Depth);
				crd._stubDepthRTI = new RenderTargetIdentifier(crd.StubDepth);
			}
			crd.CmdBufBeforeTransparent.SetGlobalBuffer(_ED3E._E000(98013), crd.CameraParameters);
			crd.CmdBufBeforeTransparent.Blit(BuiltinRenderTextureType.CameraTarget, crd._opaqueRTI);
			crd.CmdBufBeforeTransparent.EnableShaderKeyword(_ED3E._E000(98207));
			crd.CmdBufBeforeTransparent.SetRenderTarget(crd._rtisToClear, BuiltinRenderTextureType.CameraTarget);
			crd.CmdBufBeforeTransparent.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
			crd.CmdBufBeforeTransparent.SetRenderTarget(crd._oitRTIs, BuiltinRenderTextureType.CameraTarget);
			foreach (Renderer renderersForOITMoment in RenderersForOITMoments)
			{
				crd.CmdBufBeforeTransparent.DrawRenderer(renderersForOITMoment, renderersForOITMoment.sharedMaterial);
			}
			crd.CmdBufDrawing.EnableShaderKeyword(_ED3E._E000(98192));
			crd.CmdBufDrawing.SetGlobalTexture(_ED3E._E000(98190), crd.MomentsRT);
			crd.CmdBufDrawing.SetGlobalTexture(_ED3E._E000(98182), crd.MomentsRT1);
			foreach (Renderer renderersForOITMoment2 in RenderersForOITMoments)
			{
				_ = renderersForOITMoment2;
			}
			crd.CmdBufBeforeTransparent.EnableShaderKeyword(_ED3E._E000(98228));
			crd.CmdBufBeforeTransparent.EnableShaderKeyword(_ED3E._E000(98222));
			crd.CmdBufBeforeTransparent.DrawProceduralIndirect(Matrix4x4.identity, WindowsMaterial, 0, MeshTopology.Triangles, crd.DrawArgsBuffer);
			crd.CmdBufBeforeTransparent.DisableShaderKeyword(_ED3E._E000(98228));
			crd.CmdBufBeforeTransparent.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
			crd.CmdBufBeforeTransparent.SetGlobalTexture(_ED3E._E000(98190), crd.MomentsRT);
			crd.CmdBufBeforeTransparent.SetGlobalTexture(_ED3E._E000(98182), crd.MomentsRT1);
			crd.CmdBufBeforeTransparent.SetGlobalTexture(_ED3E._E000(19728), crd._opaqueRTI);
			crd.CmdBufBeforeTransparent.SetGlobalTexture(_ED3E._E000(18256), BuiltinRenderTextureType.ResolvedDepth);
			crd.CmdBufBeforeTransparent.Blit(crd._opaqueRTI, BuiltinRenderTextureType.CameraTarget, MBOITBlitOpaque);
			crd.CmdBufBeforeTransparent.EnableShaderKeyword(_ED3E._E000(98192));
		}
		if (!this.m__E003 || FrustumCullShader == null)
		{
			crd.CmdBufDrawing.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
			crd.CmdBufDrawing.DisableShaderKeyword(_ED3E._E000(98222));
			crd.CmdBufDrawing.DrawProcedural(Matrix4x4.identity, WindowsMaterial, 0, MeshTopology.Triangles, _verticesInstances.Count);
		}
		else
		{
			crd.CmdBufCulling.SetComputeIntParam(FrustumCullShader, _ED3E._E000(98270), this.m__E010._allTransformsBuffer.count);
			if (!this.m__E004 || crd.HiZGenerator == null)
			{
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(98245), crd.FrustumPlanes);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(98299), crd.FrustumPNFactors);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(97789), this.m__E010._instancesMeshesIndices);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(97793), this.m__E010._meshesVerticesCounts);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(98274), this.m__E010._allBoundsBuffer);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(97934), this.m__E010._instanceIdToStartVertexId);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(96275), crd.DrawArgsBuffer);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(96258), this.m__E010._verticesIDs);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 0, _ED3E._E000(96311), this.m__E010._instancesEnable);
				crd.CmdBufCulling.DispatchCompute(FrustumCullShader, 0, (this.m__E010._allTransformsBuffer.count >> 6) + 1, 1, 1);
			}
			else
			{
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 2, _ED3E._E000(98245), crd.FrustumPlanes);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 2, _ED3E._E000(98299), crd.FrustumPNFactors);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 2, _ED3E._E000(97789), this.m__E010._instancesMeshesIndices);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 2, _ED3E._E000(97793), this.m__E010._meshesVerticesCounts);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 2, _ED3E._E000(98274), this.m__E010._allBoundsBuffer);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 2, _ED3E._E000(97934), this.m__E010._instanceIdToStartVertexId);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 2, _ED3E._E000(96275), crd.InstancesIDsCount);
				crd.CmdBufCulling.SetComputeBufferParam(FrustumCullShader, 2, _ED3E._E000(96258), crd.InstancesIDsAfterFrustumCulling);
				crd.CmdBufCulling.DispatchCompute(FrustumCullShader, 2, (this.m__E010._allTransformsBuffer.count >> 6) + 1, 1, 1);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 1, _ED3E._E000(98270), crd.InstancesIDsCount);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 1, _ED3E._E000(96344), crd.OcclusionComputeGroupsCountBuffer);
				crd.CmdBufCulling.DispatchCompute(OcclusionCullShader, 1, 1, 1, 1);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(98270), crd.InstancesIDsCount);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(97789), this.m__E010._instancesMeshesIndices);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(97793), this.m__E010._meshesVerticesCounts);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(98274), this.m__E010._allBoundsBuffer);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(97934), this.m__E010._instanceIdToStartVertexId);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(96275), crd.DrawArgsBuffer);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(96258), this.m__E010._verticesIDs);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(96325), crd.InstancesIDsAfterFrustumCulling);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(96377), crd.CameraProjViewMatrixBuffer);
				crd.CmdBufCulling.SetComputeBufferParam(OcclusionCullShader, 0, _ED3E._E000(96311), this.m__E010._instancesEnable);
				crd.CmdBufCulling.SetComputeIntParam(OcclusionCullShader, _ED3E._E000(96360), OcclusionAccuracy);
				crd.CmdBufCulling.SetComputeFloatParam(OcclusionCullShader, _ED3E._E000(96410), OcclusionOffset);
				crd.CmdBufCulling.SetComputeTextureParam(OcclusionCullShader, 0, _ED3E._E000(96394), crd.HiZGenerator.hiZDepthTexture);
				crd.CmdBufCulling.SetComputeVectorParam(OcclusionCullShader, _ED3E._E000(96385), crd.HiZGenerator.hiZTextureSize);
				crd.CmdBufCulling.DispatchCompute(OcclusionCullShader, 0, crd.OcclusionComputeGroupsCountBuffer, 0u);
			}
			crd.CmdBufDrawing.EnableShaderKeyword(_ED3E._E000(98222));
			if (MBOITEnergyConservative)
			{
				crd.CmdBufDrawing.BeginSample(_ED3E._E000(96437));
				crd.CmdBufDrawing.DisableShaderKeyword(_ED3E._E000(98192));
				crd.CmdBufDrawing.SetRenderTarget(crd._oitSumRcp, BuiltinRenderTextureType.CameraTarget);
				crd.CmdBufDrawing.EnableShaderKeyword(_ED3E._E000(96422));
				crd.CmdBufDrawing.DrawProceduralIndirect(Matrix4x4.identity, WindowsMaterial, 0, MeshTopology.Triangles, crd.DrawArgsBuffer);
				crd.CmdBufDrawing.DisableShaderKeyword(_ED3E._E000(96422));
				crd.CmdBufDrawing.EnableShaderKeyword(_ED3E._E000(96452));
				crd.CmdBufDrawing.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
				crd.CmdBufDrawing.SetGlobalTexture(_ED3E._E000(96489), crd.TransmittanceSumRcp);
				crd.CmdBufDrawing.EndSample(_ED3E._E000(96437));
			}
			crd.CmdBufDrawing.BeginSample(_ED3E._E000(96535));
			crd.CmdBufDrawing.DrawProceduralIndirect(Matrix4x4.identity, WindowsMaterial, 0, MeshTopology.Triangles, crd.DrawArgsBuffer);
			crd.CmdBufDrawing.DisableShaderKeyword(_ED3E._E000(96452));
			crd.CmdBufDrawing.EndSample(_ED3E._E000(96535));
			crd.CmdBufDrawing.SetComputeBufferParam(FrustumCullShader, 1, _ED3E._E000(96275), crd.DrawArgsBuffer);
			crd.CmdBufDrawing.DispatchCompute(FrustumCullShader, 1, 1, 1, 1);
			crd.CmdBufDrawing.SetComputeBufferParam(FrustumCullShader, 1, _ED3E._E000(96275), crd.InstancesIDsCount);
			crd.CmdBufDrawing.DispatchCompute(FrustumCullShader, 1, 1, 1, 1);
		}
		crd.CmdBufDrawing.DisableShaderKeyword(_ED3E._E000(98192));
	}

	public void BreakWindow(string breakerId)
	{
		if (!IsActive || !this.m__E013.ContainsKey(breakerId))
		{
			return;
		}
		if (this.m__E010._instancesEnable != null)
		{
			int computeBufferStartIndex = this.m__E013[breakerId];
			if (_E02F.Count != 0)
			{
				_E02F[0] = 0;
			}
			else
			{
				_E02F.Add(0);
			}
			this.m__E010._instancesEnable.SetData(_E02F, 0, computeBufferStartIndex, 1);
		}
		else
		{
			this.m__E016.Add(breakerId);
		}
	}
}
