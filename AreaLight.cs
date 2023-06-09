using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class AreaLight : BaseLight
{
	public enum TextureSize
	{
		x512 = 0x200,
		x1024 = 0x400,
		x2048 = 0x800,
		x4096 = 0x1000
	}

	public MeshRenderer ShadowCube;

	public MeshRenderer InvertedShadowCube;

	public MeshRenderer ShadowRendererToDraw;

	public Material ShadowRendererMaterial;

	public bool IsShadowRendererInverted;

	[Range(0f, 10f)]
	public float ShadowFeather;

	[Range(0f, 10f)]
	public float InvertedShadowFeather;

	[HideInInspector]
	public Mesh m_Cube;

	[HideInInspector]
	public Shader m_ProxyShader;

	private Material m__E000;

	private static Texture2D m__E001;

	private static Texture2D m__E002;

	private static Texture2D m__E003;

	private Vector3 m__E004;

	private Vector3 m__E005;

	private float m__E006;

	private Matrix4x4 m__E007;

	private Matrix4x4 m__E008;

	private Matrix4x4 m__E009;

	private Matrix4x4 m__E00A;

	private bool m__E00B;

	private bool m__E00C;

	private bool m__E00D;

	private int m__E00E;

	private bool m__E00F;

	private Dictionary<Camera, CommandBuffer> m__E010 = new Dictionary<Camera, CommandBuffer>();

	private Vector4[] m__E011 = new Vector4[6];

	private Vector4[] m__E012 = new Vector4[6];

	private Bounds m__E013;

	private Bounds m__E014;

	private static CameraEvent m__E015 = CameraEvent.AfterImageEffectsOpaque;

	private int m__E016;

	private int m__E017;

	private int m__E018;

	private int m__E019;

	private int m__E01A;

	private int m__E01B;

	private int m__E01C;

	private int m__E01D;

	private int m__E01E;

	private int m__E01F;

	private int _E020;

	private int _E021;

	private int _E022;

	private int _E023;

	private int _E024;

	private int _E025;

	private int _E026;

	private static readonly float[,] _E027 = new float[4, 2]
	{
		{ 1f, 1f },
		{ 1f, -1f },
		{ -1f, -1f },
		{ -1f, 1f }
	};

	private Camera _E028;

	private Transform _E029;

	[HideInInspector]
	public Shader m_ShadowmapShader;

	[HideInInspector]
	public Shader m_BlurShadowmapShader;

	private Material _E02A;

	private RenderTexture _E02B;

	private RenderTexture _E02C;

	private Texture2D _E02D;

	private int _E02E = -1;

	private int _E02F = -1;

	private FogLight _E030;

	private RenderTexture[] _E031;

	private static readonly int _E032 = Shader.PropertyToID(_ED3E._E000(16827));

	private static readonly int _E033 = Shader.PropertyToID(_ED3E._E000(42317));

	private static readonly int _E034 = Shader.PropertyToID(_ED3E._E000(42458));

	private static readonly int _E035 = Shader.PropertyToID(_ED3E._E000(42442));

	private static readonly int _E036 = Shader.PropertyToID(_ED3E._E000(42435));

	private static readonly int _E037 = Shader.PropertyToID(_ED3E._E000(19728));

	private static readonly int _E038 = Shader.PropertyToID(_ED3E._E000(42480));

	private static readonly int _E039 = Shader.PropertyToID(_ED3E._E000(42467));

	[_E3FB(0.001f)]
	public float size = 1f;

	[_E3FB(1f)]
	public float length = 1f;

	[_E3FB(0f)]
	public float depth = 2f;

	private Vector3 _E03A = new Vector3(1f, 1f, 2f);

	public Vector3 m_ClipBoxSize = new Vector3(2f, 2f, 4f);

	[Range(0f, 179f)]
	public float m_Angle;

	[Range(0f, 0.99f)]
	public float m_Hardness;

	[Range(0f, 2f)]
	public float m_SpecularScale = 1f;

	public Color m_Color = Color.white;

	[Space]
	public bool m_Ambient;

	public bool m_Negative;

	public bool m_Specular = true;

	[Space]
	public bool m_RenderSource = true;

	public Color m_SourceColor = Color.white;

	public Material SourceMaterial;

	[Space]
	public bool IsShadowCubeAnimated;

	[Header("Shadows")]
	public bool m_Spot;

	public bool m_Shadows;

	public LayerMask m_ShadowCullingMask = -1;

	public TextureSize m_ShadowmapRes = TextureSize.x2048;

	[_E3FB(0f)]
	public float m_ReceiverSearchDistance = 24f;

	[_E3FB(0f)]
	public float m_ReceiverDistanceScale = 5f;

	[_E3FB(0f)]
	public float m_LightNearSize = 4f;

	[_E3FB(0f)]
	public float m_LightFarSize = 22f;

	[Range(0f, 0.1f)]
	public float m_ShadowBias = 0.001f;

	private MeshRenderer _E03B;

	private Mesh _E03C;

	private Material[] _E03D;

	[HideInInspector]
	public Mesh m_Quad;

	private Vector2 _E03E = Vector2.zero;

	private Vector3 _E03F = Vector3.zero;

	private float _E040 = -1f;

	private bool _E041;

	private MaterialPropertyBlock _E042;

	private static Vector3[] _E043 = new Vector3[4];

	private static readonly int _E044 = Shader.PropertyToID(_ED3E._E000(40467));

	private bool _E000()
	{
		if (m_ProxyShader == null || m_Cube == null)
		{
			return false;
		}
		this.m__E000 = new Material(m_ProxyShader);
		this.m__E000.hideFlags = HideFlags.HideAndDontSave;
		return true;
	}

	private void _E001()
	{
		this.m__E000.SetTexture(_E020, AreaLight.m__E002);
		this.m__E000.SetTexture(_E021, AreaLight.m__E001);
		this.m__E000.SetTexture(_E022, AreaLight.m__E003);
	}

	private void _E002()
	{
		Dictionary<Camera, CommandBuffer>.Enumerator enumerator = this.m__E010.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<Camera, CommandBuffer> current = enumerator.Current;
			if (current.Key != null && current.Value != null)
			{
				current.Key.RemoveCommandBuffer(AreaLight.m__E015, current.Value);
			}
		}
		this.m__E010.Clear();
	}

	private CommandBuffer _E003(Camera cam)
	{
		if (cam == null)
		{
			return null;
		}
		CommandBuffer commandBuffer = null;
		if (!this.m__E010.ContainsKey(cam))
		{
			commandBuffer = new CommandBuffer();
			commandBuffer.name = base.gameObject.name;
			this.m__E010[cam] = commandBuffer;
			cam.AddCommandBuffer(AreaLight.m__E015, commandBuffer);
			cam.depthTextureMode |= DepthTextureMode.Depth;
		}
		else
		{
			commandBuffer = this.m__E010[cam];
			commandBuffer.Clear();
		}
		return commandBuffer;
	}

	public void SetUpCommandBuffer(Camera cam)
	{
		if (_E015())
		{
			return;
		}
		CommandBuffer commandBuffer = _E003(cam);
		_E004();
		_E006(commandBuffer, cam);
		if (m_Shadows)
		{
			_E011(commandBuffer);
		}
		_E007(commandBuffer);
		if (this.m__E00D)
		{
			commandBuffer.DrawRenderer(ShadowRendererToDraw, ShadowRendererMaterial);
		}
		else
		{
			commandBuffer.DrawMesh(m_Cube, this.m__E008, this.m__E000, 0, 0);
		}
		int num = this.m__E00E;
		if (this.m__E00D)
		{
			num += 2;
			if (IsShadowRendererInverted)
			{
				num += 2;
			}
		}
		commandBuffer.DrawMesh(m_Cube, this.m__E008, this.m__E000, 0, num);
	}

	private void _E004()
	{
		if (!this.m__E00F)
		{
			float z = 0.01f;
			Transform transform = base.transform;
			this.m__E004 = transform.position;
			this.m__E005 = transform.forward;
			this.m__E006 = 2f * Mathf.Atan(0.5f * m_Angle) / (float)Math.PI;
			this.m__E007 = default(Matrix4x4);
			for (int i = 0; i < 4; i++)
			{
				this.m__E007.SetRow(i, transform.TransformPoint(new Vector3(_E03A.x * _E027[i, 0], _E03A.y * _E027[i, 1], z) * 0.5f));
			}
			Bounds frustumBounds = GetFrustumBounds();
			Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 0f, frustumBounds.size.z * 0.5f), Quaternion.identity, frustumBounds.size);
			this.m__E008 = transform.localToWorldMatrix * matrix4x;
			Matrix4x4 matrix4x2 = Matrix4x4.Ortho(-0.5f * frustumBounds.size.x, 0.5f * frustumBounds.size.x, -0.5f * frustumBounds.size.y, 0.5f * frustumBounds.size.y, 0f, 0f - frustumBounds.size.z);
			this.m__E009 = matrix4x2 * base.transform.worldToLocalMatrix;
			this.m__E00A = GetProjectionMatrix();
			if (AreaLight.m__E002 == null)
			{
				AreaLight.m__E002 = _E3FA.LoadLUT(_E3FA.LUTType.TransformInv_DisneyDiffuse);
			}
			if (AreaLight.m__E001 == null)
			{
				AreaLight.m__E001 = _E3FA.LoadLUT(_E3FA.LUTType.TransformInv_GGX);
			}
			if (AreaLight.m__E003 == null)
			{
				AreaLight.m__E003 = _E3FA.LoadLUT(_E3FA.LUTType.AmpDiffAmpSpecFresnel);
			}
			this.m__E00B = ShadowCube != null && ShadowCube.gameObject.activeInHierarchy;
			this.m__E00C = InvertedShadowCube != null && InvertedShadowCube.gameObject.activeInHierarchy;
			this.m__E00D = ShadowRendererToDraw != null && ShadowRendererMaterial != null;
			this.m__E00E = ((!m_Negative && !m_Ambient && m_Shadows) ? 1 : 2);
			_E005();
			this.m__E00F = true;
		}
	}

	private void _E005()
	{
		this.m__E016 = Shader.PropertyToID(_ED3E._E000(47948));
		this.m__E017 = Shader.PropertyToID(_ED3E._E000(47942));
		this.m__E018 = Shader.PropertyToID(_ED3E._E000(19825));
		this.m__E019 = Shader.PropertyToID(_ED3E._E000(47992));
		this.m__E01A = Shader.PropertyToID(_ED3E._E000(47983));
		this.m__E01B = Shader.PropertyToID(_ED3E._E000(47969));
		_E020 = Shader.PropertyToID(_ED3E._E000(48026));
		_E021 = Shader.PropertyToID(_ED3E._E000(48000));
		_E022 = Shader.PropertyToID(_ED3E._E000(48047));
		this.m__E01C = Shader.PropertyToID(_ED3E._E000(48086));
		this.m__E01D = Shader.PropertyToID(_ED3E._E000(48075));
		this.m__E01E = Shader.PropertyToID(_ED3E._E000(48127));
		this.m__E01F = Shader.PropertyToID(_ED3E._E000(48103));
		_E023 = Shader.PropertyToID(_ED3E._E000(42009));
		_E024 = Shader.PropertyToID(_ED3E._E000(41989));
		_E025 = Shader.PropertyToID(_ED3E._E000(42043));
		_E026 = Shader.PropertyToID(_ED3E._E000(42079));
	}

	private void _E006(CommandBuffer buf, Camera cam)
	{
		if (m_Negative)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42053));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42053));
		}
		if (m_Ambient)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42110));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42110));
		}
		if (m_Spot)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42102));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42102));
		}
		if (m_Specular && !m_Negative && !m_Ambient)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42088));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42088));
		}
		if (false)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42081));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42081));
		}
		if (cam.CompareTag(_ED3E._E000(42129)))
		{
			buf.EnableShaderKeyword(_ED3E._E000(42117));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42117));
		}
		buf.SetGlobalVector(this.m__E016, this.m__E004);
		buf.SetGlobalVector(this.m__E017, this.m__E005);
		buf.SetGlobalVector(this.m__E018, _E00C());
		buf.SetGlobalFloat(this.m__E019, m_SpecularScale);
		buf.SetGlobalFloat(this.m__E01A, m_Hardness);
		buf.SetGlobalFloat(this.m__E01B, this.m__E006);
		_E001();
		buf.SetGlobalFloat(this.m__E01C, 0f);
		buf.SetGlobalMatrix(this.m__E01D, this.m__E007);
		buf.SetGlobalMatrix(this.m__E01E, this.m__E00A);
		buf.SetGlobalMatrix(this.m__E01F, this.m__E009);
	}

	private void _E007(CommandBuffer buf)
	{
		if (IsShadowCubeAnimated)
		{
			_E008(ShadowCube, this.m__E011, ref this.m__E013);
		}
		if (this.m__E00B)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42169));
			buf.SetGlobalFloat(_E023, ShadowFeather);
			buf.SetGlobalVectorArray(_E024, this.m__E011);
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42169));
		}
		if (this.m__E00C)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42154));
			buf.SetGlobalFloat(_E025, InvertedShadowFeather);
			buf.SetGlobalVectorArray(_E026, this.m__E012);
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42154));
		}
	}

	private void _E008(MeshRenderer cube, Vector4[] planes, ref Bounds bounds)
	{
		if (!(cube == null) && !(cube.bounds == bounds))
		{
			_E009(cube, planes);
			bounds = cube.bounds;
		}
	}

	private void _E009(MeshRenderer cube, Vector4[] planes)
	{
		if (!(cube == null))
		{
			Transform transform = cube.transform;
			Vector3 position = transform.position;
			Vector3 rhs = -transform.forward;
			planes[0] = new Vector4(w: Vector3.Dot(position + -transform.forward * transform.lossyScale.z / 2f, rhs), x: rhs.x, y: rhs.y, z: rhs.z);
			rhs = transform.forward;
			planes[1] = new Vector4(w: Vector3.Dot(position + transform.forward * transform.lossyScale.z / 2f, rhs), x: rhs.x, y: rhs.y, z: rhs.z);
			rhs = -Vector3.Cross(transform.forward, transform.up);
			planes[2] = new Vector4(w: Vector3.Dot(position + rhs * transform.lossyScale.x / 2f, rhs), x: rhs.x, y: rhs.y, z: rhs.z);
			rhs = Vector3.Cross(transform.forward, transform.up);
			planes[3] = new Vector4(w: Vector3.Dot(position + rhs * transform.lossyScale.x / 2f, rhs), x: rhs.x, y: rhs.y, z: rhs.z);
			rhs = -transform.up;
			planes[4] = new Vector4(w: Vector3.Dot(position + rhs * transform.lossyScale.y / 2f, rhs), x: rhs.x, y: rhs.y, z: rhs.z);
			rhs = transform.up;
			float w6 = Vector3.Dot(position + rhs * transform.lossyScale.y / 2f, rhs);
			planes[5] = new Vector4(rhs.x, rhs.y, rhs.z, w6);
		}
	}

	private void _E00A(string keyword, bool on)
	{
		if (on)
		{
			this.m__E000.EnableKeyword(keyword);
		}
		else
		{
			this.m__E000.DisableKeyword(keyword);
		}
	}

	private void _E00B(ref RenderTexture rt)
	{
		if (!(rt == null))
		{
			RenderTexture.ReleaseTemporary(rt);
			rt = null;
		}
	}

	private Color _E00C()
	{
		if (QualitySettings.activeColorSpace == ColorSpace.Gamma)
		{
			return m_Color * m_Intensity;
		}
		return new Color(Mathf.GammaToLinearSpace(m_Color.r * m_Intensity), Mathf.GammaToLinearSpace(m_Color.g * m_Intensity), Mathf.GammaToLinearSpace(m_Color.b * m_Intensity), 1f);
	}

	private void _E00D(int res)
	{
		if (_E02B != null && _E02E == Time.renderedFrameCount)
		{
			return;
		}
		if (_E028 == null)
		{
			if (m_ShadowmapShader == null)
			{
				Debug.LogError(_ED3E._E000(42195), this);
				return;
			}
			GameObject gameObject = new GameObject(_ED3E._E000(42214));
			gameObject.AddComponent(typeof(Camera));
			_E028 = gameObject.GetComponent<Camera>();
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			_E028.enabled = false;
			_E028.clearFlags = CameraClearFlags.Color;
			_E028.renderingPath = RenderingPath.Forward;
			_E028.backgroundColor = Color.white;
			_E029 = gameObject.transform;
			_E029.parent = base.transform;
			_E029.localRotation = Quaternion.identity;
		}
		if (m_Angle == 0f)
		{
			_E028.orthographic = true;
			_E029.localPosition = Vector3.zero;
			_E028.nearClipPlane = 0.05f;
			_E028.farClipPlane = _E03A.z;
			_E028.orthographicSize = 0.5f * _E03A.y;
			_E028.aspect = _E03A.x / _E03A.y;
		}
		else
		{
			_E028.orthographic = false;
			float num = _E01D();
			_E029.localPosition = (0f - num) * Vector3.forward;
			_E028.nearClipPlane = num;
			_E028.farClipPlane = num + _E03A.z;
			_E028.fieldOfView = m_Angle;
			_E028.aspect = _E03A.x / _E03A.y;
		}
		_E00B(ref _E02B);
		_E02B = RenderTexture.GetTemporary(res, res, 24, RenderTextureFormat.Shadowmap);
		_E02B.name = _ED3E._E000(42263);
		_E02B.filterMode = FilterMode.Bilinear;
		_E02B.wrapMode = TextureWrapMode.Clamp;
		_E028.targetTexture = _E02B;
		_E028.cullingMask = 0;
		_E028.Render();
		_E028.cullingMask = m_ShadowCullingMask;
		bool invertCulling = GL.invertCulling;
		GL.invertCulling = false;
		_E028.RenderWithShader(m_ShadowmapShader, _ED3E._E000(18067));
		GL.invertCulling = invertCulling;
		_E02E = Time.renderedFrameCount;
	}

	public RenderTexture GetBlurredShadowmap()
	{
		_E00E();
		return _E02C;
	}

	private void _E00E()
	{
		if (_E02C != null && _E02F == Time.renderedFrameCount)
		{
			return;
		}
		_E014();
		int num = (int)m_ShadowmapRes;
		int num2 = (int)_E030.m_ShadowmapRes;
		if (base.isActiveAndEnabled && m_Shadows)
		{
			num2 = Mathf.Min(num2, num / 2);
		}
		else
		{
			num = 2 * num;
		}
		_E00D(num);
		RenderTexture active = RenderTexture.active;
		_E00B(ref _E02C);
		_E012(ref _E02A, m_BlurShadowmapShader);
		int num3 = (int)Mathf.Log(num / num2, 2f);
		if (_E031 == null || _E031.Length != num3)
		{
			_E031 = new RenderTexture[num3];
		}
		RenderTextureFormat format = RenderTextureFormat.RGHalf;
		int i = 0;
		int num4 = num / 2;
		for (; i < num3; i++)
		{
			_E031[i] = RenderTexture.GetTemporary(num4, num4, 0, format, RenderTextureReadWrite.Linear);
			_E031[i].name = _ED3E._E000(42243);
			_E031[i].filterMode = FilterMode.Bilinear;
			_E031[i].wrapMode = TextureWrapMode.Clamp;
			_E02A.SetVector(_E032, new Vector4(0.5f / (float)num4, 0.5f / (float)num4, 0f, 0f));
			if (i == 0)
			{
				_E02A.SetTexture(_E033, _E02B);
				_E013();
				_E02A.SetTexture(_E034, _E02D);
				_E02A.SetVector(_E035, _E016());
				_E02A.SetFloat(_E036, _E030.m_ESMExponent);
				_E00F(_E02B, _E031[i], 0);
			}
			else
			{
				_E02A.SetTexture(_E037, _E031[i - 1]);
				_E00F(_E031[i - 1], _E031[i], 1);
			}
			num4 /= 2;
		}
		for (int j = 0; j < num3 - 1; j++)
		{
			RenderTexture.ReleaseTemporary(_E031[j]);
		}
		_E02C = _E031[num3 - 1];
		if (_E030.m_BlurIterations > 0)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(num2, num2, 0, format, RenderTextureReadWrite.Linear);
			temporary.name = _ED3E._E000(42279);
			temporary.filterMode = FilterMode.Bilinear;
			temporary.wrapMode = TextureWrapMode.Clamp;
			_E02A.SetVector(_E038, new Vector4(1f / (float)num2, 1f / (float)num2, 0f, 0f));
			float num5 = _E030.m_BlurSize;
			for (int k = 0; k < _E030.m_BlurIterations; k++)
			{
				_E02A.SetFloat(_E039, num5);
				_E00F(_E02C, temporary, 2);
				_E00F(temporary, _E02C, 3);
				num5 *= 1.2f;
			}
			RenderTexture.ReleaseTemporary(temporary);
		}
		RenderTexture.active = active;
		_E02F = Time.renderedFrameCount;
	}

	private void _E00F(RenderTexture src, RenderTexture dst, int pass)
	{
		RenderTexture.active = dst;
		_E02A.SetTexture(_E037, src);
		_E02A.SetPass(pass);
		_E010();
	}

	private void _E010()
	{
		GL.Begin(7);
		GL.TexCoord2(0f, 0f);
		GL.Vertex3(-1f, 1f, 0f);
		GL.TexCoord2(0f, 1f);
		GL.Vertex3(-1f, -1f, 0f);
		GL.TexCoord2(1f, 1f);
		GL.Vertex3(1f, -1f, 0f);
		GL.TexCoord2(1f, 0f);
		GL.Vertex3(1f, 1f, 0f);
		GL.End();
	}

	private void _E011(CommandBuffer buf)
	{
		_E00D((int)m_ShadowmapRes);
		buf.SetGlobalTexture(_ED3E._E000(42317), _E02B);
		_E013();
		this.m__E000.SetTexture(_E034, _E02D);
		float num = (float)m_ShadowmapRes;
		float num2 = num / 2048f;
		buf.SetGlobalFloat(_ED3E._E000(42304), num2 * m_ReceiverSearchDistance / num);
		buf.SetGlobalFloat(_ED3E._E000(42349), m_ReceiverDistanceScale * 0.5f / 10f);
		Vector2 vector = new Vector2(m_LightNearSize, m_LightFarSize) * num2 / num;
		buf.SetGlobalVector(_ED3E._E000(42386), vector);
		buf.SetGlobalFloat(_ED3E._E000(42428), m_ShadowBias);
	}

	private void _E012(ref Material material, Shader shader)
	{
		if (!material)
		{
			if (!shader)
			{
				Debug.LogError(_ED3E._E000(42416));
				return;
			}
			material = new Material(shader);
			material.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	private void _E013()
	{
		if (!(_E02D != null))
		{
			_E02D = new Texture2D(1, 1, TextureFormat.Alpha8, mipChain: false, linear: true);
			_E02D.filterMode = FilterMode.Point;
			_E02D.SetPixel(0, 0, new Color(0f, 0f, 0f, 0f));
			_E02D.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		}
	}

	private void _E014()
	{
		if (!(_E030 != null))
		{
			_E030 = GetComponent<FogLight>();
		}
	}

	private bool _E015()
	{
		if (Camera.current == null)
		{
			return false;
		}
		RenderTexture targetTexture = Camera.current.targetTexture;
		if (targetTexture != null)
		{
			return targetTexture.format == RenderTextureFormat.Shadowmap;
		}
		return false;
	}

	private Vector4 _E016()
	{
		float num = _E01D();
		float num2 = num + _E03A.z;
		return new Vector4(num / (num - num2), (num + num2) / (num - num2), 0f, 0f);
	}

	private void Awake()
	{
		if (_E017())
		{
			_E01C();
			_E018(forceUpdate: true);
			_E01B();
		}
	}

	private bool _E017()
	{
		if (_E041)
		{
			return true;
		}
		if (m_Quad == null || !_E000())
		{
			return false;
		}
		_E03B = GetComponent<MeshRenderer>();
		_E03B.enabled = true;
		_E03C = UnityEngine.Object.Instantiate(m_Quad);
		_E03C.hideFlags = HideFlags.HideAndDontSave;
		base.gameObject.GetComponent<MeshFilter>().sharedMesh = _E03C;
		_E03D = _E03B.sharedMaterials;
		Transform transform = base.transform;
		if (transform.localScale != Vector3.one)
		{
			transform.localScale = Vector3.one;
		}
		_E01A();
		_E009(ShadowCube, this.m__E011);
		_E009(InvertedShadowCube, this.m__E012);
		_E041 = true;
		return true;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (_E017())
		{
			_E018(forceUpdate: true);
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E019));
			if (_E03B == null)
			{
				_E03B = GetComponent<MeshRenderer>();
			}
			_E03B.enabled = true;
			_E03D = _E03B.sharedMaterials;
			_E01B();
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_E002();
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E019));
		if (_E03B == null)
		{
			_E03B = GetComponent<MeshRenderer>();
		}
		_E03B.enabled = false;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m__E000 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.m__E000);
		}
		if (_E03C != null)
		{
			UnityEngine.Object.DestroyImmediate(_E03C);
		}
		_E002();
	}

	private void _E018(bool forceUpdate = false)
	{
		if (!_E03A.Equals(_E03F) || !(Math.Abs(m_Angle - _E040) < 0.01f) || forceUpdate)
		{
			_E03A.x = Mathf.Max(_E03A.x, 0.001f);
			_E03A.y = Mathf.Max(_E03A.y, 0.001f);
			_E03A.z = Mathf.Max(_E03A.z, 0f);
			Vector2 vector = ((m_RenderSource && base.enabled) ? new Vector2(_E03A.x, _E03A.y) : (Vector2.one * 0.0001f));
			if (vector != _E03E)
			{
				float num = vector.x * 0.5f;
				float num2 = vector.y * 0.5f;
				float newZ = -0.001f;
				_E043[0].Set(0f - num, num2, newZ);
				_E043[1].Set(num, 0f - num2, newZ);
				_E043[2].Set(num, num2, newZ);
				_E043[3].Set(0f - num, 0f - num2, newZ);
				_E03C.vertices = _E043;
				_E03E = vector;
			}
			_E03C.bounds = GetFrustumBounds();
			_E03F = _E03A;
			_E040 = m_Angle;
		}
	}

	public override void ManualUpdate(float dt)
	{
		if (m_Intensity <= 0f)
		{
			if (this.m__E010.Count > 0)
			{
				_E002();
				_E03B.enabled = false;
			}
		}
		else if (!base.gameObject.activeInHierarchy || !base.enabled)
		{
			_E002();
		}
		else if (_E017())
		{
			_E018();
			_E03B.enabled = true;
			_E01B();
		}
	}

	private void _E019(Camera cam)
	{
		if (!(m_Intensity <= 0f) && _E017() && (cam.CompareTag(_ED3E._E000(42407)) || cam.CompareTag(_ED3E._E000(42129))))
		{
			SetUpCommandBuffer(cam);
		}
	}

	private void _E01A()
	{
		_E03A = new Vector3(length * size, size, depth);
		m_ClipBoxSize.x = ((m_ClipBoxSize.x < _E03A.x) ? _E03A.x : m_ClipBoxSize.x);
		m_ClipBoxSize.y = ((m_ClipBoxSize.y < _E03A.y) ? _E03A.y : m_ClipBoxSize.y);
		m_ClipBoxSize.z = ((m_ClipBoxSize.z < _E03A.z) ? _E03A.z : m_ClipBoxSize.z);
	}

	private void _E01B()
	{
		if (m_RenderSource)
		{
			_E01C();
			if (_E03D.Length == 0)
			{
				_E03B.sharedMaterial = SourceMaterial;
				_E03D = _E03B.sharedMaterials;
			}
		}
		else if (_E03D.Length != 0)
		{
			_E03B.sharedMaterials = new Material[0];
			_E03D = _E03B.sharedMaterials;
		}
	}

	private void _E01C()
	{
		Color color = new Color(Mathf.GammaToLinearSpace(m_SourceColor.r), Mathf.GammaToLinearSpace(m_SourceColor.g), Mathf.GammaToLinearSpace(m_SourceColor.b), 1f);
		if (_E042 == null)
		{
			_E042 = new MaterialPropertyBlock();
		}
		_E042.SetVector(_E044, color * m_Intensity);
		_E03B.SetPropertyBlock(_E042);
	}

	private float _E01D()
	{
		if (m_Angle == 0f)
		{
			return 0f;
		}
		return _E03A.y * 0.5f / Mathf.Tan(m_Angle * 0.5f * ((float)Math.PI / 180f));
	}

	private Matrix4x4 _E01E(float zOffset)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetColumn(3, new Vector4(0f, 0f, zOffset, 1f));
		return identity;
	}

	public Matrix4x4 GetProjectionMatrix(bool linearZ = false)
	{
		Matrix4x4 matrix4x;
		if (m_Angle == 0f)
		{
			matrix4x = Matrix4x4.Ortho(-0.5f * _E03A.x, 0.5f * _E03A.x, -0.5f * _E03A.y, 0.5f * _E03A.y, 0f, 0f - _E03A.z);
		}
		else
		{
			float num = _E01D();
			if (linearZ)
			{
				matrix4x = _E01F(m_Angle, _E03A.x / _E03A.y, num, num + _E03A.z);
			}
			else
			{
				matrix4x = Matrix4x4.Perspective(m_Angle, _E03A.x / _E03A.y, num, num + _E03A.z);
				matrix4x *= Matrix4x4.Scale(new Vector3(1f, 1f, -1f));
			}
			matrix4x *= _E01E(num);
		}
		return matrix4x * base.transform.worldToLocalMatrix;
	}

	public Vector4 MultiplyPoint(Matrix4x4 m, Vector3 v)
	{
		Vector4 result = default(Vector4);
		result.x = m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03;
		result.y = m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13;
		result.z = m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23;
		result.w = m.m30 * v.x + m.m31 * v.y + m.m32 * v.z + m.m33;
		return result;
	}

	private Matrix4x4 _E01F(float fov, float aspect, float near, float far)
	{
		float f = (float)Math.PI / 180f * fov * 0.5f;
		float num = Mathf.Cos(f) / Mathf.Sin(f);
		float num2 = 1f / (far - near);
		Matrix4x4 result = default(Matrix4x4);
		result.m00 = num / aspect;
		result.m01 = 0f;
		result.m02 = 0f;
		result.m03 = 0f;
		result.m10 = 0f;
		result.m11 = num;
		result.m12 = 0f;
		result.m13 = 0f;
		result.m20 = 0f;
		result.m21 = 0f;
		result.m22 = 2f * num2;
		result.m23 = (0f - (far + near)) * num2;
		result.m30 = 0f;
		result.m31 = 0f;
		result.m32 = 1f;
		result.m33 = 0f;
		return result;
	}

	public Vector4 GetPosition()
	{
		Transform transform = base.transform;
		if (m_Angle == 0f)
		{
			Vector3 vector = -transform.forward;
			return new Vector4(vector.x, vector.y, vector.z, 0f);
		}
		Vector3 vector2 = transform.position - _E01D() * transform.forward;
		return new Vector4(vector2.x, vector2.y, vector2.z, 1f);
	}

	public Bounds GetFrustumBounds()
	{
		if (m_Spot || m_Shadows)
		{
			if (m_Angle == 0f)
			{
				return new Bounds(Vector3.zero, _E03A);
			}
			float num = Mathf.Tan(m_Angle * 0.5f * ((float)Math.PI / 180f));
			float num2 = _E03A.y * 0.5f / num;
			float z = _E03A.z;
			float num3 = (num2 + _E03A.z) * num * 2f;
			float x = _E03A.x * num3 / _E03A.y;
			return new Bounds(Vector3.forward * _E03A.z * 0.5f, new Vector3(x, num3, z));
		}
		return new Bounds(new Vector3(0f, 0f, 0.5f * m_ClipBoxSize.z), m_ClipBoxSize);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		if (m_Angle == 0f)
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			if (m_Spot || m_Shadows)
			{
				Gizmos.DrawWireCube(new Vector3(0f, 0f, 0.5f * _E03A.z), _E03A);
				return;
			}
			Gizmos.DrawWireCube(new Vector3(0f, 0f, 0f), new Vector3(_E03A.x, _E03A.y, 0f));
		}
		float num = _E01D();
		Gizmos.matrix = base.transform.localToWorldMatrix * _E01E(0f - num);
		if (m_Spot || m_Shadows)
		{
			Gizmos.DrawFrustum(new Vector3(0f, 0f, num), m_Angle, num + _E03A.z, num, _E03A.x / _E03A.y);
		}
		else
		{
			Gizmos.DrawFrustum(new Vector3(0f, 0f, num), m_Angle, num, num, _E03A.x / _E03A.y);
		}
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = Color.yellow;
		Bounds frustumBounds = GetFrustumBounds();
		Gizmos.DrawWireCube(frustumBounds.center, frustumBounds.size);
	}
}
