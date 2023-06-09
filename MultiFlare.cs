using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

[ExecuteInEditMode]
public sealed class MultiFlare : MonoBehaviour
{
	public enum EFlareType
	{
		Normal,
		OffScreen,
		Shit
	}

	public enum ERotationType
	{
		None,
		Normal,
		Inverse
	}

	public sealed class _E000 : IDisposable
	{
		[CanBeNull]
		public readonly Color32[] Colors;

		[CanBeNull]
		public readonly Vector3[] Positions;

		public bool ColorsWasChanged;

		public bool PositionsWasChanged;

		private readonly Material m__E000;

		private readonly Mesh _E001;

		private readonly bool _E002;

		public _E000(Material material, Mesh mesh, Vector3[] vertices, Color32[] colors)
		{
			m__E000 = material;
			_E001 = mesh;
			_E002 = _E001 == null;
			Positions = vertices;
			Colors = colors;
		}

		public void Draw(in Matrix4x4 matrix, in int layer)
		{
			if (!_E002)
			{
				if (ColorsWasChanged)
				{
					_E001.colors32 = Colors;
					ColorsWasChanged = false;
				}
				if (PositionsWasChanged)
				{
					_E001.vertices = Positions;
					PositionsWasChanged = false;
				}
				Graphics.DrawMesh(_E001, matrix, m__E000, layer, null, 0, null, castShadows: false, receiveShadows: false);
			}
		}

		public void Dispose()
		{
			if (_E001 != null)
			{
				UnityEngine.Object.DestroyImmediate(_E001);
			}
		}
	}

	public ProFlareAtlas Atlas;

	public Material NormalMat;

	public Material ShitMat;

	public LayerMask LayerMask;

	public Space Space;

	public int StartCapacity = 4;

	private static readonly _E000 m__E000 = new _E000(null, null, null, null);

	private readonly List<MultiFlareLight> m__E001 = new List<MultiFlareLight>(16);

	private List<Vector4> m__E002;

	private List<Vector3> m__E003;

	private List<Vector2> m__E004;

	private List<Vector2> m__E005;

	private List<Vector2> _E006;

	private List<Vector2> _E007;

	private List<int> _E008;

	private _E000 _E009;

	private _E000 _E00A;

	private int _E00B = -1;

	private Bounds _E00C;

	public int RegisteredLightsCount => this.m__E001.Count;

	public IReadOnlyList<MultiFlareLight> Lights => this.m__E001;

	private bool _E00D => RegisteredLightsCount > StartCapacity;

	private void Awake()
	{
		_E001(StartCapacity);
	}

	private void OnDestroy()
	{
		_E009?.Dispose();
		_E00A?.Dispose();
	}

	public void GenerateMesh()
	{
		if (!(Atlas == null))
		{
			_E009?.Dispose();
			_E00A?.Dispose();
			EFlareType type = EFlareType.Normal;
			_E009 = _E000(in type, NormalMat);
			type = EFlareType.Shit;
			_E00A = _E000(in type, ShitMat);
			_E00B = this.m__E001.Count;
		}
	}

	private void LateUpdate()
	{
		if (_E009 == null || _E00A == null || _E00B != this.m__E001.Count)
		{
			GenerateMesh();
			_E005();
			return;
		}
		for (int i = 0; i < this.m__E001.Count; i++)
		{
			MultiFlareLight multiFlareLight = this.m__E001[i];
			if (multiFlareLight != null && multiFlareLight.IsGenerating && multiFlareLight.IsVisible)
			{
				multiFlareLight.RefreshState(_E009, _E00A);
			}
		}
		_E005();
	}

	public void RegisterLight(MultiFlareLight flareLight)
	{
		if (!this.m__E001.Contains(flareLight))
		{
			this.m__E001.Add(flareLight);
		}
	}

	public void RemoveLight(MultiFlareLight flareLight)
	{
		this.m__E001.Remove(flareLight);
	}

	public void RemoveLight(int index)
	{
		this.m__E001.RemoveAt(index);
	}

	public void RemoveAllLights()
	{
		this.m__E001.Clear();
	}

