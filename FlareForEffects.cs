using System.Collections.Generic;
using UnityEngine;

public class FlareForEffects : OnRenderObjectConnector
{
	private class _E000
	{
		public Mesh ScreenMesh;

		public Mesh ShitMesh;
	}

	private class _E001
	{
		public int FlareID;

		public Vector3 Position;

		private float _E000;

		public float Energy;

		public _E001(Vector3 pos, int flareID, float time)
		{
			Position = pos;
			FlareID = flareID;
			Energy = 1f;
			_E000 = 1f / time;
		}

		public bool Process(float delta)
		{
			Energy -= _E000 * delta;
			return Energy < 0f;
		}
	}

	public MultiFlare MultiFlareObject;

	private _E000[] _E02B;

	private static readonly LinkedList<_E001> _E02C = new LinkedList<_E001>();

	private Material _E02D;

	private Material _E02E;

	private float _E02F;

	private float _E030;

	private static readonly int _E031 = Shader.PropertyToID(_ED3E._E000(35970));

	public void Awake()
	{
		bool active = Application.isEditor && !Application.isPlaying;
		MultiFlareObject.gameObject.SetActive(active);
		_E02D = Object.Instantiate(MultiFlareObject.NormalMat);
		_E02E = Object.Instantiate(MultiFlareObject.ShitMat);
		_E02F = _E02D.GetFloat(_E031);
		_E030 = _E02E.GetFloat(_E031);
		_E000();
	}

	private void _E000()
	{
		IReadOnlyList<MultiFlareLight> lights = MultiFlareObject.Lights;
		_E02B = new _E000[lights.Count];
		for (int i = 0; i < _E02B.Length; i++)
		{
			MultiFlareLight flareLight = lights[i];
			_E02B[i] = new _E000
			{
				ScreenMesh = _E001(flareLight, MultiFlare.EFlareType.Normal),
				ShitMesh = _E001(flareLight, MultiFlare.EFlareType.Shit)
			};
		}
	}

	private void LateUpdate()
	{
		if (_E02C.Count == 0)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		LinkedListNode<_E001> linkedListNode = _E02C.First;
		while (linkedListNode != null)
		{
			if (linkedListNode.Value.Process(deltaTime))
			{
				LinkedListNode<_E001> node = linkedListNode;
				linkedListNode = linkedListNode.Next;
				_E02C.Remove(node);
			}
			else
			{
				linkedListNode = linkedListNode.Next;
			}
		}
	}

	public override void ManualOnRenderObject(Camera currentCamera)
	{
		base.ManualOnRenderObject(currentCamera);
		if (_E02C.Count == 0 || Camera.current != _E8A8.Instance.Camera)
		{
			return;
		}
		for (LinkedListNode<_E001> linkedListNode = _E02C.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			_E001 value = linkedListNode.Value;
			_E000 obj = _E02B[value.FlareID];
			if (obj.ScreenMesh != null)
			{
				_E02D.SetFloat(_E031, _E02F * value.Energy);
				_E02D.SetPass(0);
				Graphics.DrawMeshNow(obj.ScreenMesh, value.Position, Quaternion.identity);
			}
			if (obj.ShitMesh != null)
			{
				_E02E.SetFloat(_E031, _E030 * value.Energy);
				_E02E.SetPass(0);
				Graphics.DrawMeshNow(obj.ShitMesh, value.Position, Quaternion.identity);
			}
		}
	}

	public static void Add(Vector3 pos, int flareID, float time)
	{
		_E02C.AddLast(new _E001(pos, flareID, time));
	}

	private Mesh _E001(MultiFlareLight flareLight, MultiFlare.EFlareType type)
	{
		int num = 0;
		MultiFlareLight.Flare[] flares = flareLight.Flares;
		for (int i = 0; i < flares.Length; i++)
		{
			if (flares[i].Type == type)
			{
				num++;
			}
		}
		if (num == 0 || MultiFlareObject.Atlas == null)
		{
			return null;
		}
		int num2 = num << 2;
		Vector3[] array = new Vector3[num2];
		Vector4[] tangents = new Vector4[num2];
		Vector3[] normals = new Vector3[num2];
		Vector2[] array2 = new Vector2[num2];
		Vector2[] array3 = new Vector2[num2];
		Vector2[] array4 = new Vector2[num2];
		Vector2[] array5 = new Vector2[num2];
		Color32[] array6 = new Color32[num2];
		int[] array7 = new int[num * 6];
		int j = 0;
		int num3 = 0;
		for (; j < num; j++)
		{
			int num4 = j << 2;
			array7[num3++] = num4;
			array7[num3++] = num4 + 3;
			array7[num3++] = num4 + 2;
			array7[num3++] = num4 + 2;
			array7[num3++] = num4 + 1;
			array7[num3++] = num4;
		}
		int pI = 0;
		ProFlareAtlas atlas = MultiFlareObject.Atlas;
		Space space = default(Space);
		flareLight.DrawSelf(ref pI, in type, atlas, array, tangents, normals, array2, array3, array4, array5, array6, in space);
		for (int k = 0; k < num2; k++)
		{
			array6[k].a = byte.MaxValue;
			array[k] = Vector3.zero;
		}
		return new Mesh
		{
			vertices = array,
			normals = normals,
			tangents = tangents,
			uv = array2,
			uv2 = array3,
			uv3 = array4,
			uv4 = array5,
			colors32 = array6,
			triangles = array7,
			bounds = new Bounds(Vector3.zero, Vector3.one * 0.1f),
			name = _ED3E._E000(88228)
		};
	}
}
