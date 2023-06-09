using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class PhotoCamera : MonoBehaviour
{
	public delegate void _E000(PhotoCamera sender);

	private RenderTexture m__E000;

	private Camera m__E001;

	private bool _E002;

	private bool _E003;

	private int _E004;

	private Vector3 _E005 = new Vector3(0f, 0f, 5f);

	[CompilerGenerated]
	private _E000 _E006;

	private Action<Texture> _E007;

	private GameObject _E008;

	private Matrix4x4 _E009;

	private Light _E00A;

	public event _E000 OnTextureRenderFinished
	{
		[CompilerGenerated]
		add
		{
			_E000 obj = _E006;
			_E000 obj2;
			do
			{
				obj2 = obj;
				_E000 value2 = (_E000)Delegate.Combine(obj2, value);
				obj = Interlocked.CompareExchange(ref _E006, value2, obj2);
			}
			while ((object)obj != obj2);
		}
		[CompilerGenerated]
		remove
		{
			_E000 obj = _E006;
			_E000 obj2;
			do
			{
				obj2 = obj;
				_E000 value2 = (_E000)Delegate.Remove(obj2, value);
				obj = Interlocked.CompareExchange(ref _E006, value2, obj2);
			}
			while ((object)obj != obj2);
		}
	}

	public void Awake()
	{
		_E004 = LayerMask.NameToLayer(_ED3E._E000(96552));
		this.m__E001 = base.gameObject.AddComponent<Camera>();
		this.m__E001.backgroundColor = new Color(1f, 0f, 0f, 0f);
		this.m__E001.orthographic = true;
		this.m__E001.cullingMask = 1 << _E004;
		this.m__E001.enabled = false;
		_E00A = base.gameObject.AddComponent<Light>();
		_E00A.type = LightType.Directional;
		_E00A.color = Color.white;
		_E00A.enabled = false;
		_E009 = Matrix4x4.TRS(base.transform.position + _E005, Quaternion.identity, Vector3.one);
		Debug.Log(_E009);
	}

	public void MakeTexture(GameObject gObject, Action<Texture> callback, int cameraOrthoSize, Vector2 renderTextureSize)
	{
		this.m__E001.enabled = true;
		this.m__E000 = new RenderTexture(Mathf.FloorToInt(GetComponent<Camera>().pixelWidth * 2), Mathf.FloorToInt(GetComponent<Camera>().pixelHeight * 2), 24);
		this.m__E000.name = _ED3E._E000(96547);
		this.m__E001.targetTexture = this.m__E000;
		this.m__E001.orthographicSize = 0.5f;
		_E008 = gObject;
		_E007 = callback;
		_E002 = true;
	}

	private void OnPreCull()
	{
		if (_E002)
		{
			_E00A.enabled = true;
			_E000(_E008.transform);
			_E002 = false;
			_E003 = true;
		}
	}

	private void _E000(Transform t)
	{
		foreach (Transform item in t)
		{
			_E000(item);
			MeshFilter component = item.gameObject.GetComponent<MeshFilter>();
			MeshRenderer component2 = item.gameObject.GetComponent<MeshRenderer>();
			if (!(component != null) || !(component2 != null))
			{
				continue;
			}
			if (component.sharedMesh.subMeshCount == 1)
			{
				Debug.Log(component.sharedMesh);
				Matrix4x4 matrix = _E009 * item.localToWorldMatrix;
				Graphics.DrawMesh(component.sharedMesh, matrix, component2.sharedMaterial, _E004, this.m__E001);
				Debug.Log(_ED3E._E000(96594) + item.localToWorldMatrix);
				Debug.Log(_ED3E._E000(96591) + _E009);
				Debug.Log(_ED3E._E000(96580) + _E009 * item.localToWorldMatrix);
				Debug.Log(matrix.GetColumn(3));
			}
			else
			{
				for (int i = 0; i < component.sharedMesh.subMeshCount; i++)
				{
					Debug.Log(string.Concat(component.sharedMesh, _ED3E._E000(96637), component.sharedMesh.subMeshCount));
					int num = ((i < component2.sharedMaterials.Length) ? i : (component2.sharedMaterials.Length - 1));
					Graphics.DrawMesh(_E001(component.sharedMesh.GetTriangles(i), component.sharedMesh), item.localToWorldMatrix * this.m__E001.worldToCameraMatrix * this.m__E001.projectionMatrix, component2.sharedMaterials[num], _E004, this.m__E001);
				}
			}
		}
	}

	private Mesh _E001(int[] triangles, Mesh originalMesh)
	{
		Mesh mesh = new Mesh();
		mesh.name = _ED3E._E000(96631);
		mesh.Clear();
		mesh.vertices = originalMesh.vertices;
		mesh.uv = originalMesh.uv;
		mesh.uv2 = originalMesh.uv2;
		mesh.uv2 = originalMesh.uv2;
		mesh.colors = originalMesh.colors;
		mesh.normals = originalMesh.normals;
		mesh.triangles = triangles;
		mesh.tangents = originalMesh.tangents;
		return mesh;
	}

	private void OnPostRender()
	{
		if (_E003)
		{
			_E00A.enabled = false;
			_E007(this.m__E000);
			if (_E006 != null)
			{
				_E006(this);
			}
			GetComponent<Camera>().targetTexture = null;
			GetComponent<Camera>().enabled = false;
			_E003 = false;
		}
	}
}