	private _E000 _E000(in EFlareType type, Material material)
	{
		int count = _E003(in type);
		if (count == 0)
		{
			return MultiFlare.m__E000;
		}
		_E001(count);
		int num = count << 2;
		Vector3[] vertices = new Vector3[num];
		Color32[] array = new Color32[num];
		this.m__E002.SetCount(num);
		this.m__E003.SetCount(num);
		this.m__E004.SetCount(num);
		this.m__E005.SetCount(num);
		_E006.SetCount(num);
		_E007.SetCount(num);
		int i = 0;
		int pI = 0;
		for (; i < this.m__E001.Count; i++)
		{
			MultiFlareLight multiFlareLight = this.m__E001[i];
			if (!(multiFlareLight == null))
			{
				multiFlareLight.DrawSelf(ref pI, in type, Atlas, vertices, this.m__E002, this.m__E003, this.m__E004, this.m__E005, _E006, _E007, array, in Space);
				Vector3 position = multiFlareLight.transform.position;
				_E004(in position, ref _E00C);
			}
		}
		Mesh mesh = new Mesh
		{
			name = _ED3E._E000(88268),
			vertices = vertices,
			colors32 = array,
			bounds = _E00C
		};
		mesh.SetNormals(this.m__E003);
		mesh.SetTangents(this.m__E002);
		mesh.SetUVs(0, this.m__E004);
		mesh.SetUVs(1, this.m__E005);
		mesh.SetUVs(2, _E006);
		mesh.SetUVs(3, _E007);
		mesh.SetTriangles(_E002(in count), 0);
		mesh.MarkDynamic();
		return new _E000(material, mesh, vertices, array);
	}

	private void _E001(int capacity)
	{
		if (_E008 == null)
		{
			if (this.m__E001.Capacity < capacity)
			{
				this.m__E001.Capacity = capacity;
			}
			int num = capacity << 2;
			_E008 = new List<int>(num * 6);
			this.m__E002 = new List<Vector4>(num);
			this.m__E003 = new List<Vector3>(num);
			this.m__E004 = new List<Vector2>(num);
			this.m__E005 = new List<Vector2>(num);
			_E006 = new List<Vector2>(num);
			_E007 = new List<Vector2>(num);
		}
	}

	private List<int> _E002(in int count)
	{
		_E008.Clear();
		for (int i = 0; i < count; i++)
		{
			int num = i << 2;
			_E008.Add(num);
			_E008.Add(num + 3);
			_E008.Add(num + 2);
			_E008.Add(num + 2);
			_E008.Add(num + 1);
			_E008.Add(num);
		}
		return _E008;
	}

	private int _E003(in EFlareType type)
	{
		int num = 0;
		foreach (MultiFlareLight item in this.m__E001)
		{
			if (item == null)
			{
				continue;
			}
			MultiFlareLight.Flare[] flares = item.Flares;
			for (int i = 0; i < flares.Length; i++)
			{
				if (flares[i].Type == type)
				{
					num++;
				}
			}
		}
		return num;
	}

	private static void _E004(in Vector3 position, ref Bounds bounds)
	{
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		if (!(position.x > min.x) || !(position.y > min.y) || !(position.z > min.z) || !(position.x < max.x) || !(position.y < max.y) || !(position.z < max.z))
		{
			if (position.x < min.x)
			{
				min.x = position.x - 50f;
			}
			if (position.y < min.y)
			{
				min.y = position.y - 50f;
			}
			if (position.z < min.z)
			{
				min.z = position.z - 50f;
			}
			if (position.x > max.x)
			{
				max.x = position.x + 50f;
			}
			if (position.y > max.y)
			{
				max.y = position.y + 50f;
			}
			if (position.z > max.z)
			{
				max.z = position.z + 50f;
			}
			bounds.SetMinMax(min, max);
		}
	}

	[CompilerGenerated]
	private void _E005()
	{
		Matrix4x4 matrix = ((Space == Space.World) ? Matrix4x4.identity : base.transform.localToWorldMatrix);
		int layer = LayerMask.value;
		_E009.Draw(in matrix, in layer);
		_E00A.Draw(in matrix, in layer);
	}
}
