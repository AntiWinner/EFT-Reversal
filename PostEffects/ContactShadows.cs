using UnityEngine;
using UnityEngine.Rendering;

namespace PostEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public sealed class ContactShadows : MonoBehaviour
{
	[SerializeField]
	private Light _light;

	[SerializeField]
	[Range(0f, 5f)]
	private float _rejectionDepth = 0.5f;

	[SerializeField]
	[Range(0f, 32f)]
	private int _sampleCount = 16;

	[SerializeField]
	[Range(0f, 1f)]
	private float _temporalFilter = 0.5f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _contrast = 0.5f;

	[SerializeField]
	private bool _downsample;

	[SerializeField]
	private NoiseTextureSet _noiseTextures;

	[HideInInspector]
	[SerializeField]
	private Shader _shader;

	private Material m__E000;

	private RenderTexture m__E001;

	private RenderTexture m__E002;

	private CommandBuffer m__E003;

	private CommandBuffer m__E004;

	private Matrix4x4 m__E005 = Matrix4x4.identity;

	private Camera _E006;

	private SSAA _E007;

	private int _E008;

	private int _E009;

	private int _E00A;

	private int _E00B;

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(70784));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(70832));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(82443));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(70826));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(70879));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(92674));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(86660));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(70860));

	private void Awake()
	{
		_E000();
		_E001();
	}

	private void OnValidate()
	{
		_E001();
	}

	private void _E000()
	{
		_E006 = GetComponent<Camera>();
		_E007 = GetComponent<SSAA>();
		if (_shader == null)
		{
			_shader = _E3AC.Find(_ED3E._E000(38234));
		}
		if (this.m__E000 == null)
		{
			this.m__E000 = new Material(_shader);
			this.m__E000.hideFlags = HideFlags.DontSave;
		}
		_E008 = Shader.PropertyToID(_ED3E._E000(70702));
		_E009 = Shader.PropertyToID(_ED3E._E000(70690));
		_E00A = Shader.PropertyToID(_ED3E._E000(70740));
		_E00B = Shader.PropertyToID(_ED3E._E000(70734));
	}

	private void _E001()
	{
		if (!(this.m__E000 == null))
		{
			this.m__E000.SetFloat(_E00C, _rejectionDepth);
			this.m__E000.SetFloat(_E00D, _contrast);
			this.m__E000.SetInt(_E00E, _sampleCount);
			float value = Mathf.Pow(1f - _temporalFilter, 2f);
			this.m__E000.SetFloat(_E00F, value);
		}
	}

	private void OnDestroy()
	{
		if (this.m__E000 != null)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(this.m__E000);
			}
			else
			{
				Object.DestroyImmediate(this.m__E000);
			}
		}
		if (this.m__E001 != null)
		{
			RenderTexture.ReleaseTemporary(this.m__E001);
		}
		if (this.m__E002 != null)
		{
			RenderTexture.ReleaseTemporary(this.m__E002);
		}
		if (this.m__E003 != null)
		{
			this.m__E003.Release();
		}
		if (this.m__E004 != null)
		{
			this.m__E004.Release();
		}
	}

	private void OnPreCull()
	{
		_E004();
		if (_light != null)
		{
			_E005();
			_light.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this.m__E003);
			_light.AddCommandBuffer(LightEvent.AfterScreenspaceMask, this.m__E004);
		}
	}

	private void OnPreRender()
	{
		if (_light != null)
		{
			_light.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, this.m__E003);
			_light.RemoveCommandBuffer(LightEvent.AfterScreenspaceMask, this.m__E004);
			this.m__E003.Clear();
			this.m__E004.Clear();
		}
	}

	private void Update()
	{
		_E006.depthTextureMode |= DepthTextureMode.Depth;
		if (!(_light != null) && !(TOD_Sky.Instance == null))
		{
			_light = TOD_Sky.Instance.Components.LightSource;
		}
	}

	private static Matrix4x4 _E002()
	{
		Camera current = Camera.current;
		Matrix4x4 nonJitteredProjectionMatrix = current.nonJitteredProjectionMatrix;
		Matrix4x4 worldToCameraMatrix = current.worldToCameraMatrix;
		return GL.GetGPUProjectionMatrix(nonJitteredProjectionMatrix, renderIntoTexture: true) * worldToCameraMatrix;
	}

	private Vector2 _E003()
	{
		Camera current = Camera.current;
		int num = ((!_downsample) ? 1 : 2);
		int num2 = (_E007 ? _E007.GetInputWidth() : current.pixelWidth);
		return new Vector2(y: (_E007 ? _E007.GetInputHeight() : current.pixelHeight) / num, x: num2 / num);
	}

	private void _E004()
	{
		if (this.m__E002 != null)
		{
			RenderTexture.ReleaseTemporary(this.m__E002);
			this.m__E002 = null;
		}
		if (!(_light == null))
		{
			if (this.m__E003 == null)
			{
				this.m__E003 = new CommandBuffer();
				this.m__E004 = new CommandBuffer();
				this.m__E003.name = _ED3E._E000(70782);
				this.m__E004.name = _ED3E._E000(70753);
			}
			else
			{
				this.m__E003.Clear();
				this.m__E004.Clear();
			}
			this.m__E000.SetVector(_E010, base.transform.InverseTransformDirection(-_light.transform.forward) * _light.shadowBias / ((float)_sampleCount - 1.5f));
			if (_noiseTextures != null)
			{
				Texture2D texture = _noiseTextures.GetTexture();
				Vector2 vector = _E003() / texture.width;
				this.m__E000.SetVector(_E011, vector);
				this.m__E000.SetTexture(_E012, texture);
			}
			this.m__E000.SetMatrix(_E013, this.m__E005 * base.transform.localToWorldMatrix);
			this.m__E005 = _E002();
		}
	}

	private void _E005()
	{
		Vector2 vector = _E003();
		RenderTextureFormat format = RenderTextureFormat.R8;
		RenderTexture temporary = RenderTexture.GetTemporary(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), 0, format);
		if (_temporalFilter == 0f)
		{
			this.m__E003.SetGlobalTexture(_E008, BuiltinRenderTextureType.CurrentActive);
			this.m__E003.SetRenderTarget(temporary);
			this.m__E003.DrawProcedural(Matrix4x4.identity, this.m__E000, 0, MeshTopology.Triangles, 3);
		}
		else
		{
			this.m__E003.SetGlobalTexture(_E008, BuiltinRenderTextureType.CurrentActive);
			this.m__E003.GetTemporaryRT(_E00B, Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), 0, FilterMode.Point, format);
			this.m__E003.SetRenderTarget(_E00B);
			this.m__E003.DrawProcedural(Matrix4x4.identity, this.m__E000, 0, MeshTopology.Triangles, 3);
			this.m__E003.SetGlobalTexture(_E009, this.m__E001);
			this.m__E003.SetRenderTarget(temporary);
			this.m__E003.DrawProcedural(Matrix4x4.identity, this.m__E000, 1 + (Time.frameCount & 1), MeshTopology.Triangles, 3);
		}
		if (_downsample)
		{
			this.m__E004.SetGlobalTexture(_E00A, temporary);
			this.m__E004.DrawProcedural(Matrix4x4.identity, this.m__E000, 3, MeshTopology.Triangles, 3);
		}
		else
		{
			this.m__E004.Blit(temporary, BuiltinRenderTextureType.CurrentActive);
		}
		this.m__E002 = this.m__E001;
		this.m__E001 = temporary;
	}
}
