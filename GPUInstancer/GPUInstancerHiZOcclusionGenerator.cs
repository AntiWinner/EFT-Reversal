using System;
using EFT.BlitDebug;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

namespace GPUInstancer;

public class GPUInstancerHiZOcclusionGenerator : MonoBehaviour
{
	public bool debuggerEnabled;

	public bool debuggerGUIOnTop;

	[Range(0f, 0.1f)]
	public float debuggerOverlay;

	[Range(0f, 16f)]
	public int debuggerMipLevel;

	[Header("For info only, don't change:")]
	public RenderTexture hiZDepthTexture;

	public Texture unityDepthTexture;

	public Vector2 hiZTextureSize;

	[HideInInspector]
	public bool isVREnabled;

	private Camera m__E000;

	private SSAA m__E001;

	private int m__E002;

	private int m__E003;

	private bool m__E004;

	private GPUIOcclusionCullingType m__E005;

	private Material m__E006;

	private int m__E007;

	private RenderTexture[] m__E008;

	private RawImage m__E009;

	private GameObject _E00A;

	private bool _E00B;

	private float _E00C;

	private void Awake()
	{
		hiZTextureSize = Vector2.zero;
		_E4BF.SetupComputeTextureUtils();
		this.m__E005 = _E4BF.gpuiSettings.occlusionCullingType;
	}

