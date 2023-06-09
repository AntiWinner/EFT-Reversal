using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Light))]
public class VolumetricLight : MonoBehaviour
{
	private Light m__E000;

	private Shader m__E001;

	private Material m__E002;

	private CommandBuffer m__E003;

	private CommandBuffer m__E004;

	[Range(1f, 64f)]
	public int SampleCount = 8;

	[Range(0f, 1f)]
	public float ScatteringCoef = 0.5f;

	[Range(0f, 0.1f)]
	public float ExtinctionCoef = 0.01f;

	[Range(0f, 1f)]
	public float SkyboxExtinctionCoef = 0.9f;

	[Range(0f, 0.999f)]
	public float MieG = 0.1f;

	public bool HeightFog;

	[Range(0f, 0.5f)]
	public float HeightScale = 0.1f;

	public float GroundLevel;

	public bool Noise;

	public float NoiseScale = 0.015f;

	public float NoiseIntensity = 1f;

	public float NoiseIntensityOffset = 0.3f;

	public Vector2 NoiseVelocity = new Vector2(3f, 3f);

	[Tooltip("")]
	public float MaxRayLength = 400f;

	private Vector4[] m__E005 = new Vector4[4];

	private bool m__E006;

	private Vector3 m__E007;

	private Quaternion m__E008;

	private float m__E009;

	private float m__E00A;

	private Matrix4x4 m__E00B;

	private Matrix4x4 m__E00C;

	private Matrix4x4 m__E00D;

	private Matrix4x4 m__E00E;

	private Matrix4x4 m__E00F;

	private Matrix4x4 m__E010;

	private Matrix4x4 _E011;

	private Matrix4x4 _E012;

	private Vector3 _E013;

	private Vector3 _E014;

