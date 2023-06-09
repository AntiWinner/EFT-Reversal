using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class FogLight : LightOverride
{
	public enum TextureSize
	{
		x256 = 0x100,
		x512 = 0x200,
		x1024 = 0x400
	}

	private CommandBuffer _E005;

	private CommandBuffer _E006;

	private RenderTexture _E007;

	private ComputeBuffer _E008;

	public Shader m_BlurShadowmapShader;

	private Material _E009;

	public Shader m_CopyShadowParamsShader;

	private Material _E00A;

	private int[] _E00B;

	public bool m_ForceOnForFog;

	[Header("Shadows")]
	[Tooltip("Only one shadowed fog AreaLight at a time.")]
	public bool m_Shadows;

	[Tooltip("Always at most half the res of the AreaLight's shadowmap.")]
	public TextureSize m_ShadowmapRes = TextureSize.x256;

	[Range(0f, 3f)]
	public int m_BlurIterations;

	[_E3FB(0f)]
	public float m_BlurSize = 1f;

	[_E3FB(0f)]
	[Tooltip("Affects shadow softness.")]
	public float m_ESMExponent = 40f;

	public bool m_Bounded = true;

	private bool _E00C;

	private AreaLight _E00D;

	private bool _E000
	{
		get
		{
			if (m_Shadows)
			{
				return base.type == Type.Directional;
			}
			return false;
		}
	}

	private void _E000()
	{
		if (_E005 == null && this._E000)
		{
			Light component = GetComponent<Light>();
			_E005 = new CommandBuffer();
			_E005.name = _ED3E._E000(42571);
			component.AddCommandBuffer(LightEvent.AfterShadowMap, _E005);
			_E006 = new CommandBuffer();
			_E006.name = _ED3E._E000(42597);
			component.AddCommandBuffer(LightEvent.BeforeScreenspaceMask, _E006);
			_E009 = new Material(m_BlurShadowmapShader);
			_E009.hideFlags = HideFlags.HideAndDontSave;
			_E00A = new Material(m_CopyShadowParamsShader);
			_E00A.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	public void UpdateDirectionalShadowmap()
	{
		_E000();
		if (_E005 != null)
		{
			_E005.Clear();
		}
		if (_E006 != null)
		{
			_E006.Clear();
		}
		if (!this._E000)
		{
			return;
		}
		if (_E008 == null)
		{
			_E008 = new ComputeBuffer(1, 336);
		}
		Graphics.SetRandomWriteTarget(2, _E008);
		_E006.DrawProcedural(Matrix4x4.identity, _E00A, 0, MeshTopology.Points, 1);
		int num = 4096;
		int num2 = Mathf.Min((int)m_ShadowmapRes, num / 2);
		int num3 = (int)Mathf.Log(num / num2, 2f);
		RenderTargetIdentifier renderTargetIdentifier = BuiltinRenderTextureType.CurrentActive;
		_E005.SetShadowSamplingMode(renderTargetIdentifier, ShadowSamplingMode.RawDepth);
		RenderTextureFormat format = RenderTextureFormat.RGHalf;
		_E002(ref _E007);
		_E007 = RenderTexture.GetTemporary(num2, num2, 0, format, RenderTextureReadWrite.Linear);
		_E007.filterMode = FilterMode.Bilinear;
		_E007.wrapMode = TextureWrapMode.Clamp;
		if (_E00B == null || _E00B.Length != num3 - 1)
		{
			_E00B = new int[num3 - 1];
		}
		int i = 0;
		int num4 = num / 2;
		for (; i < num3; i++)
		{
			_E005.SetGlobalVector(_ED3E._E000(16827), new Vector4(0.5f / (float)num4, 0.5f / (float)num4, 0f, 0f));
			RenderTargetIdentifier dest;
			if (i < num3 - 1)
			{
				_E00B[i] = Shader.PropertyToID(_ED3E._E000(42627) + i);
				_E005.GetTemporaryRT(_E00B[i], num4, num4, 0, FilterMode.Bilinear, format, RenderTextureReadWrite.Linear);
				dest = new RenderTargetIdentifier(_E00B[i]);
			}
			else
			{
				dest = new RenderTargetIdentifier(_E007);
			}
			if (i == 0)
			{
				_E005.SetGlobalTexture(_ED3E._E000(42666), renderTargetIdentifier);
				_E005.Blit(null, dest, _E009, 4);
			}
			else
			{
				_E005.Blit(_E00B[i - 1], dest, _E009, 5);
			}
			num4 /= 2;
		}
	}

	private void _E001()
	{
		if (_E005 != null)
		{
			_E005.Clear();
		}
		if (_E006 != null)
		{
			_E006.Clear();
		}
		if (_E008 != null)
		{
			_E008.Release();
		}
		_E008 = null;
	}

	public bool SetUpDirectionalShadowmapForSampling(bool shadows, ComputeShader cs, int kernel)
	{
		if (!shadows || _E008 == null || _E007 == null)
		{
			cs.SetFloat(_ED3E._E000(42712), 0f);
			return false;
		}
		cs.SetFloat(_ED3E._E000(42712), 1f);
		cs.SetBuffer(kernel, _ED3E._E000(42697), _E008);
		cs.SetTexture(kernel, _ED3E._E000(42751), _E007);
		return true;
	}

	private void _E002(ref RenderTexture rt)
	{
		if (!(rt == null))
		{
			RenderTexture.ReleaseTemporary(rt);
			rt = null;
		}
	}

	public override bool GetForceOn()
	{
		return m_ForceOnForFog;
	}

	private void _E003()
	{
		_E00D = GetComponent<AreaLight>();
		if (_E00C && _E00D != null && _E00D.m_Negative)
		{
			LightManager<FogLight>.Remove(this);
			_E00C = false;
			_E001();
		}
		if (!_E00C && _E00D != null && !_E00D.m_Negative)
		{
			_E00C = LightManager<FogLight>.Add(this);
		}
	}

	private void OnEnable()
	{
		_E003();
	}

	private void Update()
	{
		_E003();
	}

	private void OnDisable()
	{
		LightManager<FogLight>.Remove(this);
		_E00C = false;
		_E001();
	}
}
