using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class WetRenderer : OnRenderObjectConnector
{
	public Shader WetShader;

	[Range(0f, 1f)]
	public float Intensity;

	public Color DiffuseColor;

	public Color SpecColor;

	public float WetBias = 0.0016f;

	private Material _E034;

	private static readonly int _E035 = Shader.PropertyToID(_ED3E._E000(84645));

	private static readonly int _E036 = Shader.PropertyToID(_ED3E._E000(84690));

	private static readonly int _E037 = Shader.PropertyToID(_ED3E._E000(84730));

	private static readonly int _E038 = Shader.PropertyToID(_ED3E._E000(84711));

	private Mesh _E039;

	private readonly RenderTargetIdentifier[] _E03A = new RenderTargetIdentifier[2]
	{
		BuiltinRenderTextureType.GBuffer0,
		BuiltinRenderTextureType.GBuffer1
	};

	private readonly _E42E _E02A = new _E42E(_ED3E._E000(84653), CameraEvent.BeforeReflections);

	private void OnValidate()
	{
		_E001();
	}

	private void Awake()
	{
		_E000();
		_E001();
		_E004();
	}

	private void _E000()
	{
		_E039 = _E003();
		_E02A.DestroyBuffers();
	}

	private void _E001()
	{
		Shader.SetGlobalFloat(_E035, Intensity);
		Shader.SetGlobalColor(_E036, DiffuseColor);
		Shader.SetGlobalColor(_E037, SpecColor);
		Shader.SetGlobalFloat(_E038, WetBias);
	}

	private void Update()
	{
		Shader.SetGlobalFloat(_E035, Intensity);
	}

	public override void ManualOnRenderObject(Camera currentCamera)
	{
		base.ManualOnRenderObject(currentCamera);
		if ((!(currentCamera != _E8A8.Instance.Camera) || !(currentCamera != _E8A8.Instance.OpticCameraManager.Camera)) && _E02A.OnRenderObject(out var buffer))
		{
			buffer.SetRenderTarget(_E03A, BuiltinRenderTextureType.CameraTarget);
			buffer.DrawMesh(_E039, Matrix4x4.identity, _E002());
		}
	}

	private Material _E002()
	{
		if (_E034 != null)
		{
			return _E034;
		}
		_E034 = new Material(WetShader);
		return _E034;
	}

	private static Mesh _E003()
	{
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(-1f, -1f, -0.1f),
			new Vector3(-1f, 1f, -0.1f),
			new Vector3(1f, 1f, -0.1f),
			new Vector3(1f, -1f, -0.1f)
		};
		int[] triangles = new int[6] { 0, 2, 1, 0, 3, 2 };
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(3f, 0f),
			new Vector2(2f, 0f),
			new Vector2(1f, 0f)
		};
		Mesh obj = new Mesh
		{
			name = _ED3E._E000(84614),
			vertices = vertices,
			uv = uv,
			triangles = triangles
		};
		Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 2.1474836E+09f);
		obj.bounds = bounds;
		return obj;
	}

	private void _E004()
	{
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Object.DestroyImmediate(_E039);
	}
}