	private Mesh _E015;

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(82443));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(82488));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(82479));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(82466));

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(82520));

	private static readonly int _E01B = Shader.PropertyToID(_ED3E._E000(89552));

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(82505));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(82556));

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(82547));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(82591));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(47948));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(82574));

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(82622));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(82605));

	private static readonly int _E024 = Shader.PropertyToID(_ED3E._E000(19825));

	private static readonly int _E025 = Shader.PropertyToID(_ED3E._E000(82592));

	private static readonly int _E026 = Shader.PropertyToID(_ED3E._E000(82650));

	private static readonly int _E027 = Shader.PropertyToID(_ED3E._E000(82642));

	private static readonly int _E028 = Shader.PropertyToID(_ED3E._E000(82628));

	private static readonly int _E029 = Shader.PropertyToID(_ED3E._E000(82686));

	private static readonly int _E02A = Shader.PropertyToID(_ED3E._E000(82670));

	private static readonly int _E02B = Shader.PropertyToID(_ED3E._E000(47942));

	private static readonly int _E02C = Shader.PropertyToID(_ED3E._E000(82716));

	public Light Light => this.m__E000;

	public Material VolumetricMaterial => this.m__E002;

	private void Awake()
	{
		if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal || SystemInfo.graphicsDeviceType == GraphicsDeviceType.PlayStation4 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Vulkan || SystemInfo.graphicsDeviceType == GraphicsDeviceType.XboxOne)
		{
			this.m__E006 = true;
		}
		this.m__E000 = GetComponent<Light>();
		_E001();
		_E000();
		_E002();
		_E003();
		_E004();
		_E005();
	}

	private void OnEnable()
	{
		VolumetricLightRenderer.PreRenderEvent -= VolumetricLightRendererPreRenderEvent;
		VolumetricLightRenderer.PreRenderEvent += VolumetricLightRendererPreRenderEvent;
	}

	private void OnDisable()
	{
		VolumetricLightRenderer.PreRenderEvent -= VolumetricLightRendererPreRenderEvent;
	}

	public void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(this.m__E002);
	}

	private void _E000()
	{
		this.m__E003 = new CommandBuffer
		{
			name = _ED3E._E000(82247)
		};
		this.m__E004 = new CommandBuffer
		{
			name = _ED3E._E000(82284)
		};
		this.m__E004.SetGlobalTexture(_ED3E._E000(82325), new RenderTargetIdentifier(BuiltinRenderTextureType.CurrentActive));
		if (this.m__E000.type == LightType.Directional)
		{
			this.m__E000.AddCommandBuffer(LightEvent.BeforeScreenspaceMask, this.m__E003);
			this.m__E000.AddCommandBuffer(LightEvent.AfterShadowMap, this.m__E004);
		}
		else
		{
			this.m__E000.AddCommandBuffer(LightEvent.AfterShadowMap, this.m__E003);
		}
	}

	private void _E001()
	{
		if (this.m__E003 != null)
		{
			if (this.m__E000.type == LightType.Directional)
			{
				this.m__E000.RemoveCommandBuffer(LightEvent.BeforeScreenspaceMask, this.m__E003);
				this.m__E000.RemoveCommandBuffer(LightEvent.AfterShadowMap, this.m__E004);
			}
			else
			{
				this.m__E000.RemoveCommandBuffer(LightEvent.AfterShadowMap, this.m__E003);
			}
		}
	}

	private void _E002()
	{
		if (this.m__E001 == null)
		{
			this.m__E001 = _E3AC.Find(_ED3E._E000(37424));
		}
		this.m__E002 = new Material(this.m__E001);
	}

	private void _E003()
	{
		this.m__E002.SetInt(_E016, SampleCount);
		this.m__E002.SetVector(_E017, new Vector4(NoiseVelocity.x, NoiseVelocity.y) * NoiseScale);
		this.m__E002.SetVector(_E018, new Vector4(NoiseScale, NoiseIntensity, NoiseIntensityOffset));
		this.m__E002.SetVector(_E019, new Vector4(1f - MieG * MieG, 1f + MieG * MieG, 2f * MieG, 1f / (4f * (float)Math.PI)));
		this.m__E002.SetVector(_E01A, new Vector4(ScatteringCoef, ExtinctionCoef, this.m__E000.range, 1f - SkyboxExtinctionCoef));
		this.m__E002.SetFloat(_E01B, 8f);
		if (HeightFog)
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82366));
			this.m__E002.SetVector(_E01C, new Vector4(GroundLevel, HeightScale));
		}
		else
		{
			this.m__E002.DisableKeyword(_ED3E._E000(82366));
		}
	}

	private void _E004()
	{
		switch (this.m__E000.type)
		{
		case LightType.Point:
			_E006();
			break;
		case LightType.Spot:
			_E009();
			break;
		case LightType.Directional:
			_E00C();
			break;
		}
	}

	private void _E005()
	{
		switch (this.m__E000.type)
		{
		case LightType.Point:
			_E007();
			break;
		case LightType.Spot:
			_E00A();
			break;
		case LightType.Directional:
			_E00D();
			break;
		}
	}

	public void VolumetricLightRendererPreRenderEvent(VolumetricLightRenderer renderer, Matrix4x4 viewProj)
	{
		if (this.m__E003 == null)
		{
			Awake();
		}
		if (this.m__E000 == null || this.m__E000.gameObject == null)
		{
			VolumetricLightRenderer.PreRenderEvent -= VolumetricLightRendererPreRenderEvent;
		}
		else if (this.m__E000.enabled && !(this.m__E000.intensity < 0.001f))
		{
			this.m__E002.SetVector(_E01D, Camera.current.transform.forward);
			this.m__E002.SetTexture(_E01E, renderer.GetVolumeLightDepthBuffer());
			switch (this.m__E000.type)
			{
			case LightType.Point:
				_E008(renderer, viewProj);
				break;
			case LightType.Spot:
				_E00B(renderer, viewProj);
				break;
			case LightType.Directional:
				_E00E(renderer, viewProj);
				break;
			}
		}
	}

	private void _E006()
	{
		this.m__E009 = this.m__E000.range * 2f;
		if (Noise)
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82353));
		}
		else
		{
			this.m__E002.DisableKeyword(_ED3E._E000(82353));
		}
		if (this.m__E000.cookie == null)
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82351));
			this.m__E002.DisableKeyword(_ED3E._E000(82341));
		}
		else
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82341));
			this.m__E002.DisableKeyword(_ED3E._E000(82351));
			this.m__E002.SetTexture(_E01F, this.m__E000.cookie);
		}
	}

	private void _E007()
	{
		this.m__E00B = Matrix4x4.TRS(base.transform.position, this.m__E000.transform.rotation, new Vector3(this.m__E009, this.m__E009, this.m__E009));
		this.m__E002.SetVector(_E020, new Vector4(this.m__E000.transform.position.x, this.m__E000.transform.position.y, this.m__E000.transform.position.z, 1f / (this.m__E000.range * this.m__E000.range)));
		if (this.m__E000.cookie != null)
		{
			Matrix4x4 inverse = Matrix4x4.TRS(this.m__E000.transform.position, this.m__E000.transform.rotation, Vector3.one).inverse;
			this.m__E002.SetMatrix(_E021, inverse);
		}
	}

	private void _E008(VolumetricLightRenderer renderer, Matrix4x4 viewProj)
	{
		this.m__E003.Clear();
		int num = 0;
		if (!_E00F())
		{
			num = 2;
		}
		this.m__E002.SetPass(num);
		if (!this.m__E007.Equals(base.transform.position) || this.m__E008 != base.transform.rotation)
		{
			_E007();
			this.m__E007 = base.transform.position;
			this.m__E008 = base.transform.rotation;
		}
		_E015 = VolumetricLightRenderer.GetPointLightMesh();
		this.m__E002.SetMatrix(_E022, viewProj * this.m__E00B);
		this.m__E002.SetMatrix(_E023, Camera.current.worldToCameraMatrix * this.m__E00B);
		this.m__E002.SetColor(_E024, this.m__E000.color * this.m__E000.intensity);
		bool flag = (this.m__E000.transform.position - Camera.current.transform.position).magnitude >= QualitySettings.shadowDistance;
		if (this.m__E000.shadows != 0 && !flag)
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82394));
			this.m__E003.SetGlobalTexture(_ED3E._E000(82383), BuiltinRenderTextureType.CurrentActive);
			this.m__E003.SetRenderTarget(renderer.GetVolumeLightBuffer());
			this.m__E003.DrawMesh(_E015, this.m__E00B, this.m__E002, 0, num);
		}
		else
		{
			this.m__E002.DisableKeyword(_ED3E._E000(82394));
			renderer.GlobalCommandBuffer.DrawMesh(_E015, this.m__E00B, this.m__E002, 0, num);
		}
	}

	private void _E009()
	{
		this.m__E009 = this.m__E000.range;
		this.m__E00A = Mathf.Tan((this.m__E000.spotAngle + 1f) * 0.5f * ((float)Math.PI / 180f)) * this.m__E000.range;
		this.m__E00D = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0f), Quaternion.identity, new Vector3(-0.5f, -0.5f, 1f));
		this.m__E00F = Matrix4x4.Perspective(this.m__E000.spotAngle, 1f, 0f, 1f);
		this.m__E002.SetFloat(_E025, Mathf.Cos((this.m__E000.spotAngle + 1f) * 0.5f * ((float)Math.PI / 180f)));
		this.m__E002.EnableKeyword(_ED3E._E000(82425));
		if (Noise)
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82353));
		}
		else
		{
			this.m__E002.DisableKeyword(_ED3E._E000(82353));
		}
		if (this.m__E000.cookie == null)
		{
			this.m__E002.SetTexture(_E01F, VolumetricLightRenderer.GetDefaultSpotCookie());
		}
		else
		{
			this.m__E002.SetTexture(_E01F, this.m__E000.cookie);
		}
		if (this.m__E006)
		{
			this.m__E010 = Matrix4x4.Perspective(this.m__E000.spotAngle, 1f, this.m__E000.range, this.m__E000.shadowNearPlane);
		}
		else
		{
			this.m__E010 = Matrix4x4.Perspective(this.m__E000.spotAngle, 1f, this.m__E000.shadowNearPlane, this.m__E000.range);
		}
		this.m__E00E = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, new Vector3(0.5f, 0.5f, 0.5f));
		_E011 = this.m__E00E * this.m__E010;
		_E011[0, 2] *= -1f;
		_E011[1, 2] *= -1f;
		_E011[2, 2] *= -1f;
		_E011[3, 2] *= -1f;
	}

	private void _E00A()
	{
		this.m__E00B = Matrix4x4.TRS(base.transform.position, base.transform.rotation, new Vector3(this.m__E00A, this.m__E00A, this.m__E009));
		this.m__E00C = Matrix4x4.TRS(this.m__E000.transform.position, this.m__E000.transform.rotation, Vector3.one).inverse;
		_E012 = _E011 * this.m__E00C;
		this.m__E002.SetMatrix(_E021, this.m__E00D * this.m__E00F * this.m__E00C);
		this.m__E002.SetVector(_E020, new Vector4(this.m__E000.transform.position.x, this.m__E000.transform.position.y, this.m__E000.transform.position.z, 1f / (this.m__E000.range * this.m__E000.range)));
		_E013 = base.transform.position;
		_E014 = base.transform.forward;
		float value = 0f - Vector3.Dot(_E013 + _E014 * this.m__E000.range, _E014);
		this.m__E002.SetFloat(_E026, value);
		this.m__E002.SetVector(_E027, new Vector4(_E013.x, _E013.y, _E013.z));
		this.m__E002.SetVector(_E028, new Vector4(_E014.x, _E014.y, _E014.z));
	}

	private void _E00B(VolumetricLightRenderer renderer, Matrix4x4 viewProj)
	{
		this.m__E003.Clear();
		int shaderPass = 1;
		if (!_E010())
		{
			shaderPass = 3;
		}
		if (Math.Abs(Vector3.SqrMagnitude(this.m__E007 - base.transform.position)) > 0.01f || this.m__E008 != base.transform.rotation)
		{
			_E00A();
			this.m__E007 = base.transform.position;
			this.m__E008 = base.transform.rotation;
		}
		_E015 = VolumetricLightRenderer.GetSpotLightMesh();
		this.m__E002.SetMatrix(_E022, viewProj * this.m__E00B);
		this.m__E002.SetVector(_E024, this.m__E000.color * this.m__E000.intensity);
		bool flag = (this.m__E000.transform.position - Camera.current.transform.position).magnitude >= QualitySettings.shadowDistance;
		if (this.m__E000.shadows != 0 && !flag)
		{
			this.m__E002.SetMatrix(_E029, _E012);
			this.m__E002.SetMatrix(_E023, _E012);
			this.m__E002.EnableKeyword(_ED3E._E000(82422));
			this.m__E003.SetGlobalTexture(_ED3E._E000(82383), BuiltinRenderTextureType.CurrentActive);
			this.m__E003.SetRenderTarget(renderer.GetVolumeLightBuffer());
			this.m__E003.DrawMesh(_E015, this.m__E00B, this.m__E002, 0, shaderPass);
		}
		else
		{
			this.m__E002.DisableKeyword(_ED3E._E000(82422));
			renderer.GlobalCommandBuffer.DrawMesh(_E015, this.m__E00B, this.m__E002, 0, shaderPass);
		}
	}

	private void _E00C()
	{
		if (Noise)
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82353));
		}
		else
		{
			this.m__E002.DisableKeyword(_ED3E._E000(82353));
		}
		this.m__E002.SetFloat(_E02A, MaxRayLength);
		if (this.m__E000.cookie == null)
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82404));
			this.m__E002.DisableKeyword(_ED3E._E000(82456));
		}
		else
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82456));
			this.m__E002.DisableKeyword(_ED3E._E000(82404));
			this.m__E002.SetTexture(_E01F, this.m__E000.cookie);
		}
	}

	private void _E00D()
	{
		this.m__E002.SetVector(_E02B, new Vector4(this.m__E000.transform.forward.x, this.m__E000.transform.forward.y, this.m__E000.transform.forward.z, 1f / (this.m__E000.range * this.m__E000.range)));
	}

	private void _E00E(VolumetricLightRenderer renderer, Matrix4x4 viewProj)
	{
		this.m__E003.Clear();
		int pass = 4;
		this.m__E002.SetPass(pass);
		if (!this.m__E007.Equals(base.transform.position) || this.m__E008 != base.transform.rotation)
		{
			_E00D();
			this.m__E007 = base.transform.position;
			this.m__E008 = base.transform.rotation;
		}
		this.m__E002.SetVector(_E024, this.m__E000.color * this.m__E000.intensity);
		this.m__E005[0] = Camera.current.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.current.farClipPlane));
		this.m__E005[2] = Camera.current.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.current.farClipPlane));
		this.m__E005[3] = Camera.current.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.current.farClipPlane));
		this.m__E005[1] = Camera.current.ViewportToWorldPoint(new Vector3(1f, 0f, Camera.current.farClipPlane));
		this.m__E002.SetVectorArray(_E02C, this.m__E005);
		Texture source = null;
		if (this.m__E000.shadows != 0)
		{
			this.m__E002.EnableKeyword(_ED3E._E000(82422));
			this.m__E003.Blit(source, renderer.GetVolumeLightBuffer(), this.m__E002, pass);
		}
		else
		{
			this.m__E002.DisableKeyword(_ED3E._E000(82422));
			renderer.GlobalCommandBuffer.Blit(source, renderer.GetVolumeLightBuffer(), this.m__E002, pass);
		}
	}

	private bool _E00F()
	{
		float sqrMagnitude = (this.m__E000.transform.position - Camera.current.transform.position).sqrMagnitude;
		float num = this.m__E000.range + 1f;
		return sqrMagnitude < num * num;
	}

	private bool _E010()
	{
		float num = Vector3.Dot(this.m__E000.transform.forward, Camera.current.transform.position - this.m__E000.transform.position);
		float num2 = this.m__E000.range + 1f;
		if (num > num2)
		{
			return false;
		}
		if (Mathf.Acos(Vector3.Dot(base.transform.forward, (Camera.current.transform.position - this.m__E000.transform.position).normalized)) * 57.29578f > (this.m__E000.spotAngle + 3f) * 0.5f)
		{
			return false;
		}
		return true;
	}
}
