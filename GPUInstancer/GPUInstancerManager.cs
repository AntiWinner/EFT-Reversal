using System;
using System.Collections.Generic;
using System.Threading;
using Comfort.Common;
using EFT.Settings.Graphics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace GPUInstancer;

public abstract class GPUInstancerManager : MonoBehaviour
{
	public class _E000
	{
		public Thread thread;

		public object parameter;
	}

	public bool IsOptic;

	public List<GPUInstancerPrototype> prototypeList;

	public GPUInstancerCameraData cameraData = new GPUInstancerCameraData();

	public bool useFloatingOriginHandler;

	public Transform floatingOriginTransform;

	public GPUInstancerFloatingOriginHandler floatingOriginHandler;

	public GPUInstancerTerrainSettings terrainSettings;

	public List<_E4C2> runtimeDataList;

	public Bounds instancingBounds;

	public bool isFrustumCulling = true;

	public bool isOcclusionCulling = true;

	public float minCullingDistance;

	protected _E4C6<_E4BE> spData;

	public static List<GPUInstancerManager> activeManagerList;

	public static bool showRenderedAmount;

	protected static ComputeShader _cameraComputeShader;

	protected static int[] _cameraComputeKernelIDs;

	protected static ComputeShader _visibilityComputeShader;

	protected static int[] _instanceVisibilityComputeKernelIDs;

	protected static ComputeShader _bufferToTextureComputeShader;

	protected static int _bufferToTextureComputeKernelID;

	public int maxThreads = 3;

	public readonly List<Thread> activeThreads = new List<Thread>();

	public readonly Queue<_E000> threadStartQueue = new Queue<_E000>();

	public readonly Queue<Action> threadQueue = new Queue<Action>();

	public static int lastTreePositionUpdate;

	public static GameObject treeProxyParent;

	public static Dictionary<GameObject, Transform> treeProxyList;

	public static int lastDrawCallFrame;

	public static float lastDrawCallTime;

	public static float timeSinceLastDrawCall;

	protected static Vector4 _windVector = Vector4.zero;

	protected bool isInitial = true;

	public bool isInitialized;

	public Dictionary<GPUInstancerPrototype, _E4C2> runtimeDataDictionary;

	public LayerMask layerMask = -1;

	protected static CommandBuffer motionVectorsCb;

	protected static CommandBuffer motionVectorsAfterCb;

	protected static RenderTexture motionVectorsRt;

	protected static RenderTexture motionVectorsRtDepth;

	protected static CommandBuffer motionVectorsCbOptic;

	protected static CommandBuffer motionVectorsAfterCbOptic;

	protected static RenderTexture motionVectorsRtOptic;

	protected static RenderTexture motionVectorsRtDepthOptic;

	protected static RenderTargetIdentifier[] mrt;

	protected static RenderTargetIdentifier[] mrtOptic;

	protected static bool isMotionVectorDataClear;

	protected static bool isMotionVectorDataClearOptic;

	protected static bool isMotionVectorsInit;

	protected readonly _E3A4 CompositeDisposable = new _E3A4();

	public Exception threadException;