	private void OnEnable()
	{
		if (_E4BF.gpuiSettings.isLWRP || _E4BF.gpuiSettings.isHDRP)
		{
			RenderPipelineManager.endCameraRendering += _E002;
		}
		else if (this.m__E005 == GPUIOcclusionCullingType.Default)
		{
			this.m__E006 = new Material(Shader.Find(_E4BF.SHADER_GPUI_HIZ_OCCLUSION_GENERATOR));
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(_E005));
		}
		else
		{
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(_E003));
		}
	}

	private void OnDisable()
	{
		if (_E4BF.gpuiSettings.isLWRP || _E4BF.gpuiSettings.isHDRP)
		{
			RenderPipelineManager.endCameraRendering -= _E002;
		}
		else if (this.m__E005 == GPUIOcclusionCullingType.Default)
		{
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(_E005));
		}
		else
		{
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(_E003));
		}
	}

	private void OnDestroy()
	{
		if (hiZDepthTexture != null)
		{
			hiZDepthTexture.Release();
			hiZDepthTexture = null;
		}
		if (this.m__E008 == null)
		{
			return;
		}
		for (int i = 0; i < this.m__E008.Length; i++)
		{
			if (this.m__E008[i] != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m__E008[i]);
			}
		}
		this.m__E008 = null;
	}

	public void Initialize(Camera occlusionCamera = null)
	{
		this.m__E004 = false;
		this.m__E000 = ((occlusionCamera != null) ? occlusionCamera : base.gameObject.GetComponent<Camera>());
		this.m__E001 = this.m__E000?.gameObject.GetComponent<SSAA>();
		if (this.m__E000 == null)
		{
			Debug.LogError(_ED3E._E000(115694));
			this.m__E004 = true;
			return;
		}
		if (XRSettings.enabled)
		{
			isVREnabled = true;
			_E4BF.gpuiSettings.vrRenderingMode = ((XRSettings.stereoRenderingMode != XRSettings.StereoRenderingMode.SinglePass) ? 1 : 0);
			if (isVREnabled && this.m__E006 != null)
			{
				if (_E4BF.gpuiSettings.vrRenderingMode == 1)
				{
					this.m__E006.EnableKeyword(_ED3E._E000(117852));
				}
				else
				{
					this.m__E006.EnableKeyword(_ED3E._E000(117833));
				}
				if (_E4BF.gpuiSettings.testBothEyesForVROcclusion)
				{
					this.m__E006.EnableKeyword(_ED3E._E000(117879));
				}
			}
			this.m__E002 = XRSettings.eyeTextureWidth;
			this.m__E003 = XRSettings.eyeTextureHeight;
			if (this.m__E000.stereoTargetEye != StereoTargetEyeMask.Both)
			{
				Debug.LogError(_ED3E._E000(117913));
				this.m__E004 = true;
				return;
			}
		}
		else
		{
			isVREnabled = false;
			this.m__E002 = _E000();
			this.m__E003 = _E001();
		}
		this.m__E000.depthTextureMode |= DepthTextureMode.Depth;
		_E006();
	}

	private int _E000()
	{
		int num = (this.m__E000 ? this.m__E000.pixelWidth : Screen.width);
		if (this.m__E001 != null)
		{
			num = ((this.m__E001.GetInputWidth() > 0) ? this.m__E001.GetInputWidth() : num);
		}
		return num;
	}

	private int _E001()
	{
		int num = (this.m__E000 ? this.m__E000.pixelHeight : Screen.height);
		if (this.m__E001 != null)
		{
			num = ((this.m__E001.GetInputHeight() > 0) ? this.m__E001.GetInputHeight() : num);
		}
		return num;
	}

	private void _E002(ScriptableRenderContext context, Camera camera)
	{
		_E003(camera);
	}

	private void _E003(Camera camera)
	{
		if (this.m__E004 || this.m__E000 == null || camera != this.m__E000)
		{
			return;
		}
		if (hiZDepthTexture == null)
		{
			Debug.LogWarning(_ED3E._E000(118011));
			_E006();
		}
		_E007();
		if (unityDepthTexture == null)
		{
			unityDepthTexture = Shader.GetGlobalTexture(_ED3E._E000(82547));
		}
		if (!(unityDepthTexture != null))
		{
			return;
		}
		if (isVREnabled && _E4BF.gpuiSettings.vrRenderingMode == 1)
		{
			if (this.m__E000.stereoActiveEye == Camera.MonoOrStereoscopicEye.Left)
			{
				_E004(0);
			}
			else if (_E4BF.gpuiSettings.testBothEyesForVROcclusion)
			{
				_E004((int)hiZTextureSize.x / 2);
			}
		}
		else
		{
			_E004(0);
		}
	}

	private void _E004(int offset)
	{
		_E4C8.CopyTextureWithComputeShader(unityDepthTexture, hiZDepthTexture, offset);
		for (int i = 0; i < this.m__E007 - 1; i++)
		{
			RenderTexture renderTexture = this.m__E008[i];
			if (i == 0)
			{
				_E4C8.ReduceTextureWithComputeShader(hiZDepthTexture, renderTexture, offset);
			}
			else
			{
				_E4C8.ReduceTextureWithComputeShader(this.m__E008[i - 1], renderTexture, offset);
			}
			_E4C8.CopyTextureWithComputeShader(renderTexture, hiZDepthTexture, offset, 0, i + 1, reverseZ: false);
		}
	}

	private void _E005(Camera camera)
	{
		if (this.m__E004 || this.m__E000 == null || camera != this.m__E000)
		{
			return;
		}
		if (hiZDepthTexture == null)
		{
			Debug.LogWarning(_ED3E._E000(118011));
			_E006();
		}
		_E007();
		DebugGraphics.Blit(null, hiZDepthTexture, this.m__E006, 0);
		for (int i = 0; i < this.m__E007; i++)
		{
			RenderTexture renderTexture = this.m__E008[i];
			if (i == 0)
			{
				DebugGraphics.Blit(hiZDepthTexture, renderTexture, this.m__E006, 1);
			}
			else
			{
				DebugGraphics.Blit(this.m__E008[i - 1], renderTexture, this.m__E006, 1);
			}
			Graphics.CopyTexture(renderTexture, 0, 0, hiZDepthTexture, 0, i + 1);
		}
		_E008();
	}

	private void _E006()
	{
		if (isVREnabled)
		{
			hiZTextureSize.x = XRSettings.eyeTextureWidth;
			hiZTextureSize.y = XRSettings.eyeTextureHeight;
			if (_E4BF.gpuiSettings.testBothEyesForVROcclusion)
			{
				hiZTextureSize.x *= 2f;
			}
		}
		else
		{
			hiZTextureSize.x = _E000();
			hiZTextureSize.y = _E001();
		}
		this.m__E007 = (int)Mathf.Floor(Mathf.Log(hiZTextureSize.x, 2f));
		if (hiZTextureSize.x <= 0f || hiZTextureSize.y <= 0f || this.m__E007 == 0)
		{
			if (hiZDepthTexture != null)
			{
				hiZDepthTexture.Release();
				UnityEngine.Object.DestroyImmediate(hiZDepthTexture);
			}
			Debug.LogError(_ED3E._E000(118065));
			return;
		}
		if (hiZDepthTexture != null)
		{
			hiZDepthTexture.Release();
			UnityEngine.Object.DestroyImmediate(hiZDepthTexture);
		}
		int num = (int)hiZTextureSize.x;
		int num2 = (int)hiZTextureSize.y;
		hiZDepthTexture = new RenderTexture(num, num2, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
		hiZDepthTexture.name = _ED3E._E000(118175);
		hiZDepthTexture.filterMode = FilterMode.Point;
		hiZDepthTexture.useMipMap = true;
		hiZDepthTexture.autoGenerateMips = false;
		hiZDepthTexture.enableRandomWrite = true;
		hiZDepthTexture.Create();
		hiZDepthTexture.hideFlags = HideFlags.HideAndDontSave;
		hiZDepthTexture.GenerateMips();
		if (this.m__E008 != null)
		{
			for (int i = 0; i < this.m__E008.Length; i++)
			{
				if (this.m__E008[i] != null)
				{
					this.m__E008[i].Release();
					UnityEngine.Object.DestroyImmediate(this.m__E008[i]);
				}
			}
		}
		this.m__E008 = new RenderTexture[this.m__E007];
		for (int j = 0; j < this.m__E007; j++)
		{
			num >>= 1;
			num2 >>= 1;
			if (num == 0)
			{
				num = 1;
			}
			if (num2 == 0)
			{
				num2 = 1;
			}
			this.m__E008[j] = new RenderTexture(num, num2, 0, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
			this.m__E008[j].name = _ED3E._E000(118155) + j;
			this.m__E008[j].filterMode = FilterMode.Point;
			this.m__E008[j].useMipMap = false;
			this.m__E008[j].autoGenerateMips = false;
			this.m__E008[j].enableRandomWrite = true;
			this.m__E008[j].Create();
			this.m__E008[j].hideFlags = HideFlags.HideAndDontSave;
		}
	}

	private void _E007()
	{
		if (!isVREnabled)
		{
			if (this.m__E002 != _E000() || this.m__E003 != _E001())
			{
				this.m__E002 = _E000();
				this.m__E003 = _E001();
				unityDepthTexture = null;
				_E006();
			}
		}
		else if (this.m__E002 != XRSettings.eyeTextureWidth || this.m__E003 != XRSettings.eyeTextureHeight)
		{
			this.m__E002 = XRSettings.eyeTextureWidth;
			this.m__E003 = XRSettings.eyeTextureHeight;
			_E006();
		}
	}

	private void _E008()
	{
	}

	private void _E009()
	{
		_E00A = new GameObject(_ED3E._E000(118188));
		_E00B = debuggerGUIOnTop;
		_E00C = debuggerOverlay;
		Canvas canvas = _E00A.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.pixelPerfect = true;
		canvas.sortingOrder = (debuggerGUIOnTop ? 10000 : (-10000));
		canvas.targetDisplay = 0;
		GameObject gameObject = new GameObject(_ED3E._E000(118216));
		gameObject.transform.parent = _E00A.transform;
		this.m__E009 = gameObject.AddComponent<RawImage>();
		this.m__E009.color = new Color(1f, 1f, 1f, 1f - debuggerOverlay);
		Vector2 vector = new Vector2(0f, 0f);
		this.m__E009.rectTransform.anchorMin = vector;
		this.m__E009.rectTransform.anchorMax = vector;
		this.m__E009.rectTransform.pivot = vector;
		this.m__E009.rectTransform.position = Vector2.zero;
	}
}
