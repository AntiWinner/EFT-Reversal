using EFT.BlitDebug;
using Prism.Utils;
using UnityEngine;

[AddComponentMenu("PRISM/Prism Effects SSAO")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PrismSSAO : MonoBehaviour
{
	public PrismPreset currentPrismPreset;

	private bool m__E000;

	public bool isParentPrism;

	public bool isChildPrism;

	private RenderTexture m__E001;

	public Material m_Material;

	public Shader m_Shader;

	public Material m_Material2;

	public Shader m_Shader2;

	public Material m_AOMaterial;

	public Shader m_AOShader;

	public Material m_Material3;

	public Shader m_Shader3;

	private Camera m__E002;

	private SSAA m__E003;

	public Texture2D lensDirtTexture;

	public bool useLensDirt = true;

	public bool useAmbientObscurance;

	public SampleCount aoSampleCount = SampleCount.Low;

	public bool useAODistanceCutoff;

	public float aoDistanceCutoffLength = 50f;

	public float aoDistanceCutoffStart = 500f;

	public float aoIntensity = 0.7f;

	public float aoMinIntensity;

	public float aoRadius = 1f;

	public bool aoDownsample;

	public AOBlurType aoBlurType = AOBlurType.Fast;

	[Range(0f, 3f)]
	public int aoBlurIterations = 1;

	public float aoBias = 0.1f;

	public float aoBlurFilterDistance = 1.25f;

	public float aoLightingContribution = 1f;

	public bool aoShowDebug;

	[Space(10f)]
	public bool advancedVignette;

	public bool advancedAO;

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(32986));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(33004));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(33173));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(33235));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(32939));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(33519));

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(33585));

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(33317));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(33374));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(33357));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(39289));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(39278));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(39307));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(39354));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(39340));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(39332));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(39387));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(39373));

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(39420));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(39407));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(39442));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(39424));

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(39316));

	private RenderTextureFormat _E01B
	{
		get
		{
			if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8))
			{
				return RenderTextureFormat.R8;
			}
			return RenderTextureFormat.ARGB32;
		}
	}

	public int aoSampleCountValue
	{
		get
		{
			return aoSampleCount switch
			{
				SampleCount.Low => 10, 
				SampleCount.Medium => 14, 
				SampleCount.High => 18, 
				_ => Mathf.Clamp((int)aoSampleCount, 1, 256), 
			};
		}
		set
		{
			aoSampleCount = (SampleCount)value;
		}
	}

	public bool UsingTerrain
	{
		get
		{
			if ((bool)Terrain.activeTerrain)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsGBufferAvailable => this.m__E002.actualRenderingPath == RenderingPath.DeferredShading;

	public Camera GetPrismCamera()
	{
		if (this.m__E002 == null)
		{
			this.m__E002 = GetComponent<Camera>();
			this.m__E003 = GetComponent<SSAA>();
		}
		return this.m__E002;
	}

	public void SetPrismPreset(PrismPreset preset)
	{
		if (!preset)
		{
			useAmbientObscurance = true;
			return;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.AmbientObscurance)
		{
			useAmbientObscurance = preset.useAmbientObscurance;
			useAODistanceCutoff = preset.useAODistanceCutoff;
			aoIntensity = preset.aoIntensity;
			aoRadius = preset.aoRadius;
			aoDistanceCutoffStart = preset.aoDistanceCutoffStart;
			aoDownsample = preset.aoDownsample;
			aoBlurIterations = preset.aoBlurIterations;
			aoDistanceCutoffLength = preset.aoDistanceCutoffLength;
			aoSampleCount = preset.aoSampleCount;
			aoBias = preset.aoBias;
			aoBlurFilterDistance = preset.aoBlurFilterDistance;
			aoBlurType = preset.aoBlurType;
			aoLightingContribution = preset.aoLightingContribution;
		}
		Reset();
	}

	private void OnEnable()
	{
		this.m__E002 = GetComponent<Camera>();
		this.m__E003 = GetComponent<SSAA>();
		if (!m_Shader)
		{
			m_Shader = _E3AC.Find(_ED3E._E000(38274));
			if (!m_Shader)
			{
				Debug.LogError(_ED3E._E000(40852));
			}
		}
		if (!m_Shader2)
		{
			m_Shader2 = _E3AC.Find(_ED3E._E000(38318));
			if (!m_Shader2)
			{
				Debug.LogError(_ED3E._E000(39487));
			}
		}
		if (!m_Shader3)
		{
			m_Shader3 = _E3AC.Find(_ED3E._E000(38355));
			if (!m_Shader3)
			{
				Debug.LogError(_ED3E._E000(39543));
			}
		}
		if (!m_AOShader)
		{
			m_AOShader = _E3AC.Find(_ED3E._E000(38391));
			if (!m_AOShader)
			{
				Debug.LogError(_ED3E._E000(40922));
			}
		}
		this.m__E002.depthTextureMode |= DepthTextureMode.Depth;
		if (useAmbientObscurance && (!IsGBufferAvailable || UsingTerrain))
		{
			this.m__E002.depthTextureMode |= DepthTextureMode.Depth;
			this.m__E002.depthTextureMode |= DepthTextureMode.DepthNormals;
		}
	}

	[ContextMenu("DontRenderDepthTexture")]
	private void _E000()
	{
		this.m__E002.depthTextureMode = DepthTextureMode.None;
	}

	private void OnDestroy()
	{
	}

	private void OnDisable()
	{
		if ((bool)m_Material)
		{
			Object.DestroyImmediate(m_Material);
			m_Material = null;
		}
		if ((bool)m_Material2)
		{
			Object.DestroyImmediate(m_Material2);
			m_Material2 = null;
		}
		if ((bool)m_Material3)
		{
			Object.DestroyImmediate(m_Material3);
			m_Material3 = null;
		}
		if ((bool)m_AOMaterial)
		{
			Object.DestroyImmediate(m_AOMaterial);
			m_AOMaterial = null;
		}
		if (m_AOShader == m_Shader || m_Shader2 == m_Shader || m_Shader3 == m_Shader)
		{
			m_AOShader = null;
			m_Shader2 = null;
			m_Shader3 = null;
		}
	}

	private Material _E001(Shader shader)
	{
		if (!shader)
		{
			return null;
		}
		return new Material(shader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
	}

	public void Reset()
	{
		OnDisable();
		OnEnable();
	}

	private bool _E002()
	{
		if (m_Material == null && m_Shader != null && m_Shader.isSupported)
		{
			m_Material = _E001(m_Shader);
		}
		if (m_Material2 == null && m_Shader2 != null && m_Shader2.isSupported)
		{
			m_Material2 = _E001(m_Shader2);
		}
		if (m_Material3 == null && m_Shader3 != null && m_Shader3.isSupported)
		{
			m_Material3 = _E001(m_Shader3);
		}
		if (m_AOMaterial == null && m_AOShader != null && m_AOShader.isSupported)
		{
			m_AOMaterial = _E001(m_AOShader);
		}
		if (!m_Shader.isSupported)
		{
			Debug.LogError(_ED3E._E000(38939));
			base.enabled = false;
			return false;
		}
		if (!m_Shader2.isSupported)
		{
			Debug.LogError(_ED3E._E000(39625));
			base.enabled = false;
			return false;
		}
		if (!m_Shader3.isSupported)
		{
			Debug.LogError(_ED3E._E000(39680));
			m_Shader3 = m_Shader;
		}
		if (!m_AOShader.isSupported)
		{
			Debug.LogError(_ED3E._E000(39025));
			m_AOShader = m_Shader;
			useAmbientObscurance = false;
		}
		return true;
	}

	public void UpdateShaderValues()
	{
		if (m_Material == null)
		{
			return;
		}
		m_Material.shaderKeywords = null;
		m_Material2.shaderKeywords = null;
		m_AOMaterial.shaderKeywords = null;
		m_Material.SetFloat(_E004, 0f);
		m_Material.SetFloat(_E005, 0f);
		m_Material.SetFloat(_E006, 0f);
		m_Material.SetFloat(_E007, 0f);
		m_Material.SetFloat(_E008, 0f);
		Shader.DisableKeyword(_ED3E._E000(33827));
		m_Material.SetFloat(_E009, 0f);
		m_Material.SetFloat(_E00A, 0f);
		m_Material.DisableKeyword(_ED3E._E000(39155));
		m_Material.DisableKeyword(_ED3E._E000(39111));
		Shader.DisableKeyword(_ED3E._E000(34096));
		Shader.DisableKeyword(_ED3E._E000(34141));
		Shader.DisableKeyword(_ED3E._E000(34120));
		m_Material.SetFloat(_E00B, 0f);
		m_Material.SetFloat(_E00C, 0f);
		Shader.DisableKeyword(_ED3E._E000(34171));
		m_Material.SetFloat(_E00D, 0f);
		if (useAmbientObscurance)
		{
			if (!IsGBufferAvailable || UsingTerrain)
			{
				this.m__E002.depthTextureMode |= DepthTextureMode.Depth;
				this.m__E002.depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			m_Material.SetFloat(_E00E, aoIntensity);
			m_Material.SetFloat(_E00F, aoLightingContribution);
			if (useAODistanceCutoff)
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39198));
			}
			else
			{
				m_AOMaterial.DisableKeyword(_ED3E._E000(39198));
			}
			if (IsGBufferAvailable && !UsingTerrain)
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39187));
			}
			else
			{
				m_AOMaterial.DisableKeyword(_ED3E._E000(39187));
			}
			if (aoSampleCount == SampleCount.Low)
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39171));
				m_AOMaterial.DisableKeyword(_ED3E._E000(39209));
				m_AOMaterial.SetInt(_E01A, aoSampleCountValue);
			}
			else
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39209));
				m_AOMaterial.DisableKeyword(_ED3E._E000(39171));
				m_AOMaterial.SetInt(_E01A, aoSampleCountValue);
			}
			m_AOMaterial.SetInt(_E010, aoSampleCountValue);
		}
		else
		{
			m_Material.SetFloat(_E00E, aoMinIntensity);
		}
	}

	private void _E003(Material aoMaterial)
	{
		m_Material.SetFloat(_E00F, aoLightingContribution);
		aoMaterial.SetFloat(_E00E, aoIntensity);
		aoMaterial.SetFloat(_E011, aoRadius);
		aoMaterial.SetFloat(_E012, aoBias * 0.02f);
		aoMaterial.SetFloat(_E013, aoDownsample ? 0.5f : 1f);
		aoMaterial.SetFloat(_E014, aoDistanceCutoffStart);
		aoMaterial.SetFloat(_E015, aoDistanceCutoffLength);
		aoMaterial.SetMatrix(_E016, this.m__E002.cameraToWorldMatrix);
		int num = (this.m__E003 ? this.m__E003.GetInputWidth() : Screen.width);
		int num2 = (this.m__E003 ? this.m__E003.GetInputHeight() : Screen.height);
		Matrix4x4 projectionMatrix = this.m__E002.projectionMatrix;
		aoMaterial.SetVector(value: new Vector4(-2f / ((float)num * projectionMatrix[0]), -2f / ((float)num2 * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]), nameID: _E017);
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		bool flag = true;
		if (this.m__E000)
		{
			Graphics.CopyTexture(source, destination);
			this.m__E000 = false;
			return;
		}
		if (!_E002() || !flag)
		{
			Graphics.CopyTexture(source, destination);
			return;
		}
		UpdateShaderValues();
		int num = 1;
		if (aoDownsample)
		{
			num = 2;
		}
		int width = source.width / num;
		int height = source.height / num;
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, _E01B, RenderTextureReadWrite.Linear);
		temporary.name = _ED3E._E000(35922);
		_E003(m_AOMaterial);
		DebugGraphics.Blit(null, temporary, m_AOMaterial, 0);
		if (aoBlurType == AOBlurType.Fast)
		{
			for (int i = 0; i < aoBlurIterations; i++)
			{
				m_AOMaterial.SetVector(_E018, new Vector4(-1f, 0f, 0f, 0f));
				RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, _E01B, RenderTextureReadWrite.Linear);
				temporary2.name = _ED3E._E000(35911);
				DebugGraphics.Blit(temporary, temporary2, m_AOMaterial, (int)aoBlurType);
				RenderTexture.ReleaseTemporary(temporary);
				m_AOMaterial.SetVector(_E018, new Vector4(0f, 1f, 0f, 0f));
				temporary = RenderTexture.GetTemporary(width, height, 0, _E01B, RenderTextureReadWrite.Linear);
				temporary.name = _ED3E._E000(35922);
				DebugGraphics.Blit(temporary2, temporary, m_AOMaterial, (int)aoBlurType);
				RenderTexture.ReleaseTemporary(temporary2);
			}
		}
		else
		{
			for (int j = 0; j < aoBlurIterations; j++)
			{
				for (int k = 0; k < 2; k++)
				{
					m_AOMaterial.SetVector(_E018, new Vector4(-1f, 0f, 0f, 0f));
					RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, _E01B, RenderTextureReadWrite.Linear);
					temporary2.name = _ED3E._E000(35911);
					DebugGraphics.Blit(temporary, temporary2, m_AOMaterial, (int)(aoBlurType + k));
					RenderTexture.ReleaseTemporary(temporary);
					m_AOMaterial.SetVector(_E018, new Vector4(0f, 1f, 0f, 0f));
					temporary = RenderTexture.GetTemporary(width, height, 0, _E01B, RenderTextureReadWrite.Linear);
					temporary.name = _ED3E._E000(35922);
					DebugGraphics.Blit(temporary2, temporary, m_AOMaterial, (int)(aoBlurType + k));
					RenderTexture.ReleaseTemporary(temporary2);
				}
			}
		}
		if (aoShowDebug)
		{
			DebugGraphics.Blit(temporary, destination, m_AOMaterial, 2);
		}
		else
		{
			m_Material.SetTexture(_E019, temporary);
			if (isParentPrism)
			{
				Shader.SetGlobalFloat(_E00E, aoIntensity);
				Shader.SetGlobalTexture(_E019, temporary);
			}
			DebugGraphics.Blit(source, destination, m_Material, 0);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}
}
