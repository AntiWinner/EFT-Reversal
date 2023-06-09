using System.Collections.Generic;
using EFT.BlitDebug;
using UnityEngine;
using UnityEngine.Rendering;

public class DistortCameraFX : MonoBehaviour
{
	private static readonly List<DistortRenderer> m__E000 = new List<DistortRenderer>();

	public Shader Shader;

	public float Distortion = 8f;

	[Range(1f, 4f)]
	public int BlurIterations = 1;

	private Material m__E001;

	private Camera m__E002;

	private SSAA m__E003;

	private CommandBuffer m__E004;

	private RenderTexture m__E005;

	private RenderTexture m__E006;

	private bool m__E007;

	private Vector2 _E008;

	private int _E009;

	private int _E00A;

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(16473));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(43307));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(43349));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(43338));

	private Material _E00F
	{
		get
		{
			if (this.m__E001 != null)
			{
				return this.m__E001;
			}
			return this.m__E001 = new Material(Shader);
		}
	}

	private void Awake()
	{
		_E002();
	}

	private void OnValidate()
	{
		_E006();
	}

	private void OnEnable()
	{
		if (this.m__E004 == null)
		{
			this.m__E004 = new CommandBuffer
			{
				name = _ED3E._E000(43217)
			};
		}
		this.m__E002.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m__E004);
		this.m__E002.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m__E004);
	}

	private void OnDisable()
	{
		if (this.m__E004 != null)
		{
			this.m__E002.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.m__E004);
		}
	}

	private void OnDestroy()
	{
		if (this.m__E005 != null)
		{
			Object.DestroyImmediate(this.m__E005);
		}
		if (this.m__E006 != null)
		{
			Object.DestroyImmediate(this.m__E006);
		}
	}

	private int _E000()
	{
		int num = (this.m__E002 ? this.m__E002.pixelWidth : Screen.width);
		if (this.m__E003 != null)
		{
			num = ((this.m__E003.GetInputWidth() > 0) ? this.m__E003.GetInputWidth() : num);
		}
		return num;
	}

	private int _E001()
	{
		int num = (this.m__E002 ? this.m__E002.pixelHeight : Screen.height);
		if (this.m__E003 != null)
		{
			num = ((this.m__E003.GetInputHeight() > 0) ? this.m__E003.GetInputHeight() : num);
		}
		return num;
	}

	private void Update()
	{
		if (!(this.m__E002 == null) && (_E001() != _E009 || _E000() != _E00A))
		{
			_E009 = _E001();
			_E00A = _E000();
			_E004();
		}
	}

	private void _E002()
	{
		this.m__E002 = GetComponent<Camera>();
		this.m__E003 = GetComponent<SSAA>();
		this.m__E001 = new Material(Shader);
		_E009 = _E001();
		_E00A = _E000();
		_E004();
		_E005();
		_E006();
	}

	private void OnPreCull()
	{
		if (!base.gameObject.activeInHierarchy || !base.enabled)
		{
			this.m__E007 = false;
			return;
		}
		this.m__E004.Clear();
		this.m__E007 = DistortCameraFX.m__E000.Count == 0;
		if (!this.m__E007)
		{
			this.m__E007 = !_E003(this.m__E004, this.m__E002);
		}
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (this.m__E007)
		{
			_E3A1.BlitOrCopy(src, dest);
			return;
		}
		_E007(src);
		DebugGraphics.Blit(src, dest, _E00F, 1);
	}

	private bool _E003(CommandBuffer buffer, Camera currentCamera)
	{
		buffer.SetRenderTarget(this.m__E005, BuiltinRenderTextureType.CurrentActive);
		buffer.ClearRenderTarget(clearDepth: false, clearColor: true, new Color(0.5f, 0.5f, 0f, 0f));
		Plane[] planes = _E379.CalculateFrustumPlanesNonAlloc(currentCamera);
		bool result = false;
		for (int i = 0; i < DistortCameraFX.m__E000.Count; i++)
		{
			if (GeometryUtility.TestPlanesAABB(planes, DistortCameraFX.m__E000[i].Renderer.bounds))
			{
				buffer.DrawRenderer(DistortCameraFX.m__E000[i].Renderer, DistortCameraFX.m__E000[i].MeterialToRender);
				result = true;
			}
		}
		_E00F.SetTexture(_E00C, this.m__E005);
		return result;
	}

	private void _E004()
	{
		if (this.m__E005 != null)
		{
			Object.DestroyImmediate(this.m__E005);
		}
		this.m__E005 = new RenderTexture(_E000(), _E001(), 0, RenderTextureFormat.ARGB32)
		{
			name = _ED3E._E000(43263)
		};
	}

	private void _E005()
	{
		if (!(this.m__E006 != null))
		{
			int width = _E000() >> 1;
			int height = _E001() >> 1;
			this.m__E006 = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32)
			{
				name = _ED3E._E000(43295)
			};
		}
	}

	private void _E006()
	{
		_E008 = new Vector2(Distortion / (float)_E000(), Distortion / (float)_E001());
		_E00F.SetVector(_E00B, _E008);
	}

	private void _E007(RenderTexture src)
	{
		int num = _E000() >> 1;
		int num2 = _E001() >> 1;
		RenderTexture temporary = RenderTexture.GetTemporary(num, num2, 0, RenderTextureFormat.ARGB32);
		temporary.name = _ED3E._E000(43324);
		Graphics.Blit(src, this.m__E006);
		for (int i = 0; i < BlurIterations; i++)
		{
			float num3 = 1 << i;
			_E00F.SetVector(_E00E, new Vector4(num3 / (float)num, 0f, 0f, 0f));
			DebugGraphics.Blit(this.m__E006, temporary, _E00F, 0);
			_E00F.SetVector(_E00E, new Vector4(0f, num3 / (float)num2, 0f, 0f));
			DebugGraphics.Blit(temporary, this.m__E006, _E00F, 0);
		}
		RenderTexture.ReleaseTemporary(temporary);
		_E00F.SetTexture(_E00D, this.m__E006);
	}

	public static void AddRenderer(DistortRenderer renderer)
	{
		if (!DistortCameraFX.m__E000.Contains(renderer))
		{
			DistortCameraFX.m__E000.Add(renderer);
		}
	}

	public static void RemoveRenderer(DistortRenderer renderer)
	{
		if (DistortCameraFX.m__E000.Contains(renderer))
		{
			DistortCameraFX.m__E000.Remove(renderer);
		}
	}
}
