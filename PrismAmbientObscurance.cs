using EFT.BlitDebug;
using Prism.Utils;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PrismAmbientObscurance : MonoBehaviour
{
	public Material m_Material;

	public Shader m_Shader;

	public Material m_AOMaterial;

	public Shader m_AOShader;

	private Camera m__E000;

	private SSAA m__E001;

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

	public bool advancedAO;

	private static readonly int m__E002 = Shader.PropertyToID(_ED3E._E000(39289));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(39278));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(39316));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(39307));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(39354));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(39340));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(39332));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(39387));

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(39373));

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(39420));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(39407));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(39395));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(39442));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(39424));

	private RenderTextureFormat _E010
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

	public bool UsingTerrain => Terrain.activeTerrain;

	public bool IsGBufferAvailable => this.m__E000.actualRenderingPath == RenderingPath.DeferredShading;

	public Camera GetPrismCamera()
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = GetComponent<Camera>();
			this.m__E001 = GetComponent<SSAA>();
		}
		return this.m__E000;
	}

	private void OnEnable()
	{
		this.m__E000 = GetComponent<Camera>();
		this.m__E001 = GetComponent<SSAA>();
		if (!m_Shader)
		{
			m_Shader = _E3AC.Find(_ED3E._E000(38300));
			if (!m_Shader)
			{
				Debug.LogError(_ED3E._E000(40852));
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
		if (useAmbientObscurance && (!IsGBufferAvailable || UsingTerrain))
		{
			this.m__E000.depthTextureMode |= DepthTextureMode.Depth;
			this.m__E000.depthTextureMode |= DepthTextureMode.DepthNormals;
		}
	}

	private void OnDisable()
	{
		if ((bool)m_Material)
		{
			Object.DestroyImmediate(m_Material);
			m_Material = null;
		}
		if ((bool)m_AOMaterial)
		{
			Object.DestroyImmediate(m_AOMaterial);
			m_AOMaterial = null;
		}
		if (m_AOShader == m_Shader)
		{
			m_AOShader = null;
		}
	}

	private Material _E000(Shader shader)
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

	private bool _E001()
	{
		if (m_Material == null && m_Shader != null && m_Shader.isSupported)
		{
			m_Material = _E000(m_Shader);
		}
		if (m_AOMaterial == null && m_AOShader != null && m_AOShader.isSupported)
		{
			m_AOMaterial = _E000(m_AOShader);
		}
		if (!m_Shader.isSupported)
		{
			Debug.LogError(_ED3E._E000(38939));
			base.enabled = false;
			return false;
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
		m_AOMaterial.shaderKeywords = null;
		if (this.m__E000.allowHDR)
		{
			m_Material.EnableKeyword(_ED3E._E000(39111));
			m_Material.DisableKeyword(_ED3E._E000(39155));
		}
		else
		{
			m_Material.EnableKeyword(_ED3E._E000(39155));
			m_Material.DisableKeyword(_ED3E._E000(39111));
		}
		if (useAmbientObscurance)
		{
			if (!IsGBufferAvailable || UsingTerrain)
			{
				this.m__E000.depthTextureMode |= DepthTextureMode.Depth;
				this.m__E000.depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			m_Material.SetFloat(PrismAmbientObscurance.m__E002, aoIntensity);
			m_Material.SetFloat(_E003, aoLightingContribution);
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
				m_AOMaterial.SetInt(_E004, aoSampleCountValue);
			}
			else
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39209));
				m_AOMaterial.DisableKeyword(_ED3E._E000(39171));
				m_AOMaterial.SetInt(_E004, aoSampleCountValue);
			}
			m_AOMaterial.SetInt(_E005, aoSampleCountValue);
		}
		else
		{
			m_Material.SetFloat(PrismAmbientObscurance.m__E002, aoMinIntensity);
		}
	}

	private void _E002(Material aoMaterial)
	{
		m_Material.SetFloat(_E003, aoLightingContribution);
		aoMaterial.SetFloat(PrismAmbientObscurance.m__E002, aoIntensity);
		aoMaterial.SetFloat(_E006, aoRadius);
		aoMaterial.SetFloat(_E007, aoBias * 0.02f);
		aoMaterial.SetFloat(_E008, aoDownsample ? 0.5f : 1f);
		aoMaterial.SetFloat(_E009, aoDistanceCutoffStart);
		aoMaterial.SetFloat(_E00A, aoDistanceCutoffLength);
		aoMaterial.SetMatrix(_E00B, this.m__E000.cameraToWorldMatrix);
		int num = (this.m__E001 ? this.m__E001.GetInputWidth() : Screen.width);
		int num2 = (this.m__E001 ? this.m__E001.GetInputHeight() : Screen.height);
		Matrix4x4 projectionMatrix = this.m__E000.projectionMatrix;
		aoMaterial.SetVector(value: new Vector4(-2f / ((float)num * projectionMatrix[0]), -2f / ((float)num2 * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]), nameID: _E00C);
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!useAmbientObscurance)
		{
			_E3A1.BlitOrCopy(source, destination);
			Shader.SetGlobalTexture(_E00D, Texture2D.blackTexture);
			return;
		}
		if (!_E001())
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
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, _E010, RenderTextureReadWrite.Linear);
		temporary.name = _ED3E._E000(39255);
		_E002(m_AOMaterial);
		DebugGraphics.Blit(null, temporary, m_AOMaterial, 0);
		if (aoBlurType == AOBlurType.Fast)
		{
			for (int i = 0; i < aoBlurIterations; i++)
			{
				m_AOMaterial.SetVector(_E00E, new Vector4(-1f, 0f, 0f, 0f));
				RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, _E010, RenderTextureReadWrite.Linear);
				temporary2.name = _ED3E._E000(39242);
				DebugGraphics.Blit(temporary, temporary2, m_AOMaterial, (int)aoBlurType);
				RenderTexture.ReleaseTemporary(temporary);
				m_AOMaterial.SetVector(_E00E, new Vector4(0f, 1f, 0f, 0f));
				temporary = RenderTexture.GetTemporary(width, height, 0, _E010, RenderTextureReadWrite.Linear);
				temporary.name = _ED3E._E000(39255);
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
					m_AOMaterial.SetVector(_E00E, new Vector4(-1f, 0f, 0f, 0f));
					RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, _E010, RenderTextureReadWrite.Linear);
					temporary2.name = _ED3E._E000(39242);
					DebugGraphics.Blit(temporary, temporary2, m_AOMaterial, (int)(aoBlurType + k));
					RenderTexture.ReleaseTemporary(temporary);
					m_AOMaterial.SetVector(_E00E, new Vector4(0f, 1f, 0f, 0f));
					temporary = RenderTexture.GetTemporary(width, height, 0, _E010, RenderTextureReadWrite.Linear);
					temporary.name = _ED3E._E000(39255);
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
			m_Material.SetTexture(_E00F, temporary);
			Shader.SetGlobalTexture(_E00D, temporary);
			DebugGraphics.Blit(source, destination, m_Material, 0);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}
}
