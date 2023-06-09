using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class TubeLight : BaseLight
{
	public Color m_Color = Color.white;

	[Range(0f, 2f)]
	public float m_SpecularScale = 1f;

	[Range(0f, 500f)]
	public float m_Range = 10f;

	[Range(0.001f, 30f)]
	public float m_Radius = 0.3f;

	[Range(0f, 100f)]
	public float m_Length;

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
	public Mesh m_Sphere;

	[HideInInspector]
	public Mesh m_Capsule;

	[HideInInspector]
	public Shader m_ProxyShader;

	private Material m__E000;

	[Space]
	public bool m_Ambient;

	public bool m_Negative;

	public bool m_Specular = true;

	[Space]
	public bool m_RenderSource;

	public Color m_SourceColor = Color.white;

	public Material SourceMaterial;

	private Renderer _E03B;

	private Transform _E045;

	private Mesh _E03C;

	private Material[] _E03D;

	private float _E046 = -1f;

	private bool _E041;

	private MaterialPropertyBlock _E042;

	private const float _E047 = 0.001f;

	private float _E048 = -1f;

	private float _E049 = -1f;

	private bool _E04A;

	private Matrix4x4 m__E008;

	private Vector4 _E04B;

	private Vector4 _E04C;

	private bool m__E00B;

	private bool m__E00C;

	private bool m__E00D;

	private int _E00E;

	private bool _E00F;

	private int _E016;

	private int _E04D;

	private int _E01C;

	private int _E04E;

	private int _E04F;

	private int _E018;

	private int _E019;

	private int _E023;

	private int _E024;

	private int _E025;

	private int _E026;

	private Dictionary<Camera, CommandBuffer> _E010 = new Dictionary<Camera, CommandBuffer>();

	private static CameraEvent _E015 = CameraEvent.AfterImageEffectsOpaque;

	private Vector4[] _E011 = new Vector4[6];

	private Vector4[] _E012 = new Vector4[6];

	private static readonly int _E044 = Shader.PropertyToID(_ED3E._E000(40467));

	private bool _E001
	{
		get
		{
			if (m_RenderSource)
			{
				return m_Radius >= 0.001f;
			}
			return false;
		}
	}

	private void Start()
	{
		if (_E000())
		{
			_E002(forceUpdate: true);
			_E003();
			_E00B();
		}
	}

	private bool _E000()
	{
		if (_E041)
		{
			return true;
		}
		if (m_ProxyShader == null || m_Sphere == null || m_Capsule == null)
		{
			return false;
		}
		this.m__E000 = new Material(m_ProxyShader);
		this.m__E000.hideFlags = HideFlags.HideAndDontSave;
		_E03C = UnityEngine.Object.Instantiate(m_Capsule);
		_E03C.hideFlags = HideFlags.HideAndDontSave;
		base.gameObject.GetComponent<MeshFilter>().sharedMesh = _E03C;
		_E03B = base.gameObject.GetComponent<MeshRenderer>();
		_E03B.enabled = true;
		_E03D = _E03B.sharedMaterials;
		_E045 = base.transform;
		_E00C(ShadowCube, _E011);
		_E00C(InvertedShadowCube, _E012);
		_E041 = true;
		return true;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (_E042 == null)
		{
			_E042 = new MaterialPropertyBlock();
		}
		if (_E000())
		{
			_E002(forceUpdate: true);
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(_E005));
			if (_E03B == null)
			{
				_E03B = GetComponent<MeshRenderer>();
			}
			_E03B.enabled = true;
			_E03D = _E03B.sharedMaterials;
			_E00B();
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_E001();
		Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(_E005));
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
		_E001();
	}

	private void _E001()
	{
		Dictionary<Camera, CommandBuffer>.Enumerator enumerator = _E010.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<Camera, CommandBuffer> current = enumerator.Current;
			if (current.Key != null && current.Value != null)
			{
				current.Key.RemoveCommandBuffer(_E015, current.Value);
			}
		}
		_E010.Clear();
	}

	private void _E002(bool forceUpdate = false)
	{
		bool flag = Math.Abs(m_Length - _E046) > 0.01f;
		bool num = Math.Abs(m_Radius - _E049) > 0.01f;
		bool flag2 = Math.Abs(m_Range - _E048) > 0.01f;
		bool flag3 = this._E001 != _E04A;
		if (!(num || flag2 || flag || flag3) && !forceUpdate)
		{
			return;
		}
		m_Range = Mathf.Max(m_Range, 0f);
		m_Radius = Mathf.Max(m_Radius, 0.001f);
		m_Length = Mathf.Max(m_Length, 0f);
		m_Intensity = Mathf.Max(m_Intensity, 0f);
		Vector3 vector = (this._E001 ? (Vector3.one * m_Radius * 2f) : Vector3.one);
		if (_E045.localScale != vector || flag)
		{
			_E046 = m_Length;
			Vector3[] vertices = m_Capsule.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				if (this._E001)
				{
					vertices[i].y += Mathf.Sign(vertices[i].y) * (-0.5f + 0.25f * m_Length / m_Radius);
				}
				else
				{
					vertices[i] = Vector3.one * 0.0001f;
				}
			}
			_E03C.vertices = vertices;
		}
		_E045.localScale = vector;
		float num2 = m_Range + m_Radius;
		num2 += 0.5f * m_Length;
		num2 *= 1.02f;
		num2 /= m_Radius;
		_E03C.bounds = new Bounds(Vector3.zero, Vector3.one * num2);
		_E049 = m_Radius;
		_E048 = m_Range;
		_E04A = this._E001;
	}

	private void _E003()
	{
		_E042.SetVector(_E044, m_SourceColor * Mathf.Sqrt(m_Intensity) * 2f);
		_E03B.SetPropertyBlock(_E042);
	}

	public override void ManualUpdate(float dt)
	{
		if (m_Intensity <= 0f)
		{
			if (_E010.Count > 0)
			{
				_E001();
				_E03B.enabled = false;
			}
		}
		else if (_E000())
		{
			_E002();
			_E03B.enabled = true;
			_E00B();
		}
	}

	private Color _E004()
	{
		if (QualitySettings.activeColorSpace == ColorSpace.Gamma)
		{
			return m_Color * m_Intensity;
		}
		return new Color(Mathf.GammaToLinearSpace(m_Color.r * m_Intensity), Mathf.GammaToLinearSpace(m_Color.g * m_Intensity), Mathf.GammaToLinearSpace(m_Color.b * m_Intensity), 1f);
	}

	private void _E005(Camera cam)
	{
		if (!(m_Intensity <= 0f) && !_E00D(cam) && _E000() && (cam.CompareTag(_ED3E._E000(42407)) || cam.CompareTag(_ED3E._E000(42129))))
		{
			_E006(cam);
		}
	}

	private void _E006(Camera cam)
	{
		CommandBuffer commandBuffer = null;
		if (!_E010.ContainsKey(cam))
		{
			commandBuffer = new CommandBuffer();
			commandBuffer.name = base.gameObject.name;
			_E010[cam] = commandBuffer;
			cam.AddCommandBuffer(_E015, commandBuffer);
			cam.depthTextureMode |= DepthTextureMode.Depth;
		}
		else
		{
			commandBuffer = _E010[cam];
			commandBuffer.Clear();
		}
		_E007();
		_E009(commandBuffer, cam);
		_E00A(commandBuffer);
		if (this.m__E00D)
		{
			commandBuffer.DrawRenderer(ShadowRendererToDraw, ShadowRendererMaterial);
		}
		int num = 1;
		if (this.m__E00D)
		{
			num++;
			if (IsShadowRendererInverted)
			{
				num++;
			}
		}
		commandBuffer.DrawMesh(m_Sphere, this.m__E008, this.m__E000, 0, 0);
		commandBuffer.DrawMesh(m_Sphere, this.m__E008, this.m__E000, 0, num);
	}

	private void _E007()
	{
		if (!_E00F)
		{
			Transform transform = base.transform;
			Vector3 up = transform.up;
			Vector3 vector = transform.position - 0.5f * up * m_Length;
			_E04C = new Vector4(up.x, up.y, up.z, 0f);
			float num = m_Range + m_Radius;
			num += 0.5f * (m_Length - m_Radius);
			_E04B = new Vector4(vector.x, vector.y, vector.z, 1f / (num * num));
			Matrix4x4 matrix4x = Matrix4x4.Scale(Vector3.one * num * 2.05f);
			this.m__E008 = transform.localToWorldMatrix * matrix4x;
			this.m__E00B = ShadowCube != null && ShadowCube.gameObject.activeInHierarchy;
			this.m__E00C = InvertedShadowCube != null && InvertedShadowCube.gameObject.activeInHierarchy;
			this.m__E00D = ShadowRendererToDraw != null && ShadowRendererMaterial != null;
			_E008();
			_E00F = true;
		}
	}

	private void _E008()
	{
		_E016 = Shader.PropertyToID(_ED3E._E000(47948));
		_E04D = Shader.PropertyToID(_ED3E._E000(42542));
		_E01C = Shader.PropertyToID(_ED3E._E000(48086));
		_E04E = Shader.PropertyToID(_ED3E._E000(42529));
		_E04F = Shader.PropertyToID(_ED3E._E000(42582));
		_E018 = Shader.PropertyToID(_ED3E._E000(19825));
		_E019 = Shader.PropertyToID(_ED3E._E000(47992));
		_E023 = Shader.PropertyToID(_ED3E._E000(42009));
		_E024 = Shader.PropertyToID(_ED3E._E000(41989));
		_E025 = Shader.PropertyToID(_ED3E._E000(42043));
		_E026 = Shader.PropertyToID(_ED3E._E000(42079));
	}

	private void _E009(CommandBuffer buf, Camera cam)
	{
		if (m_Ambient)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42110));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42110));
		}
		if (m_Specular && !m_Negative && !m_Ambient)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42088));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42088));
		}
		if (m_Negative)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42053));
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42053));
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
		buf.SetGlobalVector(_E016, _E04B);
		buf.SetGlobalVector(_E04D, _E04C);
		buf.SetGlobalFloat(_E01C, 0f);
		buf.SetGlobalFloat(_E04E, m_Radius);
		buf.SetGlobalFloat(_E04F, m_Length);
		buf.SetGlobalVector(_E018, _E004());
		buf.SetGlobalFloat(_E019, m_SpecularScale);
	}

	private void _E00A(CommandBuffer buf)
	{
		if (this.m__E00B)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42169));
			buf.SetGlobalFloat(_E023, ShadowFeather);
			buf.SetGlobalVectorArray(_E024, _E011);
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42169));
		}
		if (this.m__E00C)
		{
			buf.EnableShaderKeyword(_ED3E._E000(42154));
			buf.SetGlobalFloat(_E025, InvertedShadowFeather);
			buf.SetGlobalVectorArray(_E026, _E012);
		}
		else
		{
			buf.DisableShaderKeyword(_ED3E._E000(42154));
		}
	}

	private void _E00B()
	{
		if (this._E001)
		{
			_E003();
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

	private void _E00C(MeshRenderer cube, Vector4[] planes)
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

	private void OnDrawGizmosSelected()
	{
		if (!(_E045 == null))
		{
			Gizmos.color = Color.white;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(_E045.position, _E045.rotation, Vector3.one);
			Gizmos.matrix = matrix;
			Gizmos.DrawWireSphere(Vector3.zero, m_Radius + m_Range + 0.5f * m_Length);
			Vector3 vector = 0.5f * Vector3.up * m_Length;
			Gizmos.DrawWireSphere(vector, m_Radius);
			if (m_Length != 0f)
			{
				Vector3 vector2 = -0.5f * Vector3.up * m_Length;
				Gizmos.DrawWireSphere(vector2, m_Radius);
				Vector3 vector3 = Vector3.forward * m_Radius;
				Gizmos.DrawLine(vector + vector3, vector2 + vector3);
				Gizmos.DrawLine(vector - vector3, vector2 - vector3);
				vector3 = Vector3.right * m_Radius;
				Gizmos.DrawLine(vector + vector3, vector2 + vector3);
				Gizmos.DrawLine(vector - vector3, vector2 - vector3);
			}
		}
	}

	private void OnDrawGizmos()
	{
	}

	private bool _E00D(Camera cam)
	{
		RenderTexture targetTexture = cam.targetTexture;
		if (targetTexture != null)
		{
			return targetTexture.format == RenderTextureFormat.Shadowmap;
		}
		return false;
	}
}