	private static readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(77789));

	private static readonly int m__E001 = Shader.PropertyToID(_ED3E._E000(77787));

	protected virtual bool isCulled => false;

	protected Camera Camera => cameraData?.mainCamera;

	protected SSAA CameraSSAA => cameraData?.mainCameraSSAA;

	public CommandBuffer MotionVectorsCommandBuffer
	{
		get
		{
			if (!IsOptic)
			{
				return motionVectorsCb;
			}
			return motionVectorsCbOptic;
		}
	}

	public CommandBuffer MotionVectorsAfterCommandBuffer
	{
		get
		{
			if (!IsOptic)
			{
				return motionVectorsAfterCb;
			}
			return motionVectorsAfterCbOptic;
		}
	}

	public RenderTexture MotionVectorsTexture
	{
		get
		{
			if (!IsOptic)
			{
				return motionVectorsRt;
			}
			return motionVectorsRtOptic;
		}
	}

	public RenderTexture MotionVectorsDepth
	{
		get
		{
			if (!IsOptic)
			{
				return motionVectorsRtDepth;
			}
			return motionVectorsRtDepthOptic;
		}
	}

	public bool IsMotionVectorDataClear
	{
		get
		{
			if (!IsOptic)
			{
				return isMotionVectorDataClear;
			}
			return isMotionVectorDataClearOptic;
		}
		set
		{
			if (IsOptic)
			{
				isMotionVectorDataClearOptic = value;
			}
			else
			{
				isMotionVectorDataClear = value;
			}
		}
	}

	public RenderTargetIdentifier[] MRT
	{
		get
		{
			if (!IsOptic)
			{
				return mrt;
			}
			return mrtOptic;
		}
	}

	protected static bool bGenerateMotionVectors
	{
		get
		{
			if (_E8A8.Exist && _E8A8.Instance.GetSSREnabled())
			{
				return true;
			}
			if (Singleton<_E7DE>.Instantiated)
			{
				_E7EB settings = Singleton<_E7DE>.Instance.Graphics.Settings;
				if (!settings.IsTaaEnabled() && !settings.IsDLSSEnabled() && !settings.IsFSR2Enabled())
				{
					return settings.SSR.Value > ESSRMode.Off;
				}
				return true;
			}
			return false;
		}
	}

	protected static bool isTaaEnabled
	{
		get
		{
			if (!Singleton<_E7DE>.Instantiated)
			{
				return false;
			}
			_E7EB settings = Singleton<_E7DE>.Instance.Graphics.Settings;
			if (!settings.IsTaaEnabled())
			{
				return settings.IsDLSSEnabled();
			}
			return true;
		}
	}

	protected static int OpticResolution => _E8A8.Instance.OpticCameraManager.OpticRenderResolution;

	public virtual void Awake()
	{
		if (!isMotionVectorsInit)
		{
			isMotionVectorsInit = true;
			if (Singleton<_E7DE>.Instantiated)
			{
				CompositeDisposable.BindState(Singleton<_E7DE>.Instance.Graphics.Settings.DisplaySettings, OnResolutionChangeStatic);
			}
			CreateMotionVectorsData();
		}
		if (_E4BF.gpuiSettings == null)
		{
			_E4BF.gpuiSettings = GPUInstancerSettings.GetDefaultGPUInstancerSettings();
		}
		_E4BF.gpuiSettings.SetDefultBindings();
		_E4C8.SetPlatformDependentVariables();
		if (Application.isPlaying && activeManagerList == null)
		{
			activeManagerList = new List<GPUInstancerManager>();
		}
		if (_visibilityComputeShader == null)
		{
			_visibilityComputeShader = (ComputeShader)Resources.Load(_E4BF.VISIBILITY_COMPUTE_RESOURCE_PATH);
			switch (_E4C8.matrixHandlingType)
			{
			case GPUIMatrixHandlingType.MatrixAppend:
				_visibilityComputeShader = (ComputeShader)Resources.Load(_E4BF.VISIBILITY_COMPUTE_RESOURCE_PATH_VULKAN);
				_E4BF.DETAIL_STORE_INSTANCE_DATA = true;
				_E4BF.COMPUTE_MAX_LOD_BUFFER = 3;
				break;
			case GPUIMatrixHandlingType.CopyToTexture:
				_visibilityComputeShader = (ComputeShader)Resources.Load(_E4BF.VISIBILITY_COMPUTE_RESOURCE_PATH);
				_bufferToTextureComputeShader = (ComputeShader)Resources.Load(_E4BF.BUFFER_TO_TEXTURE_COMPUTE_RESOURCE_PATH);
				_bufferToTextureComputeKernelID = _bufferToTextureComputeShader.FindKernel(_E4BF.BUFFER_TO_TEXTURE_KERNEL);
				break;
			default:
				_visibilityComputeShader = (ComputeShader)Resources.Load(_E4BF.VISIBILITY_COMPUTE_RESOURCE_PATH);
				break;
			}
			_instanceVisibilityComputeKernelIDs = new int[_E4BF.VISIBILITY_COMPUTE_KERNELS.Length];
			for (int i = 0; i < _instanceVisibilityComputeKernelIDs.Length; i++)
			{
				_instanceVisibilityComputeKernelIDs[i] = _visibilityComputeShader.FindKernel(_E4BF.VISIBILITY_COMPUTE_KERNELS[i]);
			}
			_E4BF.TEXTURE_MAX_SIZE = SystemInfo.maxTextureSize;
		}
		if (_cameraComputeShader == null)
		{
			_cameraComputeShader = (ComputeShader)Resources.Load(_E4BF.CAMERA_COMPUTE_RESOURCE_PATH);
			if (isOcclusionCulling && XRSettings.enabled && _E4BF.gpuiSettings.testBothEyesForVROcclusion)
			{
				_cameraComputeShader = (ComputeShader)Resources.Load(_E4BF.CAMERA_VR_COMPUTE_RESOURCE_PATH);
			}
			_cameraComputeKernelIDs = new int[_E4BF.CAMERA_COMPUTE_KERNELS.Length];
			for (int j = 0; j < _cameraComputeKernelIDs.Length; j++)
			{
				_cameraComputeKernelIDs[j] = _cameraComputeShader.FindKernel(_E4BF.CAMERA_COMPUTE_KERNELS[j]);
			}
		}
		_E4BF.SetupComputeRuntimeModification();
		_E4BF.SetupComputeSetDataPartial();
		showRenderedAmount = false;
		InitializeCameraData();
		SetupOcclusionCulling();
	}

	protected static void OnResolutionChangeStatic(_E7E4 displaySettings)
	{
		_E000(displaySettings.Resolution);
		_E001();
	}

	private static void _E000(EftResolution resolution)
	{
		if (motionVectorsRt != null)
		{
			motionVectorsRt.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRt);
		}
		motionVectorsRt = new RenderTexture(resolution.Width, resolution.Height, 0, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
		{
			name = _ED3E._E000(77223),
			useMipMap = false,
			antiAliasing = 1,
			wrapMode = TextureWrapMode.Clamp
		};
		if ((bool)motionVectorsRtDepth)
		{
			motionVectorsRtDepth.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtDepth);
		}
		motionVectorsRtDepth = new RenderTexture(resolution.Width, resolution.Height, 0, RenderTextureFormat.RFloat)
		{
			name = _ED3E._E000(77260),
			useMipMap = false,
			antiAliasing = 1,
			wrapMode = TextureWrapMode.Clamp
		};
		mrt = new RenderTargetIdentifier[2] { motionVectorsRt, motionVectorsRtDepth };
	}

	private static void _E001()
	{
		if (motionVectorsRtOptic != null)
		{
			motionVectorsRtOptic.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtOptic);
		}
		motionVectorsRtOptic = new RenderTexture(OpticResolution, OpticResolution, 0, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
		{
			name = _ED3E._E000(77302),
			useMipMap = false,
			antiAliasing = 1,
			wrapMode = TextureWrapMode.Clamp
		};
		if (motionVectorsRtDepthOptic != null)
		{
			motionVectorsRtDepthOptic.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtDepthOptic);
		}
		motionVectorsRtDepthOptic = new RenderTexture(OpticResolution, OpticResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear)
		{
			name = _ED3E._E000(77336),
			useMipMap = false,
			antiAliasing = 1,
			wrapMode = TextureWrapMode.Clamp
		};
		mrtOptic = new RenderTargetIdentifier[2] { motionVectorsRtOptic, motionVectorsRtDepthOptic };
	}

	protected void CreateMotionVectorsData()
	{
		DisposeMotionVectorsData();
		motionVectorsCb = new CommandBuffer
		{
			name = _ED3E._E000(77375)
		};
		motionVectorsAfterCb = new CommandBuffer
		{
			name = _ED3E._E000(77353)
		};
		if (motionVectorsRt != null)
		{
			motionVectorsRt.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRt);
		}
		motionVectorsRt = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
		{
			name = _ED3E._E000(77223),
			useMipMap = false,
			antiAliasing = 1,
			wrapMode = TextureWrapMode.Clamp
		};
		if (motionVectorsRtDepth != null)
		{
			motionVectorsRtDepth.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtDepth);
		}
		motionVectorsRtDepth = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.RFloat)
		{
			name = _ED3E._E000(77260),
			useMipMap = false,
			antiAliasing = 1,
			wrapMode = TextureWrapMode.Clamp
		};
		mrt = new RenderTargetIdentifier[2] { motionVectorsRt, motionVectorsRtDepth };
		motionVectorsCbOptic = new CommandBuffer
		{
			name = _ED3E._E000(77392)
		};
		motionVectorsAfterCbOptic = new CommandBuffer
		{
			name = _ED3E._E000(77439)
		};
		if (motionVectorsRtOptic != null)
		{
			motionVectorsRtOptic.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtOptic);
		}
		motionVectorsRtOptic = new RenderTexture(OpticResolution, OpticResolution, 0, RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
		{
			name = _ED3E._E000(77302),
			useMipMap = false,
			antiAliasing = 1,
			wrapMode = TextureWrapMode.Clamp
		};
		if (motionVectorsRtDepthOptic != null)
		{
			motionVectorsRtDepthOptic.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtDepthOptic);
		}
		motionVectorsRtDepthOptic = new RenderTexture(OpticResolution, OpticResolution, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear)
		{
			name = _ED3E._E000(77336),
			useMipMap = false,
			antiAliasing = 1,
			wrapMode = TextureWrapMode.Clamp
		};
		mrtOptic = new RenderTargetIdentifier[2] { motionVectorsRtOptic, motionVectorsRtDepthOptic };
		Shader.SetGlobalFloat(GPUInstancerManager.m__E001, 1f);
	}

	public virtual void OnEnable()
	{
		if (activeManagerList != null && !activeManagerList.Contains(this))
		{
			activeManagerList.Add(this);
		}
		if (_E4BF.gpuiSettings == null || _E4BF.gpuiSettings.shaderBindings == null)
		{
			Debug.LogWarning(_ED3E._E000(77411));
		}
		if (runtimeDataList == null || runtimeDataList.Count == 0)
		{
			InitializeRuntimeDataAndBuffers();
		}
		isInitial = true;
		if (!useFloatingOriginHandler || !(floatingOriginTransform != null))
		{
			return;
		}
		if (floatingOriginHandler == null)
		{
			floatingOriginHandler = floatingOriginTransform.gameObject.GetComponent<GPUInstancerFloatingOriginHandler>();
			if (floatingOriginHandler == null)
			{
				floatingOriginHandler = floatingOriginTransform.gameObject.AddComponent<GPUInstancerFloatingOriginHandler>();
			}
		}
		if (floatingOriginHandler.gPUIManagers == null)
		{
			floatingOriginHandler.gPUIManagers = new List<GPUInstancerManager>();
		}
		if (!floatingOriginHandler.gPUIManagers.Contains(this))
		{
			floatingOriginHandler.gPUIManagers.Add(this);
		}
	}

	protected void ClearMotionVectorData()
	{
		if (!IsMotionVectorDataClear && !(Camera == null) && MotionVectorsCommandBuffer != null)
		{
			MotionVectorsCommandBuffer.Clear();
			MotionVectorsAfterCommandBuffer.Clear();
			if (bGenerateMotionVectors)
			{
				Matrix4x4 value = GL.GetGPUProjectionMatrix(Camera.projectionMatrix, renderIntoTexture: true) * Camera.worldToCameraMatrix;
				Matrix4x4 previousViewProjectionMatrix = Camera.previousViewProjectionMatrix;
				MotionVectorsCommandBuffer.SetGlobalMatrix(_ED3E._E000(77532), previousViewProjectionMatrix);
				MotionVectorsCommandBuffer.SetGlobalMatrix(_ED3E._E000(77520), value);
				MotionVectorsCommandBuffer.SetRenderTarget(MRT, BuiltinRenderTextureType.CameraTarget);
				MotionVectorsCommandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
				MotionVectorsAfterCommandBuffer.SetGlobalTexture(_ED3E._E000(77511), MotionVectorsTexture);
				MotionVectorsAfterCommandBuffer.SetGlobalTexture(_ED3E._E000(77540), MotionVectorsDepth);
			}
			else
			{
				MotionVectorsCommandBuffer.SetRenderTarget(MotionVectorsDepth, BuiltinRenderTextureType.CameraTarget);
				MotionVectorsCommandBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.black);
				MotionVectorsCommandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.CameraTarget);
			}
			IsMotionVectorDataClear = true;
		}
	}

	public virtual void Update()
	{
		if (Camera != null && !IsOptic && motionVectorsRt != null && CameraSSAA != null)
		{
			int inputWidth = CameraSSAA.GetInputWidth();
			int inputHeight = CameraSSAA.GetInputHeight();
			if (inputWidth != MotionVectorsTexture.width || inputHeight != MotionVectorsTexture.height)
			{
				_E000(new EftResolution(inputWidth, inputHeight));
			}
		}
		int opticRenderResolution;
		if (IsOptic && motionVectorsRtOptic != null && motionVectorsRtDepthOptic != null && ((opticRenderResolution = _E8A8.Instance.OpticCameraManager.OpticRenderResolution) != motionVectorsRtOptic.width || opticRenderResolution != motionVectorsRtOptic.height))
		{
			_E001();
		}
		ClearCompletedThreads();
		ClearMotionVectorData();
		while (threadStartQueue.Count > 0 && activeThreads.Count < maxThreads)
		{
			_E000 obj = threadStartQueue.Dequeue();
			obj.thread.Start(obj.parameter);
			activeThreads.Add(obj.thread);
		}
		if (threadQueue.Count > 0)
		{
			threadQueue.Dequeue()?.Invoke();
		}
		if (Application.isPlaying && (bool)treeProxyParent && lastTreePositionUpdate != Time.frameCount && cameraData.mainCamera != null)
		{
			treeProxyParent.transform.position = cameraData.mainCamera.transform.position;
			treeProxyParent.transform.rotation = cameraData.mainCamera.transform.rotation;
			lastTreePositionUpdate = Time.frameCount;
		}
		if (Camera == null)
		{
			InitializeCameraData();
		}
		if (!(Camera == null) && !isCulled && Camera.isActiveAndEnabled)
		{
			UpdateTreeMPB();
			UpdateBuffers();
			_E4C8.DrawPrePass(MotionVectorsCommandBuffer, runtimeDataList, instancingBounds, cameraData, bGenerateMotionVectors);
			_E4C8.GPUIDrawMeshInstancedIndirect(runtimeDataList, instancingBounds, cameraData);
		}
	}

	public void LateUpdate()
	{
		IsMotionVectorDataClear = false;
	}

	public virtual void OnDestroy()
	{
		CompositeDisposable.Dispose();
		DisposeMotionVectorsData();
	}

	public void DisposeMotionVectorsData()
	{
		Shader.SetGlobalFloat(GPUInstancerManager.m__E001, 0f);
		isMotionVectorsInit = false;
		if (motionVectorsCb != null)
		{
			motionVectorsCb.Release();
		}
		if (motionVectorsCbOptic != null)
		{
			motionVectorsCbOptic.Release();
		}
		if (motionVectorsRt != null)
		{
			motionVectorsRt.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRt);
		}
		if (motionVectorsRtOptic != null)
		{
			motionVectorsRtOptic.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtOptic);
		}
		if (motionVectorsAfterCb != null)
		{
			motionVectorsAfterCb.Release();
		}
		if (motionVectorsAfterCbOptic != null)
		{
			motionVectorsAfterCbOptic.Release();
		}
		if (motionVectorsRtDepth != null)
		{
			motionVectorsRtDepth.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtDepth);
		}
		if (motionVectorsRtDepthOptic != null)
		{
			motionVectorsRtDepthOptic.Release();
			UnityEngine.Object.DestroyImmediate(motionVectorsRtDepthOptic);
		}
		motionVectorsCb = null;
		motionVectorsCbOptic = null;
		motionVectorsRt = null;
		motionVectorsRtOptic = null;
		motionVectorsRtDepth = null;
		motionVectorsRtDepthOptic = null;
		motionVectorsAfterCb = null;
		motionVectorsAfterCbOptic = null;
	}

	public virtual void Reset()
	{
		_E4BF.gpuiSettings.SetDefultBindings();
	}

	public virtual void OnDisable()
	{
		activeManagerList?.Remove(this);
		ClearInstancingData();
		if (floatingOriginHandler?.gPUIManagers != null && floatingOriginHandler.gPUIManagers.Contains(this))
		{
			floatingOriginHandler.gPUIManagers.Remove(this);
		}
	}

	public virtual void ClearInstancingData()
	{
		_E4C8.ReleaseInstanceBuffers(runtimeDataList);
		_E4C8.ReleaseSPBuffers(spData);
		runtimeDataList?.Clear();
		runtimeDataDictionary?.Clear();
		spData = null;
		threadStartQueue.Clear();
		threadQueue.Clear();
		isInitialized = false;
	}

	public virtual void GeneratePrototypes(bool forceNew = false)
	{
		ClearInstancingData();
		if (forceNew || prototypeList == null)
		{
			prototypeList = new List<GPUInstancerPrototype>();
		}
		else
		{
			prototypeList.RemoveAll((GPUInstancerPrototype p) => p == null);
		}
		if (_E4BF.gpuiSettings == null)
		{
			_E4BF.gpuiSettings = GPUInstancerSettings.GetDefaultGPUInstancerSettings();
		}
		_E4BF.gpuiSettings.SetDefultBindings();
	}

	public virtual void InitializeRuntimeDataAndBuffers(bool forceNew = true)
	{
		_E4C8.SetPlatformDependentVariables();
		if (forceNew || !isInitialized)
		{
			instancingBounds = new Bounds(Vector3.zero, Vector3.one * _E4BF.gpuiSettings.instancingBoundsSize);
			_E4C8.ReleaseInstanceBuffers(runtimeDataList);
			_E4C8.ReleaseSPBuffers(spData);
			if (runtimeDataList != null)
			{
				runtimeDataList.Clear();
			}
			else
			{
				runtimeDataList = new List<_E4C2>();
			}
			if (runtimeDataDictionary != null)
			{
				runtimeDataDictionary.Clear();
			}
			else
			{
				runtimeDataDictionary = new Dictionary<GPUInstancerPrototype, _E4C2>();
			}
			if (prototypeList == null)
			{
				prototypeList = new List<GPUInstancerPrototype>();
			}
		}
	}

	public virtual void InitializeSpatialPartitioning()
	{
	}

	public virtual void UpdateSpatialPartitioningCells(GPUInstancerCameraData renderingCameraData)
	{
	}

	public virtual void DeletePrototype(GPUInstancerPrototype prototype, bool removeSO = true)
	{
		prototypeList.Remove(prototype);
		if (removeSO && prototype.useGeneratedBillboard && prototype.billboard != null && _E4BF.gpuiSettings.billboardAtlasBindings.DeleteBillboardTextures(prototype))
		{
			prototype.billboard = null;
		}
	}

	public virtual void RemoveInstancesInsideBounds(Bounds bounds, float offset, List<GPUInstancerPrototype> prototypeFilter = null)
	{
		if (runtimeDataList == null)
		{
			return;
		}
		foreach (_E4C2 runtimeData in runtimeDataList)
		{
			if (prototypeFilter == null || prototypeFilter.Contains(runtimeData.prototype))
			{
				_E4C8.RemoveInstancesInsideBounds(runtimeData.transformationMatrixVisibilityBuffer, bounds.center, bounds.extents, offset);
			}
		}
	}

	public virtual void RemoveInstancesInsideCollider(Collider collider, float offset, List<GPUInstancerPrototype> prototypeFilter = null)
	{
		if (runtimeDataList == null)
		{
			return;
		}
		foreach (_E4C2 runtimeData in runtimeDataList)
		{
			if (prototypeFilter == null || prototypeFilter.Contains(runtimeData.prototype))
			{
				if (collider is BoxCollider)
				{
					_E4C8.RemoveInstancesInsideBoxCollider(runtimeData.transformationMatrixVisibilityBuffer, (BoxCollider)collider, offset);
				}
				else if (collider is SphereCollider)
				{
					_E4C8.RemoveInstancesInsideSphereCollider(runtimeData.transformationMatrixVisibilityBuffer, (SphereCollider)collider, offset);
				}
				else if (collider is CapsuleCollider)
				{
					_E4C8.RemoveInstancesInsideCapsuleCollider(runtimeData.transformationMatrixVisibilityBuffer, (CapsuleCollider)collider, offset);
				}
				else
				{
					_E4C8.RemoveInstancesInsideBounds(runtimeData.transformationMatrixVisibilityBuffer, collider.bounds.center, collider.bounds.extents, offset);
				}
			}
		}
	}

	public virtual void SetGlobalPositionOffset(Vector3 offsetPosition)
	{
	}

	public void ClearCompletedThreads()
	{
		if (activeThreads.Count <= 0)
		{
			return;
		}
		for (int num = activeThreads.Count - 1; num >= 0; num--)
		{
			if (!activeThreads[num].IsAlive)
			{
				activeThreads.RemoveAt(num);
			}
		}
	}

	public void InitializeCameraData()
	{
		Camera camera = (IsOptic ? _E8A8.Instance.OpticCameraManager.Camera : _E8A8.Instance.Camera);
		cameraData.SetCamera(camera);
		if (!(Camera == null) && MotionVectorsCommandBuffer != null)
		{
			Camera.RemoveCommandBuffer(CameraEvent.BeforeGBuffer, MotionVectorsCommandBuffer);
			Camera.RemoveCommandBuffer(CameraEvent.AfterGBuffer, MotionVectorsAfterCommandBuffer);
			Camera.AddCommandBuffer(CameraEvent.BeforeGBuffer, MotionVectorsCommandBuffer);
			Camera.AddCommandBuffer(CameraEvent.AfterGBuffer, MotionVectorsAfterCommandBuffer);
		}
	}

	public void SetupOcclusionCulling()
	{
		if (!(Camera == null) && isOcclusionCulling && !(cameraData.hiZOcclusionGenerator != null))
		{
			GPUInstancerHiZOcclusionGenerator orAddComponent = cameraData.mainCamera.gameObject.GetOrAddComponent<GPUInstancerHiZOcclusionGenerator>();
			cameraData.hiZOcclusionGenerator = orAddComponent;
			cameraData.hiZOcclusionGenerator.Initialize(cameraData.mainCamera);
		}
	}

	public void UpdateBuffers()
	{
		if (!(Camera == null))
		{
			if (isOcclusionCulling && cameraData.hiZOcclusionGenerator == null)
			{
				SetupOcclusionCulling();
			}
			cameraData.CalculateCameraData();
			instancingBounds.center = cameraData.mainCamera.transform.position;
			if (lastDrawCallFrame != Time.frameCount)
			{
				lastDrawCallFrame = Time.frameCount;
				timeSinceLastDrawCall = Time.realtimeSinceStartup - lastDrawCallTime;
				lastDrawCallTime = Time.realtimeSinceStartup;
			}
			UpdateSpatialPartitioningCells(cameraData);
			_E4C8.UpdateGPUBuffers(_cameraComputeShader, _cameraComputeKernelIDs, _visibilityComputeShader, _instanceVisibilityComputeKernelIDs, runtimeDataList, terrainSettings, cameraData, isFrustumCulling, isOcclusionCulling, showRenderedAmount, isInitial);
			if (_E4C8.matrixHandlingType == GPUIMatrixHandlingType.CopyToTexture)
			{
				_E4C8.DispatchBufferToTexture(runtimeDataList, _bufferToTextureComputeShader, _bufferToTextureComputeKernelID);
			}
			isInitial = false;
		}
	}

	public void SetCamera(Camera camera)
	{
		if (cameraData == null)
		{
			cameraData = new GPUInstancerCameraData(camera);
		}
		else
		{
			cameraData.SetCamera(camera);
		}
		if (cameraData.hiZOcclusionGenerator != null)
		{
			UnityEngine.Object.DestroyImmediate(cameraData.hiZOcclusionGenerator);
		}
		SetupOcclusionCulling();
	}

	public ComputeBuffer GetTransformDataBuffer(GPUInstancerPrototype prototype)
	{
		return GetRuntimeData(prototype)?.transformationMatrixVisibilityBuffer;
	}

	public void SetLODBias(float newLODBias, List<GPUInstancerPrototype> prototypeFilter)
	{
		if (runtimeDataList == null || newLODBias <= 0f)
		{
			return;
		}
		foreach (_E4C2 runtimeData in runtimeDataList)
		{
			if (runtimeData == null || runtimeData.lodBiasApplied <= 0f || runtimeData.lodBiasApplied == newLODBias || runtimeData.instanceLODs == null || runtimeData.instanceLODs.Count == 0 || (prototypeFilter != null && !prototypeFilter.Contains(runtimeData.prototype)))
			{
				continue;
			}
			for (int i = 0; i < runtimeData.instanceLODs.Count; i++)
			{
				if (i < 4)
				{
					runtimeData.lodSizes[i * 4] *= runtimeData.lodBiasApplied / newLODBias;
				}
				else
				{
					runtimeData.lodSizes[(i - 4) * 4 + 1] *= runtimeData.lodBiasApplied / newLODBias;
				}
			}
			runtimeData.lodBiasApplied = newLODBias;
		}
	}

	public void UpdateTreeMPB()
	{
		if (treeProxyList == null || treeProxyList.Count <= 0)
		{
			return;
		}
		foreach (_E4C2 runtimeData in runtimeDataList)
		{
			if (runtimeData.bufferSize == 0 || (runtimeData.prototype.treeType != GPUInstancerTreeType.SpeedTree && runtimeData.prototype.treeType != GPUInstancerTreeType.SpeedTree8))
			{
				continue;
			}
			Transform transform = treeProxyList[runtimeData.prototype.prefabObject];
			if (!transform)
			{
				continue;
			}
			for (int i = 0; i < runtimeData.instanceLODs.Count; i++)
			{
				if (transform.childCount <= i)
				{
					continue;
				}
				_E4C3 obj = runtimeData.instanceLODs[i];
				MeshRenderer component = transform.GetChild(i).GetComponent<MeshRenderer>();
				for (int j = 0; j < obj.renderers.Count; j++)
				{
					_E4C4 obj2 = obj.renderers[j];
					component.GetPropertyBlock(obj2.mpb);
					if (obj2.shadowMPB != null)
					{
						component.GetPropertyBlock(obj2.shadowMPB);
					}
				}
			}
			_E4C8.SetAppendBuffers(runtimeData);
		}
	}

	public static void AddTreeProxy(GPUInstancerPrototype treePrototype, _E4C2 runtimeData)
	{
		switch (treePrototype.treeType)
		{
		case GPUInstancerTreeType.SpeedTree:
		case GPUInstancerTreeType.SpeedTree8:
		{
			if (treeProxyParent == null)
			{
				treeProxyParent = new GameObject(_ED3E._E000(77582));
				if (treeProxyList != null)
				{
					treeProxyList.Clear();
				}
			}
			if (treeProxyList == null)
			{
				treeProxyList = new Dictionary<GameObject, Transform>();
			}
			else if (treeProxyList.ContainsKey(treePrototype.prefabObject))
			{
				if (!(treeProxyList[treePrototype.prefabObject] == null))
				{
					break;
				}
				treeProxyList.Remove(treePrototype.prefabObject);
			}
			Mesh mesh = new Mesh();
			mesh.name = _ED3E._E000(77622);
			GameObject gameObject = new GameObject(treeProxyList.Count + _ED3E._E000(48793) + treePrototype.name);
			gameObject.transform.SetParent(treeProxyParent.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			treeProxyList.Add(treePrototype.prefabObject, gameObject.transform);
			if (treePrototype.prefabObject.GetComponent<LODGroup>() != null)
			{
				LOD[] lODs = treePrototype.prefabObject.GetComponent<LODGroup>().GetLODs();
				for (int i = 0; i < lODs.Length; i++)
				{
					if (!lODs[i].renderers[0].GetComponent<BillboardRenderer>())
					{
						Material[] proxyMaterials = new Material[1]
						{
							new Material(Shader.Find(_E4BF.SHADER_GPUI_TREE_PROXY))
						};
						InstantiateTreeProxyObject(lODs[i].renderers[0].gameObject, gameObject, proxyMaterials, mesh, i == 0);
					}
				}
			}
			else
			{
				Material[] proxyMaterials2 = new Material[1]
				{
					new Material(Shader.Find(_E4BF.SHADER_GPUI_TREE_PROXY))
				};
				InstantiateTreeProxyObject(treePrototype.prefabObject, gameObject, proxyMaterials2, mesh, setBounds: true);
			}
			break;
		}
		case GPUInstancerTreeType.TreeCreatorTree:
			Shader.SetGlobalVector(GPUInstancerManager.m__E000, GetWindVector());
			break;
		}
	}

	public static void InstantiateTreeProxyObject(GameObject treePrefab, GameObject proxyObjectParent, Material[] proxyMaterials, Mesh proxyMesh, bool setBounds)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(treePrefab, proxyObjectParent.transform);
		gameObject.name = treePrefab.name;
		if (setBounds)
		{
			proxyMesh.bounds = gameObject.GetComponent<MeshFilter>().sharedMesh.bounds;
		}
		MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
		component.shadowCastingMode = ShadowCastingMode.Off;
		component.receiveShadows = false;
		component.lightProbeUsage = LightProbeUsage.Off;
		for (int i = 0; i < proxyMaterials.Length; i++)
		{
			proxyMaterials[i].CopyPropertiesFromMaterial(component.materials[i]);
			proxyMaterials[i].enableInstancing = true;
		}
		component.sharedMaterials = proxyMaterials;
		component.GetComponent<MeshFilter>().sharedMesh = proxyMesh;
		Component[] components = gameObject.GetComponents(typeof(Component));
		for (int j = 0; j < components.Length; j++)
		{
			if (!(components[j] is Transform) && !(components[j] is MeshFilter) && !(components[j] is MeshRenderer) && !(components[j] is Tree))
			{
				UnityEngine.Object.Destroy(components[j]);
			}
		}
	}

	public static Vector4 GetWindVector()
	{
		if (_windVector != Vector4.zero)
		{
			return _windVector;
		}
		UpdateSceneWind();
		return _windVector;
	}

	public static void UpdateSceneWind()
	{
		WindZone[] array = UnityEngine.Object.FindObjectsOfType<WindZone>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].mode == WindZoneMode.Directional)
			{
				_windVector = new Vector4(array[i].windTurbulence, array[i].windPulseMagnitude, array[i].windPulseFrequency, array[i].windMain);
				break;
			}
		}
	}

	public void LogThreadException()
	{
		Debug.LogException(threadException);
	}

	public _E4C2 GetRuntimeData(GPUInstancerPrototype prototype, bool logError = false)
	{
		_E4C2 value = null;
		if (!runtimeDataDictionary.TryGetValue(prototype, out value) && logError)
		{
			Debug.LogError(string.Concat(_ED3E._E000(77604), prototype, _ED3E._E000(77694)));
		}
		return value;
	}
}
