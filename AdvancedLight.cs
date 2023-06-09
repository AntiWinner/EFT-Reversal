using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class AdvancedLight : MonoBehaviour, _E411
{
	public enum LightTypeEnum
	{
		Point,
		Directional
	}

	public enum ShadingTypeEnum
	{
		Diffuse,
		Specular,
		DiffuseAndSpecular
	}

	public Shader Shader;

	public LightTypeEnum LightType;

	public Mesh Mesh;

	public int SubMesh;

	public Transform LightPosition;

	[ColorUsage(false, true)]
	public Color Color;

	public float Radius;

	public ShadingTypeEnum ShadingType;

	public bool ShowDebugShapes;

	public Transform CubeControlHelper;

	public bool FullScreen;

	[HideInInspector]
	public Vector3 CubeScale = _E00A;

	[HideInInspector]
	public Vector3 CubeShift = _E00B;

	private Material m__E000;

	private Transform m__E001;

	private Transform _E002;

	private int _E003;

	private int _E004;

	private int _E005;

	private int _E006;

	private int _E007;

	private int _E008;

	private float _E009;

	private static readonly Vector3 _E00A = Vector3.one;

	private static readonly Vector3 _E00B = Vector3.zero;

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(36528));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(90009));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(90031));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(90019));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(90060));

	private void _E000()
	{
		if (Shader == null)
		{
			throw new NullReferenceException(base.name + _ED3E._E000(89893));
		}
		if (Mesh == null)
		{
			throw new NullReferenceException(base.name + _ED3E._E000(89924));
		}
		if (this.m__E000 == null)
		{
			this.m__E000 = new Material(Shader);
		}
		this.m__E000.SetColor(_E00C, Color);
		Radius = Mathf.Max(Radius, 0f);
		this.m__E000.SetFloat(_E00D, 1f / (Radius * Radius));
		UpdateKeywords(this.m__E000, ShadingType);
		UpdateKeywords(this.m__E000, LightType);
		this.m__E000.SetFloat(_E00E, FullScreen ? 1 : 0);
		this.m__E001 = base.transform;
		_E002 = ((LightPosition != null) ? LightPosition : this.m__E001);
		_E003 = Shader.PropertyToID(_ED3E._E000(19778));
		_E004 = Shader.PropertyToID(_ED3E._E000(89961));
		_E005 = Shader.PropertyToID(_ED3E._E000(41354));
		_E006 = Shader.PropertyToID(_ED3E._E000(41404));
		_E007 = Shader.PropertyToID(_ED3E._E000(36528));
		_E008 = Shader.PropertyToID(_ED3E._E000(90009));
		_E001();
	}

	private void Update()
	{
		this.m__E000.SetColor(_E007, Color);
		if (_E009 != Radius)
		{
			_E009 = Radius;
			Radius = Mathf.Max(Radius, 0f);
			this.m__E000.SetFloat(_E008, 1f / (Radius * Radius));
		}
		_E001();
	}

	private void _E001()
	{
		if (CubeControlHelper != null && !FullScreen)
		{
			CubeScale = CubeControlHelper.localScale;
			CubeShift = CubeControlHelper.localPosition;
			CubeControlHelper.localRotation = Quaternion.identity;
		}
		else
		{
			CubeScale = _E00A;
			CubeShift = _E00B;
		}
		this.m__E000.SetVector(_E00F, CubeScale);
		this.m__E000.SetVector(_E010, CubeShift);
	}

	public static void UpdateKeywords(Material material, Enum enumValue)
	{
		string[] names = Enum.GetNames(enumValue.GetType());
		string text = enumValue.ToString();
		string[] array = names;
		foreach (string text2 in array)
		{
			if (text2 == text)
			{
				material.EnableKeyword(text2.ToUpper());
			}
			else
			{
				material.DisableKeyword(text2.ToUpper());
			}
		}
	}

	private void OnValidate()
	{
		_E000();
	}

	private void Start()
	{
		_E000();
		LightManager.Add(this);
	}

	private void OnEnable()
	{
		_E000();
		LightManager.Add(this);
	}

	private void OnDisable()
	{
		LightManager.Remove(this);
	}

	private void OnDestroy()
	{
		OnDisable();
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.transform.position, _E38D.True ? _ED3E._E000(90047) : _ED3E._E000(89998), allowScaling: true);
	}

	private void OnDrawGizmosSelected()
	{
		if (ShowDebugShapes)
		{
			Gizmos.DrawWireMesh(Mesh, SubMesh, this.m__E001.position, this.m__E001.rotation, this.m__E001.lossyScale);
			Gizmos.DrawWireSphere(_E002.position, Radius);
			Gizmos.color = new Color(0f, 1f, 0f, 0.1f);
			Gizmos.DrawMesh(Mesh, SubMesh, this.m__E001.position, this.m__E001.rotation, this.m__E001.lossyScale);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(CubeShift, CubeScale);
		}
	}

	public void Draw(CommandBuffer buffer, Plane[] frustrumPlanes, Camera cam)
	{
		Bounds bounds = Mesh.bounds;
		bounds.extents = Vector3.Scale(bounds.extents, base.transform.localScale);
		bounds.center += base.transform.position;
		if (GeometryUtility.TestPlanesAABB(frustrumPlanes, bounds) || FullScreen)
		{
			BlendMode blendMode = (cam.allowHDR ? BlendMode.One : BlendMode.DstColor);
			BlendMode blendMode2 = (cam.allowHDR ? BlendMode.One : BlendMode.Zero);
			buffer.SetGlobalFloat(_E005, (float)blendMode);
			buffer.SetGlobalFloat(_E006, (float)blendMode2);
			this.m__E000.SetVector(_E003, _E002.position);
			this.m__E000.SetVector(_E004, _E002.forward);
			buffer.DrawMesh(Mesh, base.transform.localToWorldMatrix, this.m__E000, SubMesh, 0);
		}
	}
}
