using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
[ExecuteAlways]
public class SSAOMask : MonoBehaviour
{
	[SerializeField]
	private Shader _maskShader;

	private static Mesh m__E000;

	private static readonly Matrix4x4 _E001 = Matrix4x4.identity;

	private Camera _E002;

	private SSAA _E003;

	private CameraEvent _E004 = CameraEvent.BeforeLighting;

	private CommandBuffer _E005;

	private RenderTexture _E006;

	private Material _E007;

	private int _E008;

	private int _E009;

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(88887));

	private void Start()
	{
		if (SSAOMask.m__E000 == null)
		{
			SSAOMask.m__E000 = GetQuadMesh();
		}
		if (_maskShader == null)
		{
			_maskShader = _E3AC.Find(_ED3E._E000(38457));
		}
		_E005 = new CommandBuffer();
		_E005.name = _ED3E._E000(88854);
		_E002 = GetComponent<Camera>();
		_E003 = GetComponent<SSAA>();
		_E002.RemoveCommandBuffer(_E004, _E005);
		_E002.AddCommandBuffer(_E004, _E005);
		_E007 = new Material(_maskShader);
		int num = (_E003 ? _E003.GetInputWidth() : _E002.pixelWidth);
		int num2 = (_E003 ? _E003.GetInputHeight() : _E002.pixelHeight);
		_E008 = num2;
		_E009 = num;
		_E000();
	}

	private void _E000()
	{
		if (_E005 != null)
		{
			_E005.Clear();
			if (_E006 != null)
			{
				Object.DestroyImmediate(_E006);
			}
			int width = (_E003 ? _E003.GetInputWidth() : _E002.pixelWidth);
			int height = (_E003 ? _E003.GetInputHeight() : _E002.pixelHeight);
			_E006 = new RenderTexture(width, height, 0, RenderTextureFormat.R8)
			{
				name = _ED3E._E000(88840) + _E002.name
			};
			_E005.SetRenderTarget(_E006, BuiltinRenderTextureType.CurrentActive);
			_E005.ClearRenderTarget(clearDepth: false, clearColor: true, Color.white);
			_E005.DrawMesh(SSAOMask.m__E000, _E001, _E007);
			Shader.SetGlobalTexture(_E00A, _E006);
		}
	}

	private void OnPreCull()
	{
		if (!(_E002 == null))
		{
			int num = (_E003 ? _E003.GetInputWidth() : _E002.pixelWidth);
			int num2 = (_E003 ? _E003.GetInputHeight() : _E002.pixelHeight);
			if (num2 != _E008 || num != _E009)
			{
				_E008 = num2;
				_E009 = num;
				_E000();
			}
		}
	}

	private void OnDestroy()
	{
		if (_E005 != null)
		{
			_E002.RemoveCommandBuffer(_E004, _E005);
		}
		if (_E006 != null)
		{
			Object.DestroyImmediate(_E006);
		}
	}

	public static Mesh GetQuadMesh(float z = 0.1f)
	{
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(-1f, -1f, z),
			new Vector3(1f, -1f, z),
			new Vector3(-1f, 1f, z),
			new Vector3(1f, 1f, z)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] triangles = new int[6] { 0, 1, 3, 3, 2, 0 };
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.name = _ED3E._E000(41328);
		return mesh;
	}
}
